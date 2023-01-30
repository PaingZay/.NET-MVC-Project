using Microsoft.AspNetCore.Mvc;
using ASPSession.Connectors;
using ASPSession.Models;
using ASPSession.Models.ViewModels;

namespace ASPSession.Controllers;

[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
public class CartController : Controller
{
    private readonly EFConnector _context;

    public CartController(EFConnector context)
    {
        _context = context;
    }

    public async Task<IActionResult> Add(int id)
    {
        Product product = await _context.Product.FindAsync(id);

        List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

        CartItem cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();

        if (cartItem == null)
        {
            cart.Add(new CartItem(product));
        }
        else
        {
            cartItem.Quantity += 1;
        }

        HttpContext.Session.SetJson("Cart", cart);

        TempData["Success"] = "The product has been added!";

        return Redirect(Request.Headers["Referer"].ToString());
    }

    public async Task<IActionResult> Decrease(int id)
    {
        List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

        CartItem cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();

        if (cartItem.Quantity > 1)
        {
            --cartItem.Quantity;
        }
        else
        {
            cart.RemoveAll(p => p.ProductId == id);
        }

        if (cart.Count == 0)
        {
            HttpContext.Session.Remove("Cart");
        }
        else
        {
            HttpContext.Session.SetJson("Cart", cart);
        }

        TempData["Success"] = "The product has been removed!";

        return RedirectToAction("Cart","Home");
    }

    public async Task<IActionResult> Remove(long id)
    {
        List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

        cart.RemoveAll(p => p.ProductId == id);

        if (cart.Count == 0)
        {
            HttpContext.Session.Remove("Cart");
        }
        else
        {
            HttpContext.Session.SetJson("Cart", cart);
        }

        TempData["Success"] = "The product has been removed!";

        return RedirectToAction("Cart", "Home");
    }

    public IActionResult Clear()
    {
        HttpContext.Session.Remove("Cart");

        return RedirectToAction("Cart", "Home");
    }
}
