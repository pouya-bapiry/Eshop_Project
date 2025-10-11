namespace Eshop.Domain.Dtos.ProductCategory
{
    public class EditProductCategoryDto : CreateProductCategoryDto
    {
        public long Id { get; set; }
    }

    public enum EditProductCategoryResult
    {
        NotFound,
        Error,
        Success,
        ImageErrorType,
    }
}
