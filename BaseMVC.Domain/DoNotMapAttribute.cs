using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseMVC.Domain
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DoNotMapAttribute : Attribute
    {
    }
}
