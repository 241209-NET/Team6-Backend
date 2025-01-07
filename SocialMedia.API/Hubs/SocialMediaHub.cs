using Microsoft.AspNetCore.SignalR;

namespace SocialMedia.API.Hubs
{
    public class SocialMediaHub : Hub
    {
        public async Task SendTweet(string user, string tweet)
        {
            // this let's the new tweet be seen by all connected clients
            await Clients.All.SendAsync("ReceiveTweet", user, tweet);
        }
    }
}
