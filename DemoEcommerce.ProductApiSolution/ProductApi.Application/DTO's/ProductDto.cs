using System.ComponentModel.DataAnnotations;

namespace ProductApi.Application.DTO_s
{
    public record ProductDto(int Id ,
        [Required] string Name ,
        [Required][DataType(DataType.Currency)] decimal Price ,
        [Required][Range(1, int.MaxValue)] int Quantity);
}
