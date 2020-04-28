using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GraceCareCenterOrder.Models
{
    public class UserRole
    {
        [Display(Name = "User Id")]
        public string UserId { get; set; }

        [Display(Name = "Role Id")]
        public string RoleId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }
    }

    public class RoleItem
    {
        [Display(Name = "Role Id")]
        public string RoleId { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}