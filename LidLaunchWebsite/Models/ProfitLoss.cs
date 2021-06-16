using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Models
{
    public class ProfitLoss
    {
        public List<Expense> Expenses { get; set; }
        public CostEsimate CostEstimate { get; set; }
    }
}