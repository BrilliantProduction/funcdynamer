using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Dependencies
{
    public class MyClass : IDependencyHolder
    {
        public DependencyValue<MyClass, int> AwesomeProperty { get; set; }

        public DependencyValue<MyClass, int> AwesomeDependentProperty { get; set; }

        public DependencyGraph DependencyGraph { get; }

        public MyClass()
        {
            AwesomeProperty = new DependencyValue<MyClass, int>(this);
            AwesomeDependentProperty = new DependencyValue<MyClass, int>(this);

            var graphBuilder = DependencyGraphBuilder<MyClass>.GetBuilder(this);
            graphBuilder.CreateDependency(x => x.AwesomeProperty, x => x.AwesomeDependentProperty)
                        .WithAction((x) => AwesomeDependentProperty.Value = 0)
                        .Build();


            // This should throw error
            // cannot create circular dependencies, cause it may result in a deadlock
            // if you want to allow circular dependencies, init builder with options
            // in which set AllowCircularDependencies to true
            graphBuilder.CreateDependency(x => x.AwesomeDependentProperty, x => x.AwesomeProperty);

            DependencyGraph = graphBuilder.Build();
        }
    }
}
