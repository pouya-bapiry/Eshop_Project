using Eshop.Domain.Dtos.Paging;
using Eshop.Domain.Entities.Account.Role;

namespace Eshop.Domain.Dtos.Account.User
{
    public class FilterRoleDto : BasePaging
    {
        public long Id { get; set; }
        public string RoleName { get; set; }
        public List<Role> Roles { get; set; }

        #region Methods

        public FilterRoleDto SetRoles(List<Role> roles)
        {
            this.Roles = roles;
            return this;
        }

        public FilterRoleDto SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;

            return this;
        }



        #endregion
    }
}
