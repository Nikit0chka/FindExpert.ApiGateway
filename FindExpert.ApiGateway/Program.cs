using System.Text;
using FindExpert.ApiGateway.GatewayConfig;
using FindExpert.ApiGateway.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

const string secretKey = "your-256-bit-secrett-secretttttt";
const string issuer = "http://localhost:5001";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(static options =>
    {
        options.TokenValidationParameters = new()
                                            {
                                                ValidateIssuerSigningKey = true,
                                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                                                ValidateLifetime = true,
                                                ValidateAudience = false,
                                                ValidateIssuer = true,
                                                ValidIssuer = issuer
                                            };
    });

builder.Services.AddReverseProxy()
    .LoadFromMemory(Configuration.Routes, Configuration.Clusters);

builder.Services.AddSwaggerGen(static swaggerGenOptions => { swaggerGenOptions.SwaggerDoc("aggregated", new() { Title = "Aggregated API", Version = "v1" }); });

builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(static swaggerUiOptions => { swaggerUiOptions.SwaggerEndpoint("/api/docs/aggregated", "Aggregated API"); });

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<AddHeadersMiddleware>();

app.MapControllers();
app.MapReverseProxy();

app.Run();