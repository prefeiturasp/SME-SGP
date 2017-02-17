/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
    public class ACA_CalendarioAnual : Abstract_ACA_CalendarioAnual
	{
        [MSNotNullOrEmpty("Entidade é obrigatório.")]
        public override Guid ent_id { get; set; }
        [MSNotNullOrEmpty("Ano letivo é obrigatório e deve ser um número inteiro maior que 0 (zero).")]
        public override int cal_ano { get; set; }
        [MSValidRange(200, "Descrição do calendário escolar pode conter até 200 caracteres.")]
        [MSNotNullOrEmpty("Descrição do calendário escolar é obrigatório.")]
        public override string cal_descricao { get; set; }
        [MSNotNullOrEmpty("Data de início é obrigatório.")]
        public override DateTime cal_dataInicio { get; set; }
        [MSNotNullOrEmpty("Data de fim é obrigatório.")]
        public override DateTime cal_dataFim { get; set; }
        [MSDefaultValue(1)]
        public override byte cal_situacao { get; set; }
        public override DateTime cal_dataCriacao { get; set; }
        public override DateTime cal_dataAlteracao { get; set; }
	}
}