namespace EMSWebApi.GenericRepo
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(object id);
        Task Insert(T obj);
        Task Update(T obj);
        Task<T> Delete(Object id);
        Task Save();
    }
}
