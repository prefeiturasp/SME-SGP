/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System.Web;
using System.Data;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumerador
    
    /// <summary>
    /// Tela que será utilizada a mensagem.
    /// </summary>
    public enum CFG_ParametroMensagemTela : byte
    {
        PlanejamentoAnual = 1, 
        CadastroAulas = 2,
        CapturaFotoAluno = 3,
        BoletimOnline = 4
    }

    /// <summary>
    /// Chaves dos parâmetros de mensagem.
    /// </summary>
    public enum CFG_ParametroMensagemChave
    {
        PLANEJAMENTO_DIAGNOSTICOINICIAL = 1
        , PLANEJAMENTO_PLANEJAMENTOANUAL
        , PLANEJAMENTO_COC_AVALIACAO
        , PLANEJAMENTO_COC_REPLANEJAMENTO
        , PLANEJAMENTO_HABILIDADES
        , AULAS_PLANOAULA
        , AULAS_AVALIACAOAULA
        , AULAS_ATIVIDADECASA
        , AULAS_ANOTACOESALUNO
        , AULAS_SINTESE
        , CAPTURA_REQUERFLASH
        , BOLETIMONLINE_RODAPE
        , PLANEJAMENTO_RECURSOS
        , PLANEJAMENTO_COC_RECURSOS
        , ATIVIDADE_AVALIATIVA_EFETIVADO
        , PLANEJAMENTO_AVALIACAO
        , PLANEJAMENTO_BIMESTRE_INTERVENCOESPEDAGOGICAS
        , PLANEJAMENTO_BIMESTRE_REGISTROINTERVENCOES
        , PLANEJAMENTO_COC_REPLANEJAMENTOFINAL
    }

    /// <summary>
    /// Situação do registro.
    /// </summary>
    public enum CFG_ParametroMensagemSituacao : byte
    {
        Ativo = 1
        , Excluido = 3
    }

    #endregion

    /// <summary>
	/// CFG_ParametroMensagem Business Object 
	/// </summary>
	public class CFG_ParametroMensagemBO : BusinessBase<CFG_ParametroMensagemDAO,CFG_ParametroMensagem>
	{
        #region Propriedades

        private static IDictionary<CFG_ParametroMensagemChave, string[]> parametros;

        /// <summary>
        /// Retorna os parâmetros de mensagens do sistema.
        /// </summary>
        internal static IDictionary<CFG_ParametroMensagemChave, string[]> Parametros
        {
            get
            {
                if ((parametros == null) || (parametros.Count == 0))
                {
                    // O objeto não pode estar nulo quando lock.
                    parametros = new Dictionary<CFG_ParametroMensagemChave, string[]>();
                    lock (parametros)
                    {
                        SelecionaParametrosAtivos(out parametros);
                    }
                }
                return parametros;
            }
        }

        /// <summary>    
        /// Retorna os parâmetros de mensagem
        /// </summary>
        public static List<CFG_ParametroMensagem> ParametrosMSG
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    List<CFG_ParametroMensagem> paramMSG = (List<CFG_ParametroMensagem>)HttpContext.Current.Application["ParamMSG"];
                    if ((paramMSG == null) || (paramMSG.Count == 0))
                    {
                        paramMSG = (List<CFG_ParametroMensagem>)BLL.CFG_ParametroMensagemBO.GetSelect();
                        try
                        {
                            HttpContext.Current.Application.Lock();
                            HttpContext.Current.Application["ParamMSG"] = paramMSG;
                        }
                        finally
                        {
                            HttpContext.Current.Application.UnLock();
                        }
                    }
                    return paramMSG;
                }
                return new List<CFG_ParametroMensagem>();
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Seleciona o valor de um parâmetro filtrado pela chave.
        /// </summary>
        /// <param name="pms_chave">Enum que representa a chave a ser pesquisada.</param>        
        /// <returns>O valor do parâmetro (pms_valor).</returns>
        public static string RetornaValor
        (
            CFG_ParametroMensagemChave pms_chave
        )
        {
            string valor = "";
            if (Parametros.ContainsKey(pms_chave))
                valor = Parametros[pms_chave].FirstOrDefault();

            valor = HttpUtility.HtmlDecode(valor);

            return valor;
        }

        /// <summary>
        /// Retorna os parâmetros de integração ativos.
        /// </summary>
        private static void SelecionaParametrosAtivos(out IDictionary<CFG_ParametroMensagemChave, string[]> dictionary)
        {
            IList<CFG_ParametroMensagem> lt = GetSelect();

            dictionary = (from CFG_ParametroMensagem ent in lt
                          where Enum.IsDefined(typeof(CFG_ParametroMensagemChave), ent.pms_chave)
                          group ent by ent.pms_chave into t
                          select new
                          {
                              chave = t.Key
                              ,
                              valor = t.Select(p => p.pms_valor).ToArray()
                          }).ToDictionary(
                                p =>(CFG_ParametroMensagemChave)Enum.Parse(typeof(CFG_ParametroMensagemChave), p.chave)
                                , p => p.valor);
        }

         /// <summary>
        /// Recarrega os parâmetros de integração do sistema.
        /// </summary>
        public static void RecarregaParametrosAtivos()
        {
            parametros = new Dictionary<CFG_ParametroMensagemChave, string[]>();
            lock (parametros)
            {
                SelecionaParametrosAtivos(out parametros);
            }
        }

        /// <summary>
        /// Busca o valor do parametro disciplina pela chave.
        /// </summary>
        /// <param name="pms_chave">Chave de parametro mensagem</param>
        /// <returns></returns>
        public static string BuscaValoraPorChave(string pms_chave)
        {
            CFG_ParametroMensagemDAO dao = new CFG_ParametroMensagemDAO();

            DataTable dt = dao.BuscaValoraPorChave(pms_chave);

            return dt.Rows[0]["pms_valor"].ToString();
        }

        /// <summary>
        /// Verifica se já tem cadastrado um parâmetro com a mesma chave informada
        /// </summary>
        /// <param name="entity">Chave de parametro mensagem</param>        
        /// <returns>Se já tem um parâmetro com a mesma chave.</returns>
        public static bool VerificaParametroExistente(string pms_chave)
        {
            CFG_ParametroMensagemDAO dao = new CFG_ParametroMensagemDAO();
            return dao.BuscaValoraPorChave(pms_chave).Rows.Count > 0;
        }

        #endregion
    }
}