/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;
using System.ComponentModel;

namespace MSTech.GestaoEscolar.Entities
{
	/// <summary>
	/// 
	/// </summary>
    [Serializable]
    public class MTR_Movimentacao : AbstractMTR_Movimentacao
	{
        /// <summary>
        /// ID da Movimentação
        /// </summary>        
        [DataObjectField(true, false, false)]
        public override int mov_id { get; set; }

        /// <summary>
        /// Data em que a movimentação foi realizada
        /// </summary>
        [MSNotNullOrEmpty("Data de realização é obrigatório.")]
        public override DateTime mov_dataRealizacao { get; set; }

        /// <summary>
        /// 1 - Ativo, 3 - Excluído
        /// </summary>
        [MSDefaultValue(1)]
        public override byte mov_situacao { get; set; }

        /// <summary>
        /// Data de criação do registro
        /// </summary>        
        public override DateTime mov_dataCriacao { get; set; }

        /// <summary>
        /// Data de alteração do registro
        /// </summary>        
        public override DateTime mov_dataAlteracao { get; set; }
	}
}