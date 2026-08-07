using System.Text;
using System.Text.Json;
using ProjectCore.Util;

namespace ProjectCore.Web.Middlewares
{
    public class RequestFilterController
    {
        private readonly RequestDelegate _next;

        public RequestFilterController(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (ShouldSkip(context))
            {
                await _next(context);
                return;
            }

            await DecryptRequestBodyAsync(context);
            await _next(context);
        }

        private static bool ShouldSkip(HttpContext context)
        {
            var path = context.Request.Path;

            return path.StartsWithSegments("/swagger")
                   || HttpMethods.IsOptions(context.Request.Method);
        }

        private static async Task DecryptRequestBodyAsync(HttpContext context)
        {
            var request = context.Request;

            if (request.ContentLength is null or 0 || !request.Body.CanRead)
            {
                return;
            }

            request.EnableBuffering();

            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var encryptedBody = await reader.ReadToEndAsync();
            request.Body.Position = 0;

            if (string.IsNullOrWhiteSpace(encryptedBody))
            {
                return;
            }

            var encryptedText = ReadEncryptedText(encryptedBody);
            var decryptedBody = DecryptOrOriginal(encryptedText, encryptedBody);

            var requestBodyBytes = Encoding.UTF8.GetBytes(decryptedBody);
            request.Body = new MemoryStream(requestBodyBytes);
            request.ContentLength = requestBodyBytes.Length;
            request.ContentType = "application/json; charset=utf-8";
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <param name="originalBody"></param>
        /// <returns></returns>
        private static string DecryptOrOriginal(string encryptedText, string originalBody)
        {
            try
            {
                var decryptedBody = FilterAES.FileterDecrypt(encryptedText);
                return string.IsNullOrWhiteSpace(decryptedBody) ? originalBody : decryptedBody;
            }
            catch
            {
                return originalBody;
            }
        }
        private static string ReadEncryptedText(string body)
        {
            var text = body.Trim();

            try
            {
                return JsonSerializer.Deserialize<string>(text) ?? text;
            }
            catch (JsonException)
            {
                return text;
            }
        }
    }
}
