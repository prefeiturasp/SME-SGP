namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class ACA_TipoDisciplinaDTO : ACA_TipoDisciplina
    {
        public bool? IsNew { get { return null; } }
        public int? tne_id { get; set; }
        public int? mds_id { get; set; }
        public int? aco_id { get; set; }
        public ACA_TipoNivelEnsinoDTO.Referencia tipoNivelEnsino { get; set; }
        public ACA_AreaConhecimentoDTO.Referencia areaConhecimento { get; set; }
        public List<ACA_TipoDisciplinaDeficienciaDTO> listaTiposDisciplinaDeficiencia { get; set; }

        public class Referencia
        {
            public int? tds_id { get; set; }
        }

        public ACA_TipoDisciplinaDTO()
        {
            this.tipoNivelEnsino = new ACA_TipoNivelEnsinoDTO.Referencia();
            this.areaConhecimento = new ACA_AreaConhecimentoDTO.Referencia();
            this.listaTiposDisciplinaDeficiencia = new List<ACA_TipoDisciplinaDeficienciaDTO>();
        }

        // Delegate utilizado no POST 
        public delegate TResult FuncTipoDisciplina<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    }
}
