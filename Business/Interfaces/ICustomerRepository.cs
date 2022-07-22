namespace Business.Interfaces
{
    /// <summary>
    /// This Interface Is For The Repository Customer ,
    /// Which Calls The Function Of Writing To The Json File
    /// And Reading The ICollection From The Json File.
    /// </summary>
    public interface ICustomerRepository
    {
        void WriteJson(User user);
        ICollection<User> ReadJson(User user);
        void Update(User user);
        void Delete(User user);
    }
}
