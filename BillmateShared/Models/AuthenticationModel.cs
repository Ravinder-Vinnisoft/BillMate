﻿using System;
using System.ComponentModel.DataAnnotations;
namespace BillMate.Models
{
    public class AuthenticationModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
