using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaseMVC.ViewModels
{
    public class UserItem
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName))
                {
                    return string.Empty;
                }
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
    }
}