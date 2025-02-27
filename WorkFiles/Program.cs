// See https://aka.ms/new-console-template for more information
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

Console.WriteLine("Hello, World!");
string directoryPath = @"C:\Otus\TestDir";

// Замечания:
// + по п. 1 директории требуется создавать именно через класс DirectoryInfo, вы создаете через - Directory - Исправлено
// + Создание файла, как и запись в него лучше делать в using, вы его используете только при создании файла.
// + Запись в файл должна происходить в кодировке UTF8, вы этого нигде явно не указываете. - Исправлено
// + После создания файла и перед записью в него, было бы хорошо проверять у него права на запись, через - AccessRule, проверить на AccessControlType.Allow
// + Чтение из файла так же стоит делать в using.

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
        foreach (var item in Directory.GetFiles(directoryPath + i))
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
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
        directoryInfo.Create();
    }
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
            File.Create(newfilePath).Close();

            //Проверка правил, можно ли считать
            if (CheckRules(newfilePath))
            {
                File.AppendAllText(newfilePath, $"File{j}.txt \n", Encoding.UTF8);
            }
        }
    }
}

static bool CheckRules(string filePath)
{        
    bool result = false;

    FileInfo fileInfo = new FileInfo(filePath);
    FileSecurity fileSecurity = fileInfo.GetAccessControl();
    AuthorizationRuleCollection rules = fileSecurity.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));

    foreach (FileSystemAccessRule rule in rules)
    {
        // Проверяем, есть ли правило Allow для записи
        if (rule.AccessControlType == AccessControlType.Allow &&
            (rule.FileSystemRights & FileSystemRights.Write) == FileSystemRights.Write)
        {
            result = true;
            break;
        }
    }
    return result;
}

static void WriteTextToFile(string filePath, string text)
{
    //Внутри каждой папки создаём 10 файлов
    for (int j = 1; j <= 10; j++)
    {
        string newfilePath = filePath.Replace("№", j.ToString()) + ".txt";

        if (File.Exists(newfilePath))
        {
            //Проверка правил, можно ли считать
            if (CheckRules(newfilePath))
            {
                File.AppendAllText(newfilePath, text + "\n", Encoding.UTF8);
            }
        }
    }
}

static (string, string) ReadFromFile(string filePath)
{
    string context = "";
    string fileName = "";
    if (File.Exists(filePath))
    {
        using (File.OpenRead(filePath))
        {
            context = File.ReadAllText(filePath);
            FileInfo fileInfo = new FileInfo(filePath);
            fileName = fileInfo.Name;
        }
    }
    return (fileName, context);
}
