using Data;
using Models;

namespace EcoPower_Logistics.Repository
{
    public class CustomerRepository
    {
        protected readonly SuperStoreContext _context = new SuperStoreContext();

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }
        public Customer GetDetails(int id)
        {
            return _context.Customers.SingleOrDefault();
        }  
        public void Create(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                //If said customer does not exist
                throw new ArgumentNullException("Customer not found", nameof(id));
            }
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }

        public void Edit(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            //Check if the customer with provided ID exists
            var existingcustomer = _context.Customers.Find(customer.CustomerId);

            if(existingcustomer == null)
            {
                throw new ArgumentException("Customer not found", nameof(customer.CustomerId));
            }

            //Update existing customer's properties with the new values
            _context.Entry(existingcustomer).CurrentValues.SetValues(customer);
            _context.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _context.Customers.Any(customer => customer.CustomerId == id);
        }
    }
}
