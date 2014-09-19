using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Services;
using Ignite.DAL.Exceptions;
using System.Net;

namespace Ignite.DAL
{
    public class DataProvider
    {
        readonly SqlConnection conn;
        readonly SqlConnection emailConn;
      //  SqlConnection posConn = null;
        /// <summary>
        /// delay before the expiration of the userToken
        /// </summary>
       private readonly double _timeOut;
        private readonly string _userName = string.Empty;
        private readonly string _password = string.Empty;

        private readonly string _fromName = string.Empty;
        private readonly string _fromAddress = string.Empty;
        private readonly string _emailSubject = string.Empty;
        private readonly string _smtpServer = string.Empty;
        private readonly string _appName = string.Empty;
        private readonly string _appURL = string.Empty; 

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

            string connString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            string emailDbConnString = ConfigurationManager.ConnectionStrings["EmailDBConnString"].ConnectionString;

            conn = new SqlConnection(connString);
            emailConn = new SqlConnection(emailDbConnString);

            _timeOut = double.Parse(ConfigurationManager.AppSettings["TimeOut"]);
            _appName = ConfigurationManager.AppSettings["AppName"];
            _appURL = ConfigurationManager.AppSettings["AppURL"];
            _fromAddress = ConfigurationManager.AppSettings["FromAddress"];
            _fromName = ConfigurationManager.AppSettings["FromName"];
            _emailSubject = ConfigurationManager.AppSettings["EmailSubject"];
            _smtpServer = ConfigurationManager.AppSettings["SMTPServer"];
            _userName = ConfigurationManager.AppSettings["UserName"];
            _password = ConfigurationManager.AppSettings["Password"];
        }	



        #region User Management

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
                    //throw new ExpiredUserTokenException("the provided userToken has expired");
                   // Res
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
        /// Gets the password for the supplied logon name
        /// </summary>
        /// <param name="userId">The userId to be used for the search</param>
        /// <returns>The hashed string for the logon name provided</returns>
        public string GetPasswordByUserId(int userId)
        {
            var retValue = string.Empty;
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetPasswordByUserId";

                var p = new SqlParameter[1];
                for (int i = 0; i < 1; i++)
                {
                    p[i] = new SqlParameter();
                }

                p[0] = new SqlParameter("userId", userId);
                cmd.Parameters.AddRange(p);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        retValue = reader["Hash"].ToString();
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
            return retValue;
        }



        /// <summary>
        /// This method gets the user specified by the supplied user logon name
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="logon">The logon name of the user whose data is being sought for</param>
        /// <returns>An inner variable which is a <see cref="UserDataSet"/></returns>
        private UserDataSet GetUserByLogon(int orgId, string logon)
        {
            var retDataset = new UserDataSet();

            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetUserByLogon";

                var p = new SqlParameter[2];
                for (int i = 0; i < 2; i++)
                {
                    p[i] = new SqlParameter();
                }

                p[0] = new SqlParameter("logon", logon);
                p[1] = new SqlParameter("orgId", orgId);
                cmd.Parameters.AddRange(p);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        var user = retDataset.User.NewUserRow();
                        user.UserId = int.Parse(reader["UserId"].ToString());
                        user.FirstName = reader["FirstName"].ToString();
                        user.LastName = reader["LastName"].ToString();
                        user.Email = reader["Email"].ToString();
                        user.Logon = reader["Logon"].ToString();
                        user.Hash = reader["Hash"].ToString();
                        user.Status = reader["Status"].ToString();
                        user.StatusId = int.Parse(reader["StatusId"].ToString());
                        user.UserGroup = reader["UserGroup"].ToString();
                        user.UserGroupId = int.Parse(reader["UserGroupId"].ToString());
                        user.Active = bool.Parse(reader["Active"].ToString());

                        retDataset.User.AddUserRow(user);
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


        /// <summary>
        /// This method creates a salted hash of a clear form password
        /// </summary>
        /// <param name="password">Clear form apssword</param>
        /// <returns>String that is a salted hash of the clear form passowrd</returns>
        private static string Hash(string password)
        {
            return Utils.HashProperty(password, Utils.GetRandomString(8));
        }

        /// <summary>
        /// This method authenticates the login credentials passed
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="logon">The clear form logon name of the user</param>
        /// <param name="password">The clear form password of the user</param>
        /// <returns>An inner variable which is a <see cref="AuthResult"/></returns>
        public AuthResult Authenticate(int orgId, string logon, string password)
        {
            var result = new AuthResult();

            UserDataSet users = GetUserByLogon(orgId, logon);

            if (users.User.Count == 0)
            {
                //user unknown
                result.ExtraMessage = string.Format("the user is unknown.");
            }
            else
            {
                //there is a user with such logon
                //now check the password
                if (Utils.IsEqual(users.User[0].Hash, password))
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
                    switch (users.User[0].StatusId)
                    {
                        case 1:
                            result.ReturnCode = LoginReturn.Success;
                            result.Success = true;
                            _userId = users.User[0].UserId;
                            result.UserId = users.User[0].UserId.ToString();
                            result.UserToken = Utils.GenerateUserToken();
                            _userToken = result.UserToken;
                            lastOperationDateTime = DateTime.Now;
                            result.UserFullName = string.Format("{0} {1}", users.User[0].FirstName,
                                                                users.User[0].LastName);

                            break;
                        case 2:
                            result.ReturnCode = LoginReturn.InactiveUser;
                            result.Success = false;
                            _userId = users.User[0].UserId;
                            result.UserId = users.User[0].UserId.ToString();
                            result.UserToken = Utils.GenerateUserToken();
                            _userToken = result.UserToken;
                            lastOperationDateTime = DateTime.Now;
                            result.UserFullName = string.Format("{0} {1}", users.User[0].FirstName,
                                                                users.User[0].LastName);
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


        /// <summary>
        /// Returns all the security questions in database
        /// </summary>
        /// <returns>An inner variable which is a <see cref="SecurityQuestionDataSet"/></returns>
        public SecurityQuestionDataSet GetAllSecurityQuestions()
        {
            var retDataset = new SecurityQuestionDataSet();
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


        public Dictionary<string,string> GetAllAdminUserEmail()
        {
            var retDataset = new Dictionary<string, string>();
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetAllAdminUserEmail";

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        retDataset.Add(reader["Name"].ToString(),reader["Email"].ToString());
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


        public Dictionary<string,string> GetAllCustomerEmail()
        {
            var retDataset = new Dictionary<string, string>();
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetAllCustomerEmail";

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        retDataset.Add(reader["Name"].ToString(),reader["Email"].ToString());
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


        public int AddEmailOut(EmailOutDataSet sysParameter)
        {
            int newId = -1;

            try
            {
                if (emailConn.State != System.Data.ConnectionState.Open) { emailConn.Open(); }
                SqlCommand cmd = emailConn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spAddEmailOut";

                var p2 = new SqlParameter[12];
                for (int i = 0; i < 12; i++)
                    p2[i] = new SqlParameter();

                p2[0] = new SqlParameter("senderName", sysParameter.EmailOut[0].SenderName);
                p2[1] = new SqlParameter("subject", sysParameter.EmailOut[0].Subject);
                p2[2] = new SqlParameter("recipient", sysParameter.EmailOut[0].Recipient);
                p2[3] = new SqlParameter("cc", sysParameter.EmailOut[0].CC);
                p2[4] = new SqlParameter("bcc", sysParameter.EmailOut[0].BCC);
                p2[5] = new SqlParameter("message", sysParameter.EmailOut[0].Message);
                p2[6] = new SqlParameter("gatewayId", sysParameter.EmailOut[0].GatewayId);
                p2[7] = new SqlParameter("attachments", sysParameter.EmailOut[0].Attachments);
                p2[8] = new SqlParameter("status", sysParameter.EmailOut[0].Status);
                p2[9] = new SqlParameter("errorCode", sysParameter.EmailOut[0].ErrorCode);
                p2[10] = new SqlParameter("e_datetime", sysParameter.EmailOut[0].EDateTime);
                p2[11] = new SqlParameter("s_datetime", sysParameter.EmailOut[0].SDateTime);

                cmd.Parameters.AddRange(p2);

                newId = int.Parse(cmd.ExecuteScalar().ToString());

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (emailConn.State != System.Data.ConnectionState.Closed)
                {
                    emailConn.Close();
                }
            }

            return newId;
        }


        public Dictionary<string,string> GetAllOwnerOrgEmail()
        {
            var retDataset = new Dictionary<string, string>();
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetAllOwnerOrgEmail";

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        retDataset.Add(reader["Name"].ToString(), reader["Email"].ToString());
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


        public Dictionary<string, string> GetAllCustomerEmailByInterest(int interestId)
        {
            var retDataset = new Dictionary<string, string>();
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetAllCustomerEmailByInterests";

                var p = new SqlParameter("interestId", interestId);
                cmd.Parameters.Add(p);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        retDataset.Add(reader["Name"].ToString(),reader["Email"].ToString());
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


        public Dictionary<string,string> GetAllOwnerOrgEmailByCategory(int categoryId)
        {
            var retDataset = new Dictionary<string, string>();
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetAllOwnerOrgEmailByCategory";

                var p = new SqlParameter("categoryId", categoryId);
                cmd.Parameters.Add(p);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        retDataset.Add(reader["Name"].ToString(),reader["Email"].ToString());
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


        /// <summary>
        /// Hashes and validates the supplied old password and if corrects replaces it with the hash of the new password
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="userId">The id of the user whose passowrd is to be reset</param>
        /// <param name="oldPassword">The clear form old password of teh user</param>
        /// <param name="newPassword">The new password of the user</param>
        /// <returns>A <code>true</code> for success or <false>for failure or an exception if password  mismatch</false></returns>
        public bool ChangePassword(string userToken, int userId, string oldPassword, string newPassword)
        {
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    //get the old hashed password
                    string oldPasswordHash = GetPasswordByUserId(userId);

                    string newPasswordHash = Hash(newPassword);

                    if (oldPasswordHash != string.Empty)
                    {
                        if (!Utils.IsEqual(oldPasswordHash, oldPassword))
                            throw new OperationFailedException("The old password provided is wrong");
                    }

                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spChangePassword";

                    var p = new SqlParameter[2];
                    for (int i = 0; i < 2; i++)
                        p[i] = new SqlParameter();

                    p[0] = new SqlParameter("userId", userId);
                    p[1] = new SqlParameter("hash", newPasswordHash); 
                    
                    cmd.Parameters.AddRange(p);

                    cmd.ExecuteNonQuery();
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
            }
            return true;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        private int GetUserIdByEmail(string userEmail)
        {
            int retValue = -1;
            try
            {

                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetUserIdByEmail";

                var p = new SqlParameter[1];
                for (int i = 0; i < 1; i++)
                {
                    p[i] = new SqlParameter();
                }

                p[0] = new SqlParameter("email", userEmail);
                cmd.Parameters.AddRange(p);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        retValue = int.Parse(reader["UserId"].ToString());
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
            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public int GetUserIdBySecurityDetails(int orgId, string userEmail, int securityQuestionId, string securityAnswer)
        {
            int retValue = -1;
            try
            {

                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetUserIdBySecurityDetails";

                var p = new SqlParameter[4];
                for (int i = 0; i < 4; i++)
                {
                    p[i] = new SqlParameter();
                }

                p[0] = new SqlParameter("orgId", orgId);
                p[1] = new SqlParameter("email", userEmail);
                p[2] = new SqlParameter("security_question_id", securityQuestionId);
                p[3] = new SqlParameter("security_answer", securityAnswer);
                cmd.Parameters.AddRange(p);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        retValue = int.Parse(reader["UserId"].ToString());
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
            return retValue;
        }



        /// <summary>
        /// This method attempts to crate the status of the specified user
        /// (by email) to confirmed.
        /// This means the password as well as the security question and answer and user status are set
        /// </summary>
        /// <param name="userEmail">the email of the user to be reset</param>
        /// <param name="password">the clear form password of the user which will be hashed</param>
        /// <param name="securityQuestionId">The id of the security question of the user</param>
        /// <param name="securityAnswer">The answer to th esecurity question</param>
        /// <returns>A <code>true</code> if the set is successful or <code>false</code> if the email does not match a valid user</returns>
        public bool CreateUserPassword(string userEmail, string password, int securityQuestionId, string securityAnswer)
        {
            bool retValue;
            try
            {
                int userId = GetUserIdByEmail(userEmail);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spCreateUserPassword";

                var p2 = new SqlParameter[4];
                for (int i = 0; i < 4; i++)
                    p2[i] = new SqlParameter();

                p2[0] = new SqlParameter("userId", userId);
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

            return retValue;
        }


        /// <summary>
        /// Hashes and validates the supplied old password and if corrects replaces it with the hash of the new password
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="orgId"></param>
        /// <param name="userEmail">the email of the user to be reset</param>
        /// <param name="logon">The logon name provided by the user</param>
        /// <param name="securityQuestionId">The id of the security question of the user</param>
        /// <param name="securityAnswer">The answer to teh security question</param>
        /// <param name="newSecurityAnswer"></param>
        /// <param name="oldPassword">The clear form old password of the user</param>
        /// <param name="newPassword">The new password of the user</param>
        /// <param name="newSecurityQuestionId"></param>
        /// <returns>A <code>true</code> for success or <false>for failure or an exception if password  mismatch</false></returns>
        public bool ChangePasswordEmail(string userToken, int orgId, string userEmail, string logon, int securityQuestionId, string securityAnswer, string oldPassword,
                                        string newPassword, int newSecurityQuestionId, string newSecurityAnswer)
        {
            var retValue = false;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    //get the old hashed password
                    int userId = GetUserIdByEmail(userEmail);

                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetUserByEmailSecurity";

                    var p = new SqlParameter[4];
                    for (int i = 0; i < 4; i++)
                    {
                        p[i] = new SqlParameter();
                    }

                    p[0] = new SqlParameter("orgId", orgId); 
                    p[1] = new SqlParameter("email", userEmail);
                    p[2] = new SqlParameter("securityQuestionId", securityQuestionId);
                    p[3] = new SqlParameter("securityAnswer", securityAnswer);
                    cmd.Parameters.AddRange(p);

                    SqlDataReader reader = cmd.ExecuteReader();
                    var users = new UserDataSet();
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            var user = users.User.NewUserRow();
                            user.UserId = int.Parse(reader["UserId"].ToString());
                            user.FirstName = reader["FirstName"].ToString();
                            user.LastName = reader["LastName"].ToString();
                            user.Email = reader["Email"].ToString();
                            user.Logon = reader["Logon"].ToString();
                            user.Hash = reader["Hash"].ToString();
                            user.Status = reader["Status"].ToString();
                            user.StatusId = int.Parse(reader["StatusId"].ToString());
                            user.UserGroup = reader["UserGroup"].ToString();
                            user.UserGroupId = int.Parse(reader["UserGroupId"].ToString());
                            user.Active = bool.Parse(reader["Active"].ToString());

                            users.User.AddUserRow(user);
                        }
                        reader.Close();
                    }

                    foreach (UserDataSet.UserRow v in users.User)
                    {
                        userId = v.UserId;
                        string logonD = v.Logon;

                        if (logonD.ToLower() != logon.ToLower())
                        {
                            return false;
                        }
                    }
                    //get the old hashed password
                    string oldPasswordHash = GetPasswordByUserId(userId);
                    if (oldPasswordHash != string.Empty)
                    {
                        if (!Utils.IsEqual(oldPasswordHash, oldPassword))
                            return false;
                    }

                    cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spResetUserStatus";

                    var p2 = new SqlParameter[2];
                    for (int i = 0; i < 2; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("userId", userId);
                    p2[1] = new SqlParameter("updateUser", _userId);
                    cmd.Parameters.AddRange(p2);

                    cmd.ExecuteNonQuery();

                    CreateUserPassword(userEmail, newPassword, newSecurityQuestionId, newSecurityAnswer);

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
            }
            return retValue;
        }



        /// <summary>
        /// Gets the list of authorised actions for that user
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="userGroupId">The Id of the user group whose authorisations are being sought for</param>
        /// <returns>A string list collection of the specifed user's rights</returns>
        public OperationDataSet GetAuthorizedOperations(string userToken, int userGroupId)
        {
            var retValues = new OperationDataSet();
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetAuthorizedOperations"; 
                    
                    var p2 = new SqlParameter[1];
                    for (int i = 0; i < 1; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("userGroupId", userGroupId);
                    cmd.Parameters.AddRange(p2);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var oper = retValues.Operation.NewOperationRow();
                            oper.OperationId = int.Parse(reader["OperationId"].ToString());
                            oper.OperationName = reader["Operation"].ToString();
                            oper.UserGroup_Id = reader["Id"].ToString();

                            retValues.Operation.AddOperationRow(oper);
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
            }
            return retValues;
        }


        public Dictionary<int,string> GetCategoriesByOrgId(string userToken, int orgId)
        {
            var retValues = new CategoryDataSet();
            var retDic = new Dictionary<int, string>();
            
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetCategoriesByOrgId";

                    var p2 = new SqlParameter[1];
                    for (int i = 0; i < 1; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("orgId", orgId);
                    cmd.Parameters.AddRange(p2);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
//                            var cat = retValues.Category.NewCategoryRow();
//                            cat.CategoryId = int.Parse(reader["CategoryId"].ToString());
//                            cat.Category = reader["Category"].ToString();
//
//                            retValues.Category.AddCategoryRow(cat);

                            retDic.Add(int.Parse(reader["CategoryId"].ToString()), reader["Category"].ToString());
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
            }
            return retDic;
        }


        public Dictionary<int, string> GetCategoriesByEventId(string userToken, int eventId)
        {
            //var retValues = new EventDataSet();
            var retDic = new Dictionary<int, string>();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetCategoriesByEventId";

                    var p2 = new SqlParameter[1];
                    for (int i = 0; i < 1; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("eventId", eventId);
                    cmd.Parameters.AddRange(p2);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            //                            var cat = retValues.Category.NewCategoryRow();
                            //                            cat.CategoryId = int.Parse(reader["CategoryId"].ToString());
                            //                            cat.Category = reader["Category"].ToString();
                            //
                            //                            retValues.Category.AddCategoryRow(cat);

                            retDic.Add(int.Parse(reader["CategoryId"].ToString()), reader["Category"].ToString());
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
            }
            return retDic;
        }


        public Dictionary<int,string> GetInterestsByEventId(string userToken, int eventId)
        {
            var retValues = new Dictionary<int, string>();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetInterestsByEventId";

                    var p2 = new SqlParameter[1];
                    for (int i = 0; i < 1; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("eventId", eventId);
                    cmd.Parameters.AddRange(p2);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            
                            retValues.Add(int.Parse(reader["InterestId"].ToString()),reader["Interest"].ToString());

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
            }
            return retValues;
        }


        public LocationDataSet GetLocationsByOrgId(string userToken, int orgId)
        {
            var retValues = new LocationDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetLocationsByOrgId";

                    var p2 = new SqlParameter[1];
                    for (int i = 0; i < 1; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("org_id", orgId);
                    cmd.Parameters.AddRange(p2);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var cat = retValues.Location.NewLocationRow();
                            cat.Id = int.Parse(reader["Id"].ToString());
                            cat.Name = reader["Name"].ToString();
                            cat.Latitude = double.Parse(reader["Latitude"].ToString());
                            cat.Longitude = double.Parse(reader["Longitude"].ToString());
                            cat.Country = reader["Country"].ToString();
                            cat.Town = reader["Town"].ToString();
                            cat.City = reader["City"].ToString();
                            cat.Active = bool.Parse(reader["Active"].ToString());

                            retValues.Location.AddLocationRow(cat);
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
            }
            return retValues;
        }


        public LocationDataSet GetActiveLocationsByOrgId(string userToken, int orgId)
        {
            var retValues = new LocationDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetActiveLocationsByOrgId";

                    var p2 = new SqlParameter[1];
                    for (int i = 0; i < 1; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("org_id", orgId);
                    cmd.Parameters.AddRange(p2);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var cat = retValues.Location.NewLocationRow();
                            cat.Id = int.Parse(reader["Id"].ToString());
                            cat.Name = reader["Name"].ToString();
                            cat.Latitude = double.Parse(reader["Latitude"].ToString());
                            cat.Longitude = double.Parse(reader["Longitude"].ToString());
                            cat.Country = reader["Country"].ToString();
                            cat.Town = reader["Town"].ToString();
                            cat.City = reader["City"].ToString();

                            retValues.Location.AddLocationRow(cat);
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
            }
            return retValues;
        }

       
        public OperationDataSet GetAllOperations(string userToken)
        {
            var retValues = new OperationDataSet();
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (Utilities.isSystemAdmin)
                    {
                        cmd.CommandText = "spGetAllAdminOperations";
                    }
                    else
                    {
                        cmd.CommandText = "spGetAllNonAdminOperations";
                    }

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var oper = retValues.Operation.NewOperationRow();
                            oper.OperationId = int.Parse(reader["OperationId"].ToString());
                            oper.OperationName = reader["Operation"].ToString();

                            retValues.Operation.AddOperationRow(oper);
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
            }
            return retValues;
        }


        public SysParameterDataSet GetAllInterests(string userToken)
        {
            var retValues = new SysParameterDataSet();
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetAllInterests";
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var intr = retValues.SysParameter.NewSysParameterRow();
                            intr.Id = int.Parse(reader["Id"].ToString());
                            intr.Name = reader["Name"].ToString();
                            intr.Description = reader["Description"].ToString();
                            intr.Active = bool.Parse(reader["Active"].ToString());

                            retValues.SysParameter.AddSysParameterRow(intr);
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
            }
            return retValues;
        }


        public Dictionary<int,string> GetAllSocialMediaTypes(string userToken)
        {
            var retValues = new Dictionary<int, string>();
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetAllSocialMediaTypes";

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            retValues.Add(int.Parse(reader["Id"].ToString()),reader["Name"].ToString());
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
            }
            return retValues;
        }



        /// <summary>
        /// Returns all the users in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="orgId"></param>
        /// <returns>An collection of inner variable which is an <see cref="UserDataSet"/></returns>
        public UserDataSet GetAllUsersByOrg(string userToken, int orgId)
        {

            var retDataset = new UserDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetAllUsersByOrg";

                    var p2 = new SqlParameter[1];
                    for (int i = 0; i < 1; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("orgId", orgId);
                    cmd.Parameters.AddRange(p2);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var user = retDataset.User.NewUserRow();

                            user.UserId = int.Parse(reader["UserId"].ToString());
                            user.FirstName = reader["FirstName"].ToString();
                            user.LastName = reader["LastName"].ToString();
                            user.Email = reader["Email"].ToString();
                            user.Logon = reader["Logon"].ToString();
                            user.Hash = reader["Hash"].ToString();
                            user.Status = reader["Status"].ToString();
                            user.StatusId = int.Parse(reader["StatusId"].ToString());
                            user.UserGroup = reader["UserGroup"].ToString();
                            user.UserGroupId = int.Parse(reader["UserGroupId"].ToString());
                            user.Active = bool.Parse(reader["Active"].ToString());

                            retDataset.User.AddUserRow(user);
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
            }
            return retDataset;
        }

        /// <summary>
        /// Returns all the users in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <returns>An collection of inner variable which is an <see cref="UserDataSet"/></returns>
        public UserDataSet GetAllUsers(string userToken)
        {

            var retDataset = new UserDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetAllUsers";


                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var user = retDataset.User.NewUserRow();

                            user.UserId = int.Parse(reader["UserId"].ToString());
                            user.FirstName = reader["FirstName"].ToString();
                            user.LastName = reader["LastName"].ToString();
                            user.Email = reader["Email"].ToString();
                            user.Logon = reader["Logon"].ToString();
                            user.Hash = reader["Hash"].ToString();
                            user.Status = reader["Status"].ToString();
                            user.StatusId = int.Parse(reader["StatusId"].ToString());
                            user.UserGroup = reader["UserGroup"].ToString();
                            user.UserGroupId = int.Parse(reader["UserGroupId"].ToString());
                            user.Organisation = reader["Organisation"].ToString();
                            user.OrganisationId = reader["OrganisationId"].ToString();
                            user.Active = bool.Parse(reader["Active"].ToString());

                            retDataset.User.AddUserRow(user);
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
            }
            return retDataset;


        }


        /// <summary>
        /// Returns all the countries in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="orgId"></param>
        /// <returns>An collection of inner variable which is an <see cref="UserGroupOperationDataSet"/></returns>
        public UserGroupOperationDataSet GetUserGroupOperations(string userToken, int orgId)
        {

            var retDataset = new UserGroupOperationDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetUserGroupOperations";

                    var p = new SqlParameter("orgId", orgId);
                    cmd.Parameters.Add(p);


                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var usgrpoper = retDataset.UserGroupOperation.NewUserGroupOperationRow();
                            usgrpoper.OperationId = int.Parse(reader["OperationId"].ToString());
                            usgrpoper.Operation = reader["Operation"].ToString();
                            usgrpoper.UserGroupId = int.Parse(reader["UserGroupId"].ToString());
                            usgrpoper.UserGroup = reader["UserGroup"].ToString();

                            retDataset.UserGroupOperation.AddUserGroupOperationRow(usgrpoper);
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
            }
            return retDataset;

        }



        /// <summary>
        /// This method attempts to create the status of the specified user
        /// (by email) to confirmed.
        /// This means the password as well as the security question and answer and user status are set
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="userId">the id of the user to be reset</param>
        /// <param name="password">the clear form password of the user which will be hashed</param>
        /// <param name="securityQuestionId">The id of the security question of the user</param>
        /// <param name="securityAnswer">The answer to th esecurity question</param>
        /// <returns>A <code>true</code> if the set is successful or <code>false</code> if the email does not match a valid user</returns>
        public bool UpdateUserSecurity(string userToken, int userId, string password, int securityQuestionId, string securityAnswer)
        {
            bool retValue = false;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spCreateUserPassword";

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
            }
            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool InitialisePassword(int userId, string password)
        {
            try
            {
                string newPasswordHash = Hash(password);
                if (conn.State != ConnectionState.Open) { conn.Open(); }
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spChangePassword";

                var p = new SqlParameter[2];
                for (int i = 0; i < 2; i++)
                    p[i] = new SqlParameter();

                p[0] = new SqlParameter("userId", userId);
                p[1] = new SqlParameter("hash", newPasswordHash);

                cmd.Parameters.AddRange(p);

                cmd.ExecuteNonQuery();
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
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userToken"></param>
        /// <param name="user"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public int CreateUser(string userToken, UserDataSet user, int orgId)
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spAddUser";

                    var p2 = new SqlParameter[7];
                    for (int i = 0; i < 7; i++)
                        p2[i] = new SqlParameter();

                    string firstName = user.User[0].FirstName;
                    string email = user.User[0].Email;
                    string logon = user.User[0].Logon;

                    p2[0] = new SqlParameter("firstName", firstName);
                    p2[1] = new SqlParameter("lastName", user.User[0].LastName);
                    p2[2] = new SqlParameter("email", email);
                    p2[3] = new SqlParameter("logon", logon);
                    p2[4] = new SqlParameter("orgId", orgId);
                    p2[5] = new SqlParameter("userGroupId", user.User[0].UserGroupId);
                    p2[6] = new SqlParameter("updateUser", _userId);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());

                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    #region send the email

                    string pw = Utils.GenerateRandomPassword();
                    InitialisePassword(newId, pw);

                    string messageBody =
                        "Hello " + firstName +
                        ",\n\nYour account in the " + _appName +
                        " Application needs to be confirmed.\n\nPlease find below your initial login credentials.\n\nUsername: " + logon + "\nPassword : " + pw +
                        "\n\nPlease change to a password of your choice when you log in to activate your account at " + _appURL + " .\n";
                    messageBody += "\n\nThank You.\n\n\n" + _fromName + " Team";
                    var message = new System.Net.Mail.MailMessage();
                    message.To.Add(email); //recipient address
                    message.From = new System.Net.Mail.MailAddress(_fromAddress, _fromName);
                    message.Subject = _emailSubject;

                    message.Body = messageBody;

                    var client = new System.Net.Mail.SmtpClient(_smtpServer, 587)
                    {
                        EnableSsl = true,
                        UseDefaultCredentials = false,
                        Credentials =
                            new NetworkCredential(_userName, _password)
                    };
                    client.Send(message);

                    #endregion

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
            }
            return newId;
        }


        public bool ResetPassword(string userEmail, int securityQuestionId, string securityAnswer, int orgId)
        {
            var retValue = false;
            try
            {
                if (conn.State != ConnectionState.Open) { conn.Open(); }
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetUserByEmailSecurity";

                var p = new SqlParameter[4];
                for (int i = 0; i < 4; i++)
                {
                    p[i] = new SqlParameter();
                }

                p[0] = new SqlParameter("orgId", orgId);
                p[1] = new SqlParameter("email", userEmail);
                p[2] = new SqlParameter("securityQuestionId", securityQuestionId);
                p[3] = new SqlParameter("securityAnswer", securityAnswer);
                cmd.Parameters.AddRange(p);

                string firstName = string.Empty, email = string.Empty, logon = string.Empty;

                SqlDataReader reader = cmd.ExecuteReader();
                var users = new UserDataSet();
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        var user = users.User.NewUserRow();
                        user.UserId = int.Parse(reader["UserId"].ToString());
                        user.FirstName = reader["FirstName"].ToString();
                        firstName = user.FirstName;
                        user.LastName = reader["LastName"].ToString();
                        user.Email = reader["Email"].ToString();
                        email = user.Email;
                        user.Logon = reader["Logon"].ToString();
                        logon = user.Logon;
                        user.Hash = reader["Hash"].ToString();
                        user.Status = reader["Status"].ToString();
                        user.StatusId = int.Parse(reader["StatusId"].ToString());
                        user.UserGroup = reader["UserGroup"].ToString();
                        user.UserGroupId = int.Parse(reader["UserGroupId"].ToString());
                        user.Active = bool.Parse(reader["Active"].ToString());

                        users.User.AddUserRow(user);
                    }
                    reader.Close();
                }

                foreach (UserDataSet.UserRow v in users.User)
                {
                    int userId = v.UserId;
                    cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spResetUserStatus";

                    var p2 = new SqlParameter[2];
                    for (int i = 0; i < 2; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("userId", userId);
                    p2[1] = new SqlParameter("updateUser", _userId);

                    cmd.Parameters.AddRange(p2);
                    cmd.ExecuteNonQuery();

                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    #region send the email

                    string pw = Utils.GenerateRandomPassword();
                    InitialisePassword(userId, pw);

                    string messageBody =
                        "Hello " + firstName +
                        ",\n\nYour account in " + _appName + " has been reset and needs to be reconfirmed.\n\nPlease find below your initial login credentials.\n\nUsername: " + logon + "\nPassword : " + pw +
                        "\n\nPlease change to a password of your choice when you log in to reactivate your account  at " + _appURL + " .\n";
                    messageBody += "\n\nThank You.\n\n\n" + _fromName + " Team";
                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                    message.To.Add(email); //recipient address
                    message.From = new System.Net.Mail.MailAddress(_fromAddress, _fromName);
                    message.Subject = _emailSubject;

                    message.Body = messageBody;

                    var client = new System.Net.Mail.SmtpClient(_smtpServer, 587)
                    {
                        EnableSsl = true,
                        UseDefaultCredentials = false,
                        Credentials =
                            new NetworkCredential(_userName, _password)
                    };
                    client.Send(message);

                    #endregion

                    retValue = true;
                }

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
            return retValue;
        }


        

        /// <summary>
        /// Sets the status user specified by the Id to deleted
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="userId">The userId of the user to be deleted</param>
        
        public void DeleteUser(string userToken, int userId)
        {
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spDeleteUser";

                    var p2 = new SqlParameter[2];
                    for (int i = 0; i < 2; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("id", userId);
                    p2[1] = new SqlParameter("updateUser", _userId);
                    
                    cmd.Parameters.AddRange(p2);

                    cmd.ExecuteNonQuery();

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
            }
        }

//Adwoa
        /// <summary>
        /// Updates the user's information of user without updating the password
        /// </summary>
        /// <param name="userToken">The token of the user requesting the transaction</param>
        /// <param name="userId">The userId of the user to be updated</param>
        /// <param name="password">The clear form password to be saved</param>
        public void UpdateUserPassword(string userToken, int userId, string password)
        {
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateUserPassword";

                    var p2 = new SqlParameter[3];
                    for (int i = 0; i < 3; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("id", userId);
                    p2[1] = new SqlParameter("hash", Hash(password));
                    p2[2] = new SqlParameter("updateUser", _userId);

                    cmd.Parameters.AddRange(p2);

                    cmd.ExecuteNonQuery();
                   
  
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
            }
        }

        /// <summary>
        /// Updates the user's information of user without updating the password
        /// </summary>
        /// <param name="userToken">The token of the user requesting the transaction</param>
        /// <param name="userId">The userId of the user to be updated</param>
        /// <param name="firstName">The possibly updated first name of the user</param>
        /// <param name="lastName">The possibly updated last name of the user</param>
        /// <param name="email">The possibly updated email address of the user</param>
        /// <param name="logon">The logon name of the user</param>
        /// <param name="userGroupId">The current user group of the user</param>
        /// <param name="status">The current status of the user to be updated</param>
        public int UpdateUserWithoutPassword(string userToken, int userId, string firstName, string lastName, string email, string logon, int userGroupId, int status)
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateUser";

                    var p2 = new SqlParameter[8];
                    for (int i = 0; i < 8; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("id", userId);
                    p2[1] = new SqlParameter("firstName", firstName);
                    p2[2] = new SqlParameter("lastName", lastName);
                    p2[3] = new SqlParameter("email", email);
                    p2[4] = new SqlParameter("logon", logon);
                    p2[5] = new SqlParameter("userGroupId", userGroupId);
                    p2[6] = new SqlParameter("updateUser", _userId);
                    p2[7] = new SqlParameter("status", status);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());

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
            }
            return newId;
        }
        /// <summary>
        /// Saves the specified right for ths specified user group
        /// </summary>
        /// <param name="userGroupId">The userId of the user whose authorisations are being to be stored</param>
        /// <param name="rights">The string list collection of rights the user has being assigned</param>
        public int SetRightsForUserGroup(int userGroupId, List<string> rights)
        {
            string CvsRights = string.Join(",", rights.ToArray());
            int returnValue = -1;

            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spSetRightsForUserGroup";

                var p2 = new SqlParameter[3];
                for (int i = 0; i < 3; i++)
                    p2[i] = new SqlParameter();

                p2[0] = new SqlParameter("userGroupId", userGroupId);
                p2[1] = new SqlParameter("rights", CvsRights);
                p2[2] = new SqlParameter("updateUser", _userId);

                cmd.Parameters.AddRange(p2);

                cmd.ExecuteNonQuery();

                returnValue = 34;

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
            return returnValue;
        }


        public int SetInterestsForEvent(int eventId, List<string> interests)
        {
            string CvsInterests = string.Join(",", interests.ToArray());
            int returnValue = -1;

            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spSetInterestsForEvent";

                var p2 = new SqlParameter[3];
                for (int i = 0; i < 3; i++)
                    p2[i] = new SqlParameter();

                p2[0] = new SqlParameter("eventId", eventId);
                p2[1] = new SqlParameter("interests", CvsInterests);
                p2[2] = new SqlParameter("updateUser", _userId);

                cmd.Parameters.AddRange(p2);

                cmd.ExecuteNonQuery();

                returnValue = 34;

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
            return returnValue;
        }


        /// <summary>
        /// Saves the specified category for ths specified organisation
        /// </summary>
        /// <param name="userGroupId">The orgId of the user whose categories are being to be stored</param>
        /// <param name="rights">The string list collection of categories the organisation has being assigned</param>
        public int SetCategoriesForOrganisation(int orgId, List<string> categories)
        {
            string CvsCategories = string.Join(",", categories.ToArray());
            int returnValue = -1;

            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spSetCategoriesForOrganisation";

                var p2 = new SqlParameter[3];
                for (int i = 0; i < 3; i++)
                    p2[i] = new SqlParameter();

                p2[0] = new SqlParameter("orgId", orgId);
                p2[1] = new SqlParameter("rights", CvsCategories);
                p2[2] = new SqlParameter("updateUser", _userId);

                cmd.Parameters.AddRange(p2);

                cmd.ExecuteNonQuery();

                returnValue = 34;

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
            return returnValue;
        }


        public int SetCategoriesForEvent(int eventId, List<string> categories)
        {
            string CvsCategories = string.Join(",", categories.ToArray());
            int returnValue = -1;

            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spSetCategoriesForEvent";

                var p2 = new SqlParameter[3];
                for (int i = 0; i < 3; i++)
                    p2[i] = new SqlParameter();

                p2[0] = new SqlParameter("eventId", eventId);
                p2[1] = new SqlParameter("rights", CvsCategories);
                p2[2] = new SqlParameter("updateUser", _userId);

                cmd.Parameters.AddRange(p2);

                cmd.ExecuteNonQuery();

                returnValue = 34;

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
            return returnValue;
        }


        /// <summary>
        /// Creates a user group and saves it in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="sysParameter">The system parameter to be added</param>
        /// <param name="rights">The list of authorised accesses the user has</param>
        /// <param name="orgId">The id of the organisation</param>
        /// <returns>An integer whih is the id of the newly created user</returns>
        public int AddUserGroup(string userToken, SysParameterDataSet sysParameter, List<string> rights, int orgId)
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spAddUserGroup";

                    var p2 = new SqlParameter[4];
                    for (int i = 0; i < 4; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("name", sysParameter.SysParameter[0].Name);
                    p2[1] = new SqlParameter("orgId", orgId);
                    p2[2] = new SqlParameter("description", sysParameter.SysParameter[0].Description);
                    p2[3] = new SqlParameter("updateUser", _userId);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
                    if (conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    SetRightsForUserGroup(newId, rights);


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
            }
            return newId;
        }

        /// <summary>
        /// Returns active the user groups in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        ///  <param name="orgId">The id of the organisation </param>
        /// <returns>An collection of inner variable which is an <see cref="SysParameterDataSet"/></returns>
        public SysParameterDataSet GetActiveUserGroups(string userToken, int orgId)
        {

            var retDataset = new SysParameterDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetActiveUserGroups";


                    var p = new SqlParameter("orgId", orgId);

                    cmd.Parameters.Add(p);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var sysParam = retDataset.SysParameter.NewSysParameterRow();

                            sysParam.Id = int.Parse(reader["Id"].ToString());
                            sysParam.Name = reader["Name"].ToString();
                            sysParam.Description = reader["Description"].ToString();
                            sysParam.Active = bool.Parse(reader["Active"].ToString());

                            retDataset.SysParameter.AddSysParameterRow(sysParam);
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
            }
            return retDataset;
        }


        public OrganisationDataSet GetActiveOwnerOrganisations(string userToken)
        {

            var retDataset = new OrganisationDataSet();

           if(IsUserTokenValid(userToken)){
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetActiveOwnerOrgs";


                    //var p = new SqlParameter("orgId", orgId);

                    //cmd.Parameters.Add(p);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var orgDS = retDataset.Organisation.NewOrganisationRow();

                            orgDS.Id = int.Parse(reader["Id"].ToString());
                            orgDS.Name = reader["Name"].ToString();
                            orgDS.SMSName = reader["SMSName"].ToString();
                            orgDS.Code = reader["Code"].ToString();
                            orgDS.OfficeAddress1 = reader["OfficeAddress1"].ToString();
                            orgDS.OfficeAddress2 = reader["OfficeAddress2"].ToString();
                            orgDS.OfficeAddress3 = reader["OfficeAddress3"].ToString();
                            orgDS.OfficeAddress = reader["OfficeAddress"].ToString();
                            orgDS.PostAddress1 = reader["PostAddress"].ToString();
                            orgDS.PostAddress2 = reader["PostAddress2"].ToString();
                            orgDS.PostAddress3 = reader["PostAddress3"].ToString();
                            orgDS.PostAddress = reader["PostAddress"].ToString();
                            orgDS.Telephone = reader["Telephone"].ToString();
                            orgDS.Fax = reader["Fax"].ToString();
                            orgDS.Email = reader["Email"].ToString();
                            orgDS.Active = bool.Parse(reader["Active"].ToString());
                            
                            retDataset.Organisation.AddOrganisationRow(orgDS);
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
           }
            
            return retDataset;
        }


        public OrganisationDataSet GetAllOwnerOrganisations()
        {

            var retDataset = new OrganisationDataSet();


            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetAllOwnerOrgs";


                //var p = new SqlParameter("orgId", orgId);

                //cmd.Parameters.Add(p);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var orgDS = retDataset.Organisation.NewOrganisationRow();

                        orgDS.Id = int.Parse(reader["Id"].ToString());
                        orgDS.Name = reader["Name"].ToString();
                        orgDS.SMSName = reader["SMSName"].ToString();
                        orgDS.Code = reader["Code"].ToString();
                        orgDS.OfficeAddress1 = reader["OfficeAddress1"].ToString();
                        orgDS.OfficeAddress2 = reader["OfficeAddress2"].ToString();
                        orgDS.OfficeAddress3 = reader["OfficeAddress3"].ToString();
                        orgDS.OfficeAddress = reader["OfficeAddress"].ToString();
                        orgDS.PostAddress1 = reader["PostAddress1"].ToString();
                        orgDS.PostAddress2 = reader["PostAddress2"].ToString();
                        orgDS.PostAddress3 = reader["PostAddress3"].ToString();
                        orgDS.PostAddress = reader["PostAddress"].ToString();
                        orgDS.Telephone = reader["Telephone"].ToString();
                        orgDS.Fax = reader["Fax"].ToString();
                        orgDS.Email = reader["Email"].ToString();
                        orgDS.Facebook = reader["Facebook"].ToString();
                        orgDS.Twitter = reader["Twitter"].ToString();
                        orgDS.Google = reader["Google"].ToString();
                        orgDS.Active = bool.Parse(reader["Active"].ToString());

                        retDataset.Organisation.AddOrganisationRow(orgDS);
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

        public SysParameterDataSet GetAllCategoriesByType(string userToken, bool type)
        {

            var retDataset = new SysParameterDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetAllCategoriesByType";


                    var p = new SqlParameter("type", type);

                    cmd.Parameters.Add(p);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var catDS = retDataset.SysParameter.NewSysParameterRow();

                            catDS.Id = int.Parse(reader["Id"].ToString());
                            catDS.Name = reader["Name"].ToString();
                            catDS.Description = reader["Description"].ToString();
                            catDS.Active = bool.Parse(reader["Active"].ToString());

                            retDataset.SysParameter.AddSysParameterRow(catDS);
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
            }

            return retDataset;
        }


        public SysParameterDataSet GetActiveCategoriesByType(string userToken, bool type)
        {

            var retDataset = new SysParameterDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetActiveCategoriesByType";


                    var p = new SqlParameter("type", type);

                    cmd.Parameters.Add(p);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var catDS = retDataset.SysParameter.NewSysParameterRow();

                            catDS.Id = int.Parse(reader["Id"].ToString());
                            catDS.Name = reader["Name"].ToString();
                            catDS.Description = reader["Description"].ToString();
                            catDS.Active = bool.Parse(reader["Active"].ToString());

                            retDataset.SysParameter.AddSysParameterRow(catDS);
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
            }

            return retDataset;
        }


        public SysParameterDataSet GetActiveInterests(string userToken)
        {

            var retDataset = new SysParameterDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetActiveInterests";

                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var catDS = retDataset.SysParameter.NewSysParameterRow();

                            catDS.Id = int.Parse(reader["Id"].ToString());
                            catDS.Name = reader["Name"].ToString();
                            catDS.Description = reader["Description"].ToString();
                            catDS.Active = bool.Parse(reader["Active"].ToString());

                            retDataset.SysParameter.AddSysParameterRow(catDS);
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
            }

            return retDataset;
        }

        
        

        /// <summary>
        /// This method gets the user specified by the supplied user Id
        /// </summary>
        /// <param name="userToken">The userToken of the user requesting the action</param>
        /// <param name="userId">The userId of the user whose data is being sought for</param>
        /// <returns>An inner variable which is a <see cref="UserDataSet"/></returns>
        public UserDataSet GetUserById(string userToken, int userId)
        {
            var retDataset = new UserDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetUserById";


                    var p2 = new SqlParameter[1];
                    for (int i = 0; i < 1; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("id", userId);
              
                    cmd.Parameters.AddRange(p2);


                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            var user = retDataset.User.NewUserRow();

                            user.UserId = int.Parse(reader["UserId"].ToString());
                            user.FirstName = reader["FirstName"].ToString();
                            user.LastName = reader["LastName"].ToString();
                            user.Email = reader["Email"].ToString();
                            user.Logon = reader["Logon"].ToString();
                            user.Hash = reader["Hash"].ToString();
                            user.Status = reader["Status"].ToString();
                            user.StatusId = int.Parse(reader["StatusId"].ToString());
                            user.UserGroup = reader["UserGroup"].ToString();
                            user.UserGroupId = int.Parse(reader["UserGroupId"].ToString());
                            user.Active = bool.Parse(reader["Active"].ToString());

                            retDataset.User.AddUserRow(user);
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
            }

            return retDataset;

        }

        /// <summary>
        /// Returns all the user groups in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="orgId">The id of the organisation </param>
        /// <returns>An collection of inner variable which is an <see cref="SysParameterDataSet"/></returns>
        public SysParameterDataSet GetAllUserGroups(string userToken,int orgId)
        {

            var retDataset = new SysParameterDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetAllUserGroups";

                    var p = new SqlParameter("orgId", orgId);

                    cmd.Parameters.Add(p);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var sysParam = retDataset.SysParameter.NewSysParameterRow();

                            sysParam.Id = int.Parse(reader["Id"].ToString());
                            sysParam.Name = reader["Name"].ToString();
                            sysParam.Description = reader["Description"].ToString();
                            sysParam.Active = bool.Parse(reader["Active"].ToString());

                            retDataset.SysParameter.AddSysParameterRow(sysParam);
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
            }
            return retDataset;
        }

        
        /// <summary>
        /// Sets the admin settings in the database 
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="orgId">The id of the organisation </param>
        /// <param name="settings">Filter to be applied for search</param>
        /// <returns>"An integer value which is the rowId"/></returns>
        public int SetAdminSettings(string userToken, int orgId, string settings)
        {
            int returnId = -1;

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spSetAdminSettings";

                    var p = new SqlParameter("orgId", orgId);
                    cmd.Parameters.Add(p);
                    p = new SqlParameter("settings", settings);
                    cmd.Parameters.Add(p);
                    p = new SqlParameter("updateUser", _userId);
                    cmd.Parameters.Add(p);


                    returnId = int.Parse(cmd.ExecuteScalar().ToString());
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
            }
            return returnId;
        }

        /// <summary>
        /// Gets the admin settings in the database 
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="orgId">The id of the organisation </param>
        /// <returns>"A string which contains the settings for the application"/></returns>
        public string GetAdminSettings(string userToken, int orgId)
        {
            string settings = string.Empty;

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetAdminSettings";


                    var p = new SqlParameter("orgId", orgId);
                    cmd.Parameters.Add(p);



                    settings = cmd.ExecuteScalar().ToString();
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
            }
            return settings;
        }


        /// <summary>
        /// Creates a user group and saves it in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="sysParameter">The system parameter to be added</param>
        /// <param name="rights">The list of authorised accesses the user has</param>
        /// <returns>An integer whih is the id of the newly created user</returns>
        public int UpdateUserGroup(string userToken,SysParameterDataSet sysParameter)
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateUserGroup";

                    var p2 = new SqlParameter[6];
                    for (int i = 0; i < 6; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("id", sysParameter.SysParameter[0].Id);
                    p2[1] = new SqlParameter("name", sysParameter.SysParameter[0].Name);
                    p2[2] = new SqlParameter("description", sysParameter.SysParameter[0].Description);
                    p2[3] = new SqlParameter("active", sysParameter.SysParameter[0].Active);
                    p2[4] = new SqlParameter("updateUser", _userId);
                    p2[5] = new SqlParameter("orgId", Utilities.companyId);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());

                    if(newId != -1)
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        //SetRightsForUserGroup(newId, rights);
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
            }
            return newId;
        }


        /// <summary>
        /// Creates a user group and saves it in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="user">The system parameter to be added</param>
        /// <returns>An integer whih is the id of the newly created user</returns>
        public int UpdateUser(string userToken, UserDataSet user)
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateUser";


                    var p2 = new SqlParameter[9];
                    for (int i = 0; i < 9; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("id", user.User[0].UserId);
                    p2[1] = new SqlParameter("firstName", user.User[0].FirstName);
                    p2[2] = new SqlParameter("lastName", user.User[0].LastName);
                    p2[3] = new SqlParameter("email", user.User[0].Email);
                    p2[4] = new SqlParameter("logon", user.User[0].Logon);
                    p2[5] = new SqlParameter("userGroupId", user.User[0].UserGroupId);
                    p2[6] = new SqlParameter("updateUser", _userId);
                    p2[7] = new SqlParameter("status", user.User[0].StatusId);
                    p2[8] = new SqlParameter("orgId",Utilities.companyId);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
                   

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
            }
            return newId;
        }

        public int UpdateOwnerOrganiasations(string userToken, OrganisationDataSet OrganisationDS, List<string> categories )
        {
            int newId = -1;

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateOwnerOrg";

                    var p2 = new SqlParameter[18];

                    for(int i = 0; i < 18; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("id", OrganisationDS.Organisation[0].Id);
                    p2[1] = new SqlParameter("code", OrganisationDS.Organisation[0].Code);
                    p2[2] = new SqlParameter("name", OrganisationDS.Organisation[0].Name);
                    p2[3] = new SqlParameter("offAddress1", OrganisationDS.Organisation[0].OfficeAddress1);
                    p2[4] = new SqlParameter("offAddress2", OrganisationDS.Organisation[0].OfficeAddress2);
                    p2[5] = new SqlParameter("offAddress3", OrganisationDS.Organisation[0].OfficeAddress2);
                    p2[6] = new SqlParameter("postAddress1", OrganisationDS.Organisation[0].PostAddress1);
                    p2[7] = new SqlParameter("postAddress2", OrganisationDS.Organisation[0].PostAddress2);
                    p2[8] = new SqlParameter("postAddress3", OrganisationDS.Organisation[0].PostAddress3);
                    p2[9] = new SqlParameter("telephone", OrganisationDS.Organisation[0].Telephone);
                    p2[10] = new SqlParameter("fax", OrganisationDS.Organisation[0].Fax);
                    p2[11] = new SqlParameter("email", OrganisationDS.Organisation[0].Email);
                    p2[12] = new SqlParameter("facebook", OrganisationDS.Organisation[0].Facebook);
                    p2[13] = new SqlParameter("twitter", OrganisationDS.Organisation[0].Twitter);
                    p2[14] = new SqlParameter("google", OrganisationDS.Organisation[0].Google);
                    p2[15] = new SqlParameter("active", OrganisationDS.Organisation[0].Active);
                    p2[16] = new SqlParameter("updateUser", _userId);
                    p2[17] = new SqlParameter("smsName", OrganisationDS.Organisation[0].SMSName);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
                    if (conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    SetCategoriesForOrganisation(newId, categories);
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
            }
            return newId;
        }


        public int UpdateCategory(string userToken, SysParameterDataSet CategoryDS, bool isOrg)
        {
            int newId = -1;

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateCategory";

                    var p2 = new SqlParameter[6];

                    for (int i = 0; i < 6; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("id", CategoryDS.SysParameter[0].Id);
                    p2[1] = new SqlParameter("name", CategoryDS.SysParameter[0].Name);
                    p2[2] = new SqlParameter("description", CategoryDS.SysParameter[0].Description);
                    p2[3] = new SqlParameter("active", CategoryDS.SysParameter[0].Active);
                    p2[4] = new SqlParameter("isOrg", isOrg);
                    p2[5] = new SqlParameter("updateUser", _userId);
                    
                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
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
            }
            return newId;
        }


        public int UpdateInterest(string userToken, SysParameterDataSet CategoryDS)
        {
            int newId = -1;

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateInterest";

                    var p2 = new SqlParameter[5];

                    for (int i = 0; i < 5; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("id", CategoryDS.SysParameter[0].Id);
                    p2[1] = new SqlParameter("name", CategoryDS.SysParameter[0].Name);
                    p2[2] = new SqlParameter("description", CategoryDS.SysParameter[0].Description);
                    p2[3] = new SqlParameter("active", CategoryDS.SysParameter[0].Active);
                    p2[4] = new SqlParameter("updateUser", _userId);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
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
            }
            return newId;
        }


        public int AddOwnerOrganiasation(string userToken, OrganisationDataSet OrganisationDS)
        {
            int newId = -1;

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spAddOwnerOrg";

                    var p2 = new SqlParameter[16];

                    for (int i = 0; i < 16; i++)
                        p2[i] = new SqlParameter();


                    p2[0] = new SqlParameter("name", OrganisationDS.Organisation[0].Name);
                    p2[1] = new SqlParameter("code", OrganisationDS.Organisation[0].Code);
                    p2[2] = new SqlParameter("offAddress1", OrganisationDS.Organisation[0].OfficeAddress1);
                    p2[3] = new SqlParameter("offAddress2", OrganisationDS.Organisation[0].OfficeAddress2);
                    p2[4] = new SqlParameter("offAddress3", OrganisationDS.Organisation[0].OfficeAddress3);
                    p2[5] = new SqlParameter("postAddress1", OrganisationDS.Organisation[0].PostAddress1);
                    p2[6] = new SqlParameter("postAddress2", OrganisationDS.Organisation[0].PostAddress2);
                    p2[7] = new SqlParameter("postAddress3", OrganisationDS.Organisation[0].PostAddress3);
                    p2[8] = new SqlParameter("telephone", OrganisationDS.Organisation[0].Telephone);
                    p2[9] = new SqlParameter("fax", OrganisationDS.Organisation[0].Fax);
                    p2[10] = new SqlParameter("email", OrganisationDS.Organisation[0].Email);
                    p2[11] = new SqlParameter("facebook", OrganisationDS.Organisation[0].Facebook);
                    p2[12] = new SqlParameter("twitter", OrganisationDS.Organisation[0].Twitter);
                    p2[13] = new SqlParameter("google", OrganisationDS.Organisation[0].Google);
                    p2[14] = new SqlParameter("updateUser", _userId);
                    p2[15] = new SqlParameter("smsName", OrganisationDS.Organisation[0].SMSName);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
                    if (conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    //SetCategoriesForOrganisation(newId, categories);
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
            }
            return newId;
        }


        public int AddCategory(string userToken, SysParameterDataSet catDS, bool isOrg)
        {
            int newId = -1;

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spAddCategory";

                    var p2 = new SqlParameter[5];

                    for (int i = 0; i < 5; i++)
                        p2[i] = new SqlParameter();


                    p2[0] = new SqlParameter("name", catDS.SysParameter[0].Name);
                    p2[1] = new SqlParameter("description", catDS.SysParameter[0].Description);
                    p2[2] = new SqlParameter("active", catDS.SysParameter[0].Active);
                    p2[3] = new SqlParameter("isOrg", isOrg);
                    p2[4] = new SqlParameter("updateUser", _userId);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
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
            }
            return newId;
        }


        public int AddEventSession(string userToken, EventSessionDataSet eventDS, int eventId)
        {
            int newId = -1;

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spAddEventSession";

                    var p2 = new SqlParameter[6];

                    for (int i = 0; i < 6; i++)
                        p2[i] = new SqlParameter();


                    p2[0] = new SqlParameter("name", eventDS.EventSession[0].Name);
                    p2[1] = new SqlParameter("beginTime", eventDS.EventSession[0].BeginTime);
                    p2[2] = new SqlParameter("endTime", eventDS.EventSession[0].EndTime);
                    p2[3] = new SqlParameter("eventId", eventId);
                    p2[4] = new SqlParameter("updateUser", _userId);
                    p2[5] = new SqlParameter("description", eventDS.EventSession[0].Description);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
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
            }
            return newId;
        }


        public int AddEventAlertEmail(string userToken, EmailDataSet eventDS)
        {
            int newId = -1;

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spAddEventAlertEmail";

                    var p2 = new SqlParameter[8];

                    for (int i = 0; i < 8; i++)
                        p2[i] = new SqlParameter();


                    p2[0] = new SqlParameter("subject", eventDS.Email[0].Subject);
                    p2[1] = new SqlParameter("body", eventDS.Email[0].Body);
                    p2[2] = new SqlParameter("sendDate", eventDS.Email[0].SendDate);
                    p2[3] = new SqlParameter("eventId", eventDS.Email[0].EventId);
                    p2[4] = new SqlParameter("updateUser", _userId);
                    p2[5] = new SqlParameter("isUser", eventDS.Email[0].isUser);
                    p2[6] = new SqlParameter("processed", eventDS.Email[0].Processed);
                    p2[7] = new SqlParameter("msgType", eventDS.Email[0].MessageType);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
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
            }
            return newId;
        }



        public int AddEventAlertSMS(string userToken, SMSDataSet eventDS)
        {
            int newId = -1;

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spAddEventAlertSMS";

                    var p2 = new SqlParameter[8];

                    for (int i = 0; i < 8; i++)
                        p2[i] = new SqlParameter();


                    p2[0] = new SqlParameter("smsName", eventDS.SMS[0].SMSName);
                    p2[1] = new SqlParameter("sms", eventDS.SMS[0].SMS);
                    p2[2] = new SqlParameter("sendDate", eventDS.SMS[0].SendDate);
                    p2[3] = new SqlParameter("eventId", eventDS.SMS[0].EventId);
                    p2[4] = new SqlParameter("updateUser", _userId);
                    p2[5] = new SqlParameter("isUser", eventDS.SMS[0].isUser);
                    p2[6] = new SqlParameter("processed", eventDS.SMS[0].Processed);
                    p2[7] = new SqlParameter("msgType", eventDS.SMS[0].MessageType);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
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
            }
            return newId;
        }



        public int AddEventSocialMediaPage(string userToken, SocialMediaDataSet eventDS, int eventId)
        {
            int newId = -1;

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spAddEventSocialMediaPage";

                    var p2 = new SqlParameter[5];

                    for (int i = 0; i < 5; i++)
                        p2[i] = new SqlParameter();


                    p2[0] = new SqlParameter("url", eventDS.SocialMedia[0].URL);
                    p2[1] = new SqlParameter("typeId", eventDS.SocialMedia[0].TypeId);
                    p2[2] = new SqlParameter("active", eventDS.SocialMedia[0].Active);
                    p2[3] = new SqlParameter("eventId", eventId);
                    p2[4] = new SqlParameter("updateUser", _userId);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
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
            }
            return newId;
        }


        public int AddEventCoordinator(string userToken, SysParameterDataSet eventDS, int eventId)
        {
            int newId = -1;

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spAddEventCoordinator";

                    var p2 = new SqlParameter[5];

                    for (int i = 0; i < 5; i++)
                        p2[i] = new SqlParameter();


                    p2[0] = new SqlParameter("name", eventDS.SysParameter[0].Name);
                    p2[1] = new SqlParameter("email", eventDS.SysParameter[0].Description);
                    p2[2] = new SqlParameter("active", eventDS.SysParameter[0].Active);
                    p2[3] = new SqlParameter("eventId", eventId);
                    p2[4] = new SqlParameter("updateUser", _userId);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
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
            }
            return newId;
        }


        public int AddInterest(string userToken, SysParameterDataSet intDS)
        {
            int newId = -1;

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spAddInterest";

                    var p2 = new SqlParameter[4];

                    for (int i = 0; i < 4; i++)
                        p2[i] = new SqlParameter();


                    p2[0] = new SqlParameter("name", intDS.SysParameter[0].Name);
                    p2[1] = new SqlParameter("description", intDS.SysParameter[0].Description);
                    p2[2] = new SqlParameter("active", intDS.SysParameter[0].Active);
                    p2[3] = new SqlParameter("updateUser", _userId);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
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
            }
            return newId;
        }


        public int DeactivateOwnerOrganisation(int orgId, string userToken)
        {
            int result = -1;

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spDeactivateOwnerOrg";

                   

                    var p2 = new SqlParameter[2];

                    for (int i = 0; i < 2; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("id", orgId);
                    p2[1] = new SqlParameter("updateUser", _userId);

                    cmd.Parameters.AddRange(p2);

                    result = int.Parse(cmd.ExecuteScalar().ToString());

                   
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
            }

            return result;
        }


        public int DeactivateCategory(int catId, string userToken)
        {
            int result = -1;

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spDeactivateCategory";



                    var p2 = new SqlParameter[2];

                    for (int i = 0; i < 2; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("id", catId);
                    p2[1] = new SqlParameter("updateUser", _userId);

                    cmd.Parameters.AddRange(p2);

                    cmd.ExecuteNonQuery();

                    result = 0;
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
            }

            return result;
        }

        public OrganisationDataSet GetOwnerOrganisationByID(string userToken,int orgId)
        {
            var retDataset = new OrganisationDataSet();

            if(IsUserTokenValid(userToken)){
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetOwnerOrgById";


                var p = new SqlParameter("id", orgId);

                cmd.Parameters.Add(p);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var orgDS = retDataset.Organisation.NewOrganisationRow();

                        orgDS.Id = int.Parse(reader["Id"].ToString());
                        orgDS.Name = reader["Name"].ToString();
                        orgDS.SMSName = reader["SMSName"].ToString();
                        orgDS.Code = reader["Code"].ToString();
                        orgDS.OfficeAddress1 = reader["OfficeAddress1"].ToString();
                        orgDS.OfficeAddress2 = reader["OfficeAddress2"].ToString();
                        orgDS.OfficeAddress3 = reader["OfficeAddress3"].ToString();
                        orgDS.OfficeAddress = reader["OfficeAddress"].ToString();
                        orgDS.PostAddress1 = reader["PostAddress1"].ToString();
                        orgDS.PostAddress2 = reader["PostAddress2"].ToString();
                        orgDS.PostAddress3 = reader["PostAddress3"].ToString();
                        orgDS.PostAddress = reader["PostAddress"].ToString();
                        orgDS.Telephone = reader["Telephone"].ToString();
                        orgDS.Fax = reader["Fax"].ToString();
                        orgDS.Email = reader["Email"].ToString();
                        orgDS.Facebook = reader["Facebook"].ToString();
                        orgDS.Twitter = reader["Twitter"].ToString();
                        orgDS.Google = reader["Google"].ToString();
                        orgDS.Active = bool.Parse(reader["Active"].ToString());

                        retDataset.Organisation.AddOrganisationRow(orgDS);
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
            }

            return retDataset;
        }

        public List<String> GetControlListByOperation(string userToken, int roleId)
        {
            List<string> returnList = new List<string>();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetControlsByOperation";


                    var p = new SqlParameter("id", roleId);

                    cmd.Parameters.Add(p);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        returnList.Add(reader["Control"].ToString());
                    }
                    reader.Close();
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
            }

            return returnList;

        } 

        #endregion

        #region Event Management


        /// <summary>
        /// Creates an event and saves it in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="sysParameter">The system parameter to be added</param>
        /// <returns>An integer whih is the id of the newly created user</returns>
        public int AddEvent(string userToken, EventDataSet sysParameter)
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spAddEvent";

                    var p2 = new SqlParameter[12];
                    for (int i = 0; i < 12; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("name", sysParameter.Event[0].Name);
                    p2[1] = new SqlParameter("beginDate", sysParameter.Event[0].BeginDate);
                    p2[2] = new SqlParameter("endDate", sysParameter.Event[0].EndDate);
                    p2[3] = new SqlParameter("beginTime", sysParameter.Event[0].BeginTime);
                    p2[4] = new SqlParameter("endTime", sysParameter.Event[0].EndTime);
                    p2[5] = new SqlParameter("locationId", sysParameter.Event[0].LocationId);
                    p2[6] = new SqlParameter("orgId", sysParameter.Event[0].OrganisationId);
                    p2[7] = new SqlParameter("isRoutine", sysParameter.Event[0].isRoutine);
                    p2[8] = new SqlParameter("active", sysParameter.Event[0].Active);
                    p2[9] = new SqlParameter("updateUser", _userId);
                    p2[10] = new SqlParameter("hasSession",sysParameter.Event[0].hasSession);
                    p2[11] = new SqlParameter("comments", sysParameter.Event[0].Comments);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());

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
            }
            return newId;
        }


        public int AddCurrency(string userToken, CurrencyDataSet sysParameter,int orgId)
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spAddCurrency";

                    var p2 = new SqlParameter[11];
                    for (int i = 0; i < 11; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("name", sysParameter.Currency[0].Name);
                    p2[1] = new SqlParameter("symbol", sysParameter.Currency[0].Symbol);
                    p2[2] = new SqlParameter("iso_code", sysParameter.Currency[0].ISOCode);
                    p2[3] = new SqlParameter("unitName", sysParameter.Currency[0].UnitName);
                    p2[4] = new SqlParameter("subUnitName", sysParameter.Currency[0].SubUnitName);
                    p2[5] = new SqlParameter("unitNameSingle", sysParameter.Currency[0].UnitNameSingle);
                    p2[6] = new SqlParameter("subUnitNameSingle", sysParameter.Currency[0].SubUnitNameSingle);
                    p2[7] = new SqlParameter("baseCurrency", sysParameter.Currency[0].BaseCurrency);
                    p2[8] = new SqlParameter("active", sysParameter.Currency[0].Active);
                    p2[9] = new SqlParameter("updateUser", _userId);
                    p2[10] = new SqlParameter("orgId", orgId);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());

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
            }
            return newId;
        }


        public int AddLocation(string userToken, LocationDataSet sysParameter, int orgId)
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spAddLocation";

                    var p2 = new SqlParameter[9];
                    for (int i = 0; i < 9; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("name", sysParameter.Location[0].Name);
                    p2[1] = new SqlParameter("long", sysParameter.Location[0].Longitude);
                    p2[2] = new SqlParameter("lat", sysParameter.Location[0].Latitude);
                    p2[3] = new SqlParameter("country", sysParameter.Location[0].Country);
                    p2[4] = new SqlParameter("city", sysParameter.Location[0].City);
                    p2[5] = new SqlParameter("town", sysParameter.Location[0].Town);
                    p2[6] = new SqlParameter("active", sysParameter.Location[0].Active);
                    p2[7] = new SqlParameter("updateUser", _userId);
                    p2[8] = new SqlParameter("orgId", orgId);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());

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
            }
            return newId;
        }



        /// <summary>
        /// Updates an event and saves it in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="sysParameter">The system parameter to be added</param>
        /// <returns>An integer whih is the id of the newly created user</returns>
        public int UpdateEvent(string userToken, EventDataSet sysParameter )
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateEvent";

                    var p2 = new SqlParameter[13];
                    for (int i = 0; i < 13; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("name", sysParameter.Event[0].Name);
                    p2[1] = new SqlParameter("beginDate", DateTime.Parse(sysParameter.Event[0].BeginDate));
                    p2[2] = new SqlParameter("endDate", DateTime.Parse(sysParameter.Event[0].EndDate));
                    p2[3] = new SqlParameter("beginTime", sysParameter.Event[0].BeginTime);
                    p2[4] = new SqlParameter("endTime", sysParameter.Event[0].EndTime);
                    p2[5] = new SqlParameter("locationId", sysParameter.Event[0].LocationId);
                    p2[6] = new SqlParameter("orgId", sysParameter.Event[0].OrganisationId);
                    p2[7] = new SqlParameter("isRoutine", sysParameter.Event[0].isRoutine);
                    p2[8] = new SqlParameter("active", sysParameter.Event[0].Active);
                    p2[9] = new SqlParameter("updateUser", _userId);
                    p2[10] = new SqlParameter("id",sysParameter.Event[0].Id);
                    p2[11] = new SqlParameter("hasSession",sysParameter.Event[0].hasSession);
                    p2[12] = new SqlParameter("comments", sysParameter.Event[0].Comments);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
                    if (conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    //SetInterestsForEvent(newId, interests);
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
            }
            return newId;
        }


        public int UpdateCurrency(string userToken, CurrencyDataSet sysParameter, int orgId)
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateCurrency";

                    var p2 = new SqlParameter[12];
                    for (int i = 0; i < 12; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("name", sysParameter.Currency[0].Name);
                    p2[1] = new SqlParameter("symbol", sysParameter.Currency[0].Symbol);
                    p2[2] = new SqlParameter("iso_code", sysParameter.Currency[0].ISOCode);
                    p2[3] = new SqlParameter("unitName", sysParameter.Currency[0].UnitName);
                    p2[4] = new SqlParameter("subUnitName", sysParameter.Currency[0].SubUnitName);
                    p2[5] = new SqlParameter("unitNameSingle", sysParameter.Currency[0].UnitNameSingle);
                    p2[6] = new SqlParameter("subUnitNameSingle", sysParameter.Currency[0].SubUnitNameSingle);
                    p2[7] = new SqlParameter("baseCurrency", sysParameter.Currency[0].BaseCurrency);
                    p2[8] = new SqlParameter("active", sysParameter.Currency[0].Active);
                    p2[9] = new SqlParameter("updateUser", _userId);
                    p2[10] = new SqlParameter("id", sysParameter.Currency[0].Id);
                    p2[11] = new SqlParameter("orgId", orgId);
                    
                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
                    if (conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    //SetInterestsForEvent(newId, interests);
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
            }
            return newId;
        }


        public int UpdateLocation(string userToken, LocationDataSet sysParameter, int orgId)
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateLocation";

                    var p2 = new SqlParameter[10];
                    for (int i = 0; i < 10; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("name", sysParameter.Location[0].Name);
                    p2[1] = new SqlParameter("long", sysParameter.Location[0].Longitude);
                    p2[2] = new SqlParameter("lat", sysParameter.Location[0].Latitude);
                    p2[3] = new SqlParameter("country", sysParameter.Location[0].Country);
                    p2[4] = new SqlParameter("city", sysParameter.Location[0].City);
                    p2[5] = new SqlParameter("town", sysParameter.Location[0].Town);
                    p2[6] = new SqlParameter("active", sysParameter.Location[0].Active);
                    p2[7] = new SqlParameter("updateUser", _userId);
                    p2[8] = new SqlParameter("id", sysParameter.Location[0].Id);
                    p2[9] = new SqlParameter("orgId", orgId);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
                    if (conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
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
            }
            return newId;
        }


        public int UpdateEventSession(string userToken, EventSessionDataSet sysParameter)
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateEventSession";

                    var p2 = new SqlParameter[8];
                    for (int i = 0; i < 8; i++)
                        p2[i] = new SqlParameter();

                    p2[1] = new SqlParameter("name", sysParameter.EventSession[0].Name);
                    p2[2] = new SqlParameter("beginTime", sysParameter.EventSession[0].BeginTime);
                    p2[3] = new SqlParameter("endTime", sysParameter.EventSession[0].EndTime);
                    p2[4] = new SqlParameter("active", sysParameter.EventSession[0].Active);
                    p2[5] = new SqlParameter("updateUser", _userId);
                    p2[6] = new SqlParameter("eventId", sysParameter.EventSession[0].EventId);
                    p2[0] = new SqlParameter("id", sysParameter.EventSession[0].Id);
                    p2[7] = new SqlParameter("description", sysParameter.EventSession[0].Description);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
                    
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
            }
            return newId;
        }



        public int UpdateEventAlertSMS(string userToken, SMSDataSet sysParameter)
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateEventAlertSMS";

                    var p2 = new SqlParameter[9];
                    for (int i = 0; i < 9; i++)
                        p2[i] = new SqlParameter();

                    p2[1] = new SqlParameter("smsName", sysParameter.SMS[0].SMSName);
                    p2[2] = new SqlParameter("sms", sysParameter.SMS[0].SMS);
                    p2[3] = new SqlParameter("sendDate", sysParameter.SMS[0].SendDate);
                    p2[4] = new SqlParameter("processed", sysParameter.SMS[0].Processed);
                    p2[5] = new SqlParameter("updateUser", _userId);
                    p2[6] = new SqlParameter("eventId", sysParameter.SMS[0].EventId);
                    p2[7] = new SqlParameter("isUser", sysParameter.SMS[0].isUser);
                    p2[8] = new SqlParameter("msgType", sysParameter.SMS[0].MessageType);
                    p2[0] = new SqlParameter("id", sysParameter.SMS[0].Id);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());

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
            }
            return newId;
        }



        public int UpdateEventAlertEmail(string userToken, EmailDataSet sysParameter)
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateEventAlertEmail";

                    var p2 = new SqlParameter[9];
                    for (int i = 0; i < 9; i++)
                        p2[i] = new SqlParameter();

                    p2[1] = new SqlParameter("subject", sysParameter.Email[0].Subject);
                    p2[2] = new SqlParameter("body", sysParameter.Email[0].Body);
                    p2[3] = new SqlParameter("sendDate", sysParameter.Email[0].SendDate);
                    p2[4] = new SqlParameter("processed", sysParameter.Email[0].Processed);
                    p2[5] = new SqlParameter("updateUser", _userId);
                    p2[6] = new SqlParameter("eventId", sysParameter.Email[0].EventId);
                    p2[7] = new SqlParameter("isUser", sysParameter.Email[0].isUser);
                    p2[8] = new SqlParameter("msgType", sysParameter.Email[0].MessageType);
                    p2[0] = new SqlParameter("id", sysParameter.Email[0].Id);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());

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
            }
            return newId;
        }




        public int UpdateEventSocialMedia(string userToken, SocialMediaDataSet sysParameter, int eventId)
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateEventSocialMediaPage";

                    var p2 = new SqlParameter[6];
                    for (int i = 0; i < 6; i++)
                        p2[i] = new SqlParameter();

                    p2[1] = new SqlParameter("url", sysParameter.SocialMedia[0].URL);
                    p2[2] = new SqlParameter("typeId", sysParameter.SocialMedia[0].TypeId);
                    p2[3] = new SqlParameter("active", sysParameter.SocialMedia[0].Active);
                    p2[4] = new SqlParameter("updateUser", _userId);
                    p2[5] = new SqlParameter("eventId", eventId);
                    p2[0] = new SqlParameter("id", sysParameter.SocialMedia[0].Id);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());

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
            }
            return newId;
        }


        public int UpdateEventCoordinator(string userToken, SysParameterDataSet sysParameter, int eventId)
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateEventCoordinator";

                    var p2 = new SqlParameter[6];
                    for (int i = 0; i < 6; i++)
                        p2[i] = new SqlParameter();

                    p2[1] = new SqlParameter("name", sysParameter.SysParameter[0].Name);
                    p2[2] = new SqlParameter("email", sysParameter.SysParameter[0].Description);
                    p2[3] = new SqlParameter("active", sysParameter.SysParameter[0].Active);
                    p2[4] = new SqlParameter("updateUser", _userId);
                    p2[5] = new SqlParameter("eventId",eventId);
                    p2[0] = new SqlParameter("id", sysParameter.SysParameter[0].Id);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());

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
            }
            return newId;
        }

        /// <summary>
        /// Returns the active events in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <returns>An collection of inner variable which is an <see cref="SysParameterDataSet"/></returns>
        /*public EventDataSet GetActiveEvents(string userToken)
        {

            var retDataset = new EventDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetActiveEvents";



                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var sysParam = retDataset.Event.NewEventRow();

                            sysParam.Id = int.Parse(reader["Id"].ToString());
                            sysParam.Name = reader["Name"].ToString();
                            sysParam.BeginDate = DateTime.Parse(reader["BeginDate"].ToString());
                            sysParam.EndDate = DateTime.Parse(reader["EndDate"].ToString());
                            sysParam.BeginTime = DateTime.Parse(reader["BeginTime"].ToString());
                            sysParam.EndTime = DateTime.Parse(reader["EndTime"].ToString());
                            sysParam.VenueName = reader["VenueName"].ToString();
                            sysParam.OrganizerId = int.Parse(reader["OrganizerId"].ToString());
                            sysParam.OrganizerName = reader["OrganizerName"].ToString();
                            sysParam.VenueId = int.Parse(reader["VenueId"].ToString());
                            sysParam.MaxEntries = int.Parse(reader["MaxEntries"].ToString());
                            sysParam.HasExit = (bool)reader["HasExit"];
                            sysParam.Comments = reader["Comments"].ToString();
                            sysParam.Active = (bool)reader["Active"];

                            retDataset.Event.AddEventRow(sysParam);
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
            }
            return retDataset;
        }*/

        /// <summary>
        /// Returns all events in the database for the specified orgId
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <returns>An collection of inner variable which is an <see cref="SysParameterDataSet"/></returns>
        public EventDataSet GetEventsByOrgId(string userToken,int orgId)
        {

            var retDataset = new EventDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetEventsByOrgId";

                    var p = new SqlParameter("orgId",orgId);

                    cmd.Parameters.Add(p);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var sysParam = retDataset.Event.NewEventRow();

                            sysParam.Id = int.Parse(reader["Id"].ToString());
                            sysParam.Name = reader["Name"].ToString();
                            sysParam.BeginDate = DateTime.Parse(reader["BeginDate"].ToString()).ToLongDateString();
                            sysParam.EndDate = DateTime.Parse(reader["EndDate"].ToString()).ToLongDateString();
                            sysParam.BeginTime = TimeSpan.Parse(reader["BeginTime"].ToString());
                            sysParam.EndTime = TimeSpan.Parse(reader["EndTime"].ToString());
                            sysParam.Location = reader["Location"].ToString();
                            sysParam.LocationId = int.Parse(reader["LocationId"].ToString());
                            sysParam.OrganisationId = int.Parse(reader["OrganisationId"].ToString());
                            sysParam.isRoutine = bool.Parse(reader["isRoutine"].ToString());
                            sysParam.hasSession = bool.Parse(reader["hasSession"].ToString());
                            sysParam.Active = (bool)reader["Active"];
                            sysParam.Comments = reader["Comments"].ToString();
                            

                            retDataset.Event.AddEventRow(sysParam);
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
            }
            return retDataset;
        }


        public CurrencyDataSet GetCurrencyByOrgId(string userToken, int orgId)
        {

            var retDataset = new CurrencyDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetCurrencyByOrgId";

                    var p = new SqlParameter("orgId", orgId);

                    cmd.Parameters.Add(p);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var sysParam = retDataset.Currency.NewCurrencyRow();

                            sysParam.Id = int.Parse(reader["Id"].ToString());
                            sysParam.Name = reader["Name"].ToString();
                            sysParam.Symbol = reader["Symbol"].ToString();
                            sysParam.ISOCode = reader["ISOCode"].ToString();
                            sysParam.UnitName = reader["UnitName"].ToString();
                            sysParam.SubUnitName = reader["SubUnitName"].ToString();
                            sysParam.UnitNameSingle = reader["UnitNameSingle"].ToString();
                            sysParam.SubUnitNameSingle = reader["SubUnitNameSingle"].ToString();
                            sysParam.BaseCurrency = bool.Parse(reader["BaseCurrency"].ToString());
                            sysParam.Active = bool.Parse(reader["Active"].ToString());


                            retDataset.Currency.AddCurrencyRow(sysParam);
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
            }
            return retDataset;
        }


        public CurrencyDataSet GetActiveCurrencyByOrgId(string userToken, int orgId)
        {

            var retDataset = new CurrencyDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetActiveCurrencyByOrgId";

                    var p = new SqlParameter("orgId", orgId);

                    cmd.Parameters.Add(p);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var sysParam = retDataset.Currency.NewCurrencyRow();

                            sysParam.Id = int.Parse(reader["Id"].ToString());
                            sysParam.Name = reader["Name"].ToString();
                            sysParam.Symbol = reader["Symbol"].ToString();
                            sysParam.ISOCode = reader["ISOCode"].ToString();
                            sysParam.UnitName = reader["UnitName"].ToString();
                            sysParam.SubUnitName = reader["SubUnitName"].ToString();
                            sysParam.UnitNameSingle = reader["UnitNameSingle"].ToString();
                            sysParam.SubUnitNameSingle = reader["SubUnitNameSingle"].ToString();
                            sysParam.BaseCurrency = bool.Parse(reader["BaseCurrency"].ToString());
                            sysParam.Active = bool.Parse(reader["Active"].ToString());


                            retDataset.Currency.AddCurrencyRow(sysParam);
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
            }
            return retDataset;
        }


        public EventSessionDataSet GetEventSessionsByEventId(string userToken, int eventId)
        {

            var retDataset = new EventSessionDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetEventSessionsByEventId";

                    var p = new SqlParameter("eventId", eventId);

                    cmd.Parameters.Add(p);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var sysParam = retDataset.EventSession.NewEventSessionRow();

                            sysParam.Id = int.Parse(reader["Id"].ToString());
                            sysParam.Name = reader["Name"].ToString();
                           sysParam.BeginTime = TimeSpan.Parse(reader["BeginTime"].ToString());
                            sysParam.EndTime = TimeSpan.Parse(reader["EndTime"].ToString());
                            sysParam.EventName  = reader["EventName"].ToString();
                            sysParam.EventId = int.Parse(reader["EventId"].ToString());
                            sysParam.Active = (bool)reader["Active"];
                            sysParam.Description = reader["Description"].ToString();


                            retDataset.EventSession.AddEventSessionRow(sysParam);
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
            }
            return retDataset;
        }

        public SMSDataSet GetEventAlertSMS(string userToken, int eventId, int msgType, bool isUser)
        {

            var retDataset = new SMSDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetEventAlertSMS";

                    var p2 = new SqlParameter[3];
                    for (int i = 0; i < 3; i++)
                    {
                        p2[i] = new SqlParameter();
                    }

                    p2[0] = new SqlParameter("eventId",eventId);
                    p2[1] = new SqlParameter("msgType",msgType);
                    p2[2] = new SqlParameter("isUser", isUser);

                    cmd.Parameters.AddRange(p2);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var sysParam = retDataset.SMS.NewSMSRow();

                            sysParam.Id = int.Parse(reader["Id"].ToString());
                            sysParam.SMS = reader["SMS"].ToString();
                            sysParam.SMSName = reader["SMSName"].ToString();
                            sysParam.isUser = bool.Parse(reader["isUser"].ToString());
                            sysParam.Processed = bool.Parse(reader["Processed"].ToString());
                            sysParam.EventId = int.Parse(reader["EventId"].ToString());
                            sysParam.Event = reader["Event"].ToString();
                            sysParam.SendDate = DateTime.Parse(reader["SendDate"].ToString());
                            sysParam.MessageType = int.Parse(reader["MessageType"].ToString());


                            retDataset.SMS.AddSMSRow(sysParam);
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
            }
            return retDataset;
        }


        public EmailDataSet GetEventAlertEmail(string userToken, int eventId, int msgType, bool isUser)
        {

            var retDataset = new EmailDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetEventAlertEmail";

                    var p = new SqlParameter[3];
                    for (int i = 0; i < 3; i++)
                    {
                        p[i] = new SqlParameter();
                    }

                    p[0] = new SqlParameter("eventId", eventId);
                    p[1] = new SqlParameter("msgType", msgType);
                    p[2] = new SqlParameter("isUser", isUser);

                    cmd.Parameters.AddRange(p);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var sysParam = retDataset.Email.NewEmailRow();

                            sysParam.Id = int.Parse(reader["Id"].ToString());
                            sysParam.Body = reader["Body"].ToString();
                            sysParam.Subject = reader["Subject"].ToString();
                            sysParam.isUser = bool.Parse(reader["isUser"].ToString());
                            sysParam.Processed = bool.Parse(reader["Processed"].ToString());
                            sysParam.EventId = int.Parse(reader["EventId"].ToString());
                            sysParam.Event = reader["Event"].ToString();
                            sysParam.SendDate = DateTime.Parse(reader["SendDate"].ToString());
                            sysParam.MessageType = int.Parse(reader["MessageType"].ToString());


                            retDataset.Email.AddEmailRow(sysParam);
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
            }
            return retDataset;
        }


        public SocialMediaDataSet GetSocialMediaPageByEventId(string userToken, int eventId)
        {

            var retDataset = new SocialMediaDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetSocialMediaPageByEventId";

                    var p = new SqlParameter("eventId", eventId);

                    cmd.Parameters.Add(p);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var sysParam = retDataset.SocialMedia.NewSocialMediaRow();

                            sysParam.Id = int.Parse(reader["Id"].ToString());
                            sysParam.Type = reader["Type"].ToString();
                            sysParam.TypeId = int.Parse(reader["TypeId"].ToString());
                            sysParam.URL = reader["URL"].ToString();
                            sysParam.Active = (bool)reader["Active"];


                            retDataset.SocialMedia.AddSocialMediaRow(sysParam);
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
            }
            return retDataset;
        }


        public SysParameterDataSet GetEventCoordinatorsByEventId(string userToken, int eventId)
        {

            var retDataset = new SysParameterDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetEventCoordinatorsByEventId";

                    var p = new SqlParameter("eventId", eventId);

                    cmd.Parameters.Add(p);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var sysParam = retDataset.SysParameter.NewSysParameterRow();

                            sysParam.Id = int.Parse(reader["Id"].ToString());
                            sysParam.Name = reader["Name"].ToString();
                            sysParam.Description = reader["Email"].ToString();
                            sysParam.Active = (bool)reader["Active"];


                            retDataset.SysParameter.AddSysParameterRow(sysParam);
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
            }
            return retDataset;
        }

        /// <summary>
        /// Creates an event and saves it in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="sysParameter">The system parameter to be added</param>
        /// <returns>An integer whih is the id of the newly created user</returns>
        public int AddEventTicketPrice(string userToken, EventTicketDataSet sysParameter)
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spAddEventTicketPrice";

                    var p2 = new SqlParameter[11];
                    for (int i = 0; i < 11; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("name", sysParameter.EventTicket[0].Name);
                    p2[1] = new SqlParameter("eventId", sysParameter.EventTicket[0].EventId);
                    p2[2] = new SqlParameter("price", sysParameter.EventTicket[0].Price);
                    p2[3] = new SqlParameter("currencyId", sysParameter.EventTicket[0].CurrencyId);
                    p2[4] = new SqlParameter("maxEntries", sysParameter.EventTicket[0].MaxEntries);
                    p2[5] = new SqlParameter("numDays", sysParameter.EventTicket[0].NumDays);
                    p2[6] = new SqlParameter("duplicates", sysParameter.EventTicket[0].Duplicates);
                    p2[7] = new SqlParameter("updateUser", _userId);
                    p2[8] = new SqlParameter("active", sysParameter.EventTicket[0].Active);
                    p2[9] = new SqlParameter("maxSession", sysParameter.EventTicket[0].MaxSession);
                    p2[10] = new SqlParameter("description", sysParameter.EventTicket[0].Description);

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());

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
            }
            return newId;
        }

        /// <summary>
        /// Updates an event ticket and saves it in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="sysParameter">The system parameter to be added</param>
        /// <returns>An integer whih is the id of the newly created user</returns>
        public int UpdateEventTicketPrice(string userToken, EventTicketDataSet sysParameter)
        {
            int newId = -1;
            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateEventTicketPrice";

                    var p2 = new SqlParameter[12];
                    for (int i = 0; i < 12; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("name", sysParameter.EventTicket[0].Name);
                    p2[1] = new SqlParameter("eventId", sysParameter.EventTicket[0].EventId);
                    p2[2] = new SqlParameter("price", sysParameter.EventTicket[0].Price);
                    p2[3] = new SqlParameter("currencyId", sysParameter.EventTicket[0].CurrencyId);
                    p2[4] = new SqlParameter("maxEntries", sysParameter.EventTicket[0].MaxEntries);
                    p2[5] = new SqlParameter("numDays", sysParameter.EventTicket[0].NumDays);
                    p2[6] = new SqlParameter("updateUser", _userId);
                    p2[7] = new SqlParameter("duplicates", sysParameter.EventTicket[0].Duplicates);
                    p2[8] = new SqlParameter("id", sysParameter.EventTicket[0].Id);
                    p2[9] = new SqlParameter("active", sysParameter.EventTicket[0].Active);
                    p2[10] = new SqlParameter("maxSession", sysParameter.EventTicket[0].MaxSession);
                    p2[11] = new SqlParameter("description", sysParameter.EventTicket[0].Description);


                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());

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
            }
            return newId;
        }

        /// <summary>
        /// Returns the active events in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <returns>An collection of inner variable which is an <see cref="SysParameterDataSet"/></returns>
        public EventTicketDataSet GetActiveEventTicketPrices(string userToken)
        {

            var retDataset = new EventTicketDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetActiveEventTicketPrices";



                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var sysParam = retDataset.EventTicket.NewEventTicketRow();

                            sysParam.Id = int.Parse(reader["Id"].ToString());
                            sysParam.Name = reader["Name"].ToString();
                            sysParam.Price = Decimal.Parse(reader["Price"].ToString());
                            sysParam.CurrencyId = int.Parse(reader["CurrencyId"].ToString());
                            sysParam.EventName = reader["EventName"].ToString();
                            sysParam.EventId = int.Parse(reader["EventId"].ToString());
                            sysParam.NumDays = int.Parse(reader["NumDays"].ToString());
                            sysParam.MaxEntries = int.Parse(reader["MaxEntries"].ToString());
                            sysParam.Duplicates = bool.Parse(reader["Duplicates"].ToString());
                            sysParam.Active = bool.Parse(reader["Active"].ToString());
                            sysParam.Description = reader["Description"].ToString();

                            retDataset.EventTicket.AddEventTicketRow(sysParam);
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
            }
            return retDataset;
        }

        /// <summary>
        /// Returns the active events in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <returns>An collection of inner variable which is an <see cref="SysParameterDataSet"/></returns>
        public EventTicketDataSet GetAllEventTicketPrices(string userToken)
        {

            var retDataset = new EventTicketDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetAllEventTicketPrices";



                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var sysParam = retDataset.EventTicket.NewEventTicketRow();

                            sysParam.Id = int.Parse(reader["Id"].ToString());
                            sysParam.Name = reader["Name"].ToString();
                            sysParam.Price = Decimal.Parse(reader["Price"].ToString());
                            sysParam.CurrencyId = int.Parse(reader["CurrencyId"].ToString());
                            sysParam.EventName = reader["EventName"].ToString();
                            sysParam.EventId = int.Parse(reader["EventId"].ToString());
                            sysParam.NumDays = int.Parse(reader["NumDays"].ToString());
                            sysParam.MaxEntries = int.Parse(reader["MaxEntries"].ToString());
                            sysParam.Duplicates = bool.Parse(reader["Duplicates"].ToString());
                            sysParam.Active = bool.Parse(reader["Active"].ToString());
                            sysParam.Description = reader["Description"].ToString();

                            retDataset.EventTicket.AddEventTicketRow(sysParam);
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
            }
            return retDataset;
        }

        /// <summary>
        /// Returns the active events in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        ///<param name="eventId">The id of the event</param>
        /// <returns>An collection of inner variable which is an <see cref="SysParameterDataSet"/></returns>
        public EventTicketDataSet GetAllEventTicketPricesByEvent(string userToken, int eventId)
        {

            var retDataset = new EventTicketDataSet();

            if (IsUserTokenValid(userToken))
            {
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetAllEventTicketPricesByEvent";

                    var p = new SqlParameter("eventId", eventId);

                    cmd.Parameters.Add(p);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var sysParam = retDataset.EventTicket.NewEventTicketRow();

                            sysParam.Id = int.Parse(reader["Id"].ToString());
                            sysParam.Name = reader["Name"].ToString();
                            sysParam.Price = Decimal.Parse(reader["Price"].ToString());
                            sysParam.CurrencyId = int.Parse(reader["CurrencyId"].ToString());
                            sysParam.EventName = reader["EventName"].ToString();
                            sysParam.EventId = int.Parse(reader["EventId"].ToString());
                            sysParam.NumDays = int.Parse(reader["NumDays"].ToString());
                            sysParam.MaxEntries = int.Parse(reader["MaxEntries"].ToString());
                            sysParam.Duplicates = bool.Parse(reader["Duplicates"].ToString());
                            sysParam.Active = bool.Parse(reader["Active"].ToString());
                            sysParam.MaxSession = int.Parse(reader["MaxSession"].ToString());
                            sysParam.Currency = reader["Currency"].ToString();
                            sysParam.Description = reader["Description"].ToString();

                            retDataset.EventTicket.AddEventTicketRow(sysParam);
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
            }
            return retDataset;
        }

        #endregion


    }
}