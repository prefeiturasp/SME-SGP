using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using MSTech.Business.Common;
using MSTech.CoreSSO.DAL;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Situações do curso
    /// </summary>
    public enum ACA_CursoSituacao : byte
    {
        Ativo = 1
        ,

        Excluido = 3
        ,

        Desativado = 4
        ,

        EmAtivacao = 5
        ,

        EmDesativacao = 6
    }
    

    #endregion Enumeradores

    #region Estruturas

    public struct sComboCurso
    {
        public string cur_crr_id { get; set; }

        public string cur_crr_nome { get; set; }
        public int tne_id { get; set; }
    }

    #endregion Estruturas

    public class ACA_CursoBO : BusinessBase<ACA_CursoDAO, ACA_Curso>
    {
        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCursoCurriculo(int esc_id, int uni_id, byte cur_situacao, Guid ent_id)
        {
            return string.Format("Cache_SelecionaCursoCurriculo_{0}_{1}_{2}_{3}", esc_id, uni_id, cur_situacao, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCursoCurriculoPorUsuario(Guid usu_id, Guid gru_id, byte cur_situacao, Guid ent_id)
        {
            return string.Format("Cache_SelecionaCursoCurriculoPorUsuario_{0}_{1}_{2}_{3}", usu_id, gru_id, cur_situacao, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCursoCurriculoPorEscola(int esc_id, int uni_id, Guid ent_id)
        {
            return string.Format("Cache_SelecionaCursoCurriculoPorEscola_{0}_{1}_{2}", esc_id, uni_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCursoCurriculoVigentesPorEscola(int esc_id, int uni_id, Guid ent_id)
        {
            return string.Format("Cache_SelecionaCursoCurriculoVigentesPorEscola_{0}_{1}_{2}", esc_id, uni_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCursoCurriculoPorEscolaTipoCiclo(int esc_id, int uni_id, int tci_id, Guid ent_id)
        {
            return string.Format("Cache_SelecionaCursoCurriculoPorEscolaTipoCiclo_{0}_{1}_{2}_{3}", esc_id, uni_id, tci_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCursoCurriculoVigentesPorEscolaTipoCiclo(int esc_id, int uni_id, int tci_id, Guid ent_id)
        {
            return string.Format("Cache_SelecionaCursoCurriculoVigentesPorEscolaTipoCiclo_{0}_{1}_{2}_{3}", esc_id, uni_id, tci_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCursoCurriculoPorEscolaCalendarioTipoCiclo(int esc_id, int uni_id, int cal_id, int tci_id, Guid ent_id)
        {
            return string.Format("Cache_SelecionaCursoCurriculoPorEscolaCalendarioTipoCiclo_{0}_{1}_{2}_{3}_{4}", esc_id, uni_id, cal_id, tci_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCursoCurriculoPorEscolaCalendarioAnoTipoCiclo(int esc_id, int uni_id, int cal_ano, int tci_id, Guid ent_id)
        {
            return string.Format("Cache_SelecionaCursoCurriculoPorEscolaCalendarioAnoTipoCiclo_{0}_{1}_{2}_{3}_{4}", esc_id, uni_id, cal_ano, tci_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCursoCurriculoPorEscolaNivelEnsino(int esc_id, int uni_id, int tne_id, Guid ent_id)
        {
            return string.Format("Cache_SelecionaCursoCurriculoPorEscolaNivelEnsino_{0}_{1}_{2}_{3}", esc_id, uni_id, tne_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCursoCurriculoPorNivelEnsino(int tne_id, Guid ent_id)
        {
            return string.Format("Cache_SelecionaCursoCurriculoPorNivelEnsino_{0}_{1}", tne_id, ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelectCursoComDisciplinaEletiva(int esc_id, int uni_id, Guid ent_id, int cur_situacao)
        {
            return string.Format("Cache_SelectCursoComDisciplinaEletiva_{0}_{1}_{2}_{3}", esc_id, uni_id, ent_id, cur_situacao);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_Seleciona_CursosRelacionados_Por_Escola(int cur_id, int crr_id, int esc_id, int uni_id, bool somenteAtivos)
        {
            return string.Format("Cache_Seleciona_CursosRelacionados_Por_Escola_{0}_{1}_{2}_{3}_{4}", cur_id, crr_id, esc_id, uni_id, somenteAtivos);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_Seleciona_CursosRelacionadosVigentes_Por_Escola(int cur_id, int crr_id, int esc_id, int uni_id)
        {
            return string.Format("Cache_Seleciona_CursosRelacionadosVigentes_Por_Escola_{0}_{1}_{2}_{3}", cur_id, crr_id, esc_id, uni_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCursoCurriculoCalendarioEscola(int esc_id, int uni_id, byte cur_situacao, Guid ent_id, int cal_id)
        {
            return string.Format("Cache_SelecionaCursoCurriculoCalendarioEscola_{0}_{1}_{2}_{3}_{4}", esc_id, uni_id, cur_situacao, ent_id, cal_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaCursoCurriculoVigentesCalendarioEscola(int esc_id, int uni_id, byte cur_situacao, Guid ent_id, int cal_id)
        {
            return string.Format("Cache_SelecionaCursoCurriculoVigentesCalendarioEscola_{0}_{1}_{2}_{3}_{4}", esc_id, uni_id, cur_situacao, ent_id, cal_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaApenasCursosRelacionadosPorEscola(int cur_id, int crr_id, int esc_id, int uni_id, bool somenteAtivos)
        {
            return string.Format("Cache_SelecionaApenasCursosRelacionadosPorEscola_{0}_{1}_{2}_{3}_{4}", cur_id, crr_id, esc_id, uni_id, somenteAtivos);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_CursoCurriculoEfetivacaoSemestral(Guid ent_id)
        {
            return string.Format("Cache_CursoCurriculoEfetivacaoSemestral_{0}", ent_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_Seleciona_Cursos_Por_ModalidadeEnsino(int tme_id, int esc_id, int uni_id, Guid ent_id)
        {
            return string.Format("Cache_Seleciona_Cursos_Por_ModalidadeEnsino_{0}_{1}_{2}_{3}", tme_id, esc_id, uni_id, ent_id);
        }

        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static ACA_Curso GetEntity(ACA_Curso entity, TalkDBTransaction banco = null)
        {
            ACA_CursoDAO dao = banco == null ? new ACA_CursoDAO() : new ACA_CursoDAO { _Banco = banco };

            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_GetEntity(entity);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    dao.Carregar(entity);
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

            dao.Carregar(entity);

            return entity;
        }

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(ACA_Curso entity)
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
        private static string RetornaChaveCache_GetEntity(ACA_Curso entity)
        {
            return string.Format("ACA_Curso_GetEntity_{0}", entity.cur_id);
        }

        /// <summary>
        /// Verifica se os cursos são equivalentes ou são iguais.
        /// Se o aluno pode ir do curso de origem para o curso de destino.
        /// </summary>
        /// <param name="cur_idOrigem">Curso de origem</param>
        /// <param name="crr_idOrigem">Currículo de origem</param>
        /// <param name="cur_idDestino">Curso de destino</param>
        /// <param name="crr_idDestino">Currículo de destino</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        internal static bool Sao_Cursos_Equivalentes
        (
            int cur_idOrigem
            , int crr_idOrigem
            , int cur_idDestino
            , int crr_idDestino
            , TalkDBTransaction banco
        )
        {
            bool cursoIgual = ((cur_idOrigem == cur_idDestino) &&
                               (crr_idOrigem == crr_idDestino));

            bool cursoEquivalente = cursoIgual;

            if (!cursoIgual)
            {
                DataTable dt = Seleciona_CursosRelacionados(cur_idOrigem, crr_idOrigem, banco);

                var x = from DataRow dr in dt.Rows
                        where
                            Convert.ToInt32(dr["cur_id"]) == cur_idDestino
                            &&
                            Convert.ToInt32(dr["crr_id"]) == crr_idDestino
                        select
                            new
                            {
                                cur_id = Convert.ToInt32(dr["cur_id"])
                                ,
                                crr_id = Convert.ToInt32(dr["crr_id"])
                            };

                // Verifica se o curso de destino é equivalente.
                cursoEquivalente = x.Count() > 0;
            }

            return cursoEquivalente;
        }

        /// <summary>
        /// Verifica se possui cursos equivalentes
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="banco">Transação com banco</param>
        /// <param name="cursosRelacionados">DataTable com os cursos relacionados</param>
        /// <returns></returns>
        internal static bool Possui_Cursos_Equivalentes(int cur_id, int crr_id, int esc_id, int uni_id, TalkDBTransaction banco, out List<sComboCurso> cursosRelacionados)
        {
            List<sComboCurso> dt = Seleciona_CursosRelacionados_Por_Escola(cur_id, crr_id, esc_id, uni_id, false, GestaoEscolarUtilBO.MinutosCacheLongo);
            cursosRelacionados = dt;
            return dt.Count > 0;
        }

        /// <summary>
        /// Retorna os cur_id e crr_id relacionados ao curso informado,
        /// que tenham ligação com a escola informada.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="somenteAtivos">True - Trazer os cursos relacionados ativos / False - Trazer os cursos relacionados não excluídos logicamente</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> Seleciona_CursosRelacionados_Por_Escola
        (
            int cur_id
            , int crr_id
            , int esc_id
            , int uni_id
            , bool somenteAtivos
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCurso> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_Seleciona_CursosRelacionados_Por_Escola(cur_id, crr_id, esc_id, uni_id, somenteAtivos);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ACA_CursoDAO dao = new ACA_CursoDAO();
                        DataTable dtDados = dao.Seleciona_CursosRelacionados_Por_Escola(cur_id, crr_id, esc_id, uni_id, somenteAtivos);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboCurso
                                 {
                                     cur_crr_id = dr["cur_crr_id"].ToString(),
                                     cur_crr_nome = dr["cur_crr_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboCurso>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ACA_CursoDAO dao = new ACA_CursoDAO();
                DataTable dtDados = dao.Seleciona_CursosRelacionados_Por_Escola(cur_id, crr_id, esc_id, uni_id, somenteAtivos);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboCurso
                         {
                             cur_crr_id = dr["cur_crr_id"].ToString(),
                             cur_crr_nome = dr["cur_crr_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna os cur_id e crr_id relacionados ao curso informado, (exceto ele mesmo)
        /// que tenham ligação com a escola informada.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="somenteAtivos">True - Trazer os cursos relacionados ativos / False - Trazer os cursos relacionados não excluídos logicamente</param>
        /// <returns></returns>
        public static List<sComboCurso> SelecionaApenasCursosRelacionadosPorEscola
        (
            int cur_id
            , int crr_id
            , int esc_id
            , int uni_id
            , bool somenteAtivos
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCurso> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaApenasCursosRelacionadosPorEscola(cur_id, crr_id, esc_id, uni_id, somenteAtivos);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        List<sComboCurso> lista = (from sComboCurso dr in Seleciona_CursosRelacionados_Por_Escola(cur_id, crr_id, esc_id, uni_id, somenteAtivos, appMinutosCacheLongo)
                                                   let cur_idRelacionado = Convert.ToInt32(dr.cur_crr_id.Split(';')[0])
                                                   where cur_idRelacionado != cur_id
                                                   select dr).ToList();

                        if (lista.Any())
                            dados = lista;
                        else
                            dados = new List<sComboCurso>();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboCurso>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                List<sComboCurso> lista = (from sComboCurso dr in Seleciona_CursosRelacionados_Por_Escola(cur_id, crr_id, esc_id, uni_id, somenteAtivos, appMinutosCacheLongo)
                                           let cur_idRelacionado = Convert.ToInt32(dr.cur_crr_id.Split(';')[0])
                                           where cur_idRelacionado != cur_id
                                           select dr).ToList();

                if (lista.Any())
                    dados = lista;
                else
                    dados = new List<sComboCurso>();
            }

            return dados;
        }

        /// <summary>
        /// Retorna os cur_id e crr_id relacionados ao curso informado,
        /// que tenham ligação com a escola informada e que estejam vigentes.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> Seleciona_CursosRelacionadosVigentes_Por_Escola
        (
            int cur_id
            , int crr_id
            , int esc_id
            , int uni_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCurso> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_Seleciona_CursosRelacionadosVigentes_Por_Escola(cur_id, crr_id, esc_id, uni_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ACA_CursoDAO dao = new ACA_CursoDAO();
                        DataTable dtDados = dao.Seleciona_CursosRelacionadosVigentes_Por_Escola(cur_id, crr_id, esc_id, uni_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboCurso
                                 {
                                     cur_crr_id = dr["cur_crr_id"].ToString(),
                                     cur_crr_nome = dr["cur_crr_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboCurso>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ACA_CursoDAO dao = new ACA_CursoDAO();
                DataTable dtDados = dao.Seleciona_CursosRelacionadosVigentes_Por_Escola(cur_id, crr_id, esc_id, uni_id);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboCurso
                         {
                             cur_crr_id = dr["cur_crr_id"].ToString(),
                             cur_crr_nome = dr["cur_crr_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna os dados relacionados ao curso informado (além dele mesmo),
        /// que tenham ligação com a escola e o período informados.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do período do currículo</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>        
        /// <returns></returns>
        public static List<ACA_CurriculoEscolaPeriodo> SelecionaCursosRelacionados_Por_EscolaCursoPeriodo
        (
            int cur_id
            , int crr_id
            , int crp_id
            , int esc_id
            , int uni_id
        )
        {
            ACA_CursoDAO daoCurso = new ACA_CursoDAO();
            DataTable dt = daoCurso.SelecionaCursosRelacionados_Por_EscolaCursoPeriodo(cur_id, crr_id, crp_id, esc_id, uni_id);

            ACA_CurriculoEscolaPeriodoDAO dao = new ACA_CurriculoEscolaPeriodoDAO();

            return (from DataRow dr in dt.Rows
                    let ent = new ACA_CurriculoEscolaPeriodo()
                    select dao.DataRowToEntity(dr, ent)).ToList();
        }

        /// <summary>
        /// Retorna os cur_id e crr_id relacionados ao curso informado.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        public static DataTable Seleciona_CursosRelacionados
        (
            int cur_id
            , int crr_id
            , TalkDBTransaction banco
        )
        {
            ACA_CursoDAO dao = new ACA_CursoDAO { _Banco = banco };
            return dao.Seleciona_CursosRelacionados(cur_id, crr_id);
        }

        /// <summary>
        /// Envia parametros para metodo [DAO]SelectCursoRelacionadoNiveEnsino.
        /// </summary>
        /// <param name="cur_id">Id curso.</param>
        /// <param name="crr_id">Id do currículo do curso</param>
        /// <param name="tne_id">Id Nível de Ensino.</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>Retorna DataTable com dados dos cursos relacionados com mesmo Nível de Ensino.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectCursoRelacionadoNivelEnsino
        (
            int cur_id
            , int crr_id
            , int tne_id
            , Guid ent_id
        )
        {
            ACA_CursoDAO dao = new ACA_CursoDAO();
            return dao.SelectCursoRelacionadoNivelEnsino(cur_id, crr_id, tne_id, ent_id);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectByEscolasUnidadesEscolaCursoPadrao
        (
            string esc_ids
            , string uni_ids
            , string cur_padrao
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            try
            {
                ACA_CursoDAO dao = new ACA_CursoDAO();
                return dao.SelectBy_esc_ids_uni_ids_cur_padrao(esc_ids, uni_ids, cur_padrao, paginado, currentPage / pageSize, pageSize, out totalRecords);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna datatable com registro dos cursos não excluidos lógicamente.
        /// </summary>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>Datatable de cursos</returns>
        public static DataTable SelecionaCursoNaoExcluido(Guid ent_id)
        {
            try
            {
                ACA_CursoDAO dao = new ACA_CursoDAO();
                return dao.SelecionaCursoNaoExcluido(ent_id);
            }
            catch
            {
                throw;
            }
        }

        #region Cursos da escola

        [Serializable]
        public struct TmpTurnos
        {
            [XmlAttribute]
            public int cur_id { get; set; }

            [XmlIgnore]
            public int crr_id { get; set; }

            [XmlIgnore]
            public int esc_id { get; set; }

            [XmlIgnore]
            public int uni_id { get; set; }

            [XmlIgnore]
            public int crt_id { get; set; }

            [XmlIgnore]
            public int ces_id { get; set; }

            [XmlIgnore]
            public int ttn_id { get; set; }

            [XmlIgnore]
            public string crt_vigenciaInicio { get; set; }

            [XmlIgnore]
            public string crt_vigenciaFim { get; set; }

            [XmlIgnore]
            public string ttn_nome { get; set; }

            [XmlIgnore]
            public bool crt_delete { get; set; }

            [XmlIgnore]
            public bool isNew { get; set; }
        }

        public static void AddTmpTurnos(int cur_id, int crr_id, int esc_id, int uni_id, int ces_id, int crt_id, int ttn_id, string ttn_nome, string vigenciaInicio, string vigenciaFim, bool isNew, IList<TmpTurnos> ltTurnos)
        {
            TmpTurnos tmp = new TmpTurnos();
            tmp.cur_id = cur_id;
            tmp.crr_id = crr_id;
            tmp.esc_id = esc_id;
            tmp.uni_id = uni_id;
            tmp.ces_id = ces_id;
            tmp.ttn_id = ttn_id;
            tmp.ttn_nome = ttn_nome;
            tmp.crt_id = crt_id;
            tmp.crt_vigenciaInicio = vigenciaInicio;
            tmp.crt_vigenciaFim = vigenciaFim;
            tmp.crt_delete = false;
            tmp.isNew = isNew;
            ltTurnos.Add(tmp);
        }

        public static void RemoveTmpTurnos(int cur_id, int crr_id, int esc_id, int uni_id, int crt_id, int ces_id, SortedDictionary<int, List<TmpTurnos>> ltTurnos)
        {
            if (ltTurnos.Count > 0)
            {
                for (int i = 0; i < ltTurnos[cur_id].Count; i++)
                {
                    if (ltTurnos[cur_id][i].ces_id == ces_id && ltTurnos[cur_id][i].crt_id == crt_id
                        && ltTurnos[cur_id][i].cur_id == cur_id && ltTurnos[cur_id][i].crr_id == crr_id
                        && ltTurnos[cur_id][i].esc_id == esc_id && ltTurnos[cur_id][i].uni_id == uni_id)
                    {
                        ltTurnos[cur_id].Remove(ltTurnos[cur_id][i]);
                        break;
                    }
                }
            }
        }

        public static void UpdateTmpTurnos(int cur_id, int crr_id, int esc_id, int uni_id, int ces_id, int crt_id, string crt_vigenciaInicio, string crt_vigenciaFim, SortedDictionary<int, List<TmpTurnos>> ltTurnos)
        {
            for (int i = 0; i < ltTurnos.Values.ToList()[ces_id - 1].Count; i++)
            {
                if (ltTurnos.Values.ToList()[ces_id - 1][i].crt_id == crt_id &&
                    ltTurnos.Values.ToList()[ces_id - 1][i].cur_id == cur_id &&
                    ltTurnos.Values.ToList()[ces_id - 1][i].crr_id == crr_id &&
                    ltTurnos.Values.ToList()[ces_id - 1][i].esc_id == esc_id &&
                    ltTurnos.Values.ToList()[ces_id - 1][i].esc_id == uni_id)
                {
                    SortedDictionary<int, List<TmpTurnos>> lt = ltTurnos;
                    TmpTurnos tmpTurnos = lt[ces_id][i];
                    tmpTurnos.crt_vigenciaInicio = crt_vigenciaInicio;
                    tmpTurnos.crt_vigenciaFim = crt_vigenciaFim;
                    lt[ces_id][i] = tmpTurnos;
                    break;
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int esc_id
            , int uni_id
            , int cur_id
            , string cur_nome
            , string cur_codigo
            , int tne_id
            , int tme_id
            , byte cur_situacao
            , Guid ent_id
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

            try
            {
                ACA_CursoDAO dao = new ACA_CursoDAO();
                return dao.SelectBy_Pesquisa(esc_id, uni_id, cur_id, cur_nome, cur_codigo, tne_id, tme_id, cur_situacao, ent_id, usu_id, gru_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sem paginação
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="cur_id"></param>
        /// <param name="cur_nome"></param>
        /// <param name="cur_codigo"></param>
        /// <param name="tne_id"></param>
        /// <param name="tme_id"></param>
        /// <param name="cur_situacao"></param>
        /// <param name="ent_id"></param>
        /// <param name="usu_id"></param>
        /// <param name="gru_id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect_CadastroCurso
        (
            int esc_id
            , int uni_id
            , int cur_id
            , string cur_nome
            , string cur_codigo
            , int tne_id
            , int tme_id
            , byte cur_situacao
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
        )
        {
            totalRecords = 0;
            try
            {
                ACA_CursoDAO dao = new ACA_CursoDAO();
                return dao.SelectBy_Pesquisa(esc_id, uni_id, cur_id, cur_nome, cur_codigo, tne_id, tme_id, cur_situacao, ent_id, usu_id, gru_id, false, 1, 1, out totalRecords);
            }
            catch
            {
                throw;
            }
        }

        #endregion Cursos da escola

        /// <summary>
        /// Traz todos os cursos e currículos da entidade.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaCursoCurriculo
        (
            Guid ent_id
        )
        {
            ACA_CursoDAO dao = new ACA_CursoDAO();
            return dao.SelectCursoCurriculo(ent_id);
        }

        /// <summary>
        /// Traz todos os cursos que permitem efetivação semestral.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> CursoCurriculoEfetivacaoSemestral
        (
            Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCurso> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_CursoCurriculoEfetivacaoSemestral(ent_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ACA_CursoDAO dao = new ACA_CursoDAO();
                        DataTable dtDados = dao.CursoCurriculoEfetivacaoSemestral(ent_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboCurso
                                 {
                                     cur_crr_id = dr["cur_crr_id"].ToString(),
                                     cur_crr_nome = dr["cur_crr_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboCurso>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ACA_CursoDAO dao = new ACA_CursoDAO();
                DataTable dtDados = dao.CursoCurriculoEfetivacaoSemestral(ent_id);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboCurso
                         {
                             cur_crr_id = dr["cur_crr_id"].ToString(),
                             cur_crr_nome = dr["cur_crr_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os cursos/currículos não excluídas logicamente
        /// Sem paginação
        /// </summary>
        /// <param name="esc_id">Escola do curso</param>
        /// <param name="uni_id">Unidade da escola </param>
        /// <param name="cur_situacao">Situação do curso</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> SelecionaCursoCurriculo
        (
            int esc_id
            , int uni_id
            , byte cur_situacao
            , Guid ent_id
            , bool mostraEJAModalidades
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCurso> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaCursoCurriculo(esc_id, uni_id, cur_situacao, ent_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ACA_CursoDAO dao = new ACA_CursoDAO();
                        DataTable dtDados = dao.SelectCursoCurriculo(esc_id, uni_id, cur_situacao, ent_id, false, 1, 1, out totalRecords, mostraEJAModalidades);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboCurso
                                 {
                                     cur_crr_id = dr["cur_crr_id"].ToString(),
                                     cur_crr_nome = dr["cur_crr_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboCurso>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ACA_CursoDAO dao = new ACA_CursoDAO();
                DataTable dtDados = dao.SelectCursoCurriculo(esc_id, uni_id, cur_situacao, ent_id, false, 1, 1, out totalRecords, mostraEJAModalidades);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboCurso
                         {
                             cur_crr_id = dr["cur_crr_id"].ToString(),
                             cur_crr_nome = dr["cur_crr_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os cursos/currículos não excluídas logicamente
        /// Sem paginação
        /// </summary>
        /// <param name="usu_id">ID do usuário</param>
        /// <param name="gru_id">ID do grupo do usuário</param>
        /// <param name="cur_situacao">Situação do curso</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> SelecionaCursoCurriculoPorUsuario
        (
            Guid usu_id
            , Guid gru_id
            , byte cur_situacao
            , Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCurso> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaCursoCurriculoPorUsuario(usu_id, gru_id, cur_situacao, ent_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ACA_CursoDAO dao = new ACA_CursoDAO();
                        DataTable dtDados = dao.SelectCursoCurriculoPorUsuario(usu_id, gru_id, cur_situacao, ent_id, false, 1, 1, out totalRecords);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboCurso
                                 {
                                     cur_crr_id = dr["cur_crr_id"].ToString(),
                                     cur_crr_nome = dr["cur_crr_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboCurso>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ACA_CursoDAO dao = new ACA_CursoDAO();
                DataTable dtDados = dao.SelectCursoCurriculoPorUsuario(usu_id, gru_id, cur_situacao, ent_id, false, 1, 1, out totalRecords);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboCurso
                         {
                             cur_crr_id = dr["cur_crr_id"].ToString(),
                             cur_crr_nome = dr["cur_crr_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os cursos/currículos não excluídas logicamente
        /// filtrado por escola
        /// </summary>
        /// <param name="esc_id">Escola do curso</param>
        /// <param name="uni_id">Unidade da escola </param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> SelecionaCursoCurriculoPorEscola
        (
            int esc_id
            , int uni_id
            , Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCurso> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaCursoCurriculoPorEscola(esc_id, uni_id, ent_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ACA_CursoDAO dao = new ACA_CursoDAO();
                        DataTable dtDados = dao.SelectCursoCurriculoPorEscola(esc_id, uni_id, ent_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboCurso
                                 {
                                     cur_crr_id = dr["cur_crr_id"].ToString(),
                                     cur_crr_nome = dr["cur_crr_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboCurso>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ACA_CursoDAO dao = new ACA_CursoDAO();
                DataTable dtDados = dao.SelectCursoCurriculoPorEscola(esc_id, uni_id, ent_id);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboCurso
                         {
                             cur_crr_id = dr["cur_crr_id"].ToString(),
                             cur_crr_nome = dr["cur_crr_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os cursos/currículos não excluídas logicamente
        /// e que estão vigentes filtrado por escola
        /// </summary>
        /// <param name="esc_id">Escola do curso</param>
        /// <param name="uni_id">Unidade da escola </param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> SelecionaCursoCurriculoVigentesPorEscola
        (
            int esc_id
            , int uni_id
            , Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCurso> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaCursoCurriculoVigentesPorEscola(esc_id, uni_id, ent_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ACA_CursoDAO dao = new ACA_CursoDAO();
                        DataTable dtDados = dao.SelectCursoCurriculoVigentesPorEscola(esc_id, uni_id, ent_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboCurso
                                 {
                                     cur_crr_id = dr["cur_crr_id"].ToString(),
                                     cur_crr_nome = dr["cur_crr_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboCurso>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ACA_CursoDAO dao = new ACA_CursoDAO();
                DataTable dtDados = dao.SelectCursoCurriculoVigentesPorEscola(esc_id, uni_id, ent_id);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboCurso
                         {
                             cur_crr_id = dr["cur_crr_id"].ToString(),
                             cur_crr_nome = dr["cur_crr_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os cursos/currículos não excluídos logicamente
        /// filtrado por escola
        /// </summary>
        /// <param name="esc_id">Escola do curso</param>
        /// <param name="uni_id">Unidade da escola </param>
        /// <param name="tci_id">Tipo de ciclo</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> SelecionaCursoCurriculoPorEscolaTipoCiclo
        (
            int esc_id
            , int uni_id
            , int tci_id
            , Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCurso> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaCursoCurriculoPorEscolaTipoCiclo(esc_id, uni_id, tci_id, ent_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ACA_CursoDAO dao = new ACA_CursoDAO();
                        DataTable dtDados = dao.SelecionaPorEscolaTipoCiclo(esc_id, uni_id, tci_id, ent_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboCurso
                                 {
                                     cur_crr_id = dr["cur_crr_id"].ToString(),
                                     cur_crr_nome = dr["cur_crr_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboCurso>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ACA_CursoDAO dao = new ACA_CursoDAO();
                DataTable dtDados = dao.SelecionaPorEscolaTipoCiclo(esc_id, uni_id, tci_id, ent_id);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboCurso
                         {
                             cur_crr_id = dr["cur_crr_id"].ToString(),
                             cur_crr_nome = dr["cur_crr_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os cursos/currículos não excluídos logicamente
        /// e que estão vigentes filtrado por escola
        /// </summary>
        /// <param name="esc_id">Escola do curso</param>
        /// <param name="uni_id">Unidade da escola </param>
        /// <param name="tci_id">Tipo de ciclo</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> SelecionaCursoCurriculoVigentesPorEscolaTipoCiclo
        (
            int esc_id
            , int uni_id
            , int tci_id
            , Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCurso> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaCursoCurriculoVigentesPorEscolaTipoCiclo(esc_id, uni_id, tci_id, ent_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ACA_CursoDAO dao = new ACA_CursoDAO();
                        DataTable dtDados = dao.SelecionaVigentesPorEscolaTipoCiclo(esc_id, uni_id, tci_id, ent_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboCurso
                                 {
                                     cur_crr_id = dr["cur_crr_id"].ToString(),
                                     cur_crr_nome = dr["cur_crr_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboCurso>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ACA_CursoDAO dao = new ACA_CursoDAO();
                DataTable dtDados = dao.SelecionaVigentesPorEscolaTipoCiclo(esc_id, uni_id, tci_id, ent_id);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboCurso
                         {
                             cur_crr_id = dr["cur_crr_id"].ToString(),
                             cur_crr_nome = dr["cur_crr_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }


        /// <summary>
        /// Retorna todos os cursos/currículos não excluídos logicamente
        /// filtrado por escola, calendário e tipo de ciclo
        /// </summary>
        /// <param name="esc_id">Escola do curso</param>
        /// <param name="uni_id">Unidade da escola </param>
        /// <param name="tci_id">Tipo de ciclo</param>
        /// <param name="cal_id">Calendario</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> SelecionaCursoCurriculoPorEscolaCalendarioTipoCiclo
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int tci_id
            , Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCurso> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.                    
                    string chave = RetornaChaveCache_SelecionaCursoCurriculoPorEscolaCalendarioTipoCiclo(esc_id, uni_id, cal_id, tci_id, ent_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ACA_CursoDAO dao = new ACA_CursoDAO();
                        DataTable dtDados = dao.SelecionaPorEscolaCalendarioTipoCiclo(esc_id, uni_id, cal_id, tci_id, ent_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboCurso
                                 {
                                     cur_crr_id = dr["cur_crr_id"].ToString(),
                                     cur_crr_nome = dr["cur_crr_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboCurso>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ACA_CursoDAO dao = new ACA_CursoDAO();
                DataTable dtDados = dao.SelecionaPorEscolaCalendarioTipoCiclo(esc_id, uni_id, cal_id, tci_id, ent_id);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboCurso
                         {
                             cur_crr_id = dr["cur_crr_id"].ToString(),
                             cur_crr_nome = dr["cur_crr_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os cursos/currículos não excluídos logicamente
        /// filtrado por escola, ano letivo e tipo de ciclo
        /// </summary>
        /// <param name="esc_id">Escola do curso</param>
        /// <param name="uni_id">Unidade da escola </param>
        /// <param name="tci_id">Tipo de ciclo</param>
        /// <param name="cal_ano">Ano letivo</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> SelecionaCursoCurriculoPorEscolaCalendarioAnoTipoCiclo
        (
            int esc_id
            , int uni_id
            , int cal_ano
            , int tci_id
            , Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            return CacheManager.Factory.Get
                (
                    RetornaChaveCache_SelecionaCursoCurriculoPorEscolaCalendarioAnoTipoCiclo(esc_id, uni_id, cal_ano, tci_id, ent_id)
                    ,
                    delegate ()
                    {
                        ACA_CursoDAO dao = new ACA_CursoDAO();
                        using (DataTable dtDados = dao.SelecionaPorEscolaCalendarioAnoTipoCiclo(esc_id, uni_id, cal_ano, tci_id, ent_id))
                        {
                            return (from DataRow dr in dtDados.Rows
                                    select new sComboCurso
                                    {
                                        cur_crr_id = dr["cur_crr_id"].ToString(),
                                        cur_crr_nome = dr["cur_crr_nome"].ToString()
                                    }).ToList();
                        }
                    }
                    ,
                    appMinutosCacheLongo
                );
        }


        /// <summary>
        /// Retorna todos os cursos/currículos não excluídas logicamente
        /// Filtrado por escola
        /// </summary>
        /// <param name="esc_id">Escola do curso</param>
        /// <param name="uni_id">Unidade da escola </param>
        /// <param name="tne_id">ID do tipo nivel de ensino</param> 
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> SelecionaCursoCurriculoPorEscolaNivelEnsino
        (
            int esc_id
            , int uni_id
            , int tne_id
            , Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCurso> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaCursoCurriculoPorEscolaNivelEnsino(esc_id, uni_id, tne_id, ent_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ACA_CursoDAO dao = new ACA_CursoDAO();
                        DataTable dtDados = dao.SelectCursoCurriculoPorEscolaNivelEnsino(esc_id, uni_id, tne_id, ent_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboCurso
                                 {
                                     cur_crr_id = dr["cur_crr_id"].ToString(),
                                     cur_crr_nome = dr["cur_crr_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboCurso>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ACA_CursoDAO dao = new ACA_CursoDAO();
                DataTable dtDados = dao.SelectCursoCurriculoPorEscolaNivelEnsino(esc_id, uni_id, tne_id, ent_id);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboCurso
                         {
                             cur_crr_id = dr["cur_crr_id"].ToString(),
                             cur_crr_nome = dr["cur_crr_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os cursos/currículos não excluídas logicamente
        /// Filtrado por tipo de nível de ensino
        /// </summary>
        /// <param name="tne_id">ID do tipo nivel de ensino</param> 
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> SelecionaCursoCurriculoPorNivelEnsino
        (
            int tne_id
            , Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCurso> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaCursoCurriculoPorNivelEnsino(tne_id, ent_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ACA_CursoDAO dao = new ACA_CursoDAO();
                        DataTable dtDados = dao.SelectCursoCurriculoPorNivelEnsino(tne_id, ent_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboCurso
                                 {
                                     cur_crr_id = dr["cur_crr_id"].ToString(),
                                     cur_crr_nome = dr["cur_crr_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboCurso>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ACA_CursoDAO dao = new ACA_CursoDAO();
                DataTable dtDados = dao.SelectCursoCurriculoPorNivelEnsino(tne_id, ent_id);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboCurso
                         {
                             cur_crr_id = dr["cur_crr_id"].ToString(),
                             cur_crr_nome = dr["cur_crr_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna um datatable contendo todos os cursos e curriculos do PEJA
        /// que não foram excluídas logicamente, filtrados por
        /// cur_situacao
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade de escola</param>
        /// <param name="cur_situacao">Situacao do curso</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <returns>DataTable com tipos de dependencia paginado</returns>
        public static DataTable SelectCursoCurriculoPorEscolaPEJA(int esc_id, int uni_id, byte cur_situacao, Guid ent_id)
        {
            return new ACA_CursoDAO().SelectCursoCurriculoPorEscolaPEJA(esc_id, uni_id, cur_situacao, ent_id);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os cursos e curriculos
        /// que não foram excluídos logicamente, filtrados (ou não)
        /// por escola e situacao que possuem disciplina eletiva
        /// </summary>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="tds_id">ID do tipo de disciplina</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="cur_situacao">Situação do curso, caso seja passado o valor 0 a situação será desconsiderada</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> SelectCursoComDisciplinaEletiva
        (
            int esc_id
            , int uni_id
            , Guid ent_id
            , int cur_situacao
            , bool mostraEJAModalidades
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCurso> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelectCursoComDisciplinaEletiva(esc_id, uni_id, ent_id, cur_situacao);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ACA_CursoDAO dao = new ACA_CursoDAO();
                        DataTable dtDados = dao.SelectCursoComDisciplinaEletiva(esc_id, uni_id, ent_id, cur_situacao, mostraEJAModalidades);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboCurso
                                 {
                                     cur_crr_id = dr["cur_crr_id"].ToString(),
                                     cur_crr_nome = dr["cur_crr_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboCurso>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ACA_CursoDAO dao = new ACA_CursoDAO();
                DataTable dtDados = dao.SelectCursoComDisciplinaEletiva(esc_id, uni_id, ent_id, cur_situacao, mostraEJAModalidades);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboCurso
                         {
                             cur_crr_id = dr["cur_crr_id"].ToString(),
                             cur_crr_nome = dr["cur_crr_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os cursos/currículos não excluídas logicamente
        /// Sem paginação
        /// </summary>
        /// <param name="esc_id">Escola do curso</param>
        /// <param name="uni_id">Unidade da escola </param>
        /// <param name="cur_situacao">Situação do curso</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="cal_id">Calendario</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> SelecionaCursoCurriculoCalendarioEscola
        (
            int esc_id
            , int uni_id
            , byte cur_situacao
            , Guid ent_id
            , int cal_id
            , bool mostraEJAModalidades
            , int appMinutosCacheLongo = 0            
        )
        {
            List<sComboCurso> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaCursoCurriculoCalendarioEscola(esc_id, uni_id, cur_situacao, ent_id, cal_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ACA_CursoDAO dao = new ACA_CursoDAO();
                        DataTable dtDados = dao.SelectCursoCurriculoCalendarioEscola(esc_id, uni_id, cur_situacao, ent_id, cal_id, mostraEJAModalidades);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboCurso
                                 {
                                     cur_crr_id = dr["cur_crr_id"].ToString(),
                                     cur_crr_nome = dr["cur_crr_nome"].ToString(),
                                     tne_id = Convert.ToInt32(dr["tne_id"].ToString())
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboCurso>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ACA_CursoDAO dao = new ACA_CursoDAO();
                DataTable dtDados = dao.SelectCursoCurriculoCalendarioEscola(esc_id, uni_id, cur_situacao, ent_id, cal_id, mostraEJAModalidades);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboCurso
                         {
                             cur_crr_id = dr["cur_crr_id"].ToString(),
                             cur_crr_nome = dr["cur_crr_nome"].ToString(),
                             tne_id = Convert.ToInt32(dr["tne_id"].ToString())
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os cursos/currículos não excluídas logicamente
        /// Sem paginação
        /// </summary>
        /// <param name="esc_id">Escola do curso</param>
        /// <param name="uni_id">Unidade da escola </param>
        /// <param name="dis_id">Disciplina</param>
        /// <param name="cur_situacao">Situação do curso</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="cal_id">Calendario</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> SelecionaCursoCurriculoCalendarioEscolaDisciplina
        (
            int esc_id
            , int uni_id
            , int dis_id
            , byte cur_situacao
            , Guid ent_id
            , int cal_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCurso> dados = null;

            Func<List<sComboCurso>> retorno = delegate()
            {
                using (DataTable dt = new ACA_CursoDAO().SelecionaPorCalendarioEscolaDisciplina(esc_id, uni_id, dis_id, cur_situacao, ent_id, cal_id))
                {
                    return dt.Rows.Cast<DataRow>()
                                  .Select
                                  (
                                      p => new sComboCurso
                                          {
                                              cur_crr_id = p["cur_crr_id"].ToString(),
                                              cur_crr_nome = p["cur_crr_nome"].ToString()
                                          }
                                  ).ToList();
                }
            };

            if (appMinutosCacheLongo > 0)
            {
                string chave = String.Format(ModelCache.CURSO_CALENDARIO_ESCOLA_DISCIPLINA_MODEL_KEY, esc_id, uni_id, dis_id, cur_situacao, ent_id, cal_id);

                dados = CacheManager.Factory.Get
                    (
                        chave,
                        retorno,
                        appMinutosCacheLongo
                    );
            }
            else
            {
                dados = retorno();
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os cursos/currículos não excluídas logicamente
        /// Sem paginação
        /// </summary>
        /// <param name="esc_id">Escola do curso</param>
        /// <param name="uni_id">Unidade da escola </param>
        /// <param name="cur_situacao">Situação do curso</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="cal_id">Calendario</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> SelecionaCursoCurriculoVigentesCalendarioEscola
        (
            int esc_id
            , int uni_id
            , byte cur_situacao
            , Guid ent_id
            , int cal_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCurso> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaCursoCurriculoVigentesCalendarioEscola(esc_id, uni_id, cur_situacao, ent_id, cal_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ACA_CursoDAO dao = new ACA_CursoDAO();
                        DataTable dtDados = dao.SelectCursoCurriculoVigentesCalendarioEscola(esc_id, uni_id, cur_situacao, ent_id, cal_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboCurso
                                 {
                                     cur_crr_id = dr["cur_crr_id"].ToString(),
                                     cur_crr_nome = dr["cur_crr_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboCurso>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ACA_CursoDAO dao = new ACA_CursoDAO();
                DataTable dtDados = dao.SelectCursoCurriculoVigentesCalendarioEscola(esc_id, uni_id, cur_situacao, ent_id, cal_id);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboCurso
                         {
                             cur_crr_id = dr["cur_crr_id"].ToString(),
                             cur_crr_nome = dr["cur_crr_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Seleciona as etapas de ensino ativas e seus turnos por escola e etapa de ensino.
        /// </summary>
        /// <param name="uad_idSuperior">ID da unidade superior.</param>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaCursosTurnosPorEscola(Guid uad_idSuperior, int esc_id, int uni_id, int cur_id, int crr_id, Guid ent_id, bool mostraCodigoEscola)
        {
            return new ACA_CursoDAO().SelecionaCursosTurnosPorEscola(uad_idSuperior, esc_id, uni_id, cur_id, crr_id, ent_id, mostraCodigoEscola);
        }

        /// <summary>
        /// Verifica se existe o curso na escola
        /// </summary>
        /// <param name="esc_id">Escola do curso</param>
        /// <param name="uni_id">Unidade da escola </param>
        /// <param name="cur_id">Curso da escola </param>
        /// <param name="cur_situacao">Situação do curso</param>
        /// <param name="ent_id"></param>
        /// <returns>
        /// TRUE - o curso passado pelo parametro existe na escola
        /// FALSE - o curso passado pelo parametro NÃO existe na escola
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaCursoExistenteEscola
        (
            int esc_id
            , int uni_id
            , int cur_id
            , byte cur_situacao
            , Guid ent_id
        )
        {
            ACA_CursoDAO dao = new ACA_CursoDAO();
            return dao.VerificaCursoExistenteEscola(esc_id, uni_id, cur_id, cur_situacao, ent_id);
        }

        /// <summary>
        /// Verifica se já existe um curso cadastrado com o mesmo nome
        /// </summary>
        /// <param name="entity">Entidade ACA_Curso</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaCursoExistenteNome
        (
            ACA_Curso entity
        )
        {
            ACA_CursoDAO dao = new ACA_CursoDAO();
            return dao.SelectBy_NomeCurso(entity);
        }

        /// <summary>
        /// Verifica se já existe um curso cadastrado com o mesmo nome
        /// </summary>
        /// <param name="entity">Entidade ACA_Curso</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaCursoExistenteNome
        (
            string cur_nome, int cur_id, Guid ent_id,
            out ACA_Curso entity
        )
        {
            entity = new ACA_Curso();
            ACA_CursoDAO dao = new ACA_CursoDAO();
            return dao.SelectBy_NomeCurso(cur_nome, cur_id, ent_id, out entity);
        }

        /// <summary>
        /// Verifica se já existe um curso cadastrado com o mesmo código
        /// </summary>
        /// <param name="entity">Entidade ACA_Curso</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaCursoExistenteCodigo
        (
            ACA_Curso entity
        )
        {
            ACA_CursoDAO dao = new ACA_CursoDAO();
            return dao.SelectBy_CodigoCurso(entity);
        }

        /// <summary>
        /// Verifica se existe algum aluno curriculo para o curso
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaAlunoCurriculo
        (
            int cur_id
            , int crr_id
        )
        {
            ACA_CursoDAO dao = new ACA_CursoDAO();
            return dao.SelectBy_VerificaAlunoCurriculo(cur_id, crr_id);
        }

        /// <summary>
        /// Verifica se existe alguma turma para o curso
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaTurma
        (
            int cur_id
            , int crr_id
        )
        {
            ACA_CursoDAO dao = new ACA_CursoDAO();
            return dao.SelectBy_VerificaTurma(cur_id, crr_id);
        }

        /// <summary>
        /// Retorna todos os cursos/currículos não excluídas logicamente
        /// por modalidade de ensino
        /// </summary>
        /// <param name="tme_id">id tipo da modalidade de ensino</param>   
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboCurso> Seleciona_Cursos_Por_ModalidadeEnsino
        (
            int tme_id
            , int esc_id
            , int uni_id
            , Guid ent_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboCurso> dados = null;

            Func<List<sComboCurso>> retorno = delegate()
            {
                using (DataTable dt = new ACA_CursoDAO().Seleciona_Cursos_Por_ModalidadeEnsino(tme_id, esc_id, uni_id, ent_id))
                {
                    return dt.Rows.Cast<DataRow>()
                                  .Select
                                  (
                                      p => new sComboCurso
                                      {
                                          cur_crr_id = p["cur_crr_id"].ToString(),
                                          cur_crr_nome = p["cur_crr_nome"].ToString()
                                      }
                                  ).ToList();
                }
            };

            if (appMinutosCacheLongo > 0)
            {
                string chave = String.Format(ModelCache.CURSO_MODALIDADE_ENSINO_MODEL_KEY, tme_id, esc_id, uni_id ,ent_id);

                dados = CacheManager.Factory.Get
                    (
                        chave,
                        retorno,
                        appMinutosCacheLongo
                    );
            }
            else
            {
                dados = retorno();
            }

            return dados;
        }


        /// <summary>
        /// Classe de excessão referente à entidade ACA_Curso.
        /// Utilizada nas telas de cadastro, para identificar se houve erro de validação
        /// na entidade do curso.
        /// </summary>
        public class ACA_Curso_ValidationException : ValidationException
        {
            public ACA_Curso_ValidationException(string message)
                : base(message)
            {
            }
        }

        #region Plataforma de Itens e Avaliações

        /// <summary>
        /// Retorna os cursos de acordo com entidade e calendario
        /// </summary>
        /// <param name="ent_id">Entidade</param>
        /// <param name="cal_id">Calendario</param>
        public static DataTable BuscaCursoPorEntidadeCalendario
        (
            Guid ent_id
            , int cal_id
        )
        {
            ACA_CursoDAO dao = new ACA_CursoDAO();
            return dao.Seleciona_CursosEntidadeCalendario(ent_id, cal_id);
        }

        /// <summary>
        /// Retorna os cursos de acordo com entidade e calendario
        /// </summary>
        /// <param name="ent_id">Entidade</param>
        /// <param name="cal_id">Calendario</param>
        public static DataTable BuscaCursoPorEntidadeCalendarioEscola
        (
            Guid ent_id
            , int cal_id
            , int esc_id
        )
        {
            ACA_CursoDAO dao = new ACA_CursoDAO();
            return dao.Seleciona_CursosEntidadeCalendarioEscola(ent_id, cal_id, esc_id);
        }

        #endregion Plataforma de Itens e Avaliações
    }
}