namespace Project.Models.Entities;

public class Capsule
{
    public required Guid Id { get; set; }
    public required Guid CreaterId { get; set; }
    public required Guid OwnerId { get; set; }
    public DateTime CrationDate { get; set; }
    public DateTime LockDate { get; set; }
    public DateTime ExpireDate { get; set; }
    public required string CapsuleName { get; set; }
    public CapsuleData? CapsuleData { get; set; }
}
