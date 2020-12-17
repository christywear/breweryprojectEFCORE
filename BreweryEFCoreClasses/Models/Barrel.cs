using System;
using System.Collections.Generic;

namespace BreweryEFCoreClasses.Models
{
    public partial class Barrel
    {
        public int BrewContainerId { get; set; }
        public string Treatment { get; set; }

        public virtual BrewContainer BrewContainer { get; set; }
    }
}
