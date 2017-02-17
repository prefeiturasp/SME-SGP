/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using MSTech.Validation.Exceptions;
    using System.Data;
    using MSTech.Data.Common;
    using System;
    using System.Collections.Generic;

    #region Enumeradores

    /// <summary>
    /// Enumerador com as situações do plano de ciclo.
    /// </summary>
    public enum PlanejamentoCicloSituacao
    {
        Ativo = 1
        ,
        Bloqueado = 2
        ,
        Excluido = 3
        ,
        Inativo = 4
    }

    #endregion

    /// <summary>
	/// Description: CLS_PlanejamentoCiclo Business Object. 
	/// </summary>
	public class CLS_PlanejamentoCicloBO : BusinessBase<CLS_PlanejamentoCicloDAO, CLS_PlanejamentoCiclo>
    {
        #region Métodos de consulta

        /// <summary>
        /// Seleciona o plano de ciclo ativo por turma e tipo de ciclo.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="tci_id">ID do tipo de ciclo.</param>
        /// <returns></returns>
        public static CLS_PlanejamentoCiclo SelecionaAtivoPorTurmaTipoCiclo(long tur_id, int tci_id)
        {
            return new CLS_PlanejamentoCicloDAO().SelecionaAtivoPorTurmaTipoCiclo(tur_id, tci_id);
        }

        /// <summary>
        /// Seleciona as alterações realizadas no plano de ciclo
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="tci_id">ID do tipo de ciclo.</param>
        /// <returns></returns>
        public static DataTable SelecionaHistoricoAlteracoes(long tur_id, int tci_id)
        {
            return new CLS_PlanejamentoCicloDAO().SelecionaHistoricoAlteracoes(tur_id, tci_id);
        }

        #endregion Métodos de consulta

        #region Métodos para inserir / alterar

        /// <summary>
        /// Salva a entidade de planejamento do ciclo.
        /// </summary>
        /// <param name="entity">Entidade de planejamento do ciclo.</param>
        /// <returns></returns>
        public static new bool Save(CLS_PlanejamentoCiclo entity)
        {
            if (entity.Validate())
            {
                return new CLS_PlanejamentoCicloDAO().Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// Salva a entidade de planejamento do ciclo.
        /// </summary>
        /// <param name="entity">Entidade de planejamento do ciclo.</param>
        /// <returns></returns>
        public static new bool Save(CLS_PlanejamentoCiclo entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                return new CLS_PlanejamentoCicloDAO { _Banco = banco }.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// Atualiza a situação dos planos de ciclo por escola, ano letivo e tipo de ciclo.
        /// </summary>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade da escola.</param>
        /// <param name="plc_anoLetivo">Ano letivo.</param>
        /// <param name="tci_id">Id do tipo de ciclo.</param>
        /// <param name="plc_situacao">Situacao.</param>
        /// <returns></returns>
        public static bool AtualizaSituacaoPorEscolaAnoTipoCiclo(int esc_id, int uni_id, int plc_anoLetivo, int tci_id, byte plc_situacao, TalkDBTransaction banco)
        {
            return new CLS_PlanejamentoCicloDAO { _Banco = banco }.AtualizaSituacaoPorEscolaAnoTipoCiclo(esc_id, uni_id, plc_anoLetivo, tci_id, plc_situacao);
        }

        /// <summary>
        /// Salva a entidade de planejamento do ciclo e inativa as alterações anteriores.
        /// </summary>
        /// <param name="entity">Entidade de planejamento do ciclo.</param>
        /// <returns></returns>
        public static bool SalvarPlanoCiclo(CLS_PlanejamentoCiclo entity)
        {
            TalkDBTransaction banco = new CLS_PlanejamentoCicloDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                AtualizaSituacaoPorEscolaAnoTipoCiclo(entity.esc_id, entity.uni_id, entity.plc_anoLetivo, entity.tci_id, (byte)PlanejamentoCicloSituacao.Inativo, banco);

                Save(entity, banco);

                return true;
            }
            catch (Exception ex)
            {
                if (banco.ConnectionIsOpen)
                {
                    banco.Close(ex);
                }

                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                {
                    banco.Close();
                }
            }
        }

        #endregion Métodos para inserir / alterar
    }
}