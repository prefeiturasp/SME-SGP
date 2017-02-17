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
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System.Text.RegularExpressions;
using MSTech.GestaoEscolar.CustomResourceProviders;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Situações do colaborador
    /// </summary>
    public enum RHU_ColaboradorSituacao : byte
    {
        Ativo = 1
        ,

        Bloqueado = 2
        ,

        Excluido = 3
        ,

        Demitido = 4
        ,

        Afastado = 5
    }

    #endregion Enumeradores

    #region Estruturas

    /// <summary>
    /// Estrutura para guardar os registros de ColaboradorCargo, de docente e usuário.
    /// </summary>
    [Serializable]
    public struct ColaboradoresAtribuicao
    {
        public Int64 col_id;
        public Int64 doc_id;
        public int crg_id;
        public int coc_id;
        public string pes_nome;
        public Guid usu_id;
        public int esc_id;
    }
    
    /// <summary>
    /// Estrutura utilizada para cadastrar o colaborador.
    /// </summary>
    public struct StructColaboradorFiliacao
    {
        public PES_Pessoa entPessoa;
        public List<PES_PessoaDocumento> listaDocumentos;
    }

    #endregion Estruturas

    public class RHU_ColaboradorBO : BusinessBase<RHU_ColaboradorDAO, RHU_Colaborador>
    {
        #region Métodos consultar

        /// <summary>
        /// Retorna os docentes e colaboradores para atribuiçao esporádica
        /// qualquer docente pelos filtros;
        /// outros colaboradores nao docentes que possuam cargo base de docente
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="coc_matricula"></param>
        /// <returns></returns>
        public static List<ColaboradoresAtribuicao> PesquisaPorFiltros_AtribuicaoEsporadica(string coc_matricula, Guid ent_id)
        {
            DataTable dt = new RHU_ColaboradorDAO().PesquisaPorFiltros_AtribuicaoEsporadica(coc_matricula, ent_id);

            return (from DataRow dr in dt.Rows
                    select (ColaboradoresAtribuicao)GestaoEscolarUtilBO.DataRowToEntity
                    (dr, new ColaboradoresAtribuicao())).ToList();
        }

        /// <summary>
        /// Retorna os docentes e colaboradores com atribuiçao esporádica cadastrada para aquela escola
        /// </summary>
        /// <param name="esc_id"></param>
        /// <returns></returns>
        public static DataTable PesquisaAtribuicoesEsporadicas_PorFiltros(int esc_id)
        {
            DataTable dt = new RHU_ColaboradorDAO().PesquisaAtribuicaoEsporadica_PorFiltros(esc_id);
            totalRecords = dt.Rows.Count;
            return dt;
        }

        /// <summary>
        /// Retorna um datatable contendo todos os colaboladores
        /// que não foram excluídos logicamente
        /// Com paginação
        /// </summary>
        /// <param name="pes_nome">Id da tabela PES_Pessoa do bd</param>
        /// <param name="tipo_cpf"></param>
        /// <param name="tipo_rg"></param>
        /// <param name="crg_id">Id da tabela RHU_Cargo do bd</param>
        /// <param name="fun_id">Id da tabela RHU_Funcao do bd</param>
        /// <param name="uad_id"></param>
        /// <param name="adm"></param>
        /// <param name="ent_id">Id da tabela SYS_Entidade do bd</param>
        /// <param name="usu_id"></param>
        /// <param name="gru_id"></param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <returns>DataTable com os colaboradores</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaColaboradorPaginado
        (
            string pes_nome
            , string tipo_cpf
            , string tipo_rg
            , int crg_id
            , int fun_id
            , Guid uad_id
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            RHU_ColaboradorDAO dao = new RHU_ColaboradorDAO();
            return dao.SelectBy_Pesquisa(pes_nome, tipo_cpf, tipo_rg, crg_id, fun_id, uad_id, ent_id, adm, usu_id, gru_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os colaboradores sem considerar permissão, se assim for configurado
        /// </summary>
        /// <param name="pes_nome">Nome do colaborador</param>
        /// <param name="coc_matricula">Matricula do colaborador</param>
        /// <param name="tipo_cpf">CPF do colaborador</param>
        /// <param name="tipo_rg">RG do colaborador</param>
        /// <param name="crg_id">Cargo do colaborador</param>
        /// <param name="fun_id">Função do colaborador</param>
        /// <param name="uad_id">UA do cargo/função do colaborador</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="todosColaboradores">Indica se vai considerar a permissão ou não</param>
        /// <param name="usu_id">Usuário logado</param>
        /// <param name="gru_id">Grupo do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaColaboradorNaoPaginadoComPermissaoTotal
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
            , bool todosColaboradores
            , Guid usu_id
            , Guid gru_id
        )
        {
            totalRecords = 0;

            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            RHU_ColaboradorDAO dao = new RHU_ColaboradorDAO();
            return dao.SelectBy_Pesquisa_PermissaoTotal(pes_nome, coc_matricula, tipo_cpf, tipo_rg, crg_id, fun_id, uad_id, uad_idSuperior, ent_id, todosColaboradores, usu_id, gru_id, MostraCodigoEscola, out totalRecords);
        }

        /// <summary>
        /// Verifica se um documento já
        /// existe pra algum colaborador.
        /// </summary>
        /// <param name="tdo_id">Tipo de documento</param>
        /// <param name="psd_numero">Número do documento</param>
        /// <param name="entityColaborador">Entidade RHU_Colaborador(contendo ent_id) </param>
        /// <returns></returns>
        public static bool ConsultarDocumento
        (
            Guid tdo_id
            , string psd_numero
            , RHU_Colaborador entityColaborador
        )
        {
            RHU_ColaboradorDAO dao = new RHU_ColaboradorDAO();
            return dao.SelectBy_Documento(tdo_id, psd_numero, entityColaborador);
        }

        public static DataTable BuscaPessoas(string pes_nome, string cpf, string rg, bool permiteAlunos)
        {
            RHU_ColaboradorDAO dao = new RHU_ColaboradorDAO();
            return dao.BuscaPessoas(pes_nome, cpf, rg, permiteAlunos);
        }

        public static DataTable SelecionaDocentesPorNomeMatricula(string pes_nome, string coc_matricula)
        {
            RHU_ColaboradorDAO dao = new RHU_ColaboradorDAO();
            DataTable dt = dao.SelecionaDocentesPorNomeMatricula(pes_nome, coc_matricula);
            return dt;
        }

        /// <summary>
        /// Seleciona os dados do colaborador/docente da tela de VisualizaConteudo
        /// </summary>
        /// <param name="parametro">Parâmetro: Nome do colaborador/docente OU matrícula</param>
        /// <returns>Retorna dados do colaborador/docente</returns>
        public static DataSet VisualizaConteudo(string parametro)
        {
            return new RHU_ColaboradorDAO().SelecionaVisualizaConteudo(parametro);
        }

        #endregion Métodos consultar

        #region Métodos incluir e alterar

        /// <summary>
        /// Altera apenas a vigência da entidade ColaboradorCargo (atribuição esporádica).
        /// </summary>
        /// <returns></returns>
        public static bool SalvarAtribuicaoEsporadicaAlteracao
        (
            ColaboradoresAtribuicao atribuicao
            , DateTime coc_vigenciaInicio
            , DateTime coc_vigenciaFim
            , Guid ent_id
        )
        {
            TalkDBTransaction banco = new RHU_ColaboradorDAO()._Banco.CopyThisInstance();

            try
            {
                banco.Open(IsolationLevel.ReadCommitted);

                #region Validações

                if (coc_vigenciaInicio > coc_vigenciaFim)
                {
                    throw new ValidationException("A data de início deve ser menor ou igual à data de fim da vigência.");
                }

                DataTable dtAulas = CLS_TurmaAulaBO.PesquisaPor_AtribuicaoEsporadica(atribuicao.col_id, atribuicao.crg_id, atribuicao.coc_id, banco);

                if (dtAulas.Rows.Count > 0)
                {
                    // Verificar se as aulas estão fora da nova vigência do docente.
                    var x = (from DataRow dr in dtAulas.Rows
                             let tau_data = Convert.ToDateTime(dr["tau_data"])
                             where
                                !(tau_data <= coc_vigenciaFim && tau_data >= coc_vigenciaInicio)
                             select tau_data);
                    if (x.Count() > 0)
                    {
                        throw new ValidationException
                            (CustomResource.GetGlobalResourceObject("BLL", "RHU_Colaborador.ValidacaoAulaAtribuicaoEsporadicaAlteracao"));
                    }
                }

                #endregion

                RHU_ColaboradorCargo entColaboradorCargoSalvar = RHU_ColaboradorCargoBO.GetEntity
                    (new RHU_ColaboradorCargo
                    {
                        col_id = atribuicao.col_id
                        ,
                        crg_id = atribuicao.crg_id
                        ,
                        coc_id = atribuicao.coc_id
                    }, banco);

                entColaboradorCargoSalvar.coc_vigenciaInicio = coc_vigenciaInicio;
                entColaboradorCargoSalvar.coc_vigenciaFim = coc_vigenciaFim;

                #region Valida se existe atribuição esporádica igual

                if (RHU_ColaboradorCargoBO.VerificaRegistroDuplicado(entColaboradorCargoSalvar, banco))
                {
                    throw new ValidationException
                        (CustomResource.GetGlobalResourceObject("BLL", "RHU_Colaborador.ValidacaoDuplicacaoAlteracao"));
                }

                #endregion

                // Seta a situação desativado caso já tenha terminado a vigência.
                if (coc_vigenciaFim < DateTime.Now.Date)
                {
                    entColaboradorCargoSalvar.coc_situacao = (byte)RHU_ColaboradorCargoSituacao.Desativado;
                }
                else
                {
                    entColaboradorCargoSalvar.coc_situacao = (byte)RHU_ColaboradorCargoSituacao.Ativo;
                }

                if (!entColaboradorCargoSalvar.Validate())
                {
                    throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entColaboradorCargoSalvar));
                }

                if (!RHU_ColaboradorCargoBO.Save(entColaboradorCargoSalvar, banco))
                {
                    throw new Exception("Não foi possível salvar a entidade ColaboradorCargo.");
                }


                // Alterar todas as atribuições de docentes para ajustar a data.
                List<TUR_TurmaDocente> listaTurmaDocente = 
                    TUR_TurmaDocenteBO.PesquisaPor_AtribuicaoEsporadica(atribuicao.col_id, atribuicao.crg_id, atribuicao.coc_id, banco);

                foreach (TUR_TurmaDocente item in listaTurmaDocente)
                {
                    item.tdt_vigenciaInicio = coc_vigenciaInicio;
                    item.tdt_vigenciaFim = coc_vigenciaFim;

                    TUR_TurmaDocenteBO.Save(item, banco);
                    TUR_TurmaDocenteBO.LimpaCache(item, ent_id);
                }

                // Limpar cache para a tela de atribuição de docentes.
                GestaoEscolarUtilBO.LimpaCache(ESC_UnidadeEscolaBO.RetornaChaveCache_SelectEscolas_VisaoIndividual(atribuicao.doc_id, ent_id, 1));
                GestaoEscolarUtilBO.LimpaCache(ESC_UnidadeEscolaBO.RetornaChaveCache_SelectEscolas_VisaoIndividual(atribuicao.doc_id, ent_id, 0));
                GestaoEscolarUtilBO.LimpaCache(ESC_UnidadeEscolaBO.RetornaChaveCache_SelectEscolas_VisaoIndividual(atribuicao.doc_id, ent_id, 2));

                return true;
            }
            catch (Exception ex)
            {
                if (banco.ConnectionIsOpen)
                {
                    banco.Close(ex);
                }

                throw ex;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                {
                    banco.Close();
                }
            }
        }

        /// <summary>
        /// Salva um registro na ColaboradorCargo, "transforma" o colaborador em docente, e adiciona o grupo de usuário de
        /// docente para o registro informado na entidade "atribuicao".
        /// </summary>
        /// <param name="atribuicao"></param>
        /// <param name="ent_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="coc_matricula"></param>
        /// <param name="coc_vigenciaInicio"></param>
        /// <param name="coc_vigenciaFim"></param>
        /// <returns></returns>
        public static bool SalvarAtribuicaoEsporadica
        (
            ColaboradoresAtribuicao atribuicao
            , Guid ent_id
            , int esc_id
            , string coc_matricula
            , DateTime coc_vigenciaInicio
            , DateTime coc_vigenciaFim
        )
        {
            List<RHU_Cargo> CargoAtribuicao = RHU_CargoBO.SelecionaPorTipo(ent_id, eTipoCargo.AtribuicaoEsporadica);

            if (CargoAtribuicao.Count > 0)
            {
                TalkDBTransaction banco = new RHU_ColaboradorDAO()._Banco.CopyThisInstance();
                TalkDBTransaction bancoCore = new SYS_UsuarioGrupoDAO()._Banco.CopyThisInstance();

                try
                {
                    banco.Open(IsolationLevel.ReadCommitted);
                    bancoCore.Open(IsolationLevel.ReadCommitted);

                    #region Validações

                    if (!ESC_EscolaClassificacaoBO.VerificaExisteCargoClassificacao(esc_id, atribuicao.crg_id, banco))
                    {
                        throw new ValidationException(CustomResource.GetGlobalResourceObject("BLL", "RHU_Colaborador.ValidacaoCargoTipoClassificacao"));
                    }

                    if (coc_vigenciaInicio > coc_vigenciaFim)
                    {
                        throw new ValidationException("A data de início deve ser menor ou igual à data de fim da vigência.");
                    }

                    #endregion

                    // Buscar a escola para saber o uad_id ligado à ela.
                    ESC_Escola escola = ESC_EscolaBO.GetEntity(new ESC_Escola { esc_id = esc_id }, banco);

                    RHU_ColaboradorCargo entColaboradorCargo = new RHU_ColaboradorCargo
                    {
                        col_id = atribuicao.col_id
                        ,
                        crg_id = CargoAtribuicao[0].crg_id
                        ,
                        coc_matricula = coc_matricula
                        ,
                        coc_vigenciaInicio = coc_vigenciaInicio
                        ,
                        coc_vigenciaFim = coc_vigenciaFim
                        ,
                        ent_id = ent_id
                        ,
                        uad_id = escola.uad_id
                        ,
                        // Seta a situação desativado caso já tenha terminado a vigência.
                        coc_situacao = coc_vigenciaFim < DateTime.Now.Date ? (byte)RHU_ColaboradorCargoSituacao.Desativado 
                            : (byte)RHU_ColaboradorCargoSituacao.Ativo
                        ,
                        IsNew = true
                    };
                    
                    if (!entColaboradorCargo.Validate())
                    {
                        throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entColaboradorCargo));
                    }

                    #region Valida se existe atribuição esporádica igual

                    if (RHU_ColaboradorCargoBO.VerificaRegistroDuplicado(entColaboradorCargo, banco))
                    {
                        throw new ValidationException
                            (CustomResource.GetGlobalResourceObject("BLL", "RHU_Colaborador.ValidacaoDuplicacaoInclusao"));
                    }

                    #endregion

                    if (!RHU_ColaboradorCargoBO.Save(entColaboradorCargo, banco))
                    {
                        throw new Exception("Não foi possível salvar a entidade ColaboradorCargo.");
                    }

                    if (CargoAtribuicao[0].crg_cargoDocente)
                    {
                        if (atribuicao.doc_id <= 0)
                        {
                            // Criar registro na tabela de docentes para transformar o colaborador em docente.
                            ACA_Docente entDocente = new ACA_Docente
                            {
                                col_id = atribuicao.col_id
                                ,
                                doc_situacao = 1
                                ,
                                IsNew = true
                            };

                            if (!entDocente.Validate())
                            {
                                throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entDocente));
                            }

                            if (!ACA_DocenteBO.Save(entDocente, banco))
                            {
                                throw new Exception("Não foi possível salvar a entidade Docente.");
                            }

                            atribuicao.doc_id = entDocente.doc_id;
                        }

                        if (atribuicao.usu_id != Guid.Empty)
                        {
                            string parametroPerfilDocente = ACA_ParametroAcademicoBO.ParametroValorPorEntidade
                                (eChaveAcademico.PAR_GRUPO_PERFIL_DOCENTE, ent_id);

                            // Buscar os grupos de docente que devem ser incluídos para o usuário.
                            DataTable dtGrupos = SYS_ParametroGrupoPerfilBO.GetSelect_gru_idBy_pgs_chave
                                (parametroPerfilDocente, false, 1, 1);

                            foreach (DataRow dr in dtGrupos.Rows)
                            {
                                SYS_UsuarioGrupo usg = new SYS_UsuarioGrupo
                                {
                                    usu_id = atribuicao.usu_id,
                                    usg_situacao = 1,
                                    gru_id = new Guid(dr["gru_id"].ToString())
                                };
                                SYS_UsuarioGrupoBO.Save(usg, bancoCore);
                            }
                        }

                        // Limpar cache para a tela de atribuição de docentes.
                        GestaoEscolarUtilBO.LimpaCache(ESC_UnidadeEscolaBO.RetornaChaveCache_SelectEscolas_VisaoIndividual(atribuicao.doc_id, ent_id, 1));
                        GestaoEscolarUtilBO.LimpaCache(ESC_UnidadeEscolaBO.RetornaChaveCache_SelectEscolas_VisaoIndividual(atribuicao.doc_id, ent_id, 0));
                        GestaoEscolarUtilBO.LimpaCache(ESC_UnidadeEscolaBO.RetornaChaveCache_SelectEscolas_VisaoIndividual(atribuicao.doc_id, ent_id, 2));
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    if (banco.ConnectionIsOpen)
                    {
                        banco.Close(ex);
                    }

                    if (bancoCore.ConnectionIsOpen)
                    {
                        bancoCore.Close(ex);
                    }

                    throw ex;
                }
                finally
                {
                    if (banco.ConnectionIsOpen)
                    {
                        banco.Close();
                    }

                    if (bancoCore.ConnectionIsOpen)
                    {
                        bancoCore.Close();
                    }
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Salva os dados passados pela API
        /// </summary>
        /// <param name="entityPessoa"></param>
        /// <param name="entityColaborador"></param>
        /// <param name="entityDocente"></param>
        /// <param name="documentos"></param>
        /// <param name="lstColaboradorCargo"></param>
        /// <param name="lstColaboradorCargoDisciplina"></param>
        /// <param name="lstColaboradorFuncao"></param>
        /// <param name="bancoGestao"></param>
        /// <param name="bancoCore"></param>
        public static void SaveAPI
            (
                PES_Pessoa entityPessoa
                , RHU_Colaborador entityColaborador
                , ACA_Docente entityDocente
                , List<PES_PessoaDocumento> documentos
                , List<RHU_ColaboradorCargoDTO> lstColaboradorCargo
                , List<RHU_ColaboradorFuncao> lstColaboradorFuncao
                , TalkDBTransaction bancoGestao = null
                , TalkDBTransaction bancoCore = null
            )
        {
            RHU_ColaboradorDAO colDao = new RHU_ColaboradorDAO();
            PES_PessoaDAO pesDao = new PES_PessoaDAO();
            if (bancoGestao == null)
                colDao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                colDao._Banco = bancoGestao;
            if (bancoCore == null)
                pesDao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                pesDao._Banco = bancoCore;

            try
            {
                if (!lstColaboradorCargo.Any() && !lstColaboradorFuncao.Any())
                    throw new ValidationException("É obrigatório o preenchimento de pelo menos um vínculo de trabalho do colaborador.");

                if (entityColaborador.IsNew)
                {
                    //Incrementa um na integridade da entidade
                    SYS_EntidadeDAO entDao = new SYS_EntidadeDAO { _Banco = pesDao._Banco };
                    entDao.Update_IncrementaIntegridade(entityColaborador.ent_id);

                    //Incrementa um na integridade da pessoa
                    pesDao.Update_IncrementaIntegridade(entityColaborador.pes_id);
                }

                entityPessoa.IsNew = entityPessoa.pes_id == new Guid();

                if (!documentos.Any(p => p.psd_situacao == 1 &&
                                         p.tdo_id == new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF))))
                    throw new ValidationException("Documentação do tipo CPF é obrigatório.");

                //Verifica se os dados da pessoa serão sempre salvos em maiúsculo.
                string sSalvarMaiusculo = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.SALVAR_SEMPRE_MAIUSCULO);
                bool Salvar_Sempre_Maiusculo = !string.IsNullOrEmpty(sSalvarMaiusculo) && Convert.ToBoolean(sSalvarMaiusculo);

                DataTable dtDocumento = new DataTable();

                dtDocumento.Columns.Add("tdo_id");
                dtDocumento.Columns.Add("unf_idEmissao");
                dtDocumento.Columns.Add("unf_idAntigo");
                dtDocumento.Columns.Add("tdo_nome");
                dtDocumento.Columns.Add("numero");
                dtDocumento.Columns.Add("dataemissao");
                dtDocumento.Columns.Add("orgaoemissao");
                dtDocumento.Columns.Add("unf_nome");
                dtDocumento.Columns.Add("info");
                dtDocumento.Columns.Add("banco", typeof(Boolean));
                dtDocumento.Columns.Add("excluido", typeof(Boolean));

                foreach (PES_PessoaDocumento documento in documentos)
                {
                    DataRow rowDoc = dtDocumento.NewRow();

                    rowDoc["tdo_id"] = documento.tdo_id;
                    rowDoc["unf_idEmissao"] = documento.unf_idEmissao;
                    rowDoc["unf_idAntigo"] = Guid.Empty;
                    rowDoc["tdo_nome"] = "";
                    rowDoc["numero"] = documento.psd_numero;
                    rowDoc["dataemissao"] = documento.psd_dataEmissao;
                    rowDoc["orgaoemissao"] = documento.psd_orgaoEmissao;
                    rowDoc["unf_nome"] = "";
                    rowDoc["info"] = documento.psd_infoComplementares;
                    rowDoc["banco"] = !documento.IsNew;
                    rowDoc["excluido"] = documento.psd_situacao == 3;
                    if (documento.psd_situacao == 3)
                        rowDoc.Delete();

                    string msg = "";

                    //Valida o tipo de documento.
                    if (documento.tdo_id != Guid.Empty && documento.psd_situacao == 1)
                    {
                        SYS_TipoDocumentacao tdo = new SYS_TipoDocumentacao { tdo_id = documento.tdo_id };

                        //Valida o número do documento.
                        if (!String.IsNullOrEmpty(documento.psd_numero))
                        {
                            SYS_TipoDocumentacaoBO.GetEntity(tdo);

                            if (tdo.tdo_validacao == 1)
                            {
                                if (!UtilBO._ValidaCPF(documento.psd_numero))
                                    msg += "Número inválido para CPF.</br>";
                            }

                            if (tdo.tdo_validacao == 2)
                            {
                                Regex regex = new Regex("^[0-9]+$");
                                if (!regex.IsMatch(documento.psd_numero))
                                    msg += "Número inválido para este documento.</br>";
                            }

                            string docPadraoCPF = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF);
                            if (tdo.tdo_id == new Guid(docPadraoCPF))
                            {
                                if (PES_PessoaDocumentoBO.VerificaDocumentoExistente(documento.psd_numero, entityPessoa.pes_id))
                                    msg += "Já existe uma pessoa cadastrada com esse documento (" + tdo.tdo_nome + ").</br>";
                            }
                        }
                        else
                            msg += "Número é obrigatório.</br>";
                    }
                    else
                        msg += "Tipo de documento é obrigatório.</br>";

                    // Valida se já existe o documentos do mesmo tipo cadastrados.
                    var x = from DataRow row in dtDocumento.Rows
                            where
                                row["tdo_id"].ToString() == rowDoc["tdo_id"].ToString()
                                && !String.IsNullOrEmpty(documento.psd_numero)
                                && documento.psd_situacao == 1
                                && Convert.ToByte(row["psd_situacao"]) == 1

                            select row;

                    if (x.Count() > 1)
                        msg += "Existem documentos cadastrados com mesmo tipo.";

                    if (!string.IsNullOrEmpty(msg))
                        throw new ValidationException(msg);

                    dtDocumento.Rows.Add(rowDoc);
                }

                for (int index = 0; index < dtDocumento.Rows.Count; index++)
                {
                    if (Convert.ToBoolean(dtDocumento.Rows[index]["excluido"]))
                    {
                        // Se não for do banco e estiver excluído, remove o registro do dtDocumento.
                        if (!(Convert.ToBoolean(dtDocumento.Rows[index]["banco"])))
                        {
                            dtDocumento.Rows.Remove(dtDocumento.Rows[index]);
                            // Diminui o índice pois foi removida uma linha.
                            index--;
                        }
                        else
                        {
                            // Força o rowstate a ficar como deleted.
                            dtDocumento.Rows[index].AcceptChanges();
                            dtDocumento.Rows[index].Delete();
                        }
                    }
                    else if (Convert.ToBoolean(dtDocumento.Rows[index]["banco"]))
                    {
                        // Força o rowstate a ficar como modified.
                        dtDocumento.Rows[index].AcceptChanges();
                        dtDocumento.Rows[index].SetModified();
                    }
                }

                DataTable dtEndereco = new DataTable();
                DataTable dtContato = new DataTable();
                PES_CertidaoCivil entityCertidaoCivil = new PES_CertidaoCivil();
                PES_PessoaDeficiencia pesDeficiencia = new PES_PessoaDeficiencia();

                if (entityPessoa.pes_id != Guid.Empty)
                {
                    DataTable dtPessoaDeficiencia = PES_PessoaDeficienciaBO.GetSelect(entityPessoa.pes_id, false, 1, 1);
                    if (dtPessoaDeficiencia.Rows.Count > 0)
                    {
                        pesDeficiencia.pes_id = entityPessoa.pes_id;
                        pesDeficiencia.tde_id = new Guid(dtPessoaDeficiencia.Rows[0]["tde_id"].ToString());
                        pesDeficiencia.IsNew = false;
                    }

                    dtEndereco.Columns.Add("cpr_id");
                    dtEndereco.Columns.Add("id");
                    dtEndereco.Columns.Add("end_id");
                    dtEndereco.Columns.Add("end_cep");
                    dtEndereco.Columns.Add("end_logradouro");
                    dtEndereco.Columns.Add("end_distrito");
                    dtEndereco.Columns.Add("end_zona");
                    dtEndereco.Columns.Add("end_bairro");
                    dtEndereco.Columns.Add("cid_id");
                    dtEndereco.Columns.Add("cid_nome");
                    dtEndereco.Columns.Add("numero");
                    dtEndereco.Columns.Add("complemento");
                    dtEndereco.Columns.Add("banco", typeof(Boolean));
                    dtEndereco.Columns.Add("excluido", typeof(Boolean));
                    dtEndereco.Columns.Add("principal", typeof(Boolean));

                    DataTable dtEnderecoBanco = PES_PessoaEnderecoBO.GetSelect(entityPessoa.pes_id, false, 1, 1);
                    foreach (DataRow item in dtEnderecoBanco.Rows)
                    {
                        DataRow dr = dtEndereco.NewRow();

                        dr["end_id"] = item["end_id"];

                        dr["id"] = item["end_id"];

                        dr["end_cep"] = item["end_ced"];
                        dr["end_logradouro"] = item["end_logradouro"];
                        dr["end_distrito"] = item["end_distrito"];

                        dr["end_zona"] = item["end_zona"];

                        dr["end_bairro"] = item["end_bairro"];
                        dr["cid_id"] = item["cid_id"];
                        dr["cid_nome"] = item["cid_nome"];
                        dr["numero"] = item["numero"];
                        dr["complemento"] = item["complemento"];

                        dr["banco"] = true;
                        dr["excluido"] = false;

                        dr["principal"] = item["principal"];

                        dtEndereco.Rows.Add(dr);
                    }

                    List<PES_CertidaoCivil> lt = PES_CertidaoCivilBO.SelecionaPorPessoa(entityPessoa.pes_id);

                    if (lt.Count > 0)
                    {
                        entityCertidaoCivil = (lt.Select(p => p.ctc_tipo).Distinct().Count() > 1
                                                   ? lt.Find(
                                                       p =>
                                                       p.ctc_tipo ==
                                                       Convert.ToByte(PES_CertidaoCivilBO.TipoCertidaoCivil.Casamento))
                                                   : lt.FirstOrDefault());
                    }

                    dtContato.Columns.Add("tmc_id");
                    dtContato.Columns.Add("tmc_validacao");
                    dtContato.Columns.Add("tmc_nome");
                    dtContato.Columns.Add("contato");
                    dtContato.Columns.Add("id");
                    dtContato.Columns.Add("banco");

                    DataTable dtContatoBanco = SYS_TipoMeioContatoBO.SelecionaContatosDaPessoa(entityPessoa.pes_id);

                    foreach (DataRow ri in dtContatoBanco.Rows)
                    {
                        if (!string.IsNullOrEmpty(ri["contato"].ToString()))
                        {
                            DataRow dr = dtContato.NewRow();
                            dr["tmc_id"] = ri["tmc_id"];
                            dr["tmc_validacao"] = ri["tmc_validacao"];
                            dr["tmc_nome"] = ri["tmc_nome"];
                            dr["contato"] = ri["contato"];
                            dr["id"] = ri["id"];
                            dr["banco"] = true;
                            dtContato.Rows.Add(dr);
                        }
                    }
                }

                if (entityPessoa.pes_situacao > 0 && !PES_PessoaBO.Save(entityPessoa, pesDeficiencia,
                                                                        entityCertidaoCivil, dtEndereco,
                                                                        dtContato, dtDocumento, pesDao._Banco))
                    throw new ValidationException("Erro ao salvar entidade de pessoa.");

                entityColaborador.pes_id = entityPessoa.pes_id;
                entityColaborador.IsNew = entityColaborador.col_id <= 0;

                if (entityColaborador.col_situacao > 0 && !RHU_ColaboradorBO.Save(entityColaborador, colDao._Banco))
                    throw new ValidationException("Erro ao salvar entidade de colaborador.");

                entityDocente.col_id = entityColaborador.col_id;
                entityDocente.IsNew = entityDocente.doc_id <= 0;

                if (entityDocente.doc_dataCriacao != new DateTime() && entityDocente.doc_situacao > 0 &&
                    !ACA_DocenteBO.Save(entityDocente, colDao._Banco))
                    throw new ValidationException("Erro ao salvar entidade de docente.");

                DataTable dtCargoFuncao = RHU_ColaboradorCargoBO.GetSelect(entityColaborador.col_id, false, 1, 1, entityColaborador.ent_id, colDao._Banco);
                foreach (DataRow row in dtCargoFuncao.Rows)
                {
                    if (row["crg_id"] != null && !string.IsNullOrEmpty(row["crg_id"].ToString()) &&
                        lstColaboradorCargo.Any(p => p.coc_id != Convert.ToInt32(row["seqcrg_id"]) && p.crg_id == Convert.ToInt32(row["crg_id"]) &&
                                                     p.uad_id == new Guid(row["uad_id"].ToString()) &&
                                                     p.coc_matricula.Equals(row["coc_matricula"].ToString()) &&
                                                     p.coc_complementacaoCargaHoraria == Convert.ToBoolean(row["coc_complementacaoCargaHoraria"]) &&
                                                     p.coc_situacao == (byte)RHU_ColaboradorCargoSituacao.Ativo &&
                                                     Convert.ToByte(row["situacao_id"].ToString()) == (byte)RHU_ColaboradorCargoSituacao.Ativo))
                        throw new ValidationException("Não é permitido cadastrar mais de um vínculo para o mesmo cargo na mesma escola com matrícula igual.");
                    if (row["fun_id"] != null && !string.IsNullOrEmpty(row["fun_id"].ToString()) &&
                        lstColaboradorFuncao.Any(p => p.cof_id != Convert.ToInt32(row["seqfun_id"]) && p.fun_id == Convert.ToInt32(row["fun_id"]) &&
                                                      p.uad_id == new Guid(row["uad_id"].ToString()) &&
                                                      p.cof_matricula.Equals(row["cof_matricula"].ToString()) &&
                                                      p.cof_situacao == (byte)RHU_ColaboradorFuncaoSituacao.Ativo &&
                                                      Convert.ToByte(row["situacao_id"].ToString()) == (byte)RHU_ColaboradorFuncaoSituacao.Ativo))
                        throw new ValidationException("Não é permitido cadastrar mais de um vínculo para a mesma função na mesma escola com matrícula igual.");
                }

                if (!lstColaboradorCargo.Any(p => p.coc_situacao != (byte)RHU_ColaboradorCargoSituacao.Excluido) &&
                    !dtCargoFuncao.AsEnumerable().Any(row => row["crg_id"] != null && !string.IsNullOrEmpty(row["crg_id"].ToString()) &&
                                                     Convert.ToByte(row["situacao_id"].ToString()) != (byte)RHU_ColaboradorCargoSituacao.Excluido &&
                                                     !lstColaboradorCargo.Any(p => p.coc_id == Convert.ToInt32(row["seqcrg_id"]) && p.crg_id == Convert.ToInt32(row["crg_id"]))) &&
                    !lstColaboradorFuncao.Any(p => p.cof_situacao != (byte)RHU_ColaboradorFuncaoSituacao.Excluido) &&
                    !dtCargoFuncao.AsEnumerable().Any(row => row["fun_id"] != null && !string.IsNullOrEmpty(row["fun_id"].ToString()) &&
                                                     Convert.ToByte(row["situacao_id"].ToString()) != (byte)RHU_ColaboradorCargoSituacao.Excluido &&
                                                     !lstColaboradorFuncao.Any(p => p.cof_id == Convert.ToInt32(row["seqfun_id"]) && p.fun_id == Convert.ToInt32(row["fun_id"]))))
                    throw new ValidationException("É necessário que o colaborador tenha pelo menos um cargo ou função não excluídos.");


                DataTable dtColCrgDisc = RHU_ColaboradorCargoDisciplinaBO.GetSelectBy_Colaborador(entityColaborador.col_id, colDao._Banco);
                foreach (DataRow row in dtColCrgDisc.Rows)
                {
                    RHU_ColaboradorCargoDisciplina colCrgDisc = new RHU_ColaboradorCargoDisciplina
                    {
                        col_id = entityColaborador.col_id,
                        crg_id = Convert.ToInt32(row["crg_id"]),
                        coc_id = Convert.ToInt32(row["coc_id"]),
                        tds_id = Convert.ToInt32(row["tds_id"]),
                        IsNew = false
                    };
                    RHU_ColaboradorCargoDisciplinaBO.Delete(colCrgDisc, colDao._Banco);
                }

                foreach (RHU_ColaboradorCargoDTO entityColaboradorCargo in lstColaboradorCargo)
                {
                    if (entityColaboradorCargo.coc_situacao != (byte)RHU_ColaboradorCargoSituacao.Excluido)
                    {
                        if (entityColaboradorCargo.coc_vigenciaFim != new DateTime() &&
                            entityColaboradorCargo.coc_vigenciaFim.Date < DateTime.Now.Date)
                            entityColaboradorCargo.coc_situacao = (byte)RHU_ColaboradorCargoSituacao.Desativado;

                        if (entityColaboradorCargo.coc_dataInicioMatricula != new DateTime() &&
                            entityColaboradorCargo.coc_dataInicioMatricula.Date < entityColaborador.col_dataAdmissao)
                            throw new ValidationException("Data de início da matrícula não pode ser menor que a data de admissão.");

                        if (entityColaboradorCargo.coc_vigenciaInicio < entityColaborador.col_dataAdmissao)
                            throw new ValidationException("Data de admissão não pode ser maior que vigência inicial em nenhum cargo.");

                        if (entityColaboradorCargo.coc_vigenciaFim != new DateTime() && entityColaboradorCargo.coc_vigenciaFim < entityColaborador.col_dataAdmissao)
                            throw new ValidationException("Data de admissão não pode ser maior que vigência final em nenhum cargo.");

                        if (entityColaborador.col_dataDemissao != new DateTime())
                        {
                            if (entityColaboradorCargo.coc_vigenciaInicio > entityColaborador.col_dataDemissao)
                                throw new ValidationException("Data de demissão não pode ser menor que vigência inicial em nenhum cargo.");

                            if (entityColaboradorCargo.coc_vigenciaFim != new DateTime() && entityColaboradorCargo.coc_vigenciaFim > entityColaborador.col_dataDemissao)
                                throw new ValidationException("Data de demissão não pode ser menor que a vigência final em nenhum cargo.");
                        }

                        if (RHU_ColaboradorBO.VerificaMatriculaExistente(entityColaborador.col_id, entityColaboradorCargo.coc_matricula, entityColaborador.ent_id, colDao._Banco))
                            throw new ValidationException(string.Format("Outro docente/colaborador já possui um vínculo com a matrícula {0}.", entityColaboradorCargo.coc_matricula));

                        if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_VINCULO_INTEGRADO_VIRTUAL, entityColaborador.ent_id) &&
                            entityColaboradorCargo.coc_vinculoSede && entityColaboradorCargo.coc_complementacaoCargaHoraria)
                            throw new ValidationException(string.Format("Não é permitido selecionar as opções 'Vínculo sede' e 'Complementação de carga horária' no mesmo vínculo da matrícula {0}.", entityColaboradorCargo.coc_matricula));
                    }

                    entityColaboradorCargo.coc_observacao = Salvar_Sempre_Maiusculo ?
                        (entityColaboradorCargo.coc_observacao ?? "").ToUpper() : (entityColaboradorCargo.coc_observacao ?? "");
                    entityColaboradorCargo.coc_matricula = Salvar_Sempre_Maiusculo ?
                        (entityColaboradorCargo.coc_matricula ?? "").ToUpper() : (entityColaboradorCargo.coc_matricula ?? "");

                    entityColaboradorCargo.col_id = entityColaborador.col_id;
                    entityColaboradorCargo.IsNew = entityColaboradorCargo.coc_id <= 0;

                    RHU_ColaboradorCargo entityColaboradorCargoSalvar = (RHU_ColaboradorCargo)entityColaboradorCargo;

                    if (!RHU_ColaboradorCargoBO.Save(entityColaboradorCargoSalvar, colDao._Banco))
                        throw new ValidationException("Erro ao salvar entidade de colaborador cargo.");

                    if (entityColaboradorCargoSalvar.coc_situacao == (byte)RHU_ColaboradorCargoSituacao.Excluido)
                        // Atualiza a ligação do colaborador docente com a(s) disciplina(s) da turma(s)
                        TUR_TurmaDocenteBO.AtualizarTurmaDocentePorColaboradorDocente(entityColaborador.col_id, entityColaboradorCargo.crg_id, entityColaboradorCargoSalvar.coc_id, 0, entityColaboradorCargo.ent_id, entityColaboradorCargo.uad_id, colDao._Banco);

                    foreach (RHU_ColaboradorCargoDisciplina entityColaboradorCargoDisc in entityColaboradorCargo.colaboradorCargoDisciplina)
                    {
                        if (entityColaboradorCargoDisc.tds_id > 0)
                        {
                            entityColaboradorCargoDisc.col_id = entityColaborador.col_id;
                            entityColaboradorCargoDisc.coc_id = entityColaboradorCargoSalvar.coc_id;
                            entityColaboradorCargoDisc.IsNew = true;

                            if (!RHU_ColaboradorCargoDisciplinaBO.Save(entityColaboradorCargoDisc, colDao._Banco))
                                throw new ValidationException("Erro ao salvar entidade de colaborador função.");

                        }
                    }
                }

                foreach (RHU_ColaboradorFuncao entityColaboradorFuncao in lstColaboradorFuncao)
                {
                    if (entityColaboradorFuncao.cof_situacao != (byte)RHU_ColaboradorFuncaoSituacao.Excluido)
                    {
                        if (entityColaboradorFuncao.cof_vigenciaFim.Date < DateTime.Now.Date)
                            entityColaboradorFuncao.cof_situacao = (byte)RHU_ColaboradorFuncaoSituacao.Desativado;

                        if (entityColaboradorFuncao.cof_vigenciaInicio < entityColaborador.col_dataAdmissao)
                            throw new ValidationException("Data de admissão não pode ser maior que vigência inicial em nenhuma função.");

                        if (entityColaboradorFuncao.cof_vigenciaFim != new DateTime() && entityColaboradorFuncao.cof_vigenciaFim < entityColaborador.col_dataAdmissao)
                            throw new ValidationException("Data de admissão não pode ser maior que vigência final em nenhuma função.");

                        if (entityColaborador.col_dataDemissao != new DateTime())
                        {
                            if (entityColaboradorFuncao.cof_vigenciaInicio > entityColaborador.col_dataDemissao)
                                throw new ValidationException("Data de demissão não pode ser menor que vigência inicial em nenhuma função.");

                            if (entityColaboradorFuncao.cof_vigenciaFim != new DateTime() && entityColaboradorFuncao.cof_vigenciaFim > entityColaborador.col_dataDemissao)
                                throw new ValidationException("Data de demissão não pode ser menor que a vigência final em nenhuma função.");
                        }

                        if (RHU_ColaboradorBO.VerificaMatriculaExistente(entityColaborador.col_id, entityColaboradorFuncao.cof_matricula, entityColaborador.ent_id, colDao._Banco))
                            throw new ValidationException(string.Format("Outro docente/colaborador já possui um vínculo com a matrícula {0}.", entityColaboradorFuncao.cof_matricula));
                    }

                    entityColaboradorFuncao.cof_observacao = Salvar_Sempre_Maiusculo ?
                        (entityColaboradorFuncao.cof_observacao ?? "").ToUpper() : (entityColaboradorFuncao.cof_observacao ?? "");
                    entityColaboradorFuncao.cof_matricula = Salvar_Sempre_Maiusculo ?
                        (entityColaboradorFuncao.cof_matricula ?? "").ToUpper() : (entityColaboradorFuncao.cof_matricula ?? "");

                    entityColaboradorFuncao.col_id = entityColaborador.col_id;
                    entityColaboradorFuncao.IsNew = entityColaboradorFuncao.cof_id <= 0;

                    if (!RHU_ColaboradorFuncaoBO.Save(entityColaboradorFuncao, colDao._Banco))
                        throw new ValidationException("Erro ao salvar entidade de colaborador função.");
                }
            }
            catch (Exception err)
            {
                if (colDao._Banco.ConnectionIsOpen && bancoGestao == null)
                    colDao._Banco.Close(err);
                if (pesDao._Banco.ConnectionIsOpen && bancoCore == null)
                    pesDao._Banco.Close(err);

                throw;
            }
            finally
            {
                if (colDao._Banco.ConnectionIsOpen && bancoGestao == null)
                    colDao._Banco.Close();
                if (pesDao._Banco.ConnectionIsOpen && bancoCore == null)
                    pesDao._Banco.Close();
            }
        }
        
        /// <summary>
        /// Inclui ou altera um colaborador
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
        /// <param name="bSalvarUserLive"></param>
        /// <param name="entityUsuario">Entidade SYS_Usuario</param>
        /// <param name="entityUserLive"></param>
        /// <param name="sPadraoUsuario">String de busca genérica para inclusão dos grupos do usuário automaticamente</param>
        /// <param name="bEnviaEmail">Bool se envia e-mail</param>
        /// <param name="sNomePortal">String com o nome do portal para e-mail</param>
        /// <param name="sHost">String com o host para e-mail</param>
        /// <param name="sEmailSuporte">String com o endereço do e-mail de suporte</param>
        /// <param name="ent_idUsuario"></param>
        /// <param name="bancoCore">Conexão aberta com o banco de dados Core ou null para uma nova conexão</param>
        /// <param name="bancoEscolar">Conexão aberta com o banco de dados Escolar ou null para uma nova conexão</param>
        /// <param name="arquivosPermitidos">Extensões de tipos de arquivos permitidos para a foto</param>
        /// <param name="tamanhoMaximoKB">Tamanho máximo da foto permitida</param>
        /// <param name="entFoto">Entidade da foto da pessoa</param>
        /// <param name="ExcluirImagemAtual">Indica se imagem atual será excluída</param>
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
            , bool bSalvarUserLive
            , SYS_Usuario entityUsuario
            , UserLive entityUserLive
            , string sPadraoUsuario
            , bool bEnviaEmail
            , string sNomePortal
            , string sHost
            , string sEmailSuporte
            , Guid ent_idUsuario
            , TalkDBTransaction bancoCore
            , TalkDBTransaction bancoEscolar
            , string[] arquivosPermitidos
            , int tamanhoMaximoKB
            , CFG_Arquivo entFoto
            , bool ExcluirImagemAtual
        )
        {
            for (int i = 0; i < dtCargoFuncao.Rows.Count; i++)
            {
                if (dtCargoFuncao.Rows[i].RowState != DataRowState.Deleted)
                {
                    if (Convert.ToDateTime(dtCargoFuncao.Rows[i]["vigenciaini"].ToString()) < entityColaborador.col_dataAdmissao)
                        throw new ArgumentException("Data de admissão não pode ser maior que vigência inicial em nenhum cargo ou função.");

                    if (dtCargoFuncao.Rows[i]["vigenciafim"].ToString() != string.Empty && Convert.ToDateTime(dtCargoFuncao.Rows[i]["vigenciafim"].ToString()) < entityColaborador.col_dataAdmissao)
                        throw new ArgumentException("Data de admissão não pode ser maior que vigência final em nenhum cargo ou função.");

                    if (entityColaborador.col_dataDemissao != new DateTime())
                    {
                        if (Convert.ToDateTime(dtCargoFuncao.Rows[i]["vigenciaini"].ToString()) > entityColaborador.col_dataDemissao)
                            throw new ArgumentException("Data de demissão não pode ser menor que vigência inicial em nenhum cargo ou função.");

                        if (dtCargoFuncao.Rows[i]["vigenciafim"].ToString() != string.Empty && Convert.ToDateTime(dtCargoFuncao.Rows[i]["vigenciafim"].ToString()) > entityColaborador.col_dataDemissao)
                            throw new ArgumentException("Data de demissão não pode ser menor que a vigência final em nenhum cargo ou função.");
                    }
                }
            }

            var y = from DataRow dr in dtCargoFuncao.Rows
                    where dr.RowState == DataRowState.Deleted
                    select dr;

            if (dtCargoFuncao.Rows.Count == 0 || y.Count() == dtCargoFuncao.Rows.Count)
                throw new ValidationException("É obrigatório o preenchimento de pelo menos um vínculo de trabalho do colaborador.");

            bool permissoes_colaborador;

            if (!string.IsNullOrEmpty(ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.CONTROLAR_PERMISSAO_COLABORADOR_CARGO, ent_idUsuario)))

                permissoes_colaborador = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_PERMISSAO_COLABORADOR_CARGO, ent_idUsuario);
            else
                permissoes_colaborador = true;

            RHU_ColaboradorDAO colDao = new RHU_ColaboradorDAO();
            PES_PessoaDAO pesDao = new PES_PessoaDAO();

            if (bancoEscolar == null)
            {
                colDao._Banco.Open(IsolationLevel.ReadCommitted);
                pesDao._Banco.Open(IsolationLevel.ReadCommitted);
            }
            else
            {
                colDao._Banco = bancoEscolar;
                pesDao._Banco = bancoCore;
            }

            try
            {
                //Verifica se os dados da pessoa serão sempre salvos em maiúsculo.
                string sSalvarMaiusculo = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.SALVAR_SEMPRE_MAIUSCULO);
                bool Salvar_Sempre_Maiusculo = !string.IsNullOrEmpty(sSalvarMaiusculo) && Convert.ToBoolean(sSalvarMaiusculo);

                Guid pes_idFiliacaoPai;
                Guid pes_idFiliacaoMae;
                SalvarPaiMae(entityPessoa, ref PaiAntigo, ref MaeAntigo, Salvar_Sempre_Maiusculo, out pes_idFiliacaoPai, out pes_idFiliacaoMae, pesDao._Banco);

                foreach (DataRow row in dtEndereco.Rows)
                {
                    if (Convert.ToBoolean(row["excluido"]))
                        row.Delete();
                }

                // Salva os dados da Pessoa.
                PES_PessoaBO.Save(entityPessoa
                                  , entityPessoaDeficiencia
                                  , entityCertidaoCivil
                                  , dtEndereco
                                  , dtContato
                                  , dtDocumento
                                  , pesDao._Banco
                                  , arquivosPermitidos
                                  , tamanhoMaximoKB
                                  , entFoto
                                  , ExcluirImagemAtual
                    );

                //se a data de demissão for menor que a data atual, seta a situação como "demitido"
                if (entityColaborador.col_dataDemissao != new DateTime() && entityColaborador.col_dataDemissao.Date < DateTime.Now.Date)
                    entityColaborador.col_situacao = (byte)RHU_ColaboradorSituacao.Demitido;

                // Salva os dados na tabela RHU_Colaborador.
                entityColaborador.pes_id = entityPessoa.pes_id;
                if (entityColaborador.Validate())
                {
                    if (entityColaborador.col_dataDemissao != new DateTime())
                    {
                        if (entityColaborador.col_dataAdmissao > entityColaborador.col_dataDemissao)
                            throw new ArgumentException("Data de admissão não pode ser maior que a data de demissão.");
                    }

                    colDao.Salvar(entityColaborador);
                }
                else
                {
                    throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entityColaborador));
                }

                // Salva dados na tabela de usuários se necessário
                if (entityUsuario != null)
                {
                    entityUsuario.pes_id = entityPessoa.pes_id;
                    if (bSalvarUsuario)
                    {
                        // Deleta as ligações anteriores dos grupos do usuário do colaborador
                        // Verifica se o parâmetro CONTROLAR_PERMISSAO_COLABORADOR_CARGO for verdadeiro
                        if (entityUsuario.usu_id != Guid.Empty && permissoes_colaborador)
                            SYS_UsuarioGrupoBO.DeletarPorUsuario(entityUsuario.usu_id, pesDao._Banco);

                        if ((RHU_ColaboradorSituacao)entityColaborador.col_situacao == RHU_ColaboradorSituacao.Ativo)
                        {
                            if (SYS_UsuarioBO.Save(entityUsuario, entityUserLive, bSalvarUserLive, sPadraoUsuario, bEnviaEmail, entityPessoa.pes_nome, sNomePortal, sHost, sEmailSuporte, ent_idUsuario, pesDao._Banco))
                            {
                                if (entityUsuario.IsNew)
                                {
                                    // Incrementa um na integridade do usuario
                                    SYS_UsuarioBO.IncrementaIntegridade(entityUsuario.usu_id, pesDao._Banco);
                                }
                            }

                            // Inclui os grupos do usuário de acordo com o sPadraoUsuario
                            DataTable dtGrupos = SYS_ParametroGrupoPerfilBO.GetSelect_gru_idBy_pgs_chave(sPadraoUsuario, false, 1, 1, pesDao._Banco);

                            for (int i = 0; i < dtGrupos.Rows.Count; i++)
                            {
                                SYS_UsuarioGrupo usg = new SYS_UsuarioGrupo
                                {
                                    usu_id = entityUsuario.usu_id,
                                    usg_situacao = 1,
                                    gru_id = new Guid(dtGrupos.Rows[i]["gru_id"].ToString())
                                };
                                SYS_UsuarioGrupoBO.Save(usg, pesDao._Banco);
                            }
                        }
                        else if ((RHU_ColaboradorSituacao)entityColaborador.col_situacao == RHU_ColaboradorSituacao.Demitido)
                        {
                            PES_Pessoa pes = new PES_Pessoa { pes_id = entityPessoa.pes_id };
                            PES_PessoaBO.GetEntity(pes, pesDao._Banco);

                            if (pes.pes_integridade <= 2)
                            {
                                entityUsuario.usu_situacao = 2;
                            }

                            SYS_UsuarioBO.Save(entityUsuario, entityUserLive, bSalvarUserLive, sPadraoUsuario, bEnviaEmail, entityPessoa.pes_nome, sNomePortal, sHost, sEmailSuporte, ent_idUsuario, pesDao._Banco);

                            // Exclui logicamente os grupos do usuário de acordo com o sPadraoUsuario
                            DataTable dtGrupos = SYS_ParametroGrupoPerfilBO.GetSelect_gru_idBy_pgs_chave
                                (sPadraoUsuario, false, 1, 1, pesDao._Banco);
                            for (int i = 0; i < dtGrupos.Rows.Count; i++)
                            {
                                SYS_UsuarioGrupo usg = new SYS_UsuarioGrupo
                                {
                                    usu_id = entityUsuario.usu_id,
                                    gru_id = new Guid(dtGrupos.Rows[i]["gru_id"].ToString())
                                };
                                SYS_UsuarioGrupoBO.Delete(usg, pesDao._Banco);
                            }
                        }
                        else
                        {
                            SYS_UsuarioBO.Save(entityUsuario, entityUserLive, bSalvarUserLive, sPadraoUsuario, bEnviaEmail, entityPessoa.pes_nome, sNomePortal, sHost, sEmailSuporte, ent_idUsuario, pesDao._Banco);

                            // Bloqueia os grupos do usuário de acordo com o sPadraoUsuario
                            DataTable dtGrupos = SYS_ParametroGrupoPerfilBO.GetSelect_gru_idBy_pgs_chave(sPadraoUsuario, false, 1, 1, pesDao._Banco);

                            for (int i = 0; i < dtGrupos.Rows.Count; i++)
                            {
                                SYS_UsuarioGrupo usg = new SYS_UsuarioGrupo
                                {
                                    usu_id = entityUsuario.usu_id,
                                    usg_situacao = 2,
                                    gru_id = new Guid(dtGrupos.Rows[i]["gru_id"].ToString())
                                };
                                SYS_UsuarioGrupoBO.Save(usg, pesDao._Banco);
                            }
                        }
                    }
                    else
                    {
                        if (entityUsuario.usu_id != Guid.Empty)
                        {
                            // Decrementa um na integridade do usuario
                            SYS_UsuarioBO.DecrementaIntegridade(entityUsuario.usu_id, pesDao._Banco);
                            SYS_UsuarioBO.Delete(entityUsuario, pesDao._Banco);
                        }
                    }
                }

                // Salva os dados nas tabelas RHU_ColaboradorCargo e RHU_ColaboradorFuncao
                RHU_ColaboradorCargo entityColaboradorCargo = new RHU_ColaboradorCargo
                {
                    col_id = entityColaborador.col_id
                };

                RHU_ColaboradorCargoDisciplina entityColaboradorCargoDisciplina = new RHU_ColaboradorCargoDisciplina
                {
                    col_id = entityColaborador.col_id
                };

                RHU_ColaboradorFuncao entityColaboradorFuncao = new RHU_ColaboradorFuncao
                {
                    col_id = entityColaborador.col_id
                };

                for (int i = 0; i < dtCargoFuncao.Rows.Count; i++)
                {
                    if (dtCargoFuncao.Rows[i].RowState != DataRowState.Deleted)
                    {
                        if (!string.IsNullOrEmpty(dtCargoFuncao.Rows[i]["crg_id"].ToString()))
                        {
                            entityColaboradorCargo.coc_id = Convert.ToInt32(dtCargoFuncao.Rows[i]["seqcrg_id"].ToString());
                            entityColaboradorCargo.ent_id = entityColaborador.ent_id;
                            entityColaboradorCargo.crg_id = Convert.ToInt32(dtCargoFuncao.Rows[i]["crg_id"].ToString());
                            entityColaboradorCargo.coc_matricula = Salvar_Sempre_Maiusculo ? dtCargoFuncao.Rows[i]["coc_matricula"].ToString().ToUpper() : dtCargoFuncao.Rows[i]["coc_matricula"].ToString();
                            entityColaboradorCargo.uad_id = new Guid(dtCargoFuncao.Rows[i]["uad_id"].ToString());
                            entityColaboradorCargo.coc_vigenciaInicio = Convert.ToDateTime(dtCargoFuncao.Rows[i]["vigenciaini"].ToString());
                            entityColaboradorCargo.coc_situacao = Convert.ToByte(dtCargoFuncao.Rows[i]["situacao_id"].ToString());
                            entityColaboradorCargo.coc_observacao = Salvar_Sempre_Maiusculo ? dtCargoFuncao.Rows[i]["observacao"].ToString().ToUpper() : dtCargoFuncao.Rows[i]["observacao"].ToString();
                            entityColaboradorCargo.chr_id = String.IsNullOrEmpty(dtCargoFuncao.Rows[i]["chr_id"].ToString()) ? 0 : Convert.ToInt32(dtCargoFuncao.Rows[i]["chr_id"]);
                            entityColaboradorCargo.coc_vinculoSede = (dtCargoFuncao.Rows[i]["coc_vinculoSede"].ToString() == "True");
                            entityColaboradorCargo.coc_readaptado = (dtCargoFuncao.Rows[i]["coc_readaptado"].ToString() == "True");
                            entityColaboradorCargo.coc_vinculoExtra = (dtCargoFuncao.Rows[i]["coc_vinculoExtra"].ToString() == "True");
                            entityColaboradorCargo.coc_complementacaoCargaHoraria = (dtCargoFuncao.Rows[i]["coc_complementacaoCargaHoraria"].ToString() == "True");

                            if (!string.IsNullOrEmpty(dtCargoFuncao.Rows[i]["vigenciafim"].ToString()))
                            {
                                entityColaboradorCargo.coc_vigenciaFim = Convert.ToDateTime(dtCargoFuncao.Rows[i]["vigenciafim"].ToString());

                                if (entityColaboradorCargo.coc_vigenciaFim.Date < DateTime.Now.Date)
                                {
                                    entityColaboradorCargo.coc_situacao = (byte)RHU_ColaboradorCargoSituacao.Desativado;
                                }
                            }
                            else
                            {
                                entityColaboradorCargo.coc_vigenciaFim = new DateTime();
                            }

                            if (!string.IsNullOrEmpty(dtCargoFuncao.Rows[i]["coc_dataInicioMatricula"].ToString()))
                            {
                                entityColaboradorCargo.coc_dataInicioMatricula = Convert.ToDateTime(dtCargoFuncao.Rows[i]["coc_dataInicioMatricula"].ToString());

                                if (entityColaboradorCargo.coc_dataInicioMatricula.Date < entityColaborador.col_dataAdmissao)
                                {
                                    throw new ArgumentException("Data de início da matrícula não pode ser menor que a data de admissão.");
                                }
                            }
                            else
                            {
                                entityColaboradorCargo.coc_dataInicioMatricula = new DateTime();
                            }

                            if (dtCargoFuncao.Rows[i].RowState == DataRowState.Added)
                            {                                
                                entityColaboradorCargo.IsNew = true;
                                RHU_ColaboradorCargoBO.Save(entityColaboradorCargo, colDao._Banco);

                                // Salva os dados na tabela RHU_ColaboradorCargoDisciplina
                                // Na inclusão do cargo
                                for (int j = 0; j < dtCargoDisciplina.Rows.Count; j++)
                                {
                                    if (dtCargoDisciplina.Rows[j].RowState != DataRowState.Deleted)
                                    {
                                        if (dtCargoDisciplina.Rows[j].RowState == DataRowState.Added)
                                        {
                                            if (Convert.ToInt32(dtCargoFuncao.Rows[i]["crg_id"].ToString()) == Convert.ToInt32(dtCargoDisciplina.Rows[j]["crg_id"].ToString()) &&
                                                Convert.ToInt32(dtCargoFuncao.Rows[i]["seqcrg_id"].ToString()) == Convert.ToInt32(dtCargoDisciplina.Rows[j]["coc_id"].ToString()))
                                            {
                                                entityColaboradorCargoDisciplina.crg_id = entityColaboradorCargo.crg_id;
                                                entityColaboradorCargoDisciplina.coc_id = entityColaboradorCargo.coc_id;
                                                entityColaboradorCargoDisciplina.tds_id = Convert.ToInt32(dtCargoDisciplina.Rows[j]["tds_id"].ToString());

                                                RHU_ColaboradorCargoDisciplinaBO.Save(entityColaboradorCargoDisciplina, colDao._Banco);
                                            }
                                        }
                                    }
                                }
                                
                                // Incrementa um na integridade da entidade
                                SYS_EntidadeDAO entDao = new SYS_EntidadeDAO { _Banco = pesDao._Banco };
                                entDao.Update_IncrementaIntegridade(entityColaboradorCargo.ent_id);

                                // Incrementa um na integridade da unidade administrativa
                                SYS_UnidadeAdministrativaDAO uadDao = new SYS_UnidadeAdministrativaDAO { _Banco = pesDao._Banco };
                                uadDao.Update_IncrementaIntegridade(entityColaboradorCargo.ent_id, entityColaboradorCargo.uad_id);
                            }
                            else if (dtCargoFuncao.Rows[i].RowState == DataRowState.Modified)
                            {
                                entityColaboradorCargo.IsNew = false;
                                RHU_ColaboradorCargoBO.Save(entityColaboradorCargo, colDao._Banco);

                                // Salva os dados na tabela RHU_ColaboradorCargoDisciplina
                                // Caso tenha alteração no cargo
                                for (int j = 0; j < dtCargoDisciplina.Rows.Count; j++)
                                {
                                    if (dtCargoDisciplina.Rows[j].RowState != DataRowState.Deleted)
                                    {
                                        if (dtCargoDisciplina.Rows[j].RowState == DataRowState.Added)
                                        {
                                            if (Convert.ToInt32(dtCargoFuncao.Rows[i]["crg_id"].ToString()) == Convert.ToInt32(dtCargoDisciplina.Rows[j]["crg_id"].ToString()) &&
                                                Convert.ToInt32(dtCargoFuncao.Rows[i]["seqcrg_id"].ToString()) == Convert.ToInt32(dtCargoDisciplina.Rows[j]["coc_id"].ToString()))
                                            {
                                                entityColaboradorCargoDisciplina.crg_id = Convert.ToInt32(dtCargoDisciplina.Rows[j]["crg_id"].ToString());
                                                entityColaboradorCargoDisciplina.coc_id = Convert.ToInt32(dtCargoDisciplina.Rows[j]["coc_id"].ToString());
                                                entityColaboradorCargoDisciplina.tds_id = Convert.ToInt32(dtCargoDisciplina.Rows[j]["tds_id"].ToString());

                                                RHU_ColaboradorCargoDisciplinaBO.Save(entityColaboradorCargoDisciplina, colDao._Banco);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(dtCargoFuncao.Rows[i]["crg_id"].ToString()) == Convert.ToInt32(dtCargoDisciplina.Rows[j]["crg_id", DataRowVersion.Original].ToString()) &&
                                            Convert.ToInt32(dtCargoFuncao.Rows[i]["seqcrg_id"].ToString()) == Convert.ToInt32(dtCargoDisciplina.Rows[j]["coc_id", DataRowVersion.Original].ToString()))
                                        {
                                            entityColaboradorCargoDisciplina.crg_id = Convert.ToInt32(dtCargoDisciplina.Rows[j]["crg_id", DataRowVersion.Original].ToString());
                                            entityColaboradorCargoDisciplina.coc_id = Convert.ToInt32(dtCargoDisciplina.Rows[j]["coc_id", DataRowVersion.Original].ToString());
                                            entityColaboradorCargoDisciplina.tds_id = Convert.ToInt32(dtCargoDisciplina.Rows[j]["tds_id", DataRowVersion.Original].ToString());

                                            RHU_ColaboradorCargoDisciplinaBO.Delete(entityColaboradorCargoDisciplina, colDao._Banco);

                                            // Atualiza a ligação do colaborador docente com a(s) disciplina(s) da turma(s)
                                            TUR_TurmaDocenteBO.AtualizarTurmaDocentePorColaboradorDocente(entityColaborador.col_id, entityColaboradorCargoDisciplina.crg_id, entityColaboradorCargoDisciplina.coc_id, entityColaboradorCargoDisciplina.tds_id, entityColaboradorCargo.ent_id, entityColaboradorCargo.uad_id, colDao._Banco);
                                        }
                                    }
                                }

                                if ((RHU_ColaboradorCargoSituacao)entityColaboradorCargo.coc_situacao != RHU_ColaboradorCargoSituacao.Ativo
                                    && (RHU_ColaboradorCargoSituacao)entityColaboradorCargo.coc_situacao != RHU_ColaboradorCargoSituacao.Designado)
                                {
                                    // Atualiza a ligação do colaborador docente com a(s) disciplina(s) da turma(s)
                                    TUR_TurmaDocenteBO.AtualizarTurmaDocentePorColaboradorDocente(entityColaborador.col_id, entityColaboradorCargo.crg_id, entityColaboradorCargo.coc_id, 0, entityColaboradorCargo.ent_id, entityColaboradorCargo.uad_id, colDao._Banco);
                                }

                                if (new Guid(dtCargoFuncao.Rows[i]["uad_id"].ToString()) != new Guid(dtCargoFuncao.Rows[i]["uad_idAntigo"].ToString()))
                                {
                                    SYS_UnidadeAdministrativaDAO uadDao = new SYS_UnidadeAdministrativaDAO { _Banco = pesDao._Banco };

                                    //Decrementa um na integridade da unidade administrativa anterior
                                    uadDao.Update_DecrementaIntegridade(entityColaboradorCargo.ent_id, new Guid(dtCargoFuncao.Rows[i]["uad_idAntigo"].ToString()));

                                    //Incrementa um na integridade da unidade administrativa atual
                                    uadDao.Update_IncrementaIntegridade(entityColaboradorCargo.ent_id, new Guid(dtCargoFuncao.Rows[i]["uad_id"].ToString()));
                                }
                            }

                            // Inclui/Exclui os grupos do usuário de acordo com o grupo padrão da função, se existir e caso o parâmetro seja true
                            if (permissoes_colaborador)
                                SalvarGruposUsuarioCargo(entityColaboradorCargo, entityUsuario, bSalvarUsuario, pesDao._Banco, colDao._Banco);
                        }
                        else
                        {
                            entityColaboradorFuncao.cof_id = Convert.ToInt32(dtCargoFuncao.Rows[i]["seqfun_id"].ToString());
                            entityColaboradorFuncao.ent_id = entityColaborador.ent_id;
                            entityColaboradorFuncao.fun_id = Convert.ToInt32(dtCargoFuncao.Rows[i]["fun_id"].ToString());
                            entityColaboradorFuncao.cof_responsavelUa = (dtCargoFuncao.Rows[i]["cof_responsavelUA"].ToString() == "True");
                            entityColaboradorFuncao.cof_matricula = Salvar_Sempre_Maiusculo ? dtCargoFuncao.Rows[i]["coc_matricula"].ToString().ToUpper() : dtCargoFuncao.Rows[i]["coc_matricula"].ToString();
                            entityColaboradorFuncao.uad_id = new Guid(dtCargoFuncao.Rows[i]["uad_id"].ToString());
                            entityColaboradorFuncao.cof_situacao = Convert.ToByte(dtCargoFuncao.Rows[i]["situacao_id"].ToString());
                            entityColaboradorFuncao.cof_vigenciaInicio = Convert.ToDateTime(dtCargoFuncao.Rows[i]["vigenciaini"].ToString());
                            entityColaboradorFuncao.cof_situacao = Convert.ToByte(dtCargoFuncao.Rows[i]["situacao_id"].ToString());
                            entityColaboradorFuncao.cof_observacao = Salvar_Sempre_Maiusculo ? dtCargoFuncao.Rows[i]["observacao"].ToString().ToUpper() : dtCargoFuncao.Rows[i]["observacao"].ToString();

                            if (!String.IsNullOrEmpty(dtCargoFuncao.Rows[i]["vigenciafim"].ToString()))
                            {
                                entityColaboradorFuncao.cof_vigenciaFim = Convert.ToDateTime(dtCargoFuncao.Rows[i]["vigenciafim"].ToString());

                                if (entityColaboradorFuncao.cof_vigenciaFim.Date < DateTime.Now.Date)
                                    entityColaboradorFuncao.cof_situacao = (byte)RHU_ColaboradorFuncaoSituacao.Desativado;
                            }
                            else
                            {
                                entityColaboradorFuncao.cof_vigenciaFim = new DateTime();
                            }

                            if (dtCargoFuncao.Rows[i].RowState == DataRowState.Added)
                            {
                                entityColaboradorFuncao.IsNew = true;
                                RHU_ColaboradorFuncaoBO.Save(entityColaboradorFuncao, colDao._Banco);

                                //Incrementa um na integridade da entidade
                                SYS_EntidadeDAO entDao = new SYS_EntidadeDAO { _Banco = pesDao._Banco };
                                entDao.Update_IncrementaIntegridade(entityColaboradorFuncao.ent_id);

                                //Incrementa um na integridade da unidade administrativa
                                SYS_UnidadeAdministrativaDAO uadDao = new SYS_UnidadeAdministrativaDAO { _Banco = pesDao._Banco };
                                uadDao.Update_IncrementaIntegridade(entityColaboradorFuncao.ent_id, entityColaboradorFuncao.uad_id);
                            }
                            else if (dtCargoFuncao.Rows[i].RowState == DataRowState.Modified)
                            {
                                entityColaboradorFuncao.IsNew = false;
                                RHU_ColaboradorFuncaoBO.Save(entityColaboradorFuncao, colDao._Banco);

                                if (new Guid(dtCargoFuncao.Rows[i]["uad_id"].ToString()) != new Guid(dtCargoFuncao.Rows[i]["uad_idAntigo"].ToString()))
                                {
                                    SYS_UnidadeAdministrativaDAO uadDao = new SYS_UnidadeAdministrativaDAO { _Banco = pesDao._Banco };

                                    //Decrementa um na integridade da unidade administrativa anterior
                                    uadDao.Update_DecrementaIntegridade(entityColaboradorFuncao.ent_id, new Guid(dtCargoFuncao.Rows[i]["uad_idAntigo"].ToString()));

                                    //Incrementa um na integridade da unidade administrativa atual
                                    uadDao.Update_IncrementaIntegridade(entityColaboradorFuncao.ent_id, new Guid(dtCargoFuncao.Rows[i]["uad_id"].ToString()));
                                }
                            }
                        }

                        // Inclui/Exclui os grupos do usuário de acordo com o grupo padrão da função, se existir e caso o parâmetro seja true
                        if (permissoes_colaborador)
                            SalvarGruposUsuarioFuncao(entityColaboradorFuncao, entityUsuario, bSalvarUsuario, pesDao._Banco, colDao._Banco);
                    }
                    else
                    {
                        //Caso o vínculo seja cargo
                        if (!string.IsNullOrEmpty(dtCargoFuncao.Rows[i]["crg_id", DataRowVersion.Original].ToString()))
                        {
                            entityColaboradorCargo.crg_id = Convert.ToInt32(dtCargoFuncao.Rows[i]["crg_id", DataRowVersion.Original]);
                            entityColaboradorCargo.coc_id = Convert.ToInt32(dtCargoFuncao.Rows[i]["seqcrg_id", DataRowVersion.Original]);
                            entityColaboradorCargo.ent_id = entityColaborador.ent_id;
                            entityColaboradorCargo.uad_id = new Guid(dtCargoFuncao.Rows[i]["uad_id", DataRowVersion.Original].ToString());
                            entityColaboradorCargo.coc_situacao = (byte)RHU_ColaboradorCargoSituacao.Excluido;
                           
                            // Atualiza a ligação do colaborador docente com a(s) disciplina(s) da turma(s)
                            TUR_TurmaDocenteBO.AtualizarTurmaDocentePorColaboradorDocente(entityColaborador.col_id, entityColaboradorCargo.crg_id, entityColaboradorCargo.coc_id, 0, entityColaboradorCargo.ent_id, entityColaboradorCargo.uad_id, colDao._Banco);

                            RHU_ColaboradorCargoDAO colcarDAO = new RHU_ColaboradorCargoDAO { _Banco = colDao._Banco };
                            colcarDAO.Delete(entityColaboradorCargo);

                            // Inclui/Exclui os grupos do usuário de acordo com o grupo padrão da função, se existir e caso o parâmetro seja true
                            if (permissoes_colaborador)
                                SalvarGruposUsuarioCargo(entityColaboradorCargo, entityUsuario, bSalvarUsuario, pesDao._Banco, colDao._Banco);

                            //Decrementa um na integridade da entidade
                            SYS_EntidadeDAO entDao = new SYS_EntidadeDAO { _Banco = pesDao._Banco };
                            entDao.Update_DecrementaIntegridade(entityColaboradorCargo.ent_id);

                            //Decrementa um na integridade da unidade administrativa
                            SYS_UnidadeAdministrativaDAO uadDao = new SYS_UnidadeAdministrativaDAO { _Banco = pesDao._Banco };
                            uadDao.Update_DecrementaIntegridade(entityColaboradorCargo.ent_id, entityColaboradorCargo.uad_id);
                        }
                        else //Caso o vínculo seja função
                        {
                            entityColaboradorFuncao.fun_id = Convert.ToInt32(dtCargoFuncao.Rows[i]["fun_id", DataRowVersion.Original]);
                            entityColaboradorFuncao.cof_id = Convert.ToInt32(dtCargoFuncao.Rows[i]["seqfun_id", DataRowVersion.Original]);
                            entityColaboradorFuncao.ent_id = entityColaborador.ent_id;
                            entityColaboradorFuncao.uad_id = new Guid(dtCargoFuncao.Rows[i]["uad_id", DataRowVersion.Original].ToString());
                            entityColaboradorFuncao.cof_situacao = (byte)RHU_ColaboradorFuncaoSituacao.Excluido;

                            RHU_ColaboradorFuncaoDAO colFunDAL = new RHU_ColaboradorFuncaoDAO { _Banco = colDao._Banco };
                            colFunDAL.Delete(entityColaboradorFuncao);

                            // Inclui/Exclui os grupos do usuário de acordo com o grupo padrão da função, se existir e caso o parâmetro seja true
                            if (permissoes_colaborador)
                                SalvarGruposUsuarioFuncao(entityColaboradorFuncao, entityUsuario, bSalvarUsuario, pesDao._Banco, colDao._Banco);

                            //Decrementa um na integridade da entidade
                            SYS_EntidadeDAO entDao = new SYS_EntidadeDAO { _Banco = pesDao._Banco };
                            entDao.Update_DecrementaIntegridade(entityColaboradorFuncao.ent_id);

                            //Decrementa um na integridade da unidade administrativa
                            SYS_UnidadeAdministrativaDAO uadDao = new SYS_UnidadeAdministrativaDAO { _Banco = pesDao._Banco };
                            uadDao.Update_DecrementaIntegridade(entityColaboradorFuncao.ent_id, entityColaboradorFuncao.uad_id);
                        }
                    }
                }

                //Cria o usuario colaborador para integraçao como o live se necessário
                if (entityUsuario != null)
                {
                    if (bSalvarUsuario)
                    {
                        if (bSalvarUserLive)
                        {
                            ManageUserLive live = new ManageUserLive();
                            if (entityUserLive == null)
                            {
                                //Cria o usuario colaborador para integraçao como o live
                                entityUserLive = new UserLive(eTipoUserLive.Colaborador)
                                {
                                    email = entityUsuario.usu_email,
                                    senha = entityUsuario.usu_senha
                                };

                                // Cargo
                                RHU_Cargo cargo = new RHU_Cargo { crg_id = entityColaboradorCargo.crg_id };
                                RHU_CargoBO.GetEntity(cargo, colDao._Banco);

                                // Função
                                RHU_Funcao funcao = new RHU_Funcao { fun_id = entityColaboradorFuncao.fun_id };
                                RHU_FuncaoBO.GetEntity(funcao, colDao._Banco);

                                //Obtendo CPF do usuário
                                string tdo_id = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF);

                                var x = from DataRow dr in dtDocumento.Rows
                                        where dr.RowState != DataRowState.Deleted && dr["tdo_id"].ToString() == tdo_id
                                        select dr["numero"].ToString();
                                if (x.Count() > 0)
                                {
                                    DadosUserColaborador dados = new DadosUserColaborador
                                    {
                                        nome = entityPessoa.pes_nome,
                                        CPF = x.First(),
                                        cargo = cargo.crg_nome,
                                        funcao = funcao.fun_nome,
                                        setor = dtCargoFuncao.Rows.Count > 0 ? dtCargoFuncao.Rows[dtCargoFuncao.Rows.Count - 1]["uad_nome"].ToString() : string.Empty
                                    };

                                    entityUserLive.dadosUserColaborador = dados;
                                }
                                else
                                {
                                    if (!live.VerificarContaEmailExistente(entityUserLive))
                                        throw new ArgumentException("CPF é um documento obrigatório, para integração do colaborador com live.");
                                }
                            }
                        }
                    }
                }

                if (entityColaborador.IsNew)
                {
                    //Incrementa um na integridade da entidade
                    SYS_EntidadeDAO entDao = new SYS_EntidadeDAO { _Banco = pesDao._Banco };
                    entDao.Update_IncrementaIntegridade(entityColaborador.ent_id);

                    //Incrementa um na integridade da pessoa
                    pesDao.Update_IncrementaIntegridade(entityColaborador.pes_id);
                }

                return true;
            }
            catch (Exception err)
            {
                if (bancoEscolar == null)
                {
                    colDao._Banco.Close(err);
                    pesDao._Banco.Close(err);
                }

                throw;
            }
            finally
            {
                if (bancoEscolar == null)
                {
                    colDao._Banco.Close();
                    pesDao._Banco.Close();
                }
            }
        }

        /// <summary>
        /// Inclui/Exclui grupos do cargo para o usuário do colaborador
        /// </summary>
        /// <param name="entityColaboradorCargo">Entidade do cargo do colaborador</param>
        /// <param name="entityUsuario">Entidade do usuário</param>
        /// <param name="bSalvarUsuario">Indica se será incluído um usuário para o colaborador</param>
        /// <param name="bancoCore">Conexão aberta com o banco de dados do CoreSSO</param>
        /// <param name="bancoEscolar">Conexão aberta com o banco de dados do GestaoEscolar</param>
        public static void SalvarGruposUsuarioCargo
        (
            RHU_ColaboradorCargo entityColaboradorCargo
            , SYS_Usuario entityUsuario
            , bool bSalvarUsuario
            , TalkDBTransaction bancoCore
            , TalkDBTransaction bancoEscolar
        )
        {
            if (bSalvarUsuario)
            {
                RHU_Cargo crg = new RHU_Cargo { crg_id = entityColaboradorCargo.crg_id };
                RHU_CargoBO.GetEntity(crg, bancoEscolar);

                // Se a situação da ligação do cargo com o colaborador for ativo
                if ((RHU_ColaboradorCargoSituacao)entityColaboradorCargo.coc_situacao == RHU_ColaboradorCargoSituacao.Ativo)
                {
                    // e se existir algum grupo padrão para o cargo
                    if (!string.IsNullOrEmpty(crg.pgs_chave))
                    {
                        // Inclui a ligação de grupo com o usuário
                        DataTable dtGrupos = SYS_ParametroGrupoPerfilBO.GetSelect_gru_idBy_pgs_chave
                            (crg.pgs_chave, false, 1, 1, bancoCore);

                        for (int i = 0; i < dtGrupos.Rows.Count; i++)
                        {
                            SYS_UsuarioGrupo usg = new SYS_UsuarioGrupo
                            {
                                usu_id = entityUsuario.usu_id,
                                usg_situacao = 1,
                                gru_id = new Guid(dtGrupos.Rows[i]["gru_id"].ToString())
                            };
                            SYS_UsuarioGrupoBO.Save(usg, bancoCore);

                            // Se a visão do grupo for gestão ou ua, tem que incluir também a ua do cargo do colaborador,
                            // na ligação com as ua's do grupo do usuário
                            SYS_Grupo gru = new SYS_Grupo { gru_id = usg.gru_id };
                            SYS_GrupoBO.GetEntity(gru, bancoCore);
                            if (gru.vis_id == SysVisaoID.Gestao || gru.vis_id == SysVisaoID.UnidadeAdministrativa)
                            {
                                DataTable dtGrupoUA = SYS_UsuarioGrupoUABO.GetSelect(entityUsuario.usu_id, gru.gru_id);
                                var x = from DataRow dr in dtGrupoUA.Rows
                                        where new Guid(dr["ent_id"].ToString()) == entityColaboradorCargo.ent_id
                                              && new Guid(dr["uad_id"].ToString()) == entityColaboradorCargo.uad_id
                                        select new Guid(dr["ent_id"].ToString());

                                // Se não existir a ligação da ua do cargo do colaborador com o grupo, ela é incluída
                                if (x.Count() <= 0)
                                {
                                    SYS_UsuarioGrupoUA ugu = new SYS_UsuarioGrupoUA
                                    {
                                        usu_id = entityUsuario.usu_id,
                                        gru_id = usg.gru_id,
                                        ent_id = entityColaboradorCargo.ent_id,
                                        uad_id = entityColaboradorCargo.uad_id,
                                        IsNew = true
                                    };
                                    SYS_UsuarioGrupoUABO.Save(ugu, bancoCore);
                                }
                            }
                        }
                    }
                }
                // Se a situação da ligação com o cargo for diferente de "Ativo"
                else
                {
                    // e se existir algum grupo padrão para o cargo
                    if (!string.IsNullOrEmpty(crg.pgs_chave))
                    {
                        //Exclui logicamente os grupos do usuário de acordo com o grupo do cargo
                        DataTable dtGrupos = SYS_ParametroGrupoPerfilBO.GetSelect_gru_idBy_pgs_chave
                            (crg.pgs_chave, false, 1, 1, bancoCore);

                        for (int i = 0; i < dtGrupos.Rows.Count; i++)
                        {
                            Guid gru_id = new Guid(dtGrupos.Rows[i]["gru_id"].ToString());

                            // Deleta a ligação com a ua se existir
                            SYS_UsuarioGrupoUA ugu = new SYS_UsuarioGrupoUA
                            {
                                usu_id = entityUsuario.usu_id,
                                gru_id = gru_id,
                                ent_id = entityColaboradorCargo.ent_id,
                                uad_id = entityColaboradorCargo.uad_id,
                            };
                            SYS_UsuarioGrupoUABO.DeletarPorUsuarioGrupoUA(ugu, bancoCore);

                            // Se não existir mais nenhuma ua para o grupo do usuário, exclui o grupo do usuário.
                            DataTable dtGrupoUA = SYS_UsuarioGrupoUABO.GetSelect(entityUsuario.usu_id, gru_id);
                            if (dtGrupoUA.Rows.Count <= 0)
                            {
                                SYS_UsuarioGrupo usg = new SYS_UsuarioGrupo
                                {
                                    usu_id = entityUsuario.usu_id,
                                    gru_id = gru_id
                                };
                                SYS_UsuarioGrupoBO.Delete(usg, bancoCore);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Inclui/Exclui grupos do cargo para o usuário do colaborador
        /// </summary>
        /// <param name="entityColaboradorFuncao">Entidade da funcao do colaborador</param>
        /// <param name="entityUsuario">Entidade do usuário</param>
        /// <param name="bSalvarUsuario">Indica se será incluído um usuário para o colaborador</param>
        /// <param name="bancoCore">Conexão aberta com o banco de dados do CoreSSO</param>
        /// <param name="bancoEscolar">Conexão aberta com o banco de dados do GestaoEscolar</param>
        public static void SalvarGruposUsuarioFuncao
        (
            RHU_ColaboradorFuncao entityColaboradorFuncao
            , SYS_Usuario entityUsuario
            , bool bSalvarUsuario
            , TalkDBTransaction bancoCore
            , TalkDBTransaction bancoEscolar
        )
        {
            if (bSalvarUsuario)
            {
                RHU_Funcao fun = new RHU_Funcao { fun_id = entityColaboradorFuncao.fun_id };
                RHU_FuncaoBO.GetEntity(fun, bancoEscolar);

                // Se a situação da ligação da função com o colaborador for ativo
                if ((RHU_ColaboradorFuncaoSituacao)entityColaboradorFuncao.cof_situacao == RHU_ColaboradorFuncaoSituacao.Ativo)
                {
                    // e se existir algum grupo padrão para a função
                    if (!string.IsNullOrEmpty(fun.pgs_chave))
                    {
                        // Inclui a ligação de grupo com o usuário
                        DataTable dtGrupos = SYS_ParametroGrupoPerfilBO.GetSelect_gru_idBy_pgs_chave
                            (fun.pgs_chave, false, 1, 1, bancoCore);

                        for (int i = 0; i < dtGrupos.Rows.Count; i++)
                        {
                            SYS_UsuarioGrupo usg = new SYS_UsuarioGrupo
                            {
                                usu_id = entityUsuario.usu_id,
                                usg_situacao = 1,
                                gru_id = new Guid(dtGrupos.Rows[i]["gru_id"].ToString())
                            };
                            SYS_UsuarioGrupoBO.Save(usg, bancoCore);

                            // Se a visão do grupo for gestão ou ua, tem que incluir também a ua da função do colaborador,
                            // na ligação com as ua's do grupo do usuário
                            SYS_Grupo gru = new SYS_Grupo { gru_id = usg.gru_id };
                            SYS_GrupoBO.GetEntity(gru, bancoCore);
                            if (gru.vis_id == SysVisaoID.Gestao || gru.vis_id == SysVisaoID.UnidadeAdministrativa)
                            {
                                DataTable dtGrupoUA = SYS_UsuarioGrupoUABO.GetSelect(entityUsuario.usu_id, gru.gru_id);
                                var x = from DataRow dr in dtGrupoUA.Rows
                                        where new Guid(dr["ent_id"].ToString()) == entityColaboradorFuncao.ent_id
                                              && new Guid(dr["uad_id"].ToString()) == entityColaboradorFuncao.uad_id
                                        select new Guid(dr["ent_id"].ToString());

                                // Se não existir a ligação da ua da função do colaborador com o grupo, ela é incluída
                                if (x.Count() <= 0)
                                {
                                    SYS_UsuarioGrupoUA ugu = new SYS_UsuarioGrupoUA
                                    {
                                        usu_id = entityUsuario.usu_id,
                                        gru_id = usg.gru_id,
                                        ent_id = entityColaboradorFuncao.ent_id,
                                        uad_id = entityColaboradorFuncao.uad_id,
                                        IsNew = true
                                    };
                                    SYS_UsuarioGrupoUABO.Save(ugu, bancoCore);
                                }
                            }
                        }
                    }
                }
                // Se a situação da ligação com a função for diferente de "Ativo"
                else
                {
                    // e se existir algum grupo padrão para o cargo
                    if (!string.IsNullOrEmpty(fun.pgs_chave))
                    {
                        //Exclui logicamente os grupos do usuário de acordo com o grupo do cargo
                        DataTable dtGrupos = SYS_ParametroGrupoPerfilBO.GetSelect_gru_idBy_pgs_chave
                            (fun.pgs_chave, false, 1, 1, bancoCore);

                        for (int i = 0; i < dtGrupos.Rows.Count; i++)
                        {
                            Guid gru_id = new Guid(dtGrupos.Rows[i]["gru_id"].ToString());

                            // Deleta a ligação com a ua se existir
                            SYS_UsuarioGrupoUA ugu = new SYS_UsuarioGrupoUA
                            {
                                usu_id = entityUsuario.usu_id,
                                gru_id = gru_id,
                                ent_id = entityColaboradorFuncao.ent_id,
                                uad_id = entityColaboradorFuncao.uad_id,
                            };
                            SYS_UsuarioGrupoUABO.DeletarPorUsuarioGrupoUA(ugu, bancoCore);

                            // Se não existir mais nenhuma ua para o grupo do usuário, exclui o grupo do usuário.
                            DataTable dtGrupoUA = SYS_UsuarioGrupoUABO.GetSelect(entityUsuario.usu_id, gru_id);
                            if (dtGrupoUA.Rows.Count <= 0)
                            {
                                SYS_UsuarioGrupo usg = new SYS_UsuarioGrupo
                                {
                                    usu_id = entityUsuario.usu_id,
                                    gru_id = gru_id
                                };
                                SYS_UsuarioGrupoBO.Delete(usg, bancoCore);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Salva pai e mãe do colaborador
        /// </summary>
        /// <param name="entityPessoa">Entidade PES_Pessoa</param>
        /// <param name="PaiAntigo">StructColaboradorFiliacao com os dados do pai</param>
        /// <param name="MaeAntigo">StructColaboradorFiliacao com os dados do pai</param>
        /// <param name="Salvar_Sempre_Maiusculo"></param>
        /// <param name="pes_idFiliacaoPai">pes_id do pai</param>
        /// <param name="pes_idFiliacaoMae">pes_id da mãe</param>
        /// <param name="banco">Transação do banco</param>
        private static void SalvarPaiMae(PES_Pessoa entityPessoa, ref StructColaboradorFiliacao PaiAntigo, ref StructColaboradorFiliacao MaeAntigo, bool Salvar_Sempre_Maiusculo, out Guid pes_idFiliacaoPai, out Guid pes_idFiliacaoMae, TalkDBTransaction banco)
        {
            pes_idFiliacaoPai = Guid.Empty;
            pes_idFiliacaoMae = Guid.Empty;

            //Controle da integridade do pai antigo.
            PES_Pessoa registroAntigo = new PES_Pessoa
            {
                pes_id = entityPessoa.pes_id
            };

            //Detalhes do pai
            if (PaiAntigo.entPessoa != null)
            {
                //Controle da integridade do pai antigo.
                PES_PessoaBO.GetEntity(registroAntigo, banco);
                if (registroAntigo.pes_idFiliacaoPai != PaiAntigo.entPessoa.pes_id)
                {
                    PES_PessoaBO.DecrementaIntegridade(entityPessoa.pes_idFiliacaoPai, banco);
                }

                //Coloca maiusculo o que for pra colocar
                if (Salvar_Sempre_Maiusculo)
                {
                    PaiAntigo.entPessoa.pes_nome = PaiAntigo.entPessoa.pes_nome.ToUpper();
                }

                //carrega lista dos documentos antigos que o pai poderia ter
                DataTable listaAntigaDT = PES_PessoaDocumentoBO.GetSelect(PaiAntigo.entPessoa.pes_id, false, 0, 5);
                //cria LIST com os documentos antigos do pai
                List<PES_PessoaDocumento> listaAntiga = new List<PES_PessoaDocumento>();
                foreach (DataRow dr in listaAntigaDT.Rows)
                {
                    PES_PessoaDocumento pesDoc = new PES_PessoaDocumento
                    {
                        pes_id = new Guid(PaiAntigo.entPessoa.pes_id.ToString())
                        ,
                        tdo_id = new Guid(dr["tdo_id"].ToString())
                        ,
                        psd_situacao = 1
                    };
                    PES_PessoaDocumentoBO.GetEntity(pesDoc, banco);
                    listaAntiga.Add(pesDoc);
                }

                //salva pai e aumenta integridade
                PES_PessoaBO.Save(PaiAntigo.entPessoa, banco);
                if (PaiAntigo.entPessoa.IsNew)
                    PES_PessoaBO.IncrementaIntegridade(PaiAntigo.entPessoa.pes_id, banco);

                //salva cada documento na listDocumentos do pai
                foreach (PES_PessoaDocumento pesDoc in PaiAntigo.listaDocumentos)
                {
                    //Coloca maiusculo o que for pra colocar
                    if (Salvar_Sempre_Maiusculo)
                    {
                        pesDoc.psd_numero = pesDoc.psd_numero.ToUpper();
                    }
                    //arruma pes_id com o pes_id do pai que acabou de ser salvo e salva documento
                    pesDoc.pes_id = PaiAntigo.entPessoa.pes_id;
                    PES_PessoaDocumentoBO.Save(pesDoc, banco);
                    // Incrementa integridade do tipo de documento.
                    if (pesDoc.IsNew)
                        SYS_TipoDocumentacaoBO.IncrementaIntegridade(pesDoc.tdo_id, banco);
                }

                //confere se existiu alguma alteração em relação aos documentos cadastrados,
                //caso exista, exclui os documentos que nao sao mais vinculados a pessoa
                if (listaAntiga.Count > PaiAntigo.listaDocumentos.Count)
                {
                    ExcluirDocumento(listaAntiga, PaiAntigo.listaDocumentos, PaiAntigo.entPessoa.pes_id, banco);
                }

                //arruma o pes_idFiliacaoPai do colaborador a ser inserido com o pai que acabou de ser salvo
                entityPessoa.pes_idFiliacaoPai = PaiAntigo.entPessoa.pes_id;
            }
            //Detalhes da mae
            if (MaeAntigo.entPessoa != null)
            {
                //Controle da integridade da mae antigo.
                if (registroAntigo.pes_idFiliacaoMae != MaeAntigo.entPessoa.pes_id)
                {
                    PES_PessoaBO.DecrementaIntegridade(entityPessoa.pes_idFiliacaoMae, banco);
                }

                //Coloca maiusculo o que for pra colocar
                if (Salvar_Sempre_Maiusculo)
                {
                    MaeAntigo.entPessoa.pes_nome = MaeAntigo.entPessoa.pes_nome.ToUpper();
                }

                //carrega lista dos documentos antigos que a mae poderia ter
                DataTable listaAntigaDT = PES_PessoaDocumentoBO.GetSelect(MaeAntigo.entPessoa.pes_id, false, 0, 5);
                //cria LIST com os documentos antigos da mae
                List<PES_PessoaDocumento> listaAntiga = new List<PES_PessoaDocumento>();
                foreach (DataRow dr in listaAntigaDT.Rows)
                {
                    PES_PessoaDocumento pesDoc = new PES_PessoaDocumento
                    {
                        pes_id = new Guid(MaeAntigo.entPessoa.pes_id.ToString())
                        ,
                        tdo_id = new Guid(dr["tdo_id"].ToString())
                        ,
                        psd_situacao = 1
                    };
                    PES_PessoaDocumentoBO.GetEntity(pesDoc, banco);

                    listaAntiga.Add(pesDoc);
                }
                //salva mae e aumenta integridade
                PES_PessoaBO.Save(MaeAntigo.entPessoa, banco);

                if (MaeAntigo.entPessoa.IsNew)
                    PES_PessoaBO.IncrementaIntegridade(MaeAntigo.entPessoa.pes_id, banco);

                //salva cada documento na listDocumentos da mae
                foreach (PES_PessoaDocumento pesDoc in MaeAntigo.listaDocumentos)
                {
                    //Coloca maiusculo o que for pra colocar
                    if (Salvar_Sempre_Maiusculo)
                    {
                        pesDoc.psd_numero = pesDoc.psd_numero.ToUpper();
                    }
                    //arruma pes_id com o pes_id do pai que acabou de ser salvo e salva documento
                    pesDoc.pes_id = MaeAntigo.entPessoa.pes_id;
                    PES_PessoaDocumentoBO.Save(pesDoc, banco);
                    // Incrementa integridade do tipo de documento.
                    if (pesDoc.IsNew)
                        SYS_TipoDocumentacaoBO.IncrementaIntegridade(pesDoc.tdo_id, banco);
                }
                //confere se existiu alguma alteração em relação aos documentos cadastrados,
                //caso exista, exclui os documentos que nao sao mais vinculados a pessoa
                if (listaAntiga.Count > MaeAntigo.listaDocumentos.Count)
                {
                    ExcluirDocumento(listaAntiga, MaeAntigo.listaDocumentos, MaeAntigo.entPessoa.pes_id, banco);
                }
                //arruma o pes_idFiliacaoPai do colaborador a ser inserido com o pai que acabou de ser salvo
                entityPessoa.pes_idFiliacaoMae = MaeAntigo.entPessoa.pes_id;
            }
        }

        #endregion Métodos incluir e alterar

        #region Métodos de verificação

        /// <summary>
        /// Verifica se existe outro colaborador com um vínculo que possua o mesmo número de matrícula.
        /// </summary>
        /// <param name="col_id">ID do colaborador</param>
        /// <param name="matricula">Matrícula do vínculo do colaborador</param>
        /// <param name="ent_id">ID da entidade do usuário</param>
        /// <returns>True se existe.</returns>
        public static bool VerificaMatriculaExistente
        (
            long col_id,
            string matricula,
            Guid ent_id,
            TalkDBTransaction banco = null
        )
        {
            RHU_ColaboradorDAO dao = new RHU_ColaboradorDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.VerificaMatriculaExistente(col_id, matricula, ent_id);
        }

        #endregion

        #region excluir

        /// <summary>
        /// Deleta logicamente o Colaborador
        /// </summary>
        /// <param name="entity">Entidade RHU_Colaborador</param>
        /// <param name="TransacaoGestao">Transação do banco do Gestão Escolar</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        /// <returns>True = deletado/alterado | False = não deletado/alterado</returns>
        public new static bool Delete
        (
            RHU_Colaborador entity
            , TalkDBTransaction TransacaoGestao
            , Guid ent_id
        )
        {
            RHU_ColaboradorDAO colDao = new RHU_ColaboradorDAO();
            if (TransacaoGestao != null)
                colDao._Banco = TransacaoGestao;
            else
                colDao._Banco.Open(IsolationLevel.ReadCommitted);

            PES_PessoaDAO pesDao = new PES_PessoaDAO();
            pesDao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);
                //Verifica se o Colaborador pode ser deletado
                if (GestaoEscolarUtilBO.VerificarIntegridade("col_id", entity.col_id.ToString(), "RHU_Colaborador,RHU_ColaboradorCargo,RHU_ColaboradorCargoDisciplina,RHU_ColaboradorFuncao", colDao._Banco))
                    throw new ValidationException("Não é possível excluir o colaborador pois possui outros registros ligados a ele.");

                RHU_ColaboradorCargoDAO crgDal = new RHU_ColaboradorCargoDAO { _Banco = colDao._Banco };
                DataTable dt = crgDal.SelectBy_Pesquisa(entity.col_id, MostraCodigoEscola, false, 1, 1, out totalRecords);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["crg_id"].ToString()))
                    {
                        //Decrementa um na integridade da entidade
                        SYS_EntidadeDAO entDao = new SYS_EntidadeDAO { _Banco = pesDao._Banco };
                        entDao.Update_DecrementaIntegridade(entity.ent_id);

                        //Decrementa um na integridade da unidade administrativa
                        SYS_UnidadeAdministrativaDAO uadDao = new SYS_UnidadeAdministrativaDAO { _Banco = pesDao._Banco };
                        uadDao.Update_DecrementaIntegridade(entity.ent_id, new Guid(dt.Rows[i]["uad_id"].ToString()));
                    }
                    else
                    {
                        //Decrementa um na integridade da entidade
                        SYS_EntidadeDAO entDao = new SYS_EntidadeDAO { _Banco = pesDao._Banco };
                        entDao.Update_DecrementaIntegridade(entity.ent_id);

                        //Decrementa um na integridade da unidade administrativa
                        SYS_UnidadeAdministrativaDAO uadDao = new SYS_UnidadeAdministrativaDAO { _Banco = pesDao._Banco };
                        uadDao.Update_DecrementaIntegridade(entity.ent_id, new Guid(dt.Rows[i]["uad_id"].ToString()));
                    }
                }

                //Decrementa um na integridade da pessoa
                pesDao.Update_DecrementaIntegridade(entity.pes_id);

                //Decrementa um na integridade da entidade
                SYS_EntidadeDAO entidadeDao = new SYS_EntidadeDAO { _Banco = pesDao._Banco };
                entidadeDao.Update_DecrementaIntegridade(entity.ent_id);

                //Verifica se a pessoa pode ser deletada
                if (pesDao.Select_Integridade(entity.pes_id) == 0)
                {
                    //Deleta logicamente a pessoa
                    PES_Pessoa entityPessoa = new PES_Pessoa { pes_id = entity.pes_id };
                    PES_PessoaBO.GetEntity(entityPessoa);
                    PES_PessoaBO.Delete(entityPessoa, pesDao._Banco);
                }
                else if (pesDao.Select_Integridade(entity.pes_id) == 1)
                {
                    PES_Pessoa entityPessoa = new PES_Pessoa { pes_id = entity.pes_id };
                    PES_PessoaBO.GetEntity(entityPessoa, pesDao._Banco);

                    //Recupera o id do usuario da pessoa (se existir)
                    Guid usu_id = SYS_UsuarioBO.GetSelectBy_pes_id(entityPessoa.pes_id);

                    //Verifica se existe um usuário ligado a pessoa
                    if (usu_id != Guid.Empty)
                    {
                        //Recupera os dados do banco
                        SYS_Usuario entityUsuario = new SYS_Usuario { usu_id = usu_id };
                        SYS_UsuarioBO.GetEntity(entityUsuario, pesDao._Banco);

                        //Decrementa um na integridade do usuario
                        SYS_UsuarioBO.DecrementaIntegridade(entityUsuario.usu_id, pesDao._Banco);

                        //Deleta logicamente o usuário
                        SYS_UsuarioBO.Delete(entityUsuario, pesDao._Banco);

                        //Deleta logicamente a pessoa
                        PES_PessoaBO.Delete(entityPessoa, pesDao._Banco);
                    }
                }

                //Deleta logicamente o colaborador
                colDao.Delete(entity);

                return true;
            }
            catch (Exception err)
            {
                colDao._Banco.Close(err);
                pesDao._Banco.Close(err);

                throw;
            }
            finally
            {
                colDao._Banco.Close();
                pesDao._Banco.Close();
            }
        }

        #endregion excluir

        #region Métodos Lote de Colaborador
        
        /// <summary>
        /// Excluindo documento
        /// </summary>
        /// <param name="listaDocumento">Busca do banco os documentos anteriores</param>
        /// <param name="listaDocumentoInseridos">Documentos atuais</param>
        /// <param name="pes_id">pes_id do responsável</param>
        /// <param name="bancoCore"></param>
        private static void ExcluirDocumento
            (
                List<PES_PessoaDocumento> listaDocumento
                , List<PES_PessoaDocumento> listaDocumentoInseridos
                , Guid pes_id
                , TalkDBTransaction bancoCore
            )
        {
            foreach (PES_PessoaDocumento item in listaDocumento)
            {
                if (!listaDocumentoInseridos.Exists
                (p =>
                 p.tdo_id == item.tdo_id
                ))
                {
                    // Se o tipo de documento não existir mais, excluir.

                    PES_PessoaDocumento doc = new PES_PessoaDocumento { pes_id = pes_id, tdo_id = item.tdo_id };
                    PES_PessoaDocumentoBO.Delete(doc, bancoCore);
                    // Decrementa integridade da pessoa
                    PES_PessoaBO.DecrementaIntegridade(pes_id, bancoCore);
                    // Decrementa integridade do tipo de documento.
                    SYS_TipoDocumentacaoBO.DecrementaIntegridade(doc.tdo_id, bancoCore);
                }
            }
        }

        #endregion Métodos Lote de Colaborador
    }
}