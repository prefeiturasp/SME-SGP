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
	/// Description: CLS_AlunoProjeto Business Object. 
	/// </summary>
	public class CLS_AlunoProjetoBO : BusinessBase<CLS_AlunoProjetoDAO, CLS_AlunoProjeto>
    {
        #region Métodos para incluir / alterar

        /// <summary>
        /// O método salva os dados da entidade CLS_AlunoProjeto.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public new static bool Save(CLS_AlunoProjeto entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                return new CLS_AlunoProjetoDAO { _Banco = banco }.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        #endregion
    }
}