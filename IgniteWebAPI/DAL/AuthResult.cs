using System;

namespace IgniteWebAPI.DAL
{
    [Serializable]
    public class AuthResult
    {

        /// <summary>
        /// Describes the the error message or additional info from the request response
        /// </summary>
        private string _extraMessage;

        private string _userFullName;
        /// <summary>
        /// An enum for further descbing the _success 
        /// </summary>
        private LoginReturn _returnCode;

        /// <summary>
        /// Specifies whether the login was successful or not
        /// </summary>
        private bool _success;

        /// <summary>
        /// System Identifier of the user that logged on
        /// </summary>
        private string _userId;

        /// <summary>
        /// Token returned based on userId and authentication and security parameters
        /// </summary>
        private string _userToken;

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public AuthResult() : this(false,
                                   string.Empty,
                                   LoginReturn.WrongCredentials,
                                   string.Empty,
                                   string.Empty)
        {
        }

        /// <summary>
        /// Parametered Constuctor
        /// </summary>
        /// <param name="success">Specifies whether the attempt successful was ok or not</param>
        /// <param name="extraMessage">Describes the the error message or additional info from the request response</param>
        /// <param name="returnCode">An enum for further descbing the _success </param>
        /// <param name="userToken">Token returned based on userId and authentication and security parameters</param>
        /// <param name="userFullName">Full name returned based on userId and authentication and security parameters</param>
        public AuthResult(bool success,
                          string extraMessage,
                          LoginReturn returnCode,
                          string userToken,
                          string userFullName)
        {
            _success = success;
            _extraMessage = extraMessage;
            _returnCode = returnCode;
            _userToken = userToken;
            _userFullName = userFullName;
        }

        /// <summary>
        /// Gets or Sets the success or failure status of the login request
        /// </summary>
        /// <value>A boolean value (true or false)</value>
        public bool Success
        {
            get { return _success; }
            set { _success = value; }
        }

        /// <summary>
        /// Gets or Sets the extra message that accompanies the Login Result
        /// </summary>
        /// <value>A simple, literal, customer-facing string</value>
        public string ExtraMessage
        {
            get { return _extraMessage; }
            set { _extraMessage = value; }
        }

        /// <summary>
        /// Gets or Sets the return code for the login result
        /// </summary>
        /// <value>A enumerated value that defines the result of the login request</value>
        public LoginReturn ReturnCode
        {
            get { return _returnCode; }
            set { _returnCode = value; }
        }

        /// <summary>
        /// Gets or Set the user token used for the login attempt
        /// </summary>
        /// <value>An inner variable</value>
        public string UserToken
        {
            get { return _userToken; }
            set { _userToken = value; }
        }

        /// <summary>
        /// Gets or Sets the system Identifier of the user that logged on
        /// </summary>
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>
        /// Gets or Sets the user's full names
        /// </summary>
        public string UserFullName
        {
            get { return _userFullName; }
            set { _userFullName = value; }
        }
    }
}