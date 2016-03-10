using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using prj_chamadosBRA.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using prj_chamadosBRA.Repositories;
using System.Collections.Generic;
using prj_chamadosBRA.Utils;
using prj_chamadosBRA.GN;

namespace prj_chamadosBRA.Controllers
{

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
        [Authorize]
        public ActionResult Index(string filtro)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var perfil = Convert.ToInt32(Session["PerfilUsuario"].ToString());
                    var users = new ApplicationUserGN(context).usuariosPorPerfil(perfil, User.Identity.GetUserId(), filtro);
                    return View(users);
                }
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
            var user = new ApplicationUserDAO().retornarUsuario(id);
            if(user.EnvioEmailSuperior == null)
            {
                user.EnvioEmailSuperior = false;
            }
            if (!string.IsNullOrEmpty(user.Superior))
            {
                ViewBag.SuperiorSelecionado = new ApplicationUserDAO().retornarUsuario(user.Superior).Nome;
            }
            if (Session["PerfilUsuario"].ToString().Equals("1") || Session["PerfilUsuario"].ToString().Equals("9"))
            {
                //montagem de perfil
                var perfil = new PerfilUsuarioDAO().BuscarPerfil(Convert.ToInt32(user.PerfilUsuario));
                var perfils = new SelectList(new PerfilUsuarioDAO().BuscarPerfis(), "IdPerfil", "Descricao", perfil.IdPerfil);
                ViewBag.PerfilUsuario = perfils;

                //montagem de obra
                var obrasUsuario = new UsuarioObraDAO().buscarUsuariosObras(user);
                ViewBag.ObrasUsuario = obrasUsuario;
                var obras = new SelectList(new ObraDAO().BuscarObras(), "IDO", "Descricao");
                ViewBag.ObrasDisponiveis = obras;

                //montagem de setor
                var setoresUsuario = new UsuarioSetorDAO().buscarUsuariosSetores(user);
                ViewBag.SetorUsuario = setoresUsuario;
                var setores = new SelectList(new SetorDAO().BuscarSetores(), "Id", "Descricao");
                ViewBag.SetoresDisponiveis = setores;
            }
            else if (Session["PerfilUsuario"].ToString().Equals("5"))
            {
                var perfils = new SelectList(PerfilUsuarioDAO.BuscarPerfisParaGestor(), "IdPerfil", "Descricao", user.PerfilUsuario);
                ViewBag.PerfilUsuario = perfils;
            }
            else if (Session["PerfilUsuario"].ToString().Equals("6"))
            {
                //montagem de perfil
                var perfils = new SelectList(PerfilUsuarioDAO.BuscarPerfisParaAdmObra(), "IdPerfil", "Descricao", user.PerfilUsuario);
                ViewBag.PerfilUsuario = perfils;

                //montagem de obra
                var obrasUsuario = new UsuarioObraDAO().buscarUsuariosObras(user);
                ViewBag.ObrasUsuario = obrasUsuario;
                var obras = new SelectList(new ObraDAO().BuscarObrasPorUsuario(user.Id), "IDO", "Descricao");
                ViewBag.ObrasDisponiveis = obras;

                //montagem de setor
                var setoresUsuario = new UsuarioSetorDAO().buscarUsuariosSetores(user);
                ViewBag.SetorUsuario = setoresUsuario;
                var setores = new SelectList(new SetorDAO().BuscarSetoresPorObras(new ObraDAO().BuscarObrasPorUsuario(user.Id)), "Id", "Descricao");
                ViewBag.SetoresDisponiveis = setores;

            }
            return View(user);
        }

        // POST: /Account/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(ApplicationUser user, string PerfilUsuario, string Superior)
        {
            using (var context = new ApplicationDbContext())
            {
                var appDAO = new ApplicationUserDAO(context);
                var userOrigem = appDAO.retornarUsuario(user.Id);
                userOrigem.Nome = user.Nome;
                userOrigem.UserName = user.UserName;
                userOrigem.PerfilUsuario = Convert.ToInt32(PerfilUsuario);
                userOrigem.Contato = user.Contato;
                userOrigem.Chapa = user.Chapa;
                if (!string.IsNullOrEmpty(Superior)) { userOrigem.Superior = Superior; };
                userOrigem.EnvioEmailSuperior = user.EnvioEmailSuperior;
                appDAO.atualizarApplicationUser(user.Id, userOrigem);
            }
            TempData["notice"] = "Dados alterados com sucesso!";
            return RedirectToAction("Index");
        }

        // GET: /Account/EsqueceuSenha/5
        public ActionResult EsqueceuSenha()
        {
            return View();
        }


        [HttpPost]
        public ActionResult EsqueceuSenha(RecoveryInitalViewModel usuario)
        {
            try
            {
                var user = new ApplicationUserDAO().retornarUsuarioPorUsername(usuario.UserName);
                if (user != null)
                {
                    new EmailEnvioDAO().salvarEmailEnvio(new EmailEnvio
                    {
                        InfoEmail = user.Id,
                        Data = DateTime.Now,
                        IdTipoEmail = (int)EmailTipo.EmailTipos.RedefinicaoSenhaUsuario
                    });
                    TempData["notice"] = "Email de recuperação de senha foi enviado com sucesso, verifique seu email.";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    TempData["notice"] = "Usuario não encontrado.";
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: /Account/RedefinicaoSenha/5
        public ActionResult RedefinicaoSenha(string idCript)
        {
            var objCript = new Criptografia(idCript);
            var idUser = objCript["id"].ToString();
            var user = UserManager.FindById(idUser);
            var userRecovery = new RecoveryViewModel
            {
                Id = user.Id,
                Nome = user.Nome
            };
            return View(userRecovery);
        }
        
        [HttpPost]
        public ActionResult RedefinicaoSenha(RecoveryViewModel user, string password)
        {
            try
            {
                UserManager.RemovePassword(user.Id);
                UserManager.AddPassword(user.Id, password);
                TempData["notice"] = "Senha alterada com sucesso!";
                return RedirectToAction("Login", "Account");

            }
            catch (NullReferenceException)
            {
                TempData["notice"] = "Problemas ao Alterar a senha!";
                return RedirectToAction("Login", "Account");
            }
        }
        
        [Authorize]
        public ActionResult ReiniciarSenha(string id)
        {
            try
            {
                const string newPassword = "123456";
                UserManager.RemovePassword(id);
                UserManager.AddPassword(id, newPassword);
                using (var context = new ApplicationDbContext())
                {
                    new EmailEnvioDAO(context).salvarEmailEnvio(new EmailEnvio
                    {
                        InfoEmail = id,
                        Data = DateTime.Now,
                        IdTipoEmail = (int)EmailTipo.EmailTipos.RedefinicaoSenhaUsuario
                    });
                    TempData["notice"] = "Senha do usuário redefinida com Sucesso!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                TempData["notice"] = "Problemas ao reiniciar senha do usuario";
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        public ActionResult AdicionarObraSetorUsuario(string idUser, string obra, string setor)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    if (setor != "" && setor != "undefined")
                    {
                        var usuarioSetor = new UsuarioSetor()
                        {
                            Setor = new SetorDAO(context).BuscarSetorId(Convert.ToInt32(setor)),
                            Usuario = idUser
                        };
                        var usuarioObra = new UsuarioObra()
                        {
                            Obra = usuarioSetor.Setor.obra,
                            Usuario = idUser
                        };
                        new UsuarioSetorDAO(context).salvarUsuarioSetor(usuarioSetor);
                        new UsuarioObraDAO(context).salvarUsuarioObra(usuarioObra);
                        TempData["notice"] = "Obra/Setor Adicionado com Sucesso!";
                        return RedirectToAction("Edit", new { id = idUser });
                    }
                    else if (obra != "")
                    {
                        var usuarioObra = new UsuarioObra()
                        {
                            Obra = new ObraDAO(context).BuscarObraId(Convert.ToInt32(obra)),
                            Usuario = idUser
                        };
                        new UsuarioObraDAO(context).salvarUsuarioObra(usuarioObra);
                        TempData["notice"] = "Obra Adicionada com Sucesso!";
                        return RedirectToAction("Edit", new { id = idUser });
                    }
                    else
                    {
                        TempData["notice"] = "Nenhuma Obra/Setor selecionada";
                        return RedirectToAction("Edit", new { id = idUser });
                    }
                }
            }
            catch (Exception)
            {
                TempData["notice"] = "Problemas ao adicionar Obra/Setor";
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        public ActionResult EliminarUsuarioObra(string id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var usuarioobra = new UsuarioObraDAO(context).buscarUsuarioObraPorId(Convert.ToInt32(id));
                    if (new UsuarioObraDAO(context).removerUsuarioObra(usuarioobra))
                    {
                        TempData["notice"] = "Obra Removida do Usuario com Sucesso!";
                        return RedirectToAction("Edit", new { id = usuarioobra.Usuario });
                    }
                    else
                    {
                        TempData["notice"] = "Problemas ao remover Obra do usuario";
                        return RedirectToAction("Edit", new { id = usuarioobra.Usuario });
                    }

                }
            }
            catch (Exception)
            {
                TempData["notice"] = "Problemas ao remover Obra do usuario";
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        public ActionResult EliminarUsuarioSetor(string id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var usuariosetor = new UsuarioSetorDAO(context).buscarUsuarioSetorPorId(Convert.ToInt32(id));
                    if (new UsuarioSetorDAO(context).removerUsuarioSetor(usuariosetor))
                    {
                        TempData["notice"] = "Obra Removida do Usuario com Sucesso!";
                        return RedirectToAction("Edit", new { id = usuariosetor.Usuario });
                    }
                    else
                    {
                        TempData["notice"] = "Problemas ao remover Obra do usuario";
                        return RedirectToAction("Edit", new { id = usuariosetor.Usuario });
                    }

                }
            }
            catch (Exception)
            {
                TempData["notice"] = "Problemas ao remover Obra do usuario";
                return RedirectToAction("Index");
            }
        }
        
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
                    new ApplicationUserDAO().atualizarUltimoAcesso(user.Id, DateTime.Now);
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
                            Session["ObraVisivel"] = true;
                            Session["TipoChamadoVisivel"] = true;
                            Session["SelecionarResponsavelAbertura"] = true;
                            break;
                        case "4": //Usuário
                            Session["SetorVisivel"] = true;
                            Session["ObraVisivel"] = false;
                            Session["TipoChamadoVisivel"] = true;
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
                        case "7": //Tecnico Totvs
                            Session["SetorVisivel"] = true;
                            Session["ObraVisivel"] = true;
                            Session["TipoChamadoVisivel"] = true;
                            Session["SelecionarResponsavelAbertura"] = true;
                            break;
                        case "9": //Central Atendimento
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
                    if (new UsuarioSetorDAO().buscarSetoresCorporativosDoUsuario(user).Count > 0)
                    {
                        var cookie = new HttpCookie("UsuarioSetorCorporativo")
                        {
                            Value = "true"
                        };
                        ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                        //Session["UsuarioSetorCorporativo"] = true;
                    }
                    else
                    {
                        var cookie = new HttpCookie("UsuarioSetorCorporativo")
                        {
                            Value = "false"
                        };
                        ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                        //Session["UsuarioSetorCorporativo"] = false;
                    }                   
                    
                    await SignInAsync(user, model.RememberMe);
                    if (returnUrl == null)
                    {
                        if (user.PerfilUsuario == 1 || user.PerfilUsuario == 6 || user.PerfilUsuario == 5 || user.PerfilUsuario == 3 || user.PerfilUsuario == 7 || user.PerfilUsuario == 9)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Acompanhamento", "Chamado");
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

        //
        // GET: /Account/Register
        [Authorize]
        public ActionResult Register()
        {
            var obras = new ObraDAO().BuscarObrasPorUsuario(User.Identity.GetUserId());
            ViewBag.UserId = User.Identity.GetUserId();
            var listObra = new SelectList(obras, "IDO", "Descricao");
            ViewBag.ObraDestino = listObra;
            if (listObra.Count() == 1)
            {
                ViewBag.ObraDestino = obras[0].IDO;
            }
            else
            {
                ViewBag.SetorDestino = new SelectList(new SetorDAO().BuscarSetores(), "Id", "Nome");
            }
            //ViewBag.Superiores = new SelectList(new ApplicationUserDAO().retornarUsuariosObras(obras, null), "Id", "Nome","-- Selecione o superior do usuario --");

            if (Session["PerfilUsuario"].ToString().Equals("1"))
            {
                ViewBag.Perfis = new PerfilUsuarioDAO().BuscarPerfis();
            }
            else if (Session["PerfilUsuario"].ToString().Equals("5"))
            {
                ViewBag.Perfis = PerfilUsuarioDAO.BuscarPerfisParaGestor();
            }
            else if (Session["PerfilUsuario"].ToString().Equals("6"))
            {
                ViewBag.UserId = User.Identity.GetUserId();
                ViewBag.Perfis = PerfilUsuarioDAO.BuscarPerfisParaAdmObra();
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
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    if (ModelState.IsValid)
                    {
                        var user = new ApplicationUser { UserName = model.UserName, PerfilUsuario = model.perfil, Nome = model.Nome, Contato = model.Contato, Email = model.UserName, Chapa = model.Chapa, Superior = model.Superior, EnvioEmailSuperior = model.EnvioEmailSuperior };
                        var result = UserManager.Create(user, model.Password);
                        if (result.Succeeded)
                        {
                            //if (obra == null && (Session["PerfilUsuario"].ToString().Equals("5") || Session["PerfilUsuario"].ToString().Equals("6")))
                            //{
                            //    obra = new UsuarioObraDAO(context).buscarObrasDoUsuario(new ApplicationUserDAO(context).retornarUsuario(User.Identity.GetUserId()))[0].IDO.ToString();
                            //}
                            var usuarioObra = new UsuarioObra
                            {
                                Usuario = user.Id,
                                Obra = new ObraDAO(context).BuscarObraId(Convert.ToInt32(obra))
                            };
                            if (new UsuarioObraDAO(context).salvarUsuarioObra(usuarioObra))
                            {
                                if (setor != null)
                                {
                                    var usuarioSetor = new UsuarioSetor
                                    {
                                        Usuario = user.Id,
                                        Setor = new SetorDAO(context).BuscarSetorId(Convert.ToInt32(setor))
                                    };
                                    if (new UsuarioSetorDAO(context).salvarUsuarioSetor(usuarioSetor))
                                    {
                                        new EmailEnvioDAO(context).salvarEmailEnvio(new EmailEnvio
                                        {
                                            InfoEmail = user.Id,
                                            Data = DateTime.Now,
                                            IdTipoEmail = (int)EmailTipo.EmailTipos.CriacaoUsuario
                                        });
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
                                ViewBag.Perfis = new PerfilUsuarioDAO().BuscarPerfis();
                            }
                            else if (Session["PerfilUsuario"].ToString().Equals("5"))
                            {
                                ViewBag.Perfis = PerfilUsuarioDAO.BuscarPerfisParaGestor();
                            }
                            AddErrors(result);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (Session["PerfilUsuario"].ToString().Equals("1"))
                {
                    ViewBag.Perfis = new PerfilUsuarioDAO().BuscarPerfis();
                }
                else if (Session["PerfilUsuario"].ToString().Equals("5"))
                {
                    ViewBag.Perfis = PerfilUsuarioDAO.BuscarPerfisParaGestor();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        public ActionResult RetornaSetoresPorObra(string selectedValue)
        {
            if (selectedValue != "")
            {
                var setores = new SetorDAO().BuscarSetoresPorObra(Convert.ToInt32(selectedValue));
                ActionResult json = Json(new SelectList(setores, "Id", "Nome"));
                return json;
            }
            else
            {
                var setores = new List<Setor>();
                ActionResult json = Json(new SelectList(setores, "Id", "Nome"));
                return json;
            }
        }

        public ActionResult RetornaSuperioresPorSetor(string selectedValue)
        {
            if (selectedValue != "")
            {
                if (new SetorDAO().isCorporativo(Convert.ToInt32(selectedValue)))
                {
                    var setor = new SetorDAO().BuscarSetorId(Convert.ToInt32(selectedValue));
                    var setores = new SetorDAO().BuscarSetoresCoorporativoPorId(setor.SetorCorporativo.Value);
                    var superiores = new ApplicationUserDAO().retornarUsuariosSetores(setores, null);
                    ActionResult json = Json(new SelectList(superiores, "Id", "Nome"));
                    return json;
                }
                else
                {
                    var superiores = new ApplicationUserDAO().retornarUsuariosSetor(new SetorDAO().BuscarSetorId(Convert.ToInt32(selectedValue)), null);
                    ActionResult json = Json(new SelectList(superiores, "Id", "Nome"));
                    return json;
                }
                
            }
            else
            {
                var superiores = new List<ApplicationUser>();
                ActionResult json = Json(new SelectList(superiores, "Id", "Nome"));
                return json;
            }
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
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
        [Authorize]
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
            var hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
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
                var state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
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
                var user = new ApplicationUser { UserName = model.UserName };
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
            var myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
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
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, identity);
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
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
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