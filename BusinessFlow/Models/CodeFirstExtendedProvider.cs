using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace BusinessFlow.Models
{
    public abstract class CodeFirstExtendedProvider : MembershipProvider
    {

        private const int OneDayInMinutes = 24 * 60;
        public virtual string CreateAccount(string userName, string password, string email, out MembershipCreateStatus status)
        {
            return CreateAccount(userName, password, email, out status,requireConfirmationToken: false);
        }

        public abstract string CreateAccount(string userName, string password, string email,out MembershipCreateStatus status, bool requireConfirmationToken);

        public abstract bool ExtendedValidateUser(string userNameOrEmail, string password);

        public abstract bool ConfirmAccount(string accountConfirmationToken);

        public abstract bool DeleteAccount(string userName);

        public virtual string GeneratePasswordResetToken(string userName)
        {
            return GeneratePasswordResetToken(userName, tokenExpirationInMinutesFromNow: OneDayInMinutes);
        }

        public abstract string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow);

        public abstract Guid GetUserIdFromPasswordResetToken(string token);
        public abstract bool IsConfirmed(string userName);
        public abstract bool ResetPasswordWithToken(string token, string newPassword);
        public abstract int GetPasswordFailuresSinceLastSuccess(string userName);
        public abstract DateTime GetCreateDate(string userName);
        public abstract DateTime GetPasswordChangedDate(string userName);
        public abstract DateTime GetLastPasswordFailureDate(string userName);

    }
}