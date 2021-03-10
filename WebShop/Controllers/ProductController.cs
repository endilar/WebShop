using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebShop_DataAccess.Data;
using WebShop_DataAccess.Repository.IRepository;
using WebShop_Models;
using WebShop_Models.ViewModels;
using WebShop_Utility;

namespace WebShop.Controllers
{
    [Authorize(Roles =WC.AdminRole)]
    public class ProductController : Controller
    {

        private readonly IProductRepository _prodRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductRepository prodRepo, IWebHostEnvironment webHostEnvironment)
        {
            _prodRepo = prodRepo;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            IEnumerable<Product> objList = _prodRepo.GetAll(includeProperties:("Category,ApplicationType"));
            //foreach (var obj in objList)
            //{
            //    obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
            //    obj.ApplicationType = _db.ApplicationType.FirstOrDefault(u => u.Id == obj.ApplicationTypeId);
            //}
            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});

            //ViewBag.CategoryDropDown = CategoryDropDown;

            //Product product = new Product(); 

            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _prodRepo.GetAllDropdownList(WC.CategoryName),
                ApplicationTypeSelectList = _prodRepo.GetAllDropdownList(WC.ApplicationTypeName),
            };

            if (id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _prodRepo.Find(id.GetValueOrDefault());
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if(productVM.Product.Id == 0)
                {
                    string upload = webRootPath + WC.ImagePath;
                    string filename = Guid.NewGuid().ToString();
                    string extention = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, filename + extention), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productVM.Product.Image = filename + extention;

                    _prodRepo.Add(productVM.Product);
                }
                else
                {
                    var objectFromDb = _prodRepo.FirstOrDefault(u => u.Id == productVM.Product.Id,isTreacking:false);

                    if(files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string filename = Guid.NewGuid().ToString();
                        string extention = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objectFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, filename + extention), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = filename + extention;
                    }
                    else
                    {
                        productVM.Product.Image = objectFromDb.Image;
                    }
                    _prodRepo.Update(productVM.Product);
                }
                _prodRepo.Save();
                TempData[WC.Success] = "Action completed successfully";
                return RedirectToAction("Index");
            }
            productVM.CategorySelectList = _prodRepo.GetAllDropdownList(WC.CategoryName);
            productVM.ApplicationTypeSelectList = _prodRepo.GetAllDropdownList(WC.ApplicationTypeName);
            return View(productVM);

        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product product = _prodRepo.FirstOrDefault(u=>u.Id==id,includeProperties:"Category,ApplicationType");

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _prodRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, obj.Image);
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }


            _prodRepo.Remove(obj);
            _prodRepo.Save();
            TempData[WC.Success] = "Action completed successfully";
            return RedirectToAction("Index");


        }
    }
}

        