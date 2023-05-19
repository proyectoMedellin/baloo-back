using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Applications.Item.CreatedOnBehalfOf;
using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;
using SiecaAPI.Commons;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.DTO.Data;
using SiecaAPI.Errors;
using SiecaAPI.Models;
using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SiecaAPI.Data.SQLImpl
{
    public class DaoBeneficiariesSqlImpl : IDaoBeneficiaries
    {
        public async Task<DtoBeneficiaries> CreateAsync(DtoBeneficiaries beneficiary)
        {
            using SqlContext context = new();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                OrganizationEntity org = await context.Organizations
                    .Where(o => o.Id.Equals(beneficiary.OrganizationId)).FirstAsync();
                DocumentTypeEntity bDocType = await context.DocumentTypes
                    .Where(dt => dt.Id.Equals(beneficiary.DocumentTypeId)).FirstAsync();
                BeneficiariesParametersEntity gender = await context.BeneficiariesParameters
                    .Where(g => g.Id.Equals(beneficiary.GenderId)).FirstAsync();
                BeneficiariesParametersEntity rh = await context.BeneficiariesParameters
                    .Where(rh => rh.Id.Equals(beneficiary.RhId)).FirstAsync();
                BeneficiariesParametersEntity bloodType = await context.BeneficiariesParameters
                    .Where(bt => bt.Id.Equals(beneficiary.BloodTypeId)).FirstAsync();
                BeneficiariesParametersEntity adressZone = await context.BeneficiariesParameters
                    .Where(az => az.Id.Equals(beneficiary.AdressZoneId)).FirstAsync();

                CountryEntity birthContry = await context.Countries
                    .Where(c => c.Id.Equals(beneficiary.BirthCountryId)).FirstAsync();
                DepartmentEntity birthDepartment = await context.Departments
                    .Where(d => d.Id.Equals(beneficiary.BirthDepartmentId)).FirstAsync();
                CityEntity birthCity = await context.Cities
                    .Where(c => c.Id.Equals(beneficiary.BirthCityId)).FirstAsync();

                BeneficiariesEntity newBeneficiary = new()
                {
                    OrganizationId = beneficiary.OrganizationId,
                    Organization = org,
                    DocumentTypeId = beneficiary.DocumentTypeId,
                    DocumentType = bDocType,
                    DocumentNumber = beneficiary.DocumentNumber,
                    FirstName = beneficiary.FirstName,  
                    OtherNames = beneficiary.OtherNames,
                    LastName = beneficiary.LastName,
                    OtherLastName = beneficiary.OtherLastName,
                    GenderId = gender.Id,
                    Gender = gender,
                    BirthDate = beneficiary.BirthDate.ToUniversalTime(),
                    BirthCountryId = birthContry.Id,
                    BirthCountry = birthContry,
                    BirthDepartmentId = birthDepartment.Id,
                    BirthDepartment = birthDepartment,
                    BirthCityId = birthCity.Id,
                    BirthCity = birthCity,
                    RhId = rh.Id,
                    Rh = rh,
                    BloodTypeId = bloodType.Id,
                    BloodType = bloodType,
                    EmergencyPhoneNumber = beneficiary.EmergencyPhoneNumber,
                    PhotoUrl = beneficiary.PhotoUrl,
                    AdressZoneId = adressZone.Id,
                    AdressZone = adressZone,
                    Adress = beneficiary.Adress,
                    Neighborhood = beneficiary.Neighborhood,
                    AdressPhoneNumber = beneficiary.AdressPhoneNumber,
                    AdressObservations = beneficiary.AdressObservations,
                    Enabled = beneficiary.Enabled,
                    CreatedBy = beneficiary.CreatedBy,
                    CreatedOn = DateTime.UtcNow,
                };
                await context.Beneficiaries.AddAsync(newBeneficiary);
                await context.SaveChangesAsync();
                beneficiary.Id = newBeneficiary.Id;

                //se adicionan los familiares
                for (int fcount = 0; fcount < beneficiary.FamilyMembers.Count; fcount++)
                {
                    DtoBeneficiariesFamily f = beneficiary.FamilyMembers[fcount];

                    DocumentTypeEntity bDocTypeFM = await context.DocumentTypes
                    .Where(dt => dt.Id.Equals(f.DocumentTypeId)).FirstAsync();

                    BeneficiariesParametersEntity fRelation = await context.BeneficiariesParameters
                    .Where(fr => fr.Id.Equals(f.FamilyRelationId)).FirstAsync();

                    BeneficiariesFamilyEntity newFmember = new()
                    {
                        OrganizationId = org.Id,
                        Organization = org,
                        BeneficiaryId = newBeneficiary.Id,
                        Beneficiary = newBeneficiary,
                        DocumentTypeId = bDocTypeFM.Id,
                        DocumentType = bDocTypeFM,
                        DocumentNumber = f.DocumentNumber,
                        FirstName = f.FirstName,
                        OtherNames = f.OtherNames,
                        LastName = f.LastName,
                        OtherLastName = f.OtherLastName,
                        FamilyRelationId = fRelation.Id,
                        FamilyRelation = fRelation,
                        Attendant = f.Attendant,
                        Enabled = f.Enabled,
                        CreatedBy = newBeneficiary.CreatedBy,
                        CreatedOn = DateTime.UtcNow
                    };

                    await context.BeneficiariesFamilies.AddAsync(newFmember);
                    await context.SaveChangesAsync();
                    beneficiary.FamilyMembers[fcount].Id = newFmember.Id;
                    beneficiary.FamilyMembers[fcount].BeneficiaryId = newFmember.BeneficiaryId;
                    beneficiary.FamilyMembers[fcount].OrganizationId = newFmember.OrganizationId;
                }

                transaction.Commit();
                return beneficiary;
            }
            catch(Exception e)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<DtoBeneficiaries> UpdateAsync(DtoBeneficiaries beneficiary)
        {
            using SqlContext context = new();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                BeneficiariesEntity updBen = await context.Beneficiaries
                    .Where(b => b.Id.Equals(beneficiary.Id)).FirstAsync();

                OrganizationEntity org = await context.Organizations
                    .Where(o => o.Id.Equals(beneficiary.OrganizationId)).FirstAsync();
                DocumentTypeEntity bDocType = await context.DocumentTypes
                    .Where(dt => dt.Id.Equals(beneficiary.DocumentTypeId)).FirstAsync();
                BeneficiariesParametersEntity gender = await context.BeneficiariesParameters
                    .Where(g => g.Id.Equals(beneficiary.GenderId)).FirstAsync();
                BeneficiariesParametersEntity rh = await context.BeneficiariesParameters
                    .Where(rh => rh.Id.Equals(beneficiary.RhId)).FirstAsync();
                BeneficiariesParametersEntity bloodType = await context.BeneficiariesParameters
                    .Where(bt => bt.Id.Equals(beneficiary.BloodTypeId)).FirstAsync();
                BeneficiariesParametersEntity adressZone = await context.BeneficiariesParameters
                    .Where(az => az.Id.Equals(beneficiary.AdressZoneId)).FirstAsync();

                CountryEntity birthContry = await context.Countries
                    .Where(c => c.Id.Equals(beneficiary.BirthCountryId)).FirstAsync();
                DepartmentEntity birthDepartment = await context.Departments
                    .Where(d => d.Id.Equals(beneficiary.BirthDepartmentId)).FirstAsync();
                CityEntity birthCity = await context.Cities
                    .Where(c => c.Id.Equals(beneficiary.BirthCityId)).FirstAsync();

                updBen.OrganizationId = beneficiary.OrganizationId;
                updBen.Organization = org;
                updBen.DocumentTypeId = beneficiary.DocumentTypeId;
                updBen.DocumentType = bDocType;
                updBen.DocumentNumber = beneficiary.DocumentNumber;
                updBen.FirstName = beneficiary.FirstName;
                updBen.OtherNames = beneficiary.OtherNames;
                updBen.LastName = beneficiary.LastName;
                updBen.OtherLastName = beneficiary.OtherLastName;
                updBen.GenderId = gender.Id;
                updBen.Gender = gender;
                updBen.BirthDate = beneficiary.BirthDate.ToUniversalTime();
                updBen.BirthCountryId = birthContry.Id;
                updBen.BirthCountry = birthContry;
                updBen.BirthDepartmentId = birthDepartment.Id;
                updBen.BirthDepartment = birthDepartment;
                updBen.BirthCityId = birthCity.Id;
                updBen.BirthCity = birthCity;
                updBen.RhId = rh.Id;
                updBen.Rh = rh;
                updBen.BloodTypeId = bloodType.Id;
                updBen.BloodType = bloodType;
                updBen.EmergencyPhoneNumber = beneficiary.EmergencyPhoneNumber;
                updBen.PhotoUrl = beneficiary.PhotoUrl;
                updBen.AdressZoneId = adressZone.Id;
                updBen.AdressZone = adressZone;
                updBen.Adress = beneficiary.Adress;
                updBen.Neighborhood = beneficiary.Neighborhood;
                updBen.AdressPhoneNumber = beneficiary.AdressPhoneNumber;
                updBen.AdressObservations = beneficiary.AdressObservations;
                updBen.Enabled = beneficiary.Enabled;
                updBen.ModifiedBy = beneficiary.ModifiedBy;
                updBen.ModifiedOn = DateTime.UtcNow;
                await context.SaveChangesAsync();

                //se eliminan los familiares y se vuelven a adicionar
                context.BeneficiariesFamilies.RemoveRange(
                            await context.BeneficiariesFamilies
                            .Where(bf => bf.BeneficiaryId.Equals(updBen.Id))
                            .ToListAsync());
                await context.SaveChangesAsync();

                for (int fcount = 0; fcount < beneficiary.FamilyMembers.Count; fcount++)
                {
                    DtoBeneficiariesFamily f = beneficiary.FamilyMembers[fcount];

                    DocumentTypeEntity bDocTypeFM = await context.DocumentTypes
                    .Where(dt => dt.Id.Equals(f.DocumentTypeId)).FirstAsync();

                    BeneficiariesParametersEntity fRelation = await context.BeneficiariesParameters
                    .Where(fr => fr.Id.Equals(f.FamilyRelationId)).FirstAsync();

                    BeneficiariesFamilyEntity newFmember = new()
                    {
                        OrganizationId = org.Id,
                        Organization = org,
                        BeneficiaryId = updBen.Id,
                        Beneficiary = updBen,
                        DocumentTypeId = bDocTypeFM.Id,
                        DocumentType = bDocTypeFM,
                        DocumentNumber = f.DocumentNumber,
                        FirstName = f.FirstName,
                        OtherNames = f.OtherNames,
                        LastName = f.LastName,
                        OtherLastName = f.OtherLastName,
                        FamilyRelationId = fRelation.Id,
                        FamilyRelation = fRelation,
                        Attendant = f.Attendant,
                        Enabled = f.Enabled,
                        CreatedBy = updBen.ModifiedBy,
                        CreatedOn = DateTime.UtcNow,
                        ModifiedBy = updBen.ModifiedBy,
                        ModifiedOn = DateTime.UtcNow
                    };
                    await context.BeneficiariesFamilies.AddAsync(newFmember);
                    await context.SaveChangesAsync();

                    beneficiary.FamilyMembers[fcount].Id = newFmember.Id;
                    beneficiary.FamilyMembers[fcount].BeneficiaryId = newFmember.BeneficiaryId;
                    beneficiary.FamilyMembers[fcount].OrganizationId = newFmember.OrganizationId;
                }

                transaction.Commit();
                return beneficiary;
            }
            catch(Exception e)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<DtoBeneficiaries> UpdatePhotoUrl(Guid beneficiaryId, string url)
        {
            using SqlContext context = new();
            BeneficiariesEntity benReq = await context.Beneficiaries
                    .Where(b => b.Id.Equals(beneficiaryId)).FirstAsync();

            benReq.PhotoUrl = url;
            await context.SaveChangesAsync();

            DtoBeneficiaries response = new()
            {
                Id = benReq.Id,
                OrganizationId = benReq.OrganizationId,
                DocumentTypeId = benReq.DocumentTypeId,
                DocumentNumber = benReq.DocumentNumber,
                FirstName = benReq.FirstName,
                OtherNames = benReq.OtherNames,
                LastName = benReq.LastName,
                OtherLastName = benReq.OtherLastName,
                GenderId = benReq.GenderId,
                BirthDate = benReq.BirthDate.ToUniversalTime(),
                BirthCountryId = benReq.BirthCountryId,
                BirthDepartmentId = benReq.BirthDepartmentId,
                BirthCityId = benReq.BirthCityId,
                RhId = benReq.RhId,
                BloodTypeId = benReq.BloodTypeId,
                EmergencyPhoneNumber = benReq.EmergencyPhoneNumber,
                PhotoUrl = benReq.PhotoUrl,
                AdressZoneId = benReq.AdressZoneId,
                Adress = benReq.Adress,
                Neighborhood = benReq.Neighborhood,
                AdressPhoneNumber = benReq.AdressPhoneNumber,
                AdressObservations = benReq.AdressObservations,
                Enabled = benReq.Enabled,
                FamilyMembers = new()
            };

            return response;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using SqlContext context = new();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                context.BeneficiariesFamilies
                    .RemoveRange(await context.BeneficiariesFamilies
                        .Where(bf => bf.BeneficiaryId.Equals(id))
                        .ToListAsync());
                await context.SaveChangesAsync();

                context.Beneficiaries
                    .RemoveRange(await context.Beneficiaries
                        .Where(b => b.Id.Equals(id)).ToListAsync());
                await context.SaveChangesAsync();

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }

        public async Task<bool> ExistAsync(Guid docmentTypeId, string documentNumber)
        {
            using SqlContext context = new();
            List<BeneficiariesEntity> ben = await context.Beneficiaries
                .Where(b => b.DocumentTypeId.Equals(docmentTypeId) && b.DocumentNumber.Equals(documentNumber))
                .ToListAsync();

            return ben.Count > 0;
        }

        public async Task<List<DtoBeneficiariesParameters>> GetBeneficiaryParameterInfoByType(string type)
        {
            List<DtoBeneficiariesParameters> response = new();
            
            using SqlContext context = new();
            List<BeneficiariesParametersEntity> paramsData = await context.BeneficiariesParameters
                .Where(bp => bp.ParamType.Equals(type)).ToListAsync();

            foreach (BeneficiariesParametersEntity p in paramsData)
            {
                response.Add(new DtoBeneficiariesParameters() { 
                    Id = p.Id,
                    OrganizationId = p.OrganizationId,
                    ParamType = p.ParamType,
                    ParamCode = p.ParamCode,
                    ParamValue = p.ParamValue
                });
            }
            return response;
        }

        public async Task<DtoBeneficiaries> GetById(Guid id)
        {
            using SqlContext context = new();
            try
            {
                BeneficiariesEntity benReq = await context
                    .Beneficiaries
                    .Where(b => b.Id.Equals(id))
                    .Include(b => b.DocumentType)
                    .FirstAsync();

                DtoBeneficiaries response = new() { 
                    Id = benReq.Id,
                    OrganizationId = benReq.OrganizationId,
                    DocumentTypeId = benReq.DocumentTypeId,
                    DocumentTypeName = benReq.DocumentType.Name,
                    DocumentNumber = benReq.DocumentNumber,
                    FirstName = benReq.FirstName,
                    OtherNames = benReq.OtherNames,
                    LastName = benReq.LastName,
                    OtherLastName = benReq.OtherLastName,
                    GenderId = benReq.GenderId,
                    BirthDate = benReq.BirthDate.ToUniversalTime(),
                    BirthCountryId = benReq.BirthCountryId,
                    BirthDepartmentId = benReq.BirthDepartmentId,
                    BirthCityId = benReq.BirthCityId,
                    RhId = benReq.RhId,
                    BloodTypeId = benReq.BloodTypeId,
                    EmergencyPhoneNumber = benReq.EmergencyPhoneNumber,
                    PhotoUrl = benReq.PhotoUrl,
                    AdressZoneId = benReq.AdressZoneId,
                    Adress = benReq.Adress,
                    Neighborhood = benReq.Neighborhood,
                    AdressPhoneNumber = benReq.AdressPhoneNumber,
                    AdressObservations = benReq.AdressObservations,
                    Enabled = benReq.Enabled,
                    FamilyMembers = new()
                };

                List<BeneficiariesFamilyEntity> family = await context
                    .BeneficiariesFamilies.Where(bf => bf.BeneficiaryId.Equals(benReq.Id))
                    .Include(f => f.DocumentType)
                    .ToListAsync();

                foreach(BeneficiariesFamilyEntity f in family)
                {
                    response.FamilyMembers.Add(new DtoBeneficiariesFamily() { 
                        Id = f.Id,
                        BeneficiaryId = f.BeneficiaryId,
                        OrganizationId = f.OrganizationId,
                        DocumentNumber = f.DocumentNumber,
                        DocumentTypeId = f.DocumentTypeId,
                        FirstName = f.FirstName,
                        OtherNames = f.OtherNames,
                        LastName = f.LastName,
                        OtherLastName = f.OtherLastName,
                        FamilyRelationId = f.FamilyRelationId,
                        Attendant = f.Attendant,
                        Enabled = f.Enabled
                    });
                }

                return response;
            }
            catch
            {
                throw new NoDataFoundException("the requested beneficiary dosent exist");
            }
        }

        public async Task<List<DtoBeneficiaries>> GetAllAsync(int? year, Guid? trainingCenterId,
            Guid? campusId, Guid? developmentRoomId, Guid? documentType, string? documentNumber,
            string? name, string? fGroup, bool? fEnabled, int? page, int? pageSize)
        {
            List<DtoBeneficiaries> response = new();

            using SqlContext context = new();

            List<BeneficiariesEntity> baseRsp;

            //verifico asignaciones de centro, sala, salon grupo
            if (year.HasValue || trainingCenterId.HasValue || campusId.HasValue || developmentRoomId.HasValue
                || !string.IsNullOrEmpty(fGroup))
            {
                var drAssignmentQuery = from dry in context.DevelopmentRoomGroupByYears
                                        join dr in context.DevelopmentRooms on dry.DevelopmentRoomId equals dr.Id
                                        join drb in context.DevelopmentRoomGroupBeneficiaries on dry.Id equals drb.DevelopmentRoomGroupByYearId
                                        join b in context.Beneficiaries on drb.BeneficiaryId equals b.Id
                                        where
                                        (!year.HasValue || (year.HasValue && dry.Year.Equals(year.Value))) &&
                                        (!trainingCenterId.HasValue ||
                                            (trainingCenterId.HasValue && dr.TrainingCenterId.Equals(trainingCenterId.Value))) &&
                                        (!campusId.HasValue ||
                                            (campusId.HasValue && dr.CampusId.Equals(campusId.Value))) &&
                                        (!developmentRoomId.HasValue ||
                                            (developmentRoomId.HasValue && dr.Id.Equals(developmentRoomId.Value))) &&
                                        (string.IsNullOrEmpty(fGroup) || (dry.GroupCode + dry.GroupName).ToLower().Contains(fGroup.ToLower())) &&
                                        ((!documentType.HasValue) ||
                                            (documentType.HasValue && b.DocumentTypeId.Equals(documentType.Value))) &&
                                        (string.IsNullOrEmpty(documentNumber) || b.DocumentNumber.Contains(documentNumber)) &&
                                        (string.IsNullOrEmpty(name) ||
                                            (b.FirstName + b.OtherNames + b.LastName + b.OtherLastName).ToLower().Contains(name.ToLower())) &&
                                        ((!fEnabled.HasValue && b.Enabled) || (fEnabled.HasValue && b.Enabled == fEnabled))
                                        select b;
                if (page.HasValue && pageSize.HasValue)
                {
                    int skipData = page.Value > 0 ? (page.Value - 1) * pageSize.Value : 0;
                    drAssignmentQuery.Skip(skipData).Take(pageSize.Value);
                }
                
                baseRsp = await drAssignmentQuery.Include(b => b.DocumentType).ToListAsync();
            }
            else
            {
                //si no requeriere filtros por ubicación
                var benQuery = from b in context.Beneficiaries
                               where
                                ((!documentType.HasValue) ||
                                    (documentType.HasValue && b.DocumentTypeId.Equals(documentType.Value))) &&
                                (string.IsNullOrEmpty(documentNumber) || b.DocumentNumber.Contains(documentNumber)) &&
                                (string.IsNullOrEmpty(name) || 
                                    (b.FirstName + b.OtherNames + b.LastName + b.OtherLastName).ToLower().Contains(name.ToLower())) &&
                                ((!fEnabled.HasValue && b.Enabled) || (fEnabled.HasValue && b.Enabled == fEnabled))
                               select b;

                if (page.HasValue && pageSize.HasValue)
                {
                    int skipData = page.Value > 0 ? (page.Value - 1) * pageSize.Value : 0;
                    benQuery.Skip(skipData).Take(pageSize.Value);
                }
                baseRsp = await benQuery.Include(b => b.DocumentType).ToListAsync();
            }

            if (baseRsp != null && baseRsp.Count > 0)
            {
                foreach (BeneficiariesEntity benReq in baseRsp)
                {
                    response.Add(new()
                    {
                        Id = benReq.Id,
                        OrganizationId = benReq.OrganizationId,
                        DocumentTypeId = benReq.DocumentTypeId,
                        DocumentTypeName = benReq.DocumentType.Name,
                        DocumentNumber = benReq.DocumentNumber,
                        FirstName = benReq.FirstName,
                        OtherNames = benReq.OtherNames,
                        LastName = benReq.LastName,
                        OtherLastName = benReq.OtherLastName,
                        GenderId = benReq.GenderId,
                        BirthDate = benReq.BirthDate.ToUniversalTime(),
                        BirthCountryId = benReq.BirthCountryId,
                        BirthDepartmentId = benReq.BirthDepartmentId,
                        BirthCityId = benReq.BirthCityId,
                        RhId = benReq.RhId,
                        BloodTypeId = benReq.BloodTypeId,
                        EmergencyPhoneNumber = benReq.EmergencyPhoneNumber,
                        PhotoUrl = benReq.PhotoUrl,
                        AdressZoneId = benReq.AdressZoneId,
                        Adress = benReq.Adress,
                        Neighborhood = benReq.Neighborhood,
                        AdressPhoneNumber = benReq.AdressPhoneNumber,
                        AdressObservations = benReq.AdressObservations,
                        Enabled = benReq.Enabled,
                        FamilyMembers = new()
                    });
                }
            }

            return response;
        }

        public async Task<List<DtoBeneficiariesAnthropometricRecord>> GetAnthropometricDataFromBeneficiaryId(Guid id
            , DateTime from, DateTime to)
        {
            using SqlContext context = new();
            try
            {
                //el filtro de fecha no funciona bien
                List<BeneficiaryAnthropometricDataEntity> benReq = await context.BeneficiaryAnthropometricRecords
                    .Where(r => r.BeneficiaryId.Equals(id) && 
                        r.CreatedOn >= from.ToUniversalTime() && r.CreatedOn <= to.ToUniversalTime())
                    .Include(r => r.TrainingCenter)
                    .OrderByDescending(r => r.CreatedOn)
                    .ToListAsync();

                List<DtoBeneficiariesAnthropometricRecord> response = new();
                if (benReq.Count > 0)
                {
                    foreach (BeneficiaryAnthropometricDataEntity bar in benReq)
                    {
                        response.Add(new DtoBeneficiariesAnthropometricRecord() { 
                            Id = bar.Id,
                            BeneficiaryId = bar.BeneficiaryId,
                            TrainingCenterId = bar.TrainingCenterId,
                            TrainingCenterCode = bar.TrainingCenter.Code,
                            TrainingCenterName = bar.TrainingCenter.Name,
                            Weight = bar.Weight,
                            Height = bar.Height,
                            Bmi = bar.Bmi,
                            Comment = bar.Comment ?? String.Empty,
                            CreatedOn = bar.CreatedOn,
                            ModifiedOn = bar.ModifiedOn
                        });
                    }
                }

                return response;
            }
            catch
            {
                throw new NoDataFoundException("the requested beneficiary dosen't have anthrometric data");
            }
        }   

        public async Task<List<DtoBeneficiariesEmotionsRecord>> GetEmotionsDataById(Guid beneficiaryId, DateTime from, DateTime to)
        {
            using SqlContext context = new();
            try
            {
                List<DateTime> days = new();
                DateTime cDate = from;
                while (cDate <= to)
                {
                    days.Add(cDate);
                    cDate = cDate.AddDays(1);
                }

                string queryString = "SELECT (md5(((random())::text || (clock_timestamp())::text)))::uuId as \"Id\", " +
                    "b.\"Id\" as \"BeneficiaryId\", dr.\"Id\" as \"DevelopmentRoomId\", " +
                    "bred.\"Id\" as \"IntegrationId\", bred.\"EmotionId\", bred.\"CreatedOn\" " +
                    "FROM \"DevelopmentRoom\" dr " +
                    "JOIN \"BeneficiaryRawEmotionsData\" bred ON dr.\"DahuaChannelCode\" = bred.\"DahuaChannelName\" " +
                    "JOIN \"Beneficiaries\" b ON bred.\"PersonId\" = b.\"DocumentNumber\" " +
                    $"WHERE b.\"Id\" = '{beneficiaryId}' and bred.\"CreatedOn\" between '{from.ToString("yyyy-MM-dd")}' and '{to.AddDays(1).ToString("yyyy-MM-dd")}' " +
                    "ORDER BY b.\"Id\", dr.\"Id\", bred.\"CreatedOn\" ";

                List<BeneficiariesEmotionsRecordEntity> benReq = await context
                    .BeneficiariesEmotionsRecords.FromSqlRaw(queryString).ToListAsync();

                List <DtoBeneficiariesEmotionsRecord> response = new();
                foreach (DateTime d in days)
                {
                    DtoBeneficiariesEmotionsRecord dayEmotion = new();
                    dayEmotion.CreatedOn = d;
                    List<BeneficiariesEmotionsRecordEntity> reqs = benReq
                        .Where(r => r.CreatedOn.ToString("yyyy-MM-dd").Equals(d.ToString("yyyy-MM-dd"))).ToList();

                    foreach (BeneficiariesEmotionsRecordEntity bar in reqs)
                    {
                        dayEmotion = new();
                        dayEmotion.CreatedOn = bar.CreatedOn;
                        dayEmotion.Id = bar.Id;
                        dayEmotion.BeneficiaryId = bar.BeneficiaryId;
                        dayEmotion.DevelopmentRoomId = bar.DevelopmentRoomId;
                        dayEmotion.IntegrationId = bar.IntegrationId;
                        dayEmotion.EmotionId = bar.EmotionId;
                        dayEmotion.EmotionName = PrConstants.GetEmotionName(bar.EmotionId);
                        response.Add(dayEmotion);
                    }
                    if(dayEmotion.Id.Equals(Guid.Empty)) response.Add(dayEmotion);
                }
                return response;
            }
            catch(Exception ex)
            {
                throw new NoDataFoundException("the requested beneficiary dosen't have emotions data");
            }
        }

        public async Task<List<DtoBeneficiariesEmotionsRecord>> GetAssistenceDataById(Guid beneficiaryId, DateTime from, DateTime to)
        {
            using SqlContext context = new();
            try
            {
                List<DateTime> days = new();
                DateTime cDate = from;
                while (cDate <= to)
                {
                    days.Add(cDate);
                    cDate = cDate.AddDays(1);
                }

                string queryString = "SELECT (md5(((random())::text || (clock_timestamp())::text)))::uuId as \"Id\", " +
                    "b.\"Id\" as \"BeneficiaryId\", dr.\"Id\" as \"DevelopmentRoomId\", " +
                    "bred.\"Id\" as \"IntegrationId\", bred.\"EmotionId\", bred.\"CreatedOn\" " +
                    "FROM \"DevelopmentRoom\" dr " +
                    "JOIN \"BeneficiaryRawEmotionsData\" bred ON dr.\"DahuaChannelCode\" = bred.\"DahuaChannelName\" " +
                    "JOIN \"Beneficiaries\" b ON bred.\"PersonId\" = b.\"DocumentNumber\" " +
                    $"WHERE b.\"Id\" = '{beneficiaryId}' and bred.\"CreatedOn\" between '{from.ToString("yyyy-MM-dd")}' and '{to.AddDays(1).ToString("yyyy-MM-dd")}' " +
                    "ORDER BY b.\"Id\", dr.\"Id\", bred.\"CreatedOn\" ";

                List<BeneficiariesEmotionsRecordEntity> benReq = await context
                    .BeneficiariesEmotionsRecords.FromSqlRaw(queryString).ToListAsync();

                List<DtoBeneficiariesEmotionsRecord> response = new();
                foreach (DateTime d in days)
                {
                    DtoBeneficiariesEmotionsRecord dayEmotion = new();
                    dayEmotion.CreatedOn = d;

                    List<BeneficiariesEmotionsRecordEntity> reqs = benReq
                        .Where(r => r.CreatedOn.ToString("yyyy-MM-dd").Equals(d.ToString("yyyy-MM-dd"))).ToList();

                    if (reqs.Count > 0)
                    {
                        BeneficiariesEmotionsRecordEntity bar = reqs.First();
                        dayEmotion.CreatedOn = d;
                        dayEmotion.Id = bar.Id;
                        dayEmotion.BeneficiaryId = bar.BeneficiaryId;
                        dayEmotion.DevelopmentRoomId = bar.DevelopmentRoomId;
                        dayEmotion.IntegrationId = bar.IntegrationId;
                        dayEmotion.EmotionId = bar.EmotionId;
                        dayEmotion.EmotionName = PrConstants.GetEmotionName(bar.EmotionId);
                    }
                    response.Add(dayEmotion);
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new NoDataFoundException("the requested beneficiary dosen't have emotions data");
            }
        }
    }
}
