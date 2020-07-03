using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ZJ.App.Common
{
    [DataContract]
    public class ServiceFault
    {
        [DataMember]
        public string Message { get; set; }
    }
}
