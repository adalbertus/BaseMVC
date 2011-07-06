using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace BaseMVC.Infrastructure
{
    public class NLogFactory : ILoggerFactory
    {
        private static readonly IInternalLogger internalLogger = new NLogLogger();
        #region ILoggerFactory Members

        public IInternalLogger LoggerFor(System.Type type)
        {
            return internalLogger;
        }

        public IInternalLogger LoggerFor(string keyName)
        {
            return internalLogger;
        }

        #endregion
    }
}
