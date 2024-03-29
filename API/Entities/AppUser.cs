﻿namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }
    public string Mail { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    
}
