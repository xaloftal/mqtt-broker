using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mqtt_broker.model
{
    /// <summary>
    /// Class for the recipies, to use for processing data
    /// </summary>
    public class Recipe
    {
        /// <summary>
        /// name of the recipe
        /// </summary>
        public string Name { get; set; }
       
        /// <summary>
        /// Recipe description, explains what it does
        /// </summary>
        public string Description { get; set; }
          
        /// <summary>
        /// The strategy (ex: sum, average...)
        /// </summary>
        public string Strategy { get; set; }
       
        /// <summary>
        /// How long data is collected before processing it
        /// </summary>
        public int WindowSize { get; set; }

        /// <summary>
        /// What topics this recipe is applied to
        /// </summary>
        public List<string> Topics { get; set; }

    }
}
