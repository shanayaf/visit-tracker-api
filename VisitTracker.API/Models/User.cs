﻿using System.ComponentModel.DataAnnotations;

namespace VisitTracker.API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
      //  public string Email { get; set; } = string.Empty;

        public string? Role { get; set; }

       
        public ICollection<Visit> Visits { get; set; } = new List<Visit>();
    }
}
