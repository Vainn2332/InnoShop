using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductService.API.Middlewares;
using ProductService.ApplicationLayer;
using ProductService.ApplicationLayer.Interfaces;
using ProductService.CoreLayer;
using ProductService.CoreLayer.Static_Entities;
using ProductService.InfrastructureLayer;
using ProductService.InfrastructureLayer.DBContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
    {
        Name = "Authorization",//как я понял имя из какого header брать
        Description = "Enter Bearer Authorization:",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
        new OpenApiSecurityScheme
        {
            Reference=new OpenApiReference
            {
                Type=ReferenceType.SecurityScheme,
                Id="JWT"
            }
        },new string[]{ }
        }
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidAudience = AuthOptions.AUDIENCE,
        ValidateIssuer = true,
        ValidIssuer = AuthOptions.ISSUER,
        ValidateLifetime = true,
        //секретный ключ    
        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<ProductDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(ProductDBContext)))
    );
builder.Services.AddTransient<IProductRepository, ProductRepository>();

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IProductsService,ProductsService>();
builder.Services.AddTransient<IUserService,UserService>();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
