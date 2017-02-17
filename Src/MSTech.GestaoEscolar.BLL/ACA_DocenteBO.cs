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
using MSTech.CoreSSO.WebServices.Consumer;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region Estruturas
    
    [Serializable]
    public struct sUsrDocente
    {
        public Guid usu_id { get; set; }

        public string usu_login { get; set; }

        public string usu_email { get; set; }

        public string pes_nome { get; set; }
    }

    #endregion Estruturas

    public class ACA_DocenteBO : BusinessBase<ACA_DocenteDAO, ACA_Docente>
    {
        #region Métodos consultar

        /// <summary>
        /// Retorna todos os docentes que
        /// não foram excluídos logicamente.
        /// </summary>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <returns>Id e nome do docente.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorTurma
        (
            Guid ent_id
            , int esc_id
            , long tur_id
        )
        {
            ACA_DocenteDAO dao = new ACA_DocenteDAO();
            return dao.SelecionaPorTurma(ent_id, esc_id, tur_id);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os docentes
        /// que não foram excluídas logicamente.
        /// </summary>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <returns>Id e nome do docente.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorEscola
        (
            int esc_id
            , int uni_id
            , Guid ent_id
        )
        {
            ACA_DocenteDAO dao = new ACA_DocenteDAO();
            return dao.SelecionaPorEscola(esc_id, uni_id, ent_id);
        }

        /// <summary>
        /// Retorna os professores de acordo com a especialidade dele
        /// (ou ele é especialista ou pode lecionar a matéiria informada [tds_id])
        /// </summary>
        public static DataTable GetSelectBy_Especialidade_Escola
        (
            Int32 esc_id
            , Int32 uni_id
            , Guid ent_id
        )
        {
            ACA_DocenteDAO dao = new ACA_DocenteDAO();
            return dao.SelectBy_Especialidade_Escola(esc_id, uni_id, ent_id);
        }

        /// <summary>
        /// Retorna os professores de acordo com a especialidade dele
        /// (ou ele é especialista ou pode lecionar a matéiria informada [tds_id])
        /// </summary>
        public static DataTable GetSelectBy_Especialidade
        (
            Int32 esc_id
            , Int32 uni_id
            , Guid ent_id
            , bool crg_especialista
            , Int32 tds_id
        )
        {
            ACA_DocenteDAO dao = new ACA_DocenteDAO();
            return dao.SelectBy_Especialidade(esc_id, uni_id, ent_id, crg_especialista, tds_id);
        }

        /// <summary>
        /// Traz os docentes que lecionam na escola informada (pela UA do cargo).
        /// Exibindo os cargos dos docentes
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        public static DataTable GetSelectBy_EscolaCargo_ExibindoCargo
        (
            Int32 esc_id
            , Guid ent_id
        )
        {
            ACA_DocenteDAO dao = new ACA_DocenteDAO();
            return dao.SelectBy_EscolaCargo_ExibindoCargo(esc_id, ent_id);
        }

        /// <summary>
        /// Traz os docentes que lecionam na escola informada (pela UA do cargo).
        /// Exibindo os cargos dos docentes
        /// Seleciona apenas um docente se passar o doc_id
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="doc_id">ID do docente</param>
        /// <returns></returns>
        public static DataTable GetSelectBy_Docente_EscolaCargo_ExibindoCargo
        (
            Int32 esc_id
            , Int64 doc_id
            , Guid ent_id
        )
        {
            ACA_DocenteDAO dao = new ACA_DocenteDAO();
            return dao.SelectBy_Docente_EscolaCargo_ExibindoCargo(esc_id, doc_id, ent_id);
        }

        /// <summary>
        /// Traz os docentes que lecionam na turma informada (pela turma docente).
        /// Exibindo os cargos dos docentes
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="tur_id">ID da turma</param>
        /// <returns></returns>
        public static DataTable GetSelectBy_Turma_EscolaCargo_ExibindoCargo
        (
            Int32 esc_id
            , Int64 tur_id
            , Guid ent_id
        )
        {
            ACA_DocenteDAO dao = new ACA_DocenteDAO();
            return dao.SelectBy_Turma_EscolaCargo_ExibindoCargo(esc_id, tur_id, ent_id);
        }

        /// <summary>
        /// Traz os docentes que lecionam na escola informada (pela UA do cargo).
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        public static DataTable GetSelectBy_EscolaCargo
        (
            Int32 esc_id
            , Guid ent_id
        )
        {
            ACA_DocenteDAO dao = new ACA_DocenteDAO();
            return dao.SelectBy_EscolaCargo(esc_id, ent_id);
        }

        /// <summary>
        /// Traz os docentes de acordo com a função na escola
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="fun_id">ID da função</param>
        ///<param name="doc_id">ID do docente especifico(pode ser ignorado o filtro)</param>
        /// <returns></returns>
        public static DataTable SelecionaDocentesPorFuncaoEscola
        (
            Int32 esc_id
            , Int32 uni_id
            , Int32 fun_id
            , Int64 doc_id
        )
        {
            ACA_DocenteDAO dao = new ACA_DocenteDAO();
            return dao.SelectBy_EscolaFuncao(esc_id, uni_id, fun_id, doc_id);
        }

        /// <summary>
        /// retorna o docente apartir da entidade e pessoa
        /// </summary>
        /// <param name="pes_id">Id da pessoa</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="entity">Entidade ACA_Docente</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool GetSelectBy_Pessoa
        (
            Guid ent_id
            , Guid pes_id
            , out ACA_Docente entity
        )
        {
            entity = new ACA_Docente();

            entity = CacheManager.Factory.Get(
                String.Format(ModelCache.DOCENTE_POR_ENTIDADE_PESSOA_MODEL_KEY, ent_id, pes_id),
                () =>
                {
                    return new ACA_DocenteDAO().SelectBy_Pessoa(ent_id, pes_id);
                },
                GestaoEscolarUtilBO.MinutosCacheLongo
            );

            return entity.doc_id > 0;
        }

        /// <summary>
        /// Retorna um datatable contendo todos os docentes
        /// que não foram excluídos logicamente, filtrados por
        /// nome da pessoa, tipo de documento CPF, tipo de documento RG,
        /// entidade, unidade administrativa, cargo, função e situação
        /// </summary>
        /// <param name="pes_nome">Id da tabela PES_Pessoa do bd</param>
        /// <param name="tipo_cpf"></param>
        /// <param name="tipo_rg"></param>
        /// <param name="ent_id">Id da tabela SYS_Entidade do bd</param>
        /// <param name="gru_id"></param>
        /// <param name="crg_id">Id da tabela RHU_Cargo do bd</param>
        /// <param name="esc_id">Id da tabela ESC_Escola do bd</param>
        /// <param name="uni_id">Id da tabela ESC_UnidadeEscola do bd</param>
        /// <param name="pes_id"></param>
        /// <param name="doc_situacao">Campo doc_situacao da tabela ACA_Docente do bd</param>
        /// <param name="fun_id"></param>
        /// <param name="uad_idSuperior"></param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="usu_id"></param>
        /// <returns>DataTable com os docentes</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            string pes_nome
            , string tipo_cpf
            , string tipo_rg
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int crg_id
            , int esc_id
            , int uni_id
            , Guid pes_id
            , byte doc_situacao
            , int fun_id
            , Guid uad_idSuperior
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;
            ACA_DocenteDAO dao = new ACA_DocenteDAO();
            return dao.SelectBy_Pesquisa(pes_nome, tipo_cpf, tipo_rg, ent_id, usu_id, gru_id, crg_id, esc_id, uni_id, pes_id, doc_situacao, fun_id, uad_idSuperior, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Seleciona todos os docentes que não foram excluídos
        /// logicamente conforme os filtros passados.
        /// </summary>
        /// <param name="pes_nome">Nome do docente.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="gru_id">Id do grupo do usuário.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade escolar.</param>
        /// <param name="doc_situacao">Situação do docente.</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não.</param>
        /// <param name="currentPage">Página atual do grid.</param>
        /// <param name="coc_matricula">Matrícula do docente.</param>
        /// <param name="pageSize">Total de registros por página do grid.</param>
        /// <param name="uad_idSuperior">Id da unidade adm superior.</param>
        /// <returns>DataTable com os docentes</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            string pes_nome
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int esc_id
            , int uni_id
            , byte doc_situacao
            , Guid uad_idSuperior
            , string coc_matricula
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;
            ACA_DocenteDAO dao = new ACA_DocenteDAO();

            return dao.SelectBy_Pesquisa(pes_nome, ent_id, usu_id, gru_id, esc_id, uni_id, doc_situacao, uad_idSuperior, coc_matricula, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os docentes
        /// que não foram excluídas logicamente, filtrados por
        /// código do docente, código do colaborador, código da escola,
        /// código da unidade e situação do docente
        /// </summary>
        /// <param name="doc_id">Id da Tabela ACA_Docente do bd</param>
        /// <param name="col_id">ID da Tabela RHU_Colaborador do bd</param>
        /// <param name="esc_id">Id da tabela ESC_Escola do bd</param>
        /// <param name="uni_id">Id da tabela ESC_UnidadeEscola do bd</param>
        /// <param name="ent_id"></param>
        /// <param name="doc_situacao">Campo col_situcao da tabela ACA_Docente do bd</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <returns>DataTable com os docentes</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int doc_id
            , int col_id
            , int esc_id
            , int uni_id
            , Guid ent_id
            , byte doc_situacao
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;
            ACA_DocenteDAO dao = new ACA_DocenteDAO();
            return dao.SelectBy_All(doc_id, col_id, esc_id, uni_id, doc_situacao, ent_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os colaboradores sem considerar permissão, se assim for configurado
        /// </summary>
        /// <param name="pes_nome">Nome do colaborador</param>
        /// <param name="coc_matricula">Matrícula do colaborador</param>
        /// <param name="tipo_cpf">CPF do colaborador</param>
        /// <param name="tipo_rg">RG do colaborador</param>
        /// <param name="crg_id">Cargo do colaborador</param>
        /// <param name="fun_id">Função do colaborador</param>
        /// <param name="uad_id">UA do cargo/função do colaborador</param>
        /// <param name="uad_idSuperior">ID da unidade administrativa superior</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="todosDocentes">Indica se vai considerar a permissão ou não</param>
        /// <param name="usu_id">Usuário logado</param>
        /// <param name="gru_id">Grupo do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDocentesNaoPaginadoComPermissaoTotal
        (
            string pes_nome
            , string coc_matricula
            , string tipo_cpf
            , string tipo_rg
            , int crg_id
            , int fun_id
            , Guid uad_id
            , Guid uad_idSuperior
            , Guid ent_id
            , bool todosDocentes
            , Guid usu_id
            , Guid gru_id
        )
        {
            totalRecords = 0;

            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            ACA_DocenteDAO dao = new ACA_DocenteDAO();
            return dao.SelectBy_Pesquisa_PermissaoTotal(pes_nome, coc_matricula, tipo_cpf, tipo_rg, crg_id, fun_id, uad_id, uad_idSuperior, ent_id, todosDocentes, usu_id, gru_id, MostraCodigoEscola, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os docentes
        /// que não foram excluídas logicamente, filtrados por
        /// código do docente, código do colaborador, código da escola,
        /// código da unidade e situação do docente
        /// </summary>
        /// <param name="doc_id">Id da Tabela ACA_Docente do bd</param>
        /// <param name="col_id">ID da Tabela RHU_Colaborador do bd</param>
        /// <param name="esc_id">Id da tabela ESC_Escola do bd</param>
        /// <param name="uni_id">Id da tabela ESC_UnidadeEscola do bd</param>
        /// <param name="ent_id"></param>
        /// <param name="doc_situacao">Campo col_situcao da tabela ACA_Docente do bd</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <returns>DataTable com os docentes</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_All
        (
            int doc_id
            , int col_id
            , int esc_id
            , int uni_id
            , Guid ent_id
            , byte doc_situacao
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;
            ACA_DocenteDAO dao = new ACA_DocenteDAO();
            return dao.SelectBy_All(doc_id, col_id, esc_id, uni_id, doc_situacao, ent_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Seleciona os dados utilizados na tela de alteração de docente
        /// </summary>
        /// <param name="col_id">ID do colaborador</param>
        /// <param name="doc_id">ID do docente</param>
        /// <returns>DataTable de dados utilizados na tela de alteração de docente</returns>
        public static DataTable SelecionaPorColaboradorDocente(long col_id, long doc_id)
        {
            ACA_DocenteDAO dao = new ACA_DocenteDAO();
            return dao.SelectByColaboradorDocente(col_id, doc_id);
        }

        /// <summary>
        /// Seleciona os dados da pessoa e das turmas do docente.
        /// </summary>
        /// <param name="doc_id">ID do docente.</param>
        /// <returns></returns>
        public static DataTable SelecionaDadosDocente(long doc_id)
        {
            return new ACA_DocenteDAO().SelecionaDadosDocente(doc_id);
        }

        /// <summary>
        /// Seleciona os dados da pessoa e das escolas do docente.
        /// </summary>
        /// <param name="doc_id">ID do docente.</param>
        /// <returns></returns>
        public static DataTable SelecionaEscolaDocente(long doc_id)
        {
            return new ACA_DocenteDAO().SelecionaEscolaDocente(doc_id);
        }

        /// <summary>
        /// Retorna os docentes de uma escola que não foram excluidos logicamente conforme os filtros passados.
        /// </summary>
        /// <param name="escId"></param>
        /// <param name="nome"></param>
        /// <param name="matricula"></param>
        /// <param name="cpf"></param>
        /// <param name="tdsId"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectBy_PesquisaEscola(
            int escId
            , string nome
            , string matricula
            , string cpf
            , int tdsId)
        {
            totalRecords = 0;
            return new ACA_DocenteDAO().SelectBy_PesquisaEscola(escId, nome, matricula, cpf, tdsId, out totalRecords);
        }

        /// <summary>
        /// Retorna os docentes de uma escola quando informado ou da rede quando não informado conforme o filtro passado.
        /// </summary>
        /// <param name="escId"></param>
        /// <param name="nome"></param>
        /// <param name="matricula"></param>
        /// <param name="cpf"></param>
        /// <param name="rg"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectBy_PesquisaEscolaRede(
            int escId
            , string nome
            , string matricula
            , string cpf
            , string rg
            , Guid usu_id
            , Guid gru_id
            , Guid ent_id
            , bool adm
        )
        {
            totalRecords = 0;
            return new ACA_DocenteDAO().SelectBy_PesquisaEscolaRede(escId, nome, matricula, cpf, rg, usu_id, gru_id, ent_id, adm, out totalRecords);
        }
        
        /// <summary>
        /// Retorna se o docente possui uma atribuição esporádica 
        /// com vigência que englobe a data da aula, ou, no caso do docente normal,
        /// retorna o proprio docente.
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="dataAula"></param>
        /// <returns></returns>
        public static bool ValidaAulaAtribuicaoEsporadica(long docId, DateTime dataAula)
        {
            ACA_DocenteDAO dao = new ACA_DocenteDAO();
            return dao.ValidaAulaAtribuicaoEsporadica(docId, dataAula);
        }

        /// <summary>
        /// Retorna uma listagem de usuários de docentes na mesma unidade ou abaixo da unidade do usuário/grupo informado.
        /// </summary>
        /// <param name="usu_id">ID do usuário que está pesquisando</param>
        /// <param name="gru_id">ID do grupo do usuário que está pesquisando</param>
        /// <param name="sis_id">ID do sistema</param>
        /// <returns></returns>
        public static List<sUsrDocente> SelecionarUsrDocentesPorUsuarioGrupo(Guid usu_id, Guid gru_id, int sis_id, string usu_login, string usu_email, string pes_nome)
        {
            ACA_DocenteDAO dao = new ACA_DocenteDAO();
            DataTable dt = dao.SelecionarUsrDocentesPorUsuarioGrupo(usu_id, gru_id, sis_id, usu_login, usu_email, pes_nome);

            return dt.AsEnumerable().Select(u => new sUsrDocente
                                                    {
                                                        usu_id = new Guid(u["usu_id"].ToString()),
                                                        usu_login = u["usu_login"].ToString(),
                                                        usu_email = u["usu_email"].ToString(),
                                                        pes_nome = u["pes_nome"].ToString()
                                                    }).ToList();
        }
        
        #endregion Métodos consultar

        #region Métodos incluir e alterar

        /// <summary>
        /// Inclui ou Altera um Docente
        /// </summary>
        /// <param name="entityPessoa">Entidade PES_Pessoa</param>
        /// <param name="entityPessoaDeficiencia"></param>
        /// <param name="dtEndereco">Datatable Enderecos</param>
        /// <param name="dtContato">Datatable Contatos</param>
        /// <param name="dtDocumento">Datatable Documentos</param>
        /// <param name="entityCertidaoCivil"></param>
        /// <param name="pai_idAntigo">Campo pai_idNacionalidade antigo</param>
        /// <param name="cid_idAntigo">Campo cid_idNaturalidade antigo</param>
        /// <param name="PaiAntigo">StructColaboradorFiliacao PaiAntigo com dados do pai</param>
        /// <param name="MaeAntigo">StructColaboradorFiliacao MaeAntigo com dados da mae</param>
        /// <param name="tes_idAntigo">Campo tes_id antigo</param>
        /// <param name="tde_idAntigo"></param>
        /// <param name="entityColaborador">Entidade RHU_Colaborador</param>
        /// <param name="dtCargoFuncao">Datatable Cargos/Funções</param>
        /// <param name="dtCargoDisciplina"></param>
        /// <param name="bSalvarUsuario">Indica se será incluído um usuário para o colaborador</param>
        /// <param name="bSalvarLive"></param>
        /// <param name="entityUsuario">Entidade SYS_Usuario</param>
        /// <param name="bEnviaEmail">Bool se envia e-mail</param>
        /// <param name="sNomePortal">String com o nome do portal para e-mail</param>
        /// <param name="sHost">String com o host para e-mail</param>
        /// <param name="sEmailSuporte">String com o endereço do e-mail de suporte</param>
        /// <param name="arquivosPermitidos">Extensões de tipos de arquivos permitidos para a foto</param>
        /// <param name="tamanhoMaximoKB">Tamanho máximo da foto permitida</param>
        /// <param name="entFoto">Entidade da foto da pessoa</param>
        /// <param name="ExcluirImagemAtual">Indica se imagem atual será excluída</param>
        /// <param name="entityDocente"></param>
        /// <param name="ent_idUsuario"></param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool Save
        (
            PES_Pessoa entityPessoa
            , PES_PessoaDeficiencia entityPessoaDeficiencia
            , DataTable dtEndereco
            , DataTable dtContato
            , DataTable dtDocumento
            , PES_CertidaoCivil entityCertidaoCivil
            , Guid pai_idAntigo
            , Guid cid_idAntigo
            , StructColaboradorFiliacao PaiAntigo
            , StructColaboradorFiliacao MaeAntigo
            , Guid tes_idAntigo
            , Guid tde_idAntigo
            , RHU_Colaborador entityColaborador
            , DataTable dtCargoFuncao
            , DataTable dtCargoDisciplina
            , bool bSalvarUsuario
            , bool bSalvarLive
            , SYS_Usuario entityUsuario
            , bool bEnviaEmail
            , string sNomePortal
            , string sHost
            , string sEmailSuporte
            , Guid ent_idUsuario
            , ACA_Docente entityDocente
            , string[] arquivosPermitidos
            , int tamanhoMaximoKB
            , CFG_Arquivo entFoto
            , bool ExcluirImagemAtual
        )
        {
            TalkDBTransaction bancoCore = new PES_PessoaDAO()._Banco.CopyThisInstance();
            TalkDBTransaction bancoGestao = new ACA_DocenteDAO()._Banco.CopyThisInstance();

            bancoCore.Open(IsolationLevel.ReadCommitted);
            bancoGestao.Open(IsolationLevel.ReadCommitted);

            try
            {
                //Limpa o cache do docente
                CacheManager.Factory.Remove(string.Format(ModelCache.DOCENTE_POR_ENTIDADE_PESSOA_MODEL_KEY, entityColaborador.ent_id, entityPessoa.pes_id));

                //Verifica se os dados da pessoa serão sempre salvos em maiúsculo.
                string sSalvarMaiusculo = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.SALVAR_SEMPRE_MAIUSCULO);
                bool Salvar_Sempre_Maiusculo = !string.IsNullOrEmpty(sSalvarMaiusculo) && Convert.ToBoolean(sSalvarMaiusculo);

                var y = from DataRow dr in dtCargoFuncao.Rows
                        where dr.RowState == DataRowState.Deleted
                        select dr;

                if (dtCargoFuncao.Rows.Count == 0 || y.Count() == dtCargoFuncao.Rows.Count)
                    throw new ValidationException("É obrigatório o preenchimento de pelo menos um vínculo de trabalho do docente.");

                string sPadraoUsuarioDocente = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.PAR_GRUPO_PERFIL_DOCENTE, ent_idUsuario);

                //Integraçao como o live
                UserLive entityUserLive = null;
                if (bSalvarLive)
                {
                    entityUserLive = new UserLive(eTipoUserLive.Docente);

                    //Cria o usuário docente para integraçao como o live
                    ManageUserLive live = new ManageUserLive();
                    entityUserLive.email = entityUsuario.usu_email;
                    entityUserLive.senha = entityUsuario.usu_senha;

                    //Caso seja alteração carrega as turma
                    //TODO: Fazer método específico para buscar apenas pelo doc_id.
                    DataTable dtTurmas = entityDocente.doc_id > 0 ?
                        TUR_TurmaBO.GetSelectBy_Pesquisa_TodosTipos
                        (Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, 0, 0, 0, 0, 0, 0, "", entityDocente.doc_id, false) : new DataTable();

                    //Obtendo CPF do docente
                    string tdo_id = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF);

                    var x = from DataRow dr in dtDocumento.Rows
                            where dr.RowState != DataRowState.Deleted && dr["tdo_id"].ToString() == tdo_id
                            select dr["numero"].ToString();

                    //Carrega primeira matricula ativa associada ao cargo de docente do colaborador
                    var mat = from DataRow dr in dtCargoFuncao.Rows
                              where dr.RowState != DataRowState.Deleted && dr["situacao_id"].ToString() == "1"
                              select dr["coc_matricula"].ToString();

                    //Carrega os tipos de disciplinas
                    var tipo_dis = from DataRow dr in dtCargoDisciplina.Rows
                                   where dr.RowState != DataRowState.Deleted
                                   select dr["tds_id"].ToString();

                    DataTable dtTipoDisciplinas = tipo_dis.Count() > 0 ?
                        ACA_TipoDisciplinaBO.SelecionaTipoDisciplinaPorTipoDisciplina
                        (Convert.ToInt32(tipo_dis.First()), bancoGestao, ent_idUsuario) : new DataTable();

                    if (x.Count() > 0)
                    {
                        DadosUserDocente dadosDocente = new DadosUserDocente
                        {
                            nome = entityPessoa.pes_nome
                            ,
                            CPF = x.First()
                            ,
                            matricula = mat.Count() > 0 ? mat.First() : string.Empty
                            ,
                            turma = dtTurmas.Rows.Count > 0 ? dtTurmas.Rows[0]["tur_cod_desc_nome"].ToString() : string.Empty
                            ,
                            serie = dtTurmas.Rows.Count > 0 ? dtTurmas.Rows[0]["crp_descricao"].ToString() : string.Empty
                            ,
                            disciplina = dtTipoDisciplinas.Rows.Count > 0 ? dtTipoDisciplinas.Rows[0]["tds_nome"].ToString() : string.Empty
                        };
                        entityUserLive.dadosUserDocente = dadosDocente;
                    }
                    else
                    {
                        if (!live.VerificarContaEmailExistente(entityUserLive))
                            throw new ArgumentException("CPF é um documento obrigatório, para integração do docente com live.");
                    }
                }

                RHU_ColaboradorBO.Save(entityPessoa
                                             , entityPessoaDeficiencia
                                             , dtEndereco
                                             , dtContato
                                             , dtDocumento
                                             , entityCertidaoCivil
                                             , pai_idAntigo
                                             , cid_idAntigo
                                             , PaiAntigo
                                             , MaeAntigo
                                             , tes_idAntigo
                                             , tde_idAntigo
                                             , entityColaborador
                                             , dtCargoFuncao
                                             , dtCargoDisciplina
                                             , bSalvarUsuario
                                             , bSalvarLive
                                             , entityUsuario
                                             , entityUserLive
                                             , sPadraoUsuarioDocente
                                             , bEnviaEmail
                                             , sNomePortal
                                             , sHost
                                             , sEmailSuporte
                                             , ent_idUsuario
                                             , bancoCore
                                             , bancoGestao
                                             , arquivosPermitidos
                                             , tamanhoMaximoKB
                                             , entFoto
                                             , ExcluirImagemAtual);

                entityDocente.col_id = entityColaborador.col_id;

                if (entityDocente.Validate())
                {
                    Save(entityDocente, bancoGestao);
                }
                else
                {
                    throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entityDocente));
                }
                
                return true;
            }
            catch (Exception err)
            {
                bancoGestao.Close(err);
                bancoCore.Close(err);

                throw;
            }
            finally
            {
                bancoGestao.Close();
                bancoCore.Close();
            }
        }
        
        #endregion Métodos incluir e alterar

        #region Métodos excluir

        /// <summary>
        /// Deleta logicamente um docente
        /// </summary>
        /// <param name="entity">Entidade ACA_Docente</param>
        /// <returns>True = deletado/alterado | False = não deletado/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            ACA_Docente entity
        )
        {
            ACA_DocenteDAO dao = new ACA_DocenteDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                //Verifica se o docente pode ser deletado
                if (GestaoEscolarUtilBO.VerificarIntegridade("doc_id", entity.doc_id.ToString(), "ACA_Docente,REL_SituacaoPlanejamentoAulasNotas", dao._Banco))
                    throw new ValidationException("Não é possível excluir o docente pois possui outros registros ligados a ele.");

                //Deleta logicamente o docente
                dao.Delete(entity);

                RHU_Colaborador col = new RHU_Colaborador { col_id = entity.col_id };
                RHU_ColaboradorBO.GetEntity(col);

                RHU_ColaboradorBO.Delete(col, dao._Banco);

                //Limpa o cache do docente
                CacheManager.Factory.Remove(string.Format(ModelCache.DOCENTE_POR_ENTIDADE_PESSOA_MODEL_KEY, col.ent_id, col.pes_id));

                return true;
            }
            catch (Exception err)
            {
                dao._Banco.Close(err);
                throw;
            }
            finally
            {
                dao._Banco.Close();
            }
        }

        #endregion Métodos excluir
        
    }
}