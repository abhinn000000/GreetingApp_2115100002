﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RepositoryLayer.Entity
{
    public class UserEntity
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        // Navigation Property
        public virtual ICollection<GreetingEntity> Greetings { get; set; }
    }
}
