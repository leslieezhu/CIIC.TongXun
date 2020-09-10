using System.Collections.Generic;

namespace CIIC.TongXun.Areas.Admin
{
    public class SelectOption
    {
        public int id { get; set; }

        public string text { get; set; }

        public List<SelectOption> children { get; set; }
    }
}