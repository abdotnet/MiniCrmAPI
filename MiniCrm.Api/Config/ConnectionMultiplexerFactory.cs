using StackExchange.Redis;

namespace MiniCrm.Api.Config
{
    public static class ConnectionMultiplexerFactory
    {
        private const string UseRedis = nameof(UseRedis);
        private const string Redis = nameof(Redis);

        public static async Task<IConnectionMultiplexer?> CreateConnection(IConfiguration configuration)
        {
            if (!configuration.GetValue<bool>(UseRedis)) return null;
            var connectionString = configuration.GetConnectionString(Redis) ?? string.Empty;
            return await ConnectionMultiplexer.ConnectAsync(connectionString);
        }
    }
}
