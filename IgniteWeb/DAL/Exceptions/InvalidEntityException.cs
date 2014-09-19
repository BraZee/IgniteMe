using System;
using System.Runtime.Serialization;

namespace IgniteWeb.DAL.Exceptions
{
    /// <summary>
    /// Summary description for InvalidEntityException.
    /// </summary>
    [Serializable]
    public class InvalidEntityException : Exception
    {
        private const string IdentifierSerializationField = "FaultyIdentifier";

        protected string _entityIdentifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEntityException"/> class by
        /// providing the faulty pieces of data used to identify the Entity.
        /// </summary>
        /// <param name="message">Explanatory message</param>
        /// <param name="entityIdentifier">(possibly null) Identifier used to retrieve the Entity</param>
        protected InvalidEntityException(string message, string entityIdentifier)
            : base(message)
        {
            _entityIdentifier = entityIdentifier;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEntityException"/> class.
        /// </summary>
        /// <param name="message">Explanatory message</param>
        /// <param name="entityIdentifier">(possibly null) Identifier used to retrieve the Entity</param>
        /// <param name="inner">Exception that caused the current instance to be triggered</param>
        protected InvalidEntityException(string message, string entityIdentifier, Exception inner)
            : base(message, inner)
        {
            _entityIdentifier = entityIdentifier;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEntityException"/> class.
        /// </summary>
        /// <param name="message">Explanatory message</param>
        /// <param name="inner">Exception that caused the current instance to be triggered</param>
        protected InvalidEntityException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected InvalidEntityException(SerializationInfo info, StreamingContext context)
        {
            _entityIdentifier = info.GetString(IdentifierSerializationField);
        }

        #region ISerializable members
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(IdentifierSerializationField, _entityIdentifier);

        }
        #endregion
    }
}