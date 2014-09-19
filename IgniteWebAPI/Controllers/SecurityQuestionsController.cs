using System;
using System.Collections.Generic;
using System.Web.Http;
using IgniteWebAPI.Models;

namespace IgniteWebAPI.Controllers
{
    public class SecurityQuestionsController : ApiController
    {
        static readonly SecurityQuestionsRepository repository = new SecurityQuestionsRepository();

        public IEnumerable<SecurityQuestions> GetAll()
        {
            return repository.GetAll();
        } 
    }
}
