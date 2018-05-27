using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POEApi.Model
{
    public class FatedUniqueInfo
    {
        /// <summary>
        /// The name of the unique item that will be upgraded.
        /// </summary>
        public string TargetItemName { get; set; }
        /// <summary>
        /// The name of the item that is created from upgrading the target item.
        /// </summary>
        public string FatedItemName { get; set; }
        /// <summary>
        /// The name of the base type of the fated unique.
        /// </summary>
        public string BaseTypeName { get; set; }
        /// <summary>
        /// The name of the prophecy that upgrades the target item.
        /// </summary>
        public string ProphecyName { get; set; }
    }
}
