using System;
using System.Collections.Generic;
using System.Text;

namespace DATA.Domain.Views
{
    public class ViewPagination
    {
        //Objeto padrao para paginacao dos resultados
        public ViewPagination(int? count=0)
        {
            Page = 0;
            OffSet = 20;
            Count = count ?? 0;
        }
        public int Page { get; set; }
        public int Count { get; set; }
        public int OffSet { get; set; }

        public int StartAt()
        {
            return Page * OffSet;
        }
        public int TotalPages()
        {
            int total = (int)(Count / OffSet);
            if (Count % OffSet > 0)
                total += 1;
            return total;
        }
    }
}
