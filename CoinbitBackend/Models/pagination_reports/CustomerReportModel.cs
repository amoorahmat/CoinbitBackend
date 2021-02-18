using CoinbitBackend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinbitBackend.Models.pagination_reports
{
    public class CustomerReportModel : Customer
    {
        public int row_count { get; set; }  
    }
}
