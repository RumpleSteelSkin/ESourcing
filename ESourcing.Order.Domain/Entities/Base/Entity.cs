using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESourcing.Order.Domain.Entities.Base;

public abstract class Entity : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public Entity Clone()
    {
        return (Entity)MemberwiseClone();
    }
}