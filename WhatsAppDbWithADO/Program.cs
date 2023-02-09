using WhatsAppLib;
using WhatsAppLib.Model;

namespace WhatsAppDbWithADO
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            using(IWhatsAppInterface whatsAppService = new WhatsAppService(new WhatsAppDbConnection()) )
            {
                var userData = new UserViewModel
                {                    
                    Name = "onah moses",
                    PhoneNumber= "+6734554884",
                    ProfilePicture="loo.png"
                };

                /*var createdUserId = await whatsAppService.CreateUser(userData);
                Console.WriteLine(createdUserId);*/

                /*var getUserId = await whatsAppService.UpdateUser(34, userData);
                Console.WriteLine(getUserId);*/

                /*var deletedId = await whatsAppService.DeleteUser(2);
                Console.WriteLine(deletedId);*/

                //var getUser = await whatsAppService.GetUser(34);
                //Console.WriteLine(getUser);

                await whatsAppService.GetUsers();
                
            }
            

        }
    }
}