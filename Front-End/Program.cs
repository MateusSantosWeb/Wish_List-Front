using WishListWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona suporte a MVC (Controllers e Views)
builder.Services.AddControllersWithViews();

// Registra o serviço que faz chamadas para a API
builder.Services.AddHttpClient<ApiServices>(client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5214/";
    client.BaseAddress = new Uri(baseUrl);
});

// Configura o armazenamento de Sessão (Onde guardaremos o Token JWT)
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Auth/Login");
}

app.UseStaticFiles();
app.UseRouting();

// A sessão DEVE vir antes da autenticação e autorização
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Rota padrão ajustada para Auth/Login já que não existe HomeController
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
