using System.Security.Cryptography;
using System.Text;

namespace Application;

public static class PasswordHandler
{
    public static string GetPasswordHash(string password) => Convert.ToBase64String(MD5.HashData(Encoding.UTF8.GetBytes(password)));
}