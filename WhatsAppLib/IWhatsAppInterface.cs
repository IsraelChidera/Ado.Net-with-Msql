using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhatsAppLib.Model;

namespace WhatsAppLib
{
    public interface IWhatsAppInterface: IDisposable
    {
        Task<int> CreateUser(UserViewModel user);

        Task<int> UpdateUser(int id, UserViewModel user);

        Task<int> DeleteUser(int UserId);

        Task<UserViewModel> GetUser(int UserId);

        Task<IEnumerable<UserViewModel>> GetUsers();
    }

}
