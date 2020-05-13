using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.GUI.Data
{
    /// <summary>
    /// "Technical support information" group data.
    /// </summary>
    public class SupportInfo
    {
        /// <summary>
        /// General international support mailing address
        /// </summary>
        public string InternationalAddress { get; set; }

        /// <summary>
        /// Regional support contact address
        /// </summary>
        public string RegionalAddress { get; set; }

        /// <summary>
        /// Technical support website URL
        /// </summary>
        public string SupportUrl { get; set; }

        /// <summary>
        /// Technical support email
        /// </summary>
        public string SupportEmail { get; set; }

        /// <summary>
        /// Technical support phone
        /// </summary>
        public string SupportPhone { get; set; }
    }
}
