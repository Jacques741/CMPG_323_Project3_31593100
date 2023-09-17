using Data;
using Models;

namespace EcoPower_Logistics.Repository
{
    public class ProductRepository
    {
        protected readonly SuperStoreContext _context = new SuperStoreContext();

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Product GetDetails(int id)
        {
            return _context.Products.SingleOrDefault();
        }

        public void Create(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                //If said product does not exist
                throw new ArgumentNullException("Product not found", nameof(id));
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public void Edit(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            //Check if the product with provided ID exists
            var existingProduct = _context.Products.Find(product.ProductId);

            if (existingProduct == null)
            {
                throw new ArgumentException("Product not found", nameof(product.ProductId));
            }

            //Update existing product's properties with the new values
            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            _context.SaveChanges();
        }
        public bool Exists(int id)
        {
            return _context.Products.Any(product => product.ProductId == id);
        }
    }
}
