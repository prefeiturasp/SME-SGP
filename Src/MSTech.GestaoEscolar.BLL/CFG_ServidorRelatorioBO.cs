/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using System;
    using MSTech.Validation.Exceptions;
    using System.ComponentModel;
    using System.Data;
    using System.Web;

    /// <summary>
    /// Description: CFG_ServidorRelatorio Business Object. 
    /// </summary>
    [DataObjectAttribute()]
    public class CFG_ServidorRelatorioBO : BusinessBase<CFG_ServidorRelatorioDAO, CFG_ServidorRelatorio>
    {
        public static string RetornaChaveCache_CarregarServidorRelatorioPorEntidade(Guid ent_id)
        {
            return String.Format("Cache_CarregarServidorRelatorioPorEntidade_{0}", ent_id);
        }

        /// <summary>
        /// Retorna as configurações do servidor de relatório conforme o id do relatório da entidade do sistema.
        /// </summary>
        /// <param name="ent_id">Entidade em que o usuário está logado.</param>        
        /// <returns>Servidor de relatório</returns>
        public static CFG_ServidorRelatorio CarregarServidorRelatorioPorEntidade(Guid ent_id, int appMinutosCacheLongo = 0)
        {
            CFG_ServidorRelatorio entity = null;

            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_CarregarServidorRelatorioPorEntidade(ent_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    entity = new CFG_ServidorRelatorioDAO().CarregarServidorRelatorioPorEntidade(ent_id);
                    HttpContext.Current.Cache.Insert(chave, entity, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    entity = (CFG_ServidorRelatorio)cache;
                }
            }

            if (entity == null)
            {
                entity = new CFG_ServidorRelatorioDAO().CarregarServidorRelatorioPorEntidade(ent_id);
            }

            return entity;
        }

        /// <summary>
        /// Salva todas as informações relacionadas ao servidor de relatórios
        /// </summary>
        /// <param name="srr">Servidor de relatórios</param>
        /// <param name="relatorios">Lista de relatórios relacionados ao servidor</param>
        /// <returns>Verdadeiro ou Falso</returns>        
        public static bool SalvarServidorRelatorio(CFG_ServidorRelatorio srr, List<CFG_RelatorioServidorRelatorio> listRlt)
        {
            bool salvou = false;

            // Única transação para salvar todos os dados
            CFG_RelatorioDAO rltDao = new CFG_RelatorioDAO();
            rltDao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                // Salva os dados do servidor de relatórios
                CFG_ServidorRelatorioBO.Save(ValidarEntidade(srr), rltDao._Banco);

                // Deleta todos os relatórios relacionados ao Servidor para posterior atualização
                CFG_RelatorioServidorRelatorioBO.DeletarRelatoriosPorEntidadeServidor(srr, rltDao._Banco);

                // Inserção dos relatórios
                listRlt.ForEach(p => CFG_RelatorioServidorRelatorioBO.Save(p, rltDao._Banco));

                salvou = true;
            }
            catch (Exception ex)
            {
                rltDao._Banco.Close(ex);
                throw;
            }
            finally
            {
                if (rltDao._Banco.ConnectionIsOpen)
                    rltDao._Banco.Close();
            }

            return salvou;
        }

        /// <summary>
        /// Valida as informações preenchidas na entidade CFG_ServidorRelatorio
        /// </summary>
        /// <param name="srr">Servidor de relatórios</param>
        /// <returns>Entidade CFG_ServidorRelatorio</returns>
        private static CFG_ServidorRelatorio ValidarEntidade(CFG_ServidorRelatorio srr)
        {
            CFG_ServidorRelatorio rlt = new CFG_ServidorRelatorio()
            {
                ent_id = srr.ent_id
                ,
                srr_id = srr.srr_id
            };

            rlt = CFG_ServidorRelatorioBO.GetEntity(rlt);

            rlt.srr_nome = srr.srr_nome;
            rlt.srr_descricao = srr.srr_descricao;
            rlt.srr_remoteServer = srr.srr_remoteServer;
            rlt.srr_usuario = srr.srr_usuario;            
            rlt.srr_dominio = srr.srr_dominio;
            rlt.srr_diretorioRelatorios = srr.srr_diretorioRelatorios;
            rlt.srr_pastaRelatorios = srr.srr_pastaRelatorios;
            rlt.srr_situacao = srr.srr_situacao;
            
            if (!(String.IsNullOrEmpty(srr.srr_senha)) && rlt.srr_remoteServer)
                rlt.srr_senha = srr.srr_senha;

            if (!rlt.srr_remoteServer)
            {
                rlt.srr_usuario = String.Empty;
                rlt.srr_senha = String.Empty;
                rlt.srr_dominio = String.Empty;
                rlt.srr_diretorioRelatorios = String.Empty;
            }

            if (rlt.IsNew)
            {
                rlt.srr_dataCriacao = DateTime.Now;
                rlt.srr_dataAlteracao = DateTime.Now;
            }

            return rlt;
        }

        /// <summary>
        /// Retorna as configurações do servidor de relatório conforme o id do relatório da entidade do sistema.
        /// </summary>
        /// <param name="idEntidade">id da entidade do usuário logado.</param>
        /// <param name="idRelatorio"></param>
        /// <returns></returns>
        public static CFG_ServidorRelatorio CarregarCredencialServidorPorRelatorio(Guid idEntidade, int idRelatorio)
        {
            #region VALIDA PARAMETROS DE ENTRADA

            if (idEntidade.Equals(Guid.Empty))
                throw new ValidationException("Parâmetro idEntidade é obrigatório.");
            if (idRelatorio <= 0)
                throw new ValidationException("Parâmetro idRelatorio é obrigatório.");

            #endregion

            CFG_ServidorRelatorioDAO dal = new CFG_ServidorRelatorioDAO();
            return dal.CarregarPorIdRelatorioSistema(idEntidade, idRelatorio);
        }
    }
}