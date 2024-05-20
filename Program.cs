using Microsoft.Azure.Cosmos;
CosmosClientOptions options = new()
{
    ApplicationPreferredRegions = new List<string>(){ "WestUS", "EastUS" },
    ConnectionMode = ConnectionMode.Direct,
    ConsistencyLevel = ConsistencyLevel.Eventual
};
string endpoint = "";
string key = "";
CosmosClient client = new (endpoint, key, options);
AccountProperties account = await client.ReadAccountAsync();
Console.WriteLine($"Cosmos Account Name:\t{account.Id}");
Console.WriteLine($"Cosmos Primary Region:\t{account.WritableRegions.FirstOrDefault()?.Name}");
string databaseId = "cosmicworks";
Database database = await client.CreateDatabaseIfNotExistsAsync(databaseId);
Console.WriteLine($"Cosmos Database:\t{database.Id}");
string containerId = "task";
Container container = await database.CreateContainerIfNotExistsAsync(containerId,"/taskId",400);
Console.WriteLine($"Cosmos Container:\t{container.Id}");