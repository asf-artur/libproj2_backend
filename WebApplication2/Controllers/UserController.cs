using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication2.Contracts;
using WebApplication2.Contracts.Users;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly UserAccountRepository _userAccountRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(UserRepository userRepository, UserAccountRepository userAccountRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _userAccountRepository = userAccountRepository;
            _logger = logger;
        }

        [HttpGet]
        public void test()
        {
            _userRepository.GetAllLibrarians();
        }

        [NonAction]
        public IActionResult NoLogin()
        {
            return Unauthorized("nologin haha lol");
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            if (User.IsInRole(UserCategory.Admin.ToString()) || User.IsInRole(UserCategory.Librarian.ToString()))
            {
                return Json(_userRepository.GetAll());
            }

            //"Доступно только библиотекарям"
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (User.IsInRole(UserCategory.Admin.ToString()) || User.IsInRole(UserCategory.Librarian.ToString()))
            {
                var user = await _userRepository.Get(id);
                return Json(user);
            }

            //"Доступно только библиотекарям"
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Find(string searchTerm)
        {
            if (User.IsInRole(UserCategory.Admin.ToString()) || User.IsInRole(UserCategory.Librarian.ToString()))
            {
                if (searchTerm == "")
                {
                    var allUsers = _userRepository.GetAll();
                    return Json(allUsers);
                }
                var users = _userRepository.Search(searchTerm);

                return Json(users);
            }

            //"Доступно только библиотекарям"
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        [Authorize]
        [HttpPost]
        public IActionResult FindByBarcode(string barcode)
        {
            if (User.IsInRole(UserCategory.Admin.ToString()) || User.IsInRole(UserCategory.Librarian.ToString()))
            {
                var user = _userRepository.FindByBarcode(barcode);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }

                return Json(user);
            }

            //"Доступно только библиотекарям"
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        [HttpGet]
        public int admin1()
        {
            if (User.IsInRole(UserCategory.Admin.ToString()))
            {
                return 99;
            }
            else
            {
                return -1;
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendClientToken(string clientToken)
        {
            var user = await GetCurrentUser();
            if (user == null)
            {
                return Unauthorized();
            }
            _userRepository.SetClientToken(user.Id, clientToken);

            return Accepted();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var currentUserAccountName = User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultNameClaimType);
            var user = await _userRepository.Get(Convert.ToInt32(currentUserAccountName.Value));
            _userRepository.SetClientToken(user.Id, null);
            _logger.LogWarning($"user login: id[{user.Id}] name[{user.Name}] date[{DateTime.Now}]");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Accepted();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CheckLogin()
        {
            var currentUserAccountName = User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultNameClaimType);
            var user = await _userRepository.Get(Convert.ToInt32(currentUserAccountName.Value));
            _logger.LogWarning($"user check_login: id[{user.Id}] name[{user.Name}] date[{DateTime.Now}]");
            return Accepted(user);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginUserData loginUserData)
        {
            var userAccountId = _userAccountRepository.CheckLogin(loginUserData);
            if (userAccountId != null)
            {
                var user = await _userRepository.Get(userAccountId.Value);
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserCategory.ToString()),
                };
                ClaimsIdentity id = new ClaimsIdentity(claims,
                    "ApplicationCookie",
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
                _logger.LogWarning($"user login: id[{user.Id}] name[{user.Name}] date[{DateTime.Now}]");
                return Accepted(user);
            }
            return Unauthorized();
        }

        [NonAction]
        private async Task<User?> GetCurrentUser()
        {
            var currentUserAccountName = User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultNameClaimType);
            var user = await _userRepository.Get(Convert.ToInt32(currentUserAccountName.Value));

            return user;
        }
    }
}