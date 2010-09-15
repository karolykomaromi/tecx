using System;

using Microsoft.Practices.Unity;

namespace TecX.Unity.AutoRegistration
{
    internal class RunOnceRegistration : Registration
    {
        private bool _ranOnce;

        public RunOnceRegistration(Action<Type, IUnityContainer> registrator, 
                                   IUnityContainer container) 
            : base(new Filter<Type>(t => true, "Runs once"), registrator, container)
        {
            _ranOnce = false;
        }

        public override void RegisterIfSatisfiesFilter(Type type)
        {
            if(!_ranOnce)
            {
                base.RegisterIfSatisfiesFilter(type);
                _ranOnce = true;
            }
        }
    }
}