using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRest
{
    public interface IDatabaseCommunication
    {
        void Write2Database(User user);
        User FindUser(int ID);
        Users FindUsers();
    }
}
