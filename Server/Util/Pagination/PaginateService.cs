using Amazon.Runtime;
using Microsoft.EntityFrameworkCore;

namespace Server.Util.Pagination
{
    public class PaginateService
    {
        public static PaginateResponse<T> ToPaginateList<T>(IQueryable<T> source, PaginateRequest paginationRequest)
        {
            return new PaginateResponse<T>
            {
                TotalCount = source.Count(),
                Items = source.Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize).Take(paginationRequest.PageSize).ToList(),
                PageSize = paginationRequest.PageSize,
                CurrentPage = paginationRequest.PageNumber,
            };
        }

        public static async Task<PaginateResponse<T>> ToPaginateListAsync<T>(IQueryable<T> source, PaginateRequest paginationRequest)
        {
            return new PaginateResponse<T>
            {
                TotalCount = source.Count(),
                Items = await source
                    .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
                    .Take(paginationRequest.PageSize)
                    .ToListAsync(),
                PageSize = paginationRequest.PageSize,
                CurrentPage = paginationRequest.PageNumber,
            };
        }
    }
}
