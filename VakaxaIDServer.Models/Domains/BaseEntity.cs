using VakaxaIDServer.Commons.Helpers;

namespace VakaxaIDServer.Models.Domains
{
    public class BaseEntity
    {
        public string Id { get; set; } = CommonHelper.GenerateUuid();
        public long CreatedAt { get; set; } = CommonHelper.GetUnixTimestamp();
        public long UpdatedAt { get; set; } = CommonHelper.GetUnixTimestamp();
    }
}