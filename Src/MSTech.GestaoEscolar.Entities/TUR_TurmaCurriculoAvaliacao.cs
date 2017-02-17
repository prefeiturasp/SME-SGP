/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.ComponentModel;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class TUR_TurmaCurriculoAvaliacao : Abstract_TUR_TurmaCurriculoAvaliacao
	{        
        [DataObjectField(true, false, false)]
        public override int tca_id { get; set; }
        [MSDefaultValue(1)]
        public override byte tca_situacao { get; set; }        
        public override DateTime tca_dataCriacao { get; set; }        
        public override DateTime tca_dataAlteracao { get; set; }

        // Variáveis utilizadas no cadastro de turma
        public virtual string crp_nomeAvaliacao { get; set; }
        public virtual string tca_id_numeroAvaliacao { get; set; }

        // Variável utilizada na confirmação de solicitação de transferência
        public virtual int ala_id { get; set; }
	}
}