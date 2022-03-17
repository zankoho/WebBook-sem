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

namespace WebBook.Controllers
{
    public class StocksController : Controller
    {
        private readonly DBContext _context;

        public StocksController(DBContext context)
        {
            _context = context;
        }

        // GET: Stocks
        public async Task<IActionResult> Index()
        {
            var dBContext = _context.Stocks.Include(s => s.Product).Include(s => s.Warehause);
            return View(await dBContext.ToListAsync());
        }

        // GET: Stocks/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Warehause)
                .FirstOrDefault(m => m.StockId == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // GET: Stocks/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            ViewData["WarehauseId"] = new SelectList(_context.Warehauses, "WarehauseId", "WarehauseLocation");
            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create( Stock stock)
        {
            if (StockExists(stock))
            {
                TempData["error"] = "Stock allready exsists";
                return RedirectToAction("Create");
            }
            _context.Add(stock);
            _context.SaveChanges();

            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", stock.ProductId);
            ViewData["WarehauseId"] = new SelectList(_context.Warehauses, "WarehauseId", "WarehauseLocation", stock.WarehauseId);
            return RedirectToAction("Index");
            
        }

        // GET: Stocks/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock =  _context.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            ViewData["WarehauseId"] = new SelectList(_context.Warehauses, "WarehauseId", "WarehauseLocation");
            return View(stock);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Stock stock)
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", stock.ProductId);
            ViewData["WarehauseId"] = new SelectList(_context.Warehauses, "WarehauseId", "WarehauseLocation", stock.WarehauseId);
            //if (ModelState.IsValid)
            //{
            
            _context.ChangeTracker.Clear();
            if (StockExists(stock))
            {
                TempData["error"] = "Stock allready exsists";
                return RedirectToAction("Edit");
            }
            _context.ChangeTracker.Clear();
            _context.Update(stock);
            _context.SaveChanges();

            return RedirectToAction("Index");

        }
        public IActionResult EditQuantity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Warehause)
                .FirstOrDefault(m => m.StockId == id);
            if (stock == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            ViewData["WarehauseId"] = new SelectList(_context.Warehauses, "WarehauseId", "WarehauseLocation");
            return View(stock);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditQuantity(Stock stock)
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", stock.ProductId);
            ViewData["WarehauseId"] = new SelectList(_context.Warehauses, "WarehauseId", "WarehauseLocation", stock.WarehauseId);
            if (ModelState.IsValid)
            {

            _context.ChangeTracker.Clear();

            _context.Update(stock);
            _context.SaveChanges();
            TempData["success"] = "Quantity edited succesfully";
            return RedirectToAction("Index");
            }

            return View(stock);
        }

        // GET: Stocks/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Warehause)
                .FirstOrDefault(m => m.StockId == id);
            //var stock = _context.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var stock = _context.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }
            _context.Stocks.Remove(stock);
            _context.SaveChanges();
            TempData["success"] = "Stock deleted succesfully";
            return RedirectToAction("Index");
        }




        // GET: Stocks/Edit/5
        public IActionResult Sell()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            ViewData["WarehauseId"] = new SelectList(_context.Warehauses, "WarehauseId", "WarehauseLocation");
            return View();
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Sell(Stock stock, int minus)
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", stock.ProductId);
            ViewData["WarehauseId"] = new SelectList(_context.Warehauses, "WarehauseId", "WarehauseLocation", stock.WarehauseId);
            var stockList = _context.Stocks.Include(s => s.Product);
            bool temp = false;
            foreach (var item in stockList)
            {
                if (item.ProductId == stock.ProductId && item.WarehauseId == stock.WarehauseId)
                {
                    stock = item;
                    temp = true;
                }
            }
            if (!temp)
            {
                TempData["error"] ="Product is not in that warehause!";
                return RedirectToAction("Sell");
            }
            _context.ChangeTracker.Clear();
            if (stock.Quantity < minus)
            {
                TempData["error"] = "Out of stock! Stock is lower than" + minus;
                return RedirectToAction("Sell");
            }
            stock.Quantity -= minus;

            string prodname = stock.Product.ProductName;

            _context.ChangeTracker.Clear();
            _context.Update(stock);
            _context.SaveChanges();
            TempData["success"] = minus + " items of " + prodname +" sold!";
            return RedirectToAction("index");

        }


        public IActionResult Supply()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            ViewData["WarehauseId"] = new SelectList(_context.Warehauses, "WarehauseId", "WarehauseLocation");
            return View("Supply");
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Supply(Stock stock, int plus)
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", stock.ProductId);
            ViewData["WarehauseId"] = new SelectList(_context.Warehauses, "WarehauseId", "WarehauseLocation", stock.WarehauseId);
            var stockList = _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Warehause);
            bool temp = false;
            foreach (var item in stockList)
            {
                if (item.ProductId == stock.ProductId && item.WarehauseId == stock.WarehauseId)
                {
                    stock = item;
                    temp = true;
                }
            }
            if (plus == 0)
            {
                return View("Supply", stock);
            }
            if (!temp)
            {
                TempData["error"] = "Product is not in that warehause!";
                return RedirectToAction("Supply");
            }
            _context.ChangeTracker.Clear();
            stock.Quantity += plus;

            string prodname = stock.Product.ProductName;
            string warehasename = stock.Warehause.WarehauseName;

            _context.ChangeTracker.Clear();
            _context.Update(stock);
            _context.SaveChanges();
            TempData["success"] = plus + " of " + prodname + " supplied to " + warehasename ;
            return RedirectToAction("index");

        }



        public bool StockExists(Stock obj)
        {
            IEnumerable<Stock> stocklist = _context.Stocks;
            bool exists = false;
            foreach (var item in stocklist)
            {
                if (obj.ProductId == item.ProductId && obj.WarehauseId == item.WarehauseId && obj.StockId != item.StockId)
                {
                    exists = true;
                }
            }
            return exists;
        }
    }
}
