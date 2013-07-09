using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PVLog.Models
{

  #region Models

  [PropertiesMustMatch("NewPassword", "PasswordConfirmation", ErrorMessage = "Die Passwörter stimmen nicht überein.")]
  public class ResetPasswordModel
  {
    [DisplayName("Neues Passwort")]
    [DataType(DataType.Password)]
    [ValidatePasswordLength]
    public string NewPassword { get; set; }

    [DisplayName("Passwort Wiederholen")]
    [ValidatePasswordLength]
    [DataType(DataType.Password)]
    public string PasswordConfirmation { get; set; }

    public Guid ResetKey { get; set; }
  }

  public class ForgettPasswordModel
  {
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
  }

  [PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "Die Passwörter stimmen nicht überein.")]
  public class ChangePasswordModel
  {
    [Required]
    [DataType(DataType.Password)]
    [DisplayName("Aktuelles Passwort")]
    public string OldPassword { get; set; }

    [Required]
    [ValidatePasswordLength]
    [DataType(DataType.Password)]
    [DisplayName("Neues Passwort")]
    public string NewPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [DisplayName("Passwort bestätigen")]
    public string ConfirmPassword { get; set; }
  }

  public class LogOnModel
  {
    [Required]
    [DisplayName("Name")]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [DisplayName("Passwort")]
    public string Password { get; set; }

    [DisplayName("Merken?")]
    public bool RememberMe { get; set; }
  }

  [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "Die Passwörter stimmen nicht überein.")]
  public class RegisterModel
  {
    [Required]
    [DisplayName("Benutzername")]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    [DisplayName("E-Mail Adresse")]
    public string Email { get; set; }

    [Required]
    [ValidatePasswordLength]
    [DataType(DataType.Password)]
    [DisplayName("Passwort")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [DisplayName("Passwort bestätigen")]
    public string ConfirmPassword { get; set; }
  }
  #endregion

  #region Services
  // The FormsAuthentication type is sealed and contains static members, so it is difficult to
  // unit test code that calls its members. The interface and helper class below demonstrate
  // how to create an abstract wrapper around such a type in order to make the AccountController
  // code unit testable.

  public interface IMembershipService
  {
    int MinPasswordLength { get; }

    bool ValidateUser(string userName, string password);
    MembershipCreateStatus CreateUser(string userName, string password, string email);
    bool ChangePassword(string userName, string oldPassword, string newPassword);

    int CurrentUserId { get; }

  }

  public class AccountMembershipService : IMembershipService
  {
    private readonly MembershipProvider _provider;

    public AccountMembershipService()
      : this(null)
    {
    }

    public AccountMembershipService(MembershipProvider provider)
    {
      _provider = provider ?? Membership.Provider;

    }

    public int MinPasswordLength
    {
      get
      {
        return _provider.MinRequiredPasswordLength;
      }
    }

    public bool ValidateUser(string userName, string password)
    {
      if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
      if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

      return _provider.ValidateUser(userName, password);
    }

    public MembershipCreateStatus CreateUser(string userName, string password, string email)
    {
      if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
      if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
      if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");

      MembershipCreateStatus status;
      _provider.CreateUser(userName, password, email, null, null, true, null, out status);
      return status;
    }

    public bool ChangePassword(string userName, string oldPassword, string newPassword)
    {
      if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
      if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
      if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

      // The underlying ChangePassword() will throw an exception rather
      // than return false in certain failure scenarios.
      try
      {
        MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
        return currentUser.ChangePassword(oldPassword, newPassword);
      }
      catch (ArgumentException)
      {
        return false;
      }
      catch (MembershipPasswordException)
      {
        return false;
      }
    }


    public MembershipUser GetUser()
    {
      return Membership.GetUser();
    }

    public int CurrentUserId
    {
      get
      {
        return (int)Membership.GetUser().ProviderUserKey;
      }
    }
  }

  public interface IFormsAuthenticationService
  {
    void SignIn(string userName, bool createPersistentCookie);
    void SignOut();
  }

  public class FormsAuthenticationService : IFormsAuthenticationService
  {
    public void SignIn(string userName, bool createPersistentCookie)
    {
      if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

      FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
    }

    public void SignOut()
    {
      FormsAuthentication.SignOut();
    }
  }
  #endregion

  #region Validation
  public static class AccountValidation
  {
    public static string ErrorCodeToString(MembershipCreateStatus createStatus)
    {
      // See http://go.microsoft.com/fwlink/?LinkID=177550 for
      // a full list of status codes.
      switch (createStatus)
      {
        case MembershipCreateStatus.DuplicateUserName:
          return "Username already exists. Please enter a different user name.";

        case MembershipCreateStatus.DuplicateEmail:
          return "A username for that e-mail address already exists. Please enter a different e-mail address.";

        case MembershipCreateStatus.InvalidPassword:
          return "The password provided is invalid. Please enter a valid password value.";

        case MembershipCreateStatus.InvalidEmail:
          return "The e-mail address provided is invalid. Please check the value and try again.";

        case MembershipCreateStatus.InvalidAnswer:
          return "The password retrieval answer provided is invalid. Please check the value and try again.";

        case MembershipCreateStatus.InvalidQuestion:
          return "The password retrieval question provided is invalid. Please check the value and try again.";

        case MembershipCreateStatus.InvalidUserName:
          return "The user name provided is invalid. Please check the value and try again.";

        case MembershipCreateStatus.ProviderError:
          return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        case MembershipCreateStatus.UserRejected:
          return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        default:
          return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
      }
    }
  }

  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
  public sealed class PropertiesMustMatchAttribute : ValidationAttribute
  {
    private const string _defaultErrorMessage = "'{0}' and '{1}' do not match.";
    private readonly object _typeId = new object();

    public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
      : base(_defaultErrorMessage)
    {
      OriginalProperty = originalProperty;
      ConfirmProperty = confirmProperty;
    }

    public string ConfirmProperty { get; private set; }
    public string OriginalProperty { get; private set; }

    public override object TypeId
    {
      get
      {
        return _typeId;
      }
    }

    public override string FormatErrorMessage(string name)
    {
      return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
          OriginalProperty, ConfirmProperty);
    }

    public override bool IsValid(object value)
    {
      PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
      object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
      object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
      return Object.Equals(originalValue, confirmValue);
    }
  }

  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
  {
    private const string _defaultErrorMessage = "'{0}' must be at least {1} characters long.";
    private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

    public ValidatePasswordLengthAttribute()
      : base(_defaultErrorMessage)
    {
    }

    public override string FormatErrorMessage(string name)
    {
      return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
          name, _minCharacters);
    }

    public override bool IsValid(object value)
    {
      string valueAsString = value as string;
      return (valueAsString != null && valueAsString.Length >= _minCharacters);
    }
  }
  #endregion

}
