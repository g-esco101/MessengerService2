using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerService2.Services
{
    interface IUsersServices
    {
        string[] stringCSVToArray(string csvString);
    }
}
