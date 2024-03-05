using Identity.Framework.Core.Extension;
using Shared.Extension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddBasicAuth(builder.Configuration);
builder.Services.AddMongoDbConfiguration(builder.Configuration);
builder.Services.AddFrameworkServices();
builder.Services.AddCustomMappers();
builder.Services.SaveOrUpdateIdentityApiUser();

var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseCustomExceptionHandling();
app.UseLocalHostIpFilter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
   .RequireAuthorization();

app.Run();
