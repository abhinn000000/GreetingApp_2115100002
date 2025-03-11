using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryLayer.Entity
{
    public class GreetingEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Greeting { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
