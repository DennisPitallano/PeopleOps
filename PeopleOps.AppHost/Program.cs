var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.PeopleOps_ApiService>("apiservice");

builder.AddProject<Projects.PeopleOps_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.BlazorApp1>("blazorapp1");

builder.Build().Run();
