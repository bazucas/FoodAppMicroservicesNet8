using Tasko.Web.Models;
using Tasko.Web.Service.IService;
using Tasko.Web.Utility;

namespace Tasko.Web.Service;

public class ProductService(IBaseService baseService) : IProductService
{
    public async Task<ResponseDto?> CreateProductsAsync(ProductDto productDto)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = productDto,
            Url = SD.ProductAPIBase + "/api/product",
            ContentType = SD.ContentType.MultipartFormData
        });
    }

    public async Task<ResponseDto?> DeleteProductsAsync(int id)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.DELETE,
            Url = SD.ProductAPIBase + "/api/product/" + id
        });
    }

    public async Task<ResponseDto?> GetAllProductsAsync()
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.ProductAPIBase + "/api/product"
        });
    }

    public async Task<ResponseDto?> GetProductByIdAsync(int id)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.ProductAPIBase + "/api/product/" + id
        });
    }

    public async Task<ResponseDto?> UpdateProductsAsync(ProductDto productDto)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.PUT,
            Data = productDto,
            Url = SD.ProductAPIBase + "/api/product",
            ContentType = SD.ContentType.MultipartFormData
        });
    }
}
