using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
