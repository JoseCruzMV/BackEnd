using System.Runtime.Serialization;

namespace Ecommerce.Domain;

public enum ProductStatus
{
    [EnumMember(Value = "Producto Inactivo")]
    Inactivo,
    [EnumMember(Value = "Producto activo")]
    Activo
}