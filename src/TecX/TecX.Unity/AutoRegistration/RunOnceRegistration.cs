using System;

using Microsoft.Practices.Unity;

namespace TecX.Unity.AutoRegistration
{
    public class RunOnceRegistration : Registration
    {
        private bool _ranOnce;

        public RunOnceRegistration(Action<Type, IUnityContainer> registrator) 
            : base(new Filter<Type>(t => true, "Runs once"), registrator)
        {
            _ranOnce = false;
        }

        public override void RegisterIfSatisfiesFilter(Type type, IUnityContainer container)
        {
            if(!_ranOnce)
            {
                base.RegisterIfSatisfiesFilter(type, container);
                _ranOnce = true;
            }
        }
    }
}