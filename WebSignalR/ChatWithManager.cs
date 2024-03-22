using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ProjectAspEShop2024.AppIdentity;
using System.Security.Claims;

namespace ProjectAspEShop2024.WebSignalR
{
    public class ChatWithManager : Hub
    {
        private readonly string _receiveMethod = "ReceiveMessage";
        private readonly UserManager<AppUser> _userManager;
        private readonly HttpContext _httpContext;
        private static string _managerConnectionId;
        private static string _managerName;

        private static Dictionary<string, string> _allClientsById = 
            new Dictionary<string, string>();
        private static Dictionary<string, string> _allClientsByName =
            new Dictionary<string, string>();


        public ChatWithManager(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContext = httpContextAccessor?.HttpContext;
        }

        private async void DefineManagerName(ClaimsPrincipal principle)
        {
            // контекст для БД - создаётся новый
            var userManager = _httpContext
                .RequestServices
                .GetRequiredService<UserManager<AppUser>>();

            var user = await userManager.GetUserAsync(principle);

            if (!String.IsNullOrEmpty(user.Lastname) &&
                !String.IsNullOrEmpty(user.Firstname))  
            {
                _managerName = $"{user.Lastname} {user.Firstname}";
            }
            else
            {
                _managerName = user.UserName;
            }
        }

        public async override Task<Task> OnConnectedAsync()
        {
            string id = Context.ConnectionId;
            var principle = Context.User;

            if (principle.IsInRole("sales_manager"))
            {
                _managerConnectionId = id;
                DefineManagerName(principle);
            } else if (!_allClientsById.ContainsKey(id))
            {
                _allClientsById.Add(id, string.Empty);
            }

            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string userName, string message)
        {
            string id = Context.ConnectionId;

            if (_allClientsById.ContainsKey(id))
            { // сообщение от клиента - переправляем Менеджеру
                _allClientsById[id] = userName;
                if (!_allClientsByName.ContainsKey(userName))
                {
                    _allClientsByName.Add(userName, id);
                }
                else
                {
                    if (id != _allClientsByName[userName])
                    { // коллизия - попытка использовать занятый Логин
                        await Clients.Client(id)
                            .SendAsync("this user name already occupied!");
                    }
                }

                if (!String.IsNullOrEmpty(_managerConnectionId))
                {
                    await Clients
                        .Client(_managerConnectionId)
                        .SendAsync(_receiveMethod, userName, message);
                }
                else
                {
                    // менеджер не в сети - в базу писать?
                }
            }
            else if (id == _managerConnectionId) 
            { // СООБЩЕНИЕ ОТ МЕНЕДЖЕРА - ПЕРЕПРАВЛЯЕМ КОНКРЕТНОМУ КЛИЕНТУ!
                if (_allClientsByName.TryGetValue(userName,out string clientId))
                {
                    await Clients
                        .Client(clientId)
                        .SendAsync(_receiveMethod, $"Менеджер '{_managerName}'", message);
                }
                else
                {
                    await Clients
                        .Client(_managerConnectionId)
                        .SendAsync(_receiveMethod, $"client '{userName}' not found");
                }
            }
        }
    }
}
