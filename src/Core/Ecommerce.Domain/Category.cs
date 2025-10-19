using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Domain.Common;

namespace Ecommerce.Domain;

public class Category : BaseDomainModel
{
    [Column(TypeName = "NVARCHAR(4000)")]
    public string? Nombre { get; set; }
}