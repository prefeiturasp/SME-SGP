/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.Validation;
using MSTech.GestaoEscolar.Entities.Abstracts;

namespace MSTech.GestaoEscolar.Entities
{	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class RHU_CargaHoraria : Abstract_RHU_CargaHoraria
	{   
        [MSNotNullOrEmpty("Entidade é obrigatório.")]
        public override Guid ent_id { get; set; }
        [MSValidRange(200,"Descrição da carga horária pode conter até 200 caracteres.")]
        public override string chr_descricao { get; set; }
        [MSNotNullOrEmpty("Padrão é obrigatório.")]
        public override bool chr_padrao { get; set; }
        [MSNotNullOrEmpty("Horas semanais é obrigatório e deve ser um número inteiro maior que 0 (zero).")]
        public override int chr_cargaHorariaSemanal { get; set; }
        [MSDefaultValue(1)]
        public override byte chr_situacao { get; set; }
        public override DateTime chr_dataCriacao { get; set; }
        public override DateTime chr_dataAlteracao { get; set; }
	}
}