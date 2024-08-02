using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CollectionManagerWebService.Models;

public class GameLink
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint ID { get; set; }
    [Required]
    public required string Link { get; set; } 

    public virtual Game? Game { get; set; }
}
