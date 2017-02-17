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
	public class ACA_ParametroAcademico : Abstract_ACA_ParametroAcademico
	{
        [MSValidRange(100, "Chave pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Chave é obrigatório.")]
        public override string pac_chave { get; set; }
        [MSValidRange(1000, "Valor pode conter até 1000 caracteres.")]       
        public override string pac_valor { get; set; }
        [MSValidRange(200, "Descrição pode conter até 200 caracteres.")]
        public override string pac_descricao { get; set; }
        [MSNotNullOrEmpty("Vigência inicial é obrigatório.")]
        public override DateTime pac_vigenciaInicio { get; set; }
        [MSDefaultValue(1)]
        public override byte pac_situacao { get; set; }
        public override DateTime pac_dataCriacao { get; set; }
        public override DateTime pac_dataAlteracao { get; set; }
        public override Guid ent_id { get; set; }
	}
}