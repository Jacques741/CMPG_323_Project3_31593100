using Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;


namespace EcoPower_Logistics.Repository
{
    public class OrderRepository
    {
        protected readonly SuperStoreContext _context = new SuperStoreContext();
        //Get All
        public IEnumerable<Order> GetAll()
        {
            return _context.Orders.Include(o => o.Customer).ToList();
        }

        //Get By ID
        public async Task<Order> GetById(int id)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.OrderId == id);
        }

        //Create orders
        public async Task Create(Order order)
        {
            _context.Add(order);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<SelectListItem> GetLists()
        {
            return new SelectList(_context.Customers, "CustomerId", "CustomerId");
        }

        //Methods to enable edit
        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public IEnumerable<SelectListItem> GetCustomerSelectList()
        {
            // Assuming you have a Customers DbSet in your context
            return new SelectList(_context.Customers, "CustomerId", "CustomerId");
        }

        //Edit orders
        public async Task Update(Order order)
        {
            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(order.OrderId))
                {
                    throw new InvalidOperationException("Order not found.");
                }
                else
                {
                    throw;
                }
            }
        }

        //Delete an order
        public async Task DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
        //Check wheter an order exists
        public bool Exists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
