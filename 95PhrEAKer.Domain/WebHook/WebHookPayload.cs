using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _95PhrEAKer.Domain.WebHook
{
    public class WebHookPayload
    {
        public string EventType { get; set; }
        public string Data { get; set; }
    }
}
