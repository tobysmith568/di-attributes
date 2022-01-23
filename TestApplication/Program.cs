namespace DiAttributes;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.RegisterDiAttributes(builder.Configuration);

        var app = builder.Build();

        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}