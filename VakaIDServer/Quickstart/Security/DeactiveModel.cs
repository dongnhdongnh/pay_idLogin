using System.ComponentModel.DataAnnotations;

namespace VakaxaIDServer.Quickstart.Security
{
    public class DeactiveModel
    {
        public string TabShow { get; set; } = "";
        public bool IsTabSelected { get; set; } = false;
        public string Code { get; set; }
        public int Status { get; set; } = SecurityViewModel.StatusDefault;
    }
}