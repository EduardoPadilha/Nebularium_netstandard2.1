using System;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class FuncExtensao
    {
        public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T2, T3> f, Func<T1, T2> g)
        {
            return x => f(g(x));
        }
    }
}
