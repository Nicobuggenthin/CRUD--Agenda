using Agenda.Managers;
using Agenda.Managers.Entidades;
using Agenda.Managers.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar la cadena de conexión usando inyección de dependencias
builder.Services.AddScoped<IContactoRepository, ContactoRepository>(sp =>
    new ContactoRepository(builder.Configuration["Db:ConnectionString"]));
builder.Services.AddScoped<IContactoManager, ContactoManager>();

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
    pattern: "{controller=Contactos}/{action=Index}/{id?}");

app.Run();
