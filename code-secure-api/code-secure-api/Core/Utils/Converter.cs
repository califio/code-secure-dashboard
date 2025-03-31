using System.Text.RegularExpressions;

namespace CodeSecure.Core.Utils;

public static class Converter
{
    public static string MarkdownToJira(string input)
    {
        // Placeholder for extracted code blocks
        const string start = "J2MBLOCKPLACEHOLDER";
        var replacementsList = new List<KeyValuePair<string, string>>();
        int counter = 0;

        // Convert code blocks ``` ... ```
        input = Regex.Replace(input, @"`{3,}(\w+)?((?:\n|.)+?)`{3,}", match =>
        {
            string synt = match.Groups[1].Value;
            string content = match.Groups[2].Value;

            string code = "{code";
            if (!string.IsNullOrEmpty(synt))
            {
                code += ":" + synt;
            }
            code += "}" + content + "{code}";

            string key = start + (counter++) + "%%";
            replacementsList.Add(new KeyValuePair<string, string>(key, code));
            return key;
        });

        // Convert Setext-style headings
        input = Regex.Replace(input, @"^(.*?)\n([=-])+$", match =>
        {
            string content = match.Groups[1].Value;
            string level = match.Groups[2].Value;
            return "h" + (level[0] == '=' ? 1 : 2) + ". " + content;
        }, RegexOptions.Multiline);

        // Convert ATX-style headings (### Heading)
        input = Regex.Replace(input, @"^([#]+)(.*?)$", match =>
        {
            string level = match.Groups[1].Value;
            string content = match.Groups[2].Value;
            return "h" + level.Length + "." + content;
        }, RegexOptions.Multiline);

        // Convert bold and italic text
        input = Regex.Replace(input, @"([*_]+)(.*?)\1", match =>
        {
            string wrapper = match.Groups[1].Value;
            string content = match.Groups[2].Value;
            string to = wrapper.Length == 1 ? "_" : "*";
            return to + content + to;
        });

        // Convert bullet lists (multi-level)
        input = Regex.Replace(input, @"^(\s*)- (.*)$", match =>
        {
            string level = match.Groups[1].Value;
            string content = match.Groups[2].Value;
            int len = 2;
            if (level.Length > 0)
            {
                len = (level.Length / 4) + 2;
            }
            return new string('-', len) + " " + content;
        }, RegexOptions.Multiline);

        // HTML tag replacements
        var htmlTagMap = new Dictionary<string, string>
        {
            { "cite", "??" },
            { "del", "-" },
            { "ins", "+" },
            { "sup", "^" },
            { "sub", "~" }
        };

        input = Regex.Replace(input, @"<(" + string.Join("|", htmlTagMap.Keys) + @")>(.*?)<\/\1>", match =>
        {
            string from = match.Groups[1].Value;
            string content = match.Groups[2].Value;
            string to = htmlTagMap[from];
            return to + content + to;
        });

        // Convert strikethrough ~~text~~ -> -text-
        input = Regex.Replace(input, @"~~(.*?)~~", "-$1-");

        // Convert images: ![](url) -> !url!
        input = Regex.Replace(input, @"!\[\]\((.*?)\)", "!$1!");

        // Convert inline code: `code` -> {{code}}
        input = Regex.Replace(input, @"`([^`]+)`", "{{$1}}");

        // Convert links: [text](url) -> [text|url]
        input = Regex.Replace(input, @"$begin:math:display$(.*?)$end:math:display$$begin:math:text$(.*?)$end:math:text$", "[$1|$2]");

        // Convert angle bracket links: <url> -> [url]
        input = Regex.Replace(input, @"<([^>]+)>", "[$1]");

        // Restore extracted sections
        foreach (var replacement in replacementsList)
        {
            input = input.Replace(replacement.Key, replacement.Value);
        }

        // Convert table header rows
        var lines = input.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        var modifiedLines = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            if (Regex.IsMatch(line, @"\|---"))
            {
                modifiedLines[modifiedLines.Count - 1] = modifiedLines[^1].Replace("|", "||");
                continue; // Skip current line
            }

            modifiedLines.Add(line);
        }

        // Join modified lines back
        return string.Join("\n", modifiedLines);
    }
}