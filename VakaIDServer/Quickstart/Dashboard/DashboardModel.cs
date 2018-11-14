using System.Collections.Generic;
using VakaxaIDServer.Models;
using VakaxaIDServer.Quickstart.Profile;

namespace VakaxaIDServer.Quickstart.Dashboard
{
    public class DashboardModel
    {
        public List<LogActionModel> LogActionModels { get; set; }
        public List<WebSessionModel> WebSessionModels { get; set; }
        public ApplicationUser UserInfo { get; set; }
        public SocialNetwork SocialNetwork { get; set; }
    }
}