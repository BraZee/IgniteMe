using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using IgniteWebAPI.DAL.Data_Objects;
using IgniteWebAPI.DAL.Exceptions;
using IgniteWebAPI.Models;

namespace IgniteWebAPI.DAL
{
    public class DataProvider
    {
        readonly SqlConnection conn;
        //  SqlConnection posConn = null;
        /// <summary>
        /// delay before the expiration of the userToken
        /// </summary>
        private readonly double _timeOut;
       
        static DataProvider instance;

        public static DataProvider GetInstance()
        {
            if (instance == null)
                instance = new DataProvider();
            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        protected DataProvider()
        {

            string connString = ConfigurationManager.ConnectionStrings["IgniteDBConnectionString"].ConnectionString;


            conn = new SqlConnection(connString);

            _timeOut = double.Parse(ConfigurationManager.AppSettings["TimeOut"]);
            
        }


        private static int _userId = -1;
        private static string _userToken = string.Empty;
        private static DateTime lastOperationDateTime;

        public static string UserToken
        {
            get { return _userToken; }
            set { _userToken = value; }
        }


        public static int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public static string Hash(string password)
        {
            return Utils.HashProperty(password, Utils.GetRandomString(8));
        }

        /// <summary>
        /// Checks if the userToken is valid
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns>true if the userToken is valid</returns>
        /// <exception cref="InvalidUserTokenException">if the provided userToken is not valid</exception>
        /// <exception cref="ExpiredUserTokenException">if the provided userToken has expired</exception>
        public bool IsUserTokenValid(string userToken)
        {
            //for testing purpose only. remove later
            if (userToken != "token")
            {
                if ((_userToken == string.Empty) || (userToken != _userToken))
                    throw new InvalidUserTokenException("The provided userToken is not valid");

                if (DateTime.Now.Subtract(lastOperationDateTime).TotalMinutes > _timeOut)
                {
                    //Logout(userToken);
                    throw new ExpiredUserTokenException("the provided userToken has expired");
                
               }
            }
            lastOperationDateTime = DateTime.Now;
            return true;
        }

        /// <summary>
        /// Invalidates the userToken
        /// </summary>
        /// <param name="userToken">The userToken to be invalidated</param>
        public void Logout(string userToken)
        {
            if (IsUserTokenValid(userToken))
            {
                _userToken = string.Empty;
            }
        }


        /// <summary>
        /// This method authenticates the login credentials passed
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="logon">The clear form logon name of the user</param>
        /// <param name="password">The clear form password of the user</param>
        /// <returns>An inner variable which is a <see cref="AuthResult"/></returns>
        public AuthResult Authenticate(string logon, string password)
        {
            var result = new AuthResult();

            Customers users = GetCustomerByLogon(logon);

            if (users == null)
            {
                //user unknown
                result.ExtraMessage = string.Format("Unknown User!");
            }
            else
            {
                //there is a user with such logon
                //now check the password
                if (Utils.IsEqual(users.Hash, password))
                {
                    /*//the password is correct
                    result.ReturnCode = LoginReturn.Success;
                    result.Success = true;
                    _userId = users.User[0].UserId;
                    result.UserId = _userId.ToString();
                    result.UserToken = Utils.GenerateUserToken();
                    _userToken = result.UserToken;
                    lastOperationDateTime = DateTime.Now;
                    result.UserFullName = string.Format("{0} {1}", users.User[0].FirstName, users.User[0].LastName);
*/
                    switch (users.StatusId)
                    {
                        case 1:
                            result.ReturnCode = LoginReturn.Success;
                            result.Success = true;
                            _userId = users.Id;
                            result.UserId = users.Id.ToString();
                            result.UserToken = Utils.GenerateUserToken();
                            _userToken = result.UserToken;
                            lastOperationDateTime = DateTime.Now;
                            result.UserFullName = string.Format("{0},{1},{2}", users.FirstName,users.MiddleName,
                                                                users.LastName);
                            result.ExtraMessage = users.Username;

                            break;
                        case 2:
                            result.ReturnCode = LoginReturn.InactiveUser;
                            result.Success = false;
                            _userId = users.Id;
                            result.UserId = users.Id.ToString();
                            result.UserToken = Utils.GenerateUserToken();
                            _userToken = result.UserToken;
                            lastOperationDateTime = DateTime.Now;
                            result.UserFullName = string.Format("{0},{1},{2}", users.FirstName, users.MiddleName,
                                                                users.LastName);
                            result.ExtraMessage = users.Username;
                            break;

                        case 3:
                            result.Success = false;
                            result.ReturnCode = LoginReturn.DeletedUser;
                            break;

                        case 4:
                            result.Success = false;
                            result.ReturnCode = LoginReturn.ExpiredCredentials;
                            break;
                    }

                }
                else
                {
                    //the password is wrong
                    result.ExtraMessage = string.Format("Incorrect Password!");
                }

            }

            return result;
        }

        public Customers GetCustomerByLogon(string username)
        {
            Customers customer = null;

            try
            {
                if (conn.State != ConnectionState.Open || conn.State != ConnectionState.Connecting)
                conn.Open();
               

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetCustomerByLogon";

                var p = new SqlParameter("username",username);
                
                cmd.Parameters.Add(p);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.Read())
                    {   
                        customer = new Customers();
                        customer.Id = int.Parse(reader["CustomerId"].ToString());
                        customer.FirstName = reader["FirstName"].ToString();
                        customer.LastName = reader["LastName"].ToString();
                        customer.Email = reader["Email"].ToString();
                        customer.Username = reader["Username"].ToString();
                        customer.Hash = reader["Hash"].ToString();
                        customer.Status = reader["Status"].ToString();
                        customer.StatusId = int.Parse(reader["StatusId"].ToString());
                        customer.MiddleName = reader["MiddleName"].ToString();
                        customer.Country = reader["Country"].ToString();
                        customer.City = reader["City"].ToString();
                        customer.Town = reader["Town"].ToString();
                        customer.Telephone = reader["Telephone"].ToString();
                        customer.Dob = DateTime.Parse(reader["Dob"].ToString());
                        customer.Active = bool.Parse(reader["Active"].ToString());
                    }
                    reader.Close();
                }



            }
            catch (Exception ex)
            {
                throw new OperationFailedException(ex.Message, ex);
            }
            finally
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();
                }
            }

            return customer;
        }



        public List<Customers> GetAllCustomers()
        {
            List<Customers> retDataset = new List<Customers>();

            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetAllCustomers";

               SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    while(reader.Read())
                    {
                        var customer = new Customers();
                        customer.Id = int.Parse(reader["CustomerId"].ToString());
                        customer.FirstName = reader["FirstName"].ToString();
                        customer.LastName = reader["LastName"].ToString();
                        customer.Email = reader["Email"].ToString();
                        customer.Username = reader["Username"].ToString();
                        customer.Hash = reader["Hash"].ToString();
                        customer.Status = reader["Status"].ToString();
                        customer.StatusId = int.Parse(reader["StatusId"].ToString());
                        customer.Active = bool.Parse(reader["Active"].ToString());

                        retDataset.Add(customer);
                        
                    }
                    reader.Close();
                }



            }
            catch (Exception ex)
            {
                throw new OperationFailedException(ex.Message, ex);
            }
            finally
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();
                }
            }

            return retDataset;
        }


        public SecurityQuestiondataSet GetAllSecurityQuestions()
        {
            var retDataset = new SecurityQuestiondataSet();
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetAllSecurityQuestions";

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var secquest = retDataset.SecurityQuestion.NewSecurityQuestionRow();
                        secquest.SecurityQuestionId = int.Parse(reader["SecurityQuestionId"].ToString());
                        secquest.SecurityQuestionName = reader["SecurityQuestionName"].ToString();

                        retDataset.SecurityQuestion.AddSecurityQuestionRow(secquest);
                    }
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                throw new OperationFailedException(ex.Message, ex);
            }
            finally
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
            return retDataset;
        }


        public int AddCustomer(Customers customer)
        {
            int newId = -1;
            
                try
                {
                    if (conn.State != ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spAddCustomer";

                    var p2 = new SqlParameter[11];
                    for (int i = 0; i < 11; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("firstName", customer.FirstName);
                    p2[1] = new SqlParameter("lastName", customer.LastName);
                    p2[2] = new SqlParameter("email", customer.Email);
                    p2[3] = new SqlParameter("username",  customer.Username);
                    p2[4] = new SqlParameter("updatUser", 100);
                    if (customer.Dob > DateTime.MinValue)
                    {
                        p2[5] = new SqlParameter("dob", customer.Dob);
                    }
                    else
                    {
                        p2[5] = new SqlParameter("dob",SqlDateTime.MinValue);
                    }

                    p2[6] = new SqlParameter("country",customer.Country);
                    p2[7] = new SqlParameter("city", customer.City);
                    p2[8] = new SqlParameter("town", customer.Town);
                    p2[9] = new SqlParameter("middleName", customer.MiddleName);
                    p2[10] = new SqlParameter("telephone", customer.Telephone);
                    
                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    UpdateUserSecurity(newId, customer.Hash, customer.SecurityQuestionId,
                        customer.SecurityAnswer);
               
                }
                catch (Exception ex)
                {
                    throw new OperationFailedException(ex.Message, ex);
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            
            return newId;
        }

        public bool UpdateUserSecurity(int userId, string password, int securityQuestionId, string securityAnswer)
        {
            bool retValue = false;
//            if (IsUserTokenValid(userToken))
//            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spCreateCustomerPassword";

                    var p2 = new SqlParameter[4];
                    for (int i = 0; i < 4; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("id", userId);
                    p2[1] = new SqlParameter("securityQuestionId", securityQuestionId);
                    p2[2] = new SqlParameter("securityAnswer", securityAnswer);
                    p2[3] = new SqlParameter("hash", Hash(password));

                    cmd.Parameters.AddRange(p2);

                    cmd.ExecuteNonQuery();

                    retValue = true;
                }
                catch (Exception ex)
                {
                    throw new OperationFailedException(ex.Message, ex);
                }
                finally
                {
                    if (conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            //}
            return retValue;
        }
    }
}