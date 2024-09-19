using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mqtt_broker.model
{
    public class Subscription
    {
        public string Topic { get; set; }
        public int QoS { get; set; }
    }
}
