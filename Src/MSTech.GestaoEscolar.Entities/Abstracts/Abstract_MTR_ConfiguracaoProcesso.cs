/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MSTech.Data.Common.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities.Abstracts
{	
	/// <summary>
	/// 
	/// </summary>
	[Serializable()]
    public abstract class Abstract_MTR_ConfiguracaoProcesso : Abstract_Entity
    {
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int cfg_id { get; set; }
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int cpr_id { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int cpr_tipoProcesso { get; set; }
		[MSValidRange(200)]
		[MSNotNullOrEmpty()]
		public virtual string cpr_nome { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int cpr_ordem { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool cpr_preMatricula { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool cpr_confirmacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool cpr_confirmacaoPresencial { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool cpr_entregaDoc { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool cpr_internet { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool cpr_permiteAlteracao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte cpr_listaEspera { get; set; }
		[MSNotNullOrEmpty()]
		public virtual int cpr_qtdeOpcoes { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool cpr_agendamento { get; set; }
		public virtual byte cpr_localAtendimento { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool cpr_alocacaoAutomatica { get; set; }
		public virtual bool cpr_porSorteio { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool cpr_moveAluno { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool cpr_alterarTurma { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool cpr_manterTurma { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool cpr_porIdade { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool cpr_porSexo { get; set; }
		[MSNotNullOrEmpty()]
		public virtual bool cpr_manual { get; set; }
		[MSNotNullOrEmpty()]
		public virtual byte cpr_situacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime cpr_dataCriacao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual DateTime cpr_dataAlteracao { get; set; }
		[MSNotNullOrEmpty()]
		public virtual Int64 evt_id { get; set; }

    }
}