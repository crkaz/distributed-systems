using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Server;

namespace WebApplication2.Hubs
{
    public class ChatHub : Hub
    {
        public async Task BroadcastMessage(string username, string message)
        {
            await Clients.All.SendAsync("GetMessage", username, message);

            using (var ctx = new Context())
            {
                User user = new User() { Username = username };
                Message msg = new Message() { Content = message, User = user };

                if (!ctx.Users.Contains(user))
                {
                    ctx.Users.Add(user);
                }

                ctx.Messages.Add(msg);

                ctx.SaveChanges();
            }
        }
    }
}
