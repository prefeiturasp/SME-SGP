using System;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using MSTech.Data.Common;
using MSTech.Business.Common;
using MSTech.CoreSSO.DAL;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.CustomResourceProviders;
using System.Web;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Tipo de formato de avaliação.
    /// </summary>
    public enum ACA_FormatoAvaliacaoTipo : byte
    {
        ConceitoGlobal = 1
        ,
        Disciplina = 2
        ,
        GlobalDisciplina = 3
    }

    public enum ACA_FormatoAvaliacaoTipoLancamentoFrequencia : byte
    {
        AulasPlanejadas = 1
        ,
        Periodo = 2
        ,
        Mensal = 3
        ,
        AulasPlanejadasMensal = 4
        ,
        AulasDadas = 5
        ,
        AulasPrevistasDocente = 6
    }

    public enum ACA_FormatoAvaliacaoTipoApuracaoFrequencia : byte
    {
        TemposAula = 1
        ,
        Dia = 2
    }

    public enum ACA_FormatoAvaliacaoCalculoQtdeAulasDadas : byte
    {
        Automatico = 1
        ,
        Manual = 2
    }

    public enum ACA_FormatoAvaliacaoCriterioAprovacaoResultadoFinal : byte
    {
        ConceitoGlobalFrequencia = 1
        ,
        ConceitoGlobal = 2
        ,
        NotaDisciplina = 3
        ,
        ApenasFrequencia = 4
        ,
        TodosAprovados = 5
        ,
        FrequenciaFinalAjustadaDisciplina = 6
    }

    #endregion

    public class ACA_FormatoAvaliacaoBO : BusinessBase<ACA_FormatoAvaliacaoDAO, ACA_FormatoAvaliacao>
    {
        #region Estruturas

        /// <summary>
        /// Estrutura que armazena as entidades de escalas de avaliação do formato.
        /// </summary>
        [Serializable]
        public class FormatoEscalas
        {
            /// <summary>
            /// Entidade do formato de avaliação.
            /// </summary>
            public ACA_FormatoAvaliacao EntFormato;

            /// <summary>
            /// Entidades de escala de avaliação do conceito global.
            /// </summary>
            public Escalas escalasConceitoGlobal;

            /// <summary>
            /// Entidades de escala de avaliação da avaliação adicional, do conceito global.
            /// </summary>
            public Escalas escalasConceitoGlobalAdicional;

            /// <summary>
            /// Entidades de escala de avaliação das disciplinas.
            /// </summary>
            public Escalas escalasDisciplina;

            /// <summary>
            /// Entidades de escala de avaliação da secretaria.
            /// </summary>
            public Escalas escalasDocente;

            /// <summary>
            /// Retorna o tipo de formato de avaliação da entidade carregada.
            /// </summary>
            public ACA_FormatoAvaliacaoTipo TipoFormato
            {
                get
                {
                    if (EntFormato != null)
                    {
                        return (ACA_FormatoAvaliacaoTipo) EntFormato.fav_tipo;
                    }

                    return 0;
                }
            }

            /// <summary>
            /// Retorna o tipo de escala de avaliação do conceito global.
            /// </summary>
            public EscalaAvaliacaoTipo TipoEscalaGlobal
            {
                get
                {
                    if (escalasConceitoGlobal != null && escalasConceitoGlobal.EntEscala != null)
                    {
                        return (EscalaAvaliacaoTipo)escalasConceitoGlobal.EntEscala.esa_tipo;
                    }

                    return 0;
                }
            }

            /// <summary>
            /// Retorna o tipo de escala de avaliação do conceito global na avaliação adicional.
            /// </summary>
            public EscalaAvaliacaoTipo TipoEscalaGlobalAdicional
            {
                get
                {
                    if (escalasConceitoGlobalAdicional != null && escalasConceitoGlobalAdicional.EntEscala != null)
                    {
                        return (EscalaAvaliacaoTipo)escalasConceitoGlobalAdicional.EntEscala.esa_tipo;
                    }

                    return 0;
                }
            }

            /// <summary>
            /// Retorna o tipo de escala de avaliação das disciplinas.
            /// </summary>
            public EscalaAvaliacaoTipo TipoEscalaDisciplina
            {
                get
                {
                    if (escalasDisciplina != null && escalasDisciplina.EntEscala != null)
                    {
                        return (EscalaAvaliacaoTipo)escalasDisciplina.EntEscala.esa_tipo;
                    }

                    return 0;
                }
            }

            /// <summary>
            /// Retorna o tipo de escala de avaliação da secretaria.
            /// </summary>
            public EscalaAvaliacaoTipo TipoEscalaDocente
            {
                get
                {
                    if (escalasDocente != null && escalasDocente.EntEscala != null)
                    {
                        return (EscalaAvaliacaoTipo)escalasDocente.EntEscala.esa_tipo;
                    }

                    return 0;
                }
            }


            /// <summary>
            /// Estruturas com entidades das escalas de avaliação.
            /// </summary>
            [Serializable]
            public class Escalas
            {
                public ACA_EscalaAvaliacao EntEscala;
                public ACA_EscalaAvaliacaoNumerica EntEscalaNumerica;
                public List<ACA_EscalaAvaliacaoParecer> ListaPareceres;
            }
        }

        #endregion Estruturas

        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static ACA_FormatoAvaliacao GetEntity(ACA_FormatoAvaliacao entity, TalkDBTransaction banco = null)
        {
            string chave = RetornaChaveCache_GetEntity(entity);

            ACA_FormatoAvaliacaoDAO dao = new ACA_FormatoAvaliacaoDAO();
            if (banco != null)
                dao._Banco = banco;

            GestaoEscolarUtilBO.CopiarEntity
            (
                CacheManager.Factory.Get
                (
                    chave,
                    () =>
                    {
                        dao.Carregar(entity);
                        return entity;
                    },
                    GestaoEscolarUtilBO.MinutosCacheMedio
                ),
                entity
            );

            return entity;
        }

        /// <summary>
        /// Seleciona o formato de avaliação de acordo com a disciplina da turma.
        /// </summary>
        /// <param name="tud_id">Id da disciplina da turma.</param>
        /// <returns>Formato de avaliação.</returns>
        public new static ACA_FormatoAvaliacao CarregarPorTud(long tud_id, TalkDBTransaction banco = null)
        {
            ACA_FormatoAvaliacao entity = new ACA_FormatoAvaliacao();
            string chave = string.Format(ModelCache.FORMATO_AVALIACAO_POR_TURMADISCIPLINA_MODEL_KEY, tud_id);

            ACA_FormatoAvaliacaoDAO dao = new ACA_FormatoAvaliacaoDAO();
            if (banco != null)
                dao._Banco = banco;

            GestaoEscolarUtilBO.CopiarEntity
            (
                CacheManager.Factory.Get
                (
                    chave,
                    () =>
                    {
                        entity = dao.SelecionarPorTud(tud_id);
                        return entity;
                    },
                    GestaoEscolarUtilBO.MinutosCacheMedio
                ),
                entity
            );

            return entity;
        }

        /// <summary>
        /// Seleciona o formato de avaliação de acordo com a turma.
        /// </summary>
        /// <param name="tur_id">Id da turma.</param>
        /// <returns>Formato de avaliação.</returns>
        public new static ACA_FormatoAvaliacao CarregarPorTur(long tur_id, TalkDBTransaction banco = null)
        {
            ACA_FormatoAvaliacao entity = new ACA_FormatoAvaliacao();
            string chave = string.Format(ModelCache.FORMATO_AVALIACAO_POR_TURMA_MODEL_KEY, tur_id);

            ACA_FormatoAvaliacaoDAO dao = new ACA_FormatoAvaliacaoDAO();
            if (banco != null)
                dao._Banco = banco;

            GestaoEscolarUtilBO.CopiarEntity
            (
                CacheManager.Factory.Get
                (
                    chave,
                    () =>
                    {
                        entity = dao.SelecionarPorTur(tur_id);
                        return entity;
                    },
                    GestaoEscolarUtilBO.MinutosCacheMedio
                ),
                entity
            );

            return entity;
        }

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(ACA_FormatoAvaliacao entity)
        {
            CacheManager.Factory.Remove(RetornaChaveCache_GetEntity(entity));
        }

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(ACA_FormatoAvaliacao entity)
        {
            return string.Format(ModelCache.FORMATO_AVALIACAO_MODEL_KEY, entity.fav_id);
        }

        /// <summary>
        /// Carrega a estrutura com as entidades do formato de avaliação e suas escalas, de acordo com seu
        /// tipo.
        /// </summary>
        /// <param name="fav_id">ID do formato de avlaiação</param>
        /// <returns></returns>
        public static FormatoEscalas RetornaEscalasFormato(int fav_id)
        {
            FormatoEscalas estrutura = new FormatoEscalas();
            estrutura.EntFormato = GetEntity(new ACA_FormatoAvaliacao { fav_id = fav_id });

            ACA_FormatoAvaliacaoTipo tipoFormato = (ACA_FormatoAvaliacaoTipo)estrutura.EntFormato.fav_tipo;

            if (tipoFormato == ACA_FormatoAvaliacaoTipo.ConceitoGlobal ||
                tipoFormato == ACA_FormatoAvaliacaoTipo.GlobalDisciplina)
            {
                // Carregar entidades do conceito global.
                estrutura.escalasConceitoGlobal = RetornaEscalas(estrutura.EntFormato.esa_idConceitoGlobal);

                if (estrutura.EntFormato.fav_conceitoGlobalAdicional)
                {
                    // Se tem avaliação adicional, carrega as escalas.
                    estrutura.escalasConceitoGlobalAdicional =
                        RetornaEscalas(estrutura.EntFormato.esa_idConceitoGlobalAdicional);
                }
            }

            if (tipoFormato == ACA_FormatoAvaliacaoTipo.Disciplina ||
                tipoFormato == ACA_FormatoAvaliacaoTipo.GlobalDisciplina)
            {
                // Carregar entidades da disciplina.
                estrutura.escalasDisciplina = RetornaEscalas(estrutura.EntFormato.esa_idPorDisciplina);
            }

            estrutura.escalasDocente = RetornaEscalas(estrutura.EntFormato.esa_idDocente);

            return estrutura;
        }

        /// <summary>
        /// Retorna a estrutura de escalas carregadas de acordo com o tipo de escala.
        /// </summary>
        /// <param name="esa_id">ID da escala.</param>
        /// <returns></returns>
        private static FormatoEscalas.Escalas RetornaEscalas(int esa_id)
        {
            FormatoEscalas.Escalas escalas = new FormatoEscalas.Escalas
            {
                ListaPareceres = new List<ACA_EscalaAvaliacaoParecer>()
                , EntEscalaNumerica = new ACA_EscalaAvaliacaoNumerica()
                , EntEscala = new ACA_EscalaAvaliacao()
            };
            escalas.EntEscala =
                ACA_EscalaAvaliacaoBO.GetEntity(new ACA_EscalaAvaliacao { esa_id = esa_id });

            EscalaAvaliacaoTipo tipoEscala = (EscalaAvaliacaoTipo) escalas.EntEscala.esa_tipo;

            if (tipoEscala == EscalaAvaliacaoTipo.Numerica)
            {
                // Carrega entidade da escala de avaliação numérica.
                escalas.EntEscalaNumerica = ACA_EscalaAvaliacaoNumericaBO.GetEntity
                    (new ACA_EscalaAvaliacaoNumerica {esa_id = esa_id});
            }
            else if (tipoEscala == EscalaAvaliacaoTipo.Pareceres)
            {
                // Carrega os pareceres da escala.
                escalas.ListaPareceres = ACA_EscalaAvaliacaoParecerBO.GetSelectBy_Escala(esa_id);
            }

            return escalas;
        }

        /// <summary>
        /// Retorna os formatos de avaliação padrão, de acordo com as regras para o currículo.
        /// </summary>
        /// <param name="fav_id">Id do formato de avaliação.</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <param name="qtdeAvaliacaoPeriodica">Quantidade de avaliações periódicas e periódica+final.</param>
        /// <param name="seriadoAvaliacoes">Indica se o currículo do curso é seriado por avaliações.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPor_RegrasCurriculoPeriodo
        (
            int fav_id
            , Guid ent_id
            , int qtdeAvaliacaoPeriodica
            , bool seriadoAvaliacoes
        )
        {
            ACA_FormatoAvaliacaoDAO dao = new ACA_FormatoAvaliacaoDAO();
            return dao.SelectBy_RegrasCurriculo(fav_id, ent_id, qtdeAvaliacaoPeriodica, seriadoAvaliacoes);
        }

        /// <summary>
        /// Retorna os formatos de avaliação padrão de um formato específico ou ativo
        /// </summary>
        /// <param name="fav_id">Id do formato de avaliação</param>
        /// <param name="ent_id">Id da entidade do usuário logado</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaFormatosPorFormatoPadraoAtivo
        (
            int fav_id
            , Guid ent_id
        )
        {
            ACA_FormatoAvaliacaoDAO dao = new ACA_FormatoAvaliacaoDAO();
            return dao.SelectBy_FormatoPadraoAtivo(fav_id, ent_id);
        }

        /// <summary>
        /// Retorna os formatos de avaliação padrão de um formato específico ou ativo.
        /// Verifica as regras do curriculoPeriodo para trazer os formatos válidos.
        /// </summary>
        /// <param name="fav_id">Id do formato de avaliação</param>
        /// <param name="ent_id">Id da entidade do usuário logado</param>
        /// <param name="tur_docenteEspecialista">Flag se turma é de docente especialista</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="banco">Transação com banco</param>
        /// <param name="crp_id">ID do curriculoPeriodo</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPor_RegrasCurriculoPeriodo
        (
            int fav_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Nullable<bool> tur_docenteEspecialista
            , Guid ent_id
            , TalkDBTransaction banco
        )
        {
            ACA_FormatoAvaliacaoDAO dao = new ACA_FormatoAvaliacaoDAO {_Banco = banco};
            return dao.SelectBy_RegrasCurriculoPeriodo(fav_id, cur_id, crr_id, crp_id, 0, false, tur_docenteEspecialista, ent_id);
        }

        /// <summary>
        /// Retorna os formatos de avaliação padrão de um formato específico ou ativo.
        /// Verifica as regras do curriculoPeriodo para trazer os formatos válidos.
        /// </summary>
        /// <param name="fav_id">Id do formato de avaliação</param>
        /// <param name="ent_id">Id da entidade do usuário logado</param>
        /// <param name="tur_docenteEspecialista">Flag se turma é de docente especialista</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do curriculoPeriodo</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPor_RegrasCurriculoPeriodo
        (
            int fav_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Nullable<bool> tur_docenteEspecialista
            , Guid ent_id
        )
        {
            ACA_FormatoAvaliacaoDAO dao = new ACA_FormatoAvaliacaoDAO();
            return dao.SelectBy_RegrasCurriculoPeriodo(fav_id, cur_id, crr_id, crp_id, 0, false, tur_docenteEspecialista, ent_id);
        }

        /// <summary>
        /// Retorna os formatos padrão, de um formato específico ou ativo
        /// e que tenham a quantidade de avaliações periodica ou periodica+final.
        /// Verifica também as regras do curriculoPeriodo.
        /// </summary>
        /// <param name="fav_id">Id do formato de avaliação</param>
        /// <param name="qtdeAvaliacaoPeriodica">Quantidade de avaliações periódicas e periódica+final</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="tur_docenteEspecialista">Flag se turma é de docente especialista</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do curriculoPeriodo</param>
        /// <param name="banco">Transação com banco</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPor_RegrasCurriculoPeriodo_SeriadoAvaliacoes
        (
            int fav_id
            , int qtdeAvaliacaoPeriodica
            , int cur_id
            , int crr_id
            , int crp_id
            , Nullable<bool> tur_docenteEspecialista
            , Guid ent_id
            , TalkDBTransaction banco
        )
        {
            ACA_FormatoAvaliacaoDAO dao = new ACA_FormatoAvaliacaoDAO{_Banco = banco};
            return dao.SelectBy_RegrasCurriculoPeriodo(fav_id, cur_id, crr_id, crp_id, qtdeAvaliacaoPeriodica, true, tur_docenteEspecialista, ent_id);
        }

        /// <summary>
        /// Retorna os formatos padrão, de um formato específico ou ativo
        /// e que tenham a quantidade de avaliações periodica ou periodica+final.
        /// Verifica também as regras do curriculoPeriodo.
        /// </summary>
        /// <param name="fav_id">Id do formato de avaliação</param>
        /// <param name="tur_docenteEspecialista">Flag se turma é de docente especialista</param>
        /// <param name="qtdeAvaliacaoPeriodica">Quantidade de avaliações periódicas e periódica+final</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do curriculoPeriodo</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPor_RegrasCurriculoPeriodo_SeriadoAvaliacoes
        (
            int fav_id
            , int qtdeAvaliacaoPeriodica
            , int cur_id
            , int crr_id
            , int crp_id
            , Nullable<bool> tur_docenteEspecialista
            , Guid ent_id
        )
        {
            ACA_FormatoAvaliacaoDAO dao = new ACA_FormatoAvaliacaoDAO();
            return dao.SelectBy_RegrasCurriculoPeriodo(fav_id, cur_id, crr_id, crp_id, qtdeAvaliacaoPeriodica, true, tur_docenteEspecialista, ent_id);
        }

        /// <summary>
        /// Verifica se o formato de avaliação está sendo utilizado em alguma turma do peja
        /// </summary>      
        /// <param name="fav_id">ID do formato de avaliação</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaTurmaPejaExistente
        (
            int fav_id
            , Guid ent_id
        )
        {
            ACA_FormatoAvaliacaoDAO dao = new ACA_FormatoAvaliacaoDAO();
            return dao.SelectBy_VerificaTurmaPeja(fav_id, ent_id);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os formatos de avaliação
        /// que não foram excluídos logicamente, filtrados por 
        /// id do formato de avaliação, nome do formato de avaliação e situação
        /// </summary>
        /// <param name="fav_id">Id da tabela ACA_FormatoAvalicao do bd</param>
        /// <param name="fav_nome">Campo fav_nome da tabela ACA_FormatoAvalicao do bd</param>
        /// <param name="fav_situacao">Campo fav_situacao da tabela ACA_FormatoAvalicao do bd</param>
        /// <param name="ent_id"></param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param> 
        /// <returns>DataTable com os formatos de avaliação</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int fav_id
            , string fav_nome
            , int fav_situacao
            , Guid ent_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;
            ACA_FormatoAvaliacaoDAO dao = new ACA_FormatoAvaliacaoDAO();
            try
            {
                return dao.SelectBy_fav_id_fav_nome(fav_id, fav_nome, fav_situacao, ent_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna um datatable contendo todos os formatos de avaliação
        /// que não foram excluídos logicamente, filtrados por 
        /// id da escola, id da unidade e o nome do formato de avaliação.
        /// </summary>
        /// <param name="esc_uni_id"></param>
        /// <param name="fav_nome">Campo fav_nome da tabela ACA_FormatoAvalicao do bd</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <returns>DataTable com os formatos de avaliação</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_esc_id_uni_id_fav_nome
        (
            string esc_uni_id
            , string fav_nome
            , Guid ent_id
        )
        {
            int esc_id;
            int uni_id;

            if (!string.IsNullOrEmpty(esc_uni_id))
            {
                esc_id = Convert.ToInt32(esc_uni_id.Split(';')[0]);
                uni_id = Convert.ToInt32(esc_uni_id.Split(';')[1]);
            }
            else
            {
                esc_id = -1;
                uni_id = -1;
            }

            totalRecords = 0;
            ACA_FormatoAvaliacaoDAO dao = new ACA_FormatoAvaliacaoDAO();
            try
            {
                return dao.SelectBy_esc_id_uni_id_fav_nome(esc_id, uni_id, fav_nome, ent_id, out totalRecords);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorno booleano na qual verifica se existe um Formato de avaliação com o mesmo nome 
        /// cadastrado no banco com situação diferente de Excluido.
        /// </summary>
        /// <param name="entity">Entidade do formato de avaliação</param>
        /// <returns>True - caso encontre algum registro no select/False - caso não encontre nada no select</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaFormatoAvaliacaoExistente(ACA_FormatoAvaliacao entity)
        {
            ACA_FormatoAvaliacaoDAO dao = new ACA_FormatoAvaliacaoDAO();
            try
            {
                return dao.SelectBy_Nome(entity.fav_id, entity.fav_nome, entity.ent_id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Verifica o uso de Escala de avaliacao na tabela FormatoAvaliacao
        /// Testa os campos: esa_idConceitoGlobal , esa_idDocente ou esa_idPorDisciplina 
        /// </summary>
        /// <param name="esa_id">Id da escala a ser testada</param>
        /// <returns>TRUE se achar algum registro</returns>
        public static bool VerificaUso_EscalaAvaliacao(int esa_id)
        {
            ACA_FormatoAvaliacaoDAO dao = new ACA_FormatoAvaliacaoDAO();
            try
            {
                DataTable dtRet = dao.SelectBy_esa_id(esa_id);
                return (dtRet.Rows.Count > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Valida se os campos estão vazios ou com valor inválido.
        /// </summary>
        /// <param name="id">ID do campo a ser verificado.</param>
        /// <param name="valorMinimo">Valores dos limites para o campo passado por parametro.</param>
        /// <param name="mensagem">Mensagem de erro a ser mostrada.</param>
        public static void ValidaCampos(int id, string valorMinimo, string mensagem)
        {
            ACA_EscalaAvaliacao ObjEscalaAvaliacao = new ACA_EscalaAvaliacao {esa_id = id};
            ACA_EscalaAvaliacaoBO.GetEntity(ObjEscalaAvaliacao);
            //numerico
            if (ObjEscalaAvaliacao.esa_tipo == 1)
            {
                if (string.IsNullOrEmpty(valorMinimo) || valorMinimo == "0")
                {
                    throw new ArgumentException("Valor mínimo " + mensagem + " é obrigatório.");
                }
            }
            else
                if (ObjEscalaAvaliacao.esa_tipo == 2)
                {
                    if (string.IsNullOrEmpty(valorMinimo) || valorMinimo == "-1")
                    {
                        throw new ArgumentException("Parecer mínimo para aprovação " + mensagem + " é obrigatório.");
                    }
                }
        }

        /// <summary>
        /// Verifica se os valores mínimos estão dentro dos limites setados.
        /// </summary>
        /// <param name="id">ID do campo a ser verificado.</param>
        /// <param name="valorMinimo">Valores dos limites para o campo passado por parametro.</param>
        /// <param name="mensagem">Mensagem de erro a ser mostrada.</param>
        public static void VerificaValoresMinimosDosCampos(int id, string valorMinimo, string mensagem)
        {
            ACA_EscalaAvaliacaoNumerica ObjAvaliacaoNum = new ACA_EscalaAvaliacaoNumerica { esa_id = id };
            ACA_EscalaAvaliacaoNumericaBO.GetEntity(ObjAvaliacaoNum);

            if (Convert.ToDecimal(valorMinimo) <
                ObjAvaliacaoNum.ean_menorValor)
            {
                decimal numero = ObjAvaliacaoNum.ean_menorValor != Decimal.Truncate(ObjAvaliacaoNum.ean_menorValor) ? ObjAvaliacaoNum.ean_menorValor : Decimal.Truncate(ObjAvaliacaoNum.ean_menorValor);
                throw new ArgumentException(
                    "Valor mínimo para " + mensagem + " deve ser maior que " + numero + ".");
            }
            if (Convert.ToDecimal(valorMinimo) >
                ObjAvaliacaoNum.ean_maiorValor)
            {
                decimal numero = ObjAvaliacaoNum.ean_maiorValor != Decimal.Truncate(ObjAvaliacaoNum.ean_maiorValor) ? ObjAvaliacaoNum.ean_maiorValor : Decimal.Truncate(ObjAvaliacaoNum.ean_maiorValor);
                throw new ArgumentException(
                    "Valor mínimo para " + mensagem + " deve ser menor que " + numero + ".");
            }
            if (Convert.ToDecimal(valorMinimo) %
                ObjAvaliacaoNum.ean_variacao != 0)
            {
                decimal numero = ObjAvaliacaoNum.ean_variacao != Decimal.Truncate(ObjAvaliacaoNum.ean_variacao) ? ObjAvaliacaoNum.ean_variacao : Decimal.Truncate(ObjAvaliacaoNum.ean_variacao);
                throw new ArgumentException(
                    "Valor mínimo para " + mensagem + " deve ter variação de " + numero + ".");
            }
        }
    }
}
