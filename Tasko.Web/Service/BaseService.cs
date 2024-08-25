using System.Net;
using System.Text;
using System.Text.Json;
using Tasko.Web.Models;
using Tasko.Web.Service.IService;
using Tasko.Web.Utility;

namespace Tasko.Web.Service;

public class BaseService(IHttpClientFactory httpClientFactory) : IBaseService
{
    public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
    {
        try
        {
            HttpClient client = httpClientFactory.CreateClient("TaskoAPI");
            HttpRequestMessage message = new();

            message.Headers.Add("Accept",
                requestDto.ContentType == ContentType.MultipartFormData ? "*/*" : "application/json");

            message.RequestUri = new Uri(requestDto.Url);

            if (requestDto.ContentType == ContentType.MultipartFormData)
            {
                var content = new MultipartFormDataContent();

                foreach (var prop in requestDto.Data.GetType().GetProperties())
                {
                    var value = prop.GetValue(requestDto.Data);

                    if (value is FormFile file)
                    {
                        content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                    }
                    else
                    {
                        content.Add(new StringContent((value == null ? "" : value.ToString()) ?? string.Empty), prop.Name);
                    }
                }
                message.Content = content;
            }
            else
            {
                message.Content = new StringContent(JsonSerializer.Serialize(requestDto.Data), Encoding.UTF8, "application/json");
            }

            message.Method = requestDto.ApiType switch
            {
                ApiType.POST => HttpMethod.Post,
                ApiType.DELETE => HttpMethod.Delete,
                ApiType.PUT => HttpMethod.Put,
                _ => HttpMethod.Get
            };

            HttpResponseMessage? apiResponse = await client.SendAsync(message);

            switch (apiResponse.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new() { IsSuccess = false, Message = "Not Found" };
                case HttpStatusCode.Forbidden:
                    return new() { IsSuccess = false, Message = "Access Denied" };
                case HttpStatusCode.Unauthorized:
                    return new() { IsSuccess = false, Message = "Unauthorized" };
                case HttpStatusCode.InternalServerError:
                    return new() { IsSuccess = false, Message = "Internal Server Error" };
                default:
                    var apiContent = await apiResponse.Content.ReadAsStringAsync();
                    var apiResponseDto = JsonSerializer.Deserialize<ResponseDto>(apiContent);
                    return apiResponseDto;
            }
        }
        catch (Exception ex)
        {
            var dto = new ResponseDto
            {
                Message = ex.Message,
                IsSuccess = false
            };
            return dto;
        }
    }
}
