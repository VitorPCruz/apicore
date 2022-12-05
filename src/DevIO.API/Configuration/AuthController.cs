using DevIO.API.Controllers;
using DevIO.API.ViewModels;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.API.Configuration
{
    [Route("api")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(
            INotificador notificador, 
            SignInManager<IdentityUser> signManager, 
            UserManager<IdentityUser> userManager) : base(notificador)
        {
            _signManager = signManager;
            _userManager = userManager;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                await _signManager.SignInAsync(user, false);
                return CustomResponse(registerUser);
            }
            
            foreach (var error in result.Errors)
                NotificarErro(error.Description);

            return CustomResponse(registerUser);
        }

        [HttpPost("entrar")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signManager.PasswordSignInAsync(
                loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
                return CustomResponse(loginUser);

            if (result.IsLockedOut)
            {
                NotificarErro("Usuário temporáriamente bloqueado por várias tentativas inválidas.");
                return CustomResponse(loginUser);
            }

            NotificarErro("Credenciais inválidas.");
            return CustomResponse(loginUser);
        }
    }
}
