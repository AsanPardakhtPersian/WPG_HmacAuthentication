using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace WPG_HmacAuthentication;

public static class Helpers
{
    public static string GenerateGuidToString()
    {
        return Guid.NewGuid().ToString("N");
    }

    public static long CalculateUnixTimeInSeconds(DateTimeOffset requestDateTimeUtc)
    {
        return requestDateTimeUtc.ToUnixTimeSeconds();
    }

    public static string GenerateNonce()
    {
        return GenerateGuidToString();
    }

    public static byte[] ConvertStringToByteArray(string input)
    {
        return Encoding.UTF8.GetBytes(input);
    }
    
    public static byte[] CalculateSha1(byte[] inputBytes)
    {
        using SHA1 sha1 = SHA1.Create();
        byte[] hashBytes = sha1.ComputeHash(inputBytes);

        return hashBytes;
    }

    public static string EncodeRequestUri(string requestUri)
    {
        return WebUtility.UrlEncode(requestUri);
    }
}