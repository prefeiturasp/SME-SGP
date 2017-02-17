/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class MTR_ConfiguracaoProcesso : Abstract_MTR_ConfiguracaoProcesso
	{
        [MSNotNullOrEmpty("Etapa do processo de matrícula é obrigatório.")]
        public override int cpr_tipoProcesso { get; set; }
        [MSValidRange(200)]
        [MSNotNullOrEmpty("Nome é obrigatório.")]
        public override string cpr_nome { get; set; }
        [MSNotNullOrEmpty("Oderm do processo de matrícula é obrigatório.")]
        public override int cpr_ordem { get; set; }        
        public override byte cpr_listaEspera { get; set; }        
        public override int cpr_qtdeOpcoes { get; set; }        
        [MSNotNullOrEmpty("Situação é obrigatório.")]
        public override byte cpr_situacao { get; set; }
        [MSNotNullOrEmpty("Data de criação é obrigatório.")]
        public override DateTime cpr_dataCriacao { get; set; }
        [MSNotNullOrEmpty("Data de alteração é obrigatório.")]
        public override DateTime cpr_dataAlteracao { get; set; }          
        [MSNotNullOrEmpty("Tipo de evento é obrigatório.")]
        public override Int64 evt_id { get; set; }
	}
}