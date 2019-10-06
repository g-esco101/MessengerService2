using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessengerService2.Services
{
    public class UsersServices : IUsersServices
    {
        public string[] stringCSVToArray(string csvString)
        {
            try
            {
                string[] split = csvString.Split(',');
                string[] arrString = new string[split.Length];
                for (int i = 0; i < split.Length; i++)
                {
                    arrString[i] = split[i].Trim();
                }
                return arrString;
            }
            catch { return null; }
        }
    }
}