namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using MSTech.GestaoEscolar.Entities;
    using System.Collections.Generic;

    public class ACA_DisciplinaDTO : ACA_Disciplina
    {
        public new bool? IsNew { get { return null; } }
        public new int? tds_id { get; set; }

        public ACA_TipoDisciplinaDTO.Referencia etapa { get; set; }
    }
}
