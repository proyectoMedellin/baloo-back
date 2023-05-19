using SiecaAPI.Commons;
using SiecaAPI.Data.DataverseImpl;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl;

namespace SiecaAPI.Data.Factory
{
    public static class DaoLocationFactory
    {
        public static IDaoLocation GetDaoLocation()
        {
            return AppParamsTools.GetEnvironmentVariable("DBMS") switch
            {
                PrConstants.DBMS_SQL => new DaoLocationSqlImpl(),
                _ => throw
                new Exception(PrConstants.DBMS_WRONG_PARAM_ERROR),
            };
        }
    }
}
