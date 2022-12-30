
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Properties_to_Rent_API.Models;
using Properties_to_Rent_API.Services;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Extensions.AspNetCore.Configuration.Secrets;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.







builder.Services.AddControllers();


//AD 
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)    
//                 .AddJwtBearer(options =>
//                 {
//                     options.Audience = builder.Configuration["AzureAd:ResourceId"];
//                     options.Authority = $"{ builder.Configuration["AzureAd:Instance"]}{builder.Configuration["AzureAd:TenantId"]}";
//                 });

builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
                //.EnableTokenAcquisitionToCallDownstreamApi()
                //.AddInMemoryTokenCaches();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Vault
string kvURL = builder.Configuration["keyvault:KVUrl"];
string tenantId = builder.Configuration["keyvault:TenantId"];
string clientId = builder.Configuration["keyvault:ClientId"];
string clientSecret = builder.Configuration["keyvault:ClientSecret"];

var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
var kvclient = new SecretClient(new Uri(kvURL), credential);

builder.Configuration.AddAzureKeyVault(kvclient,new AzureKeyVaultConfigurationOptions());

var email = kvclient.GetSecret("email").Value.Value;


//Mongo DB
builder.Services.Configure<PropertiesDbSettings>(builder.Configuration.GetSection(nameof(PropertiesDbSettings)));
builder.Services.AddSingleton<IPropertiesDbSettings>(sp => sp.GetRequiredService<IOptions<PropertiesDbSettings>>().Value);
//string cnx = builder.Configuration.GetValue<string>("PropertiesDBSettings:ConnectionString");
//builder.Services.AddSingleton<IMongoClient>(s => new MongoClient(cnx));

builder.Services.AddScoped<IPropertyServices, PropertyServices>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
