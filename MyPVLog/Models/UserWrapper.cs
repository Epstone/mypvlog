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

        public string UserName => user.UserName;

        public object ProviderUserKey => user.ProviderUserKey;

        public string Email
        {
            get => user.Email;
            set => user.Email = value;
        }

        public string PasswordQuestion => user.PasswordQuestion;

        public string Comment
        {
            get => user.Comment;
            set => user.Comment = value;
        }

        public bool IsApproved
        {
            get => user.IsApproved;
            set => user.IsApproved = value;
        }

        public bool IsLockedOut => user.IsLockedOut;

        public DateTime LastLockoutDate => user.LastLockoutDate;

        public DateTime CreationDate => user.CreationDate;

        public DateTime LastLoginDate
        {
            get => user.LastLoginDate;
            set => user.LastLoginDate = value;
        }

        public DateTime LastActivityDate
        {
            get => user.LastActivityDate;
            set => user.LastActivityDate = value;
        }

        public DateTime LastPasswordChangedDate => user.LastPasswordChangedDate;

        public bool IsOnline => user.IsOnline;

        public string ProviderName => user.ProviderName;
    }
}