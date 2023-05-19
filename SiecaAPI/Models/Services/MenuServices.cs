using SiecaAPI.Data.Factory;
using SiecaAPI.DTO.Data;
using SiecaAPI.Models;

namespace SiecaAPI.Services
{
    public static class MenuServices
    {

        public static async Task<List<DtoMenu>> GetAllMenuItemasAsync(string user)
        {

            List<DtoMenu> menuItems = await DaoMenuFactory.
                GetDaoMenu().GetItems(user);
            return menuItems;
        }
    }
}
