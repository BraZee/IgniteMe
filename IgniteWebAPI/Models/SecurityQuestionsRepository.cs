using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IgniteWebAPI.DAL;

namespace IgniteWebAPI.Models
{
    public class SecurityQuestionsRepository
    {
        private DataProvider dp = DataProvider.GetInstance();
        private readonly List<SecurityQuestions> securityQuestions = new List<SecurityQuestions>(); 
        
        public IEnumerable<SecurityQuestions> GetAll()
        {
            var questions = dp.GetAllSecurityQuestions();
            securityQuestions.Clear();
            foreach (var question in questions.SecurityQuestion)
            {
                securityQuestions.Add(new SecurityQuestions(){Id = question.SecurityQuestionId, Name = question.SecurityQuestionName});
            }

            return securityQuestions;
        }
    }
}