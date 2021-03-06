﻿using CoinbitBackend.Infrastructure;
using CoinbitBackend.Models;
using CoinbitBackend.Services;
using MD.PersianDateTime.Standard;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoinbitBackend.Controllers
{
    [ApiController]    
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;
        private DBRepository _dBRepository;

        public IJwtAuthManager JwtAuthManager => _jwtAuthManager;

        public AccountController(ILogger<AccountController> logger, IUserService userService, IJwtAuthManager jwtAuthManager, DBRepository dBRepository)
        {
            _logger = logger;
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
            _dBRepository = dBRepository;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<object> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                return Unauthorized("username and password must have value, both.");

            if (!_userService.IsValidUserCredentials(request.UserName, request.Password))
            {
                return Unauthorized();
            }

            var idrole = _userService.GetUserIDAndRole(request.UserName);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.UserName),
                new Claim(ClaimTypes.Role, idrole.Item2)
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
            _logger.LogInformation($"User [{request.UserName}] logged in the system.");
            if(idrole.Item2 == "customer")
            {
                var cus = await _dBRepository.Customers.AsNoTracking().FirstOrDefaultAsync(a => a.user_id == idrole.Item1);
                return Ok(new
                {
                    UserID = idrole.Item1.ToString(),
                    request.UserName,
                    Role = idrole.Item2,
                    jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString,
                    cus.Id,
                    cus.firstName,
                    cus.lastName,
                    cus.StatusId,
                    cus.fatherName,
                    cus.mobile,
                    cus.tel,
                    cus.address,
                    cus.postalCode,
                    cus.nationalCode,
                    cus.email,
                    birthdate = new PersianDateTime(cus.birthDate),
                    cus.card_number,
                    cus.bank_id,
                    cus.sheba_number,
                    cus.idcardpic,
                    cus.bankcardpic,
                    cus.selfiepic
                });
            }
            else
            {
                return Ok(new LoginResult
                {
                    UserID = idrole.Item1.ToString(),
                    UserName = request.UserName,
                    Role = idrole.Item2,
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                });
            }
            
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<object> GetCurrentUser()
        {
            var idrole = _userService.GetUserIDAndRole(User.Identity.Name);
            if (idrole.Item2 == "customer")
            {
                var cus = await _dBRepository.Customers.AsNoTracking().FirstOrDefaultAsync(a => a.user_id == idrole.Item1);
                return Ok(new
                {
                    UserID = idrole.Item1.ToString(),
                    UserName = User.Identity.Name,
                    Role = idrole.Item2,
                    cus.firstName,
                    cus.lastName,
                    cus.StatusId
                });
            }
            else
            {
                return Ok(new
                {
                    UserID = idrole.Item1.ToString(),
                    UserName = User.Identity.Name,
                    Role = idrole.Item2
                });
            }            
        }

        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            // optionally "revoke" JWT token on the server side --> add the current token to a block-list
            // https://github.com/auth0/node-jsonwebtoken/issues/375

            var userName = User.Identity.Name;
            _jwtAuthManager.RemoveRefreshTokenByUserName(userName);
            _logger.LogInformation($"User [{userName}] logged out the system.");
            return Ok();
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var userName = User.Identity.Name;
                _logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");

                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return Unauthorized();
                }

                var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
                var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
                _logger.LogInformation($"User [{userName}] has refreshed JWT token.");
                return Ok(new LoginResult
                {
                    UserName = userName,
                    Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                });
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
            }
        }

        //supervisor login
        //[HttpPost("impersonation")]
        //[Authorize(Roles = UserRoles.Admin)]
        //public ActionResult Impersonate([FromBody] ImpersonationRequest request)
        //{
        //    var userName = User.Identity.Name;
        //    _logger.LogInformation($"User [{userName}] is trying to impersonate [{request.UserName}].");

        //    var impersonatedRole = _userService.GetUserRole(request.UserName);
        //    if (string.IsNullOrWhiteSpace(impersonatedRole))
        //    {
        //        _logger.LogInformation($"User [{userName}] failed to impersonate [{request.UserName}] due to the target user not found.");
        //        return BadRequest($"The target user [{request.UserName}] is not found.");
        //    }
        //    if (impersonatedRole == UserRoles.Admin)
        //    {
        //        _logger.LogInformation($"User [{userName}] is not allowed to impersonate another Admin.");
        //        return BadRequest("This action is not supported.");
        //    }

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name,request.UserName),
        //        new Claim(ClaimTypes.Role, impersonatedRole),
        //        new Claim("OriginalUserName", userName)
        //    };

        //    var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
        //    _logger.LogInformation($"User [{request.UserName}] is impersonating [{request.UserName}] in the system.");
        //    return Ok(new LoginResult
        //    {
        //        UserName = request.UserName,
        //        Role = impersonatedRole,
        //        OriginalUserName = userName,
        //        AccessToken = jwtResult.AccessToken,
        //        RefreshToken = jwtResult.RefreshToken.TokenString
        //    });
        //}


        //supervisor logout
        //[HttpPost("stop-impersonation")]
        //public ActionResult StopImpersonation()
        //{
        //    var userName = User.Identity.Name;
        //    var originalUserName = User.FindFirst("OriginalUserName")?.Value;
        //    if (string.IsNullOrWhiteSpace(originalUserName))
        //    {
        //        return BadRequest("You are not impersonating anyone.");
        //    }
        //    _logger.LogInformation($"User [{originalUserName}] is trying to stop impersonate [{userName}].");

        //    var role = _userService.GetUserRole(originalUserName);
        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name,originalUserName),
        //        new Claim(ClaimTypes.Role, role)
        //    };

        //    var jwtResult = _jwtAuthManager.GenerateTokens(originalUserName, claims, DateTime.Now);
        //    _logger.LogInformation($"User [{originalUserName}] has stopped impersonation.");
        //    return Ok(new LoginResult
        //    {
        //        UserName = originalUserName,
        //        Role = role,
        //        OriginalUserName = null,
        //        AccessToken = jwtResult.AccessToken,
        //        RefreshToken = jwtResult.RefreshToken.TokenString
        //    });
        //}
    }
}
