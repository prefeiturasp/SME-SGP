/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.ComponentModel;

using MSTech.Validation;
using MSTech.GestaoEscolar.Entities.Abstracts;

namespace MSTech.GestaoEscolar.Entities
{

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class MTR_ProcessoFechamentoInicio : Abstract_MTR_ProcessoFechamentoInicio
    {
        [DataObjectField(true, true, false)]
        public override int pfi_id { get; set; }

        [MSNotNullOrEmpty("Entidade é obrigatório.")]
        public override Guid ent_id { get; set; }

        [MSNotNullOrEmpty("Ano de fechamento é obrigatório.")]
        public override int pfi_anoFechamento { get; set; }

        [MSNotNullOrEmpty("Ano de início é obrigatório.")]
        public override int pfi_anoInicio { get; set; }

        //[MSNotNullOrEmpty("Data de início da previsão de séries é obrigatório.")]
        //public override DateTime pfi_dataInicioPrevisaoSeries { get; set; }

        //[MSNotNullOrEmpty("Data de fim da previsão de séries é obrigatório.")]
        //public override DateTime pfi_dataFimPrevisaoSeries { get; set; }

        //[MSNotNullOrEmpty("Data de início da definição dos parâmetros de remanejamento é obrigatório.")]
        //public override DateTime pfi_dataInicioParametroRemanejamento { get; set; }

        //[MSNotNullOrEmpty("Data de fim da definição dos parâmetros de remanejamento é obrigatório.")]
        //public override DateTime pfi_dataFimParametroRemanejamento { get; set; }

        //[MSNotNullOrEmpty("Data de início da enturmação é obrigatório.")]
        //public override DateTime pfi_dataInicioEnturmacao { get; set; }

        //[MSNotNullOrEmpty("Data de fim da enturmação é obrigatório.")]
        //public override DateTime pfi_dataFimEnturmacao { get; set; }

        //[MSNotNullOrEmpty("Data de início do remanejamento é obrigatório.")]
        //public override DateTime pfi_dataInicioRemanejamento { get; set; }

        //[MSNotNullOrEmpty("Data de fim do remanejamento é obrigatório.")]
        //public override DateTime pfi_dataFimRemanejamento { get; set; }

        //[MSNotNullOrEmpty("Data de início da renovação é obrigatório.")]
        //public override DateTime pfi_dataInicioRenovacao { get; set; }

        //[MSNotNullOrEmpty("Data de fim da renovação é obrigatório.")]
        //public override DateTime pfi_dataFimRenovacao { get; set; }

        [MSDefaultValue(1)]
        public override byte pfi_situacao { get; set; }
        public override DateTime pfi_dataCriacao { get; set; }
        public override DateTime pfi_dataAlteracao { get; set; }
    }
}