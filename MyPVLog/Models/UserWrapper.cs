namespace PVLog.Models
{
    using System;
    using System.Web.Security;

    public class UserWrapper : IUser
    {
        private MembershipUser user;

        public UserWrapper(MembershipUser user)
        {
            this.user = user;
        }

        public string GetPassword()
        {
            return user.GetPassword();
        }

        public string GetPassword(string passwordAnswer)
        {
            return user.GetPassword(passwordAnswer);
        }

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            return user.ChangePassword(oldPassword, newPassword);
        }

        public bool ChangePasswordQuestionAndAnswer(string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return user.ChangePasswordQuestionAndAnswer(password, newPasswordQuestion, newPasswordAnswer);
        }

        public string ResetPassword(string passwordAnswer)
        {
            return user.ResetPassword(passwordAnswer);
        }

        public string ResetPassword()
        {
            return user.ResetPassword();
        }

        public bool UnlockUser()
        {
            return user.UnlockUser();
        }

        public string UserName
        {
            get { return user.UserName; }
        }

        public object ProviderUserKey
        {
            get { return user.ProviderUserKey; }
        }

        public string Email
        {
            get { return user.Email; }
            set { user.Email = value; }
        }

        public string PasswordQuestion
        {
            get { return user.PasswordQuestion; }
        }

        public string Comment
        {
            get { return user.Comment; }
            set { user.Comment = value; }
        }

        public bool IsApproved
        {
            get { return user.IsApproved; }
            set { user.IsApproved = value; }
        }

        public bool IsLockedOut
        {
            get { return user.IsLockedOut; }
        }

        public DateTime LastLockoutDate
        {
            get { return user.LastLockoutDate; }
        }

        public DateTime CreationDate
        {
            get { return user.CreationDate; }
        }

        public DateTime LastLoginDate
        {
            get { return user.LastLoginDate; }
            set { user.LastLoginDate = value; }
        }

        public DateTime LastActivityDate
        {
            get { return user.LastActivityDate; }
            set { user.LastActivityDate = value; }
        }

        public DateTime LastPasswordChangedDate
        {
            get { return user.LastPasswordChangedDate; }
        }

        public bool IsOnline
        {
            get { return user.IsOnline; }
        }

        public string ProviderName
        {
            get { return user.ProviderName; }
        }
    }
}