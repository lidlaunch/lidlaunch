using System.Collections.Generic;

namespace LidLaunchWebsite.Models
{
    public class User 
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }       
        public int Role { get; set; }        
    }
}