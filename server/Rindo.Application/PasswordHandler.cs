using System.Security.Cryptography;
using System.Text;

namespace Rindo.Infrastructure;

public static class PasswordHandler
{
    public static string GetPasswordHash(string password)
    {
        var md5 = MD5.Create();
        return Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }
}