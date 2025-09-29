var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "olá pessoal!");

//Para fazer login/senha
// clsse e propriedade para o login/senha estão em outro arquivo (LoginDTO.cs) na pasta DTOs, na pasta Dominio
app.MapPost("/login", (MinimalApi.DTOs.LoginDTO loginDTO) =>
{
    if (loginDTO.Email == "adm@teste.com" && loginDTO.Senha == "123456")

        return Results.Ok("Login com sucesso");
    else
        return Results.Unauthorized();
});


app.Run();


