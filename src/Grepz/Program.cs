using System.Text.RegularExpressions;

using CommandLine;

using Grepz;

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
                  int lineNum = 0;
                  while ( ( line = reader.ReadLine () ) != null ) {
                      lineNum++;

                      // Replace matched "elit" with colored version using ANSI escape codes
                      string highlightedLine = regex.Replace(line, match => $"\u001b[31m{match.Value}\u001b[0m");

                      // Write the modified line to stdout
                      if(options.OnlyMatching) {
                          if(regex.IsMatch(line)) {
                              writer.Write($"{lineNum}: ");
                              writer.WriteLine (highlightedLine);
                          } else {

                          }
                      }
                      else {
                          writer.WriteLine (highlightedLine);
                      }
                  }
              }) // options is an instance of Options type
              .WithNotParsed(errors => {
              }); // errors is a sequence of type IEnumerable<Error>
