using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking
{
    public class NetworkManagerSettings
    {
        public int BufferSize { get; set; } = 65535;
        public int ReceiveOperationCount { get; set; } = 25;
        public int SendOperationCount { get; set; } = 25;
    }
}
