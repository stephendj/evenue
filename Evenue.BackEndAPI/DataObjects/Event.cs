﻿using Microsoft.WindowsAzure.Mobile.Service;
using System;

namespace Evenue.BackEndAPI.DataObjects
{
    public class Event : EntityData
    {
        //public User User { get; set; }
        public string UserId { get; set; }
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