using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.Persistence.Repositories;
using ProductService.Infrastructure.Persistence;
using FluentValidation;
using ProductService.Application.Features.Products.Commands.CreateProduct;
using ProductService.Application.Mappings;
using ProductService.Api.Middleware;
using Serilog;

// --- Configuraci�n Inicial de Serilog (Bootstrap Logger) ---
// Importante: Hacer esto ANTES de crear el WebApplicationBuilder
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    // Puedes a�adir configuraci�n m�nima aqu� o leerla despu�s desde appsettings
    .CreateBootstrapLogger();

Log.Information("Iniciando ProductService API...");

try // Envolver todo en un try-catch para loguear errores fatales de inicio
{
    var builder = WebApplication.CreateBuilder(args);

    // --- Integrar Serilog con el Host de ASP.NET Core ---
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration) // Lee config de appsettings.json (Logging:Serilog section)
        .ReadFrom.Services(services) // Permite inyecci�n en Sinks (avanzado)
        .Enrich.FromLogContext()
        .WriteTo.Console()); // Configura Console Sink aqu� o en appsettings

    // --- A�adir Servicios al Contenedor (DI) ---

    // 1. Configuraci�n de EF Core y Repositorio
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ProductDbContext>(options =>
        options.UseSqlServer(connectionString,
             b => b.MigrationsAssembly(typeof(ProductDbContext).Assembly.FullName))); // Especifica ensamblado de migraciones
    builder.Services.AddScoped<IProductRepository, ProductRepository>();

    // 2. Configuraci�n de MediatR
    // Escanea el ensamblado que contiene CreateProductCommandHandler (u otro tipo de Application)
    // para encontrar todos los Handlers IRequestHandler<,>
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateProductCommandHandler>());
    // Opcional: A�adir Pipeline Behaviors (para logging, validaci�n autom�tica, etc.)
    // builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    // builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    // 3. Configuraci�n de AutoMapper
    // Escanea el ensamblado que contiene ProductProfile (u otro tipo de Application)
    // para encontrar todos los perfiles que heredan de Profile
    builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);

    // 4. Configuraci�n de FluentValidation
    // Escanea el ensamblado que contiene CreateProductCommandValidator (u otro tipo de Application)
    // para encontrar todos los validadores que heredan de AbstractValidator<>
    builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
    // Opcional: Integraci�n autom�tica con ASP.NET Core para validar modelos en Actions
    // builder.Services.AddFluentValidationAutoValidation(); // Requiere paquete FluentValidation.AspNetCore

    // 5. Configuraci�n b�sica de API (Controladores, Swagger)
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        // Opcional: Personalizar Swagger
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Version = "v1",
            Title = "Product Service API",
            Description = "API para gestionar productos",
        });
        // Podr�as a�adir configuraci�n para comentarios XML, seguridad (JWT), etc.
    });


    // --- Construir la Aplicaci�n ---
    var app = builder.Build();

    // --- Configurar el Pipeline de Peticiones HTTP ---

    // Middleware de Manejo Global de Excepciones (�Ponerlo muy temprano!)
    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

    // Configuraci�n para Desarrollo (Swagger)
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
            // Opcional: Cargar swagger en la ra�z
            // options.RoutePrefix = string.Empty;
        });

        // Opcional: Aplicar migraciones autom�ticamente al inicio en desarrollo
        // using (var scope = app.Services.CreateScope())
        // {
        //     var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
        //     dbContext.Database.Migrate();
        // }
    }

    // Middleware de Logging de Peticiones de Serilog
    app.UseSerilogRequestLogging();

    // Redirecci�n HTTPS (Est�ndar)
    app.UseHttpsRedirection();

    // Enrutamiento (Necesario para que funcionen los controllers)
    app.UseRouting();

    // Autorizaci�n (si la tuvieras configurada)
    // app.UseAuthentication(); // Si usas autenticaci�n
    app.UseAuthorization();

    // Mapeo de Controladores (Ejecuta las acciones de los controllers)
    app.MapControllers();

    // Ejecutar la aplicaci�n
    app.Run();

}
catch (Exception ex) // Capturar errores fatales del inicio
{
    Log.Fatal(ex, "La aplicaci�n ProductService API fall� al iniciar.");
}
finally
{
    // Asegurarse de que todos los logs se escriban antes de cerrar
    Log.CloseAndFlush();
}