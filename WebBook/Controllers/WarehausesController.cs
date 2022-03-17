#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBook.Data;
using WebBook.Models;
using WebBook.ViewModel;

namespace WebBook.Controllers
{
    public class WarehausesController : Controller
    {
        private readonly DBContext _db;

        public WarehausesController(DBContext obj)
        {
            _db = obj;
        }

        // GET: Warehauses
        public ActionResult Index()
        {
            WarehauseList neki = new WarehauseList();
            neki.Warehauses = _db.Warehauses;
            return View(neki);
        }

        // GET: Warehauses/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehause = _db.Warehauses.Find(id);
            if (warehause == null)
            {
                return NotFound();
            }

            return View(warehause);
        }

        // GET: Warehauses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Warehauses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Warehause warehause)
        {
            if (SameLocation(warehause))
            {
                TempData["error"] = "Same location must have different name!";
                return RedirectToAction("Create");
            }
            if (WarehauseExists(warehause))
            {
                TempData["error"] = "Warehause allready exists!";
                return RedirectToAction("Create");
            }
            if (ModelState.IsValid)
            {
                _db.Warehauses.Add(warehause);
                _db.SaveChanges();
                TempData["success"] = "Category created succesfully";
                return RedirectToAction("Index");
            }
            return View(warehause);
        }

        // GET: Warehauses/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return View();
            }

            var warehause = _db.Warehauses.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u => u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (warehause == null)
            {
                return NotFound();
            }
            return View(warehause);
        }

        // POST: Warehauses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Warehause obj)
        {

            if (SameLocation(obj))
            {
                TempData["error"] = "Same location must have different name!";
                return RedirectToAction("Edit");
            }
            if (WarehauseExists(obj))
            {
                TempData["error"] = "Warehause allready exists!";
                return RedirectToAction("Edit");
            }
            if (ModelState.IsValid)
            {
                _db.ChangeTracker.Clear();
                _db.Warehauses.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category edited succesfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // GET: Warehauses/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehause =  _db.Warehauses
                .FirstOrDefault(m => m.WarehauseId == id);
            if (warehause == null)
            {
                return NotFound();
            }

            return View(warehause);
        }

        // POST: Warehauses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var warehause = _db.Warehauses.Find(id);
            _db.Warehauses.Remove(warehause);
            _db.SaveChanges();
            TempData["success"] = "Warehause deleted succesfully";
            return RedirectToAction("Index");
        }

        public bool WarehauseExists(Warehause obj)
        {
            IEnumerable<Warehause> warehauselist = _db.Warehauses;
            bool exists = false;
            foreach (var item in warehauselist)
            {
                if (obj.WarehauseName == item.WarehauseName && obj.WarehauseLocation == item.WarehauseLocation && obj.WarehauseId != item.WarehauseId)
                {
                    exists = true;
                }
            }
            return exists;
        }
        public bool SameLocation(Warehause obj)
        {
            IEnumerable<Warehause> warehauselist = _db.Warehauses;
            bool exists = false;
            foreach (var item in warehauselist)
            {
                if (obj.WarehauseLocation == item.WarehauseLocation && obj.WarehauseId != item.WarehauseId && obj.WarehauseName == null)
                {
                    exists = true;
                }
            }
            return exists;
        }
    }
}
