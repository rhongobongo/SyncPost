using AndroidX.ConstraintLayout.Core.Widgets;
using SyncPostUIRedo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Android.Net;

namespace SyncPostUIRedo.Platforms.Android
{
    public class AndroidMessageHandler : IPlatformHTTPMessageHandler
    {
        public HttpMessageHandler GetHttpMessageHandler() =>
           new Xamarin.Android.Net.AndroidMessageHandler
           {
               ServerCertificateCustomValidationCallback = (httpRequestMessage, certificate, chain, sslPolicyErrors) =>
                   certificate?.Issuer == "CN=localhost" || sslPolicyErrors == SslPolicyErrors.None
           };
    }
}
   

