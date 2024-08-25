﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tasko.Web.Models;
using Tasko.Web.Service.IService;

namespace Tasko.Web.Controllers;

public class ProductController(IProductService productService) : Controller
{
    public async Task<IActionResult> ProductIndex()
    {
        List<ProductDto>? list = new();

        ResponseDto? response = await productService.GetAllProductsAsync();

        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
        }
        else
        {
            TempData["error"] = response?.Message;
        }

        return View(list);
    }

    public IActionResult ProductCreate()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ProductCreate(ProductDto model)
    {
        if (ModelState.IsValid)
        {
            ResponseDto? response = await productService.CreateProductsAsync(model);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product created successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
        }
        return View(model);
    }

    public async Task<IActionResult> ProductDelete(int productId)
    {
        ResponseDto? response = await productService.GetProductByIdAsync(productId);

        if (response != null && response.IsSuccess)
        {
            ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            return View(model);
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ProductDelete(ProductDto productDto)
    {
        ResponseDto? response = await productService.DeleteProductsAsync(productDto.ProductId);

        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction(nameof(ProductIndex));
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return View(productDto);
    }

    public async Task<IActionResult> ProductEdit(int productId)
    {
        ResponseDto? response = await productService.GetProductByIdAsync(productId);

        if (response != null && response.IsSuccess)
        {
            ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            return View(model);
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ProductEdit(ProductDto productDto)
    {
        if (ModelState.IsValid)
        {
            ResponseDto? response = await productService.UpdateProductsAsync(productDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product updated successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
        }
        return View(productDto);
    }
}
