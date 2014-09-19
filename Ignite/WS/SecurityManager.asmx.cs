using System;
using System.ComponentModel;
using System.Web.Services;
using Custom.LoggingWrapper;
using Ignite.DAL;
using Ignite.DAL.Exceptions;



namespace Ignite.WS
{
    /// <summary>
    /// Summary description for SecurityManager
    /// </summary>
    [WebService(Namespace = "http://thesofttribe.com/platform/slegder/services",
        Description = "This service handles all operations asscoiated with access and access rights in the system")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class SecurityManager : WebService
    {

        private readonly DataProvider dataProvider = DataProvider.GetInstance();

        private const string LoggingConfigNamespace = "SLedger.Web.Services.SecurityManager";
        private Logger logger;

        public SecurityManager()
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
        /// This method authenticates the login credentials passed
        /// </summary>        
        /// <param name="orgId"></param>
        /// <param name="logon">The clear form logon name of the user</param>
        /// <param name="password">The clear form password of the user</param>
        /// <returns>An inner varialbe which is a <see cref="AuthResult"/>se</returns>
        [WebMethod(Description = "This method authenticates the login credentials passed")]
        public AuthResult Authenticate(int orgId, string logon, string password)
        {
            try
            {
                logger.Log(Level.INFO, "Entering {0}. logon={1}. password={2}."
                                  , "Authenticate"
                                  , logon
                                  , "***");
                AuthResult result = dataProvider.Authenticate(orgId, logon, password);
                logger.Log(Level.INFO, "Exiting {0}"
                                  , "Authenticate");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in Authenticate. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in Authenticate. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in Authenticate. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in Authenticate. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// This method logout the user form the system. It changes the status of the user in the system
        /// </summary>
        /// <param name="userToken">The suer token of the user to be logged out</param>
        [WebMethod(Description = "This method logout the user form the system. It changes the status of the user in the system")]
        public void Logout(string userToken)
        {
            dataProvider.Logout(userToken);
        }

        /// <summary>
        /// This method attempts to reset the status of the specified user
        /// (by email, security question and answer) to not confirmed.
        /// This means the password as well as the security question and answer and user status are reset
        /// </summary>
        /// <param name="userEmail">The email of the user whose password is to be reset</param>
        /// <param name="securityQuestionId">The id of the security question the user provides</param>
        /// <param name="securityAnswer">The answer to the security question</param>
        /// <param name="orgId"></param>
        /// <returns>A <code>true</code> or <code>false</code> indicating success or failure</returns>
        [WebMethod(Description = "This method attempts to reset the status of the specified user. The password as well as the security question and answer and user status are reset")]
        public bool ResetPassword(string userEmail, int securityQuestionId, string securityAnswer, int orgId)
        {
            try
            {
                logger.Log(Level.INFO,
                           "Entering {0}. userEmail={1}. securityQuestionId={2}. securityAnswer={3}.orgId={4}."
                            , "ResetPassword"
                           , userEmail
                           , securityQuestionId.ToString()
                           , securityAnswer);
                bool result = dataProvider.ResetPassword( userEmail, securityQuestionId, securityAnswer, orgId);
                logger.Log(Level.INFO, "Exiting {0}"
                           , "ResetPassword");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in ResetPassword. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in ResetPassword. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in ResetPassword. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in ResetPassword. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Creates a data tracking entry and saves it in the database
        /// </summary>
        /// <param name="userToken">The userToken of the user performing the request</param>
        /// <param name="entityName"></param>
        /// <param name="parameterId"></param>
        /// <param name="fieldName"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns>An integer which is the id of the newly created data entry & tracking</returns>
        [WebMethod(Description = "Creates a data tracking entry and saves it in the database")]
        public int AddDataEntryTracking(string userToken, string entityName, int parameterId, string fieldName, string oldValue,
            string newValue)
        {
            try
            {
                logger.Log(Level.INFO, "Entering {0}. entityName={1}. parameterId={2}. fieldName={3}. oldValue={4}. newValue={5}."
                                  , "AddDataEntryTracking"
                                  , entityName
                                  , parameterId
                                  , fieldName
                                  , oldValue
                                  , newValue);
              //  var result = dataProvider.AddDataEntryTracking(userToken, entityName, parameterId, fieldName, oldValue, newValue);
                var result = int.Parse(string.Empty); //remove
                logger.Log(Level.INFO, "Exiting {0}"
                                  , "AddDataEntryTracking");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in AddDataEntryTracking. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in AddDataEntryTracking. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in AddDataEntryTracking. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in AddDataEntryTracking. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, ex.InnerException);
            }  
        }
        /// <summary>
        /// This method attempts to set the status of the specified user
        /// (by email) to confirmed.
        /// This means the password as well as the security question and answer and user status are reset
        /// </summary>
        /// <param name="userEmail">The email of the user whose password is to be reset</param>
        /// <param name="securityQuestionId">The id of the security question the user provides</param>
        /// <param name="securityAnswer">The answer to the security question</param>
        /// <param name="password">The clear form password of the user which will be hashed</param>
        /// <returns>A <code>true</code> or <code>false</code> indicating success or failure</returns>
        [WebMethod(Description = "This method attempts to set the status of the specified user. The password as well as the security question and answer and user status are set")]
        public bool CreateUserPassword(string userEmail, string password, int securityQuestionId, string securityAnswer)
        {
            try
            {
                logger.Log(Level.INFO,
                              "Entering {0}. userEmail={1}. securityQuestionId={2}. securityAnswer={3}."
                              , "CreateUserPassword"
                              , userEmail
                              , securityQuestionId.ToString()
                              , securityAnswer);

                bool result = dataProvider.CreateUserPassword(userEmail, password, securityQuestionId, securityAnswer);

                logger.Log(Level.INFO, "Exiting {0}"
                           , "CreateUserPassword");

                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in CreateUserPassword. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in CreateUserPassword. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in CreateUserPassword. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in CreateUserPassword. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// This method returns the list of authorised rights the user with the userId provided has
        /// </summary>
        /// <param name="userToken">The user token of the user requesting the action. May be an administrator</param>
        /// <param name="userGroupId">The id of the user group whose rights are being sought for</param>
        /// <returns>A string list of the rights given the user</returns>
        [WebMethod(Description = "This method returns the list of authorised rights the user with the userId provided has")]
        public OperationDataSet GetAuthorizedActions(string userToken, int userGroupId)
        {
            try
            {
                logger.Log(Level.INFO,
                           "Entering {0}. userToken={1}. userGroupId={2}."
                           , "GetAuthorizedActions"
                           , userToken
                           , userGroupId.ToString());
                OperationDataSet result = dataProvider.GetAuthorizedOperations(userToken, userGroupId);
                logger.Log(Level.INFO, "Exiting {0}.{1}"
                           , "GetAuthorizedActions");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAuthorizedActions. Exception={0}. Message: {1}."
                           , "ArgumentNullException"
                           , ex);
                throw new ArgumentNullException(ex.ParamName, ex.InnerException);
            }
            catch (InvalidUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAuthorizedActions. Exception={0}. Message: {1}."
                           , "InvalidUserTokenException"
                           , ex);
                throw new InvalidUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (ExpiredUserTokenException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAuthorizedActions. Exception={0}. Message: {1}."
                           , "ExpiredUserTokenException"
                           , ex);
                throw new ExpiredUserTokenException(ex.Message, userToken, ex.InnerException);
            }
            catch (OperationFailedException ex)
            {
                logger.Log(Level.ERROR, "Exception thrown in GetAuthorizedActions. Exception={0}. Message: {1}."
                           , "OperationFailedException"
                           , ex);
                throw new OperationFailedException(ex.Message, userToken, ex.InnerException);
            }
        }

    }
}
