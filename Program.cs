using Spectre.Console;
namespace Assignment_5_CLI_tools;

class Program
{

    // scripts:
    // dotnet run list/ls
    // dotnet run print-file/pf

    static void Main(string[] args)
    {
        if (args == null)
        {
            AnsiConsole.MarkupLine($"[red]Input == null. Try again[/]");
            return;
        }

        string command = args[0];
        string option = "";
        if (args.Length > 1) { option = args[1]; }

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
            if (path == "")
            {
                AnsiConsole.MarkupLine($"[red]X No path provided[/]");
                return;
            }

            path = Path.GetFullPath(path);
            try
            {
                using StreamReader reader = new(path);

                AnsiConsole.MarkupLine($"[blue]File content of {path}:[/]");

                string? line;
                int i = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    i++;
                }
                // Notifies about empty or unreadable file
                if (i <= 1)
                    AnsiConsole.MarkupLine($"[yellow]X[/] Empty or unreadable file");
            }
            catch
            {
                AnsiConsole.MarkupLine($"[red]X Could not find path:[/] {path}");
            }

        }
    }
}
