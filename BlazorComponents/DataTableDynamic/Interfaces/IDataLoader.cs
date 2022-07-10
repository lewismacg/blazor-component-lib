using System.Threading.Tasks;
using BlazorComponents.Models;

namespace BlazorComponents
{
    public interface IDataLoader<T>
    {
        public Task<PaginationResult<T>> LoadDataAsync(FilterData parameters);
    }
}
