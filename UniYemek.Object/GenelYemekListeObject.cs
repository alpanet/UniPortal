using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UniYemek.Object
{

    [DataContract]
    public class GenelYemekListeObject
    {
        [DataMember]
        public DateTime TarihDateTime { get; set; }

        [DataMember]
        public string TarihString { get; set; }

        [DataMember]
        public int YemekTipi { get; set; }

        [DataMember]
        public string YemekIcerik { get; set; }
    }
}
