﻿using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using Tasko.Web.Models;
using Tasko.Web.Service.IService;

namespace Tasko.Web.Controllers;

public class HomeController(IProductService productService, ICartService cartService) : Controller
{
    public async Task<IActionResult> Index()
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

    [Authorize]
    public async Task<IActionResult> ProductDetails(int productId)
    {
        ProductDto? model = new();

        ResponseDto? response = await productService.GetProductByIdAsync(productId);

        if (response != null && response.IsSuccess)
        {
            model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
        }
        else
        {
            TempData["error"] = response?.Message;
        }

        return View(model);
    }


    [Authorize]
    [HttpPost]
    [ActionName("ProductDetails")]
    public async Task<IActionResult> ProductDetails(ProductDto productDto)
    {
        CartDto cartDto = new()
        {
            CartHeader = new CartHeaderDto
            {
                UserId = User.Claims.Where(u => u.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value
            }
        };

        CartDetailsDto cartDetails = new()
        {
            Count = productDto.Count,
            ProductId = productDto.ProductId,
        };

        List<CartDetailsDto> cartDetailsDtos = new() { cartDetails };
        cartDto.CartDetails = cartDetailsDtos;

        ResponseDto? response = await cartService.UpsertCartAsync(cartDto);

        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Item has been added to the Shopping Cart";
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["error"] = response?.Message;
        }

        return View(productDto);
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}