using LettercaixaAPI.Models;
using LettercaixaAPI.Services.Implementations;
using LettercaixaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Scaffold-DbContext "Server=.\SQLExpress;Database=Lettercaixa;Trusted_Connection=True; TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force

builder.Services.AddControllers();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IProfileService, ProfileService>();
builder.Services.AddTransient<IFavoriteService, FavoriteService>();
builder.Services.AddSingleton<IFavoritesCollectionService, FavoritesCollectionService>();
builder.Services.AddDbContext<LettercaixaContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<LettercaixaDatabaseSettings>(builder.Configuration.GetSection("LettercaixaDatabase"));

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(auth =>
    {
        auth.RequireHttpsMetadata = false;
        auth.SaveToken = true;
        auth.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Settings.Key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Lettercaixa", policy => policy.WithOrigins("http://localhost:4200").AllowCredentials().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Lettercaixa");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
