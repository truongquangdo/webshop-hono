using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System.Linq;
using WebShop.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebShop.Controllers
{
    public class SearchController : Controller
    {
        private readonly dbMarketsContext _context;

        public SearchController(dbMarketsContext context)
        {
            _context = context;
        }
        [Route("/tim-kiem", Name = ("ShopSearch"))]
        public IActionResult Search(int catId, string name, int? page)
        {
            try
            {
                IOrderedQueryable<Product> lsSanPham;
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 12;
                if (catId == 0)
                {
                    lsSanPham = _context.Products
                        .AsNoTracking()
                        .Include(p => p.Cat)
                        .Where(p => p.ProductName.Contains(name))
                        .OrderByDescending(x => x.DateCreated);
                }
                else
                {
                    lsSanPham = _context.Products
                        .AsNoTracking()
                        .Include(p => p.Cat)
                        .Where(p => p.ProductName.Contains(name) && p.CatId == catId)
                        .OrderByDescending(x => x.DateCreated);
                }
                PagedList<Product> models = new PagedList<Product>(lsSanPham, pageNumber, pageSize);

                ViewBag.CurrentPage = pageNumber;
                return View("~/Views/Product/Index.cshtml", models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
