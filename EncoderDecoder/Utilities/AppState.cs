using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncoderDecoder.Utilities
{
    public class AppState
    {
        public string CurrentRoute { get; private set; }

        public string DomainName { get; private set; } = string.Empty;

        public event Action<string> OnCurrentRouteChange;

        public void SetCurrentRoute(string newRoute)
        {
            CurrentRoute = newRoute;
            NotifyStateChanged(CurrentRoute);
        }
        public void SetDomainName(string domainName)
        {
            if (!string.IsNullOrWhiteSpace(domainName))
                this.DomainName = domainName.ToLower();
            Console.WriteLine("SetDomainName : " + this.DomainName + " == " + domainName);
        }
        public string GetWebsiteName()
        {
            return (string.IsNullOrWhiteSpace(this.DomainName)  || this.DomainName.Contains("encoder")) ? "Encoder Decoder" : this.DomainName.Contains("ecrypt") ? "Ecrypt Decrypt" : this.DomainName;
        }
        private void NotifyStateChanged(string CurrentRoute) => OnCurrentRouteChange?.Invoke(CurrentRoute);
    }
}
