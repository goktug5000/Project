using Project.Data;

namespace Project.Models.Entities;

public class Capsule
{
    public Guid Id { get; set; }
    public Guid CreaterId { get; set; }
    //public List<User> AuthorizedUsers { get; set; }
    public DateTime ExpireDate { get; set; }
    public DateTime CrationDate { get; set; }
    public required string CapsuleName { get; set; }
    public string? CapsuleData { get; set; }
    public byte[]? Image { get; set; }
}
