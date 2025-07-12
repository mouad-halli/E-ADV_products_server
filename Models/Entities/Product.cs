public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Slide> Slides { get; set; } = new();
}
