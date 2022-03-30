using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace SuggestionAppUI;

public static class RegisterServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var registerServices = builder.Services;
        
        registerServices.AddRazorPages();
        registerServices.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();
        registerServices.AddMemoryCache();
        registerServices.AddControllersWithViews().AddMicrosoftIdentityUI();

        registerServices.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

        registerServices.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
            {
                policy.RequireClaim("jobTitle", "Admin");
            });
        });

        registerServices.AddSingleton<IDbConnection, DbConnection>();
        registerServices.AddSingleton<ICategoryData, MongoCategoryData>();
        registerServices.AddSingleton<IStatusData, MongoStatusData>();
        registerServices.AddSingleton<ISuggestionData, MongoSuggestionData>();
        registerServices.AddSingleton<IUserData, MongoUserData>();
    }
}