using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class TipoDocente
    {
        public int Tdc_id { get; set; }
        public int Tdc_posicao { get; set; }
        public string Tdc_nome { get; set; }
        public List<PermissaoDocente> PermissaoDocente { get; set; }

        public TipoDocente()
        {
            PermissaoDocente = new List<PermissaoDocente>();
        }
    }
}
