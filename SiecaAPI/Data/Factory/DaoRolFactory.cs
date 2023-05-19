using Microsoft.VisualBasic;
using SiecaAPI.Commons;
using SiecaAPI.Data.DataverseImpl;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl;

namespace SiecaAPI.Data.Factory
{
    public static class DaoRolFactory
    {
        public static IDaoRoles GetDaoRoles()
        {
            return AppParamsTools.GetEnvironmentVariable("DBMS") switch
            {
                PrConstants.DBMS_SQL => new DaoRolSqlImpl(),
                PrConstants.DBMS_DATAVERSE => new DaoRolDverseImpl(),
                _ => throw
                new Exception(PrConstants.DBMS_WRONG_PARAM_ERROR),
            };
        }
    }
}
