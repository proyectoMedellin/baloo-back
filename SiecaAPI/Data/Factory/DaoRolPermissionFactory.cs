using Microsoft.VisualBasic;
using SiecaAPI.Commons;
using SiecaAPI.Data.DataverseImpl;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl;

namespace SiecaAPI.Data.Factory
{
    public static class DaoRolPermissionFactory
    {
        public static IDaoRolPermission GetDaoRolPermission()
        {
            return AppParamsTools.GetEnvironmentVariable("DBMS") switch
            {
                PrConstants.DBMS_SQL => new DaoRolPermissionSqlImpl(),
                PrConstants.DBMS_DATAVERSE => new DaoRolPermissionDverseImpl(),
                _ => throw
                new Exception(PrConstants.DBMS_WRONG_PARAM_ERROR),
            };
        }
    }
}
