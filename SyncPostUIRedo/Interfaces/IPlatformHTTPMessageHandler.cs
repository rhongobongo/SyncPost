using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncPostUIRedo.Interfaces
{
    public interface IPlatformHTTPMessageHandler
    {
        HttpMessageHandler GetHttpMessageHandler();
    }
}
