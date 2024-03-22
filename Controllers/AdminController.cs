using AutoMapper;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectAspEShop2024.AppIdentity;
using ProjectAspEShop2024.Models;
using ProjectAspEShop2024.DataAccessLayer;

namespace ProjectAspEShop2024.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ShopContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AdminController(
            ShopContext context,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> EditUserView(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = _mapper.Map<UserEditViewModel>(user);
            model.AllRoles = _roleManager.Roles.ToList();
            model.UserRoles = userRoles.ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserView(UserEditViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            var allRoles = _roleManager.Roles.ToList();
            var userRoles = (await _userManager.GetRolesAsync(user)).ToList();

            //только добавленные роли
            var addedRoles = model.EditRoles.Except(userRoles);

            //только удаленные роли
            var removedRoles = userRoles.Except(model.EditRoles);

            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            _context.UpdateUser(model);

            return RedirectToAction("Index");
        }


        public IActionResult UsersView()
        {
            //TODO: разбиение на страницы и страничная навигация + поиск по имени
            var users = _userManager
                .Users
                .Select(u => _mapper.Map<UserSimpleViewModel>(u))
                .ToList();

            return View(users);
        }

        public IActionResult RolesView()
        {
            var rolesViewModels = _roleManager
                .Roles
                .Select(x => _mapper.Map<RoleViewModel>(x))
                .ToList();


            return View(rolesViewModels);
        }

        public async Task<IActionResult> RoleEditView(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var roleViewModel = _mapper.Map<RoleViewModel>(role);

            var allPermissions = ClaimsHelper.GetAllPermissions();
            var roleClaims = await _roleManager.GetClaimsAsync(role);

            var rolePermissions = roleClaims.Select(x => x.Value).ToList();

            foreach (var permission in allPermissions)
            {
                if (rolePermissions.Any(a => permission.Value == a))
                {
                    permission.Selected = true;
                }
            }
            roleViewModel.RoleClaims = allPermissions;

            return View(roleViewModel);
        }


        [HttpPost]

        public async Task<IActionResult> RoleEditView(RoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var claims in roleClaims)
            {
                await _roleManager.RemoveClaimAsync(role, claims);
            }

            var selectedClaims = model.RoleClaims.Where(x => x.Selected).ToList();
            foreach (var claims in selectedClaims)
            {
                await _roleManager.AddClaimAsync(role, 
                    new System.Security.Claims.Claim(nameof(Permissions), claims.Value));
            }

            return RedirectToAction("RolesView");
        }
    }
}
