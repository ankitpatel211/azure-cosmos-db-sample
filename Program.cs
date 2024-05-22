using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
IConfiguration config = builder.Build();

CosmosClientOptions options = new()
{
    ApplicationPreferredRegions = new List<string>() { "WestUS", "EastUS" },
    ConnectionMode = ConnectionMode.Direct,
    ConsistencyLevel = ConsistencyLevel.Eventual
};
var endpoint = config["cosmosConnection:endpoint"];
var key = config["cosmosConnection:key"];
CosmosClient client = new(endpoint, key, options);
AccountProperties account = await client.ReadAccountAsync();
Console.WriteLine($"Cosmos Account Name:\t{account.Id}");
Console.WriteLine($"Cosmos Primary Region:\t{account.WritableRegions.FirstOrDefault()?.Name}");
var databaseId = config["cosmosConnection:database:id"];
Database database = await client.CreateDatabaseIfNotExistsAsync(databaseId);
Console.WriteLine($"Cosmos Database:\t{database.Id}");
var containerId = config["cosmosConnection:database:containers:firstContainer:id"];
var partitionKey = config["cosmosConnection:database:containers:firstContainer:partitionKey"];
var throughputValue = config["cosmosConnection:database:containers:firstContainer:throughput"];
Container container = await database.CreateContainerIfNotExistsAsync(id: containerId,
    partitionKeyPath: "/" + partitionKey + "", throughput: Convert.ToInt32(throughputValue));
Console.WriteLine($"Cosmos Container:\t{container.Id}");