namespace Fakultet.Servisi.Bazni
{
    public interface IServis<T> where T : class
    {
        List<T> GetAll();
        T? GetById(int id);
        void Add(T obj);
        void Update(T obj);
        void Remove(int id);
    }
}
