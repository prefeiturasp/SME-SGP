using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class PermissaoDocente
    {
        public int Pdc_id { get; set; }
        public int Tdc_idPermissao { get; set; }
        public int Pdc_modulo { get; set; }
        public int Pdc_permissaoConsulta { get; set; }
        public int Pdc_permissaoEdicao { get; set; }
    }
}
