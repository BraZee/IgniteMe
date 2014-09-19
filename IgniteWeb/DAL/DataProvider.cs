using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using IgniteWeb.DAL.Exceptions;

namespace IgniteWeb.DAL
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

            CustomerDataSet users = GetCustomerByLogon(logon);

            if (users.Customer.Count == 0)
            {
                //user unknown
                result.ExtraMessage = string.Format("the user is unknown.");
            }
            else
            {
                //there is a user with such logon
                //now check the password
                if (Utils.IsEqual(users.Customer[0].Hash, password))
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
                    switch (users.Customer[0].StatusId)
                    {
                        case 1:
                            result.ReturnCode = LoginReturn.Success;
                            result.Success = true;
                            _userId = users.Customer[0].CustomerId;
                            result.UserId = users.Customer[0].CustomerId.ToString();
                            result.UserToken = Utils.GenerateUserToken();
                            _userToken = result.UserToken;
                            lastOperationDateTime = DateTime.Now;
                            result.UserFullName = string.Format("{0} {1}", users.Customer[0].FirstName,
                                                                users.Customer[0].LastName);

                            break;
                        case 2:
                            result.ReturnCode = LoginReturn.InactiveUser;
                            result.Success = false;
                            _userId = users.Customer[0].CustomerId;
                            result.UserId = users.Customer[0].CustomerId.ToString();
                            result.UserToken = Utils.GenerateUserToken();
                            _userToken = result.UserToken;
                            lastOperationDateTime = DateTime.Now;
                            result.UserFullName = string.Format("{0} {1}", users.Customer[0].FirstName,
                                                                users.Customer[0].LastName);
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
                    result.ExtraMessage = string.Format("the password is incorrect.");
                }

            }

            return result;
        }

        public CustomerDataSet GetCustomerByLogon(string username)
        {
            var retDataset = new CustomerDataSet();

            try
            {
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
                        var user = retDataset.Customer.NewCustomerRow();
                        user.CustomerId = int.Parse(reader["CustomerId"].ToString());
                        user.FirstName = reader["FirstName"].ToString();
                        user.LastName = reader["LastName"].ToString();
                        user.Email = reader["Email"].ToString();
                        user.Username = reader["Username"].ToString();
                        user.Hash = reader["Hash"].ToString();
                        user.Status = reader["Status"].ToString();
                        user.StatusId = int.Parse(reader["StatusId"].ToString());
                        user.Active = bool.Parse(reader["Active"].ToString());

                        retDataset.Customer.AddCustomerRow(user);
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


        public int AddCustomer(CustomerDataSet customer,int security_question_id, string security_answer)
        {
            int newId = -1;
            
                try
                {
                    if (conn.State != ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spAddCustomer";

                    var p2 = new SqlParameter[5];
                    for (int i = 0; i < 5; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("firstName", customer.Customer[0].FirstName);
                    p2[1] = new SqlParameter("lastName", customer.Customer[0].LastName);
                    p2[2] = new SqlParameter("email", customer.Customer[0].Email);
                    p2[3] = new SqlParameter("username",  customer.Customer[0].Username);
                    p2[4] = new SqlParameter("updatUser", 100);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    UpdateUserSecurity(newId, customer.Customer[0].Hash, security_question_id,
                        security_answer);
               
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