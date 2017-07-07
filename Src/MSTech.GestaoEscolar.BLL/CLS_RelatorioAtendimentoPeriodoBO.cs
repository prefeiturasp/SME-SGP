/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using Data.Common;
    using System.Data;
    using System;
    using System.Linq;/// <summary>
                      /// Description: CLS_RelatorioAtendimentoPeriodo Business Object. 
                      /// </summary>
    public class CLS_RelatorioAtendimentoPeriodoBO : BusinessBase<CLS_RelatorioAtendimentoPeriodoDAO, CLS_RelatorioAtendimentoPeriodo>
	{
        /// <summary>
        /// Seleciona os tipos de período de calendário para preenchimento do relatório de atendimento
        /// </summary>
        /// <param name="rea_id"></param>
        /// <returns></returns>
        public static List<CLS_RelatorioAtendimentoPeriodo> SelecionaPorRelatorio(int rea_id, TalkDBTransaction banco = null)
        {
            CLS_RelatorioAtendimentoPeriodoDAO dao = new CLS_RelatorioAtendimentoPeriodoDAO();
            if (banco == null)
            {
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            }
            else
            {
                dao._Banco = banco;
            }

            try
            {
                return dao.SelecionaPorRelatorio(rea_id);
            }
            catch (Exception ex)
            {
                if (banco == null)
                {
                    dao._Banco.Close(ex);
                }
                throw;
            }
            finally
            {
                if (banco == null && dao._Banco.ConnectionIsOpen)
                {
                    dao._Banco.Close();
                }
            }
        }

        /// <summary>
        /// Atualiza os periodos vinculados ao relatório de atendimento.
        /// </summary>
        /// <param name="dtRelatorioAtendimentoPeriodo"></param>
        /// <returns></returns>
        public static bool AtualizarPeriodos(List<CLS_RelatorioAtendimentoPeriodo> lstPeriodo, TalkDBTransaction banco = null)
        {
            CLS_RelatorioAtendimentoPeriodoDAO dao = new CLS_RelatorioAtendimentoPeriodoDAO();
            if (banco == null)
            {
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            }
            else
            {
                dao._Banco = banco;
            }

            try
            {
                using (DataTable dt = lstPeriodo.Select(p => new TipoTabelaRelatorioAtendimento(p).ToDataRow()).CopyToDataTable())
                {
                    return dao.AtualizarPeriodos(dt);
                }
            }
            catch (Exception ex)
            {
                if (banco == null)
                {
                    dao._Banco.Close(ex);
                }
                throw;
            }
            finally
            {
                if (banco == null && dao._Banco.ConnectionIsOpen)
                {
                    dao._Banco.Close();
                }
            }
        }
    }

    public class TipoTabelaRelatorioAtendimento : TipoTabela
    {
        [Order]
        public int rea_id { get; set; }

        [Order]
        public int tpc_id { get; set; }

        public TipoTabelaRelatorioAtendimento(CLS_RelatorioAtendimentoPeriodo entity) : base(entity)
        {

        }
    }
}