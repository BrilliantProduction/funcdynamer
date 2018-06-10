using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Dependencies
{
    public class DependencyGraph
    {
        private object _model;

        public DependencyGraph(object model)
        {
            this._model = model;
        }

        public void TriggerDependenciesCheck()
        {
        }
    }
}
