using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mqtt_broker.model
{
    /// <summary>
    /// Class to define the broker to get data from
    /// </summary>
    public class Broker
    {
        /// <summary>
        /// address of the broker
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// port of the broker
        /// </summary>
        public int Port { get; set; }
    }
}
