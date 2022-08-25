using CorridaAPI.Authentication.Identity;
using CorridaAPI.Model.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult Login(UsuarioModel usuario)
    {
        var result =  _signInManager.PasswordSignInAsync(usuario.Email,
            usuario.Senha, false, lockoutOnFailure: false).Result;
        if (result.Succeeded)
        {
            return Ok("Login feito com sucesso");
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

}
