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
    /// Enumerador com a situação do anexo.
    /// </summary>
    public enum ACA_AlunoAnexoSituacao
    {
        Ativo = 1
        ,
        Excluido = 3
    }

	/// <summary>
	/// Description: ACA_AlunoAnexo Business Object. 
	/// </summary>
	public class ACA_AlunoAnexoBO : BusinessBase<ACA_AlunoAnexoDAO, ACA_AlunoAnexo>
    {
        #region Métodos de consulta

        /// <summary>
        /// Seleciona os anexos ativos por aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="banco">Transação</param>
        /// <returns></returns>
        public static List<ACA_AlunoAnexo> SelecionaAtivosPorALuno(long alu_id, TalkDBTransaction banco = null)
        {
            return banco == null ?
                new ACA_AlunoAnexoDAO().SelecionaAtivosPorALuno(alu_id) :
                new ACA_AlunoAnexoDAO { _Banco = banco }.SelecionaAtivosPorALuno(alu_id);
        }

        #endregion

        #region Métodos para incluir / alterar

        /// <summary>
        /// Método que valida a entidade e salva o anexo.
        /// </summary>
        /// <param name="entity">Entidade ACA_AlunoAnexo</param>
        /// <param name="banco">Transação.</param>
        /// <returns></returns>
        public static new bool Save(ACA_AlunoAnexo entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
                return new ACA_AlunoAnexoDAO { _Banco = banco }.Salvar(entity);

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        #endregion
    }
}