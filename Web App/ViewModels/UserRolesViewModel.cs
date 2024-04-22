using Microsoft.AspNetCore.Identity;

namespace Identity.ViewModels
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}
