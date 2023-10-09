using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VebTechTestTask.AutoMapperConfig;
using VebTechTestTask.BLL.Services;
using VebTechTestTask.BLL.Services.Interfaces;
using VebTechTestTask.Configurations;
using VebTechTestTask.DAL;
using VebTechTestTask.DAL.UnitOfWork;
using VebTechTestTask.DAL.UnitOfWork.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Mesdfsdfsdfsfsdfsdfsdfsdfsdfsdfsdf"));
var jwtTokenConfiguration = new JwtTokenConfiguration
{
    Issuer = "Mesdfsdfsdfsfsdfsdfsdfsdfsdfsdfsdf",
    Audience = "Mesdfsdfsdfsfsdfsdfsdfsdfsdfsdfsdf",
    SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
    StartDate = DateTime.UtcNow,
    EndDate = DateTime.UtcNow.AddDays(60),
};

builder.Services.Configure<JwtTokenConfiguration>(config =>
{
    config.Audience = jwtTokenConfiguration.Audience;
    config.EndDate = jwtTokenConfiguration.EndDate;
    config.Issuer = jwtTokenConfiguration.Issuer;
    config.StartDate = jwtTokenConfiguration.StartDate;
    config.SigningCredentials = jwtTokenConfiguration.SigningCredentials;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtBearerOptions =>
{
    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtTokenConfiguration.Issuer,
        ValidAudience = jwtTokenConfiguration.Audience,
        IssuerSigningKey = signingKey
    };
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddDbContext<VebTechTestTaskDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddAutoMapper(typeof(MapperProfile));

var app = builder.Build();

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