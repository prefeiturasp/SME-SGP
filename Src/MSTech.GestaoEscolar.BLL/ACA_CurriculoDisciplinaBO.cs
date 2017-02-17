using System;
using System.Data;
using System.Web;
using System.Web.Caching;
using System.ComponentModel;
using System.Collections.Generic;
using MSTech.Data.Common;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.CustomResourceProviders;
using System.Linq;
namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// Tipo de disciplina para o currículo do curso.
    /// </summary>
    public enum ACA_CurriculoDisciplinaTipo : byte
    {
        Obrigatoria = 1
        ,
        Optativa = 3
        ,
        Eletiva = 4
        ,
        DisciplinaPrincipal = 5
        ,
        DocenteTurmaObrigatoria = 6
        ,
        DocenteTurmaEletiva = 7
        ,
        DependeDisponibilidadeProfessorObrigatoria = 8
        ,
        DependeDisponibilidadeProfessorEletiva = 9
        ,
        DisciplinaEletivaAluno = 10
        ,
        Regencia = 11
        ,
        ComponenteRegencia = 12
        ,
        DocenteEspecificoComplementacaoRegencia = 13
        ,
        DisciplinaMultisseriada = 14
        ,
        MultisseriadaDocente = 15
        ,
        MultisseriadaAluno = 16
        ,
        DocenciaCompartilhada = 17
        ,
        Experiencia = 18
        ,
        TerritorioSaber = 19
    }

    /// <summary>
    /// Situações da disciplina do currículo do curso.
    /// </summary>
    public enum ACA_CurriculoDisciplinaSituacao : byte
    {
        Ativo = 1
        ,
        Excluido = 3
    }

    /// <summary>
    /// Estrutura para salvar as disciplinas eletivas dos alunos
    /// </summary>
    [Serializable]
    public struct ACA_CurriculoDisciplina_Cadastro
    {
        public ACA_Disciplina entityDisciplina;
        public List<ACA_CurriculoDisciplina> listCurriculoDisciplina;
        public List<ACA_DisciplinaMacroCampoEletivaAluno> listMacroCampo;
    }

    [Serializable]
    public struct ACA_CurriculoDisciplina_DisciplinasBase
    {
        public int cur_id;
        public int crr_id;
        public int crp_id;
        public byte tds_base;
        public List<ACA_Disciplina> listaDisciplina;
    }

    public class ACA_CurriculoDisciplinaBO : BusinessBase<ACA_CurriculoDisciplinaDAO, ACA_CurriculoDisciplina>
    {
        /// <summary>
        /// Seleciona as disciplinas das grades curriculares
        /// </summary>
        /// <param name="listaCurriculoPeriodo"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static List<ACA_CurriculoDisciplina_DisciplinasBase> SelecionaPorGradesCurriculares(List<ACA_CurriculoPeriodo> listaCurriculoPeriodo, TalkDBTransaction banco = null)
        {
            ACA_CurriculoDisciplinaDAO dao = banco == null ?
                new ACA_CurriculoDisciplinaDAO() : new ACA_CurriculoDisciplinaDAO { _Banco = banco };

            if (banco == null)
            {
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            }

            try
            {
                List<ACA_CurriculoDisciplina_DisciplinasBase> dados = new List<ACA_CurriculoDisciplina_DisciplinasBase>();

                using (DataTable dtCurriculoPeriodo = ACA_CurriculoPeriodo.TipoTabela_CurriculoPeriodo())
                {
                    listaCurriculoPeriodo.ForEach(p => dtCurriculoPeriodo.Rows.Add(p.EntityToDataRow_TipoTabela_CurriculoPeriodo(dtCurriculoPeriodo)));

                    using (DataTable dtDados = dao.SelecionaPorGradesCurriculares(dtCurriculoPeriodo))
                    {
                        dados = (from DataRow dr in dtDados.Select()
                                 group dr by new
                                 {
                                     cur_id = Convert.ToInt32(dr["cur_id"])
                                     ,
                                     crr_id = Convert.ToInt32(dr["crr_id"])
                                     ,
                                     crp_id = Convert.ToInt32(dr["crp_id"])
                                     ,
                                     tds_base = Convert.ToByte(dr["tds_base"])
                                 } into grupo
                                 select new ACA_CurriculoDisciplina_DisciplinasBase
                                 {
                                     cur_id = grupo.Key.cur_id
                                     ,
                                     crr_id = grupo.Key.crr_id
                                     ,
                                     crp_id = grupo.Key.crp_id
                                     ,
                                     tds_base = grupo.Key.tds_base
                                     ,
                                     listaDisciplina = grupo.Select(p => (ACA_Disciplina)GestaoEscolarUtilBO.DataRowToEntity(p, new ACA_Disciplina())).ToList()
                                 }).ToList();
                    }
                }

                return dados;
            }
            catch (Exception ex)
            {
                if (banco == null)
                {
                    dao._Banco.Close(ex);
                }
                throw;
            }
            finally
            {
                if (banco == null && dao._Banco.ConnectionIsOpen)
                {
                    dao._Banco.Close();
                }
            }
        }


        /// <summary>
        /// Retorna as disciplinas cadastradas para o CurriculoPeriodo do curso 
        /// que sejam de qualquer tipo.
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Curso_TodosTipos
        (
            int cur_id
            , int crr_id
            , int crp_id
            , long tur_id
        )
        {
            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO();

            if (tur_id <= 0)
            {
                // Se for nova turma, verificar se está em cache, não buscar do banco.
                string chave = String.Format("DisciplinasBy_CursoPeriodo_{0};{1};{2}",
                                             cur_id, crr_id, crp_id);

                object cache = HttpContext.Current != null
                    ? HttpContext.Current.Cache[chave]
                    : null;

                DataTable dt;

                if (cache == null)
                {
                    // Busca os dados para guardar em cache.
                    dt = dao.SelectBy_Curso_TodosTipos(cur_id, crr_id, crp_id, tur_id);

                    // Guarda resultado da consulta em cache.
                    HttpContext.Current.Cache.Insert(chave, dt, null, DateTime.Now.AddDays(1),
                                                     Cache.NoSlidingExpiration);
                }
                else
                {
                    // Recupera do cache.
                    dt = (DataTable)cache;
                }

                return dt;
            }

            return dao.SelectBy_Curso_TodosTipos(cur_id, crr_id, crp_id, tur_id);
        }


        /// <summary>
        /// Retorna as disciplinas cadastradas para o CurriculoPeriodo do curso 
        /// que sejam do tipo informado.
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Curso_Tipo
        (
            int cur_id
            , int crr_id
            , int crp_id
            , ACA_CurriculoDisciplinaTipo tipo
            , long tur_id
        )
        {
            if (tur_id <= 0)
            {
                // Busca de todos os tipos, verificando se está guardado em cache.
                DataTable dtTodosTipos = GetSelectBy_Curso_TodosTipos(cur_id, crr_id, crp_id, tur_id);

                DataView dv = dtTodosTipos.DefaultView;
                dv.RowFilter = "crd_tipo = " + (byte)tipo;

                return dv.ToTable();
            }

            // Faz a busca no banco, pois é alteração da turma.
            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO();
            return dao.SelectBy_Curso_Tipo(cur_id, crr_id, crp_id, Convert.ToByte(tipo), tur_id);
        }

        /// <summary>
        /// Retorna as CurriculoDisciplina cadastradas cuja disciplina seja do tipo informado.
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_CurriculoDisciplina> GetSelectBy_Disciplina(int dis_id, bool somenteAtivos = true, TalkDBTransaction banco = null)
        {
            var lista = new List<ACA_CurriculoDisciplina>();

            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO();
            if (banco != null)
            {
                dao._Banco = banco;
            }
            DataTable data = dao.SelectBy_dis_id(dis_id, somenteAtivos);

            foreach (DataRow row in data.Rows)
            {
                lista.Add(new ACA_CurriculoDisciplina()
                            {
                                cur_id = Convert.ToInt32(row["cur_id"]),
                                crr_id = Convert.ToInt32(row["crr_id"]),
                                crp_id = Convert.ToInt32(row["crp_id"]),
                                dis_id = Convert.ToInt32(row["dis_id"]),
                                crd_tipo = Convert.ToByte(row["crd_tipo"]),
                                crd_situacao = Convert.ToByte(row["crd_situacao"]),
                                crd_dataCriacao = Convert.ToDateTime(row["crd_dataCriacao"]),
                                crd_dataAlteracao = Convert.ToDateTime(row["crd_dataAlteracao"]),

                            });
            }

            return lista;
        }

        /// <summary>
        /// Retorna as disciplinas cadastradas para o CurriculoPeriodo do curso        
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDisciplinasParaFormacaoTurmaNormal
        (
            int cur_id
            , int crr_id
            , int crp_id
        )
        {
            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO();
            return dao.SelecionaDisciplinasParaFormacaoTurmaNormal(cur_id, crr_id, crp_id);
        }

        /// <summary>
        /// Retorna um datatable contendo todos as disciplinas curriculo/curso
        /// que não foram excluídos logicamente, filtrados por 
        /// id do curso, id do curriculo
        /// </summary>
        /// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
        /// <param name="crr_id">Id da tabela ACA_Curriculo do bd</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param> 
        /// <returns>DataTable com as disciplinas do curriculo</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int cur_id
            , int crr_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO();
            return dao.SelectBy_cur_id_crr_id(cur_id, crr_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todos as disciplinas curriculo/curso
        /// que não foram excluídos logicamente, filtrados por 
        /// id do curso, id do curriculo
        /// </summary>
        /// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
        /// <param name="crr_id">Id da tabela ACA_Curriculo do bd</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param> 
        /// <returns>DataTable com as disciplinas do curriculo</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect_Disciplinas
        (
            int cur_id
            , int crr_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO();
            return dao.Select_Disciplinas(cur_id, crr_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todos as disciplinas curso
        /// que não foram excluídos logicamente, filtrados por 
        /// id do curso
        /// </summary>
        /// <param name="cur_id">Id da tabela ACA_Curso do bd</param>        
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param> 
        /// <returns>DataTable com as disciplinas do curso</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            int cur_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO();
            return dao.SelectBy_cur_id(cur_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
            string cur_id_crr_id_crp_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            int cur_id = 0;
            int crr_id = 0;
            int crp_id = 0;

            if (!string.IsNullOrEmpty(cur_id_crr_id_crp_id))
            {
                cur_id = Convert.ToInt32(cur_id_crr_id_crp_id.Split(';')[0]);
                crr_id = Convert.ToInt32(cur_id_crr_id_crp_id.Split(';')[1]);
                crp_id = Convert.ToInt32(cur_id_crr_id_crp_id.Split(';')[2]);
            }

            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ACA_CurriculoDisciplinaDAO dal = new ACA_CurriculoDisciplinaDAO();
            return dal.SelectBy_cur_id_crr_id_crp_id(cur_id, crr_id, crp_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna os dados das disciplinas eletivas do aluno
        /// </summary>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do curriculo</param>
        public static List<ACA_CurriculoDisciplina_Cadastro> SelecionaDisciplinasEletivasAlunos
        (
            int cur_id
            , int crr_id
        )
        {
            // Inicializa as lista de cadastro de disciplina eletiva do aluno
            List<ACA_CurriculoDisciplina_Cadastro> list = new List<ACA_CurriculoDisciplina_Cadastro>();
            ACA_CurriculoDisciplina_Cadastro cad = new ACA_CurriculoDisciplina_Cadastro();
            cad.entityDisciplina = new ACA_Disciplina();

            // Inicializa as DAO's 
            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO();
            ACA_DisciplinaDAO daoDis = new ACA_DisciplinaDAO();

            // Carrega as disciplinas eletivas do aluno
            DataTable dt = daoDis.SelectBy_EletivasAlunos(cur_id, crr_id);

            foreach (DataRow dr in dt.Rows)
            {
                cad.entityDisciplina = daoDis.DataRowToEntity(dr, cad.entityDisciplina);

                // Carrega os períodos das disciplinas eletivas do aluno
                cad.listCurriculoDisciplina = new List<ACA_CurriculoDisciplina>();
                DataTable dtPeriodos = dao.SelectBy_EletivasAlunos(cur_id, crr_id, cad.entityDisciplina.dis_id);
                foreach (DataRow dr2 in dtPeriodos.Rows)
                {
                    ACA_CurriculoDisciplina ent = new ACA_CurriculoDisciplina();
                    ent = dao.DataRowToEntity(dr2, ent);
                    cad.listCurriculoDisciplina.Add(ent);
                }

                List<ACA_DisciplinaMacroCampoEletivaAluno> listamacro = new List<ACA_DisciplinaMacroCampoEletivaAluno>();


                DataTable dt2 = ACA_TipoMacroCampoEletivaAlunoBO.SelecionaMacroCamposAssociado(cad.entityDisciplina.dis_id);
                foreach (DataRow dr2 in dt2.Rows)
                {
                    ACA_DisciplinaMacroCampoEletivaAluno entity = new ACA_DisciplinaMacroCampoEletivaAluno
                    {
                        dis_id = cad.entityDisciplina.dis_id,
                        tea_id = Convert.ToInt32(dr2["tea_id"])
                    };
                    listamacro.Add(entity);
                }
                cad.listMacroCampo = listamacro;
                // Adiciona os dados na lista de cadastro
                list.Add(cad);

            }


            // Retorna a lista de cadastro
            return list;
        }

        /// <summary>
        /// Retorna os períodos da disciplina eletiva do aluno
        /// </summary>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do curriculo</param>
        /// <param name="dis_id">Id da disciplina</param>
        public static List<ACA_CurriculoDisciplina> SelecionaListaPeriodosDisciplinasEletivasAlunos
        (
            int cur_id
            , int crr_id
            , int dis_id
        )
        {
            List<ACA_CurriculoDisciplina> list = new List<ACA_CurriculoDisciplina>();

            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO();
            DataTable dt = dao.SelectBy_EletivasAlunos(cur_id, crr_id, dis_id);

            foreach (DataRow dr in dt.Rows)
            {
                ACA_CurriculoDisciplina ent = new ACA_CurriculoDisciplina();
                ent = dao.DataRowToEntity(dr, ent);

                list.Add(ent);
            }

            // Retorna a lista de períodos da disciplina eletiva do aluno
            return list;
        }

        /// <summary>
        /// Retorna os períodos da disciplina eletiva do aluno
        /// </summary>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do curriculo</param>
        /// <param name="dis_id">Id da disciplina</param>
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        public static List<ACA_CurriculoDisciplina> SelecionaListaPeriodosDisciplinasEletivasAlunos
        (
            int cur_id
            , int crr_id
            , int dis_id
            , TalkDBTransaction banco
        )
        {
            List<ACA_CurriculoDisciplina> list = new List<ACA_CurriculoDisciplina>();

            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO { _Banco = banco };
            DataTable dt = dao.SelectBy_EletivasAlunos(cur_id, crr_id, dis_id);

            foreach (DataRow dr in dt.Rows)
            {
                ACA_CurriculoDisciplina ent = new ACA_CurriculoDisciplina();
                ent = dao.DataRowToEntity(dr, ent);

                list.Add(ent);
            }

            // Retorna a lista de períodos da disciplina eletiva do aluno
            return list;
        }

        /// <summary>
        /// Retorna os períodos do curso que possuem a disciplina eletiva do aluno passada
        /// por parâmetro.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="dis_id">ID da disciplina eletiva do aluno</param>
        /// <returns></returns>
        public static DataTable SelecionaPeriodosPor_Escola_EletivaAluno
        (
            int cur_id
            , int crr_id
            , int esc_id
            , int uni_id
            , int dis_id
        )
        {
            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO();
            return dao.SelectBy_Escola_EletivasAlunos(cur_id, crr_id, esc_id, uni_id, dis_id);
        }

        /// <summary>
        /// Retorna as disciplinas cadastradas para o curso, que sejam 
        /// do tipo informado e que possuam uma disciplina equivalente no curso da nova matriz curricular.
        /// </summary>
        /// <param name="cur_idOrigem">ID do curso de origem.</param>
        /// <param name="crr_idOrigem">ID do currículo de origem.</param>
        /// <param name="crp_idOrigem">ID do grupamento de origem.</param>
        /// <param name="crd_tipo">Tipo de currículo disciplina.</param>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="cur_idDestino">ID do curso da nova matriz curricular.</param>
        /// <param name="crr_idDestino">ID do currículo da nova matriz curricular.</param>
        /// <returns></returns>
        public static DataTable SelecionaCursosPorNovaMatrizCurricularTipo
        (
            int cur_idOrigem,
            int crr_idOrigem,
            int crp_idOrigem,
            ACA_CurriculoDisciplinaTipo crd_tipo,
            long tur_id,
            int cur_idDestino,
            int crr_idDestino
        )
        {
            return new ACA_CurriculoDisciplinaDAO().SelecionaCursosPorNovaMatrizCurricularTipo
                (
                    cur_idOrigem,
                    crr_idOrigem,
                    crp_idOrigem,
                    (byte)crd_tipo,
                    tur_id,
                    cur_idDestino,
                    crr_idDestino
                );
        }

        /// <summary>
        /// Verifica se já existe a disciplina cadastrada para o curriculo/curso/periodo
        /// e excluido logicamente
        /// filtrados por cur_id, crr_id, crp_id, dis_id
        /// </summary>
        /// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
        /// <param name="crr_id">Id da tabela ACA_Curriculo do bd</param>
        /// <param name="crp_id">Id da tabela ACA_CurriculoPeriodo do bd</param>
        /// <param name="dis_id">Id da tabela ACA_Disciplina do bd</param>
        /// <returns>true ou false</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaDisciplinaExistente
        (
            int cur_id
            , int crr_id
            , int crp_id
            , int dis_id
        )
        {
            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO();
            return dao.SelectBy_Chaves_excluido(cur_id, crr_id, crp_id, dis_id);
        }

        /// <summary>
        /// Verifica se existe disciplina principal e disciplinas obrigatórias ao mesmo tempo
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período do currículo</param>
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaDisciplinaPrincipalObrigatoria
        (
            int cur_id
            , int crr_id
            , int crp_id
            , TalkDBTransaction banco
        )
        {
            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO { _Banco = banco };
            return dao.SelectBy_VerificaDisciplinaPrincipalObrigatoria(cur_id, crr_id, crp_id);
        }

        /// <summary>
        /// Verifica se existe mais de uma disciplina principal no mesmo periodo
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período do currículo</param>
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaDisciplinaPrincipal
        (
            int cur_id
            , int crr_id
            , int crp_id
            , TalkDBTransaction banco
        )
        {
            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO { _Banco = banco };
            return dao.SelectBy_VerificaDisciplinaPrincipal(cur_id, crr_id, crp_id);
        }

        /// <summary>
        /// Verifica a soma das carga horária semanal das disciplinas obrigatorias e eletivas       
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período do currículo</param>
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static int VerificaCargaHorariaSemanal
        (
            int cur_id
            , int crr_id
            , int crp_id
            , TalkDBTransaction banco
        )
        {
            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO { _Banco = banco };
            return dao.SelectBy_VerificaCargaHorariaSemanal(cur_id, crr_id, crp_id);
        }

        /// <summary>
        /// Verifica se o curriculo disciplina está sendo utilizado        
        /// </summary>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do curriculo do curso</param>
        /// <param name="crp_id">Id do periodo do curriculo</param>        
        /// <param name="dis_id">Id da disciplina</param>
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaCurriculoDisciplina
        (
            int cur_id
            , int crr_id
            , int crp_id
            , int dis_id
            , TalkDBTransaction banco
        )
        {
            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO { _Banco = banco };
            return dao.SelectBy_VerificaCurriculoDisciplina(cur_id, crr_id, crp_id, dis_id);
        }

        /// <summary>
        /// Valida as disciplinas do curriculo de acordo com os dados do currículo do período      
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período do currículo</param>
        /// <param name="crp_controleTempo">Tipo de controle de tempo do período do currículo</param>
        /// <param name="crp_qtdeTemposSemana">Quantidade de tempos de aula na semana do período do currículo</param>
        /// <param name="crp_descricao">Descrição do período do currículo</param>
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        public static void ValidarCurriculoDisciplina
        (
            int cur_id
            , int crr_id
            , int crp_id
            , byte crp_controleTempo
            , int crp_qtdeTemposSemana
            , string crp_descricao
            , TalkDBTransaction banco
            , Guid ent_id
        )
        {
            // Verifica se existe mais de uma disciplina principal no mesmo periodo
            if (VerificaDisciplinaPrincipal(cur_id, crr_id, crp_id, banco))
                throw new ACA_CurriculoDisciplina_ValidationException("Só deve existir um(a) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " do tipo '" +
                                CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " principal' por " + GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id).ToLower() + ".");

            // Verifica se existe disciplina principal e disciplinas obrigatórias ao mesmo tempo                
            if (VerificaDisciplinaPrincipalObrigatoria(cur_id, crr_id, crp_id, banco))
                throw new ACA_CurriculoDisciplina_ValidationException("Não podem existir um(a) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                          " do tipo '" + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " principal' e " +
                          CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL") + " do tipo 'Obrigatório(a)' no(a) mesmo(a) " +
                          GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id).ToLower() + ".");

            // Verifica se a carga horária semanal das disciplinas obrigatórias e eletivas
            // não é maior que a carga horária total semanal informada no período
            // quando o controle é feito por tempo de aulas
            if (crp_controleTempo == Convert.ToByte(ACA_CurriculoPeriodoControleTempo.TemposAula))
            {
                if (VerificaCargaHorariaSemanal(cur_id, crr_id, crp_id, banco) > crp_qtdeTemposSemana)
                    throw new ACA_CurriculoDisciplina_ValidationException("A soma da carga horária semanal dos(as) " +
                        CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL") + " não pode ser maior que a quantidade de tempos de aula de uma semana informado no(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id).ToLower() + " " + crp_descricao + "." +
                        "<BR/> Obs: Para a soma da carga horária semanal dos(as) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL") +
                        " são consideradas todas os(as) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL") +
                        " do tipo 'Obrigatória' e um(a) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                        " de cada grupo de " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL") +
                        " do tipo 'Eletiva' (com maior carga horária semanal) por " +
                        GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id).ToLower() + ".");
            }
        }

        /// <summary>
        /// Inclui uma nova disciplina para o curriculo/curso
        /// </summary>
        /// <param name="entity">Entidade ACA_CurriculoDisciplina</param>        
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_CurriculoDisciplina entity
            , TalkDBTransaction banco
        )
        {
            if (entity.Validate())
            {
                ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO { _Banco = banco };
                return dao.Salvar(entity);
            }

            throw new ACA_CurriculoDisciplina_ValidationException(entity.PropertiesErrorList[0].Message);
        }

        /// <summary>
        /// Deleta logicamente o período da disciplina eletiva 
        /// </summary>
        /// <param name="entity">Entidade ACA_CurriculoDisciplina</param>
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool DeletarPorEletivasAlunos
        (
            ACA_CurriculoDisciplina entity
            , TalkDBTransaction banco
            , Guid ent_id
        )
        {
            if (VerificaCurriculoDisciplina(entity.cur_id, entity.crr_id, entity.crp_id, entity.dis_id, banco))
            {
                ACA_CurriculoPeriodo crp = new ACA_CurriculoPeriodo { cur_id = entity.cur_id, crr_id = entity.crr_id, crp_id = entity.crp_id };
                ACA_CurriculoPeriodoBO.GetEntity(crp, banco);

                ACA_Disciplina dis = new ACA_Disciplina { dis_id = entity.dis_id };
                ACA_DisciplinaBO.GetEntity(dis, banco);

                throw new ValidationException("Não é possível excluir o(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id).ToLower() + " " + crp.crp_descricao + " do(a) " +
                          CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " eletiva " + dis.dis_nome + ", pois possui outros registros ligados a ele(a).");
            }

            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO { _Banco = banco };
            return dao.Update_Situacao_By_EletivasAlunos(entity.cur_id, entity.crr_id, entity.crp_id, entity.dis_id);
        }

        /// <summary>
        /// Deleta logicamente as disciplinas do período por tipo de disciplina
        /// </summary>
        /// <param name="entity">Entidade ACA_Curso</param>              
        /// <param name="banco">Conexão aberta com o banco de dados</param>  
        /// <param name="tds_id">ID do tipo de disciplina</param>
        /// <param name="tds_nome">Nome do tipo de disciplina</param>
        /// <param name="dis_nome">Nome da disciplina</param>
        /// <returns>True = deletado/alterado | False = não deletado/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool DeletarPorTipoDisciplina
        (
            ACA_CurriculoDisciplina entity
            , int tds_id
            , string tds_nome
            , string dis_nome
            , TalkDBTransaction banco
        )
        {
            ACA_CurriculoDisciplinaDAO dao = new ACA_CurriculoDisciplinaDAO { _Banco = banco };
            return dao.Update_SituacaoBy_tds_id(entity.cur_id, entity.crr_id, tds_id, tds_nome, dis_nome);
        }

        /// <summary>
        /// Classe de excessão referente à entidade ACA_CurriculoDisciplina.
        /// Utilizada nas telas de cadastro, para identificar se houve erro de validação
        /// na entidade do ACA_CurriculoDisciplina.
        /// </summary>
        public class ACA_CurriculoDisciplina_ValidationException : ValidationException
        {
            public ACA_CurriculoDisciplina_ValidationException(string message) : base(message) { }
        }
    }
}