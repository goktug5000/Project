namespace Project.Models;

public class AddCapsuleDto
{
    public DateTime LockDate { get; set; }
    public required string CapsuleName { get; set; }
    public string? CapsuleText { get; set; }
    public byte[]? Image { get; set; }
}
