using CommandLine;

namespace Grepz;

public class Options {
    [Option ('i', "ignore-case", Required = false, HelpText = "ignore case distinctions")]
    public bool IgnoreCase { get; set; }

    [Option ('n', "line-number", Required = false, HelpText = "print line number with output lines")]
    public bool LineNumbers { get; set; }

    [Value(1, Required = true, HelpText = "Text pattern")]
    public string Pattern { get; set; }
}
