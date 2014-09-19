using System;
using System.Runtime.Serialization;

namespace IgniteWebAPI.DAL.Exceptions
{
    [Serializable]
    public class OperationFailedException : InvalidEntityException, ISerializable
    {
        /// <summary>
        /// System identifier of the User that was used in an operation
        /// while being invalid.
        /// </summary>
        public string UserToken
        {
            get
            {
                return _entityIdentifier;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationFailedException"/> class.
        /// </summary>
        public OperationFailedException()
            : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationFailedException"/> class.
        /// </summary>
        /// <param name="message">Explanatory message</param>
        public OperationFailedException(string message)
            : base(message, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationFailedException"/> class by
        /// providing the faulty pieces of data used to identify the Entity.
        /// </summary>
        /// <param name="message">Explanatory message</param>
        /// <param name="userToken">(possibly null) Token used to retrieve the User</param>
        public OperationFailedException(string message, string userToken)
            : base(message, userToken)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationFailedException"/> class.
        /// </summary>
        /// <param name="message">Explanatory message</param>
        /// <param name="userToken">(possibly null) Token used to retrieve the User</param>
        /// <param name="inner">Exception that caused the current instance to be triggered</param>
        public OperationFailedException(string message, string userToken, Exception inner)
            : base(message, userToken, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationFailedException"/> class.
        /// </summary>
        /// <param name="message">Explanatory message</param>
        /// <param name="inner">Exception that caused the current instance to be triggered</param>
        public OperationFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected OperationFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}