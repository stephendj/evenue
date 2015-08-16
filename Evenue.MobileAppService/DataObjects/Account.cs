using Microsoft.Azure.Mobile.Server;

namespace Evenue.MobileAppService.DataObjects
{
    public class Account : EntityData
    {
        public string Username { get; set; }
        public byte[] Salt { get; set; }
        public byte[] SaltedAndHashedPassword { get; set; }
    }
}