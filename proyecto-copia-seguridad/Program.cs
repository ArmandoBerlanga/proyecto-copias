// Mensaje de inicio
Console.WriteLine("Inicio de programa");

// fuente de donde vienen los archivos
string fuente = @"C:\Users\rauly\OneDrive\Escritorio\Source";
DirectoryInfo fuenteDir = new DirectoryInfo(fuente);

// destino de donde vienen los archivos
string destino = @"C:\Users\rauly\OneDrive\Escritorio\Ejemplo";
DirectoryInfo destinoDir = new DirectoryInfo(destino);

//Extensiones validas
string[] extensions = new string[2] {"json","config"};

try
{
    await Task.Run(() => Archivos(fuenteDir, destinoDir));
    Console.WriteLine("Todo salio bien");
}catch(Exception ex)
{
    Console.WriteLine("Se produjo el siguiente error: "+ex.ToString());
}


void Archivos(DirectoryInfo fuenteDir, DirectoryInfo destinoDir)
{
    Directory.CreateDirectory(destinoDir.FullName);

    var documentos = fuenteDir.GetFiles().Where(value => extensions.Contains(value.Name.ToLower().Substring(value.Name.LastIndexOf(".") + 1))).ToList();

    foreach (var documento in documentos)
        documento.CopyTo(Path.Combine(destinoDir.FullName, documento.Name), true);

    foreach (var fuente in fuenteDir.GetDirectories())
    {
        var targetSubdirectory = destinoDir.CreateSubdirectory(fuente.Name);
        Archivos(fuente, targetSubdirectory);
    }
}
