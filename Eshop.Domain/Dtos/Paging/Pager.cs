

namespace Eshop.Domain.Dtos.Paging
{
    public class Pager
    {
        public static BasePaging Build(int pageId, int allEntitiesCount, int take, int howManyShowPageAfterAndBefore)
        {
            var pageCount = Convert.ToInt32(Math.Ceiling(allEntitiesCount / (double)take));

            return new BasePaging
            {
                PageId = pageId,
                AllEntitiesCount = allEntitiesCount,
                TakeEntity = take,
                SkipEntity = (pageId - 1) * take,
                StartPage = pageId - howManyShowPageAfterAndBefore <= 0 ? 1 : pageId - howManyShowPageAfterAndBefore,
                EndPage = pageId + howManyShowPageAfterAndBefore > pageCount ? pageCount : pageId + howManyShowPageAfterAndBefore,
                PageCount = pageCount,
                HowManyShowPageAfterAndBefore = howManyShowPageAfterAndBefore
            };
        }
    }
}
