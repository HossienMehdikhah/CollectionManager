using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CollectionManagerWebService.Models;

public class Game
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint? ID { get; set; }
    [Required]
    public required string Name { get; set; } 
    [Range(1, 10)]
    public byte Praiority { get; set; } = 1;

    public virtual List<GameLink> Links { get; set; } = [];
}
