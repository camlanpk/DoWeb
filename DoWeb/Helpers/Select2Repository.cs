using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoWeb.Helpers
{
    public class Select2OptionModel
    {
        public dynamic id { get; set; }
        public string text { get; set; }
        public string textextra { get; set; }
        public dynamic dataAttr { get; set; }
    }

    public class Select2PagedResult
    {
        public int Total { get; set; }
        public List<Select2OptionModel> Results { get; set; }
    }
}