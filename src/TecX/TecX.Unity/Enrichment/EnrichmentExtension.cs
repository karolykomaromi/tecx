﻿using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace TecX.Unity.Enrichment
{
    public class EnrichmentExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.AddNew<EnrichmentStrategy>(UnityBuildStage.PostInitialization);
        }
    }
}
