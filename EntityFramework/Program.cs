using EntityFramework;

// Create the web application builder
var builder = WebApplication.CreateBuilder(args);

// Add services to the container and build the application
var app = builder.ConfigureServices();

// Configure the HTTP request pipeline
app.ConfigurePipeline();

// Run the application
await app.RunAsync();
