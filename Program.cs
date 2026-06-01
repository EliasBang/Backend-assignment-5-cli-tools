using Spectre.Console;
namespace Assignment_5_CLI_tools;

class Program
{

    // scripts:
    // dotnet run list/ls optional: (-p "path/to/directory)
    // dotnet run print-file/pf required: (-p "path/to/file)
    // dotnet run print-file-start/pfs required:(-p "path/to/file) (-l amount of lines: e.g. 9)

    static void Main(string[] args)
    {
        if (args == null)
        {
            AnsiConsole.MarkupLine($"[red]Input == null. Try again[/]");
            return;
        }

        string command = args[0];
        Dictionary<string, string> options = [];
        string temp = "";
        for (int i = 1; i < args.Length; i++)
        {
            if (args[i].StartsWith("-"))
            {
                temp = args[i]; // temporarily stores the -flag
                continue;
            }
            if (temp.StartsWith("-")) // if flag exists, add key:value (flag:input) to options Dictionary. 
            {
                options.Add(temp, args[i]);
                temp = ""; // Reset temp
            }
        }

        switch (command.ToLower())
        {
            case "list" or "ls":
                try { list(options["-path"]); }
                catch { }
                break;
            case "print-file" or "pf":
                printFile(options["-path"]);
                break;
            case "print-file-start" or "pfs":
                printFileStart(options["-path"], options["-l"]);
                break;
            case "print-file-end" or "pfe":
                printFileEnd(options["-path"], options["-l"]);
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

        static void printFileStart(string path, string sLines)
        {
            // Turn string into int
            if (!int.TryParse(sLines, out int nLines))
            {
                AnsiConsole.MarkupLine($"[red]X int.TryParse failed. Make sure -l value is a number");
                return;
            }

            // Checks that path exist
            if (path == "")
            {
                AnsiConsole.MarkupLine($"[red]X No path provided[/]");
                return;
            }

            path = Path.GetFullPath(path); //From my understanding this turns relative paths into absolute paths. I found it when googling how paths are usually handled

            try
            {
                using StreamReader reader = new(path);

                AnsiConsole.MarkupLine($"[blue]File content of {path}:[/]");

                string? line;
                int i = 0;
                while ((line = reader.ReadLine()) != null && i < nLines)
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
        ;

        static void printFileEnd(string path, string sLines)
        {
            // Turn string into int
            if (!int.TryParse(sLines, out int nLines))
            {
                AnsiConsole.MarkupLine($"[red]X int.TryParse failed. Make sure -l value is a number");
                return;
            }

            // Checks that path exist
            if (path == "")
            {
                AnsiConsole.MarkupLine($"[red]X No path provided[/]");
                return;
            }

            path = Path.GetFullPath(path); //From my understanding this turns relative paths into absolute paths. I found it when googling how paths are usually handled

            try
            {
                using StreamReader reader = new(path);

                AnsiConsole.MarkupLine($"[blue]File content of {path}:[/]");

                string? line;
                int i = reader.length - nLines;
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
        ;
    }
}
