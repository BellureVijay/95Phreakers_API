using _95PhrEAKer.Domain.EmailSetting;
using _95PhrEAKer.Domain.User;
using _95PhrEAKer.Persistence.context;
using _95PhrEAKer.Services.IServices.EmailServices;
using _95PhrEAKer.Services.IServices.UserPostService;
using _95PhrEAKer.Services.IServices.UserServices;
using _95PhrEAKer.Services.ServicesExtension.ChatServices;
using _95PhrEAKer.Services.ServicesExtension.EmailServices;
using _95PhrEAKer.Services.ServicesExtension.UserPostService;
using _95PhrEAKer.Services.ServicesExtension.UserServices;
using _95PhrEAKer_webapi.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
var secretKey = builder.Configuration.GetValue<string>("secreteKey");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey ?? string.Empty)),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };

});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 28)), // Adjust MySQL version if needed
        mySqlOptions => mySqlOptions.MigrationsAssembly("95PhrEAKer.Persistence")
    )
);


builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("ResendSettings"));
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddApplicationInsightsTelemetry();
var app = builder.Build();

// Apply EF Core migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();  // This will apply pending migrations to your DB
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(option =>
{
    option.AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials()
          .WithOrigins(
              "http://localhost:3000",
              "https://95phreaker-nextjs-production.up.railway.app"
          );
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ChathubService>("/chathub");

app.Run();













