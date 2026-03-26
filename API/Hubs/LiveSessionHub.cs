using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace API.Hubs
{
    [Authorize]
    public class LiveSessionHub : Hub
    {
        public Task JoinSessionGroup(string sessionId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
        }

        public Task LeaveSessionGroup(string sessionId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId);
        }
    }
}
