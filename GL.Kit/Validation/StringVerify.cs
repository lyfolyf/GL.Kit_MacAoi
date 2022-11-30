using System.Text.RegularExpressions;

namespace GL.Kit.Validation
{
    public static class StringVerify
    {
        public static bool IsInt(string s)
        {
            return Regex.IsMatch(s, "\\d+");
        }
    }
}
