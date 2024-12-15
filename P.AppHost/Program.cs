var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.P_ApiService>("apiservice");

builder.AddProject<Projects.P_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.AddProject<Projects.P_HumanResource_Web>("p-humanresource-web");

builder.AddProject<Projects.ThankUApp>("thankuapp");

builder.Build().Run();
