using System.Text;
using ExpenseTrackerAPI;
using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure MongoDb Settings
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));

// Add MongoDb client as a singleton service
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Add MongoDB database as a singleton service
builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ExpenseService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddControllers();

var app = builder.Build();

SeedData(app.Services);

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void SeedData(IServiceProvider sp)
{
    var context = sp.GetRequiredService<MongoDbContext>();
    var categories = context.Categories;
    
    if(categories.Find(_ => true).Any()) return;
    
    var categoriesData = new List<Category>
    {
        new Category { Title = "Food" },
        new Category { Title = "Transport" },
        new Category { Title = "Entertainment" },
        new Category { Title = "Utilities" },
        new Category { Title = "Rent" },
        new Category { Title = "Miscellaneous" }
    };

    categories.InsertMany(categoriesData);
}