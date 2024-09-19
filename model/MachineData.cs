using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mqtt_broker.model
{
    /// <summary>
    /// based on the output of the broker, the status data recieved
    /// </summary>
    public class StatusData
    {
        public List<int> TemperatureZones { get; set; }
        public InjectionUnitStatus InjectionUnitStatus { get; set; }
        public int PartsProduced { get; set; }
    }

    /// <summary>
    /// based on the output of the broker, the injection unit status
    /// </summary>
    public class InjectionUnitStatus
    {
        public int CushionVolume { get; set; }
        public int HydraulicPressureMaximum { get; set; }
        public int InjectionSpeedAverage { get; set; }
    }

    /// <summary>
    /// based on the output of the broker, the machine data
    /// </summary>
    public class MachineData
    {
        public string MachineStatus { get; set; }
        public int ProductionCounter { get; set; }
        public StatusData StatusData { get; set; }
        public string Timestamp { get; set; }
        public string Alarm {  get; set; }
    }
}
