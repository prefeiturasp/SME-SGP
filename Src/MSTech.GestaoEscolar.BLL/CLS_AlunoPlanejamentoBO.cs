/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using MSTech.Data.Common;
    using System.Data;
    using System;
    using System.Linq;
    using MSTech.Validation.Exceptions;
    using System.Web;

    #region Estruturas

    [Serializable]
    public struct sTurmasAluno
    {
        public long tur_id { get; set; }
        public string turma { get; set; }
    }

    #endregion

    #region Estruturas

    [Serializable]
    public struct AlunoPlanejamento
    {
        public long alu_id { get; set; }

        public long tud_id { get; set; }

        public int apl_id { get; set; }

        public string apl_planejamento { get; set; }

        public List<AlunoPlanejamentoRelacionada> AlunoPlanejamentoRelacionada { get; set; }

        public bool alterado { get; set; }
    }

    [Serializable]
    public struct AlunoPlanejamentoRelacionada
    {
        public long alu_id { get; set; }

        public long tud_id { get; set; }

        public int apl_id { get; set; }

        public bool Relacionado { get; set; }

        public long tud_idRelacionado { get; set; }

        public string dis_nome { get; set; }

        public bool alterado { get; set; }
    }

    #endregion Estruturas


    /// <summary>
    /// Description: CLS_AlunoPlanejamento Business Object. 
    /// </summary>
    public class CLS_AlunoPlanejamentoBO : BusinessBase<CLS_AlunoPlanejamentoDAO, CLS_AlunoPlanejamento>
    {
        public const string ChaveCache_SelecionarTurmasAluno = "Cache_SelecionarTurmasAluno";

        public static string RetornaChaveCache_SelecionarTurmasAluno(long alu_id, int esc_id, int cal_id)
        {
            return string.Format(ChaveCache_SelecionarTurmasAluno + "_{0}_{1}_{2}", alu_id, esc_id, cal_id);
        }

        /// <summary>
        /// Salva o planejamento do aluno e as turmadisciplinas relacionadas
        /// </summary>
        /// <param name="alunoPlanejamento">planejamento do aluno</param>
        /// <param name="lstRelacionada">lista de turmadisciplinas relacionadas</param>
        /// <returns></returns>
        public static bool Salvar(List<AlunoPlanejamento> ListaAlunoPlanejamento)
        {
            TalkDBTransaction banco = new CLS_AlunoPlanejamentoDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            List<String> lstAlunoPlanejamento = new List<String>();
            List<String> lstTurmaDisciplina = new List<String>();
            List<String> lstIdsPlanejamento = new List<String>();
            List<CLS_AlunoPlanejamentoRelacionada> lstAlunoPlanejamentoRelacionada = new List<CLS_AlunoPlanejamentoRelacionada>();

            try
            {
                List<AlunoPlanejamento> ListaAlunoPlanejamentoAux = CLS_AlunoPlanejamentoBO.SelecionaPlanejamentoAlunoRelacionados(ListaAlunoPlanejamento.FirstOrDefault().tud_id);

                foreach (AlunoPlanejamento alunoPlanejamento in ListaAlunoPlanejamento)
                {
                    if (alunoPlanejamento.alterado)
                    {
                        CLS_AlunoPlanejamento AlunoPlanejamentoAux = new CLS_AlunoPlanejamento
                        {
                            alu_id = alunoPlanejamento.alu_id
                            ,
                            apl_id = alunoPlanejamento.apl_id
                            ,
                            tud_id = alunoPlanejamento.tud_id
                            ,
                            apl_planejamento = alunoPlanejamento.apl_planejamento
                            ,
                            IsNew = ListaAlunoPlanejamentoAux.Find(p => p.alu_id == alunoPlanejamento.alu_id && p.tud_id == alunoPlanejamento.tud_id).apl_id <= 0
                        };
                        Save(AlunoPlanejamentoAux, banco);
                    }
                }

                ListaAlunoPlanejamentoAux = CLS_AlunoPlanejamentoBO.SelecionaPlanejamentoAlunoRelacionados(ListaAlunoPlanejamento.FirstOrDefault().tud_id, banco);

                foreach (AlunoPlanejamento alunoPlanejamento in ListaAlunoPlanejamento)
                {
                    int apl_id = ListaAlunoPlanejamentoAux.Find(p => p.alu_id == alunoPlanejamento.alu_id && p.tud_id == alunoPlanejamento.tud_id).apl_id;

                    if (string.IsNullOrEmpty(ListaAlunoPlanejamentoAux.Find(p => p.alu_id == alunoPlanejamento.alu_id && p.tud_id == alunoPlanejamento.tud_id).apl_planejamento)
                        ||
                        (alunoPlanejamento.AlunoPlanejamentoRelacionada.Any(p => p.alterado)))
                    {
                        // adiciono nas listas
                        lstAlunoPlanejamento.Add(alunoPlanejamento.alu_id.ToString());
                        lstTurmaDisciplina.Add(alunoPlanejamento.tud_id.ToString());
                        lstIdsPlanejamento.Add((apl_id > 0 ? apl_id.ToString() : "0"));
                    }

                    if (alunoPlanejamento.AlunoPlanejamentoRelacionada.Any(p => p.alterado))
                    {
                        //CLS_AlunoPlanejamentoRelacionadaBO.LimparRelacionadas(alunoPlanejamento.alu_id, alunoPlanejamento.tud_id, apl_id, banco);

                        foreach (AlunoPlanejamentoRelacionada alunoPlanejamentoRelacionada in alunoPlanejamento.AlunoPlanejamentoRelacionada)
                        {
                            if (alunoPlanejamentoRelacionada.Relacionado)
                            {
                                CLS_AlunoPlanejamentoRelacionada alunoPlanejamentoRelacionadoAux = new CLS_AlunoPlanejamentoRelacionada
                                {
                                    alu_id = alunoPlanejamentoRelacionada.alu_id
                                                    ,
                                    apl_id = apl_id
                                                    ,
                                    tud_id = alunoPlanejamentoRelacionada.tud_id
                                                    ,
                                    tud_idRelacionado = alunoPlanejamentoRelacionada.tud_idRelacionado
                                                    ,
                                    IsNew = true
                                };
                                lstAlunoPlanejamentoRelacionada.Add(alunoPlanejamentoRelacionadoAux);
                                //CLS_AlunoPlanejamentoRelacionadaBO.Save(alunoPlanejamentoRelacionadoAux, banco);
                            }
                        }
                    }
                }

                // chama rotina para realizar a remoção dos alunos.
                if (lstAlunoPlanejamento.Count > 0)
                {
                    CLS_AlunoPlanejamentoRelacionadaBO.LimparRelacionadas(lstAlunoPlanejamento,lstTurmaDisciplina,lstIdsPlanejamento,banco);                    
                }

                // chama rotina de gravação dos alunos. 
                foreach (CLS_AlunoPlanejamentoRelacionada alunoPlanejamentoAux in lstAlunoPlanejamentoRelacionada)
                {
                    CLS_AlunoPlanejamentoRelacionadaBO.Save(alunoPlanejamentoAux, banco);
                }

                return true;
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                banco.Close();
            }
        }

        /// <summary>
        /// Carrega os planejamentos do aluno na turma
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <returns></returns>
        public static DataTable SelecionaPlanejamentoTurmaAluno(long tur_id, long alu_id)
        {
            CLS_AlunoPlanejamentoDAO dao = new CLS_AlunoPlanejamentoDAO();
            return dao.SelecionaPlanejamentoTurmaAluno(tur_id, alu_id);
        }

        /// <summary>
        /// Busca os planejamentos dos alunos da turma
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="alu_id">ID do aluno</param>
        /// <returns></returns>
        public static List<CLS_AlunoPlanejamento> SelecionaPlanejamentoPorTurma(long tur_id)
        {
            CLS_AlunoPlanejamentoDAO dao = new CLS_AlunoPlanejamentoDAO();
            return dao.SelecionaPlanejamentoPorTurma(tur_id);
        }

        /// <summary>
        /// Carrega as turmas que o aluno está atribuído
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="esc_id">ID da escola que está consultando</param>
        /// <param name="cal_id">ID do calendário para pegar o ano das turmas que serão consultadas</param>
        /// <param name="appMinutosCacheLongo">Tempo de cache longo</param>
        /// <returns></returns>
        public static List<sTurmasAluno> SelecionarTurmasAluno(long alu_id, int esc_id, int cal_id, int appMinutosCacheLongo)
        {
            List<sTurmasAluno> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionarTurmasAluno(alu_id, esc_id, cal_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        CLS_AlunoPlanejamentoDAO dao = new CLS_AlunoPlanejamentoDAO();
                        DataTable dt = dao.SelecionarTurmasAluno(alu_id, esc_id, cal_id);
                        dados = new List<sTurmasAluno>();
                        foreach (DataRow dr in dt.Rows)
                            dados.Add(new sTurmasAluno { tur_id = Convert.ToInt64(dr["tur_id"]), turma = dr["turma"].ToString() });

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sTurmasAluno>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                CLS_AlunoPlanejamentoDAO dao = new CLS_AlunoPlanejamentoDAO();
                DataTable dt = dao.SelecionarTurmasAluno(alu_id, esc_id, cal_id);
                dados = new List<sTurmasAluno>();
                foreach (DataRow dr in dt.Rows)
                    dados.Add(new sTurmasAluno { tur_id = Convert.ToInt64(dr["tur_id"]), turma = dr["turma"].ToString() });
            }

            return dados;
        }

        /// <summary>
        /// Seleciona o planejamento do aluno na turmadisciplina
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="tud_id">ID da turmadisciplina</param>
        /// <returns></returns>
        public static CLS_AlunoPlanejamento SelecionaPlanejamentoAluno(long alu_id, long tud_id)
        {
            CLS_AlunoPlanejamentoDAO dao = new CLS_AlunoPlanejamentoDAO();
            return dao.SelecionaPlanejamentoAluno(alu_id, tud_id);
        }

        /// <summary>
        /// Seleciona os alunos pelo turmadisciplina
        /// </summary>
        /// <param name="tud_id">ID da turmadisciplina</param>
        /// <returns></returns>
        public static DataTable SelecionaAlunosPorTud(long tud_id, bool documentoOficial, string tur_ids = null)
        {
            CLS_AlunoPlanejamentoDAO dao = new CLS_AlunoPlanejamentoDAO();

            DataTable dtTurma = TUR_Turma.TipoTabela_Turma();

            if (!string.IsNullOrEmpty(tur_ids))
            {
                string[] ltTurIds = tur_ids.Split(';');

                ltTurIds.ToList().ForEach
                     (
                         tur_id =>
                         {
                             DataRow dr = dtTurma.NewRow();
                             dr["tur_id"] = tur_id;
                             dtTurma.Rows.Add(dr);
                         }
                     );
            }

            return dao.SelecionaAlunosPorTud(tud_id, documentoOficial, dtTurma);
        }


        /// <summary>
        /// Seleciona o planejamento dos alunos na turmadisciplina com os relacionados
        /// </summary>
        /// <param name="tud_id">ID da turmadisciplina</param>
        /// <returns></returns>
        public static List<AlunoPlanejamento> SelecionaPlanejamentoAlunoRelacionados(long tud_id, TalkDBTransaction banco = null, string tur_ids = null)
        {
            CLS_AlunoPlanejamentoDAO dao = new CLS_AlunoPlanejamentoDAO();
            if (banco != null)
                dao._Banco = banco;

            DataTable dtTurma = TUR_Turma.TipoTabela_Turma();

            if (!string.IsNullOrEmpty(tur_ids))
            {
                string[] ltTurIds = tur_ids.Split(';');

                ltTurIds.ToList().ForEach
                     (
                         tur_id =>
                         {
                             DataRow dr = dtTurma.NewRow();
                             dr["tur_id"] = tur_id;
                             dtTurma.Rows.Add(dr);
                         }
                     );
            }

            DataTable dtAlunoPlanejamento = dao.SelecionaPlanejamentoAlunoRelacionados(tud_id, dtTurma);

            return (from DataRow dr in dtAlunoPlanejamento.Rows
                    group dr by Convert.ToInt32(dr["alu_id"]) into grupo
                    select new AlunoPlanejamento
                    {
                        alu_id = Convert.ToInt64(grupo.First()["alu_id"])
                        ,
                        tud_id = Convert.ToInt64(grupo.First()["tud_id"])
                        ,
                        apl_id = Convert.ToInt32(grupo.First()["apl_id"])
                        ,
                        apl_planejamento = grupo.First()["apl_planejamento"].ToString()
                        ,
                        AlunoPlanejamentoRelacionada = !string.IsNullOrEmpty(grupo.First()["Relacionado"].ToString()) ?
                                                        (from DataRow drr in dtAlunoPlanejamento.Rows
                                                         where Convert.ToInt64(drr["alu_id"]) == Convert.ToInt64(grupo.First()["alu_id"])
                                                         where Convert.ToInt64(drr["tud_id"]) == Convert.ToInt64(grupo.First()["tud_id"])
                                                         where Convert.ToInt32(drr["apl_id"]) == Convert.ToInt32(grupo.First()["apl_id"])
                                                         select new AlunoPlanejamentoRelacionada
                                                         {
                                                             alu_id = Convert.ToInt64(drr["alu_id"])
                                                             ,
                                                             tud_id = Convert.ToInt64(drr["tud_id"])
                                                             ,
                                                             apl_id = Convert.ToInt32(drr["apl_id"])
                                                             ,
                                                             Relacionado = Convert.ToBoolean(drr["Relacionado"])
                                                             ,
                                                             tud_idRelacionado = Convert.ToInt64(drr["tud_idRelacionado"])
                                                             ,
                                                             dis_nome = drr["dis_nome"].ToString()
                                                             ,
                                                             alterado = false
                                                         }).ToList() :
                                                        new List<AlunoPlanejamentoRelacionada>()
                        ,
                        alterado = false
                    }).ToList();
        }

        #region Métodos para incluir / alterar

        /// <summary>
        /// O método salva os dados da entidade CLS_AlunoPlanejamento.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public new static bool Save(CLS_AlunoProjeto entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                return new CLS_AlunoProjetoDAO { _Banco = banco }.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        #endregion
    }
}