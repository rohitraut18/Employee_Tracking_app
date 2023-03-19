using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeTrackingWebApp.Models
{
    public class Tracking
    {
        public int TrackingId { get; set; }

        [Required]
        public int AppUserId { get; set; }
        [Required]
        public double Lat { get; set; }
        [Required]
        public double Long { get; set; } 
        public DateTime OnDateTime { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}