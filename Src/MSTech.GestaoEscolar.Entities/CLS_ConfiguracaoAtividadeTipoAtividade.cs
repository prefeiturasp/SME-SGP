/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
	using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class CLS_ConfiguracaoAtividadeTipoAtividade : Abstract_CLS_ConfiguracaoAtividadeTipoAtividade
	{
        public virtual int crp_id { get; set; }
        public virtual int dis_id { get; set; }
        public virtual string tav_nome { get; set; }
        public virtual bool Selecionado { get; set; }
	}
}