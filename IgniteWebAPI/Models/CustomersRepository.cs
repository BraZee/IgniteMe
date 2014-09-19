using System.Collections.Generic;
using System.Linq;
using IgniteWebAPI.DAL;

namespace IgniteWebAPI.Models
{
    public class CustomersRepository
    {
        private DataProvider dp = DataProvider.GetInstance();
        private List<Customers> cust = new List<Customers>();

        public AuthResult Authenticate(string username, string password)
        {
            var c = dp.Authenticate(username, password);

            return c;
        }

        public int Add(Customers customer)
        {
            int result = dp.AddCustomer(customer);

            return result;
        }

        public IQueryable<Customers> GetAll()
        {
            var c = dp.GetAllCustomers();
            
            return c.AsQueryable();
        }

        public Customers Get(int id)
        {
            GetAll();
            return cust.Find(c => c.Id == id);
        }

        public Customers Get(string username)
        {
            var c = dp.GetCustomerByLogon(username);

            return c;
        }
    }
}