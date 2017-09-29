using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.Model.Models
{
    public class Page<T>
    {
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }

        public Page()
        {

        }

        public Page(int PageIndex, int PageSize,int TotalCount, IEnumerable<T> Items)
        {
            this.PageIndex = PageIndex;
            this.PageSize = PageSize;
            this.TotalCount = TotalCount;
            this.Items = Items;
            CalculatePageCount();
        }

        private void CalculatePageCount()
        {
            PageIndex = 0;
            if(PageSize>0)
            {
                PageCount = TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize+1;
            }
        }

    }
}
