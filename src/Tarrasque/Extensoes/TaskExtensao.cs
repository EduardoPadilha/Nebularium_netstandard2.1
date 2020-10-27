using System.Threading.Tasks;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class TaskExtensao
    {
        public static Task<TResult> ComoAsync<T, TResult>(this Task<T> obj)
        {
            return obj.ContinueWith(r => r.Result.Como<TResult>());
        }
    }
}
