using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.ComponentModel;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Situações do cargo do colaborador
    /// </summary>
    public enum RHU_ColaboradorCargoSituacao : byte
    {
        Ativo = 1
        ,
        Excluido = 3
        ,
        Designado = 4
        ,
        Afastado = 5
        ,
        Desativado = 6
    }

    #endregion

    public class RHU_ColaboradorCargoBO : BusinessBase<RHU_ColaboradorCargoDAO, RHU_ColaboradorCargo>    
    {
        /// <summary>
        /// Verifica se existe cargo do colaborador com os mesmos dados: unidade, cargo e vigencia.
        /// </summary>
        /// <param name="entidade"></param>
        /// <returns></returns>
        public static bool VerificaRegistroDuplicado
            (RHU_ColaboradorCargo entidade, TalkDBTransaction banco)
        {
            RHU_ColaboradorCargoDAO dao = new RHU_ColaboradorCargoDAO { _Banco = banco };
            DataTable dt = dao.PesquisaAtribuicoesIguais(entidade.col_id, entidade.crg_id, entidade.coc_id, entidade.ent_id
                , entidade.uad_id, entidade.coc_vigenciaInicio, entidade.coc_vigenciaFim);

            return (dt.Rows.Count > 0);
        }

        /// <summary>
        /// Exclusão de uma atribuição esporádica, verificando se tem aula criada na data de sua atribuição.
        /// </summary>
        /// <param name="entidade"></param>
        /// <returns></returns>
        public static bool ExcluirAtribuicaoEsporadica
            (RHU_ColaboradorCargo entidade, long doc_id, Guid ent_id)
        {
            DataTable dtAulas = CLS_TurmaAulaBO.PesquisaPor_AtribuicaoEsporadica(entidade.col_id, entidade.crg_id, entidade.coc_id, null);

            if (dtAulas.Rows.Count > 0)
            {
                throw new ValidationException("Não é possível excluir essa atribuição esporádica, pois o(a) docente fez registros de aula neste período.");
            }

            if (Delete(entidade))
            {
                // Limpar cache para a tela de atribuição de docentes.
                GestaoEscolarUtilBO.LimpaCache(ESC_UnidadeEscolaBO.RetornaChaveCache_SelectEscolas_VisaoIndividual(doc_id, ent_id, 1));
                GestaoEscolarUtilBO.LimpaCache(ESC_UnidadeEscolaBO.RetornaChaveCache_SelectEscolas_VisaoIndividual(doc_id, ent_id, 0));
                GestaoEscolarUtilBO.LimpaCache(ESC_UnidadeEscolaBO.RetornaChaveCache_SelectEscolas_VisaoIndividual(doc_id, ent_id, 2));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Retorna um datatable contendo todos os cargos e funções
        /// dos colaboradores que não foram excluídas logicamente, filtrados por 
        /// id do colaborador        
        /// </summary>
        /// <param name="col_id">Id da tabela RHU_Colaborador do bd</param>                
        /// <param name="paginado">Indica se o datatable será paginado ou não</param> 
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param> 
        /// <returns>DataTable com os cargos e funções dos colaboradores</returns>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            long col_id
            , bool paginado
            , int currentPage
            , int pageSize
            , Guid ent_id
            , TalkDBTransaction banco = null
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            RHU_ColaboradorCargoDAO dao = new RHU_ColaboradorCargoDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.SelectBy_Pesquisa(col_id, MostraCodigoEscola, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Seleciona o id do último cargo cadastrado por colaborador/cargo + 1
        /// se não houver cargo cadastrado para o colaborador/cargo retorna 1
        /// filtrados por col_id, crg_id
        /// </summary>
        /// <param name="col_id">Campo col_id da tabela RHU_ColaboradorCargo do bd</param>        
        /// <param name="crg_id">Campo crg_id da tabela RHU_ColaboradorCargo do bd</param>        
        /// <returns>coc_id + 1</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Int32 VerificaUltimoCargoCadastrado
        (
            long col_id
            , int crg_id
        )
        {
            RHU_ColaboradorCargoDAO dao = new RHU_ColaboradorCargoDAO();
            return dao.SelectBy_col_id_crg_id_top_one(col_id, crg_id);            
        }

        /// <summary>
        /// Inclui um novo cargo para o colaborador
        /// </summary>
        /// <param name="entity">Entidade RHU_ColaboradorCargo</param>        
        /// <param name="banco"></param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            RHU_ColaboradorCargo entity
            , TalkDBTransaction banco
        )
        {            
            if (entity.Validate())
            {
                RHU_ColaboradorCargoDAO dao = new RHU_ColaboradorCargoDAO { _Banco = banco };
                return dao.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }
        

        /// <summary>
        /// Retorna o coc_id      
        /// </summary>
        /// <param name="col_id">ID do colaborador</param>                
        /// <param name="crg_id">ID do cargo</param> 
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="esc_id">ID da escola</param>

        public static int SelectColaboradorCargoID
        (
             long col_id
            , int crg_id
            , Guid ent_id
            , int esc_id
        )
        {
            RHU_ColaboradorCargoDAO dao = new RHU_ColaboradorCargoDAO();
            return dao.SelectColaboradorCargoID(col_id, crg_id, ent_id,esc_id);          
        }

        /// <summary>
        /// Retorna o coc_id se existir
        /// </summary>
        /// <param name="col_id">ID do colaborador</param>
        /// <param name="coc_matricula">Matrícula do colaborador</param>
        /// <param name="coc_complementacaoCargaHoraria">Informa se é complementação de carga horária</param>
        /// <param name="coc_vinculoExtra">Informa se é vinculo extra</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="uad_id">ID da escola</param>
        /// <param name="banco">Transação do banco</param>

        public static int SelectColaboradorCargoID
        (
             long col_id
            , int crg_id
            , string coc_matricula
            , bool coc_complementacaoCargaHoraria
            , bool coc_vinculoExtra
            , Guid ent_id
            , Guid uad_id
            , TalkDBTransaction banco
        )
        {
            RHU_ColaboradorCargoDAO dao = new RHU_ColaboradorCargoDAO();
            return dao.SelectColaboradorCargoID(col_id, crg_id, coc_matricula, coc_complementacaoCargaHoraria, coc_vinculoExtra, ent_id, uad_id, banco);
        }

        /// <summary>
        /// Salva os registros de ColaboradorCargo da lista informada, valida as datas e se for um registro  
        ///     novo verifica se já existe um coc_id correspondente ao registro que está sendo salvo.
        /// </summary>
        /// <param name="lstColaboradorCargo">Lista de entidade RHU_ColaboradorCargo</param>
        public static void SalvarColaboradorCargo
        (
            List<RHU_ColaboradorCargo> lstColaboradorCargo
        )
        {
            RHU_ColaboradorCargoDAO dao = new RHU_ColaboradorCargoDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                foreach (RHU_ColaboradorCargo entity in lstColaboradorCargo)
                {
                    RHU_ColaboradorCargo colaboradorCargo = entity;
                    if (!colaboradorCargo.Validate())
                        throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(colaboradorCargo));

                    //Carrega o registro de Colaborador do cargo que está sendo salvo
                    RHU_Colaborador colaborador = new RHU_Colaborador
                                                        {
                                                            col_id = colaboradorCargo.col_id,
                                                            ent_id = colaboradorCargo.ent_id
                                                        };
                    RHU_ColaboradorBO.GetEntity(colaborador, dao._Banco);

                    //Valida os dados do registro que está sendo adicionado
                    if (colaboradorCargo.coc_vigenciaFim != new DateTime() && colaboradorCargo.coc_vigenciaFim < colaboradorCargo.coc_vigenciaInicio)
                        throw new ArgumentException("Vigência inicial não pode ser maior que a vigência final.");

                    if (colaborador.col_dataAdmissao != new DateTime() && colaboradorCargo.coc_vigenciaInicio < colaborador.col_dataAdmissao)
                        throw new ArgumentException("Vigência inicial não pode ser menor que a data de admissão.");

                    if (colaborador.col_dataAdmissao != new DateTime() && colaboradorCargo.coc_vigenciaFim != new DateTime() &&
                        colaboradorCargo.coc_vigenciaFim < colaborador.col_dataAdmissao)
                        throw new ArgumentException("Vigência final não pode ser menor que a data de admissão.");

                    if (colaborador.col_dataDemissao != new DateTime() && colaboradorCargo.coc_vigenciaInicio > colaborador.col_dataDemissao)
                        throw new ArgumentException("Vigência inicial não pode ser maior que a data de demissão.");

                    if (colaborador.col_dataDemissao != new DateTime() && colaboradorCargo.coc_vigenciaFim != new DateTime() &&
                        colaboradorCargo.coc_vigenciaFim > colaborador.col_dataDemissao)
                        throw new ArgumentException("Vigência final não pode ser maior que a data de demissão.");

                    //Só salva se for matrícula de complmentação de carga horária ou vínculo extra
                    if (!colaboradorCargo.coc_complementacaoCargaHoraria && !colaboradorCargo.coc_vinculoExtra)
                        throw new ValidationException("Método é usado para salvar apenas colaborador cargo de vículo extra ou complementação de carga horária.");
                    
                    //Se é uma matrícula nova então verifica se já existe no banco
                    if (colaboradorCargo.IsNew)
                    {
                        //Verifica se já existe um registro salvo correspondente ao que está sendo gravado
                        int coc_id = SelectColaboradorCargoID(colaboradorCargo.col_id, colaboradorCargo.crg_id, colaboradorCargo.coc_matricula,
                                                                colaboradorCargo.coc_complementacaoCargaHoraria, colaboradorCargo.coc_vinculoExtra,
                                                                colaboradorCargo.ent_id, colaboradorCargo.uad_id, dao._Banco);

                        //Se já houver um registro de colaborador cargo salvo no banco então só atualiza esse registro
                        if (coc_id > 0)
                        {
                            RHU_ColaboradorCargo colaboradorCargoAux = new RHU_ColaboradorCargo
                                                                    {
                                                                        col_id = colaboradorCargo.col_id,
                                                                        crg_id = colaboradorCargo.crg_id,
                                                                        coc_id = coc_id,
                                                                        ent_id = colaboradorCargo.ent_id
                                                                    };
                            GetEntity(colaboradorCargoAux, dao._Banco);

                            //Atualiza os dados da matrícula já salva no sistema
                            colaboradorCargoAux.coc_observacao = colaboradorCargo.coc_observacao;
                            colaboradorCargoAux.coc_vigenciaInicio = colaboradorCargo.coc_vigenciaInicio;
                            colaboradorCargoAux.coc_vigenciaFim = colaboradorCargo.coc_vigenciaFim;

                            //Valida a matrícula já salva no sistema
                            if (!colaboradorCargoAux.Validate())
                                throw new ValidationException("Matrícula já existente no sistema. " +
                                                                GestaoEscolarUtilBO.ErrosValidacao(colaboradorCargoAux));

                            colaboradorCargo = colaboradorCargoAux;
                        }
                    }

                    //Finaliza a matrícula se a vigência fim informada fo retroativa
                    if (colaboradorCargo.coc_vigenciaFim != new DateTime() && colaboradorCargo.coc_vigenciaFim < DateTime.Now)
                        colaboradorCargo.coc_situacao = 6;
                    else
                        colaboradorCargo.coc_situacao = 1;

                    colaboradorCargo.coc_dataAlteracao = DateTime.Now;

                    if (!Save(colaboradorCargo, dao._Banco))
                        throw new ArgumentException("Erro ao salvar o colaborador cargo.");
                }
            }
            catch (SqlException ex)
            {
                dao._Banco.Close(ex);
                throw ex;
            }
            catch (ValidationException ex)
            {
                dao._Banco.Close(ex);
                throw ex;
            }
            catch (ArgumentException ex)
            {
                dao._Banco.Close(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                dao._Banco.Close(ex);
                throw ex;
            }
            finally
            {
                dao._Banco.Close();
            }
        }

        /// <summary>
        /// Salva os registros de ColaboradorCargo da lista informada, valida as datas e se for um registro  
        ///     novo verifica se já existe um coc_id correspondente ao registro que está sendo salvo.
        /// </summary>
        /// <param name="lstColaboradorCargo">Lista de entidade RHU_ColaboradorCargo</param>
        public static void SalvarColaboradorCargoAtribuiçãoDocente(List<RHU_ColaboradorCargo> lstColaboradorCargo)
        {
            RHU_ColaboradorCargoDAO dao = new RHU_ColaboradorCargoDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                foreach (RHU_ColaboradorCargo entity in lstColaboradorCargo)
                {
                    RHU_ColaboradorCargo colaboradorCargo = entity;
                    if (!colaboradorCargo.Validate())
                        throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(colaboradorCargo));

                    //Carrega o registro de Colaborador do cargo que está sendo salvo
                    RHU_Colaborador colaborador = new RHU_Colaborador
                    {
                        col_id = colaboradorCargo.col_id,
                        ent_id = colaboradorCargo.ent_id
                    };
                    RHU_ColaboradorBO.GetEntity(colaborador, dao._Banco);

                    //Valida os dados do registro que está sendo adicionado

                    if (colaboradorCargo.coc_vigenciaFim != new DateTime() && colaboradorCargo.coc_vigenciaFim < colaboradorCargo.coc_vigenciaInicio)
                        throw new ArgumentException("Vigência inicial não pode ser maior que a vigência final.");

                    if (colaborador.col_dataAdmissao != new DateTime() && colaboradorCargo.coc_vigenciaInicio < colaborador.col_dataAdmissao)
                        throw new ArgumentException("Vigência inicial não pode ser menor que a data de admissão.");

                    if (colaborador.col_dataAdmissao != new DateTime() && colaboradorCargo.coc_vigenciaFim != new DateTime() &&
                        colaboradorCargo.coc_vigenciaFim < colaborador.col_dataAdmissao)
                        throw new ArgumentException("Vigência final não pode ser menor que a data de admissão.");

                    if (colaborador.col_dataDemissao != new DateTime() && colaboradorCargo.coc_vigenciaInicio > colaborador.col_dataDemissao)
                        throw new ArgumentException("Vigência inicial não pode ser maior que a data de demissão.");

                    if (colaborador.col_dataDemissao != new DateTime() && colaboradorCargo.coc_vigenciaFim != new DateTime() &&
                        colaboradorCargo.coc_vigenciaFim > colaborador.col_dataDemissao)
                        throw new ArgumentException("Vigência final não pode ser maior que a data de demissão.");

                    ////Só salva se for matrícula de complmentação de carga horária ou vínculo extra
                    //if (!colaboradorCargo.coc_complementacaoCargaHoraria && !colaboradorCargo.coc_vinculoExtra)
                    //    throw new ValidationException("Método é usado para salvar apenas colaborador cargo de vículo extra ou complementação de carga horária.");

                    //Se é uma matrícula nova então verifica se já existe no banco
                    if (colaboradorCargo.IsNew)
                    {
                        //Verifica se já existe um registro salvo correspondente ao que está sendo gravado
                        int coc_id = SelectColaboradorCargoID(colaboradorCargo.col_id, colaboradorCargo.crg_id, colaboradorCargo.coc_matricula,
                                                                colaboradorCargo.coc_complementacaoCargaHoraria, colaboradorCargo.coc_vinculoExtra,
                                                                colaboradorCargo.ent_id, colaboradorCargo.uad_id, dao._Banco);

                        //Se já houver um registro de colaborador cargo salvo no banco então só atualiza esse registro
                        if (coc_id > 0)
                            throw new ArgumentException("O colaborador já possui um cargo nesta unidade");
                    }

                    //Finaliza a matrícula se a vigência fim informada fo retroativa
                    if (colaboradorCargo.coc_vigenciaFim != new DateTime() && colaboradorCargo.coc_vigenciaFim < DateTime.Now)
                        colaboradorCargo.coc_situacao = 6;
                    else
                        colaboradorCargo.coc_situacao = 1;

                    colaboradorCargo.coc_dataAlteracao = DateTime.Now;

                    if (!Save(colaboradorCargo, dao._Banco))
                        throw new ArgumentException("Erro ao salvar o colaborador cargo.");
                }
            }
            catch (Exception ex)
            {
                dao._Banco.Close(ex);
                throw ex;
            }
            finally
            {
                dao._Banco.Close();
            }
        }

        /// <summary>
        /// Valida a vigência passada por parâmetro com a vigência do cargo selecionado, deve estar dentro do período.
        /// </summary>
        /// <returns></returns>
        public static bool ValidarVigenciaPorData(Int64[] ids, DateTime coc_vigenciaInicio, DateTime coc_vigenciaFim,
            out string periodoVigenciaValido)
        {
            Int64 doc_id = 0, col_id = 0;
            Int32 crg_id = 0, coc_id = 0;

            doc_id = ids[0];
            col_id = ids[1];
            crg_id = Convert.ToInt32(ids[2]);
            coc_id = Convert.ToInt32(ids[3]);

            RHU_ColaboradorCargo entity = RHU_ColaboradorCargoBO.GetEntity(new RHU_ColaboradorCargo
            {
                col_id = col_id
                ,
                crg_id = crg_id
                ,
                coc_id = coc_id
            });

            bool periodoValido = (coc_vigenciaInicio >= entity.coc_vigenciaInicio)
                && ((entity.coc_vigenciaFim == new DateTime() && coc_vigenciaFim == new DateTime())
                    || (entity.coc_vigenciaFim == new DateTime() && coc_vigenciaFim != new DateTime())
                    || (entity.coc_vigenciaFim != new DateTime() && coc_vigenciaFim != new DateTime() &&
                        coc_vigenciaFim <= entity.coc_vigenciaFim));

            periodoVigenciaValido = entity.coc_vigenciaInicio.ToString("dd/MM/yyyy")
                + " - " + (entity.coc_vigenciaFim != new DateTime() ? (
                entity.coc_vigenciaFim.ToString("dd/MM/yyyy")) : "*");

            return periodoValido;
        }    
    }
}
