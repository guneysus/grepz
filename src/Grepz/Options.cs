using CommandLine;

namespace Grepz;

public class Options {
    [Option ('i', "ignore-case", Required = false, HelpText = "ignore case distinctions")]
    public bool IgnoreCase { get; set; }

    [Value (1, Required = true, HelpText = "Text pattern")]
    public string Pattern { get; set; }


    [Option (shortName: 'o', longName: "only-matching", HelpText = "Print only the matched non-empty parts of matching lines.")]
    public bool OnlyMatching { get; set; }
}
