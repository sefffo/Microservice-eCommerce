using Ecommerce.SherdLibrary.Responses;
using System.Linq.Expressions;
namespace Ecommerce.SherdLibrary.Interface
{

    //3shan el Services el 3ndna hya product w order fa kolhom byst5dmo crud 
    public interface IGenericInterface<T> where T : class
    {
        Task<Ecommerce.SherdLibrary.Responses.Response> CreateAsync(T Entity);
        Task<Ecommerce.SherdLibrary.Responses.Response> UpdateAsync(T Entity);
        Task<Ecommerce.SherdLibrary.Responses.Response> DeleteAsync(T Entity);
        Task<IEnumerable<T>> GetAllAsync(T Entity);
        Task<T> GetByIdAsync (int id);
        Task<T> FindByIdAsync(int id);
        //3shan lw 3ayz a3ml filter zy el specification pattern
        Task<T> GetByAsync(Expression<Func<T, bool>> Predicate);

    }
}
