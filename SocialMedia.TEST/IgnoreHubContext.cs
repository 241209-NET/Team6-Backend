using Microsoft.AspNetCore.SignalR;

public class NoOpHubContext<THub> : IHubContext<THub>
    where THub : Hub
{
    public IHubClients Clients => new NoOpHubClients();
    public IGroupManager Groups => null!;

    private class NoOpHubClients : IHubClients
    {
        public IClientProxy All => new NoOpClientProxy();

        public IClientProxy AllExcept(IReadOnlyList<string> excludedConnectionIds) =>
            new NoOpClientProxy();

        public IClientProxy Client(string connectionId) => new NoOpClientProxy();

        public IClientProxy Clients(IReadOnlyList<string> connectionIds) => new NoOpClientProxy();

        public IClientProxy Group(string groupName) => new NoOpClientProxy();

        public IClientProxy GroupExcept(
            string groupName,
            IReadOnlyList<string> excludedConnectionIds
        ) => new NoOpClientProxy();

        public IClientProxy Groups(IReadOnlyList<string> groupNames) => new NoOpClientProxy();

        public IClientProxy User(string userId) => new NoOpClientProxy();

        public IClientProxy Users(IReadOnlyList<string> userIds) => new NoOpClientProxy();

        private class NoOpClientProxy : IClientProxy
        {
            public Task SendCoreAsync(
                string method,
                object?[] args,
                CancellationToken cancellationToken = default
            ) => Task.CompletedTask;
        }
    }
}
