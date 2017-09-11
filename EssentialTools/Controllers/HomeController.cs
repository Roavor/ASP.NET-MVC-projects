using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EssentialTools.Models;
using Ninject;

namespace EssentialTools.Controllers
{
    public class HomeController : Controller
    {

        private IValueCalculator calc;
        // GET: Home
        private Product[] products =
        {
            new Product { Name = "Каяк", Category = "Водные ", Price = 275M },
            new Product { Name = "Каяк", Category = "Водные виды спорта", Price = 48.95M },
            new Product { Name = "Каяк", Category = "Водные виды спорта", Price =  19.50M },
            new Product { Name = "Каяк", Category = "Водные ", Price = 34.95M }
        };
        public HomeController()
        {

        }
        public HomeController(IValueCalculator calcParam)
        {
            calc = calcParam;
        }
        public HomeController(IValueCalculator calcParam, IValueCalculator calc2)
        {
            calc = calcParam;
        }
        public ActionResult Index()
        {
            ShoppingCart cart = new ShoppingCart(calc);
            cart.Products = products;
            decimal totalValue = cart.CalculateProductTotal();
            return View(totalValue);
        }
    }
}