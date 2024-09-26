namespace Utilities
{
public static class Parsing
{
    public static string[] ParseCommands(string str)
    {
        List<string> arguments = [];
        string word = "";
        bool quoted = false;
        str = str.Trim() + " ";
        for (int i = 0; i < str.Length; i++)
        {
            if (!quoted)
            {
                if (str[i] == '"')
                    quoted = true;

                else if (str[i] != ' ') 
                    word += str[i];
                
                else 
                {
                    arguments.Add(word);
                    word = "";
                }
            } else
            {
                if (str[i] == '"')
                    quoted = false;
                else
                    word += str[i];
            }
        }
        return [.. arguments];
    }
}
}