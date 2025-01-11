namespace Project.Models.Entities;

public class CapsuleData
{
    public Guid Id { get; set; }
    public required string CapsuleText { get; set; }
    public byte[]? Image { get; set; }
}
