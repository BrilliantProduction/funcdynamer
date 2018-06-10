using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Dependencies
{
    public class DependencyOptions
    {
        public static DependencyOptions Default = new DependencyOptions();

        public bool AllowCircularDependencies { get; set; }
    }
}
