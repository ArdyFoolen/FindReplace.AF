using FindReplace.AF;

if (args == null || args.Length == 0 || args.Any(a => a.Contains("-h") || a.Contains("/h") || a.Contains("/?")))
{
    helpInfo();
    return;
}

string find;
string replace = string.Empty;
List<string> subArgs = args.SkipWhile(s => !"-find".Equals(s.ToLowerInvariant())).ToList();
if (subArgs == null || subArgs.Count <= 1 || string.IsNullOrWhiteSpace(subArgs[1]))
{
    helpInfo();
    return;
}
find = subArgs[1];

subArgs = args.SkipWhile(s => !"-replace".Equals(s.ToLowerInvariant())).ToList();
if (subArgs != null && subArgs.Count > 1)
{
    if (string.IsNullOrWhiteSpace(subArgs[1]))
    {
        helpInfo();
        return;
    }
    replace = subArgs[1];
}

FileActions actions = new FileActions();
subArgs = args.SkipWhile(s => !"-path".Equals(s.ToLowerInvariant())).ToList();
if (subArgs != null && subArgs.Count > 1)
{
    if (!Directory.Exists(subArgs[1]))
    {
        helpInfo();
        return;
    }
    actions.RootDirectoryPath = subArgs[1];
}

subArgs = args.SkipWhile(s => !"-incl".Equals(s.ToLowerInvariant())).ToList();
if (subArgs != null && subArgs.Count > 1)
{
    bool inclSubDirectories;
    if (!bool.TryParse(subArgs[1], out inclSubDirectories))
    {
        helpInfo();
        return;
    }
    actions.IncludingSubdirectories = inclSubDirectories;
}

subArgs = args.SkipWhile(s => !"-case".Equals(s.ToLowerInvariant())).ToList();
if (subArgs != null && subArgs.Count > 1)
{
    bool caseSensitive;
    if (!bool.TryParse(subArgs[1], out caseSensitive))
    {
        helpInfo();
        return;
    }
    actions.CaseSensitive = caseSensitive;
}

subArgs = args.SkipWhile(s => !"-pat".Equals(s.ToLowerInvariant())).ToList();
if (subArgs != null && subArgs.Count > 1)
{
    actions.FilePattern = subArgs[1];
}

if (string.IsNullOrWhiteSpace(replace))
{
    Console.WriteLine("Find {0} in path {1}", find, actions.RootDirectoryPath);
    actions.Find(find).ForEach(f => Console.WriteLine(f));
}
else
{
    Console.WriteLine("Replace {0} with {1} in path {2}", find, replace, actions.RootDirectoryPath);
    actions.Replace(find, replace).ForEach(f => Console.WriteLine(f));
}

static void helpInfo()
{
    Console.WriteLine("Help info:");
    Console.WriteLine("  Command: ConsoleTest [Options [Param to option]]");
    Console.WriteLine("  Where:");
    Console.WriteLine("    - No options or -h or /h or /?: This help info");
    Console.WriteLine("    - -find {search text}: Find the search text");
    Console.WriteLine("    - -replace {replace text}: Replace found text with replace text");
    Console.WriteLine("    - -path {directory path}: The directory path to search and/or replace text in. Default is current directory");
    Console.WriteLine("    - -incl {including sub directories}: true or false, default is false");
    Console.WriteLine("    - -case {case sensitive}: true or false, default is false");
    Console.WriteLine("    - -pat {file pattern}: * zero or more characters, ? zero or one character.");
}
