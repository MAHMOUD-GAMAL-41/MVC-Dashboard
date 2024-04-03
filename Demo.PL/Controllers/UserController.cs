using Demo.BLL.Repositories;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index(string searchValue ="")

        {
            List<ApplicationUser> users;
            if (string.IsNullOrEmpty(searchValue))
            {
                users = await userManager.Users.ToListAsync();
            }
            else
            {
                users = await userManager.Users.Where(user => user.Email.Trim().ToLower()
                                               .Contains(searchValue.ToLower().Trim())).ToListAsync();
            }
            return View(users);
        }
        public async Task<IActionResult> Details(string id,string viewName= "Details")
        {

            if (id == null) { return NotFound(); }

            var user = await userManager.FindByIdAsync(id);
            if (user is null) { return NotFound(); }

            return View(viewName,user);


        }
        public async Task<IActionResult> Update(string id)
        {
            return await Details(id, "Update");
        }
        [HttpPost]
        public async Task<IActionResult> Update(string id,ApplicationUser applicationUser)
        {
            if(id!= applicationUser.Id)
                return NotFound();
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(id);
                user.UserName=applicationUser.UserName;
                user.NormalizedUserName = applicationUser.UserName.ToUpper();
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }


            }
            return View(applicationUser);
        }
        

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var user = await userManager.FindByIdAsync(id);
            if (user is null) { return NotFound(); }
            var result = await userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));

        }
    }
}
