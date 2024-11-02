var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.P_ApiService>("apiservice");

builder.AddProject<Projects.P_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
