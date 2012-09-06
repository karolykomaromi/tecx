namespace TecX.Unity.Test
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    using Moq;

    using TecX.Common;

    using Xunit;

    public class DefaultLifetimeFixture
    {
        [Fact]
        public void CanReplaceBuilderStrategy()
        {
            StagedStrategyChain<UnityBuildStage> stagedStrategyChain = new StagedStrategyChain<UnityBuildStage>();

            stagedStrategyChain.AddNew<LifetimeStrategy>(UnityBuildStage.Lifetime);

            StagedStrategyChainExtensions.Replace<LifetimeStrategy, DefaultLifetimeStrategy>(stagedStrategyChain);

            IStrategyChain chain = stagedStrategyChain.MakeStrategyChain();

            var mock = new Mock<IBuilderContext>();

            chain.ExecuteBuildUp(mock.Object);

            Assert.False(LifetimeStrategy.Called);
            Assert.True(DefaultLifetimeStrategy.Called);
        }

        [Fact]
        public void CanInsertNewBuilderStrategyBefore()
        {
            int index = 0;
            Func<int> getNextIndex = () => index++;
            StagedStrategyChain<UnityBuildStage> stagedStrategyChain = new StagedStrategyChain<UnityBuildStage>();

            stagedStrategyChain.Add(new LifetimeStrategy(getNextIndex), UnityBuildStage.Lifetime);

            StagedStrategyChainExtensions.InsertBefore(
                stagedStrategyChain, new DefaultLifetimeStrategy(getNextIndex), typeof(LifetimeStrategy));

            IStrategyChain chain = stagedStrategyChain.MakeStrategyChain();

            var mock = new Mock<IBuilderContext>();

            chain.ExecuteBuildUp(mock.Object);

            Assert.Equal(0, DefaultLifetimeStrategy.Index);
            Assert.Equal(1, LifetimeStrategy.Index);
        }

        class LifetimeStrategy : BuilderStrategy
        {
            private readonly Func<int> getNextIndex;

            public LifetimeStrategy()
                : this(() => 0)
            {
            }

            public LifetimeStrategy(Func<int> getNextIndex)
            {
                this.getNextIndex = getNextIndex;
            }

            public override void PreBuildUp(IBuilderContext context)
            {
                Called = true;
                Index = this.getNextIndex();
            }

            public static bool Called { get; set; }

            public static int Index { get; set; }
        }

        class DefaultLifetimeStrategy : BuilderStrategy
        {
            private readonly Func<int> getNextIndex;

            public DefaultLifetimeStrategy()
                : this(() => 0)
            {
            }

            public DefaultLifetimeStrategy(Func<int> getNextIndex)
            {
                this.getNextIndex = getNextIndex;
            }

            public override void PreBuildUp(IBuilderContext context)
            {
                Called = true;
                Index = this.getNextIndex();
            }

            public static bool Called { get; set; }

            public static int Index { get; set; }
        }
    }

    public static class StagedStrategyChainExtensions
    {
        private static readonly FieldInfo field;

        static StagedStrategyChainExtensions()
        {
            field = typeof(StagedStrategyChain<UnityBuildStage>).GetField("stages", BindingFlags.Instance | BindingFlags.NonPublic);

            if (field == null)
            {
                throw new FieldNotFoundException(typeof(StagedStrategyChain<UnityBuildStage>), "stages");
            }
        }

        public static void InsertBefore(StagedStrategyChain<UnityBuildStage> chain, IBuilderStrategy strategyToInsert, Type insertBefore)
        {
            Guard.AssertNotNull(chain, "chain");
            Guard.AssertNotNull(strategyToInsert, "strategyToInsert");
            Guard.AssertNotNull(insertBefore, "insertBefore");

            var stages = (List<IBuilderStrategy>[])field.GetValue(chain);

            foreach (List<IBuilderStrategy> stage in stages)
            {
                for (int i = 0; i < stage.Count; i++)
                {
                    if (stage[i].GetType() == insertBefore)
                    {
                        stage.Insert(i, strategyToInsert);
                        return;
                    }
                }
            }
        }

        public static void Replace<TOld, TNew>(StagedStrategyChain<UnityBuildStage> chain)
            where TOld : BuilderStrategy
            where TNew : BuilderStrategy, new()
        {
            Guard.AssertNotNull(chain, "chain");

            List<IBuilderStrategy>[] stages = (List<IBuilderStrategy>[])field.GetValue(chain);

            foreach (List<IBuilderStrategy> stage in stages)
            {
                for (int i = 0; i < stage.Count; i++)
                {
                    if (stage[i].GetType() == typeof(TOld))
                    {
                        stage[i] = new TNew();
                    }
                }
            }
        }
    }

    public class DefaultLifetimeStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            if (context.Existing == null)
            {
                ILifetimePolicy lifetimePolicy = GetLifetimePolicy(context);
                IRequiresRecovery recovery = lifetimePolicy as IRequiresRecovery;
                if (recovery != null)
                {
                    context.RecoveryStack.Add(recovery);
                }

                object existing = lifetimePolicy.GetValue();
                if (existing != null)
                {
                    context.Existing = existing;
                    context.BuildComplete = true;
                }
            }
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            ILifetimePolicy lifetimePolicy = GetLifetimePolicy(context);

            lifetimePolicy.SetValue(context.Existing);
        }

        private static ILifetimePolicy GetLifetimePolicy(IBuilderContext context)
        {
            ILifetimePolicy policy = context.Policies.GetNoDefault<ILifetimePolicy>(context.BuildKey, false);
            if (policy == null && context.BuildKey.Type.IsGenericType)
            {
                policy = GetLifetimePolicyForGenericType(context);
            }

            if (policy == null)
            {
                policy = GetDefaultLifetimePolicy(context);
                context.PersistentPolicies.Set<ILifetimePolicy>(policy, context.BuildKey);
            }

            return policy;
        }

        private static ILifetimePolicy GetDefaultLifetimePolicy(IBuilderContext context)
        {
            IPolicyList factorySource;
            IDefaultLifetimeFactoryPolicy factoryPolicy = context.Policies.Get<IDefaultLifetimeFactoryPolicy>(context.BuildKey, out factorySource);

            if (factoryPolicy != null)
            {
                ILifetimePolicy lifetime = factoryPolicy.CreateLifetimePolicy();
                factorySource.Set<ILifetimePolicy>(lifetime, context.BuildKey);
                return lifetime;
            }

            return null;
        }

        private static ILifetimePolicy GetLifetimePolicyForGenericType(IBuilderContext context)
        {
            Type typeToBuild = context.BuildKey.Type;

            object openGenericBuildKey = new NamedTypeBuildKey(typeToBuild.GetGenericTypeDefinition(), context.BuildKey.Name);

            IPolicyList factorySource;
            ILifetimeFactoryPolicy factoryPolicy = context.Policies.Get<ILifetimeFactoryPolicy>(openGenericBuildKey, out factorySource);

            if (factoryPolicy != null)
            {
                ILifetimePolicy lifetime = factoryPolicy.CreateLifetimePolicy();
                return lifetime;
            }

            return null;
        }
    }

    public interface IDefaultLifetimeFactoryPolicy : IBuilderPolicy
    {
        ILifetimePolicy CreateLifetimePolicy();
    }

    public class DefaultLifetimeManagerFactory : IDefaultLifetimeFactoryPolicy
    {
        private readonly ExtensionContext containerContext;

        private Type lifetimeType;

        public DefaultLifetimeManagerFactory(ExtensionContext containerContext, Type lifetimeType)
        {
            Guard.AssertNotNull(containerContext, "containerContext");
            Guard.AssertNotNull(lifetimeType, "lifetimeType");

            this.containerContext = containerContext;
            this.LifetimeType = lifetimeType;
        }

        public Type LifetimeType
        {
            get
            {
                return this.lifetimeType;
            }

            set
            {
                Guard.AssertNotNull(value, "LifetimeType");
                this.lifetimeType = value;
            }
        }

        public ILifetimePolicy CreateLifetimePolicy()
        {
            var lifetime = (LifetimeManager)this.containerContext.Container.Resolve(this.LifetimeType);

            if (lifetime is IDisposable)
            {
                this.containerContext.Lifetime.Add(lifetime);
            }

            ////lifetime.InUse = true;

            return lifetime;
        }
    }

    public class FieldNotFoundException : Exception
    {
        private readonly Type type;

        private readonly string fieldName;

        public FieldNotFoundException(Type type, string fieldName)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertNotEmpty(fieldName, "fieldName");

            this.type = type;
            this.fieldName = fieldName;
        }

        public Type Type
        {
            get
            {
                return this.type;
            }
        }

        public string FieldName
        {
            get
            {
                return this.fieldName;
            }
        }
    }
}
