using Business.Interfaces;
using Business.Repositories;

namespace Business
{
    public static class ReadJSON
    {
        public static void User()
        {
            ICustomerRepository customerrepository = new CustomerRepository();
            User temp = new User();
            CustomerRepository.customersList = (List<User>)customerrepository.ReadJson(temp);
        }
    }
}
