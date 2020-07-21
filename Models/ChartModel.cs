using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class ChartModel
    {
        public List<ChartFields> ChartData { get; set; }
    }
    public class ChartFields
    {
        public string EmployeeName { get; set; }
        public int Salery { get; set; }
    }
}