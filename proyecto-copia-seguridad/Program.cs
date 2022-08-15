using Microsoft.Extensions.Configuration;

// Lectrua de la configuración de la aplicación
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// Mensaje de inicio
Console.WriteLine("INICIANDO COPIADO DE ARCHVOS");

// Lectura de archivos en paths designados
string sourcePath = Path.Combine(Environment.CurrentDirectory, config.GetSection("Source").Get<string>());
DirectoryInfo sourceInfo = new DirectoryInfo(sourcePath);

string targetPath = Path.Combine(Environment.CurrentDirectory, config.GetSection("Target").Get<string>());
DirectoryInfo targetInfo = new DirectoryInfo(targetPath);

string[] extensions = config.GetSection("AllowedExtensiones").Get<string[]>();

void CopyFiles(DirectoryInfo source, DirectoryInfo target)
{
    Directory.CreateDirectory(target.FullName);

    var files = source.GetFiles()
        .Where(file => extensions.Contains(file.Name.ToLower().Substring(file.Name.LastIndexOf(".") + 1)))
        .ToList();

    foreach (var file in files)
        file.CopyTo(Path.Combine(target.FullName, file.Name), true);

    foreach (var sourceSubdirectory in source.GetDirectories())
    {
        var targetSubdirectory = target.CreateSubdirectory(sourceSubdirectory.Name);
        CopyFiles(sourceSubdirectory, targetSubdirectory);
    }
}

// Ejecución de la copia de archivos y mensaje de finalización
await Task.Run(() => CopyFiles(sourceInfo, targetInfo));
Console.WriteLine("COPIADO DE ARCHVOS EXITOSO");