namespace CinemaMagic.AuthService.Entities;

public class PhoneCode
{
    public int Id { get; set; }
    public string Phone { get; set; }
    public string Code { get; set; }
    
    public bool IsActive { get; set; } = true;
}