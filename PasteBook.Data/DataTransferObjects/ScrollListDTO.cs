using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.DataTransferObjects
{
    public class ScrollListDTO<T>
    {
        public List<T> Data { get; private set; }
        public int PageNumber { get; private set; }
        public int CountPerScroll { get; private set; }
        public int TotalCount { get; private set; }

        public ScrollListDTO(List<T> items, int count, int pageNumber, int itemsPerScroll)
        { 
            Data = items;
            PageNumber = pageNumber;
            CountPerScroll = itemsPerScroll;
            TotalCount = count;
        }

        public static async Task<ScrollListDTO<T>> GetScrollListAsync(IQueryable<T> source, int pageNumber, int itemsPerScroll)
        {
            var count = source.Count();
            var items = await source.Take(pageNumber * itemsPerScroll).ToListAsync();

            return new ScrollListDTO<T>(items, count, pageNumber, itemsPerScroll);
        }
    }
}
