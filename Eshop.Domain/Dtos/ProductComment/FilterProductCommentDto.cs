using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.ProductComment
{
    public class FilterProductCommentDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string? Email { get; set; }
        public string FullName { get; set; }
        public string Message { get; set; }
        public string? StrongPoint { get; set; }
        public string? WeakPoint { get; set; }
        public string CreateDate { get; set; }
     
    }
}
