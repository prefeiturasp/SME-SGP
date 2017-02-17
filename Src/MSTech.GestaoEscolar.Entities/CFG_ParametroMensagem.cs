/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class CFG_ParametroMensagem : Abstract_CFG_ParametroMensagem
	{
        /// <summary>
        /// ID do parâmetro de mensagem.
        /// </summary>
        [DataObjectField(true, true, false)]
        public override int pms_id { get; set; }

        /// <summary>
        /// Tela que será utilizada a mensagem. 
        /// 1-Planejamento anual, 2-Cadastro de aulas, 3-Captura de foto do aluno
        /// </summary>
        public override byte pms_tela { get; set; }

        /// <summary>
        /// Chave do parâmetro de mensagem.
        /// </summary>
        [MSValidRange(100, "Chave pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Chave é obrigatório.")]
        public override string pms_chave { get; set; }

        /// <summary>
        /// Mensagem.
        /// </summary>
        [MSValidRange(2000, "Valor pode conter até 20000 caracteres.")]
        [MSNotNullOrEmpty("Valor é obrigatório.")]
        public override string pms_valor { get; set; }

        /// <summary>
        /// Descrição da utilização da mensagem.
        /// </summary>
        [MSValidRange(200, "Descrição pode conter até 200 caracteres.")]
        public override string pms_descricao { get; set; }

        /// <summary>
        /// Situação do registro. 1-Ativo,3-Excluido
        /// </summary>
        [MSDefaultValue(1)]
        public override byte pms_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public override DateTime pms_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro.
        /// </summary>
        public override DateTime pms_dataAlteracao { get; set; }

        /// <summary>
        /// Descrição da tela de acordo com o campo pms_tela.
        /// </summary>
        public string NomeTela { get; set; }
	}
}