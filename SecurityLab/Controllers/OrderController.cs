﻿using Microsoft.AspNetCore.Mvc;
using SecurityLab.Models;


namespace SecurityLab.Controllers
{

    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repoService,
                Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }

        public ViewResult Checkout() => View(new Purchase());

        [HttpPost]
        public IActionResult Checkout(Purchase order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("",
                    "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                //order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                cart.Clear();
                return RedirectToPage("/Completed",
                    new { orderId = order.OrderId });
            }
            else
            {
                return View();
            }
        }
    }
}