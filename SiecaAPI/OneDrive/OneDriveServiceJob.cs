using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Kiota.Abstractions;
using Microsoft.VisualBasic.FileIO;
using Quartz;
using SiecaAPI.Data.SQLImpl;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.OneDrive.Manager;
using SiecaAPI.OneDrive.Models;
using System.Globalization;
using System.IO;
using System.Text;

namespace SiecaAPI.OneDrive;

public class OneDriveServiceJob : IJob
{
    private readonly IConfiguration configuration;

    public OneDriveServiceJob(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        CancellationToken stoppingToken = default;

        try
        {
            var reqestManager = new RequestManager(configuration);

            var model = await reqestManager.GetFilesAsync(stoppingToken);

            if (model == null)
            {
                return;
            }

            var filesToDownload = model.Value.Where(x => x.DownloadUrl != null && !x.Name.Contains(configuration["Suffix"]!) &&
                string.Equals(Path.GetExtension(x.Name), ".csv", StringComparison.InvariantCultureIgnoreCase))
                .Select(x => new FileDataModel
                {
                    DownloadUrl = x.DownloadUrl,
                    Name = x.Name,
                    ItemId = x.Id
                }).ToArray();

            await reqestManager.ReadFilesAsync(filesToDownload, stoppingToken);

            foreach (var file in filesToDownload)
            {
                using var sqlContext = new SqlContext();

                var models = await ToModelsAsync(sqlContext, file.Bytes);

                await sqlContext.BeneficiaryAnthropometricData.AddRangeAsync(models, stoppingToken);
                await sqlContext.SaveChangesAsync(stoppingToken);

                file.NewName = Path.GetFileNameWithoutExtension(file.Name) + configuration["Suffix"]! + Path.GetExtension(file.Name);
            }

            await reqestManager.UpdateFilesAsync(filesToDownload, stoppingToken);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed due to -> " + ex.ToString());
        }
    }

    private static async Task<IList<BeneficiaryAnthropometricDataEntity>> ToModelsAsync(SqlContext context, byte[] data)
    {
        var models = new List<BeneficiaryAnthropometricDataEntity>();
        using var ms = new MemoryStream(data);

        try
        {
            var csv = new CsvReader(reader: new StreamReader(ms, Encoding.UTF8), new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";",
            });

            csv.Read();
            csv.ReadHeader();

            while (await csv.ReadAsync())
            {
                csv.TryGetField("id", out string? docNumber);
                csv.TryGetField("doctor", out string? tcCode);

                if (!string.IsNullOrEmpty(docNumber) && !string.IsNullOrEmpty(tcCode))
                {
                    List<BeneficiariesEntity> benList = await context.Beneficiaries.Where(x => x.DocumentNumber == docNumber).ToListAsync();
                    List<TrainingCenterEntity> tcList = await context.TrainingCenters.Where(x => x.Code.ToLower() == tcCode.ToLower()).ToListAsync();

                    if (benList.Count > 0 && tcList.Count > 0)
                    {
                        csv.TryGetField("created", out string? createdOnStr);
                        csv.TryGetField("Weight value", out string? weightStr);
                        csv.TryGetField("Height value", out string? heightStr);
                        csv.TryGetField("BMI value", out string? bmiStr);
                        if (!string.IsNullOrEmpty(createdOnStr) && 
                            !string.IsNullOrEmpty(weightStr) && 
                            Decimal.TryParse(weightStr, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal weightValue) &&
                            !string.IsNullOrEmpty(heightStr) && 
                            Decimal.TryParse(heightStr, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal heightValue) &&
                            !string.IsNullOrEmpty(bmiStr) && 
                            Decimal.TryParse(bmiStr, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal bmiValue)) 
                        {
                            createdOnStr = createdOnStr.Replace("p. m.", "PM").Replace("a. m.", "AM");
                            DateTime.TryParseExact(createdOnStr, "dd/MM/yyyy h:mm:ss tt",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None, out DateTime CreatedOn);

                            models.Add(new BeneficiaryAnthropometricDataEntity
                            {
                                Id = Guid.NewGuid(),
                                BeneficiaryId = benList[0].Id,
                                Beneficiaries = benList[0],
                                TrainingCenterId = tcList[0].Id,
                                TrainingCenter = tcList[0],
                                Comment = csv.GetField("comment"),
                                Weight = weightValue,
                                Height = heightValue,
                                Bmi = bmiValue,
                                CreatedOn = CreatedOn.ToUniversalTime()
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed due to -> " + ex.ToString());
        }

        return models;
    }
}
