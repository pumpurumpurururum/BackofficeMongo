using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BackofficeDemo.Common.HttpErrors
{
    public static class NotFoundConfig
    {
        private static readonly Action<HttpRequestBase, Uri> NullOnNotFound = (req, uri) => { /*noop*/ };

        private static Action<HttpRequestBase, Uri> _onNotFound;

        /// <summary>
        /// Gets or sets the action to execute when a 404 has occurred.
        /// Here you can pass the 404 on to your own logging (NLog, log4net) or error handling (ELMAH).
        /// </summary>
        /// <value>
        /// The action to execute when a 404 has occurred.
        /// </value>
        public static Action<HttpRequestBase, Uri> OnNotFound
        {
            get
            {
                return _onNotFound ?? NullOnNotFound;
            }

            set
            {
                _onNotFound = value;
            }
        }
    }
}
