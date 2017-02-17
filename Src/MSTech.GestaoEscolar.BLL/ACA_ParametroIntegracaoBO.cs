/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System.Data;
using MSTech.Validation.Exceptions;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using MSTech.Data.Common;
using System.Web;
using System.Linq;
using System.Threading;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Enum com as situações do parâmetro.
    /// </summary>
    public enum eSituacao
    {
        Ativo = 1
        ,
        Interno = 4
    }

    /// <summary>
    /// Enum com as chaves dos parâmetros.
    /// </summary>
    public enum eChaveIntegracao
    {
        HABILITA_INTEG_COLAB_DOCENTES
        ,
        MSG_AVISO_DOCENTES
        ,
        MSG_ATUALIZACAO_DADOS
        ,
        DATA_HORA_ATUALIZACAO_DADOS_MAGISTER
    }

    #endregion

    public class ACA_ParametroIntegracaoBO : BusinessBase<ACA_ParametroIntegracaoDAO, ACA_ParametroIntegracao>
    {
        #region Propriedades

        private static IDictionary<string, string[]> parametros;

        /// <summary>
        /// Retorna os parâmetros de integração do sistema.
        /// </summary>
        private static IDictionary<string, string[]> Parametros
        {
            get
            {
                if ((parametros == null) || (parametros.Count == 0))
                {
                    // O objeto não pode estar nulo quando lock.
                    parametros = new Dictionary<string, string[]>();
                    lock (parametros)
                    {
                        SelecionaParametrosAtivos(out parametros);
                    }
                }
                return parametros;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Seleciona o valor de um parâmetro filtrado por pri_chave.
        /// </summary>
        /// <param name="pri_chave">Enum que representa a chave a ser pesquisada.</param>        
        /// <returns>O valor do parâmetro (pri_valor).</returns>
        public static string ParametroValor
        (
            eChaveIntegracao pri_chave
        )
        {
            string valor = string.Empty;
            if (Parametros.ContainsKey(Enum.GetName(typeof(eChaveIntegracao), pri_chave)))
                valor = Parametros[Enum.GetName(typeof(eChaveIntegracao), pri_chave)].FirstOrDefault();

            return valor;
        }

        /// <summary>
        /// Verifica se existe um parâmetro de integração que possua a mesma chave.
        /// </summary>
        /// <param name="entity">Entidade ACA_ParametroIntegracao</param>
        /// <returns></returns>
        public static bool VerificarChaveExistente(ACA_ParametroIntegracao entity)
        {
            ACA_ParametroIntegracaoDAO dao = new ACA_ParametroIntegracaoDAO();
            return dao.SelecionaPorChave(entity);
        }

        /// <summary>
        /// Salva (inclusão ou alteração) um parâmetro de integração.
        /// </summary>
        /// <param name="entity">Entidade ACA_ParametroIntegracao</param>
        /// <returns></returns>
        public static bool Salvar(ACA_ParametroIntegracao entity)
        {
            ACA_ParametroIntegracaoDAO dao = new ACA_ParametroIntegracaoDAO();

            // Verifica chave do parâmetro de integração
            if (VerificarChaveExistente(entity))
                throw new DuplicateNameException("Já existe um parâmetro de integração cadastrado com esta chave.");

            if (entity.Validate())
                return dao.Salvar(entity);
            else
                throw new ValidationException(MSTech.CoreSSO.BLL.UtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// Retorna os parâmetros de integração ativos.
        /// </summary>
        /// <returns>Lista de entidades ACA_ParametroIntegracao.</returns>
        public static List<ACA_ParametroIntegracao> Consultar()
        {
            ACA_ParametroIntegracaoDAO dao = new ACA_ParametroIntegracaoDAO();
            return dao.SelecionaTodosAtivos();
        }

        /// <summary>
        /// Retorna os parâmetros de integração ativos.
        /// </summary>
        private static void SelecionaParametrosAtivos(out IDictionary<string, string[]> dictionary)
        {
            List<ACA_ParametroIntegracao> lt = Consultar();

            dictionary = (from ACA_ParametroIntegracao pri in lt
                          group pri by pri.pri_chave into t
                          select new
                          {
                              chave = t.Key
                              ,
                              valor = t.Select(p => p.pri_valor).ToArray<string>()
                          }).ToDictionary(p => p.chave, p => p.valor);
        }

        /// <summary>
        /// Recarrega os parâmetros de integração do sistema.
        /// </summary>
        public static void RecarregaParametrosAtivos()
        {
            parametros = new Dictionary<string, string[]>();
            lock (parametros)
            {
                SelecionaParametrosAtivos(out parametros);
            }
        }

        #endregion
    }
}