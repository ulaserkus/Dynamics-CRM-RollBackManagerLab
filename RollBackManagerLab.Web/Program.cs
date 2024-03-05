using Microsoft.AspNetCore.Authentication.Cookies;
using Shared.Extension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Admin/Private");
                    options.Conventions.AddPageRoute("/Home/Index", "");
                    options.RootDirectory = "/Pages";
                })
                .AddRazorRuntimeCompilation();

builder.Services.RegisterApiAndClients(builder.Configuration);
builder.Services.AddSmtpConfiguration(builder.Configuration);
builder.Services.AddSession();

builder.Services.Configure<CookiePolicyOptions>(opt =>
{
    opt.CheckConsentNeeded = context => true;
    opt.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(cookieOpt =>
{
    cookieOpt.LoginPath = "/Admin/SignIn";
    cookieOpt.ExpireTimeSpan = TimeSpan.FromDays(1);
});


// Configure the HTTP request pipeline.
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();

}
else
{
    app.UseDeveloperExceptionPage();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});
app.UseStatusCodePagesWithRedirects("/Error/{0}");
app.Run();
