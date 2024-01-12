var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "ClienteIndex",
    pattern: "{controller=Cliente}/{action=Index}",
    defaults: new {controller = "Cliente", action = "Index" });

app.MapControllerRoute(
    name: "FornecedorIndex",
    pattern: "{controller=Fornecedor}/{action=Index}",
    defaults: new {controller = "Fornecedor", action = "Index" });

app.MapControllerRoute(
    name: "LoginIndex",
    pattern: "{controller=Login}/{action=Index}",
    defaults: new {controller = "Login", action = "Index" });

app.MapControllerRoute(
    name: "Login",
    pattern: "{controller=Login}/{action=Login}",
    defaults: new {controller = "Login", action = "Login" });

app.MapControllerRoute(
    name: "LoginCadastrar",
    pattern: "{controller=Login}/{action=Cadastrar}",
    defaults: new {controller = "Login", action = "Cadastrar" });

app.MapControllerRoute(
    name: "LoginAlterar",
    pattern: "{controller=Login}/{action=Alterar}",
    defaults: new {controller = "Login", action = "Alterar" });

app.MapControllerRoute(
    name: "LoginJSONListagem",
    pattern: "{controller=Login}/{action=JSON}",
    defaults: new {controller = "Login", action = "JSON" });

app.MapControllerRoute(
    name: "EventosIndex",
    pattern: "{controller=Eventos}/{action=Index}",
    defaults: new { controller = "Eventos", action = "Index" });

app.MapControllerRoute(
    name: "EventosCadastrar",
    pattern: "{controller=Eventos}/{action=Cadastrar}",
    defaults: new { controller = "Eventos", action = "Cadastrar" });

app.MapControllerRoute(
    name: "EventosAlterar",
    pattern: "{controller=Eventos}/{action=Alterar}",
    defaults: new { controller = "Eventos", action = "Alterar" });

app.MapControllerRoute(
    name: "EventosAlterar",
    pattern: "{controller=Eventos}/{action=Deletar}",
    defaults: new { controller = "Eventos", action = "Deletar" });

app.MapControllerRoute(
    name: "EventosDetalhes",
    pattern: "{controller=Eventos}/{action=Detalhes}",
    defaults: new { controller = "Eventos", action = "Detalhes" });

app.MapControllerRoute(
    name: "EventosJSONListagem",
    pattern: "{controller=Eventos}/{action=JSON}",
    defaults: new { controller = "Eventos", action = "JSON" });

app.Run();