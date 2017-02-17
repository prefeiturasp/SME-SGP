using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using MSTech.Business.Common;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.DAL;
using MSTech.CoreSSO.Entities;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Web;

namespace MSTech.GestaoEscolar.BLL
{
    public class ESC_EscolaBO : BusinessBase<ESC_EscolaDAO, ESC_Escola>
    {
        #region Enumerador

        /// <summary>
        /// Tipos de valores permitidos para o campo esc_autorizada.
        /// </summary>
        public enum Autorizada : byte
        {
            [StringValue("Não")]
            Nao = 1,

            [StringValue("Sim")]
            Sim = 2,

            [StringValue("Em tramitação")]
            EmTramitacao = 3
        }
        
        /// <summary>
        /// Situações da escola
        /// </summary>
        public enum ESC_EscolaSituacao : byte
        {
            Ativo = 1,
            Excluido = 3,
            Desativado = 4,
            Em_Ativacao = 5
        }

        #endregion Enumerador

        #region Métodos de consulta

        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static ESC_Escola GetEntity(ESC_Escola entity)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_GetEntity(entity);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    new ESC_EscolaDAO().Carregar(entity);
                    // Adiciona cache com validade de 6h.
                    HttpContext.Current.Cache.Insert(chave, entity, null, DateTime.Now.AddMinutes(GestaoEscolarUtilBO.MinutosCacheMedio)
                        , System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    GestaoEscolarUtilBO.CopiarEntity(cache, entity);
                }

                return entity;
            }

            new ESC_EscolaDAO().Carregar(entity);

            return entity;
        }

        /// <summary>
        /// Retorna a entidade sem cache para Alteração.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static ESC_Escola SelecionaEscola(ESC_Escola entity)
        {
            new ESC_EscolaDAO().Carregar(entity);

            return entity;
        }

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(ESC_Escola entity)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                HttpContext.Current.Cache.Remove(RetornaChaveCache_GetEntity(entity));
            }
        }

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(ESC_Escola entity)
        {
            return string.Format("ESC_Escola_GetEntity_{0}", entity.esc_id);
        }

        /// <summary>
        /// Retorna todas as escolas do Gestão Escolar.
        /// </summary>
        /// <returns></returns>
        public static DataTable SelecionaEscola_GestaoIntegracaoRio()
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.SelecionaEscola_GestaoIntegracaoRio();
        }

        /// <summary>
        /// Verifica se há escolas cadastradas com o tua_id passado.
        /// </summary>
        /// <param name="tua_id">Id do tipo de unidade administrativa.</param>
        /// <returns>Verdadeiro se existe pelo menos uma escola com o tipo.</returns>
        public static bool VerificaEscolaComTipo(Guid tua_id)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.VerificaEscolaComTipo(tua_id);
        }

        /// <summary>
        /// Retorna o maior esc_id
        /// </summary>
        /// <param name="qtdDigitoEsc"></param>
        /// <returns>Verdadeiro se existe pelo menos uma escola com o tipo.</returns>
        public static void RetornaMaiorIDEscola(out int qtdDigitoEsc)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            DataTable dt = dao.RetornaMaiorIDEscola();

            qtdDigitoEsc = Convert.ToString(dt.Rows[0]["esc_id"]).Length;
        }

        /// <summary>
        /// Retorna os dados da UA Superior da escola, de acordo com a permissão que o usuário tem (na UA ou na UASuperior).
        /// </summary>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="gru_id">Id do grupo.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <returns></returns>
        public static DataTable Select_UASuperiorBy_Permissao(Guid ent_id, Guid gru_id, Guid usu_id)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.Select_UASuperiorBy_Permissao(ent_id, gru_id, usu_id);
        }

        /// <summary>
        /// Busca escolas pelo nome da escola
        /// </summary>
        /// <param name="esc_nome">Nome da escola.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="gru_id">Id do grupo.</param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable ConsultarPeloNome(string esc_nome, Guid usu_id, Guid ent_id, Guid gru_id, int pageSize, int currentPage)
        {
            totalRecords = 0;
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.ConsultarPeloNome(esc_nome, usu_id, ent_id, gru_id, pageSize, currentPage / pageSize, out totalRecords);
        }

        /// <summary>
        /// Consulta no banco escolas com o mesmo nome, com Id diferente do id passado, dentro
        /// da mesma entidade e carrega a entidade escola.
        /// </summary>
        /// <param name="entity">Entidade escola</param>
        /// <returns>True = se encontrou escola com determinado nome / False = não encontrou></returns>
        public static bool ConsultarNomeExistente(ESC_Escola entity)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.SelectBy_Nome(entity);
        }

        /// <summary>
        /// Método para verificar se já existe o código da escola.
        /// </summary>
        /// <param name="entity">Entidade ESC_Escola</param>
        /// <returns>true = Encontrou código igual | false = Não encontrou código igual</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool ConsultarCodigoExistente(ESC_Escola entity)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.SelectBy_Codigo(entity);
        }

        /// <summary>
        /// Seleciona a escola pelo código para número de matrícula.
        /// </summary>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="esc_codigoNumeroMatricula">Código para número de matrícula.</param>
        /// <returns>Escola.</returns>
        public static ESC_Escola ConsultarCodigoNumeroMatricula(Guid ent_id, int esc_codigoNumeroMatricula)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.ConsultarCodigoNumeroMatricula(ent_id, esc_codigoNumeroMatricula);
        }

        /// <summary>
        /// Método para carregar as informaçoes de uma escola de acordo
        /// com o uad_id informado
        /// </summary>
        /// <param name="entity">Entidade ESC_Escola</param>
        /// <returns>true = Encontrou código igual | false = Não encontrou código igual</returns>
        public static bool ConsultarPorUnidadeAdministrativa(ESC_Escola entity)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.SelectBy_UAD(entity);
        }

        /// <summary>
        /// Consulta se o usuário tem permissão para a escola com o código passado,
        /// e carrega as informações da escola na entidade.
        /// </summary>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="gru_id">Id do grupo usuário.</param>
        /// <param name="entity">Entidade ESC_Escola com o esc_codigo preenchido.</param>
        /// <returns>True - Usuário tem permissão. | False - Usuário não tem permissão.</returns>
        public static bool ConsultarPermissaoUsuarioPeloCodigo
        (
            Guid usu_id,
            Guid ent_id,
            Guid gru_id,
            ESC_Escola entity
        )
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.VerificaPermissaoUsuarioBy_Codigo(usu_id, ent_id, gru_id, entity);
        }

        /// <summary>
        /// Busca as escolas de acordo com os filtros passados.
        /// </summary>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="esc_nome">Nome da escola.</param>
        /// <param name="esc_codigo">Código da escola.</param>
        /// <param name="esc_situacao">Situação da escola.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="TIPO_MEIOCONTATO_TELEFONE"></param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int esc_id,
            string esc_nome,
            string esc_codigo,
            byte esc_situacao,
            Guid ent_id,
            string TIPO_MEIOCONTATO_TELEFONE,
            int cur_id,
            int crr_id,
            bool paginado,
            int currentPage,
            int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
            {
                pageSize = 1;
            }

            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.SelectBy_Pesquisa(esc_id, esc_nome, esc_codigo, esc_situacao, Guid.Empty, ent_id, Guid.Empty, Guid.Empty, Guid.Empty, paginado, currentPage / pageSize, pageSize, out totalRecords, TIPO_MEIOCONTATO_TELEFONE, cur_id, crr_id);
        }

        /// <summary>
        /// Retorna um datatable contendo todos as escolas
        /// que não foram excluídas logicamente
        /// </summary>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect(int cur_id, Guid ent_id, bool paginado, int currentPage, int pageSize)
        {
            totalRecords = 0;

            if (pageSize == 0)
            {
                pageSize = 1;
            }

            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.SelectBy_cur_id(cur_id, Guid.Empty, ent_id, Guid.Empty, Guid.Empty, Guid.Empty, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Busca as escolas pelos filtros passados. Recebe também o filtro uad_idSuperior,
        /// para filtrar pela Unidade Administrativa Superior da escola.
        /// </summary>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="esc_nome">Nome da escola.</param>
        /// <param name="esc_codigo">Código da escola.</param>
        /// <param name="esc_situacao">Situação da escola.</param>
        /// <param name="tua_id">Tipo de Escola(UA).</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="uad_idSuperior">Id da unidade superior.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="TIPO_MEIOCONTATO_TELEFONE"></param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="gru_id">Id do grupo.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int esc_id,
            string esc_nome,
            string esc_codigo,
            byte esc_situacao,
            Guid tua_id,
            Guid ent_id,
            Guid uad_idSuperior,
            Guid gru_id,
            Guid usu_id,
            string TIPO_MEIOCONTATO_TELEFONE,
            int cur_id,
            int crr_id,
            bool paginado,
            int currentPage,
            int pageSize

        )
        {
            totalRecords = 0;

            if (pageSize == 0)
            {
                pageSize = 1;
            }

            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.SelectBy_Pesquisa(esc_id, esc_nome, esc_codigo, esc_situacao, tua_id, ent_id, uad_idSuperior, gru_id, usu_id, paginado, currentPage / pageSize, pageSize, out totalRecords, TIPO_MEIOCONTATO_TELEFONE, cur_id, crr_id);
        }

        /// <summary>
        /// Busca as escolas pelos filtros passados. Recebe também o filtro uad_idSuperior,
        /// para filtrar pela Unidade Administrativa Superior da escola.
        /// </summary>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="esc_nome">Nome da escola.</param>
        /// <param name="esc_codigo">Código da escola.</param>
        /// <param name="esc_situacao">Situação da escola</param>
        /// <param name="tua_id">Tipo de Escola(UA)</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="uad_idSuperior">Id da unidade superior.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="TIPO_MEIOCONTATO_TELEFONE"></param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="gru_id">Id do grupo.</param>
        /// <param name="tce_id">Id do tipo de classificação.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectNaoPaginado
        (
            int esc_id,
            string esc_nome,
            string esc_codigo,
            byte esc_situacao,
            Guid tua_id,
            Guid ent_id,
            Guid uad_idSuperior,
            Guid gru_id,
            Guid usu_id,
            string TIPO_MEIOCONTATO_TELEFONE,
            int cur_id,
            int crr_id,
            int tce_id
        )
        {
            totalRecords = 0;

            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.SelectBy_PesquisaNaoPaginado(esc_id, esc_nome, esc_codigo, esc_situacao, tua_id, ent_id, uad_idSuperior, gru_id, usu_id, out totalRecords, TIPO_MEIOCONTATO_TELEFONE, cur_id, crr_id, tce_id);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os usuarios
        /// das escolas que não foram excluídas logicamente
        /// </summary>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="tua_id"></param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="uad_idSuperior">Id da unidade superior.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="gru_id">Id do grupo.</param>
        /// <param name="sis_id">Id do sistema.</param>
        /// <param name="gru_idPadraoSistema"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect_Usuarios
        (
            int cur_id,
            Guid tua_id,
            Guid ent_id,
            Guid uad_idSuperior,
            Guid gru_id,
            Guid usu_id,
            int sis_id,
            Guid gru_idPadraoSistema
        )
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.Select_Usuarios(cur_id, tua_id, ent_id, uad_idSuperior, gru_id, usu_id, sis_id, gru_idPadraoSistema);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os usuarios
        /// das escolas que não foram excluídas logicamente
        /// </summary>
        /// <param name="esc_codigo"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static string SelecionaNomeEscolaPorCodigoEscola
        (
            string esc_codigo
        )
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.SelecionaNomeEscolaPorCodigoEscola(esc_codigo);
        }

        /// <summary>
        /// Busca as escolas pelos filtros passados. Recebe também o filtro uad_idSuperior,
        /// para filtrar pela Unidade Administrativa Superior da escola.
        /// </summary>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="esc_nome">Nome da escola.</param>
        /// <param name="esc_codigo">Código da escola.</param>
        /// <param name="esc_situacao">Situação da escola.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="uad_idSuperior">Id da unidada superior.</param>
        /// <param name="TIPO_MEIOCONTATO_TELEFONE"></param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int esc_id,
            string esc_nome,
            string esc_codigo,
            byte esc_situacao,
            Guid ent_id,
            Guid uad_idSuperior,
            string TIPO_MEIOCONTATO_TELEFONE,
            int cur_id,
            int crr_id,
            bool paginado,
            int currentPage,
            int pageSize

        )
        {
            totalRecords = 0;

            if (pageSize == 0)
            {
                pageSize = 1;
            }

            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.SelectBy_Pesquisa(esc_id, esc_nome, esc_codigo, esc_situacao, Guid.Empty, ent_id, uad_idSuperior, Guid.Empty, Guid.Empty, paginado, currentPage / pageSize, pageSize, out totalRecords, TIPO_MEIOCONTATO_TELEFONE, cur_id, crr_id);
        }

        /// <summary>
        /// Retorna uma List de Guid com todas as Unidades administrativas que o usuário tem
        /// acesso no grupo.
        /// </summary>
        /// <param name="gru_id">Id do grupo.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <returns></returns>
        public static List<Guid> GetSelect_Uad_Ids_By_PermissaoUsuario(Guid gru_id, Guid usu_id)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            DataTable dt = dao.Select_Uad_Ids_By_PermissaoUsuario(gru_id, usu_id);

            var lista = from DataRow r in dt.Rows
                        select (Guid)r["uad_id"];

            return lista.ToList();
        }

        /// <summary>
        /// Retorna uma String dos ids das uas, separado por vírgula, com todas as
        /// Unidades administrativas que o usuário tem  acesso no grupo.
        /// </summary>
        /// <param name="gru_id">Id do grupo.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <returns></returns>
        public static string GetUad_Ids_PermissaoUsuario(Guid gru_id, Guid usu_id)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            DataTable dt = dao.Select_Uad_Ids_By_PermissaoUsuario(gru_id, usu_id);

            var lista = from DataRow r in dt.Rows
                        select Convert.ToString(r["uad_id"]);

            return String.Join(",", lista.ToArray());
        }

        /// <summary>
        /// Buscas as escolas conforme o filtro passado
        /// </summary>
        /// <param name="esc_nome">nome da escola</param>
        /// <param name="esc_codigo">codigo da escola</param>
        /// <param name="uad_idSuperior">id da unidade administraviva superior</param>
        /// <param name="usu_id">id do usuario logado</param>
        /// <param name="gru_id">id do grupo do usuario logado</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectBy_NomeEscola_CodEscola
        (
            string esc_nome,
            string esc_codigo,
            Guid uad_idSuperior,
            Guid usu_id,
            Guid gru_id,
            bool paginado,
            int currentPage,
            int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
            {
                pageSize = 1;
            }

            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.SelectBy_NomeEscola_CodEscola(esc_nome, esc_codigo, uad_idSuperior, usu_id, gru_id, paginado,
                currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Seleciona a escola ou UASuperior de acordo com a permissão do grupo do usuário na entidade
        /// </summary>
        /// <param name="usu_id">ID do usuário</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <returns>DataTable com o registro selecionado</returns>
        public static DataTable RetornaUAPermissaoUsuarioGrupo(Guid usu_id, Guid ent_id, Guid gru_id)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.RetornaUAPermissaoUsuarioGrupo(usu_id, ent_id, gru_id);
        }

        /// <summary>
        /// Seleciona a escola ou UASuperior de acordo com a permissão do grupo do usuário na entidade
        /// </summary>
        /// <param name="usu_id">ID do usuário</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <param name="uad_idSuperior">id da uad superior</param>
        /// <param name="orgEscolaCodigo">Se vai mostrar e ordenar as uad pelo codigo</param>
        /// <returns>DataTable com os registros</returns>
        public static DataTable SelectBy_PermissaoDoUsuario(Guid usu_id, Guid gru_id, Guid uad_idSuperior, bool orgEscolaCodigo)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.SelectBy_PermissaoDoUsuario(usu_id, gru_id, uad_idSuperior, orgEscolaCodigo, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todas as escolas a partir do cargo do colaborador. Usado na visão individual.
        /// Utilizado na tela: Atribuição do docente.
        /// </summary>
        /// <param name="col_id">Id do colaborador.</param>
        /// <param name="crg_id">Id do cargo.</param>
        /// <param name="coc_id">Id do cargo do colaborador.</param>
        /// <returns>DataTable com as escolas.</returns>
        public static List<sComboUAEscola> SelecionaPorColaboradorCargoComHierarquia(long col_id, int crg_id, int coc_id, int appMinutosCacheLongo = 0)
        {
            List<sComboUAEscola> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaPorColaboradorCargoComHierarquia(col_id, crg_id, coc_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ESC_EscolaDAO dao = new ESC_EscolaDAO();
                        DataTable dtDados = dao.SelecionaPorColaboradorCargoComHierarquia(col_id, crg_id, coc_id);
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
                ESC_EscolaDAO dao = new ESC_EscolaDAO();
                DataTable dtDados = dao.SelecionaPorColaboradorCargoComHierarquia(col_id, crg_id, coc_id);
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
        /// Retorna a chave do cache utilizada para carregar o combo de escolas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaPorColaboradorCargoComHierarquia(long col_id, int crg_id, int coc_id)
        {
            return string.Format("Cache_SelecionaPorColaboradorCargoComHierarquia_{0}_{1}_{2}", col_id, crg_id, coc_id);
        }

        /// <summary>
        /// Retorna um datatable contendo todas as disciplinas e suas respectivas turmas que estao sem docentes e sem a marcacao da flag sem docente.
        /// Utilizado na tela: confirmação de fechamento do coc.
        /// </summary>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="esc_id">Id do calendario.</param>
        /// <returns>DataTable com as disciplinas da escola sem docente e sem marcacao de flag que esta sem docente.</returns>
        public static DataTable SelecionaDisciplinasSemDocente(int esc_id, int cal_id)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.SelecionaDisciplinasSemDocente(esc_id, cal_id);
        }

        #endregion Métodos de consulta

        #region Métodos de validação

        /// <summary>
        /// Consulta no banco escolas com o mesmo nome, com Id diferente do id passado, dentro
        /// da mesma entidade.
        /// </summary>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="esc_nome">Nome da escola.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <returns></returns>
        public static bool ExisteEscolaNomeIgual(int esc_id, string esc_nome, Guid ent_id)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.ExisteEscolaNomeIgual(esc_id, esc_nome, ent_id);
        }

        /// <summary>
        /// Consulta no banco escolas com o mesmo nome, com Id diferente do id passado, dentro
        /// da mesma entidade, para poder retornar o uad_id.
        /// </summary>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="esc_nome">Nome da escola.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <returns></returns>
        public static DataTable ExisteEscolaNomeIgualDt(int esc_id, string esc_nome, Guid ent_id)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.ExisteEscolaNomeIgualDt(esc_id, esc_nome, ent_id);
        }

        /// <summary>
        /// Verifica no banco se já existe o código da escola, dentro da mesma entidade.
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="esc_codigo">Código da escola</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <returns>True = Existe | False = Não existe</returns>
        public static bool VerificarCodigoExistente(int esc_id, string esc_codigo, Guid ent_id)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.SelectBy_Codigo(esc_id, esc_codigo, ent_id);
        }

        /// <summary>
        /// Verifica no banco se já existe o código de integração da escola, dentro da mesma entidade.
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="codigoIntegracao">Código de integração da escola</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <returns>True = Existe | False = Não existe</returns>
        public static bool VerificarCodigoIntegracaoExistente(int esc_id, string codigoIntegracao, Guid ent_id)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.SelectBy_CodigoIntegracao(esc_id, codigoIntegracao, ent_id);
        }

        /// <summary>
        /// Valida os dados da escola, antes de salvar.
        /// </summary>
        /// <param name="entityEscola">Entidade da escola.</param>
        /// <param name="msgErro">Mensagem de erro.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="dataAcesso">Data de acesso na tela de cadastro de escola.</param>
        private static bool ValidaEscola(ESC_Escola entityEscola, out string msgErro, Guid ent_id, DateTime dataAcesso)
        {
            msgErro = string.Empty;
            // bool bTestarFuncFim = true;

            ESC_Escola entAux = new ESC_Escola { esc_id = entityEscola.esc_id };
            GetEntity(entAux);

            if (entAux.esc_dataAlteracao > dataAcesso)
            {
                msgErro = "Escola não pode ser salva, pois esta foi alterada. Entre novamente no cadastro de escola para atualizar as informações.";
                return false;
            }

            // Verifica nome da escola.
            if (ExisteEscolaNomeIgual(entityEscola.esc_id, entityEscola.esc_nome, ent_id))
            {
                msgErro = "Já existe uma escola cadastrada com esse nome.";
                return false;
            }

            // Verifica código da escola.
            if (VerificarCodigoExistente(entityEscola.esc_id, entityEscola.esc_codigo, ent_id))
            {
                msgErro = "Já existe uma escola cadastrada com esse código.";
                return false;
            }

            //// Verifica se é alteração e alterou a data de fim de funcionamento
            //if (!entityEscola.IsNew)
            //{
            //    // Retornar a data de fim antes da alteração
            //    ESC_Escola esc = new ESC_Escola { esc_id = entityEscola.esc_id };
            //    GetEntity(esc);

            //    if (esc.esc_funcionamentoFim != new DateTime()
            //        && esc.esc_funcionamentoFim == entityEscola.esc_funcionamentoFim)
            //    {
            //        bTestarFuncFim = false;
            //    }
            //}

            // Retirar validação - ID 1591
            //// Verifica se data fim é anterior a data atual
            //if ((entityEscola.esc_funcionamentoFim != new DateTime()) &&
            //    (entityEscola.esc_funcionamentoFim.Date < DateTime.Now.Date) && bTestarFuncFim)
            //{
            //    msgErro = "A data de fim do funcionamento da escola não pode ser menor que a data atual.";
            //    return false;
            //}

            // Valida código de matrícula da escola.
            if (VerificarCodigoNumeroMatriculaExistente(entityEscola.esc_id, entityEscola.esc_codigoNumeroMatricula, ent_id))
            {
                msgErro = "Já existe uma escola cadastrada com esse código para " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_NUMEROMATRICULA") + ".";
                return false;
            }

            return true;
        }

        /// <summary>
        /// O método verifica se já existe uma escola com o mesmo código para número de matrícula
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="esc_codigoNumeroMatricula">Código para número de matrícula da escola</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <returns>True se já existe</returns>
        public static bool VerificarCodigoNumeroMatriculaExistente(int esc_id, int esc_codigoNumeroMatricula, Guid ent_id)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            return dao.VerificaExistentePorCodigoNumeroMatricula(esc_id, esc_codigoNumeroMatricula, ent_id);
        }

        /// <summary>
        /// Valida os dados da unidade administrativa de acordo com o padrão do censo escolar.
        /// </summary>
        /// <param name="entityEscola">Entidade ESC_Escola</param>
        /// <param name="entityEndereco">Entidade END_Endereco</param>
        /// <param name="entityPredioEndereco">Entidade ESC_PredioEndereco</param>
        public static void ValidaCensoEscolar(ESC_Escola entityEscola, END_Endereco entityEndereco, ESC_PredioEndereco entityPredioEndereco)
        {
            string numero = entityPredioEndereco != null ? entityPredioEndereco.ped_numero : string.Empty;
            string complemento = entityPredioEndereco != null ? entityPredioEndereco.ped_complemento : string.Empty;

            SYS_UnidadeAdministrativaBO.ValidaCensoEscolar(entityEscola.esc_nome, "Escola", entityEndereco, numero, complemento);
        }

        /// <summary>
        /// Verifica se a escola possui mais de uma unidade administrativa superior
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <returns></returns>
        public static bool VerificaPossuiOutraUnidadeAdministrativa(int esc_id)
        {
            return new ESC_EscolaDAO().VerificaPossuiOutraUnidadeAdministrativa(esc_id);
        }

        public static void ValidaNomeArquivoFundoVerso(string nomeArquivo)
        {
            if (nomeArquivo.Contains("/") || nomeArquivo.Contains("\\") || nomeArquivo.Contains(":") || nomeArquivo.Contains("*") ||
                nomeArquivo.Contains("?") || nomeArquivo.Contains("\"") || nomeArquivo.Contains("<") || nomeArquivo.Contains(">") || 
                nomeArquivo.Contains("|"))
                throw new ValidationException("O nome do arquivo de fundo do verso da carteirinha não pode conter os caracteres: \\ / : * ? \" < > |");
            if (nomeArquivo.Length > 260)
                throw new ValidationException("O nome do arquivo de fundo do verso da carteirinha não pode ser maior que 260 caracteres.");
        }

        #endregion Métodos de validação
    }
}