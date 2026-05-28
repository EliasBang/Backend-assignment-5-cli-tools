using Spectre.Console;
namespace Assignment_5_CLI_tools;

class Program
{
    static void Main()
    {
        bool isRunning = true;
        while (isRunning)
        {
            listener();
        }

        static void listener()
        {
            AnsiConsole.MarkupLine($"[blue]Command options are:[/]");
            AnsiConsole.MarkupLine($"[green]->[/] List (path optional).");
            string? input = Console.ReadLine();

            if (input == null)
            {
                AnsiConsole.MarkupLine($"[red]Input == null. Try again[/]");
                return;
            }

            string[] inputSplit = input.Split(" ");
            string command = inputSplit[0];
            string option = "";
            if (inputSplit.Length > 1) { option = inputSplit[1]; }

            switch (command.ToLower())
            {
                case "list" or "ls":
                    list(option);
                    break;
                case "print-file" or "pf":
                    printFile(option);
                    break;
                default:
                    AnsiConsole.MarkupLine($"[red]X Input did not match any commands. Try again[/]\n");
                    break;
            }
        }
        ;

        static string pathHandler(string path)
        {
            // Defaults to current directory if no path is specified
            if (path == "")
                path = Directory.GetCurrentDirectory();
            else
            {
                // Returns the absolute path so that it's possible to use relative paths.
                path = Path.GetFullPath(path);
            }
            return path;
        }

        static void list(string path)
        {
            path = pathHandler(path);

            AnsiConsole.MarkupLine($"[blue]Listing files and folder in {path}:[/]");

            // Gets the full paths to all the files and subfolders
            try
            {
                string[] result = Directory.GetFileSystemEntries(path);
                foreach (string entryFullPath in result)
                {
                    string entryPathEnd = entryFullPath.Split("/")[^1];

                    AnsiConsole.MarkupLine($"[green]->[/] {entryPathEnd}");
                }
                // Notifies about empty directory
                if (result.Length == 0)
                    AnsiConsole.MarkupLine($"[yellow]X[/] Empty directory");
            }
            catch
            {
                AnsiConsole.MarkupLine($"[red]X Could not find path[/]");
            }

            AnsiConsole.WriteLine("");
        }
        ;

        static void printFile(string path)
        {
            path = pathHandler(path);
        }
    }
}
