using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Util
{
    /// <summary>
    /// 分页基本参数
    /// </summary>
    public class PageInputBase
    {
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SearchText { get; set; }
    }
}
