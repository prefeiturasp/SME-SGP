namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;
    using System.Collections.Generic;

    public class ACA_CursoDTO : ACA_Curso
    {
        public List<ACA_CursoRelacionadoDTO> listaCursoRelacionado { get; set; }
        public ACA_CurriculoDTO curriculo { get; set; }
        public List<ACA_CurriculoPeriodoDTO> listaCurriculoPeriodo { get; set; }

        public new bool? IsNew { get { return null; } }
        public new int? tne_id { get; set; }
        public new int? tne_idProximo { get; set; }
        public new int? tme_id { get; set; }

        public ACA_TipoNivelEnsinoDTO.Referencia tipoNivelEnsino { get; set; }
        public ACA_TipoModalidadeEnsinoDTO.Referencia tipoModalidadeEnsino { get; set; }

    }
}
