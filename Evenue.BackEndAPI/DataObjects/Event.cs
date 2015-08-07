using Microsoft.WindowsAzure.Mobile.Service;
using System;

namespace Evenue.BackEndAPI.DataObjects
{
    public class Event : EntityData
    {
        public string title { get; set; }
        public string location { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string desc { get; set; }
        public string imageurl { get; set; }
        public string category { get; set; }
        public int fee { get; set; }
    }
}