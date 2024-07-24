using System.Security.Cryptography;
using System.Text;

namespace WPG_HmacAuthentication;

public static class HmacClient
{
    public static string CreateHmac(string appId, string apiKey, string requestUri, string requestHttpMethod, DateTime requestDateTimeUtc, string? requestBody = null)
    {
        var requestTimeStampInSeconds = Helpers.CalculateUnixTimeInSeconds(requestDateTimeUtc);

        string nonce = Helpers.GenerateNonce();

        byte[]? body = null;
        if (!string.IsNullOrWhiteSpace(requestBody))
        {
            body = Helpers.ConvertStringToByteArray(requestBody);
        }

        string requestSignatureBase64String = CreateSignatureHash(
            appId,
            apiKey,
            requestUri,
            requestHttpMethod,
            nonce,
            (ulong)requestTimeStampInSeconds,
            body);

        return $"hmacauth {appId}:{requestSignatureBase64String}:{nonce}:{requestTimeStampInSeconds}";
    }

    #region :: Private Methods ::

    private static string CreateSignatureHash(
        string appId,
        string apiKey,
        string requestUri,
        string requestHttpMethod,
        string nonce,
        ulong requestTimeStampInSeconds,
        byte[]? requestBody = null)
    {
        string requestBodyBase64String = string.Empty;
        if (requestBody is not null)
        {
            byte[] requestBodyHash = Helpers.CalculateSha1(requestBody);
            requestBodyBase64String = Convert.ToBase64String(requestBodyHash);
        }

        string signatureRawData = $"{appId}{requestHttpMethod}{requestUri}{requestTimeStampInSeconds}{nonce}{requestBodyBase64String}";

        byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);

        byte[] secretKeyByteArray = Convert.FromBase64String(apiKey);

        using HMACSHA256 hmac = new(secretKeyByteArray);
        byte[] signatureBytes = hmac.ComputeHash(signature);

        return Convert.ToBase64String(signatureBytes);
    }

    #endregion :: Private Methods ::
}