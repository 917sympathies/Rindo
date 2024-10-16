using System.Security.Cryptography;
using System.Text;

namespace Rindo.Infrastructure;

public static class PasswordHandler
{
    public static string GetPasswordHash(string password) => Convert.ToBase64String(MD5.HashData(Encoding.UTF8.GetBytes(password)));
}