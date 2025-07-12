public class Slide
{
    public int Id { get; set; }

    public required string Url { get; set; }
    public int OrderNumber { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}
