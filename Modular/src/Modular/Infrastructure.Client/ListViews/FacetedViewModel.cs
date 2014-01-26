namespace Infrastructure.ListViews
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Infrastructure.ViewModels;

    public class FacetedViewModel : ViewModel, ICustomTypeProvider
    {
        private readonly List<Facet> currentFacets;
        private readonly IDictionary<string, object> facetValues;

        public FacetedViewModel()
        {
            this.facetValues = new Dictionary<string, object>();
            this.currentFacets = new List<Facet>();
        }

        public object this[string key]
        {
            get
            {
                if (!this.facetValues.ContainsKey(key))
                {
                    return null;
                }

                return this.facetValues[key];
            }

            set
            {
                this.facetValues[key] = value;
                this.OnPropertyChanged(key);
            }
        }

        public Type GetCustomType()
        {
            return new FacetedObjectType<FacetedViewModel>(this.currentFacets);
        }

        public void AddFacet(Facet f)
        {
            this.currentFacets.Add(f);
        }
    }
}