using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBook.Data;
using WebBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBook.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Google.Apis.Admin.Directory.directory_v1.Data;

namespace WebBook.Controllers
{
    public class CategoryController : Controller
    {

        private readonly DBContext _db;
        public CategoryController(DBContext db)
        {
            _db = db;
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            CategoryList viewModel = new CategoryList();
            viewModel.Categories = _db.Categories;

            var neki1 = _db.Products.ToList();
            var neki = _db.Products.Include(x => x.Category).ToList();

            //IEnumerable<Category> objCategoryList = _db.Categories;
            
            return View(viewModel);
        }

        // GET: CategoriesTEMP/Details/5
        [Authorize(Roles = "test")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        //get
        public IActionResult Create()
        {
            return View();
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                
                ModelState.AddModelError("Name", "Name and display order cannot match!");

            }
            Exists(obj);

            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created succesfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return View();
            }
            var categoryFromDb = _db.Categories.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u => u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);

        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Name and display order cannot match!");
            }
            ExistsEdit(obj);
            _db.ChangeTracker.Clear();
            if (ModelState.IsValid) 
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category edited succesfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return View();
            }
            var categoryFromDb = _db.Categories.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u => u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);

        }
        //post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var categoryFromDb = _db.Categories.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u => u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(categoryFromDb);
            _db.SaveChanges(); 
            TempData["success"] = "Category deleted succesfully";
            return RedirectToAction("Index");
            
        }


        public bool Exists(Category obj)
        {
            IEnumerable<Category> objCategoryList = _db.Categories;
            bool exist = false;
            foreach (var item in objCategoryList)
            {
                if (obj.Name == item.Name)
                {
                    ModelState.AddModelError("Name", "Name allready exists!");
                    exist = true;
                }
            }
            return exist;
        }
        public bool ExistsEdit(Category obj)
        {
            IEnumerable<Category> objCategoryList = _db.Categories;
            bool exist = false;
            foreach (var item in objCategoryList)
            {
                if (obj.Name == item.Name && obj.Id != item.Id)
                {
                    ModelState.AddModelError("Name", "Name allready exists!");
                    exist = true;
                }
            }
            return exist;
        }
    }
}
