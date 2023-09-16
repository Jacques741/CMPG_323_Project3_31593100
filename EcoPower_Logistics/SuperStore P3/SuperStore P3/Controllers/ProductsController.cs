using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;
using EcoPower_Logistics.Repository;

namespace Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ProductRepository _productRepository;

        public ProductsController()
        {
            _productRepository = new ProductRepository();
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            ProductRepository productRepository = new ProductRepository();

            var results = productRepository.GetAll();
            return View(results);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Product product = _productRepository.GetDetails(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductDescription,UnitsInStock")] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _productRepository.Create(product);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //Handle exceptions that occur during creation
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the product.");
                }
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                Product product = _productRepository.GetDetails(id);
                if (product == null)
                {
                    //Return 404 status if product is not found
                    return NotFound();
                }
                return View(product);
            }
            catch (Exception ex)
            {
                //Handle other exceptions such as database errors
                ModelState.AddModelError(string.Empty, "An error ocurred while loading the product for editing");
                return View();
            }
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductDescription,UnitsInStock")] Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _productRepository.Edit(product);
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    //Return 404 if product is not found
                    return NotFound();
                }
                catch (Exception ex)
                {
                    // Handle other exceptions, such as database errors, here.
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the product.");
                    return View(product);
                }
            }
            //// If ModelState is not valid, return to the edit view with validation errors
            return View(product);
        }

        // GET: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _productRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                // Handle the case where the product with the given ID was not found.
                return (NotFound());

            }
            catch (Exception ex)
            {
                //Other exceptions such as database errors
                ModelState.AddModelError(String.Empty, "An error occured while deleting product.");
                return View(nameof(Index));
            }

        }
        //Check if product exists
        public IActionResult Exist(int id)
        {
            bool productExists = _productRepository.Exists(id);

            if (productExists)
            {
                return Content("Product does indeed exists.");
            }
            else
            {
                return Content("Product does not exists at all.");
            }
        }
    }
}
