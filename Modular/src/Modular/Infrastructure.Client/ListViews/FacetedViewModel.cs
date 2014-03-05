namespace Infrastructure.ListViews
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using Infrastructure.Reflection;
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
        public long Id { get; set; }

        [PropertyMeta(IsListViewRelevant = false)]
        public IEnumerable<Facet> Facets
        {
            get { return this.facets; }
        }

        [PropertyMeta(IsListViewRelevant = false)]
        public object this[string propertyName]
        {
            get
            {
                Contract.Requires(!string.IsNullOrEmpty(propertyName));

                if (!this.values.ContainsKey(propertyName))
                {
                    return null;
                }

                return this.values[propertyName];
            }

            set
            {
                Contract.Requires(!string.IsNullOrEmpty(propertyName));

                this.values[propertyName] = value;
                this.OnPropertyChanged(propertyName);
            }
        }

        public Type GetCustomType()
        {
            return new FacetedObjectType<FacetedViewModel>(this.facets);
        }

        public void AddFacet(Facet facet)
        {
            Contract.Requires(facet != null);

            this.facets.Add(facet);
        }
    }
}