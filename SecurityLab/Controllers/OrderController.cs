﻿using Microsoft.AspNetCore.Mvc;
using SecurityLab.Models;
using System.Linq;

namespace SecurityLab.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repoService, Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }

        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                // Convert CartLine[] to ICollection<CartLine>
                order.Lines = (ICollection<CartLine>)cart.Lines.ToList(); // Use ToList() to convert to ICollection<CartLine>
                repository.SaveOrder(order);
                cart.Clear();
                return RedirectToPage("/Completed", new { orderId = order.TransactionId });
            }
            else
            {
                return View(order);
            }
        }
    }
}


