using System;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using CycleHire.Persistence.UnitOfWork;
using CycleHire.Core.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using CycleHire.Services;
using Microsoft.Extensions.Options;

namespace CycleHire.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly StripeSettingAuth _stripeSettingAuth;
        public PaymentController(IUnitOfWork unitOfWork, IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IOptions<StripeSettingAuth> stripeSettingAuth)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _stripeSettingAuth = stripeSettingAuth.Value;
        }

        public IActionResult Index()
        {

            return View();
        }

        //Connect host stripe account so we can pay them
        public async Task<IActionResult> ConnectHostToStripe(String code)
        {
            var service = new StripeOAuthTokenService(_stripeSettingAuth.SecretKey);

            var result = service.Create(new StripeOAuthTokenCreateOptions()
            {
                Code = code,
                GrantType = "authorization_code"
            });

            if (result.Error != null)
            {
                //error message for stripe!
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            user.ConnectHostToStripe(result.StripeUserId, result.StripePublishableKey,
                result.AccessToken, result.RefreshToken);

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index", "Host");
        }
    }
}