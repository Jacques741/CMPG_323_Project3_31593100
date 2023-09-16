using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using EcoPower_Logistics.Repository;

namespace Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly OrderRepository _orderRepository;

        public OrdersController(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var order = _orderRepository.GetAll();
            return Ok(order);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var orderId = _orderRepository.GetById(id);

            if (orderId == null)
            {
                //Return 404 not found if order is not found
                return NotFound();
            }

            return View(orderId);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = _orderRepository.GetLists();
            return View();
        }

        // POST: Orders/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderDate,CustomerId,DeliveryAddress")] Order order)
        {
            if (ModelState.IsValid)
            {
                await _orderRepository.Create(order);
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = _orderRepository.GetLists();
            return View(order);
        }



        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.GetById(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            ViewData["CustomerId"] = _orderRepository.GetLists();
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderDate,CustomerId,DeliveryAddress")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderRepository.Update(order);
                }
                catch (InvalidOperationException)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = _orderRepository.GetLists();
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.GetById(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderRepository.DeleteOrder(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Exists/5
        [HttpGet("Exists/{id}")]
        public IActionResult Exist(int id)
        {
            bool orderExists = _orderRepository.Exists(id);

            if (orderExists)
            {
                return Content("Order does indeed exists.");
            }
            else
            {
                return Content("Order does not exists at all.");
            }
        }
    }
}
