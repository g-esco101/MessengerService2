using System.Collections.Generic;

namespace MessengerService2.Helpers
{
    public class StringChecker
    {
        public static bool IsNullEmptyWhiteSpace(string a, string b = "second", string c = "third")
        {
            if (string.IsNullOrEmpty(a) || string.IsNullOrWhiteSpace(a) || string.IsNullOrEmpty(b) || string.IsNullOrWhiteSpace(b) || string.IsNullOrEmpty(c) || string.IsNullOrWhiteSpace(c))
            {
                return true;
            }
            return false;
        }

        public static bool IsRoleNullEmptyWhiteSpace(List<string> roles)
        {
            bool isInvalid = false;
            int len = roles.Count;
            for (int i = 0; i < len; i++)
            {
                isInvalid = IsNullEmptyWhiteSpace(roles[i]);
                if (isInvalid) return true;
            }
            return false;
        }
    }
}