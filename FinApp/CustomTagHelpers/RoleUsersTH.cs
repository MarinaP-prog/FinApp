using FinApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace FinApp.CustomTagHelpers {
    [HtmlTargetElement("td", Attributes = "i-role")]
    public class RoleUsersTH : TagHelper {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleUsersTH(UserManager<AppUser> usermgr, RoleManager<IdentityRole> rolemgr) {
            userManager = usermgr;
            roleManager = rolemgr;
        }

        [HtmlAttributeName("i-role")]
        public string Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            List<string> names = new List<string>();
            IdentityRole role = await roleManager.FindByIdAsync(Role);

            if (role != null) {
                var users = await userManager.Users.AsNoTracking().ToListAsync();

                foreach (var user in users) {
                    if (user != null && await userManager.IsInRoleAsync(user, role.Name)) {
                        names.Add(user.UserName);
                    }
                }
            }

            output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names));
        }
    }
}
