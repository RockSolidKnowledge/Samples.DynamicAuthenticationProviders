using CustomProvider;

// Create the web application builder
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.ConfigureServices();

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
app.ConfigurePipeline();

// Run the application
await app.RunAsync();
