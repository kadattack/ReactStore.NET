using PowerReact.Entities;

namespace PowerReact.Data;

public static class DbInitializer
{
    public static void Initialize(DataContext context)
    {
        if (context.Products.Any()) return;

        var products = new List<Product>
        {
            new Product
            {
                Name = "test1",
                Description = "test1",
                Price = 10,
                PictureUrl = "/images/products/glove-react2.png",
                Type = "React",
                Brand = "Gloves",
                QualityInStock = 100
            },
            new Product
            {
                Name = "test2",
                Description = "test2",
                Price = 10,
                PictureUrl = "/images/products/glove-react2.png",
                Type = "React",
                Brand = "Gloves",
                QualityInStock = 100
            }
        };

        foreach (var product in products)
        {
            context.Products.Add(product);
        }

        context.SaveChanges();
    }
}