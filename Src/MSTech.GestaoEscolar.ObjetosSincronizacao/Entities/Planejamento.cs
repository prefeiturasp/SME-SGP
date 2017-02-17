using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class Planejamento
    {
        public Int64 tud_id { get; set; }
        public byte tdt_posicao { get; set; }
        public List<TurmaDisciplinaPlanejamento> TurmaDisciplinaPlanejamento { get; set; }
        
        // o get é sobreposto para quando o linq inclui um item nulo na lista.
        private List<PlanejamentoOrientacaoCurricular> planejamentoOrientacaoCurricular;
        public List<PlanejamentoOrientacaoCurricular> PlanejamentoOrientacaoCurricular
        {
            get {
                if (planejamentoOrientacaoCurricular != null)
                {
                    if (planejamentoOrientacaoCurricular.Count == 1)
                    {
                        if (planejamentoOrientacaoCurricular.First().poc == null)
                        {
                            return new List<PlanejamentoOrientacaoCurricular>();
                        }
                    }
                }

                return planejamentoOrientacaoCurricular;
            }
            set { planejamentoOrientacaoCurricular = value; }
        }

        // o get é sobreposto para quando o linq inclui um item nulo na lista.
        private List<PlanejamentoOrientacaoCurricularDiagnostico> planejamentoOrientacaoCurricularDiagnostico;
        public List<PlanejamentoOrientacaoCurricularDiagnostico> PlanejamentoOrientacaoCurricularDiagnostico {
            get
            {
                if (planejamentoOrientacaoCurricularDiagnostico != null)
                {
                    if (planejamentoOrientacaoCurricularDiagnostico.Count == 1)
                    {
                        if (planejamentoOrientacaoCurricularDiagnostico.First().ocr == null)
                        {
                            return new List<PlanejamentoOrientacaoCurricularDiagnostico>();
                        }
                    }
                }

                return planejamentoOrientacaoCurricularDiagnostico;
            }
            set
            {
                planejamentoOrientacaoCurricularDiagnostico = value;
            } 
        }

        public Planejamento()
        {
            this.TurmaDisciplinaPlanejamento = new List<TurmaDisciplinaPlanejamento>();
            this.PlanejamentoOrientacaoCurricular = new List<PlanejamentoOrientacaoCurricular>();
            this.PlanejamentoOrientacaoCurricularDiagnostico = new List<PlanejamentoOrientacaoCurricularDiagnostico>();
        }
    }
}
