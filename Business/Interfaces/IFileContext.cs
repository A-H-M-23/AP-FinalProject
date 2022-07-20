namespace Business.Interfaces
{
    public interface IFileContext<T> where T : class
    {
        void Create(T entity);
        ICollection<T> Read(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
