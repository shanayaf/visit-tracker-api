﻿namespace VisitTracker.API.Dtos
{
    public class CreateUserDto
    {
        public string Username { get; set; } = string.Empty;


        public string Email { get; set; } = string.Empty;
        public string? Role { get; set; }
    }
}