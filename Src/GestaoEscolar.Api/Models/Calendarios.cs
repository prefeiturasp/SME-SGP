using System.Collections.Generic;

namespace GestaoEscolar.Api.Models
{
    public class Calendarios
    {
        public string idSelecionado { get; set; }
        public IEnumerable<jsonObject> lista { get; set; }
    }
}