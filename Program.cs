using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            ShowProcesse();
            Console.WriteLine("Выберите действие: ");
            Console.WriteLine("1. Завершить процесс по ID");
            Console.WriteLine("2. Запустить новый процесс");
            Console.WriteLine("3. Выход");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    EndProcess();
                    break;
                case "2":
                    StartProcess();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }
    static void ShowProcesse()
    {
        Console.WriteLine("Список запущенных процессов:");
        foreach (var process in Process.GetProcesses())
        {
            try
            {
                Console.WriteLine($"Имя: {process.ProcessName}, ID: {process.Id},Памть: {(process.WorkingSet64 / 1024 / 1024)} MB, Статус: {process.Responding}");
            }
            catch
            {
                // Игнорируем процессы, к которым нет доступа
            }
        }

    }
    static void EndProcess()
    {
        Console.WriteLine("Введите ID процесса для завершения: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                var process = Process.GetProcessById(id);
                process.Kill();
                LogAction($"Завершен процесс: {process.ProcessName} (ID:{id})");
                Console.WriteLine("Процесс завершен.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        else 
        {
        Console.WriteLine("Неверный ID.");
        }
        Console.ReadKey();
    }
    static void StartProcess()
    {
        Console.WriteLine("Введите имя исполняемого файла(например notepad.exe):");
        string processName = Console.ReadLine();

        try
        {
            Process.Start(processName);
            LogAction($"Запущен процесс: {processName}");
            Console.WriteLine("Процесс запущен.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        Console.ReadKey();
    }
    static void LogAction(string message)
    {
        string logFilePath = "process_log.txt";
        using (StreamWriter writer = new StreamWriter(logFilePath))
        {
            writer.WriteLine($"{DateTime.Now}:{message}");
        }
    }
}