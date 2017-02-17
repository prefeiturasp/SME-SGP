/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using MSTech.Validation;
    using System;
    using System.ComponentModel;
		
	/// <summary>
	/// Description: .
	/// </summary>
	public class CFG_ModuloClasse : Abstract_CFG_ModuloClasse
	{

        /// <summary>
        /// Campo ID da tabela SYS_Modulo do Core..
        /// </summary>
        [MSNotNullOrEmpty("ID do módulo é obrigatório.")]
        public override int mod_id { get; set; }

        /// <summary>
        /// Campo ID da tabela CFG_ModuloClasse..
        /// </summary>
        public override int mdc_id { get; set; }

        /// <summary>
        /// Classe css que irá informar a imagem do ícone..
        /// </summary>
        [MSValidRange(50, "Classe css deve possui no máximo 50 caracteres.")]
        [MSNotNullOrEmpty("Classe css é obrigatória.")]
        public override string mdc_classe { get; set; }

        /// <summary>
        /// 1-Ativo, 3-Excluído.
        /// </summary>
        [MSDefaultValue(1)]
        public override short mdc_situacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime mdc_dataAlteracao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime mdc_dataCriacao { get; set; }

        /// <summary>
        /// Variável auxiliar de nome do módulo
        /// </summary>
        public string mod_nome { get; set; }
	}
}