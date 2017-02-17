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
    public abstract class Abstract_MTR_ParametroFormacaoTurma : Abstract_Entity
    {

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int pfi_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		[DataObjectField(true, false, false)]
		public virtual int pft_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int cur_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int crr_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int crp_id { get; set; }

		/// <summary>
		/// Tipo de turma: 1-Normal, 2-Eletiva
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte pft_tipo { get; set; }

        /// <summary>
        /// 1 - Sem controle, 2 - Capacidade normal e incluídos, 3 - Capacidade normal e individual por incluídos.
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual byte pft_tipoControleCapacidade { get; set; }

		/// <summary>
		/// Limite de alunos por turma
		/// </summary>
		public virtual int pft_capacidade { get; set; }

        /// <summary>
        /// 1 - Sem alunos incluídos, 2 - Todos os tipos, 3 - Escolher o tipo.
        /// </summary>
        [MSNotNullOrEmpty()]
        public virtual byte pft_tipoControleDeficiente { get; set; }

		/// <summary>
		/// Quantitativo de alunos com deficiência por turma
		/// </summary>
		public virtual int pft_qtdDeficiente { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int pft_capacidadeComDeficiente { get; set; }

		/// <summary>
		/// Prefixo do código da turma
		/// </summary>
		[MSValidRange(10)]
		public virtual string pft_prefixoCodigoTurma { get; set; }

		/// <summary>
		/// Quantidade de dígitos da parte dinâmica do códigodo da turma
		/// </summary>
		public virtual int pft_qtdDigitoCodigoTurma { get; set; }

		/// <summary>
		/// 1 - Numérico, 2 - Alfabético
		/// </summary>
		public virtual byte pft_tipoDigitoCodigoTurma { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual int pft_cargaHorariaSemanal { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int cal_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual int fav_id { get; set; }

		/// <summary>
		/// Indica se as turmas serão de docente especialista
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual bool pft_docenteEspecialista { get; set; }

		/// <summary>
		/// 1 – Ativo, 3 – Excluído
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual byte pft_situacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime pft_dataCriacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MSNotNullOrEmpty()]
		public virtual DateTime pft_dataAlteracao { get; set; }

    }
}