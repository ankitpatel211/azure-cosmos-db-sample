#region Packages
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Configuration;
#endregion

#region Application Configuration
    //Build configuration using a Json configuration file
    var builder = new ConfigurationBuilder();
    builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true);
    var config = builder.Build();
#endregion

#region Cosmos Connection
    //Cosmos client options
    CosmosClientOptions options = new()
    {
        ApplicationPreferredRegions = new List<string>() { "WestUS", "EastUS" },
        ConnectionMode = ConnectionMode.Direct,
        ConsistencyLevel = ConsistencyLevel.Eventual
    };
    // Connection cofiguration
    var endpoint = config["cosmos:connection:endpoint"];
    var key = config["cosmos:connection:key"];
    CosmosClient client = new (endpoint, key, options);
    // Account properties
    AccountProperties account = await client.ReadAccountAsync();
    Console.WriteLine($"Cosmos Account Name:\t{account.Id}");
    Console.WriteLine($"Cosmos Primary Region:\t{account.WritableRegions.FirstOrDefault()?.Name}");
#endregion

#region Cosmos Database
    // Database configuration
    var databaseId = config["cosmos:database:id"];
    // Create a new database
    Database database = await client.CreateDatabaseIfNotExistsAsync(databaseId);
    Console.WriteLine($"Cosmos Database:\t{database.Id}");
#endregion

#region Cosmos Container
    // Container configuration
    var containerId = config["cosmos:database:container:id"];
    var partitionKey = config["cosmos:database:container:partitionKey"];
    var throughputValue = config["cosmos:database:container:throughput"];
    // Create a new container
    Container container = await database.CreateContainerIfNotExistsAsync(id: containerId, 
      partitionKeyPath: partitionKey, throughput: Convert.ToInt32(throughputValue));
    Console.WriteLine($"Cosmos Container:\t{container.Id}");
#endregion
