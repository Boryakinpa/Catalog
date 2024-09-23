using System;
using System.Collections.Generic;

namespace CatalogNet;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ArticleNumber { get; set; }

    public string? Name { get; set; }

    public string? Unit { get; set; }

    public decimal? Cost { get; set; }

    public int? MaximumDiscountAmount { get; set; }

    public string? Manufacturer { get; set; }

    public string? Provider { get; set; }

    public int? Category { get; set; }

    public int? CurrentDiscount { get; set; }

    public int? QuantityInStock { get; set; }

    public string? Description { get; set; }

    public string? Photo { get; set; }

    public string ImagePath => string.IsNullOrEmpty(Photo) ? "C:\\Users\\79069\\Desktop\\Коты\\2.jpg" : Photo;
    public string _imagePath;
    public string BackgroundColor => QuantityInStock > 0 ? "White" : "Gray";

    public virtual ProductCategory? CategoryNavigation { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    //public Product(string imagePath, string name = "None", string description = "None", int cost = 0, int quantityInStock = 0, string manufacturer = "None") 
    //{
    //    ImagePath = string.IsNullOrEmpty(imagePath) ? "C:\\Users\\79069\\Desktop\\Коты\\2.jpg" : imagePath;
    //    Name = name;
    //    Description = description;
    //    Cost = cost;
    //    QuantityInStock = quantityInStock;
    //    Manufacturer = manufacturer;
    //}
}
