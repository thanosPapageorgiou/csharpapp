using CSharpApp.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

// Register Services
builder.Services.AddProjectServices();
builder.Services.AddProjectHttpClients(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSession();
app.UseHttpsRedirection();
//app.UseRequestPerformanceLogging();

// Register Endpoints
app.MapProjectEndpoints();

app.Run();