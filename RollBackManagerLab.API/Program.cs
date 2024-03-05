using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared.Contants;
using Shared.Extension;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterApiAndClients(builder.Configuration);
builder.Services.AddCarter();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = AppSecureConstants.JwtIssuer,
        ValidAudience = AppSecureConstants.JwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSecureConstants.JwtSecret)),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowOrigin", builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});


var app = builder.Build();

app.UseCustomExceptionHandling();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowOrigin");
app.MapCarter();

app.Run();

