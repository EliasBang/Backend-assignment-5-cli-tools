using Spectre.Console;
namespace Assignment_5_CLI_tools;

class Program
{
    static void Main(string[] args)
    {
        if (args == null)
        {
            AnsiConsole.MarkupLine($"[red]Input == null. Try again[/]");
            return;
        }

        // Handles the inputs, including command and various options.
        // This is done by storing -flags and values as key:value pairs in a dictionary.
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
        // Activates different functions based on input command
        switch (command.ToLower())
        {
            case "list" or "ls":
                try { list(options["-path"]); }
                catch { list(null); }
                break;
            case "print-file" or "pf":
                try { printFile(options["-path"]); }
                catch { AnsiConsole.MarkupLine($"[red]X No -path provided. Try again[/]\n"); }
                break;
            case "print-file-start" or "pfs":
                try { printFileStart(options["-path"], options["-l"]); }
                catch { AnsiConsole.MarkupLine($"[red]X No -path/-l provided. Try again[/]\n"); }
                break;
            case "print-file-end" or "pfe":
                try { printFileEnd(options["-path"], options["-l"]); }
                catch { AnsiConsole.MarkupLine($"[red]X No -path/-l provided. Try again[/]\n"); }
                break;
            case "create" or "c":
                try { create(options["-path"], options["-n"]); }
                catch { create(null, options["-n"]); }
                break;
            case "current-directory" or "cdir":
                AnsiConsole.MarkupLine($"[blue]Current directory: {Directory.GetCurrentDirectory()}[/]");
                break;
            default:
                AnsiConsole.MarkupLine($"[red]X Input did not match any commands. Try again[/]\n");
                break;
        }
        ;

        static string pathHandler(string? path)
        {
            // Defaults to current directory if no path is specified
            if (path == null)
                path = Directory.GetCurrentDirectory();
            else
            {
                // Returns the absolute path so that it's possible to use relative paths.
                path = Path.GetFullPath(path);
            }
            return path;
        }

        static void list(string? path)
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
                if (i == 0)
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

            path = Path.GetFullPath(path); //From my understanding this turns relative paths into absolute paths. I found it when googling how paths are usually handled

            try
            {
                AnsiConsole.MarkupLine($"[blue]First {sLines} lines of {path}:[/]");

                List<string> startLines = [.. File.ReadLines(path).Take(nLines)];

                foreach (string line in startLines)
                {
                    Console.WriteLine(line);
                }
                // Notifies about empty or unreadable file
                if (startLines.Count <= 0)
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

            path = Path.GetFullPath(path); //From my understanding this turns relative paths into absolute paths. I found it when googling how paths are usually handled

            try
            {
                using StreamReader reader = new(path);

                AnsiConsole.MarkupLine($"[blue]File content of {path}:[/]");

                List<string> endLines = [.. File.ReadLines(path).TakeLast(nLines)];
                foreach (string line in endLines)
                {
                    Console.WriteLine(line);
                }
                // Notifies about empty or unreadable file
                if (endLines.Count <= 1)
                    AnsiConsole.MarkupLine($"[yellow]X[/] Empty or unreadable file");
            }
            catch
            {
                AnsiConsole.MarkupLine($"[red]X Could not find path:[/] {path}");
            }
        }
        ;
        static void create(string? path, string name)
        {
            path = pathHandler(path);
            string completePath = Path.Join(path, name);
            try
            {
                if (Path.HasExtension(name))
                {
                    File.Create(completePath);
                    AnsiConsole.MarkupLine($"[green]File '{name}' successfully created in:\n{completePath}[/]");

                }
                else if (!Path.HasExtension(name))
                {
                    Directory.CreateDirectory(completePath);
                    AnsiConsole.MarkupLine($"[green]Folder '{name}' successfully created in:\n{completePath}[/]");

                }
            }
            catch
            {
                AnsiConsole.MarkupLine($"[red]X Could not find path[/]");
            }
        }
        ;
    }
}
