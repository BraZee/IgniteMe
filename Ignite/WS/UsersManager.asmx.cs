using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Services;
using Custom.LoggingWrapper;
using Ignite.DAL;
using Ignite.DAL.Exceptions;


namespace Ignite.WS
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://thesofttribe.com/platform/slegder/services",
        Description = "This service handles all operations associated with users: creation, update, disabling, deletion, selection")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class UsersManager : WebService
    {
        private readonly DataProvider dataProvider = DataProvider.GetInstance();

        private const string LoggingConfigNamespace = "SLedger.Web.Services.UsersManager";
        private Logger logger;

        public UsersManager()
        {
            //CODEGEN: This call is required by the ASP.NET Web Services Designer
            InitializeComponent();
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            logger = LogManager.CreateLogger(LoggingConfigNamespace);
        }


        /// <summary>
        /// This method gets the user specified by the userId
        /// </summary>
        /// <param name="userToken">The userToken of the user requesting the action</param>
        /// <param name="userId">The id of the user whose information is being sought for</param>
        /// <returns>An inner variable which is a <see cref="UserDataSet"/></returns>
        [WebMethod(Description = "This method gets the user specified by the userId")]
        public UserDataSet GetUserById(string userToken, int userId)
        {
            try
            {
                logger.Log(Level.INFO,
                           "Entering {0}. userToken={1}. userId={2}"
                           , "GetUserById"
                           , userToken
                           , userId.ToString());
                UserDataSet result = dataProvider.GetUserById(userToken, userId);
                logger.Log(Level.INFO, "Exiting {0}"
                           , "GetUserById");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetUserById. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetUserById. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetUserById. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetUserById. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }

        /// <summary>
        /// This method gets all the user groups in the system
        /// </summary>
        /// <param name="userToken">The userToken of the user requesting the action</param>
        /// <param name="orgId">The id of the organisation </param>
        /// <returns>An inner variable which is a <see cref="SysParameterDataSet"/></returns>
        [WebMethod(Description = "This method gets all the user groups in the system")]
        public SysParameterDataSet GetAllUserGroups(string userToken, int orgId)
        {
            try
            {
                logger.Log(Level.INFO,
                           "Entering {0}. userToken={1}. orgId={2}."
                           , "GetAllUserGroups"
                           , userToken
                           , orgId.ToString());
                SysParameterDataSet result = dataProvider.GetAllUserGroups(userToken, orgId);
                logger.Log(Level.INFO, "Exiting {0}"
                           , "GetAllUserGroups");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAllUserGroups. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAllUserGroups. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAllUserGroups. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAllUserGroups. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }

        /// <summary>
        /// This method gets the user groups - operations in the system
        /// </summary>
        /// <param name="userToken">The userToken of the user requesting the action</param>
        /// <param name="orgId"></param>
        /// <returns>An inner variable which is a <see cref="SysParameterDataSet"/></returns>
        [WebMethod(Description = "This method gets the user groups - operations in the system")]
        public UserGroupOperationDataSet GetUserGroupsOperations(string userToken, int orgId)
        {
            try
            {
                logger.Log(Level.INFO,
                          "Entering {0}. userToken={1}."
                          , "GetUserGroupsOperations"
                          , userToken);
                UserGroupOperationDataSet result = dataProvider.GetUserGroupOperations(userToken, orgId);
                logger.Log(Level.INFO, "Exiting {0}"
                           , "GetUserGroupsOperations");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetUserGroupsOperations. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetUserGroupsOperations. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetUserGroupsOperations. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetUserGroupsOperations. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }

        /// <summary>
        /// Creates a user and saves it in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="user">The user parameter to be added</param>
        /// <param name="orgId">The id of the organisation creating the user</param>
        /// <returns>An integer whih is the id of the newly created user</returns>
        [WebMethod(Description = "Creates a new user")]
        public int CreateUser(string userToken, UserDataSet user, int orgId)
        {
            try
            {
                logger.Log(Level.INFO,
                           "Entering {0}. userToken={1}. firstName={2}. lastName={2}. email={3}. logon={4}. orgId={5}. userGroupId={6}."
                           , "CreateUser"
                           , userToken
                           , user.User[0].FirstName
                           , user.User[0].LastName
                           , user.User[0].Email
                           , user.User[0].Logon
                           , orgId.ToString()
                           , user.User[0].UserGroupId);

                int result = dataProvider.CreateUser(userToken, user, orgId);

                logger.Log(Level.INFO, "Exiting {0}"
                           , "CreateUser");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in CreateUser. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in CreateUser. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in CreateUser. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in CreateUser. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }

        /// <summary>
        /// Creates a user group and saves it in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="sysParameter">The system parameter to be added</param>
        /// <param name="rights">The list of authorised accesses the user group has</param>
        /// <param name="orgId">The id of the organisation </param>
        /// <returns>An integer which is the id of the newly created user</returns>
        [WebMethod(Description = "Adds a user group along with its rights into the database")]
        public int AddUserGroup(string userToken, SysParameterDataSet sysParameter,
                              List<string> rights, int orgId)
        {
            try
            {
                string rightString = string.Empty;
                foreach (string s in rights)
                {
                    if (rightString == string.Empty)
                    {
                        rightString = s;
                    }
                    else
                    {
                        rightString = rightString + "," + s;
                    }
                }
                logger.Log(Level.INFO,
                           "Entering {0}. userToken={1}. name={2}. description={2}. rights={3}. orgId={4}."
                           , "AddUserGroup"
                           , userToken
                           , sysParameter.SysParameter[0].Name
                           , sysParameter.SysParameter[0].Description
                           , rightString
                           , orgId.ToString());

                int result = dataProvider.AddUserGroup(userToken, sysParameter, rights, orgId);

                logger.Log(Level.INFO, "Exiting {0}"
                           , "AddUserGroup");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in AddUserGroup. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in AddUserGroup. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in AddUserGroup. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in AddUserGroup. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }


        /// <summary>
        /// Updates the user's password and saves it in the database
        /// </summary>
        /// <param name="userId">The id of the user to be updated</param>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="oldPassword">The clear form of the old password</param>
        /// <param name="newPassword">The clear form of the new password</param>
        [WebMethod(Description = "This method attempst to chnage the old password to the new for the specified user")]
        public bool ChangePassword(string userToken, int userId, string oldPassword, string newPassword)
        {
            try
            {
                logger.Log(Level.INFO,
                           "Entering {0}. userToken={1}. userId={2}. oldPassword={3}. newPassword={4}."
                           , "ChangePassword"
                           , userToken
                           , userId
                           , "*****"
                           , "*****");

                bool result = dataProvider.ChangePassword(userToken, userId, oldPassword, newPassword);

                logger.Log(Level.INFO, "Exiting {0}"
                           , "ChangePassword");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in ChangePassword. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in ChangePassword. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in ChangePassword. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in ChangePassword. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }

        /// <summary>
        /// This method gets the active user groups in the system
        /// </summary>
        /// <param name="userToken">The userToken of the user requesting the action</param>
        /// <param name="orgId">The id of the organisation </param>
        /// <returns>An inner variable which is a <see cref="SysParameterDataSet"/></returns>
        [WebMethod(Description = "This method gets the active user groups in the system")]
        public SysParameterDataSet GetActiveUserGroups(string userToken, int orgId)
        {
            try
            {
                logger.Log(Level.INFO,
                           "Entering {0}.{1}. userToken={2}. orgId={3}."
                           , "GetActiveUserGroups"
                           , userToken
                           , orgId.ToString());
                SysParameterDataSet result = dataProvider.GetActiveUserGroups(userToken, orgId);
                logger.Log(Level.INFO, "Exiting {0}."
                           , "GetActiveUserGroups");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetActiveUserGroups. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetActiveUserGroups. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetActiveUserGroups. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetActiveUserGroups. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }

        /// <summary>
        /// This method gets all the operations allowed in the system
        /// </summary>
        /// <param name="userToken">The userToken of the user requesting the action</param>
        /// <returns>An inner variable which is a <see cref="OperationDataSet"/></returns>
        [WebMethod(Description = "This method gets all the operations allowed in the system")]
        public OperationDataSet GetAllOperations(string userToken)
        {
            try
            {
                logger.Log(Level.INFO,
                              "Entering {0}. userToken={1}."
                              , "GetAllOperationsClient"
                              , userToken);
                OperationDataSet result = dataProvider.GetAllOperations(userToken);

                logger.Log(Level.INFO, "Exiting {0}"
                           , "GetAllOperationsClient");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAllOperationsClient. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAllOperationsClient. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAllOperationsClient. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAllOperationsClient. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the Security Questions asked of the user for password resetting
        /// </summary>
        /// <returns>An inner variable of type <see cref="SecurityQuestionDataSet"/></returns>
        [WebMethod(Description = "Retrieves data for the Security Questions asked of the user for password resetting", CacheDuration = 60)]
        public SecurityQuestionDataSet GetAllSecurityQuestions()
        {
            try
            {
                logger.Log(Level.INFO,
                              "Entering {0}. userToken={1}. userId={2}."
                              , "GetAllSecurityQuestions");
                SecurityQuestionDataSet result = dataProvider.GetAllSecurityQuestions();

                logger.Log(Level.INFO, "Exiting {0}"
                           , "GetAllSecurityQuestions");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAllSecurityQuestions. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAllSecurityQuestions. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAllSecurityQuestions. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAllSecurityQuestions. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// This method gets all user known to the system
        /// </summary>
        /// <param name="userToken">The userToken of the user requesting the action</param>
        /// <param name="orgId">The organisation whose users are required</param>
        /// <returns>An inner variable which is a <see cref="UserDataSet"/></returns>
        [WebMethod(Description = "This method gets all user known for to the system a given orgainsation ")]
        public UserDataSet GetUsersByOrg(string userToken, int orgId)
        {
            try
            {
                logger.Log(Level.INFO,
                           "Entering {0}. userToken={1}. orgId={2}."
                           , "GetUsers"
                           , userToken
                           , orgId.ToString());

                UserDataSet result = dataProvider.GetAllUsersByOrg(userToken, orgId);
                logger.Log(Level.INFO, "Exiting {0}"
                           , "GetUsers");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetUsers. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetUsers. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetUsers. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetUsers. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }

        /// <summary>
        /// Updates the specified user group and saves it in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="sysParameter">The system parameter to be added</param>
        /// <param name="rights">The list of authorised accesses the user group has</param>
        /// <returns>An integer which is the id of the newly created user</returns>
        [WebMethod(Description = "Updates the specified user group and saves it in the database")]
        public int UpdateUserGroup(string userToken, SysParameterDataSet sysParameter,
                              List<string> rights)
        {
            try
            {
                string rightString = string.Empty;
                foreach (string s in rights)
                {
                    if (rightString == string.Empty)
                    {
                        rightString = s;
                    }
                    else
                    {
                        rightString = rightString + "," + s;
                    }
                }

                logger.Log(Level.INFO,
                           "Entering {0}. userToken={1}. name={2}. description={2}. id={3}"
                           , "UpdateUserGroup"
                           , userToken
                           , sysParameter.SysParameter[0].Name
                           , sysParameter.SysParameter[0].Description
                           , sysParameter.SysParameter[0].Id);

                int result = dataProvider.UpdateUserGroup(userToken, sysParameter);
                logger.Log(Level.INFO, "Exiting {0}"
                           , "UpdateUserGroup");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUserGroup. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUserGroup. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUserGroup. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUserGroup. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }


        /// <summary>
        /// Updates the specified user group and saves it in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="user">The system parameter to be added</param>
        /// <returns>An integer which is the id of the newly created user</returns>
        [WebMethod(Description = "Updates the specified user group and saves it in the database")]
        public int UpdateUser(string userToken, UserDataSet user)
        {
            try
            {
                logger.Log(Level.INFO,
                           "Entering {0}. userToken={1}. userId={2}. firstName={3}. lastName={4}. email={5}. logon={6}. userGroupId={7}. status={8}"
                           , "UpdateUser"
                           , userToken
                           , user.User[0].UserId
                           , user.User[0].FirstName
                           , user.User[0].LastName
                           , user.User[0].Email
                           , user.User[0].Logon
                           , user.User[0].UserGroupId
                           , user.User[0].StatusId);

                int result = dataProvider.UpdateUser(userToken, user);
                logger.Log(Level.INFO, "Exiting {0}"
                           , "UpdateUser");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUser. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUser. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUser. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUser. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }

        /// <summary>
        /// Updates the user and saves it in the database
        /// </summary>
        /// <param name="userId">The id of the user to be updated</param>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="firstName">The first name of the user to be created</param>
        /// <param name="lastName">The last name of the user to be created</param>
        /// <param name="email">The email address of the user to be created</param>
        /// <param name="logon">The logon name of the user to be created</param>
        /// <param name="userGroupId">The user group the user belongs to</param>
        /// <param name="status">The current status of the user to be updated</param>
        /// <returns>An integer which is the id of the newly created user</returns>
        [WebMethod(Description = "This method updates the information of the specified user as provided")]
        public int UpdateUserWihoutPassword(string userToken, int userId, string firstName, string lastName, string email, string logon, int userGroupId, int status)
        {
            try
            {
                logger.Log(Level.INFO,
                           "Entering {0}. userToken={1}. firstName={2}. lastName={2}. email={3}. logon={4}. userGroupId={5}. userId={6}. status={7}"
                           , "UpdateUserWihoutPassword"
                           , userToken
                           , firstName
                           , lastName
                           , email
                           , logon
                           , userGroupId
                           , userId
                           , status);

                int result = dataProvider.UpdateUserWithoutPassword(userToken, userId, firstName, lastName, email, logon,
                                                                   userGroupId, status);

                logger.Log(Level.INFO, "Exiting {0}"
                           , "UpdateUserWihoutPassword");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUserWihoutPassword. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUserWihoutPassword. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUserWihoutPassword. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUserWihoutPassword. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }


        /// <summary>
        /// Updates the user's password and saves it in the database
        /// </summary>
        /// <param name="userId">The id of the user to be updated</param>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="password">The clear for m password</param>
        [WebMethod(Description = "This method updates the password of the specified user as provided")]
        public void UpdateUserPassword(string userToken, int userId, string password)
        {
            try
            {
                logger.Log(Level.INFO,
                             "Entering {0}. userToken={1}. userId={2}. password={3}"
                             , "UpdateUserPassword"
                             , userToken
                             , userId
                             , "*******");

                dataProvider.UpdateUserPassword(userToken, userId, password);

                logger.Log(Level.INFO, "Exiting {0}"
                           , "UpdateUserPassword");
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUserPassword. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUserPassword. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUserPassword. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUserPassword. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }

        /// <summary>
        /// This method attempts to set the status of the specified user
        /// (by email) to confirmed.
        /// This means the password as well as the security question and answer and user status are reset
        /// </summary>
        /// <param name="userToken">The userToken of the user requesting the action. Will be an administrator</param>
        /// <param name="userId">The id of the user whose password is to be reset</param>
        /// <param name="securityQuestionId">The id of the security question the user provides</param>
        /// <param name="securityAnswer">The answer to the security question</param>
        /// <param name="password">The clear form password of the user which will be hashed</param>
        /// <returns>A <code>true</code> or <code>false</code> indicating success or failure</returns>
        [WebMethod(Description = "This method attempts to set the status of the specified user. The password as well as the security question and answer and user status are set")]
        public bool UpdateUserSecurity(string userToken, int userId, string password, int securityQuestionId, string securityAnswer)
        {
            try
            {
                logger.Log(Level.INFO,
                           "Entering {0}. userToken={1}. userId={2}. password={3}. securityQuestionId={4}. securityAnswer={4}."
                           , "UpdateUserSecurity"
                           , userToken
                           , userId
                           , "*****"
                           , securityQuestionId
                           , securityAnswer);

                bool result = dataProvider.UpdateUserSecurity(userToken, userId, password, securityQuestionId, securityAnswer);

                logger.Log(Level.INFO, "Exiting {0}"
                           , "UpdateUserSecurity");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUserSecurity. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUserSecurity. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUserSecurity. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in UpdateUserSecurity. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }

        /// <summary>
        /// This method disables the user from the system
        /// </summary>
        /// <param name="userToken">The userToken of the user requesting the action. Will be an administrator</param>
        /// <param name="userId">The id of the user to be disabled</param>
        [WebMethod(Description = "This method disables the user from the system in the system.  User is marked as inactive")]
        public void DisableUser(string userToken, int userId)
        {
            try
            {
                logger.Log(Level.INFO,
                              "Entering {0}. userToken={1}. userId={2}."
                              , "DisableUser"
                              , userToken
                              , userId);

                dataProvider.DeleteUser(userToken, userId);

                logger.Log(Level.INFO, "Exiting {0}"
                           , "DisableUser");
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in DisableUser. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in DisableUser. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in DisableUser. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in DisableUser. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }

        /// <summary>
        /// This method gets all the authorised operations allowed for the specified user group
        /// </summary>
        /// <param name="userToken">The userToken of the user requesting the action</param>
        /// <param name="userGroupId">The user group whose allowed actions are being sought for</param>
        /// <returns>An inner variable which is a <see cref="OperationDataSet"/></returns>
        [WebMethod(Description = "This method gets all the authorised operations allowed for the specified user group")]
        public OperationDataSet GetAuthorizedOperations(string userToken, int userGroupId)
        {
            try
            {
                logger.Log(Level.INFO,
                              "Entering {0}. userToken={1}. userGroupId={2}."
                              , "GetAuthorizedOperations"
                              , userToken
                              , userGroupId);
                OperationDataSet result = dataProvider.GetAuthorizedOperations(userToken, userGroupId);

                logger.Log(Level.INFO, "Exiting {0}"
                           , "GetAuthorizedOperations");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAuthorizedOperations. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAuthorizedOperations. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAuthorizedOperations. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAuthorizedOperations. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }

        /// <summary>
        /// This method gets all user known to the system
        /// </summary>
        /// <param name="userToken">The userToken of the user requesting the action</param>
        /// <returns>An inner variable which is a <see cref="UserDataSet"/></returns>
        [WebMethod(Description = "This method gets all user known to the system")]
        public UserDataSet GetUsers(string userToken)
        {
            try
            {
                logger.Log(Level.INFO,
                           "Entering {0}. userToken={1}."
                           , "GetUsers"
                           , userToken);
                UserDataSet result = dataProvider.GetAllUsers(userToken);
                logger.Log(Level.INFO, "Exiting {0}"
                           , "GetUsers");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetUsers. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetUsers. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetUsers. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetUsers. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }

    }
}
