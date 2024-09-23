using System;
using System.Collections.Generic;

namespace CatalogNet;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly? OrderDate { get; set; }

    public DateOnly? DeliveryDate { get; set; }

    public int? PickUpPointId { get; set; }

    public int? UserId { get; set; }

    public int? PickUpCode { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual PickUpPoint? PickUpPoint { get; set; }

    public virtual User? User { get; set; }
}
