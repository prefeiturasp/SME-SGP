/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
    using Validation;

    /// <summary>
    /// Description: .
    /// </summary>
    [Serializable()]
    public class ESC_TipoClassificacaoEscolaCargos : Abstract_ESC_TipoClassificacaoEscolaCargos
	{
        public override DateTime tcc_dataCriacao { get; set; }

        public override DateTime tcc_dataAlteracao { get; set; }
        [MSDefaultValue(1)]
        public override short tcc_situacao { get; set; }

        public string crg_nome { get; set; }
        public string tvi_nome { get; set; }
        public int tvi_id { get; set; }
    }
}