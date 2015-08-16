using Microsoft.Azure.Mobile.Server;

namespace Evenue.MobileAppService.DataObjects
{
    public class Event : EntityData
    {
        public string title { get; set; }
        public string location { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string desc { get; set; }
        public string category { get; set; }
        public int fee { get; set; }

        // For storing images data
        public string containerName { get; set; }
        public string resourceName { get; set; }
        public string sasQueryString { get; set; }
        public string imageUri { get; set; }
    }
}