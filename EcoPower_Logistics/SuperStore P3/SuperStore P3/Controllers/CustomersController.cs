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
using System.Linq.Expressions;

namespace Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        
        private readonly CustomerRepository _customerRepository;
        public CustomersController()
        {
            _customerRepository = new CustomerRepository();
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            CustomerRepository customerRepository = new CustomerRepository();

            var results = customerRepository.GetAll();
            return View(results);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Customer customer = _customerRepository.GetDetails(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerTitle,CustomerName,CustomerSurname,CellPhone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _customerRepository.Create(customer);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //Handle exceptions that occur during creation
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the customer.");
                }
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                Customer customer = _customerRepository.GetDetails(id);
                if (customer == null)
                {
                    //Return 404 status if customer is not found
                    return NotFound();
                }
                return View(customer);
            }
            catch (Exception ex)
            {
                //Handle other exceptions such as database errors
                ModelState.AddModelError(string.Empty, "An error ocurred while loading the customer for editing");
                return View();
            }
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,CustomerTitle,CustomerName,CustomerSurname,CellPhone")] Customer customer)
        {
           if(id != customer.CustomerId)
            {
                return BadRequest();
            }

           if(ModelState.IsValid)
            {
                try
                {
                    _customerRepository.Edit(customer);
                    return RedirectToAction(nameof(Index));
                }
                catch(ArgumentException ex)
                {
                    //Return 404 if customer is not found
                    return NotFound();
                }
                catch(Exception ex)
                {
                    // Handle other exceptions, such as database errors, here.
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the customer.");
                    return View(customer);
                }
            }
            //// If ModelState is not valid, return to the edit view with validation errors
            return View(customer);
        }

        // GET: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _customerRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                // Handle the case where the customer with the given ID was not found.
                return(NotFound());

            }
            catch (Exception ex)
            {
                //Other exceptions such as database errors
                ModelState.AddModelError(String.Empty, "An error occured while deleting customer.");
                return View(nameof(Index));
            }

        }

        //Check if customer exists
        public IActionResult Exist(int id)
        {
            bool customerExists = _customerRepository.Exists(id);

            if (customerExists)
            {
                return Content("Customer does indeed exists.");
            }
            else
            {
                return Content("Customer does not exists at all.");
            }
        }
    }
}
