using Business.Interfaces;

namespace Business.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        readonly IFileContext<User> _fileContext;
        public CustomerRepository()
        {
            _fileContext = new FileContext<User>();
        }
        public static List<User> customersList = new List<User>();
        /// <summary>
        /// Write the Information Of Customer Which is Stored in CustomerList in JSON File
        /// </summary>
        public void WriteJson(User user)
        {
            _fileContext.Create(user);
        }
        /// <summary>
        /// Read the Information Of Customer Which is Stored in JSON File and Fill List Of Customer With it
        /// </summary>
        public ICollection<User> ReadJson(User user)
        {
            return _fileContext.Read(user);
        }

        public void Update(User user)
        {
            _fileContext.Update(user);
        }

        public void Delete(User user)
        {
            _fileContext.Delete(user);
        }
    }
}
