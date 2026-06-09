### This README file was created using the create command.

# Commands:

## List all folders and files in a directory:

- dotnet run list/ls optional: (-p "path/to/directory)

## Print the contents of a file to terminal

- dotnet run print-file/pf required: (-p "path/to/file)

## Print certain numbers of lines of a file, starting from the top.

// dotnet run print-file-start/pfs required:(-p "path/to/file) (-l amount of lines: e.g. 9)

## Print certain numbers of lines of a file, starting from the bottom.

// dotnet run print-file-end/pfe required:(-p "path/to/file) (-l amount of lines: e.g. 9)

## Create a folder if no .extension, create a file if the name has an .extension

// dotnet run create/c optional: (-p "path/to/file) required: (-n name. If you add a file extension like .txt it will be a file. If you don't it will make a new folder)

## Tells you the current directory.

// dotnet run current-directory/cdir
