using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBook.Data;
using WebBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBook.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace WebBook.Controllers
{
    public class ProductController : Controller
    {
        private readonly DBContext _db;
        public ProductController(DBContext db)
        {
            _db = db;
        }
        // GET: ProductController
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            //IEnumerable<Product> ProductList = _db.Products;//.Include(x => x.Category);
            //return View(ProductList);
            ProductList viewModel = new ProductList();
            viewModel.Products = _db.Products.Include(x => x.Category);
            return View(viewModel);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var products = _db.Products
                .Include(x => x.Category)
                .FirstOrDefault(x => x.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_db.Categories, "Id", "Name");
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product obj)
        {
            //ProductList viewModel = new ProductList();
            //viewModel.Products = _db.Products.Include(x => x.Category);
            ViewData["CategoryId"] = new SelectList(_db.Categories, "Id", "Name", obj.CategoryId);

            if (ProductExists(obj))
            {
                TempData["error"] = "Product allready exsists";
                return RedirectToAction("Create");
            }
            _db.Products.Add(obj);
            _db.SaveChanges();

            return RedirectToAction("Index");

        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewData["CategoryId"] = new SelectList(_db.Categories, "Id", "Name");
            if (id == null || id == 0)
            {
                return View();
            }
            var ProductFromDb = _db.Products.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u => u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product obj)
        {
            ViewData["CategoryId"] = new SelectList(_db.Categories, "Id", "Name",obj.CategoryId);

            if (ProductExists(obj))
            {
                TempData["error"] = "Product allready exsists";
                return RedirectToAction("Edit");
            }

            _db.ChangeTracker.Clear();

            _db.Products.Update(obj);
            _db.SaveChanges();
            TempData["success"] = "Category edited succesfully";
            return RedirectToAction("Index");

        }

        // GET: ProductController/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return View();
            }
            var productdb = _db.Products
               .Include(s => s.Category)
               .FirstOrDefault(m => m.ProductId == id);

            if (productdb == null)
            {
                return NotFound();
            }
            return View(productdb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProducts(int? id)
        {
            var product = _db.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }
            _db.Products.Remove(product);
            _db.SaveChanges();
            TempData["success"] = "Product deleted succesfully";
            return RedirectToAction("Index");
        }

        public bool ProductExists(Product obj)
        {
            IEnumerable<Product> stocklist = _db.Products;
            bool exists = false;
            foreach (var item in stocklist)
            {
                if (obj.ProductName == item.ProductName && obj.CategoryId == item.CategoryId && obj.ProductId != item.ProductId)
                {
                    exists = true;
                }
            }
            return exists;
        }
    }
}
