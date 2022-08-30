using CorridaAPI.Model.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CorridaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(SignInManager<ApplicationUser> signInManager, 
        UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }



    [HttpPost("login")]
    public IActionResult Login([FromServices] IConfiguration configuration, UsuarioModel usuario)
    {
        var result =  _signInManager.PasswordSignInAsync(usuario.Email,
            usuario.Senha, false, lockoutOnFailure: false).Result;

        if (result.Succeeded)
        {       
            var applicationUser = _userManager.Users.FirstOrDefault(x => x.Email == usuario.Email);
            _signInManager.SignOutAsync();
      
            return Ok(GerarToken(applicationUser, configuration));
        }
        else
        {
            return NotFound("Usuário não registrado!");
        }
    }
    [HttpPost("registrar")]
    public IActionResult Registrar(UsuarioModel usuario)
    {
        var applicationUser = new ApplicationUser
        {
            UserName = usuario.Email,
            Email = usuario.Email,
        };

        var resultado = _userManager.CreateAsync(applicationUser,usuario.Senha).Result;
        if (resultado.Succeeded)
        {
             _signInManager.SignInAsync(applicationUser, isPersistent: false);

            return Ok(usuario);
        }
        else
        {
            return UnprocessableEntity(usuario);
        }

        
    }



    private TokenModel GerarToken(ApplicationUser usuario,IConfiguration configuration)
    {
        var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id)
        };        

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecurityKey"])); 
        var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var exp = DateTime.UtcNow.AddHours(1);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: exp,
            signingCredentials: sign
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

      

        var tokenModel = new TokenModel { Token = tokenString, Expiration = exp};

        return tokenModel;
    }

}
