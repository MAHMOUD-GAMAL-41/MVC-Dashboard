using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    //[Authorize(Roles = "Admin")]

    public class RolesController : Controller
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RolesController(RoleManager<ApplicationRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var Roles = await roleManager.Roles.ToListAsync();
            return View(Roles);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]


        public async Task<IActionResult> Create(ApplicationRole role)
        {
            if(ModelState.IsValid)
            {
                var result =await roleManager.CreateAsync(role);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(role);
        }
        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {

            if (id == null) { return NotFound(); }

            var role = await roleManager.FindByIdAsync(id);
            if (role is null) { return NotFound(); }

            return View(viewName, role);
        }
        public async Task<IActionResult> Update(string id)
        {
            return await Details(id, "Update");
        }
        [HttpPost]
        public async Task<IActionResult> Update(string id, ApplicationRole applicationRole)
        {
            if (id != applicationRole.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(id);
                role.Name = applicationRole.Name;
                role.NormalizedName = applicationRole.Name.ToUpper();
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }


            }
            return View(applicationRole);
        }


        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var role = await roleManager.FindByIdAsync(id);
            if (role is null) { return NotFound(); }
            var result = await roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role =await roleManager.FindByIdAsync(roleId);
            if (role is null)
            
                return NotFound();
            ViewBag.RoleId = roleId;
            var usersInRole = new List<UserInRoleViewModel>();
            foreach(var user in await userManager.Users.ToListAsync())
            {
                var userIdRole = new UserInRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if(await userManager.IsInRoleAsync(user, role.Name))
                
                    userIdRole.IsSelected= true;

                else 
                    userIdRole.IsSelected = false;
                usersInRole.Add(userIdRole);
            }
            return View(usersInRole);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId,List<UserInRoleViewModel> users)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null)

                return NotFound();
            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var AppUser = await userManager.FindByIdAsync(user.UserId);
                    if (AppUser != null)
                    {

                        if (user.IsSelected && !await (userManager.IsInRoleAsync(AppUser, role.Name)))
                            await userManager.AddToRoleAsync(AppUser, role.Name);
                        else if (!(user.IsSelected) && await (userManager.IsInRoleAsync(AppUser, role.Name)))
                            await userManager.RemoveFromRoleAsync(AppUser, role.Name);
                    }
                }
                return RedirectToAction("Update", new { id = roleId });
            }
            return View(users);
        }


    }
}
