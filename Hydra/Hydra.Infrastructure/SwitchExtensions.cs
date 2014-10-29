namespace Hydra.Infrastructure
{
    using System;

    public static class SwitchExtensions
    {
        public static Switch Case<T>(this Switch s, Action<T> a) where T : class
        {
            return Case<T>(s, o => true, a, false);
        }

        public static Switch Case<T>(this Switch s, Action<T> a, bool fallThrough) where T : class
        {
            return Case<T>(s, o => true, a, fallThrough);
        }

        public static Switch Case<T>(this Switch s, Func<T, bool> c, Action<T> a) where T : class
        {
            return Case<T>(s, c, a, false);
        }

        public static Switch Case<T>(this Switch s, Func<T, bool> c, Action<T> a, bool fallThrough) where T : class
        {
            if (s == null)
            {
                return null;
            }
            else
            {
                T t = s.Object as T;
                if (t != null)
                {
                    if (c(t))
                    {
                        a(t);
                        return fallThrough ? s : null;
                    }
                }
            }

            return s;
        }

        public static void Default(this Switch s, Action<object> a)
        {
            if (s != null)
            {
                a(s.Object);
            }
        }
    }
}