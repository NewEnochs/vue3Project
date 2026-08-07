using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ProjectCore.Util;

namespace ProjectCore.Web.Middlewares
{
    public class EncryptFilterController
    {
        private readonly RequestDelegate _next;

        public EncryptFilterController(RequestDelegate next)
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

            await EncryptResponseBodyAsync(context);
        }

        private static bool ShouldSkip(HttpContext context)
        {
            var path = context.Request.Path;

            return path.StartsWithSegments("/swagger")
                   || HttpMethods.IsOptions(context.Request.Method);
        }

        private async Task EncryptResponseBodyAsync(HttpContext context)
        {
            var originalBody = context.Response.Body;
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body = originalBody;
            responseBody.Position = 0;

            var responseText = await new StreamReader(responseBody, Encoding.UTF8).ReadToEndAsync();
            if (!ShouldEncryptResponse(context))
            {
                await context.Response.WriteAsync(responseText);
                return;
            }

            var apiResponse = CreateApiResponse(context.Response.StatusCode, responseText);
            var jsonResponse = JsonSerializer.Serialize(apiResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.ContentLength = Encoding.UTF8.GetByteCount(jsonResponse);
            await context.Response.WriteAsync(jsonResponse);
        }

        private static ApiResponse CreateApiResponse(int statusCode, string responseText)
        {
            var success = statusCode >= StatusCodes.Status200OK
                          && statusCode < StatusCodes.Status300MultipleChoices;
            var data = success ? ReadResponseData(responseText) : string.Empty;
            var message = success ? "请求成功" : ReadErrorMessage(responseText);

            return new ApiResponse
            {
                State = success,
                Message = message,
                Data = success ? EncryptData(responseText) : string.Empty,
                ICount = GetCount(data),
                Code = statusCode,
                Success = success
            };
        }

        private static string EncryptData(string responseText)
        {
            if (string.IsNullOrWhiteSpace(responseText))
            {
                return string.Empty;
            }

            var camelCaseJson = ConvertJsonPropertyNamesToCamelCase(responseText);
            return FilterAES.FileterEncrypt(camelCaseJson) ?? string.Empty;
        }

        private static string ConvertJsonPropertyNamesToCamelCase(string json)
        {
            try
            {
                using var document = JsonDocument.Parse(json);
                using var stream = new MemoryStream();
                using (var writer = new Utf8JsonWriter(stream))
                {
                    WriteCamelCaseJsonElement(writer, document.RootElement);
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }
            catch (JsonException)
            {
                return json;
            }
        }

        private static void WriteCamelCaseJsonElement(Utf8JsonWriter writer, JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    writer.WriteStartObject();
                    foreach (var property in element.EnumerateObject())
                    {
                        writer.WritePropertyName(JsonNamingPolicy.CamelCase.ConvertName(property.Name));
                        WriteCamelCaseJsonElement(writer, property.Value);
                    }
                    writer.WriteEndObject();
                    break;
                case JsonValueKind.Array:
                    writer.WriteStartArray();
                    foreach (var item in element.EnumerateArray())
                    {
                        WriteCamelCaseJsonElement(writer, item);
                    }
                    writer.WriteEndArray();
                    break;
                default:
                    element.WriteTo(writer);
                    break;
            }
        }

        private static object ReadResponseData(string responseText)
        {
            if (string.IsNullOrWhiteSpace(responseText))
            {
                return string.Empty;
            }

            try
            {
                return JsonSerializer.Deserialize<object>(responseText);
            }
            catch (JsonException)
            {
                return responseText;
            }
        }

        private static string ReadErrorMessage(string responseText)
        {
            if (string.IsNullOrWhiteSpace(responseText))
            {
                return "请求失败";
            }

            try
            {
                var data = JsonSerializer.Deserialize<object>(responseText);
                return data?.ToString() ?? "请求失败";
            }
            catch (JsonException)
            {
                return responseText;
            }
        }

        private static int GetCount(object data)
        {
            if (data is not JsonElement element)
            {
                return 0;
            }

            if (element.ValueKind == JsonValueKind.Array)
            {
                return element.GetArrayLength();
            }

            if (element.ValueKind != JsonValueKind.Object)
            {
                return 0;
            }

            foreach (var propertyName in new[] { "totalCount", "TotalCount", "count", "Count" })
            {
                if (element.TryGetProperty(propertyName, out var countElement)
                    && countElement.ValueKind == JsonValueKind.Number
                    && countElement.TryGetInt32(out var count))
                {
                    return count;
                }
            }

            return 0;
        }

        private static bool ShouldEncryptResponse(HttpContext context)
        {
            var statusCode = context.Response.StatusCode;
            if (statusCode == StatusCodes.Status204NoContent || statusCode == StatusCodes.Status304NotModified)
            {
                return false;
            }

            var contentType = context.Response.ContentType;
            return string.IsNullOrWhiteSpace(contentType)
                   || contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase)
                   || contentType.Contains("text/plain", StringComparison.OrdinalIgnoreCase);
        }
    }

    public class ApiResponse
    {
        public bool State { get; set; }
        public string Message { get; set; } = string.Empty;
        public object Data { get; set; } = string.Empty;
        [JsonPropertyName("icount")]
        public int ICount { get; set; }
        public int Code { get; set; }
        public bool Success { get; set; }
    }
}
