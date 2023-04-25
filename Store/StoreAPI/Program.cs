using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreAPI.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region authentication

builder.Services.Configure<ForwardedHeadersOptions>(option =>

option.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor

| Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto);



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>

{

    options.RequireHttpsMetadata = false;

    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters()

    {

        ValidateIssuer = true,

        ValidateAudience = true,

        ValidateLifetime = true,

        ValidateIssuerSigningKey = true,

        ValidAudience = builder.Configuration["Jwt:Audience"],

        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))

    };

});

#endregion





builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the SQL connection
builder.Services.AddDbContext<StoreContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("StoreDatabase")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
