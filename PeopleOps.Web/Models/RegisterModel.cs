using System.ComponentModel.DataAnnotations;

namespace PeopleOps.Web.Models;

public class RegisterModel
{
    [Required (ErrorMessage = "Email is required")]
    public string? Email { get; set; }
    
    [Required (ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    
    [Required (ErrorMessage = "Confirm Password is required")]
    [Compare(nameof(Password),ErrorMessage = "Password and Confirm Password do not match")]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }
    
    [Required (ErrorMessage = "First Name is required")]
    public string? FirstName { get; set; }
    
    [Required (ErrorMessage = "Last Name is required")]
    public string? LastName { get; set; }
}