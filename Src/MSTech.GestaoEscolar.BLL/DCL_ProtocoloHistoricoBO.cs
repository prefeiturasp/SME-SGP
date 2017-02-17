/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
using MSTech.Data.Common;
using MSTech.Validation.Exceptions;
	
	/// <summary>
	/// Description: DCL_ProtocoloHistorico Business Object. 
	/// </summary>
	public class DCL_ProtocoloHistoricoBO : BusinessBase<DCL_ProtocoloHistoricoDAO, DCL_ProtocoloHistorico>
	{
        /// <summary>
        /// Salva a entidade DCL_ProtocoloHistorico.
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <param name="banco">Transação com banco de dados</param>
        /// <returns></returns>
        public static new bool Save(DCL_ProtocoloHistorico entity, TalkDBTransaction banco)
        {
            if (!entity.Validate())
                throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));

            DCL_ProtocoloHistoricoDAO dao = new DCL_ProtocoloHistoricoDAO { _Banco = banco };
            return dao.Salvar(entity);
        }
	}
}