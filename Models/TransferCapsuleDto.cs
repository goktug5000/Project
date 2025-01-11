using Project.Data;

namespace Project.Models;

public class TransferCapsuleDto
{
    public required User NewOwner { get; set; }
}
