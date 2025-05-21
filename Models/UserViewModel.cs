﻿namespace E_commerceTechnologyWebsite.Models
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime? LockoutEndTime { get; set; }
        public bool IsLocked => LockoutEndTime.HasValue && LockoutEndTime > DateTime.Now;

    }
}
