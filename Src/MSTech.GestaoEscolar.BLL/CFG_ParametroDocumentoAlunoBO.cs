/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/
using System;
using System.Linq;
using System.Collections.Generic;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumerador

    /// <summary>
    /// Relatório que será utilizado o parâmetro.
    /// </summary>
    public enum ParametroDocumentoAlunoRelatorio : byte
    {
        BoletimEscolar = 40,
        DeclaraçãoMatrícula = 41,
        DeclaraçãoMatrículaExAluno = 42,
        DeclaraçãoPedidoTransferência = 43,
        DeclaraçãoConclusãoCurso = 44,
        FichaIndividualAlunos = 47,
        FichaCadastralAlunos = 48,
        AutorizaçãoPasseioGratuito = 49,
        ControleRecebimentoAPM = 50,
        TermoCompromisso = 51,
        DeclaraçãoEscolaridade = 55,
        HistóricoEscolar = 61,
        CertificadoConclusãoEtapaEnsino = 70,
        DeclaracaoSolicitacaoTransferencia = 158,
        DeclaracaoTrabalho = 174
    }

    /// <summary>
    /// Tipos de parâmetros de documentos do aluno que podem ser cadastrados no sistema.
    /// </summary>
    public enum TipoParametroDocumentoAluno
    {
        /// <summary>
        /// Único: possuirá sempre um único valor.
        /// </summary>
        Unico = 1
        ,
        /// <summary>
        /// Múltiplo: permitirá vários valores simultâneos.
        /// </summary>
        Multiplo = 2
    }
    /// <summary>
    /// Nome da chaves de parâmetros de documentos do aluno.
    /// </summary>
    public enum ChaveParametroDocumentoAluno
    {
        FILTRA_POR_PERIODO,                
        VALIDAR_DOCUMENTO_EMITIDO,        
        CABECALHO_ORGAO,
        DECLARACAO_HTML,
        REPORT_DEVEXPRESS
    }

    #endregion

    /// <summary>
    /// CFG_ParametroDocumentoAluno Business Object 
    /// </summary>
    public class CFG_ParametroDocumentoAlunoBO : BusinessBase<CFG_ParametroDocumentoAlunoDAO, CFG_ParametroDocumentoAluno>
    {
        #region Propriedades

        private static IDictionary<ChaveParametroDocumentoAluno, string[]> parametros;
        
        private static IList<CFG_ParametroDocumentoAluno> listParametros;

        private static IList<CFG_ParametroDocumentoAluno> listTodosParametros
        {
            get
            {
                if (listParametros == null)
                    CarregaParametrosAtivos();
                return listParametros;
            }
            set
            {
                listParametros = value;
            }
        }

        #endregion

        /// <summary>
        /// Retorna o valor do parametro para a entidade do sistema do usuário logado
        /// </summary>
        /// <param name="chave">Nome da chave do parâmetro.</param>
        /// <param name="idEntidade">id da entidade do usuário logado.</param>
        /// <returns>Valor da chave do parâmetro</returns>
        public static string ParametroValor(ChaveParametroDocumentoAluno chave, Guid idEntidade)
        {
            #region VALIDA PARÂMETROS DE ENTRADA

            if (idEntidade.Equals(Guid.Empty))
                throw new ValidationException("Parâmetro idEntidade é obrigatório.");
            string sChave = Enum.GetName(typeof(ChaveParametroDocumentoAluno), chave);
            if (String.IsNullOrEmpty(sChave))
                throw new ValidationException("Parâmetro chave é obrigatório.");

            #endregion

            return listTodosParametros.Any(p => p.pda_chave == sChave && p.ent_id == idEntidade) ?
                listTodosParametros.Where(p => p.pda_chave == sChave && p.ent_id == idEntidade)
                .FirstOrDefault().pda_valor : string.Empty;
        }


        public static string ParametroValor(ChaveParametroDocumentoAluno chave, Guid idEntidade, int rlt_id)
        {
            #region VALIDA PARÂMETROS DE ENTRADA

            if (idEntidade.Equals(Guid.Empty))
                throw new ValidationException("Parâmetro idEntidade é obrigatório.");
            string sChave = Enum.GetName(typeof(ChaveParametroDocumentoAluno), chave);
            if (String.IsNullOrEmpty(sChave))
                throw new ValidationException("Parâmetro chave é obrigatório.");

            #endregion

            return listTodosParametros.Any(p => p.pda_chave == sChave && p.ent_id == idEntidade && p.rlt_id == rlt_id) ?
                    listTodosParametros.Where(p => p.pda_chave == sChave && p.ent_id == idEntidade && p.rlt_id == rlt_id)
                    .FirstOrDefault().pda_valor : string.Empty;
        }

        public static bool ParametroValorBooleano(ChaveParametroDocumentoAluno chave, Guid idEntidade, int rlt_id)
        {
            string valor = ParametroValor(chave, idEntidade, rlt_id);

            if (string.IsNullOrEmpty(valor))
                return false;
            else
                return valor.ToLower().Equals("true") ? true : false;
        }

        /// <summary>
        /// Retorna os valores do parâmetro para a entidade do sistema do usuário logado.
        /// </summary>
        /// <param name="chave">Nome da chave do parâmetro.</param>
        /// <param name="idEntidade">id da entidade do usuário logado.</param>        
        /// <returns>null caso não encontre nada e array de valores caso encontre.</returns>
        public static string[] ParametroValores(ChaveParametroDocumentoAluno chave, Guid idEntidade)
        {
            #region VALIDA PARÂMETROS DE ENTRADA

            if (idEntidade.Equals(Guid.Empty))
                throw new ValidationException("Parâmetro idEntidade é obrigatório.");
            string sChave = Enum.GetName(typeof(ChaveParametroDocumentoAluno), chave);
            if (String.IsNullOrEmpty(sChave))
                throw new ValidationException("Parâmetro chave é obrigatório.");

            #endregion

            IList<CFG_ParametroDocumentoAluno> lt = listTodosParametros.Where(p => p.pda_chave == sChave && p.ent_id == idEntidade).ToList();

            if (lt.Count > 0)
            {
                string[] ret = new string[lt.Count];
                for (int i = 0; i < lt.Count; i++)
                    ret[i] = lt[i].pda_valor;
                return ret;
            }
            return null;
        }

        /// <summary>
        /// Retorna os parâmetros de integração ativos.
        /// </summary>
        private static void SelecionaParametrosAtivos(out IDictionary<ChaveParametroDocumentoAluno, string[]> dictionary)
        {
            dictionary = (from CFG_ParametroDocumentoAluno ent in listTodosParametros
                          group ent by ent.pda_chave into t
                          select new
                          {
                              chave = t.Key
                              ,
                              valor = t.Select(p => p.pda_valor).ToArray()
                          }).ToDictionary
                          (p =>
                              (ChaveParametroDocumentoAluno)Enum.Parse
                              (typeof(ChaveParametroDocumentoAluno), p.chave)
                            , p => p.valor);
        }

        /// <summary>
        /// Recarrega os parâmetros de integração do sistema.
        /// </summary>
        public static void RecarregaParametrosAtivos()
        {
            parametros = new Dictionary<ChaveParametroDocumentoAluno, string[]>();
            lock (parametros)
            {
                CarregaParametrosAtivos();
                SelecionaParametrosAtivos(out parametros);
            }
        }

        public static string SelectNomeDocumentoPorRelatorio(int rlt_id)
        {
            return listTodosParametros.Any(p => p.rlt_id == rlt_id) ?
                   listTodosParametros.Where(p => p.rlt_id == rlt_id)
                   .FirstOrDefault().NomeRelatorio : string.Empty;
        }

        private static void CarregaParametrosAtivos()
        {
            listParametros = new CFG_ParametroDocumentoAlunoDAO().Seleciona();
        }
    }
}