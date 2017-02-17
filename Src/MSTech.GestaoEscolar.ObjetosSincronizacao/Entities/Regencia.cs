using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class Regencia
    {
        public long Tud_idFilho { get; set; }
        public string Aul_ata { get; set; }
        public string Aul_plano { get; set; }
        public string Aul_atividadeCasa { get; set; }
        public string Aul_sintese { get; set; }

        public string DataAlteracao
        {
            get
            {
                return dataAlteracao_bd.ToString("dd/MM/yyyy HH:mm:ss.fff");
            }
        }
        public DateTime dataAlteracao_bd { private get; set; }

        public List<TurmaAulaRecurso> Recursos { get; set; }
    }
}
