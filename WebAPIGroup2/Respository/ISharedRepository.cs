namespace WebAPIGroup2.Respository
{
    public interface ISharedRepository<T, U> where T : class
    {
        public Task<T?> GetByIDAsync(U id);
        public Task<IEnumerable<T>?> GetAllAsync();
        public Task<bool> InsertAsync(T entity);
        public Task<bool> UpdateAsync(T entity);
        public Task<bool> DeleteAsync(T entity);

        public Task<bool> DeleteAllAsync(List<T> list);

        public Task<bool> UpdateAllAsync(List<T> list);

        public Task<bool> InsertAllAsync(List<T> list);
    }
}
