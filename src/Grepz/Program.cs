using System.Text.RegularExpressions;

using CommandLine;

using Grepz;

static string red (string input) => $"\u001b[31m{input}\u001b[0m";
static string green (string input) => $"\u001b[32m{input}\u001b[0m";
static string yellow (string input) => $"\u001b[33m{input}\u001b[0m";
static string blue (string input) => $"\u001b[34m{input}\u001b[0m";

var result = Parser.Default.ParseArguments<Options>(args)
    .WithParsed(options => {
        // Set up StreamReader for stdin
        using var inputStream = Console.OpenStandardInput ();
        using var reader = new StreamReader (inputStream);
        // Set up StreamWriter for stdout
        using var outputStream = Console.OpenStandardOutput ();
        using var writer = new StreamWriter (outputStream) {
            AutoFlush = true
        };
        // Define the pattern to search for (in this case "elit")
        // Use a regular expression to match the pattern
        var opts = RegexOptions.None;
        if(options.IgnoreCase)
            opts |= RegexOptions.IgnoreCase;

        Regex regex = new Regex(options.Pattern, opts);

        string line;
        int lineNum = 1;
        while ( ( line = reader.ReadLine () ) != null ) {
            var match = regex.Match (line);
            if(match.Success ) {
                // Replace matched "elit" with colored version using ANSI escape codes
                string highlightedLine = regex.Replace(line, match => yellow(match.Value));

                // Write the modified line to stdout
                if(options.LineNumbers) {
                    writer.Write($"{red(lineNum.ToString())}:");
                }

                if(options.OnlyMatching) {
                    writer.WriteLine(match.Value);
                } else {
                    writer.WriteLine (highlightedLine);

                }
            }
            lineNum++;
        }
    }) // options is an instance of Options type
    .WithNotParsed(errors => {
    }); // errors is a sequence of type IEnumerable<Error>
