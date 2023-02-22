using ApiCatalogoMinimals.ApiEndpoints;
using ApiCatalogoMinimals.Context;
using ApiCatalogoMinimals.Models;
using ApiCatalogoMinimals.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConection");

builder.Services.AddDbContext<AppDbContexxt>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddSingleton<ITokenService>(new TokenService());

builder.Services.AddAuthentication
                 (JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,

                         ValidIssuer = builder.Configuration["Jwt:Issuer"],
                         ValidAudience = builder.Configuration["Jwt:Audience"],
                         IssuerSigningKey = new SymmetricSecurityKey
                         (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                     };
                 });

builder.Services.AddAuthorization();

builder.Services.AddCors();

var app = builder.Build();

//Definir os Endpoints


app.MapAutenticacaoEndpoints();


//"/" -> raiz do projeto
//Excludefrom description usado para o m�todo N�O aparecer mais na interface do sawagger
app.MapGet("/", () => "Cat�logo de Produtos 2023").ExcludeFromDescription();


app.MapCategoriasEndpoints();

app.MapProdutosEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(p =>
{
    //define a pol�tica
    //estamos habilitando o middleware do cors
    p.AllowAnyOrigin();
    p.WithMethods("GET");
    p.AllowAnyHeader();
});

//precisa disso para a autentica��o JWT funcionar
//ATEN��O: primeiro a autentica��o, depois a autoriza��o
// se inverter, tb n�o funciona
app.UseAuthentication();
app.UseAuthorization();


app.Run();

