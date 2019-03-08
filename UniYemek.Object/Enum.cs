using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UniYemek.Object
{
    [DataContract]
    public enum YemekTipleri
    {
            Öğle,
            Akşam
    }

    [DataContract]
    public enum Okul
    {
        DokuzEylul,
        SuleymanDemirel
    }

}
