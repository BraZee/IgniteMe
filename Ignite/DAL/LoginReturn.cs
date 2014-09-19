using System;

namespace Ignite.DAL
{
    /// <summary>
    /// Enumerator for the Login Return
    /// </summary>
    [Serializable]
    public enum LoginReturn { Success, Warning, ExpiredCredentials, WrongCredentials, InvalidContext, InactiveUser, DeletedUser }
}