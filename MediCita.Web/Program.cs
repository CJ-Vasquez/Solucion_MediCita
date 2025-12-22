using MediCita.Web.Servicios.Contrato;
using MediCita.Web.Servicios.Implementacion;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// 1. SERVICIOS MVC
// ========================================
builder.Services.AddControllersWithViews();

// ========================================
// 2. INYECCIÓN DE DEPENDENCIAS (Servicios)
// ========================================
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IMedicamentoService, MedicamentoService>();
builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<IEspecialidadService, EspecialidadService>();
builder.Services.AddScoped<ICitaService, CitaService>();
builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<IRolService, RolService>();

// ========================================
// 3. AUTENTICACIÓN POR COOKIES (Login)
// ========================================
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Login";
        options.AccessDeniedPath = "/Acceso/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Duración del login
        options.SlidingExpiration = true; // Renueva automáticamente
    });

// ========================================
// 4. CONFIGURACIÓN DE SESIÓN (Carrito)
// ========================================
builder.Services.AddDistributedMemoryCache(); // Requerido para sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2); // Duración del carrito
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".MediCita.Session";
});

// ========================================
// 5. ACCESO A HttpContext (Para leer sesión en vistas)
// ========================================
builder.Services.AddHttpContextAccessor();

// ========================================
// CONSTRUIR LA APLICACIÓN
// ========================================
var app = builder.Build();

// ========================================
// CONFIGURACIÓN DEL PIPELINE (Middleware)
// ========================================

// Manejo de errores
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); // Errores detallados en desarrollo
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // CSS, JS, imágenes

app.UseRouting();

// ⚠️ ORDEN CRÍTICO: Autenticación → Autorización → Sesión
app.UseAuthentication(); // 1. Login/Cookies
app.UseAuthorization();  // 2. Roles ([Authorize])
app.UseSession();        // 3. Carrito/Sesión

// ========================================
// RUTAS
// ========================================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ========================================
// INICIAR APLICACIÓN
// ========================================
app.Run();
