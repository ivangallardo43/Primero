using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PruebaOctubre;
using PruebaOctubre.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

string key = "1usafaHJGF7Gaf7gffgIGFCHGFG7fgdet5asfdaedfI=";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<pruebaContext>();
builder.Services.AddTransient<Prestador>();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer(opt =>
{
    var signinkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)); 
    var SigningCredentials = new SigningCredentials(signinkey, SecurityAlgorithms.HmacSha256Signature);

    opt.RequireHttpsMetadata = false;

    opt.TokenValidationParameters = new TokenValidationParameters()
    {

        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = signinkey,


    };

});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");

//app.MapGet("/protected", () => "Pagina principal").RequireAuthorization();
app.MapGet("/protected", (ClaimsPrincipal user) => user.Identity?.Name).RequireAuthorization(); 


app.MapGet("/protectedwithscope", (ClaimsPrincipal user) => user.Identity?.Name)
    .RequireAuthorization(p => p.RequireClaim("scope", "myapi:admin"));

app.MapGet("/auth/{user}/{pass}", (string user, string pass) =>
{
    if (user == "administrador" && pass == "123456")
    {
        var tokenHander = new JwtSecurityTokenHandler();
        var bytekey = Encoding.UTF8.GetBytes(key);
        var tokenDes = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user),
            }),
            Expires = DateTime.UtcNow.AddMonths(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(bytekey), SecurityAlgorithms.HmacSha256Signature)

        };
        var token = tokenHander.CreateToken(tokenDes);

        return tokenHander.WriteToken(token);

    }
    else
    {
        return "usuario invalido";
    }
}
);

app.MapGet("/Proveedores", () =>

{
    using (var context = new pruebaContext())
    {
        return context.Proveedores.ToList();
    }
} );

app.MapGet("/provedorinject", async (pruebaContext context) =>
    await context.Proveedores.ToListAsync()
    );

app.MapGet("/prestador", (Prestador prestador) => prestador.Hi());

app.MapGet("/provedor/{id}", async (int id, pruebaContext context) =>
{
    var proveedor = await context.Proveedores.FindAsync(id);
    return proveedor != null ? Results.Ok(proveedor) : Results.NotFound();
});

app.MapPost("/post", (Usuario usuario) => $"{usuario.Id} {usuario.Name}");

app.Run();
