using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace apief;

[ApiController]
[Route("[controller]")]
public class RegistrationController : ControllerBase
{
    private readonly IAuthHelp _authHelp;
    private readonly IAuthRepository _authRepository;


    public RegistrationController(IAuthHelp authHelp, IAuthRepository authRepository)
    {
        _authHelp = authHelp;
        _authRepository = authRepository;
    }


    [AllowAnonymous]
    [HttpPost("Register")]
    public IActionResult Register(UserForRegistrationDto userForRegistration)
    {
        string token;
        try
        {
            _authRepository.CheckUser(userForRegistration);
            token = _authRepository.RegistrEndInsert(userForRegistration);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(new { Token = token });
    }
    [AllowAnonymous]
    [HttpPost("Login")]
    public IActionResult Login(UserForLoginDto userForLogin)
    {
        string newToken;
        string token;
        try
        {
            token = _authRepository.CheckEmail(userForLogin);
            _authRepository.CheckPassword(userForLogin);
            int Id = _authHelp.GetUserIdFromToken(token);
            if (Id == 0)
            {
                return BadRequest("Can not find this user");
            }
            newToken = _authHelp.GenerateNewToken(Id);

            bool tokenUpdate = _authHelp.UpdateTokenValueInDatabase(Id, newToken);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(new { Token = newToken });
    }


}