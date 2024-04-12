using SecurityLab.Models;

public class LegoSingleViewModel
{
    public Product Product { get; set; }
    public ProductPipeline Recommendations { get; set; }
    public List<Product> RecommendedProducts { get; set; }
}

