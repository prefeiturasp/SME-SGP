/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.ComponentModel;
	using MSTech.Data.Common.Abstracts;
	using MSTech.Validation;
	
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
    public abstract class Abstract_ACA_Curso : Abstract_Entity
    {
		
		/// <summary>
		/// Campo Id da tabela ACA_Curso.
		/// </summary>
		[MSNotNullOrEmpty("[cur_id] é obrigatório.")]
		[DataObjectField(true, true, false)]
		public virtual int cur_id { get; set; }

		/// <summary>
		/// Campo Id da tabela SYS_Entidde do CoreSSO.
		/// </summary>
		[MSNotNullOrEmpty("[ent_id] é obrigatório.")]
		public virtual Guid ent_id { get; set; }

		/// <summary>
		/// Curso padrão..
		/// </summary>
		[MSNotNullOrEmpty("[cur_padrao] é obrigatório.")]
		public virtual bool cur_padrao { get; set; }

		/// <summary>
		/// Campo Id da tabela ACA_TipoNivelEnsino.
		/// </summary>
		[MSNotNullOrEmpty("[tne_id] é obrigatório.")]
		public virtual int tne_id { get; set; }

		/// <summary>
		/// Campo Id da tabela ACA_TipoModalidadeEnsino.
		/// </summary>
		[MSNotNullOrEmpty("[tme_id] é obrigatório.")]
		public virtual int tme_id { get; set; }

		/// <summary>
		/// Codigo do curso..
		/// </summary>
		[MSValidRange(10)]
		public virtual string cur_codigo { get; set; }

		/// <summary>
		/// Nome do curso.
		/// </summary>
		[MSValidRange(200)]
		[MSNotNullOrEmpty("[cur_nome] é obrigatório.")]
		public virtual string cur_nome { get; set; }

		/// <summary>
		/// Nome abreviado do curso..
		/// </summary>
		[MSValidRange(20)]
		public virtual string cur_nome_abreviado { get; set; }

		/// <summary>
		/// Curso conclui o nível de ensino..
		/// </summary>
		public virtual bool cur_concluiNivelEnsino { get; set; }

		/// <summary>
		/// Identifica se o curso foi criado para alunos deficientes..
		/// </summary>
		[MSNotNullOrEmpty("[cur_exclusivoDeficiente] é obrigatório.")]
		public virtual bool cur_exclusivoDeficiente { get; set; }

		/// <summary>
		/// Proximo nivel de ensino.
		/// </summary>
		public virtual int tne_idProximo { get; set; }

		/// <summary>
		/// Data inicial da vigência..
		/// </summary>
		[MSNotNullOrEmpty("[cur_vigenciaInicio] é obrigatório.")]
		public virtual DateTime cur_vigenciaInicio { get; set; }

		/// <summary>
		/// Data final da vigência..
		/// </summary>
		public virtual DateTime cur_vigenciaFim { get; set; }

		/// <summary>
		/// Permitir fechamento/efetivação semestral.
		/// </summary>
		[MSNotNullOrEmpty("[cur_efetivacaoSemestral] é obrigatório.")]
		public virtual bool cur_efetivacaoSemestral { get; set; }

		/// <summary>
		/// Não causa conflito de horário do turno com matrícula em sala de recurso. (True - não causa conflito, False - causa conflito).
		/// </summary>
		public virtual bool cur_conflitoTurnoSR { get; set; }

		/// <summary>
		/// Situação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[cur_situacao] é obrigatório.")]
		public virtual byte cur_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty("[cur_dataCriacao] é obrigatório.")]
		public virtual DateTime cur_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty("[cur_dataAlteracao] é obrigatório.")]
		public virtual DateTime cur_dataAlteracao { get; set; }

		/// <summary>
		/// Carga horária do curso..
		/// </summary>
		public virtual decimal cur_cargaHoraria { get; set; }

    }
}