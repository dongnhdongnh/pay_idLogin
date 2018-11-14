using System.Collections.Generic;

namespace VakaxaIDServer.Models
{
    public class ActivityModel
    {
        public PagedResult<LogActionModel> Pagination { get; set; }
    }
}