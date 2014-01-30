namespace Infrastructure.ListViews
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Infrastructure.Meta;
    using Infrastructure.ViewModels;

    public class FacetedViewModel : ViewModel, ICustomTypeProvider
    {
        private readonly List<Facet> facets;
        private readonly IDictionary<string, object> values;

        public FacetedViewModel()
        {
            this.values = new Dictionary<string, object>();
            this.facets = new List<Facet>();
        }

        [PropertyMeta(IsListViewRelevant = false)]
        public IEnumerable<Facet> Facets
        {
            get { return this.facets; }
        }

        [PropertyMeta(IsListViewRelevant = false)]
        public object this[string key]
        {
            get
            {
                if (!this.values.ContainsKey(key))
                {
                    return null;
                }

                return this.values[key];
            }

            set
            {
                this.values[key] = value;
                this.OnPropertyChanged(key);
            }
        }

        public Type GetCustomType()
        {
            return new FacetedObjectType<FacetedViewModel>(this.facets);
        }

        public void AddFacet(Facet facet)
        {
            this.facets.Add(facet);
        }
    }
}