/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using MSTech.Data.Common;
    using MSTech.Validation.Exceptions;
	
	/// <summary>
	/// Description: CLS_ConfiguracaoAtividadeTipoAtividade Business Object. 
	/// </summary>
	public class CLS_ConfiguracaoAtividadeTipoAtividadeBO : BusinessBase<CLS_ConfiguracaoAtividadeTipoAtividadeDAO, CLS_ConfiguracaoAtividadeTipoAtividade>
	{
        /// <summary>
        /// Seleciona os tipos de atividade ativos de acordo com a configuração de atividade
        /// </summary>
        /// <param name="caa_id">ID da configuração de atividade</param>
        /// <returns>Todos os tipos de atividade ativos porém se está configurado em determinda atividade </returns>
        public static List<CLS_ConfiguracaoAtividadeTipoAtividade> GetSelectBy_Ativos(int caa_id)
        {
            CLS_ConfiguracaoAtividadeTipoAtividadeDAO dao = new CLS_ConfiguracaoAtividadeTipoAtividadeDAO();
            return dao.GetSelectBy_Ativos(caa_id);
        }

        public static new bool Save(CLS_ConfiguracaoAtividadeTipoAtividade entity, TalkDBTransaction banco)
        {
            if (!entity.Validate())
            {
                throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
            }

            CLS_ConfiguracaoAtividadeTipoAtividadeDAO dao = new CLS_ConfiguracaoAtividadeTipoAtividadeDAO { _Banco = banco };
            return dao.Salvar(entity);
        }

	}
}