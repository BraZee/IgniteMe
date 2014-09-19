using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IgniteWebAPI.DAL;
using IgniteWebAPI.Models;

namespace IgniteWebAPI.Controllers
{
    public class CustomersController : ApiController
    {
        static readonly CustomersRepository repository = new CustomersRepository();

        public IEnumerable<Customers> GetAllCustomers()
        {
            return repository.GetAll();
        }

        public Customers GetCustomersByUsername(string id)
        {
            var ret = repository.Get(id);

            return ret;

        }
        
        public Customers GetCustomers(int id)
        {
            Customers item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return item;
        }

        public AuthResult GetAuthenticate(string username, string password)
        {
            var ret = repository.Authenticate(username,password);

            return ret;
        }

        public HttpStatusCode PostCustomer(Customers customer)
        {
            var ret = repository.Add(customer);

            if (ret != -1)
            {
                return HttpStatusCode.Created;
            }
            else
            {
                return HttpStatusCode.Conflict;
            }
        }
    }
}
