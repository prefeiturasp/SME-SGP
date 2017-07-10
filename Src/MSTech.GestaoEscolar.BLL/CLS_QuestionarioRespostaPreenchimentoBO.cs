/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using Data.Common;
    using Validation.Exceptions;    /// <summary>
                                    /// Description: CLS_QuestionarioRespostaPreenchimento Business Object. 
                                    /// </summary>
    public class CLS_QuestionarioRespostaPreenchimentoBO : BusinessBase<CLS_QuestionarioRespostaPreenchimentoDAO, CLS_QuestionarioRespostaPreenchimento>
	{
        public static new bool Save(CLS_QuestionarioRespostaPreenchimento entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                return new CLS_QuestionarioRespostaPreenchimentoDAO { _Banco = banco }.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// Exclui os conteudos respondidos por reap_id
        /// </summary>
        /// <param name="reap_id"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool ExcluiPorReapId(long reap_id, TalkDBTransaction banco)
        {
            return new CLS_QuestionarioRespostaPreenchimentoDAO { _Banco = banco }.ExcluirPorReapId(reap_id);
        }
    }
}