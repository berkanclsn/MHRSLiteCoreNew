using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntityLayer
{
    public class EmailMessage
    {
        public string[] Contacts { get; set; }
        public string[] CC { get; set; }
        public string[] BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }
}
