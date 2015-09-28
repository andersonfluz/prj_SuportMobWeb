using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using prj_chamadosBRA.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using prj_chamadosBRA.Repositories;
using System.Collections.Generic;
using prj_chamadosBRA.Utils;
using prj_chamadosBRA.GN;

namespace prj_chamadosBRA.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Construtor Antigo
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager)
            {
                AllowOnlyAlphanumericUserNames = false
            };
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        #endregion

        //private ApplicationSignInManager _signInManager;
        //private ApplicationUserManager _userManager;

        //public AccountController()
        //{
        //}

        //public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        //{
        //    UserManager = userManager;
        //    SignInManager = signInManager;
        //}

        //public ApplicationSignInManager SignInManager
        //{
        //    get
        //    {
        //        return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        //    }
        //    private set
        //    {
        //        _signInManager = value;
        //    }
        //}

        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // GET: /Account/Index
        [AllowAnonymous]
        public ActionResult Index(string filtro)
        {
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                int perfil = Convert.ToInt32(Session["PerfilUsuario"].ToString());
                List<ApplicationUser> users = new ApplicationUserGN(context).usuariosPorPerfil(perfil, User.Identity.GetUserId(), filtro);
                return View(users);
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: /Account/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            ApplicationUser user = new ApplicationUserDAO().retornarUsuario(id);
            if (Session["PerfilUsuario"].ToString().Equals("1"))
            {
                SelectList list = new SelectList(new prj_chamadosBRA.Repositories.PerfilUsuarioDAO().BuscarPerfis(), "IdPerfil", "Descricao", user.PerfilUsuario);
                ViewBag.PerfilUsuario = list;
            }
            else if (Session["PerfilUsuario"].ToString().Equals("5"))
            {
                SelectList list = new SelectList(new prj_chamadosBRA.Repositories.PerfilUsuarioDAO().BuscarPerfisParaGestor(), "IdPerfil", "Descricao", user.PerfilUsuario);
                ViewBag.PerfilUsuario = list;
            }
            else if (Session["PerfilUsuario"].ToString().Equals("6"))
            {
                SelectList list = new SelectList(new prj_chamadosBRA.Repositories.PerfilUsuarioDAO().BuscarPerfisParaAdmObra(), "IdPerfil", "Descricao", user.PerfilUsuario);
                ViewBag.PerfilUsuario = list;
            }
            return View(user);
        }

        // POST: /Account/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(ApplicationUser user)
        {
            ApplicationUserDAO appDAO = new ApplicationUserDAO();
            ApplicationUser userOrigem = appDAO.retornarUsuario(user.Id);
            userOrigem.Nome = user.Nome;
            userOrigem.UserName = user.UserName;
            userOrigem.PerfilUsuario = user.PerfilUsuario;
            userOrigem.Contato = user.Contato;
            appDAO.atualizarApplicationUser(user.Id, userOrigem);
            TempData["notice"] = "Dados alterados com sucesso!";
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<ActionResult> ReiniciarSenha(string id)
        {
            try
            {
                String newPassword = "123456";
                UserManager.RemovePassword(id);
                UserManager.AddPassword(id, newPassword);
                ApplicationDbContext context = new ApplicationDbContext();
                //await EmailServiceUtil.envioEmailRedefinicaoSenhaUsuario(new ApplicationUserDAO(context).retornarUsuario(id));
                TempData["notice"] = "Senha do usuário redefinida com Sucesso!";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["notice"] = "Problemas ao reiniciar senha do usuario";
                return RedirectToAction("Index");
            }
        }

        ////
        //// POST: /Account/Login
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // This doesn't count login failures towards account lockout
        //    // To enable password failures to trigger account lockout, change to shouldLockout: true
        //    model.Email = model.UserName;
        //    var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            var user = await UserManager.FindAsync(model.UserName, model.Password);
        //            Session["UserId"] = user.Id;
        //            Session["PerfilUsuario"] = user.PerfilUsuario;
        //            switch (Session["PerfilUsuario"].ToString())
        //            {
        //                case "1": //Administrador
        //                    Session["SetorVisivel"] = true;
        //                    Session["ObraVisivel"] = true;
        //                    Session["TipoChamadoVisivel"] = true;
        //                    Session["SelecionarResponsavelAbertura"] = true;
        //                    break;
        //                case "2": //SuperiorBRA
        //                    Session["SetorVisivel"] = false;
        //                    Session["ObraVisivel"] = true;
        //                    Session["TipoChamadoVisivel"] = false;
        //                    Session["SelecionarResponsavelAbertura"] = false;
        //                    break;
        //                case "3": //Tecnico
        //                    Session["SetorVisivel"] = true;
        //                    Session["ObraVisivel"] = false;
        //                    Session["TipoChamadoVisivel"] = true;
        //                    Session["SelecionarResponsavelAbertura"] = true;
        //                    break;
        //                case "4": //Usuário
        //                    Session["SetorVisivel"] = true;
        //                    Session["ObraVisivel"] = false;
        //                    Session["TipoChamadoVisivel"] = false;
        //                    Session["SelecionarResponsavelAbertura"] = false;
        //                    break;
        //                case "5": //Gestor
        //                    Session["SetorVisivel"] = true;
        //                    Session["ObraVisivel"] = false;
        //                    Session["TipoChamadoVisivel"] = true;
        //                    Session["SelecionarResponsavelAbertura"] = true;
        //                    break;
        //                case "6": //Administrador da Obra
        //                    Session["SetorVisivel"] = true;
        //                    Session["ObraVisivel"] = false;
        //                    Session["TipoChamadoVisivel"] = true;
        //                    Session["SelecionarResponsavelAbertura"] = true;
        //                    break;
        //                default:
        //                    Session["SetorVisivel"] = true;
        //                    Session["ObraVisivel"] = false;
        //                    Session["TipoChamadoVisivel"] = true;
        //                    break;
        //            }                    
        //            if (user.PerfilUsuario == 1 || user.PerfilUsuario == 6)
        //            {
        //                return RedirectToAction("Index", "Home");
        //            }
        //            else
        //            {
        //                return RedirectToAction("Index", "Chamado");
        //            }
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        //        case SignInStatus.Failure:
        //        default:
        //            ModelState.AddModelError("", "Invalid login attempt.");
        //            return View(model);
        //    }
        //}

        #region Login Antigo
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            AuthenticationManager.SignOut();
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    Session["UserId"] = user.Id;
                    Session["PerfilUsuario"] = user.PerfilUsuario;
                    switch (Session["PerfilUsuario"].ToString())
                    {
                        case "1": //Administrador
                            Session["SetorVisivel"] = true;
                            Session["ObraVisivel"] = true;
                            Session["TipoChamadoVisivel"] = true;
                            Session["SelecionarResponsavelAbertura"] = true;
                            break;
                        case "2": //SuperiorBRA
                            Session["SetorVisivel"] = false;
                            Session["ObraVisivel"] = true;
                            Session["TipoChamadoVisivel"] = false;
                            Session["SelecionarResponsavelAbertura"] = false;
                            break;
                        case "3": //Tecnico
                            Session["SetorVisivel"] = true;
                            Session["ObraVisivel"] = false;
                            Session["TipoChamadoVisivel"] = true;
                            Session["SelecionarResponsavelAbertura"] = true;
                            break;
                        case "4": //Usuário
                            Session["SetorVisivel"] = true;
                            Session["ObraVisivel"] = false;
                            Session["TipoChamadoVisivel"] = false;
                            Session["SelecionarResponsavelAbertura"] = false;
                            break;
                        case "5": //Gestor
                            Session["SetorVisivel"] = true;
                            Session["ObraVisivel"] = false;
                            Session["TipoChamadoVisivel"] = true;
                            Session["SelecionarResponsavelAbertura"] = true;
                            break;
                        case "6": //Administrador da Obra
                            Session["SetorVisivel"] = true;
                            Session["ObraVisivel"] = true;
                            Session["TipoChamadoVisivel"] = true;
                            Session["SelecionarResponsavelAbertura"] = true;
                            break;
                        default:
                            Session["SetorVisivel"] = true;
                            Session["ObraVisivel"] = false;
                            Session["TipoChamadoVisivel"] = true;
                            break;
                    }

                    await SignInAsync(user, model.RememberMe);
                    if (returnUrl == null)
                    {
                        if (user.PerfilUsuario == 1 || user.PerfilUsuario == 6)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Chamado");
                        }
                    }
                    else
                    {
                        return RedirectToLocal(returnUrl);
                    }
                
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            List<Obra> obras = new prj_chamadosBRA.Repositories.ObraDAO().BuscarObrasPorUsuario(User.Identity.GetUserId());
            ViewBag.UserId = User.Identity.GetUserId();
            SelectList listObra = new SelectList(obras, "IDO", "Descricao");
            ViewBag.ObraDestino = listObra;
            if (listObra.Count() == 1)
            {
                ViewBag.ObraDestino = obras[0].IDO;
            }
            else
            {
                ViewBag.SetorDestino = new SelectList(new prj_chamadosBRA.Repositories.SetorDAO().BuscarSetores(), "Id", "Nome");
            }

            if (Session["PerfilUsuario"].ToString().Equals("1"))
            {
                ViewBag.Perfis = new prj_chamadosBRA.Repositories.PerfilUsuarioDAO().BuscarPerfis();
            }
            else if (Session["PerfilUsuario"].ToString().Equals("5"))
            {
                ViewBag.Perfis = new prj_chamadosBRA.Repositories.PerfilUsuarioDAO().BuscarPerfisParaGestor();
            }
            else if (Session["PerfilUsuario"].ToString().Equals("6"))
            {
                ViewBag.UserId = User.Identity.GetUserId(); 
                ViewBag.Perfis = new prj_chamadosBRA.Repositories.PerfilUsuarioDAO().BuscarPerfisParaAdmObra();
            }
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Register(RegisterViewModel model, String obra, String setor)
        {
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser() { UserName = model.UserName, PerfilUsuario = model.perfil, Nome = model.Nome, Contato = model.Contato };
                    var result = UserManager.Create(user, model.Password);
                    if (result.Succeeded)
                    {
                        if (obra == null && (Session["PerfilUsuario"].ToString().Equals("5") || Session["PerfilUsuario"].ToString().Equals("6")))
                        {
                            obra = new UsuarioObraDAO(context).buscarObrasDoUsuario(new ApplicationUserDAO(context).retornarUsuario(User.Identity.GetUserId()))[0].IDO.ToString();
                        }
                        UsuarioObra usuarioObra = new UsuarioObra();
                        usuarioObra.Usuario = user.Id;
                        usuarioObra.Obra = Convert.ToInt32(obra);
                        if (new UsuarioObraDAO(context).salvarUsuarioObra(usuarioObra))
                        {
                            if (setor != null)
                            {
                                UsuarioSetor usuarioSetor = new UsuarioSetor();
                                usuarioSetor.Usuario = user.Id;
                                usuarioSetor.Setor = Convert.ToInt32(setor);
                                if (new UsuarioSetorDAO(context).salvarUsuarioSetor(usuarioSetor))
                                {
                                    await EmailServiceUtil.envioEmailCriacaoUsuario(user);
                                    TempData["notice"] = "Usuário criado com Sucesso!";
                                    return RedirectToAction("Index");
                                }
                            }
                            else
                            {
                                TempData["notice"] = "Usuário criado com Sucesso!";
                                return RedirectToAction("Index");
                            }
                        }
                    }
                    else
                    {
                        if (Session["PerfilUsuario"].ToString().Equals("1"))
                        {
                            ViewBag.Perfis = new prj_chamadosBRA.Repositories.PerfilUsuarioDAO().BuscarPerfis();
                        }
                        else if (Session["PerfilUsuario"].ToString().Equals("5"))
                        {
                            ViewBag.Perfis = new prj_chamadosBRA.Repositories.PerfilUsuarioDAO().BuscarPerfisParaGestor();
                        }
                        AddErrors(result);
                    }
                }
            }
            catch
            {
                if (Session["PerfilUsuario"].ToString().Equals("1"))
                {
                    ViewBag.Perfis = new prj_chamadosBRA.Repositories.PerfilUsuarioDAO().BuscarPerfis();
                }
                else if (Session["PerfilUsuario"].ToString().Equals("5"))
                {
                    ViewBag.Perfis = new prj_chamadosBRA.Repositories.PerfilUsuarioDAO().BuscarPerfisParaGestor();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        public ActionResult RetornaSetoresPorObra(string selectedValue)
        {
            List<Setor> setores = new SetorDAO().BuscarSetoresPorObra(Convert.ToInt32(selectedValue));
            ActionResult json = Json(new SelectList(setores, "Id", "Nome"));
            return json;
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Sua senha foi alterada."
                : message == ManageMessageId.SetPasswordSuccess ? "Sua senha foi definida."
                : message == ManageMessageId.RemoveLoginSuccess ? "O login externo foi removido."
                : message == ManageMessageId.Error ? "Ocorreu um erro."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (message == ManageMessageId.ChangePasswordSuccess)
            {
                return View("ChangePassword");
            }
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}