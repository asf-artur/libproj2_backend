using System.Collections.Generic;

namespace WebApplication2.Repositories
{
    public interface IBaseRepository<TElement>
    {
        List<TElement> GetAll();
    }
}