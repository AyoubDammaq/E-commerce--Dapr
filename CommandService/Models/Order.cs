using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [ForeignKey("User")]
    public int CustomerId { get; set; }

    [Required]
    [ForeignKey("Product")] 
    public int ProductId { get; set; }

    [Required] 
    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be at least 1")] 
    public int Quantity { get; set; }

    [Required] 
    public DateTime OrderDate { get; set; }

    [Required] 
    [StringLength(20)]
    [RegularExpression(@"^(Pending|Completed|Cancelled)$", ErrorMessage = "Invalid status")] 
    public string Status { get; set; } = "Pending";

}
