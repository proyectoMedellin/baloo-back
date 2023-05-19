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

namespace SiecaAPI.DssPro;

public class DssProServiceJob : IJob
{
    private readonly IConfiguration configuration;

    public DssProServiceJob(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await DssProServices.DownloadFaceRecognitionData(null, null, null);
            Console.WriteLine("proceso ejecutado exitosamente");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed due to -> " + ex.ToString());
        }
    }
}
