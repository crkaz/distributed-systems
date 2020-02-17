using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace WebApplication2.Hubs
{
    public class ChatHub : Hub
    {
        public async Task BroadcastMessage(string username, string message)
        {
            await Clients.All.SendAsync("GetMessage", username, message);
        }

    }
}
