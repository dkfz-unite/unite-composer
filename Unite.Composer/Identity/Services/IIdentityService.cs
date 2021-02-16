namespace Unite.Composer.Identity.Services
{
    public interface IIdentityService<TIdentity>
    {
        /// <summary>
        /// Retrieves user identity by login 
        /// </summary>
        /// <param name="login">Login</param>
        /// <returns>Identity of user if was found. Null otherwise.</returns>
        TIdentity FindUser(string login);

        /// <summary>
        /// Registers new user if given login doesn't exist yet
        /// </summary>
        /// <param name="login">Login</param>
        /// <param name="password">Password</param>
        /// <returns>Identity of registered user in case of success. Null otherwise.</returns>
        TIdentity SignUpUser(string login, string password);

        /// <summary>
        /// Verifies if user credentials match
        /// </summary>
        /// <param name="login">Login</param>
        /// <param name="password">Password</param>
        /// <returns>Identity of signed in user in case of success. Null otherwise.</returns>
        TIdentity SignInUser(string login, string password);

        /// <summary>
        /// Changes user password if user credentials match
        /// </summary>
        /// <param name="login">Login</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        /// <returns>Identity of user with changed password in case of success. Null otherwise.</returns>
        TIdentity ChangePassword(string login, string oldPassword, string newPassword);
    }
}
