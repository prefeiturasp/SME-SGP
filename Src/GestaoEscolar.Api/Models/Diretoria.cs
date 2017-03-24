using System.Collections.Generic;

namespace GestaoEscolar.Api.Models
{
    public class Diretoria
    {
        public bool visible { get; set; }
        public List<jsonObject> diretorias { get; set; }
    }
}