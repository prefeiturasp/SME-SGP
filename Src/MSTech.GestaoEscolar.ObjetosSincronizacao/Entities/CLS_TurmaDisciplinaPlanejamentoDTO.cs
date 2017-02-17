using MSTech.GestaoEscolar.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class CLS_TurmaDisciplinaPlanejamentoDTO : CLS_TurmaDisciplinaPlanejamento
    {
        
        public DateTime tdp_dataAlteracao_bd { private get; set; }
        public DateTime tdp_dataCriacao_bd { private get; set; }

        public string tdp_dataAlteracao
        {
            get
            {
                return tdp_dataAlteracao_bd.ToString("dd/MM/yyyy HH:mm:ss.fff");
            }
        }

        public string tdp_dataCriacao
        {
            get
            {
                return tdp_dataCriacao_bd.ToString("dd/MM/yyyy HH:mm:ss.fff");
            }
        }
    }
}
