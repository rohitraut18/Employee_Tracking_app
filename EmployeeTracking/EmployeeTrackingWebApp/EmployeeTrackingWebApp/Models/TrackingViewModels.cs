using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeTrackingWebApp.Models
{
    public class TrackingDuration 
    {
        public int AppUserId { get; set; }

        [Required(ErrorMessage = "From date.*")]
        public DateTime? FromDate { get; set; }

        [Required(ErrorMessage = "Hours.*")]
        public int FromHours { get; set; }

        [Required(ErrorMessage = "Minutes.*")]
        public int FromMinutes { get; set; }


        [Required(ErrorMessage = "To date.*")]
        public DateTime? ToDate { get; set; }

        [Required(ErrorMessage = "Hours.*")]
        public int ToHours { get; set; }

        [Required(ErrorMessage = "Minutes.*")]
        public int ToMinutes { get; set; }
    }
}