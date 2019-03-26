using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAESA.Models.Response
{
    public class ResponseClient<T>
    {
        public int TotalReg { get; set; }
        public int TotalPag { get; set; }
        public List<T> Items { get; set; }
    }
}