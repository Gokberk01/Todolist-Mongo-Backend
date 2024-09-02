using System.Text;
using api.Data;
using api.repositories;
using api.services;
using api.shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var key = Encoding.ASCII.GetBytes("this is a super rare and very secure key");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddCors(); 

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<IUserService, UserService>();

//Dependency Injection
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtService>();


builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => 
{
    return new MongoClient(builder.Configuration.GetConnectionString("MongoDbConnection"));
});

builder.Services.AddSingleton(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    var databaseName = builder.Configuration["MongoDB:DatabaseName"];
    return mongoClient.GetDatabase(databaseName);
});


builder.Host.UseSerilog((context, loggerConfiguration) =>
{
loggerConfiguration
.ReadFrom.Configuration(context.Configuration)
.Enrich.FromLogContext()
.Enrich.WithMachineName()
.Enrich.WithEnvironmentName()
.WriteTo.Console();
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options => options //3000 -> REACT , 8080 -> VIEW , 4200 -> ANGULAR We can pick only one
    .WithOrigins("http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials() //This is the option that we need in order to send cookies in the 
    //front end if we dont add this option the front end wont get those cookies
);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.Run();

