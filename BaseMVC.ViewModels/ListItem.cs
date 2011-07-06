using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseMVC.ViewModels
{
    public class ListItem
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }
    }
}
