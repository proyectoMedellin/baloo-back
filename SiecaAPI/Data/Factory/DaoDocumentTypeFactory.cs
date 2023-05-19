using Microsoft.VisualBasic;
using SiecaAPI.Commons;
using SiecaAPI.Data.DataverseImpl;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl;
namespace SiecaAPI.Data.Factory
{
    public static class DaoDocumentTypeFactory
    {
        public static IDaoDocumentTypes GetDaoDocumenTypes()
        {
            return AppParamsTools.GetEnvironmentVariable("DBMS") switch
            {
                PrConstants.DBMS_SQL => new DaoDocumentTypeSqlImpl(),
                PrConstants.DBMS_DATAVERSE => new DaoDocumentTypeDverseImpl(),
                _ => throw
                new Exception(PrConstants.DBMS_WRONG_PARAM_ERROR),
            };
        }
    }
}
