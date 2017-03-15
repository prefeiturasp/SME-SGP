using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GestaoEscolar.Api.Models
{
    public class Diretoria
    {
        public bool visible { get; set; }
        public List<jsonObject> diretorias { get; set; }
    }
}