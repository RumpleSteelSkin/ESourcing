using System.ComponentModel.DataAnnotations.Schema;
using ESourcing.Order.Domain.Entities.Base;

namespace ESourcing.Order.Domain.Entities;

public class Order : Entity
{
    public string? AuctionId { get; set; }
    public string? SellerUserName { get; set; }
    public string? ProductId { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; }
}