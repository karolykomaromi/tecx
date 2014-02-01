namespace Infrastructure.Dynamic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Windows;

    public class ViewRegistry : IViewRegistry 
    {
        private static readonly Lazy<IViewRegistry> Lazy = new Lazy<IViewRegistry>(() => new ViewRegistry(new ControlAdapterFactory()));

        private readonly IDictionary<ControlId, IControl> controls;
        private readonly IControlAdapterFactory controlAdapterFactory;

        public ViewRegistry(IControlAdapterFactory controlAdapterFactory)
        {
            Contract.Requires(controlAdapterFactory != null);

            this.controlAdapterFactory = controlAdapterFactory;
            this.controls = new Dictionary<ControlId, IControl>();
        }

        public static IViewRegistry Current
        {
            get { return Lazy.Value; }
        }

        public void Register(FrameworkElement element)
        {
            Contract.Requires(element != null);

            IControl control = this.controlAdapterFactory.CreateAdapter(element);

            // TODO weberse 2014-02-01 need to check why searchresultview is added twice. eventhandler not removed in time?
            this.controls[control.Id] = control;
        }

        public bool TryFindById(ControlId id, out IControl control)
        {
            if (this.controls.TryGetValue(id, out control))
            {
                return true;
            }

            var path = id.Path;
            if (this.controls.TryGetValue(path[0], out control))
            {
                for (int i = 1; i < path.Count; i++)
                {
                    if (!control.TryFindById(path[i], out control))
                    {
                        control = null;
                        return false;
                    }
                }

                return true;
            }

            control = null;
            return false;
        }
    }
}
