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
        var connection = ConnectionMultiplexer.Connect("localhost:6379");
        redisDatabase = connection.GetDatabase();
    }
}
