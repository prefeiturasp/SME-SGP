using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.Collections.Generic;
using System.Web;

namespace MSTech.GestaoEscolar.BLL
{
    #region Estruturas

    [Serializable]
    public struct sComboUAEscola
    {
        public Guid uad_id { get; set; }

        public Guid uad_idSuperior { get; set; }

        public string uad_nome { get; set; }

        public int esc_id { get; set; }

        public int uni_id { get; set; }

        public string esc_uni_nome { get; set; }

        public string uni_escolaNome { get; set; }

        public string esc_uni_id { get; set; }
    }

    #endregion

    public class ESC_UnidadeEscolaBO : BusinessBase<ESC_UnidadeEscolaDAO, ESC_UnidadeEscola>
    {
        #region Métodos

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola de acordo com o usuario logado e
        /// o parametro esc_controleSistema, que definirá se as escolas controladas pelo sistema serã retornadas ou não, ou será indiferente a
        /// utilização desse parametro - paginado.
        /// </summary>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="gru_id">Grupo do usuário logado - Obrigatório</param>
        /// <param name="usu_id">ID do usuário logado - Obrigatório</param>
        /// <param name="esc_controleSistema">parametro que definira se as escolas controladas pelo sistema serão retornadas ou não</param>
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaEscolasControladas
            (
                Guid ent_id
                , Guid gru_id
                , Guid usu_id
                , bool esc_controleSistema
                , bool paginado
                , int currentPage
                , int pageSize
            )
        {
            totalRecords = 0;

            if (pageSize == 0)
            {
                pageSize = 1;
            }

            ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
            return dao.SelecionaEscolasControladasPorEntidadePermissaoUsuario
                (ent_id, gru_id, usu_id, esc_controleSistema, paginado, currentPage / pageSize, pageSize, out totalRecords
                , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id));
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola de acordo com o usuario logado e
        /// o parametro esc_controleSistema, que definirá se as escolas controladas pelo sistema serã retornadas ou não, ou será indiferente a
        /// utilização desse parametro - não paginado.
        /// </summary>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="gru_id">Grupo do usuário logado - Obrigatório</param>
        /// <param name="usu_id">ID do usuário logado - Obrigatório</param>
        /// <param name="esc_controleSistema">parametro que definira se as escolas controladas pelo sistema serão retornadas ou não</param>
        /// <param name="appMinutosCacheLongo">tempo do cache</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> SelecionaEscolasControladas(Guid ent_id, Guid gru_id, Guid usu_id, bool esc_controleSistema, int appMinutosCacheLongo = 0)
        {
            List<sComboUAEscola> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_EscolasControladas(ent_id, gru_id, usu_id, esc_controleSistema);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                        DataTable dtDados = dao.SelecionaEscolasControladasPorEntidadePermissaoUsuario(ent_id, gru_id, usu_id, esc_controleSistema, false, 0, 1, out totalRecords
                                            , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id));
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboUAEscola
                                 {
                                     esc_id = string.IsNullOrEmpty(dr["esc_id"].ToString()) ? 0 : Convert.ToInt32(dr["esc_id"]),
                                     uni_id = string.IsNullOrEmpty(dr["uni_id"].ToString()) ? 0 : Convert.ToInt32(dr["uni_id"]),
                                     uni_escolaNome = dr["uni_escolaNome"].ToString(),
                                     esc_uni_nome = dr["esc_uni_nome"].ToString(),
                                     esc_uni_id = dr["esc_uni_id"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboUAEscola>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                DataTable dtDados = dao.SelecionaEscolasControladasPorEntidadePermissaoUsuario(ent_id, gru_id, usu_id, esc_controleSistema, false, 0, 1, out totalRecords
                                    , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id));
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboUAEscola
                         {
                             esc_id = string.IsNullOrEmpty(dr["esc_id"].ToString()) ? 0 : Convert.ToInt32(dr["esc_id"]),
                             uni_id = string.IsNullOrEmpty(dr["uni_id"].ToString()) ? 0 : Convert.ToInt32(dr["uni_id"]),
                             uni_escolaNome = dr["uni_escolaNome"].ToString(),
                             esc_uni_nome = dr["esc_uni_nome"].ToString(),
                             esc_uni_id = dr["esc_uni_id"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de escolas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_EscolasControladas(Guid ent_id, Guid gru_id, Guid usu_id, bool esc_controleSistema)
        {
            return string.Format("Cache_SelecionaEscolasControladas_{0}_{1}_{2}_{3}", ent_id, gru_id, usu_id, esc_controleSistema);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de escolas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_Select_Cache(int esc_id, int uni_id, byte uni_situacao, Guid ent_id, Guid gru_id, Guid usu_id)
        {
            return string.Format("Cache_GetSelect_Cache_{0}_{1}_{2}_{3}_{4}_{5}", esc_id, uni_id, uni_situacao, ent_id, gru_id, usu_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de escolas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelectPermissaoTotal_Cache(Guid ent_id, bool situacao_Desativado, Nullable<bool> esc_controleSistema)
        {
            return string.Format("Cache_GetSelectPermissaoTotal_Cache_{0}_{1}_{2}", ent_id, situacao_Desativado, esc_controleSistema.HasValue ? esc_controleSistema.ToString() : "");
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de escolas
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_SelectEscolas_VisaoAluno(long alu_id, Guid ent_id)
        {
            return string.Format("Cache_GetSelectEscolas_VisaoAluno_{0}_{1}", alu_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de escolas
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_SelectEscolas_VisaoResponsavel(Guid pes_id, Guid ent_id)
        {
            return string.Format("Cache_GetSelectEscolas_VisaoResponsavel_{0}_{1}", pes_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de escolas
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_SelectEscolas_VisaoIndividual(long doc_id, Guid ent_id, byte vinculoColaboradorCargo)
        {
            return string.Format("Cache_GetSelectEscolas_VisaoIndividual_{0}_{1}_{2}", doc_id, ent_id, vinculoColaboradorCargo);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de escolas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaEscolasControladasPorUASuperior(Guid uad_idSuperior, Guid ent_id, Guid gru_id, Guid usu_id, byte uni_situacao, bool esc_controleSistema, bool buscarTerceirizadas)
        {
            return string.Format("Cache_SelecionaEscolasControladasPorUASuperior_{0}_{1}_{2}_{3}_{4}_{5}_{6}", uad_idSuperior, ent_id, gru_id, usu_id, uni_situacao, esc_controleSistema, buscarTerceirizadas);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de escolas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelectByUASuperiorSituacao(Guid uad_idSuperior, Guid ent_id, Guid gru_id, Guid usu_id, byte uni_situacao, bool buscarTerceirizadas)
        {
            return string.Format("Cache_GetSelectByUASuperiorSituacao_{0}_{1}_{2}_{3}_{4}_{5}", uad_idSuperior, ent_id, gru_id, usu_id, uni_situacao, buscarTerceirizadas);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de escolas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaEscolasControladasPorUASuperiorPermissaoTotal(Guid uad_idSuperior, Guid ent_id, bool situacao_Desativado, Nullable<bool> esc_controleSistema, byte esc_situacao)
        {
            return string.Format("Cache_SelecionaEscolasControladasPorUASuperiorPermissaoTotal_{0}_{1}_{2}_{3}_{4}", uad_idSuperior, ent_id, situacao_Desativado, esc_controleSistema.HasValue ? esc_controleSistema.ToString() : "", esc_situacao);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de escolas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelectBy_Pesquisa_PermissaoUsuario_Cache(Guid tua_id, Guid ent_id, Guid gru_id, Guid usu_id, short uad_situacao, Guid uad_id)
        {
            return string.Format("Cache_GetSelectBy_Pesquisa_PermissaoUsuario_Cache_{0}_{1}_{2}_{3}_{4}_{5}", tua_id, ent_id, gru_id, usu_id, uad_situacao, uad_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de escolas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelectBy_PesquisaTodos_Cache(Guid tua_id, Guid ent_id)
        {
            return string.Format("Cache_GetSelectBy_PesquisaTodos_Cache_{0}_{1}", tua_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de escolas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelectBySemAcesso(Guid uad_idSuperior, int cal_id, int tpc_id, Guid ent_id, Guid gru_id, Guid usu_id, byte uni_situacao)
        {
            return string.Format("Cache_GetSelectBySemAcesso_{0}_{1}_{2}_{3}_{4}_{5}_{6}", uad_idSuperior, cal_id, tpc_id, ent_id, gru_id, usu_id, uni_situacao);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de escolas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelectBySemAcessoPermissaoTotal(Guid uad_idSuperior, int cal_id, int tpc_id, Guid ent_id, bool situacao_Desativado, byte esc_situacao)
        {
            return string.Format("Cache_GetSelectBySemAcessoPermissaoTotal_{0}_{1}_{2}_{3}_{4}_{5}", uad_idSuperior, cal_id, tpc_id, ent_id, situacao_Desativado, esc_situacao);
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola,
        /// não considerando as permissões do usuário, e de acordo com o parametro esc_controleSistema, que definirá se as escolas controladas
        /// pelo sistema serã retornadas ou não, ou será indiferente a utilização desse parametro.
        /// Além disso, só exibe as escolas que possuem o curso informado ou equivalente
        /// </summary>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="situacao_Desativado">
        /// flag que determina que serão carregadas apenas
        /// escolas com as situações 1 – Ativo ou 5 – Em ativação
        /// descartando a situação 4 – Desativado
        /// </param>
        /// <param name="esc_controleSistema">parametro que definira se as escolas controladas pelo sistema serão retornadas ou não</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaEscolasControladasPorCursoPeriodo_PermissaoTotal(int cur_id, int crr_id, int crp_id, Guid ent_id, bool situacao_Desativado, bool esc_controleSistema)
        {
            ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
            return dao.SelecionaPorCursoPeriodo_PermissaoTotal(cur_id, crr_id, crp_id, ent_id, situacao_Desativado, esc_controleSistema);
        }

        /// <summary>
        /// Retorna as escolas filtradas pelo uad_idSuperior, de acordo com a permissão da pessoa e também de acordo com
        /// o parametro esc_controleSistema, que definirá se as escolas controladas
        /// pelo sistema serã retornadas ou não, ou será indiferente a utilização desse parametro.
        /// </summary>
        /// <param name="uad_idSuperior">ID da unidade superior - Obrigatório</param>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="gru_id">Grupo do usuário logado - Obrigatório</param>
        /// <param name="usu_id">ID do usuário logado - Obrigatório</param>
        /// <param name="esc_controleSistema">parametro que definira se as escolas controladas pelo sistema serão retornadas ou não</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> SelecionaEscolasControladasPorUASuperior(Guid uad_idSuperior, Guid ent_id, Guid gru_id, Guid usu_id, bool esc_controleSistema, int appMinutosCacheLongo = 0, bool buscarTerceirizadas = true)
        {
            return SelecionaEscolasControladasPorUASuperior(uad_idSuperior, ent_id, gru_id, usu_id, 0, esc_controleSistema, appMinutosCacheLongo, buscarTerceirizadas);
        }

        /// <summary>
        /// Retorna as escolas filtradas pelo uad_idSuperior, de acordo com a permissão da pessoa e também de acordo com
        /// o parametro esc_controleSistema, que definirá se as escolas controladas
        /// pelo sistema serã retornadas ou não, ou será indiferente a utilização desse parametro.
        /// </summary>
        /// <param name="uad_idSuperior">ID da unidade superior - Obrigatório</param>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="gru_id">Grupo do usuário logado - Obrigatório</param>
        /// <param name="usu_id">ID do usuário logado - Obrigatório</param>
        /// <param name="uni_situacao">situaçao da unidade escola, caso seja passado o valor 0 a situação será desconsiderada</param>
        /// <param name="esc_controleSistema">parametro que definira se as escolas controladas pelo sistema serão retornadas ou não</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> SelecionaEscolasControladasPorUASuperior(Guid uad_idSuperior, Guid ent_id, Guid gru_id, Guid usu_id, byte uni_situacao, bool esc_controleSistema, int appMinutosCacheLongo = 0, bool buscarTerceirizadas = true)
        {
            List<sComboUAEscola> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaEscolasControladasPorUASuperior(uad_idSuperior, ent_id, gru_id, usu_id, uni_situacao, esc_controleSistema, buscarTerceirizadas);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                        bool ordenarEscolasPorCodigo = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);
                        DataTable dtDados = dao.SelectBy_uad_Superior(uad_idSuperior, ent_id, gru_id, usu_id, uni_situacao, esc_controleSistema, ordenarEscolasPorCodigo, buscarTerceirizadas);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboUAEscola
                                 {
                                     uad_id = string.IsNullOrEmpty(dr["uad_id"].ToString()) ? new Guid() : new Guid(dr["uad_id"].ToString()),
                                     uad_idSuperior = string.IsNullOrEmpty(dr["uad_idSuperior"].ToString()) ? new Guid() : new Guid(dr["uad_idSuperior"].ToString()),
                                     uad_nome = dr["uad_nome"].ToString(),
                                     esc_id = string.IsNullOrEmpty(dr["esc_id"].ToString()) ? 0 : Convert.ToInt32(dr["esc_id"]),
                                     uni_id = string.IsNullOrEmpty(dr["uni_id"].ToString()) ? 0 : Convert.ToInt32(dr["uni_id"]),
                                     esc_uni_nome = dr["esc_uni_nome"].ToString(),
                                     esc_uni_id = dr["esc_uni_id"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboUAEscola>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                bool ordenarEscolasPorCodigo = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);
                DataTable dtDados = dao.SelectBy_uad_Superior(uad_idSuperior, ent_id, gru_id, usu_id, uni_situacao, esc_controleSistema, ordenarEscolasPorCodigo, buscarTerceirizadas);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboUAEscola
                         {
                             uad_id = string.IsNullOrEmpty(dr["uad_id"].ToString()) ? new Guid() : new Guid(dr["uad_id"].ToString()),
                             uad_idSuperior = string.IsNullOrEmpty(dr["uad_idSuperior"].ToString()) ? new Guid() : new Guid(dr["uad_idSuperior"].ToString()),
                             uad_nome = dr["uad_nome"].ToString(),
                             esc_id = string.IsNullOrEmpty(dr["esc_id"].ToString()) ? 0 : Convert.ToInt32(dr["esc_id"]),
                             uni_id = string.IsNullOrEmpty(dr["uni_id"].ToString()) ? 0 : Convert.ToInt32(dr["uni_id"]),
                             esc_uni_nome = dr["esc_uni_nome"].ToString(),
                             esc_uni_id = dr["esc_uni_id"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna as escolas filtradas pelo uad_idSuperior, nã considerando a permissão do usuário e também de acordo com
        /// o parametro esc_controleSistema, que definirá se as escolas controladas
        /// pelo sistema serã retornadas ou não, ou será indiferente a utilização desse parametro.
        /// </summary>
        /// <param name="uad_idSuperior">ID da unidade superior - Obrigatório</param>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="esc_controleSistema">parametro que definira se as escolas controladas pelo sistema serão retornadas ou não</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> SelecionaEscolasControladasPorUASuperiorPermissaoTotal(Guid uad_idSuperior, Guid ent_id, bool esc_controleSistema, int appMinutosCacheLongo = 0)
        {
            return SelecionaEscolasControladasPorUASuperiorPermissaoTotal(uad_idSuperior, ent_id, false, esc_controleSistema, 0, appMinutosCacheLongo);
        }

        /// <summary>
        /// Retorna as escolas filtradas pelo uad_idSuperior, nã considerando a permissão do usuário e também de acordo com
        /// o parametro esc_controleSistema, que definirá se as escolas controladas
        /// pelo sistema serã retornadas ou não, ou será indiferente a utilização desse parametro.
        /// </summary>
        /// <param name="uad_idSuperior">ID da unidade superior - Obrigatório</param>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="situacao_Desativado">
        /// flag que determina que serão carregadas apenas
        /// escolas com as situações 1 – Ativo ou 5 – Em ativação
        /// descartando a situação 4 – Desativado
        /// </param>
        /// <param name="esc_controleSistema">parametro que definira se as escolas controladas pelo sistema serão retornadas ou não</param>
        /// <param name="esc_situacao"></param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> SelecionaEscolasControladasPorUASuperiorPermissaoTotal(Guid uad_idSuperior, Guid ent_id, bool situacao_Desativado, Nullable<bool> esc_controleSistema, byte esc_situacao, int appMinutosCacheLongo = 0)
        {

            List<sComboUAEscola> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaEscolasControladasPorUASuperiorPermissaoTotal(uad_idSuperior, ent_id, situacao_Desativado, esc_controleSistema, esc_situacao);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                        bool ordenarEscolasPorCodigo = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);
                        DataTable dtDados = dao.SelectBy_uad_SuperiorPermissaoTotal(uad_idSuperior, ent_id, situacao_Desativado, esc_controleSistema, esc_situacao);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboUAEscola
                                 {
                                     uad_id = string.IsNullOrEmpty(dr["uad_id"].ToString()) ? new Guid() : new Guid(dr["uad_id"].ToString()),
                                     uad_idSuperior = string.IsNullOrEmpty(dr["uad_idSuperior"].ToString()) ? new Guid() : new Guid(dr["uad_idSuperior"].ToString()),
                                     uad_nome = dr["uad_nome"].ToString(),
                                     esc_id = string.IsNullOrEmpty(dr["esc_id"].ToString()) ? 0 : Convert.ToInt32(dr["esc_id"]),
                                     uni_id = string.IsNullOrEmpty(dr["uni_id"].ToString()) ? 0 : Convert.ToInt32(dr["uni_id"]),
                                     esc_uni_nome = dr["esc_uni_nome"].ToString(),
                                     esc_uni_id = dr["esc_uni_id"].ToString(),
                                     uni_escolaNome = dr["uni_escolaNome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboUAEscola>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                bool ordenarEscolasPorCodigo = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);
                DataTable dtDados = dao.SelectBy_uad_SuperiorPermissaoTotal(uad_idSuperior, ent_id, situacao_Desativado, esc_controleSistema, esc_situacao);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboUAEscola
                         {
                             uad_id = string.IsNullOrEmpty(dr["uad_id"].ToString()) ? new Guid() : new Guid(dr["uad_id"].ToString()),
                             uad_idSuperior = string.IsNullOrEmpty(dr["uad_idSuperior"].ToString()) ? new Guid() : new Guid(dr["uad_idSuperior"].ToString()),
                             uad_nome = dr["uad_nome"].ToString(),
                             esc_id = string.IsNullOrEmpty(dr["esc_id"].ToString()) ? 0 : Convert.ToInt32(dr["esc_id"]),
                             uni_id = string.IsNullOrEmpty(dr["uni_id"].ToString()) ? 0 : Convert.ToInt32(dr["uni_id"]),
                             esc_uni_nome = dr["esc_uni_nome"].ToString(),
                             esc_uni_id = dr["esc_uni_id"].ToString(),
                             uni_escolaNome = dr["uni_escolaNome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna as escolas filtradas pelo uad_idSuperior, curso e período, não considerando a permissão do usuário e também de acordo com
        /// o parametro esc_controleSistema, que definirá se as escolas controladas
        /// pelo sistema serã retornadas ou não, ou será indiferente a utilização desse parametro.
        /// </summary>
        /// <param name="uad_idSuperior">ID da unidade superior - Obrigatório</param>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="situacao_Desativado">
        /// flag que determina que serão carregadas apenas
        /// escolas com as situações 1 – Ativo ou 5 – Em ativação
        /// descartando a situação 4 – Desativado
        /// </param>
        /// <param name="esc_controleSistema">parametro que definira se as escolas controladas pelo sistema serão retornadas ou não</param>
        /// <param name="esc_situacao"></param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaEscolasControladasPorUASuperiorCursoPeriodo_PermissaoTotal(Guid uad_idSuperior, int cur_id, int crr_id, int crp_id, Guid ent_id, bool situacao_Desativado, bool esc_controleSistema, byte esc_situacao = 0)
        {
            ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
            return dao.SelecionaPorUASuperiorCursoPeriodo_PermissaoTotal(uad_idSuperior, cur_id, crr_id, crp_id, ent_id, situacao_Desativado, esc_controleSistema, esc_situacao);
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola,
        /// não considerando as permissões do usuário.
        /// Utilizado na tela de movimentação.
        /// </summary>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> GetSelectBy_Pesquisa_PermissaoUsuario_Cache
        (
            Guid tua_id
            , Guid ent_id
            , Guid gru_id
            , Guid usu_id
            , short uad_situacao
            , Guid uad_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboUAEscola> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelectBy_Pesquisa_PermissaoUsuario_Cache(tua_id, ent_id, gru_id, usu_id, uad_situacao, uad_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        DataTable dtDados = MSTech.CoreSSO.BLL.SYS_UnidadeAdministrativaBO.GetSelectBy_Pesquisa_PermissaoUsuario(tua_id, ent_id, gru_id, usu_id, uad_situacao, uad_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboUAEscola
                                 {
                                     uad_id = string.IsNullOrEmpty(dr["uad_id"].ToString()) ? new Guid() : new Guid(dr["uad_id"].ToString()),
                                     uad_nome = dr["uad_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboUAEscola>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                DataTable dtDados = MSTech.CoreSSO.BLL.SYS_UnidadeAdministrativaBO.GetSelectBy_Pesquisa_PermissaoUsuario(tua_id, ent_id, gru_id, usu_id, uad_situacao, uad_id);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboUAEscola
                         {
                             uad_id = string.IsNullOrEmpty(dr["uad_id"].ToString()) ? new Guid() : new Guid(dr["uad_id"].ToString()),
                             uad_nome = dr["uad_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola,
        /// não considerando as permissões do usuário.
        /// Utilizado na tela de movimentação.
        /// </summary>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> GetSelectBy_PesquisaTodos_Cache
        (
            Guid tua_id
            , Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboUAEscola> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelectBy_PesquisaTodos_Cache(tua_id, ent_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        DataTable dtDados = MSTech.CoreSSO.BLL.SYS_UnidadeAdministrativaBO.GetSelectBy_PesquisaTodos(tua_id, ent_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboUAEscola
                                 {
                                     uad_id = string.IsNullOrEmpty(dr["uad_id"].ToString()) ? new Guid() : new Guid(dr["uad_id"].ToString()),
                                     uad_nome = dr["uad_nome"].ToString()
                                 }).ToList();                     

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboUAEscola>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                DataTable dtDados = MSTech.CoreSSO.BLL.SYS_UnidadeAdministrativaBO.GetSelectBy_PesquisaTodos(tua_id, ent_id);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboUAEscola
                         {
                             uad_id = string.IsNullOrEmpty(dr["uad_id"].ToString()) ? new Guid() : new Guid(dr["uad_id"].ToString()),
                             uad_nome = dr["uad_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        #endregion Métodos

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola - paginado.
        /// </summary>
        /// <param name="esc_id">ID de escola</param>
        /// <param name="uni_id">ID de unidade escola</param>
        /// <param name="uni_situacao">Situacao de unidade escola</param>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="usu_id">ID do usuário logado - Obrigatório</param>
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <param name="gru_id">Grupo do usuário logado - Obrigatório</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int esc_id
            , int uni_id
            , Byte uni_situacao
            , Guid ent_id
            , Guid gru_id
            , Guid usu_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
            return dao.SelecionaPorEscolaEntidadeUnidadeSituacaoPemissaoUsuario
                (esc_id, uni_id, uni_situacao, ent_id, gru_id, usu_id, paginado, currentPage / pageSize, pageSize, out totalRecords
                , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id));
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola - paginado.
        /// </summary>
        /// <param name="cur_id">ID de curso</param>
        /// <param name="uad_idSuperior">ID da unidade administrativa superior</param>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="usu_id">ID do usuário logado - Obrigatório</param>
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <param name="gru_id">Grupo do usuário logado - Obrigatório</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int cur_id
            , Guid uad_idSuperior
            , Guid ent_id
            , Guid gru_id
            , Guid usu_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
            return dao.SelectBy_cur_id(cur_id, uad_idSuperior, ent_id, gru_id, usu_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola.
        /// </summary>
        /// <param name="esc_id">ID de escola</param>
        /// <param name="uni_id">ID de unidade escola</param>
        /// <param name="uni_situacao">Situacao de unidade escola</param>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="usu_id">ID do usuário logado - Obrigatório</param>
        /// <param name="gru_id">Grupo do usuário logado - Obrigatório</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            Int32 esc_id
            , Int32 uni_id
            , Byte uni_situacao
            , Guid ent_id
            , Guid gru_id
            , Guid usu_id
            , bool buscarTerceirizadas = true
            , bool esc_controleSistema = false

        )
        {
            ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
            return dao.SelecionaPorEscolaEntidadeUnidadeSituacaoPemissaoUsuario
                (esc_id, uni_id, uni_situacao, ent_id, gru_id, usu_id, false, 0, 1, out totalRecords
                , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id), buscarTerceirizadas, esc_controleSistema);
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola.
        /// </summary>
        /// <param name="esc_id">ID de escola</param>
        /// <param name="uni_id">ID de unidade escola</param>
        /// <param name="uni_situacao">Situacao de unidade escola</param>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="usu_id">ID do usuário logado - Obrigatório</param>
        /// <param name="gru_id">Grupo do usuário logado - Obrigatório</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> GetSelect_Cache
        (
            Int32 esc_id
            , Int32 uni_id
            , Byte uni_situacao
            , Guid ent_id
            , Guid gru_id
            , Guid usu_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboUAEscola> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_Select_Cache(esc_id, uni_id, uni_situacao, ent_id, gru_id, usu_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                        DataTable dtDados = dao.SelecionaPorEscolaEntidadeUnidadeSituacaoPemissaoUsuario
                                            (esc_id, uni_id, uni_situacao, ent_id, gru_id, usu_id, false, 0, 1, out totalRecords
                                            , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id));
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboUAEscola
                                 {
                                     esc_id = string.IsNullOrEmpty(dr["esc_id"].ToString()) ? 0 : Convert.ToInt32(dr["esc_id"]),
                                     uni_id = string.IsNullOrEmpty(dr["uni_id"].ToString()) ? 0 : Convert.ToInt32(dr["uni_id"]),
                                     uni_escolaNome = dr["uni_escolaNome"].ToString(),
                                     esc_uni_nome = dr["esc_uni_nome"].ToString(),
                                     esc_uni_id = dr["esc_uni_id"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboUAEscola>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                DataTable dtDados = dao.SelecionaPorEscolaEntidadeUnidadeSituacaoPemissaoUsuario
                                    (esc_id, uni_id, uni_situacao, ent_id, gru_id, usu_id, false, 0, 1, out totalRecords
                                    , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id));
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboUAEscola
                         {
                             esc_id = string.IsNullOrEmpty(dr["esc_id"].ToString()) ? 0 : Convert.ToInt32(dr["esc_id"]),
                             uni_id = string.IsNullOrEmpty(dr["uni_id"].ToString()) ? 0 : Convert.ToInt32(dr["uni_id"]),
                             uni_escolaNome = dr["uni_escolaNome"].ToString(),
                             esc_uni_nome = dr["esc_uni_nome"].ToString(),
                             esc_uni_id = dr["esc_uni_id"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola,
        /// não considerando as permissões do usuário.
        /// Utilizado na tela de movimentação.
        /// </summary>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> GetSelectPermissaoTotal_Cache
        (
            Guid ent_id
            , bool situacao_Desativado
            , Nullable<bool> esc_controleSistema
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboUAEscola> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelectPermissaoTotal_Cache(ent_id, situacao_Desativado, esc_controleSistema);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                        DataTable dtDados = dao.SelectBy_All_PermissaoTotal(ent_id, situacao_Desativado, esc_controleSistema);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboUAEscola
                                 {
                                     esc_id = string.IsNullOrEmpty(dr["esc_id"].ToString()) ? 0 : Convert.ToInt32(dr["esc_id"]),
                                     uni_id = string.IsNullOrEmpty(dr["uni_id"].ToString()) ? 0 : Convert.ToInt32(dr["uni_id"]),
                                     esc_uni_nome = dr["esc_uni_nome"].ToString(),
                                     uni_escolaNome = dr["uni_escolaNome"].ToString(),
                                     esc_uni_id = dr["esc_uni_id"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboUAEscola>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                DataTable dtDados = dao.SelectBy_All_PermissaoTotal(ent_id, situacao_Desativado, esc_controleSistema);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboUAEscola
                         {
                             esc_id = string.IsNullOrEmpty(dr["esc_id"].ToString()) ? 0 : Convert.ToInt32(dr["esc_id"]),
                             uni_id = string.IsNullOrEmpty(dr["uni_id"].ToString()) ? 0 : Convert.ToInt32(dr["uni_id"]),
                             esc_uni_nome = dr["esc_uni_nome"].ToString(),
                             uni_escolaNome = dr["uni_escolaNome"].ToString(),
                             esc_uni_id = dr["esc_uni_id"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola
        /// a partir do código do docente e entidade. Usado na visão individual
        /// Utilizado nas telas: Documentos do docente, Atribuição do docente, Compensação de Ausências. (Visão individual)
        /// </summary>
        /// <param name="doc_id">Código do docente</param>
        /// <param name="ent_id">Código da entidade</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> GetSelectEscolas_VisaoIndividual
        (
            Int64 doc_id,
            Guid ent_id,
            int appMinutosCacheLongo = 0
        )
        {
            return GetSelectEscolas_VisaoIndividual(doc_id, ent_id, 0, appMinutosCacheLongo);
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola
        /// a partir do código do aluno e entidade. Usado na visão individual
        /// Utilizado nas telas: Mensagens.
        /// </summary>
        /// <param name="doc_id">Código do aluno</param>
        /// <param name="ent_id">Código da entidade</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> GetSelectEscolas_VisaoAluno
        (
            Int64 alu_id,
            Guid ent_id,
            int appMinutosCacheLongo = 0
        )
        {
            List<sComboUAEscola> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelectEscolas_VisaoAluno(alu_id, ent_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                        DataTable dtDados = dao.SelectEscolas_VisaoAluno(alu_id, ent_id
                                            , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id));
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboUAEscola
                                 {
                                     uni_escolaNome = dr["uni_escolaNome"].ToString(),
                                     esc_uni_id = dr["esc_uni_id"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboUAEscola>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                DataTable dtDados = dao.SelectEscolas_VisaoAluno(alu_id, ent_id
                                    , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id));
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboUAEscola
                         {
                             uni_escolaNome = dr["uni_escolaNome"].ToString(),
                             esc_uni_id = dr["esc_uni_id"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola
        /// a partir do código do responsavel e entidade. Usado na visão individual
        /// Utilizado nas telas: Mensagens.
        /// </summary>
        /// <param name="pes_id">Código da pessoa do responsável</param>
        /// <param name="ent_id">Código da entidade</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> GetSelectEscolas_VisaoResponsavel
        (
            Guid pes_id,
            Guid ent_id,
            int appMinutosCacheLongo = 0
        )
        {
            List<sComboUAEscola> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelectEscolas_VisaoResponsavel(pes_id, ent_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                        DataTable dtDados = dao.SelectEscolas_VisaoResponsavel(pes_id, ent_id
                                            , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id));
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboUAEscola
                                 {
                                     uni_escolaNome = dr["uni_escolaNome"].ToString(),
                                     esc_uni_id = dr["esc_uni_id"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboUAEscola>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                DataTable dtDados = dao.SelectEscolas_VisaoResponsavel(pes_id, ent_id
                                    , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id));
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboUAEscola
                         {
                             uni_escolaNome = dr["uni_escolaNome"].ToString(),
                             esc_uni_id = dr["esc_uni_id"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola
        /// a partir do código do docente e entidade. Usado na visão individual
        /// Utilizado nas telas: Documentos do docente, Atribuição do docente, Compensação de Ausências. (Visão individual)
        /// </summary>
        /// <param name="alu_id">Código do docente</param>
        /// <param name="ent_id">Código da entidade</param>
        /// <param name="vinculoColaboradorCargo">0 - Busca as escolas pelas atribuições de turma docente do docente
        ///                                       1 - Busca as escolas do docente pelo vinculo de colaborador cargo
        ///                                       2 - Busca escolas do docente com vigência na escola</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> GetSelectEscolas_VisaoIndividual
        (
            Int64 doc_id,
            Guid ent_id,
            Byte vinculoColaboradorCargo,
            int appMinutosCacheLongo = 0
        )
        {
            List<sComboUAEscola> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelectEscolas_VisaoIndividual(doc_id, ent_id, vinculoColaboradorCargo);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                        DataTable dtDados = dao.SelectEscolas_VisaoIndividual(doc_id, ent_id, vinculoColaboradorCargo
                                            , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id));
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboUAEscola
                                 {
                                     uni_escolaNome = dr["uni_escolaNome"].ToString(),
                                     esc_uni_id = dr["esc_uni_id"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboUAEscola>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                DataTable dtDados = dao.SelectEscolas_VisaoIndividual(doc_id, ent_id, vinculoColaboradorCargo
                                    , ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id));
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboUAEscola
                         {
                             uni_escolaNome = dr["uni_escolaNome"].ToString(),
                             esc_uni_id = dr["esc_uni_id"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna as escolas controladas pelo sistema de acordo com o uad_idSuperior, de acordo com a permissão da pessoa.
        /// </summary>
        /// <param name="uad_idSuperior"></param>
        /// <param name="ent_id"></param>
        /// <param name="gru_id"></param>
        /// <param name="usu_id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> GetSelectByUASuperior
        (
            Guid uad_idSuperior
            , Guid ent_id
            , Guid gru_id
            , Guid usu_id
            , int appMinutosCacheLongo = 0
            , bool buscarTerceirizadas = true
        )
        {
            return GetSelectByUASuperiorSituacao(uad_idSuperior, ent_id, gru_id, usu_id, 0, appMinutosCacheLongo, buscarTerceirizadas);
        }

        /// <summary>
        /// Retorna as escolas pelo uad_idSuperior, de acordo com a permissão da pessoa.
        /// </summary>
        /// <param name="uad_idSuperior">The uad_id superior.</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <param name="usu_id">Id do usuário</param>
        /// <param name="uni_situacao">Situação da unidade da escola para filtrar</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> GetSelectByUASuperiorSituacao
        (
            Guid uad_idSuperior
            , Guid ent_id
            , Guid gru_id
            , Guid usu_id
            , byte uni_situacao
            , int appMinutosCacheLongo = 0
            , bool buscarTerceirizadas = true
        )
        {
            List<sComboUAEscola> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelectByUASuperiorSituacao(uad_idSuperior, ent_id, gru_id, usu_id, uni_situacao, buscarTerceirizadas);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                        bool ordenarEscolasPorCodigo = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);
                        DataTable dtDados = dao.SelectBy_uad_Superior(uad_idSuperior, ent_id, gru_id, usu_id, uni_situacao, null, ordenarEscolasPorCodigo, buscarTerceirizadas);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboUAEscola
                                 {
                                     uad_id = string.IsNullOrEmpty(dr["uad_id"].ToString()) ? new Guid() : new Guid(dr["uad_id"].ToString()),
                                     uad_idSuperior = string.IsNullOrEmpty(dr["uad_idSuperior"].ToString()) ? new Guid() : new Guid(dr["uad_idSuperior"].ToString()),
                                     uad_nome = dr["uad_nome"].ToString(),
                                     esc_id = string.IsNullOrEmpty(dr["esc_id"].ToString()) ? 0 : Convert.ToInt32(dr["esc_id"]),
                                     uni_id = string.IsNullOrEmpty(dr["uni_id"].ToString()) ? 0 : Convert.ToInt32(dr["uni_id"]),
                                     esc_uni_nome = dr["esc_uni_nome"].ToString(),
                                     esc_uni_id = dr["esc_uni_id"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboUAEscola>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                bool ordenarEscolasPorCodigo = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);
                DataTable dtDados = dao.SelectBy_uad_Superior(uad_idSuperior, ent_id, gru_id, usu_id, uni_situacao, null, ordenarEscolasPorCodigo, buscarTerceirizadas);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboUAEscola
                         {
                             uad_id = string.IsNullOrEmpty(dr["uad_id"].ToString()) ? new Guid() : new Guid(dr["uad_id"].ToString()),
                             uad_idSuperior = string.IsNullOrEmpty(dr["uad_idSuperior"].ToString()) ? new Guid() : new Guid(dr["uad_idSuperior"].ToString()),
                             uad_nome = dr["uad_nome"].ToString(),
                             esc_id = string.IsNullOrEmpty(dr["esc_id"].ToString()) ? 0 : Convert.ToInt32(dr["esc_id"]),
                             uni_id = string.IsNullOrEmpty(dr["uni_id"].ToString()) ? 0 : Convert.ToInt32(dr["uni_id"]),
                             esc_uni_nome = dr["esc_uni_nome"].ToString(),
                             esc_uni_id = dr["esc_uni_id"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna as escolas ativas pelo uad_idSuperior, de acordo com a permissão da pessoa.
        /// </summary>
        /// <param name="uad_idSuperior"></param>
        /// <param name="ent_id"></param>
        /// <param name="gru_id"></param>
        /// <param name="usu_id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> GetSelectAtivosByUASuperior(Guid uad_idSuperior, Guid ent_id, Guid gru_id, Guid usu_id)
        {
            ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
            DataTable dtDados = dao.SelectBy_uad_Superior(uad_idSuperior, ent_id, gru_id, usu_id, 1, null, false);
            return (from DataRow dr in dtDados.Rows
                     select new sComboUAEscola
                     {
                         uad_id = string.IsNullOrEmpty(dr["uad_id"].ToString()) ? new Guid() : new Guid(dr["uad_id"].ToString()),
                         uad_idSuperior = string.IsNullOrEmpty(dr["uad_idSuperior"].ToString()) ? new Guid() : new Guid(dr["uad_idSuperior"].ToString()),
                         uad_nome = dr["uad_nome"].ToString(),
                         esc_id = string.IsNullOrEmpty(dr["esc_id"].ToString()) ? 0 : Convert.ToInt32(dr["esc_id"]),
                         uni_id = string.IsNullOrEmpty(dr["uni_id"].ToString()) ? 0 : Convert.ToInt32(dr["uni_id"]),
                         esc_uni_nome = dr["esc_uni_nome"].ToString(),
                         esc_uni_id = dr["esc_uni_id"].ToString()
                     }).ToList();
        }

        /// <summary>
        /// Retorna as escolas pelo uad_idSuperior, não considerando as permissões do usuário.
        /// Utilizado na tela de movimentação.
        /// </summary>
        /// <param name="uad_idSuperior"></param>
        /// <param name="ent_id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectByUASuperiorPermissaoTotal
        (
            Guid uad_idSuperior
            , Guid ent_id
        )
        {
            ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
            return dao.SelectBy_uad_SuperiorPermissaoTotal(uad_idSuperior, ent_id, false, null, 0);
        }

        /// <summary>
        /// Retorna as escolas pelo uad_idSuperior, não considerando as permissões do usuário.
        /// Utilizado na tela de movimentação.
        /// </summary>
        /// <param name="uad_idSuperior"></param>
        /// <param name="ent_id"></param>
        /// <param name="situacao_Desativado">
        /// flag que determina que serão carregadas apenas
        /// escolas com as situações 1 – Ativo ou 5 – Em ativação
        /// descartando a situação 4 – Desativado
        /// </param>
        /// <param name="esc_situacao"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> GetSelectByUASuperiorPermissaoTotal
        (
            Guid uad_idSuperior
            , Guid ent_id
            , bool situacao_Desativado
            , byte esc_situacao
            , int appMinutosCacheLongo = 0
        )
        {
            return SelecionaEscolasControladasPorUASuperiorPermissaoTotal(uad_idSuperior, ent_id, situacao_Desativado, null, esc_situacao, appMinutosCacheLongo);
        }

        /// <summary>
        /// Retorna as escolas sem acesso
        /// </summary>
        /// <param name="uad_idSuperior">The uad_id superior.</param>
        /// <param name="cal_id">Calendario</param>
        /// <param name="tpc_id">Periodo</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <param name="usu_id">Id do usuário</param>
        /// <param name="uni_situacao">Situação da unidade da escola para filtrar</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> GetSelectBySemAcessoPermissaoTotal
        (
            Guid uad_idSuperior
            , int cal_id
            , int tpc_id
            , Guid ent_id
            , bool situacao_Desativado
            , byte esc_situacao = 0
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboUAEscola> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelectBySemAcessoPermissaoTotal(uad_idSuperior, cal_id, tpc_id, ent_id, situacao_Desativado, esc_situacao);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                        DataTable dtDados = dao.SelectBy_SemAcesso_PermissaoTotal(uad_idSuperior, cal_id, tpc_id, ent_id, situacao_Desativado, null, esc_situacao);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboUAEscola
                                 {
                                     uad_id = string.IsNullOrEmpty(dr["uad_id"].ToString()) ? new Guid() : new Guid(dr["uad_id"].ToString()),
                                     uad_idSuperior = string.IsNullOrEmpty(dr["uad_idSuperior"].ToString()) ? new Guid() : new Guid(dr["uad_idSuperior"].ToString()),
                                     uad_nome = dr["uad_nome"].ToString(),
                                     esc_id = string.IsNullOrEmpty(dr["esc_id"].ToString()) ? 0 : Convert.ToInt32(dr["esc_id"]),
                                     uni_id = string.IsNullOrEmpty(dr["uni_id"].ToString()) ? 0 : Convert.ToInt32(dr["uni_id"]),
                                     esc_uni_nome = dr["esc_uni_nome"].ToString(),
                                     esc_uni_id = dr["esc_uni_id"].ToString(),
                                     uni_escolaNome = dr["uni_escolaNome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboUAEscola>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                DataTable dtDados = dao.SelectBy_SemAcesso_PermissaoTotal(uad_idSuperior, cal_id, tpc_id, ent_id, situacao_Desativado, null, esc_situacao);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboUAEscola
                         {
                             uad_id = string.IsNullOrEmpty(dr["uad_id"].ToString()) ? new Guid() : new Guid(dr["uad_id"].ToString()),
                             uad_idSuperior = string.IsNullOrEmpty(dr["uad_idSuperior"].ToString()) ? new Guid() : new Guid(dr["uad_idSuperior"].ToString()),
                             uad_nome = dr["uad_nome"].ToString(),
                             esc_id = string.IsNullOrEmpty(dr["esc_id"].ToString()) ? 0 : Convert.ToInt32(dr["esc_id"]),
                             uni_id = string.IsNullOrEmpty(dr["uni_id"].ToString()) ? 0 : Convert.ToInt32(dr["uni_id"]),
                             esc_uni_nome = dr["esc_uni_nome"].ToString(),
                             esc_uni_id = dr["esc_uni_id"].ToString(),
                             uni_escolaNome = dr["uni_escolaNome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna as escolas sem acesso, de acordo com a permissão da pessoa.
        /// </summary>
        /// <param name="uad_idSuperior">The uad_id superior.</param>
        /// <param name="cal_id">Calendario</param>
        /// <param name="tpc_id">Periodo</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <param name="usu_id">Id do usuário</param>
        /// <param name="uni_situacao">Situação da unidade da escola para filtrar</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboUAEscola> GetSelectBySemAcesso
        (
            Guid uad_idSuperior
            , int cal_id
            , int tpc_id
            , Guid ent_id
            , Guid gru_id
            , Guid usu_id
            , byte uni_situacao
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboUAEscola> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelectBySemAcesso(uad_idSuperior, cal_id, tpc_id, ent_id, gru_id, usu_id, uni_situacao);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                        bool ordenarEscolasPorCodigo = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);
                        DataTable dtDados = dao.SelectBy_SemAcesso(uad_idSuperior, cal_id, tpc_id, ent_id, gru_id, usu_id, uni_situacao, null, ordenarEscolasPorCodigo);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboUAEscola
                                 {
                                     uad_id = string.IsNullOrEmpty(dr["uad_id"].ToString()) ? new Guid() : new Guid(dr["uad_id"].ToString()),
                                     uad_idSuperior = string.IsNullOrEmpty(dr["uad_idSuperior"].ToString()) ? new Guid() : new Guid(dr["uad_idSuperior"].ToString()),
                                     uad_nome = dr["uad_nome"].ToString(),
                                     esc_id = string.IsNullOrEmpty(dr["esc_id"].ToString()) ? 0 : Convert.ToInt32(dr["esc_id"]),
                                     uni_id = string.IsNullOrEmpty(dr["uni_id"].ToString()) ? 0 : Convert.ToInt32(dr["uni_id"]),
                                     esc_uni_nome = dr["esc_uni_nome"].ToString(),
                                     esc_uni_id = dr["esc_uni_id"].ToString(),
                                     uni_escolaNome = dr["uni_escolaNome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboUAEscola>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                bool ordenarEscolasPorCodigo = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);
                DataTable dtDados = dao.SelectBy_SemAcesso(uad_idSuperior, cal_id, tpc_id, ent_id, gru_id, usu_id, uni_situacao, null, ordenarEscolasPorCodigo);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboUAEscola
                         {
                             uad_id = string.IsNullOrEmpty(dr["uad_id"].ToString()) ? new Guid() : new Guid(dr["uad_id"].ToString()),
                             uad_idSuperior = string.IsNullOrEmpty(dr["uad_idSuperior"].ToString()) ? new Guid() : new Guid(dr["uad_idSuperior"].ToString()),
                             uad_nome = dr["uad_nome"].ToString(),
                             esc_id = string.IsNullOrEmpty(dr["esc_id"].ToString()) ? 0 : Convert.ToInt32(dr["esc_id"]),
                             uni_id = string.IsNullOrEmpty(dr["uni_id"].ToString()) ? 0 : Convert.ToInt32(dr["uni_id"]),
                             esc_uni_nome = dr["esc_uni_nome"].ToString(),
                             esc_uni_id = dr["esc_uni_id"].ToString(),
                             uni_escolaNome = dr["uni_escolaNome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola
        /// que não foram excluídas logicamente, filtradas por
        ///	cur_id, crr_id, crp_id
        /// </summary>
        /// <param name="cur_id">ID do Curso</param>
        /// <param name="crr_id">ID do Curriculo</param>
        /// <param name="crp_id">ID do Curriculo Periodo</param>
        /// <param name="ent_id">ID da Entidade</param>
        /// <param name="uad_id">ID's da Unidade Administrativa dos usuários do grupo Unidade Administrativa e Gestão (Separados por vírgula)</param>
        /// <returns>DataTable com as unidade escola</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectByCursoCurriculoPeriodo
        (
            int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , string uad_id
        )
        {
            try
            {
                ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                return dao.SelectBy_cur_id_crr_id_crp_id(cur_id, crr_id, crp_id, ent_id, uad_id);
            }
            catch
            {
                throw;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectByUASuperiorCursoCurriculoPeriodo
        (
            Guid uad_idSuperior
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , string uad_id
        )
        {
            try
            {
                ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
                return dao.SelectBy_uad_Superior_cur_id_crr_id_crp_id(uad_idSuperior, cur_id, crr_id, crp_id, ent_id, uad_id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Seleciona o id da última unidade cadastrada para a escola + 1
        /// se não houver unidade cadastrada para escola retorna 1
        /// filtrados por esc_id
        /// </summary>
        /// <param name="esc_id">Campo ent_id da tabela ESC_UnidadeEscola do bd</param>
        /// <returns>uni_id + 1</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Int32 VerificaUltimaUnidadeCadastrada
        (
            int esc_id
        )
        {
            ESC_UnidadeEscolaDAO dal = new ESC_UnidadeEscolaDAO();
            try
            {
                return dal.SelectBy_esc_id_top_one(esc_id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Seleciona a unidade administrativa superior de uma unidade de escola.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <returns></returns>
        public static Guid SelecionaUnidadeAdministrativaSuperior(int esc_id, int uni_id)
        {
            return new ESC_UnidadeEscolaDAO().SelecionaUnidadeAdministrativaSuperior(esc_id, uni_id);
        }

        /// <summary>
        /// Inclui um nova unidade para a escola
        /// </summary>
        /// <param name="entityUnidadeEscola">Entidade ESC_UnidadeEscola</param>
        /// <param name="banco"></param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ESC_UnidadeEscola entityUnidadeEscola
            , Data.Common.TalkDBTransaction banco
        )
        {
            if (entityUnidadeEscola.Validate())
            {
                ESC_UnidadeEscolaDAO uadconDAL = new ESC_UnidadeEscolaDAO { _Banco = banco };
                return uadconDAL.Salvar(entityUnidadeEscola);
            }

            throw new Validation.Exceptions.ValidationException(entityUnidadeEscola.PropertiesErrorList[0].Message);
        }

        /// <summary>
        /// Retorna as entidades da UnidadeEscola cadastradas nas escolas.
        /// </summary>
        /// <param name="esc_id">ID das escolas</param>
        /// <returns></returns>
        public static List<ESC_UnidadeEscola> SelecionaPorEscolas
        (
            string esc_id
        )
        {
            ESC_UnidadeEscolaDAO dao = new ESC_UnidadeEscolaDAO();
            DataTable dtDados = dao.SelecionaPorEscolas(esc_id);
            return (from DataRow dr in dtDados.Rows
                    select dao.DataRowToEntity(dr, new ESC_UnidadeEscola())).ToList();
        }

        /// <summary>
        /// Seleciona as escolas em que o tipo de classificação possui o cargo passado por parâmetro.
        /// </summary>
        /// <param name="crg_id">Id do cargo</param>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <param name="gru_id">ID do grupo do usuário logado.</param>
        /// <param name="usu_id">ID do usuário logado.</param>
        /// <param name="adm">Flag que indica se o usuário é administrador.</param>
        /// <returns></returns>
        public static List<sComboUAEscola> SelecionaPorCargoTipoClassificacaoVigente(int crg_id, Guid ent_id, Guid gru_id, Guid usu_id, bool adm, bool trazerTerceridas)
        {
            bool ordenarEscolasPorCodigo = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);
            return (from DataRow dr in new ESC_UnidadeEscolaDAO().SelecionaPorCargoTipoClassificacaoVigente(crg_id, ent_id, gru_id, usu_id, adm, ordenarEscolasPorCodigo, trazerTerceridas).Rows
                    select (sComboUAEscola)GestaoEscolarUtilBO.DataRowToEntity(dr, new sComboUAEscola())).ToList();
        }
    }
}