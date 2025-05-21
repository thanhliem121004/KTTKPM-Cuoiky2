using Microsoft.AspNetCore.Identity;
using System.Security;

namespace E_commerceTechnologyWebsite.Models
{
	public class AppUserModel : IdentityUser
	{
		public string Occupation {  get; set; }
		public string RoleId { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LockoutEndTime { get; set; }

    }
}
