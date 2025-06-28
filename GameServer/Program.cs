using MagicOnion;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using GameServer.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GameServer.Repositories.Interfaces;
using GameServer.Repositories;
using GameServer.UseCases;

class Program
{
    static void Main(string[] args)
    {
        try{
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            Console.WriteLine("CreateBuilder");
            var builder = WebApplication.CreateBuilder(args);
            Console.WriteLine("ConfigureKestrel");
            builder.WebHost.ConfigureKestrel(options => {
                options.ListenAnyIP(5000, listenOptions => {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
            });
            //Console.WriteLine("AddGrpc");
            //builder.Services.AddGrpc();
            Console.WriteLine("AddMagicOnion");
            builder.Services.AddMagicOnion();

            // データベースの設定
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine("AddDbContext");
            // builder.Services.AddDbContext<AppDbContext>(options =>
            //     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 42))));

            // リポジトリの依存関係を登録
            builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
            builder.Services.AddScoped<IItemRepository, ItemRepository>();
            builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();

            // ユースケースの依存関係を登録
            builder.Services.AddScoped<CharacterUseCase>();
            builder.Services.AddScoped<AdminUseCase>();

            Console.WriteLine("builder.Build");
            builder.WebHost.UseUrls("http://0.0.0.0:5000");
            var app = builder.Build();
            Console.WriteLine("PrintRpcHandlers");
            PrintRpcHandlers(app);
            app.Use(async (context, next) =>
            {
                // リクエストのログ出力
                Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
                await next.Invoke();
                // レスポンスのログ出力
                Console.WriteLine($"Response: {context.Response.StatusCode}");
            });

            Console.WriteLine("app.MapMagicOnionService");
            app.MapMagicOnionService();
            Console.WriteLine("app.Urls.Add");
            app.Urls.Add("http://0.0.0.0:5000");
            Console.WriteLine("Server listening on HTTP/2 at port 5000");
            Console.WriteLine("app.Run");
            foreach (var url in app.Urls)
            {
                Console.WriteLine($"[Startup] ASP.NET Core is configured to listen on: {url}");
            }
            app.Run();
        } catch (Exception e) {
            Console.WriteLine("Exception:" + e.Message);
            Console.WriteLine("Exception:" + e.InnerException?.Message);
        }
    }
    static void PrintRpcHandlers(WebApplication app)
    {
        var assemblies = new[] { typeof(Program).Assembly };

        foreach (var assembly in assemblies)
        {
            var serviceTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => 
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IService<>)))
                .ToList();
            foreach (var service in serviceTypes)
            {
                Console.WriteLine($"[RPC] {service.FullName}");
            }
        }
    }
}