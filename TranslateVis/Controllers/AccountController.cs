using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TranslateVis.Service;
using TranslateVis.DTO;
using TranslateVis.DAL.DataEntities;


using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;  // Claims會用到
using Microsoft.AspNetCore.Authorization;

namespace TranslateVis.Controllers
{
    public class AccountController : Controller
    {
        private UserService userService = new UserService();

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        // GET: LoginController
        public ActionResult Login(string returnUrl = "")
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }


        // POST: LoginController/Create
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.PasswordSignInAsync(model.Username,
                   model.Password, model.RememberMe);
                var user = userService.GetSingle(x => x.LoginName == model.Username);
                if (result)
                {
                    //添加 Identity 缓存
                    var claimsIdentity = new ClaimsIdentity("Cookie");
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, model.Username));
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid login attempt");

            if (string.IsNullOrEmpty(model.ReturnUrl))
            {
                model.ReturnUrl = "";
            }

            return View(model);
        }


        public async Task<IActionResult> LoginOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserInfo
                {
                    LinkMan = model.Username,
                    LoginName = model.Username,
                    CreateDate = DateTime.Now,
                    DepartmentId = 1,
                    IsNotIncumbency = false,
                    Password = model.Password
                };
                if (model.Password != model.ConfirmPassword)
                {
                    return Json(new { code = 0, msg = "输入密码不一致" });
                }
                var result = await userService.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //添加 Identity 缓存
                    var claimsIdentity = new ClaimsIdentity("Cookie");
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, model.Username));
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    string errMsg = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        errMsg += error.Description ?? string.Empty;
                    }
                    return Json(new { code = 0, msg = errMsg });
                }
            }
            else
            {
                if (model.Password != model.ConfirmPassword)
                {
                    return Json(new { code = 0, msg = "输入密码不一致" });
                }
            }
            return View();
        }
    }
}
