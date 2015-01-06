using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public static class Data
    {
        private static List<string> data { get; set; }

        public static List<string>  GetData()
        {
            if (data != null && data.Count > 0)
            {
                return data;
            }
           return data=new List<string>(){"a","b","c"};
        }

        public static void UpData()
        {
            data.Add("haha");
        }
    }
}
