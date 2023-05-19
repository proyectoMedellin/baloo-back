using Newtonsoft.Json;
using SiecaAPI.Commons;
using SiecaAPI.Data.Factory;
using SiecaAPI.DTO.Data;
using SiecaAPI.Errors;

namespace SiecaAPI.Models.Services
{
    public static class AuthenticationServices
    {
        public static async Task<string> IsValidLogin(string userData)
        {
            string userName = string.Empty;

            //string authStr = SecurityTools.DecryptAes(userData);
            string authStr = userData;

            string[] userDataArray = authStr.Split(PrConstants.USERDATASEPARATOR);
            if(userDataArray.Length == PrConstants.USERDATAPARAMSCOUNT)
            {
                userName = userDataArray[PrConstants.USERNAMEPOS];
                string pass = userDataArray[PrConstants.USERPASSPOS];
                DateTime reqGeneratedTime = DateTime.Parse(userDataArray[PrConstants.USERLOGINDATEPOS]);

                if ((DateTime.UtcNow - reqGeneratedTime).TotalSeconds > PrConstants.USERLOGINMAXVERIFICATIONTIME) 
                    throw new InvalidLoginException("Datos de acceso no validos");

                bool existUser = await DaoAccessUserFactory.GetDaoAccessUsers()
                    .ExistUserByUserNamePass(userName, pass);
                userName = existUser ? userName : String.Empty;
            }

            return userName;
        }

        public static async Task<string> CreateUserToken(string userName)
        {
            List<string> userRoles = new List<string>();

            var roles = await DaoAccessUserFactory.GetDaoAccessUsers().GetRolesByUser(userName);

            foreach (DtoUserRol role in roles)
            {
                if(role != null) userRoles.Add(role.RolId.ToString());
            }

            return SecurityTools.JwtTokenGenerator(userName, userRoles);
        }
    }
}
