namespace Eshop.Domain.Dtos.Account.User
{
    public class EditRoleDto : CreateRoleDto
    {
        public long Id { get; set; }
    }

    public enum EditRoleResult
    {
        Success,
        Error
    }
}
