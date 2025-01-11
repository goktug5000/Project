namespace Project.Models;

public class AddCapsuleDto
{
    public DateTime ExpireDate { get; set; }
    public required string CapsuleName { get; set; }
    public string? CapsuleData { get; set; }
    public byte[]? Image { get; set; }
}
