/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using System;
    using System.ComponentModel;
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
		
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
	public class CLS_PlanejamentoCiclo : Abstract_CLS_PlanejamentoCiclo
	{
        /// <summary>
        /// ID da esola.
        /// </summary>
        [MSNotNullOrEmpty("Escola é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int esc_id { get; set; }

        /// <summary>
        /// Id da unidade escolar.
        /// </summary>
        [MSNotNullOrEmpty("Unidade escolar é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int uni_id { get; set; }

        /// <summary>
        /// Id do tipo de ciclo.
        /// </summary>
        [MSNotNullOrEmpty("Tipo de ciclo é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int tci_id { get; set; }

        /// <summary>
        /// Ano letivo do plano de ciclo.
        /// </summary>
        [MSNotNullOrEmpty("Ano letivo é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override int plc_anoLetivo { get; set; }

        /// <summary>
        /// Id do plano de ciclo.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override int plc_id { get; set; }

        /// <summary>
        /// Plano do ciclo.
        /// </summary>
        [MSNotNullOrEmpty("Plano do ciclo é obrigatório.")]
        public override string plc_planoCiclo { get; set; }

        /// <summary>
        /// Id do usuario que criou o plano.
        /// </summary>
        [MSNotNullOrEmpty("Usuário é obrigatório.")]
        public override Guid usu_id { get; set; }

        /// <summary>
        /// Situacao do registro: 1- Ativo; 2- Bloqueado; 3- Excluído; 4- Inativo.
        /// </summary>
        [MSDefaultValue(1)]
        public override byte plc_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime plc_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime plc_dataAlteracao { get; set; }

        /// <summary>
        /// Variável auxiliar com o nome do usuário que realizou o planejamento.
        /// </summary>
        public string nomeUsuario { get; set; }
	}
}