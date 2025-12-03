using MediCita.Web.Servicios.Contrato;
using MediCita.Web.Servicios.Implementacion;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar servicios al contenedor.
builder.Services.AddControllersWithViews();

// 2. Inyección de Dependencias (Tus servicios)
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IMedicamentoService, MedicamentoService>();
builder.Services.AddScoped<IVentaService, VentaService>(); // Si ya creaste este servicio
builder.Services.AddScoped<IEspecialidadService, EspecialidadService>();
builder.Services.AddScoped<ICitaService, CitaService>();

// 3. Configuración de Autenticación (Login)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Acceso/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

// 4. Configuración de Sesión (CARRITO) <--- ¡ESTO ES LO QUE FALTABA!
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Duración de la sesión
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// --- ZONA DEL MIDDLEWARE (Tubería de peticiones) ---

if (!app.Environment.IsDevelopment())
{
    // app.UseExceptionHandler("/Home/Error"); // Comentamos esto porque borramos la vista Error
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 5. Activar Autenticación y Autorización
app.UseAuthentication();
app.UseAuthorization();

// 6. Activar Sesión <--- Esto fallaba porque faltaba el paso 4 arriba
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();