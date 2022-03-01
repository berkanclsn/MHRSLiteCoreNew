using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntityLayer.PagingListModels
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public List<T> ItemList { get; set; }

        public PaginatedList(List<T> items, int count, int pageindex, int pageSize)
        {
            PageIndex = pageindex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
            ItemList = items;
        }
        public bool PreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool NextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static PaginatedList<T> CreateAsync(List<T> sourceList, int pageindex, int pageSize)
        {
            var count = sourceList.Count;
            //Bulunduğum sayfadan bir eksiltip sayfada kaç eleman olacaksa o kadar veriyi atla
            var items = sourceList.Skip((pageindex - 1) * pageSize).
                //sayfada kaç eleman olmasını istiyorsam o kadarını al
                Take(pageSize).
                //Listele
                ToList();
            //oluşan veriyi yeni nesne yaratarak gönder
            return new PaginatedList<T>(items, count, pageindex, pageSize);
        }
    }
}
