using System;
using System.Collections.Generic;

using TecX.Common;

namespace TecX.Agile.ViewModel.ChangeTracking
{
    public class PropertyChangeHandlerChain
    {
        private readonly List<Action<PlanningArtefact, string, object, object>> _handlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangeHandlerChain"/> class
        /// </summary>
        public PropertyChangeHandlerChain()
        {
            _handlers = new List<Action<PlanningArtefact, string, object, object>>();
        }

        public void Add(Action<PlanningArtefact, string, object, object> handler)
        {
            Guard.AssertNotNull(handler, "handler");
            
            _handlers.Add(handler);
        }

        public void Handle(PlanningArtefact parentObject, string propertyName, object oldValue, object newValue)
        {
            Guard.AssertNotNull(parentObject, "parentObject");
            Guard.AssertNotEmpty(propertyName, "propertyName");

            foreach(var handler in _handlers)
            {
                handler(parentObject, propertyName, oldValue, newValue);
            }
        }
    }
}