﻿namespace TecX.Unity.Decoration
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class DecoratorExtension : UnityContainerExtension
    {
        private RegistrationStack registrationStack;

        protected override void Initialize()
        {
            this.registrationStack = new RegistrationStack();
            this.Context.Registering += this.OnRegistering;

            this.Context.Strategies.Add(new DecoratorStrategy(this.registrationStack), UnityBuildStage.PreCreation);
        }

        private void OnRegistering(object sender, RegisterEventArgs e)
        {
            if (!e.TypeFrom.IsInterface)
            {
                return;
            }

            NamedTypeBuildKey key = new NamedTypeBuildKey(e.TypeFrom, e.Name);

            ICollection<Type> stack = this.registrationStack[key];

            stack.Add(e.TypeTo);
        }
    }
}