using StackExchange.Redis;

namespace MessangerBack.Utils;

public static class RedisProvider
{
    private static IDatabase redisDatabase;

    public static IDatabase Database
    {
        get => redisDatabase;
    }
    static RedisProvider()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var redisHost = configuration.GetSection("ConnectionStrings:RedisConnection").Value;
        var connection = ConnectionMultiplexer.Connect(redisHost);
        redisDatabase = connection.GetDatabase();
    }
}
