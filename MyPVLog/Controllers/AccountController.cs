using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using PVLog.Models;
using AccountHelperRepositoryMySql;
using CommonUtilities.Extensions;
using MySqlRepository;
using System.Configuration;

namespace PVLog.Controllers
{

  [HandleError]
  public class AccountController : Controller
  {

    public IFormsAuthenticationService FormsService { get; set; }
    public IMembershipService MembershipService { get; set; }

    protected override void Initialize(RequestContext requestContext)
    {
      if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
      if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

      base.Initialize(requestContext);
    }

    // **************************************
    // URL: /Account/LogOn
    // **************************************

    public ActionResult LogOn()
    {
      return View();
    }

    [HttpPost]
    public ActionResult LogOn(LogOnModel model, string returnUrl)
    {
      if (ModelState.IsValid)
      {
        if (MembershipService.ValidateUser(model.UserName, model.Password))
        {
          FormsService.SignIn(model.UserName, model.RememberMe);
          if (!String.IsNullOrEmpty(returnUrl) && !returnUrl.ToLower().Contains("account"))
          {
            return Redirect(returnUrl);
          }
          else
          {
            return RedirectToAction("Index", "Home");
          }
        }
        else
        {
          ModelState.AddModelError("", "The user name or password provided is incorrect.");
        }
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }

    // **************************************
    // URL: /Account/LogOff
    // **************************************

    public ActionResult LogOff()
    {
      FormsService.SignOut();

      return RedirectToAction("Index", "Home");
    }

    // **************************************
    // URL: /Account/Register
    // **************************************

    public ActionResult Register()
    {
      ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
      return View();
    }

    [HttpPost]
    public ActionResult Register(RegisterModel model)
    {
      if (ModelState.IsValid)
      {
        // Attempt to register the user
        MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

        if (createStatus == MembershipCreateStatus.Success)
        {
          FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
          return RedirectToAction("Index", "Home");
        }
        else
        {
          ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
        }
      }

      // If we got this far, something failed, redisplay form
      ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
      return View(model);
    }

    // **************************************
    // URL: /Account/ChangePassword
    // **************************************

    [Authorize]
    public ActionResult ChangePassword()
    {
      ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
      return View();
    }

    [Authorize]
    [HttpPost]
    public ActionResult ChangePassword(ChangePasswordModel model)
    {
      if (ModelState.IsValid)
      {
        if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
        {
          return RedirectToAction("ChangePasswordSuccess");
        }
        else
        {
          ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
        }
      }

      // If we got this far, something failed, redisplay form
      ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
      return View(model);
    }

    // **************************************
    // URL: /Account/ChangePasswordSuccess
    // **************************************

    public ActionResult ChangePasswordSuccess()
    {
      return View();
    }

    [HttpGet]
    public ActionResult ForgettPassword()
    {
      return View();
    }

    [HttpPost]
    public ActionResult ForgettPassword(ForgettPasswordModel model)
    {
      bool emailOk = false;
      if (ModelState.IsValid)
      {
        var repository = GetAccountHelperRepository();


        Guid resetKey = Guid.NewGuid();
        string url = Url.AbsoluteAction("ResetPassword", "Account", new { resetKey },false);

        emailOk = ForgettPasswordHelper.SendForgetPasswordMail(model.Email, url, resetKey, repository, "Patrickeps@gmx.de");
      }

      if (!emailOk)
      {
        ModelState.AddModelError("EmailAddress", "Die E-Mail Adresse ist nicht registriert");
        return View(model);
      }
      return View("PasswordResetLinkSent");

    }

    private static MySqlAccountHelperRepository GetAccountHelperRepository()
    {
      var connStr = ConfigurationManager.ConnectionStrings["pv_data"].ConnectionString;
      MySqlRepositoryBase mySqlLayer = new MySqlRepositoryBase();
      mySqlLayer.Initialize(connStr, connStr);

      var repository = MySqlAccountHelperRepository.Create(mySqlLayer, 2);
      return repository;
    }

    [HttpGet]
    public ActionResult ResetPassword(Guid resetKey)
    {
      try
      {
        var user = ForgettPasswordHelper.ValidatePasswordResetKeyGetUserID(resetKey, GetAccountHelperRepository());
      }
      catch (ArgumentException ex)
      {
        throw ex;
      }

      ResetPasswordModel model = new ResetPasswordModel()
      {
        ResetKey = resetKey
      };

      return PartialView(model);
    }

    [HttpPost]
    public ActionResult ResetPassword(ResetPasswordModel model)
    {
      if (ModelState.IsValid)
      {
        //change password to the new value
        var repository = GetAccountHelperRepository();
        var user = ForgettPasswordHelper.ValidatePasswordResetKeyGetUserID(model.ResetKey, repository);
        string tmpPassword = user.ResetPassword();
        user.ChangePassword(tmpPassword, model.NewPassword);

        //remove the password reset key
        ForgettPasswordHelper.RemoveResetKey(model.ResetKey, repository);
      }
      else
      {
        model.NewPassword = string.Empty;
        model.PasswordConfirmation = string.Empty;
        return View(model);
      }

      return View("ResetPasswordSuccess");

    }
  }
}
