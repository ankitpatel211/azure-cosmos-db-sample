using Microsoft.Azure.Cosmos;
CosmosClientOptions options = new()
{
    ApplicationPreferredRegions = new List<string>(){ "WestUS", "EastUS" },
    ConnectionMode = ConnectionMode.Direct,
    ConsistencyLevel = ConsistencyLevel.Eventual
};
string endpoint = "<cosmos-endpoint>";
string key = "<cosmos-key>";
CosmosClient client = new (endpoint, key, options);
AccountProperties account = await client.ReadAccountAsync();
Console.WriteLine($"Cosmos Account Name:\t{account.Id}");
Console.WriteLine($"Cosmos Primary Region:\t{account.WritableRegions.FirstOrDefault()?.Name}");
string databaseId = "<cosmos-database>";
Database database = await client.CreateDatabaseIfNotExistsAsync(databaseId);
Console.WriteLine($"Cosmos Database:\t{database.Id}");
string containerId = "<cosmos-container>";
Container container = await database.CreateContainerIfNotExistsAsync(id:containerId,partitionKeyPath:"/<container-partition-key>",throughput:400);
Console.WriteLine($"Cosmos Container:\t{container.Id}");