using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Persistence
{
    public class CustomerRepository
    {
        private readonly RentContext _context;
        public CustomerRepository(RentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetCustomer(Guid Id)
        {
            return await _context.Customers.FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<Customer> CreateCustomer(Customer Customer)
        {
            _context.Customers.Add(Customer);
            await _context.SaveChangesAsync();
            return Customer;
        }

        public async Task<Customer> UpdateCustomer(Customer Customer)
        {
            _context.Customers.Update(Customer);
            await _context.SaveChangesAsync();
            return Customer;
        }

        public async Task<Customer> DeleteCustomer(Customer Customer)
        {
            _context.Customers.Remove(Customer);
            await _context.SaveChangesAsync();
            return Customer;
        }

        public async Task<IEnumerable<Customer>> GetCustomersFromUser(Guid TenantId)
        {
            var query = _context.Customers.AsQueryable();
            query = query.Where(m => m.TenantId == TenantId);
            return await query.ToListAsync();
        }
    }
}