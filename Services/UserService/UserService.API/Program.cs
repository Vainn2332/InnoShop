using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserService.API.Middlewares;
using UserService.ApplicationLayer;
using UserService.ApplicationLayer.Interfaces;
using UserService.CoreLayer;
using UserService.CoreLayer.Static_Entities;
using UserService.InfrastrucureLayer;
using UserService.InfrastrucureLayer.DBcontext;

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
builder.Services.AddAuthentication()
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

builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IPasswordService, PasswordService>();
builder.Services.AddTransient<IUsersService,UsersService>();

builder.Services.AddDbContext<UserDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(UserDBContext)))
    );
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
