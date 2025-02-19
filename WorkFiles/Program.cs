// See https://aka.ms/new-console-template for more information
using System.IO;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

Console.WriteLine("Hello, World!");
string directoryPath = @"C:\Otus\TestDir";
try
{
    for (int i = 1; i <= 2; i++)
    {
        string newDirectorypath = directoryPath + i;
        string filePath = newDirectorypath + @"\File№";

        //1. Проверка / создание директории
        CheckAndCreateDirectory(newDirectorypath);

        //2. Проверка / создание файлов в директории
        CheckAndCreateFiles(filePath);

        //4. Дозапись файла 
        WriteTextToFile(filePath, DateTime.Now.ToString("F"));

        //5. Чтение файла
        foreach(var item in Directory.GetFiles(directoryPath + i))
        {
            (string fileName, string fileContext) = ReadFromFile(item);
            Console.WriteLine($"Чтение файла: {fileName}\n" + fileContext + "\n");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine("Произошла критическая ошибка! \nОписание ошибки: " + ex.Message);
}

static void CheckAndCreateDirectory(string directoryPath)
{
    //Проверяем, есть ли папка 
    if (!Directory.Exists(directoryPath))
        Directory.CreateDirectory(directoryPath);
}

static void CheckAndCreateFiles(string filePath)
{
    //Внутри каждой папки создаём 10 файлов
    for (int j = 1; j <= 10; j++)
    {
        string newfilePath = filePath.Replace("№", j.ToString()) + ".txt";

        //Проверяем, есть ли нужный нам файл
        if (!File.Exists(newfilePath))
        {
            using (File.Create(newfilePath));

            //3. Запись в файл
            File.AppendAllText(newfilePath, $"File{j}.txt \n");
        }
    }
}

static void WriteTextToFile(string filePath, string text)
{
    //Внутри каждой папки создаём 10 файлов
    for (int j = 1; j <= 10; j++)
    {
        string newfilePath = filePath.Replace("№", j.ToString()) + ".txt";
        
        if (File.Exists(newfilePath))
            File.AppendAllText(newfilePath, text + "\n");
    }
}

static (string, string) ReadFromFile(string filePath)
{
    string context = "";
    string fileName = "";
    if (File.Exists(filePath))
    {
        context = File.ReadAllText(filePath);
        FileInfo fileInfo = new FileInfo(filePath);
        fileName = fileInfo.Name;
    }

    return (fileName, context);
}
