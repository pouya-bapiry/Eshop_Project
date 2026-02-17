using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.ProductComment
{
    public class CreateProductCommentDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        //public string? ProductTitle { get; set; }
        public string? Email { get; set; }
        public string FullName { get; set; }
        public string Message { get; set; }
        public string? StrongPoint { get; set; }
        public string? WeakPoint { get; set; }



    }
    public enum CreateCommentsResult
    {
        Success,
        Error,
        NotFound
    }
}
