using Microsoft.AspNetCore.SignalR;

namespace ProjectAspEShop2024.WebSignalR
{
    public class ChatRoomHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
