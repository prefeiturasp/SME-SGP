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
	public class MTR_ParametroFormacaoTurma : Abstract_MTR_ParametroFormacaoTurma
	{
        [MSNotNullOrEmpty("Processo de fechamento/início ano letivo é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int pfi_id { get; set; }

        public override int pft_id { get; set; }

        [MSNotNullOrEmpty("Curso é obrigatório.")]
        public override int cur_id { get; set; }

        [MSNotNullOrEmpty("Currículo é obrigatório.")]
        public override int crr_id { get; set; }

        public override int crp_id { get; set; }

        [MSNotNullOrEmpty("Calendário é obrigatório.")]
        public override int cal_id { get; set; }

        [MSNotNullOrEmpty("Formato de avaliação é obrigatório.")]
        public override int fav_id { get; set; }

        [MSDefaultValue(1)]
        public override byte pft_tipo { get; set; }

        [MSDefaultValue(1)]
        public override byte pft_tipoControleCapacidade { get; set; }

        [MSDefaultValue(0)]
        public override int pft_capacidade { get; set; }

        [MSDefaultValue(1)]
        public override byte pft_tipoControleDeficiente { get; set; }

        [MSDefaultValue(0)]
        public override int pft_qtdDeficiente { get; set; }

        [MSDefaultValue(0)]
        public override int pft_capacidadeComDeficiente { get; set; }

        [MSValidRange(10)]
        public override string pft_prefixoCodigoTurma { get; set; }

        public override int pft_qtdDigitoCodigoTurma { get; set; }

        public override byte pft_tipoDigitoCodigoTurma { get; set; }

        public override bool pft_docenteEspecialista { get; set; }

        [MSDefaultValue(1)]
        public override byte pft_situacao { get; set; }

        public override DateTime pft_dataCriacao { get; set; }

        public override DateTime pft_dataAlteracao { get; set; }
	}
}