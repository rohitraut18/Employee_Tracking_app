using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeTrackingWebApp.Models
{
    public class AppUser
    {
        public int AppUserId { get; set; }

        [Required(ErrorMessage = "Enetr full name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Enter mobile no")]
        [StringLength(10,ErrorMessage ="Mobile no must be 10 digit")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Mobile no must be 10 digit")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage ="Enter email id")]
        [StringLength(256, ErrorMessage = "Email id can be 256 characters")]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Enter valid email id")]
        public string EmailId { get; set; }

        
        [Required(ErrorMessage ="Enter password")]
        [StringLength(30,ErrorMessage = "Maximum password length is 30")]
        public string Password { get; set; }

        [Required]
        public int AppRoleId { get; set; }
          
        public virtual AppRole AppRole { get; set; }
    }
}