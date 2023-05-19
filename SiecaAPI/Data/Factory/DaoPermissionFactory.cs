using Microsoft.VisualBasic;
using SiecaAPI.Commons;
using SiecaAPI.Data.DataverseImpl;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl;

namespace SiecaAPI.Data.Factory
{
    public static class DaoPermissionFactory
    {
        public static IDaoPermissions GetDaoPermission()
        {
            return AppParamsTools.GetEnvironmentVariable("DBMS") switch
            {
                PrConstants.DBMS_SQL => new DaoPermissionSqlImpl(),
                PrConstants.DBMS_DATAVERSE => new DaoPermissionDverseImpl(),
                _ => throw
                new Exception(PrConstants.DBMS_WRONG_PARAM_ERROR),
            };
        }
    }
}
