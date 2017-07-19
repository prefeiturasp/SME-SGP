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
    using System.Data;

    /// <summary>
	/// Description: CLS_QuestionarioConteudoPreenchimento Business Object. 
	/// </summary>
	public class CLS_QuestionarioConteudoPreenchimentoBO : BusinessBase<CLS_QuestionarioConteudoPreenchimentoDAO, CLS_QuestionarioConteudoPreenchimento>
	{
        /// <summary>
        /// Retorna se o conteúdo foi preenchido.
        /// </summary>
        /// <param name="qtc_id">Id do conteúdo.</param>
        /// <returns></returns>
		public static bool ConteudoPreenchido
           (
                string qtc_ids
           )
        {
            CLS_QuestionarioConteudoPreenchimentoDAO dao = new CLS_QuestionarioConteudoPreenchimentoDAO();

            DataTable dt = string.IsNullOrEmpty(qtc_ids) ? new DataTable()
                : dao.SelecionaConteudoPreenchido(qtc_ids);
            
            return dt.Rows.Count > 0;
        }		

        public static new bool Save(CLS_QuestionarioConteudoPreenchimento entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                return new CLS_QuestionarioConteudoPreenchimentoDAO { _Banco = banco }.Salvar(entity);
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
            return new CLS_QuestionarioConteudoPreenchimentoDAO { _Banco = banco }.ExcluirPorReapId(reap_id); 
        }
	}
}