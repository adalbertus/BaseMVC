using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Diagnostics;
using System.Text;
using Castle.Core.Logging;

namespace BaseMVC
{
    public class CustomDebugWriter : TextWriter
    {
        public override void WriteLine(string value)
        {
            Debug.WriteLine(value);
        }

        public override void Write(string value)
        {
            Debug.Write(value); 
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}