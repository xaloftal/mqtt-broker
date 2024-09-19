using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mqtt_broker.model
{
    public class Config
    {
        public Broker mqtt { get; set; }
        public List<Subscription> Subscriptions { get; set; }
        public string CurrentRecipe { get; set; }
        public List<Recipe> OtherRecipes { get; set; }
        public Output Output { get; set; }
    }
}
