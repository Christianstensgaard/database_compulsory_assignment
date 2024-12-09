using System.Security.Cryptography.X509Certificates;
using StackExchange.Redis;
namespace SimpleCSApplication.Controllers;
public class Redis{
  public static Redis I {get; private set;} = new Redis();

  const string ConnectionString = "redis:6379";

  Redis(){
    var redis = ConnectionMultiplexer.Connect(ConnectionString);
    database = redis.GetDatabase();
  }

  public IDatabase database {get; private set;}

}