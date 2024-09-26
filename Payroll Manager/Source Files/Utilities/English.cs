using System.Linq;

namespace Utilities
{
public static class English
{
    public static string Sequence(this string[] values, string conjunction = "and")
    {
        if (values.Length == 0)
            return "";

        else if (values.Length == 1)
            return values[0];

        else if (values.Length == 2)
            return string.Join(" and ", values);

        else if (values.Length >= 3)
        {
            string firstPart = string.Join(", ", values[0..^1]);
            string secondPart = $", {conjunction} {values.Last()}";
            return firstPart + secondPart;
        }

        else
            throw new Exception();
    }

}
}