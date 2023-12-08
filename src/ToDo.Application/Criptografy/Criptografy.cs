using System.Security.Cryptography;
using System.Text;

namespace ToDo.Application.Criptografy;

public static class Criptografy
{
    public static string GenerateHash(this string password)
    {
        
        var hash = SHA1.Create();
        var encoding = new ASCIIEncoding();
        var array = encoding.GetBytes(password);
        array = hash.ComputeHash(array);

        var strHexa = new StringBuilder();

        foreach (var item in array)
        {
            strHexa.Append(item.ToString("x2"));
        }

        return strHexa.ToString();
    }

    public static string DesgenerateHash(this string pass)
    {
        return pass;
    }
}