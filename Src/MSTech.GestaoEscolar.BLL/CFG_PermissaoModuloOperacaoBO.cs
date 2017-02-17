/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using Data.Common;

    /// <summary>
    /// Description: CFG_PermissaoModuloOperacao Business Object. 
    /// </summary>
    public class CFG_PermissaoModuloOperacaoBO : BusinessBase<CFG_PermissaoModuloOperacaoDAO, CFG_PermissaoModuloOperacao>
	{
	    public enum Operacao
        {
            HistoricoEscolarDadosaluno = 1, 
            HistoricoEscolarEnsinoFundamental = 2, 
            HistoricoEscolarEJA = 3, 
            HistoricoEscolarTransferencia = 4,
            HistoricoEscolarInformacoesComplementares = 5,
            DiarioClasseExclusaoAulas = 6,
            DiarioClasseAnotacoesAluno = 7,
            FechamentoVisualizacaoObservacoes = 8,
            FechamentoExibicaoAbaParecerConclusivo = 9,
            FechamentoExibicaoAbaJustificativaPosConselho = 10,
            FechamentoExibicaoAbaDesempenhoAprendizagem = 11,
            FechamentoExibicaoAbaRecomendacaoAluno = 12,
            FechamentoExibicaoAbaRecomendacaoResponsavel = 13,
            FechamentoExibicaoAbaAnotacoesAluno = 14,
            DiarioClasseLancamentoFrequencia = 15,
            DiarioClasseLancamentoFrequenciaInfantil = 16
        }

        public static List<CFG_PermissaoModuloOperacao> VerificaPermissao(int sis_id, int mod_id, System.Guid gru_id, List<Operacao> list)
        {
            CFG_PermissaoModuloOperacaoDAO dao = new CFG_PermissaoModuloOperacaoDAO();
            string operacoes = string.Join(",", (from Operacao i in list select Convert.ToInt32(i)).ToArray());

            return dao.VerificaPermissao(sis_id, mod_id, gru_id, operacoes);
        }
        
        public static List<CFG_PermissaoModuloOperacao> VerificaPermissaoModuloOperacao(int sis_id, int mod_id, Guid gru_id, List<string> list)
        {
            CFG_PermissaoModuloOperacaoDAO dao = new CFG_PermissaoModuloOperacaoDAO();
            string operacoes = string.Join(",", list.ToArray());

            return dao.VerificaPermissao(sis_id, mod_id, gru_id, operacoes);
        }

        public static bool Salvar(List<CFG_PermissaoModuloOperacao> listaPermissoes)
        {
            TalkDBTransaction bancoGestao = new CFG_PermissaoModuloOperacaoDAO()._Banco.CopyThisInstance();
            try
            {
                bancoGestao.Open();
                foreach (CFG_PermissaoModuloOperacao permissao in listaPermissoes)
                {
                    Save(permissao, bancoGestao);
                }
            }
            catch (Exception e)
            {
                bancoGestao.Close(e);
                throw;
            }
            finally
            {
                if (bancoGestao.ConnectionIsOpen)
                {
                    bancoGestao.Close();
                }
            }
            return true;
        }
    }
}