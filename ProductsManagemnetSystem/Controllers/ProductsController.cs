using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductsManagemnetSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManagemnetSystem.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductsController(Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        //public ActionResult Index()
        //{
        //    return View(_context.allproducts.ToList());
        //}
        //Search 
        public async Task<IActionResult> Index(string? searchString)
        {
            //return View(_context.allproducts.ToList());

            //ViewBag.Greetings = "Happy Coding!";

            //var products = _context.allproducts.Where(pro => pro.Category == "Mobile").ToList();

            //ViewBag.Developers = products;

            //ViewData["country"] = "where are you from ?";

            //var designers = _context.allproducts.Where(pro => pro. == "Designer").ToList();

            //ViewData["allDesigners"] = designers;

            var pros = from m in _context.allproducts
                       select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                pros = pros.Where(s => s.Category.Contains(searchString));
            }

            return View(await pros.ToListAsync());
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MyProducts pro)
        {
            if (ModelState.IsValid)
            {

                string wwwRootPath = _webHostEnvironment.WebRootPath;

                string fileName = Path.GetFileNameWithoutExtension(pro.ImageFile.FileName);

                string extension = Path.GetExtension(pro.ImageFile.FileName);

                pro.ImageName = fileName = fileName + DateTime.Now.ToString("yyMMddHHmmssff") + extension;

                string imagePath = Path.Combine(wwwRootPath + "/images/" + fileName);

                FileStream fileStream = new FileStream(imagePath, FileMode.Create);
                pro.ImageFile.CopyTo(fileStream);





            }
            _context.allproducts.Add(pro);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MyProducts pro = _context.allproducts.Find(id);
            if (pro == null)
            {
                return NotFound();
            }
            return View(pro);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MyProducts pro = _context.allproducts.Find(id);
            try
            {
                var imagepath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", pro.ImageName);

                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }
            } catch
            {
               
            }
            


            _context.allproducts.Remove(pro);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            MyProducts pro = _context.allproducts.Find(id);

            if (pro == null)
                return NotFound();

            return View(pro);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int? id,  MyProducts pro)
        //{
        //    //if (id != pro.ProductId)
        //    //{
        //    //    return NotFound();
        //    //}

        //    if (ModelState.IsValid)
        //    {
        //        _context.allproducts.Update(pro);
        //        _context.SaveChanges();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(pro);
        //}


        //[HttpGet]
        //public ActionResult Edit(int id, MyProducts pro)
        //{

        //    //MyProducts pro2 = _context.allproducts.Find(id);
        //    //if (pro2 == null)
        //    //{
        //    //    return NotFound();
        //    //}
        //    //if (id != pro.ProductId)
        //    //{
        //    //    return NotFound();
        //    //}
        //    //return View(pro2);
        //    return View("Edit");
        //}




        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.allproducts.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

     
      
        private bool ProductsExists(int id)
        {
            return _context.allproducts.Any(e => e.ProductId == id);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Category,Color,Price,AvailableQuantity")] MyProducts products)
        {
            if (id != products.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }
    }
}