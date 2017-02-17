/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using MSTech.Data.Common;
    using System.Collections.Generic;
    using System;
    using MSTech.Validation.Exceptions;
    using System.Linq;

    #region Enumeradores

    /// <summary>
    /// Código de tipo de lançamento.
    /// </summary>
    public enum EnumTipoLancamento : byte
    {
        ConceitoGlobal = 1
        , Disciplinas = 2
        , HistoricoEscolar = 3
    }

    /// <summary>
    /// Resultado da avaliação geral do aluno na turmaDisciplina.
    /// </summary>
    public enum TipoResultado : byte
    {
        Aprovado = 1
        ,
        Reprovado = 2
        ,
        ReprovadoFrequenciaHistorico = 3
        ,
        ReprovadoFrequencia = 8
        ,
        RecuperacaoFinal = 9
        ,
        AprovadoConselho = 10
    }

    #endregion

    /// <summary>
    /// Description: ACA_TipoResultado Business Object. 
    /// </summary>
    public class ACA_TipoResultadoBO : BusinessBase<ACA_TipoResultadoDAO, ACA_TipoResultado>
    {
        #region Métodos de pesquisa

        /// <summary>
        /// Busca os tipos de resultados com base no curso.
        /// </summary>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do curriculo</param>
        /// <returns></returns>
        public static DataTable SELECT_By_Pesquisa(int cur_id, int crr_id)
        {
            ACA_TipoResultadoDAO dao = new ACA_TipoResultadoDAO();
            return dao.SELECT_By_Pesquisa(cur_id, crr_id, out totalRecords);                       
        }

        /// <summary>
        /// Busca os tipos de resultados pelo tipo de lançamento.
        /// </summary>
        /// <param name="cur_id">Tipo de lançamento</param>
        /// <returns></returns>
        public static DataTable SELECT_By_TipoLancamento(byte tpr_tipoLancamento)
        {
            ACA_TipoResultadoDAO dao = new ACA_TipoResultadoDAO();
            return dao.SELECT_By_TipoLancamento(tpr_tipoLancamento);
        }

        /// <summary>
        /// Seleciona tipos de resultado por tipo lançamento e tipo de currículo período e ano letivo.
        /// </summary>
        /// <param name="tpr_tipoLancamento">Tipo de lançamento do resultado.</param>
        /// <param name="tcp_id">ID do tipo de currículo período.</param>
        /// <param name="anoLetivo">Ano letivo.</param>
        /// <returns></returns>
        public static DataTable SelecionaPorTipoLancamentoTipoCurriculoPeriodoAno(byte tpr_tipoLancamento, int tcp_id, int anoLetivo)
        {
            return new ACA_TipoResultadoDAO().SelecionaPorTipoLancamentoTipoCurriculoPeriodoAno(tpr_tipoLancamento, tcp_id, anoLetivo);
        }

        #endregion Métodos de pesquisa

        #region Inclusão

        public static new bool Save(ACA_TipoResultado entity, TalkDBTransaction banco)
        {
            ACA_TipoResultadoDAO dao = new ACA_TipoResultadoDAO { _Banco = banco };
            if (entity.Validate())
                return dao.Salvar(entity);
            else
                throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));

        }

        /// <summary>
        /// Método que grava no banco a inclusao ou alteração
        /// </summary>
        /// <param name="entity">entidade a ser salva/alterada</param>
        /// <param name="lista">lista de series que estao ligadas ao tipo de resultado</param>
        /// <returns></returns>
        public static bool Save(ACA_TipoResultado entity, IList<ACA_TipoResultadoCurriculoPeriodo> lista)
        {
            ACA_TipoResultadoDAO dao = new ACA_TipoResultadoDAO();

            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                if (Save(entity, dao._Banco))
                {
                    DataTable dtCP = ACA_TipoResultadoCurriculoPeriodoBO.SelectBy_tpr_id(entity.tpr_id, dao._Banco);

                    // Busca e exclui os relacionamentos existentes
                    for (int i = 0; i < dtCP.Rows.Count; i++)
                    {
                        ACA_TipoResultadoCurriculoPeriodo entityCP = new ACA_TipoResultadoCurriculoPeriodo
                        {
                            cur_id = Convert.ToInt32(dtCP.Rows[i]["cur_id"]),
                            crr_id = Convert.ToInt32(dtCP.Rows[i]["crr_id"]),
                            crp_id = Convert.ToInt32(dtCP.Rows[i]["crp_id"]),
                            tpr_id = Convert.ToInt32(dtCP.Rows[i]["tpr_id"])
                        };
                        ACA_TipoResultadoCurriculoPeriodoBO.Delete(entityCP, dao._Banco);
                        GestaoEscolarUtilBO.LimpaCache(String.Format("{0}_{1}_{2}_{3}", ACA_TipoResultadoCurriculoPeriodoBO.Cache_TipoResultado, entityCP.cur_id, entityCP.crr_id, entityCP.crp_id));
                    }

                    // Valida se o mesmo curriculoperiodo ja esta cadastrado em outro tipo de resultado
                    foreach (ACA_TipoResultadoCurriculoPeriodo item in lista)
                    {
                        List<Struct_TipoResultado> listaTiposResultados = ACA_TipoResultadoCurriculoPeriodoBO.SelecionaTipoResultado(item.cur_id, item.crr_id, item.crp_id, (EnumTipoLancamento)entity.tpr_tipoLancamento, entity.tds_id);
                        if (listaTiposResultados.Count > 0)
                        {
                            Struct_TipoResultado tipoResultado = listaTiposResultados.FirstOrDefault(p => p.tpr_resultado == entity.tpr_resultado && p.tpr_id != entity.tpr_id);
                            if (tipoResultado.tpr_id > 0)
                                throw new ValidationException("A série " + tipoResultado.crp_descricao + " já esta cadastrada em outro tipo de resultado.");
                        }
                        item.tpr_id = entity.tpr_id;
                        ACA_TipoResultadoCurriculoPeriodoBO.Save(item, dao._Banco);
                        GestaoEscolarUtilBO.LimpaCache(String.Format("{0}_{1}_{2}_{3}", ACA_TipoResultadoCurriculoPeriodoBO.Cache_TipoResultado, item.cur_id, item.crr_id, item.crp_id));
                    }
                }
            }
            catch (Exception err)
            {
                dao._Banco.Close(err);

                throw err;
            }
            finally
            {
                if (dao._Banco.ConnectionIsOpen)
                    dao._Banco.Close();
            }

            return true;
        }

        #endregion Inclusão
    }
}