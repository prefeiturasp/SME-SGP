using System;
using System.Collections.Generic;
using System.Linq;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.Data;
using System.ComponentModel;
using MSTech.Data.Common;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.CustomResourceProviders;
using System.Threading.Tasks;
using System.Web;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region Estruturas

    /// <summary>
    /// Estrutura para controle de permissão do docente.
    /// </summary>
    [Serializable]
    public struct sTurmaAulaAluno
    {
        public int tau_id { get; set; }
		public long tud_id { get; set; }
		public long alu_id { get; set; }
		public int mtu_id { get; set; }
		public int mtd_id { get; set; }
		public int taa_frequencia { get; set; }
		public int tau_sequencia { get; set; }
		public DateTime tau_data { get; set; }
		public int tau_numeroAulas { get; set; }
		public bool tau_efetivado { get; set; }
		public string falta_justificada { get; set; }
		public byte tdt_posicao { get; set; }
		public string taa_frequenciaBitMap { get; set; }
		public bool permissaoAlteracao { get; set; }
		public bool AlunoComCompensacao { get; set; }
		public Guid usu_id { get; set; }
		public bool tau_reposicao { get; set; }
        public int dispensadisciplina { get; set; }
        public byte mtd_situacao { get; set; }
        public DateTime tau_dataAlteracao { get; set; }
        public string nomeUsuAlteracao { get; set; }
        public int tud_tipo { get; set; }
        public long tud_idExperiencia { get; set; }
        public int tau_idExperiencia { get; set; }        
        public bool falta_abonada { get; set; }
    }

    #endregion

    public class CLS_TurmaAulaAlunoBO : BusinessBase<CLS_TurmaAulaAlunoDAO, CLS_TurmaAulaAluno>
    {
        /// <summary>
        /// Retornar a frequencia de territórios do saber
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tau_id"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static List<sTurmaAulaAluno> SelecionaFrequenciaAulaTurmaDisciplinaTerritorio(long tud_id, int tau_id, TalkDBTransaction banco = null)
        {
            CLS_TurmaAulaAlunoDAO dao = banco == null ? new CLS_TurmaAulaAlunoDAO() : new CLS_TurmaAulaAlunoDAO { _Banco = banco };
            if (banco == null)
            {
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            }

            try
            {
                return dao.SelecionaFrequenciaAulaTurmaDisciplinaTerritorio(tud_id, tau_id)
                          .Select()
                          .Select(p => (sTurmaAulaAluno)GestaoEscolarUtilBO.DataRowToEntity(p, new sTurmaAulaAluno()))
                          .ToList();
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
        /// Retorna as aulas que os alunos tiveram dentro do bimestre em matrículas diferentes das enviadas no filtro.
        /// </summary>
        /// <param name="tbAlunosPeriodos"></param>
        /// <returns></returns>
        public static List<sQuantidadeAulaFaltaAdicional> ConsultaPor_Por_DiferentesMatriculas_Periodo (List<sAlunosDisciplinasPeriodos> listaAlunosFiltro, TalkDBTransaction banco = null)
        {
            CLS_TurmaAulaAlunoDAO dao = banco == null ? new CLS_TurmaAulaAlunoDAO() : new CLS_TurmaAulaAlunoDAO { _Banco = banco };
            if (banco == null)
            {
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            }

            try
            {
                DataTable dt = MTR_MatriculaTurmaDisciplina.TipoTabela_MatriculaTurmaDisciplinaPeriodo();

                listaAlunosFiltro.ForEach(
                p =>
                {
                    DataRow dr = dt.NewRow();
                    dr["tud_id"] = p.tud_id;
                    dr["alu_id"] = p.alu_id;
                    dr["mtu_id"] = p.mtu_id;
                    dr["mtd_id"] = p.mtd_id;
                    dr["tpc_id"] = p.tpc_id;
                    dr["tur_id"] = p.tur_id;
                    dr["cap_dataInicio"] = p.cap_dataInicio;
                    dr["cap_dataFim"] = p.cap_dataFim;
                    dt.Rows.Add(dr);
                }
                );

                DataTable dtRet = dao.SelecionaPor_Por_DiferentesMatriculas_Periodo(dt);
                List<sQuantidadeAulaFaltaAdicional> ret = new List<sQuantidadeAulaFaltaAdicional>();
                foreach (DataRow dr in dtRet.Rows)
                {
                    ret.Add((sQuantidadeAulaFaltaAdicional)GestaoEscolarUtilBO.DataRowToEntity(dr, new sQuantidadeAulaFaltaAdicional()));
                }

                return ret;
            }
            catch (Exception ex)
            {
                dao._Banco.Close(ex);
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
        /// Retorna os dados da CLS_TurmaAulaAluno que sejam pela 
        /// "chave" da matrícula do aluno na disciplina.
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina - obrigatório</param>
        /// <param name="alu_id">Id do aluno - obrigatório</param>
        /// <param name="mtu_id">Id da matrícula do aluno na turma - obrigatório</param>
        /// <param name="mtd_id">Id da matrícula do aluno na disciplina - obrigatório</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <returns>Lista de  CLS_TurmaAulaAluno</returns>
        public static List<CLS_TurmaAulaAluno> GetSelectBy_Disciplina_Aluno
        (
            Int64 tud_id
            , Int64 alu_id
            , Int32 mtu_id
            , Int32 mtd_id
            , TalkDBTransaction banco
        )
        {
            CLS_TurmaAulaAlunoDAO dao = new CLS_TurmaAulaAlunoDAO
            {
                _Banco = banco
            };

            return dao.SelectBy_Disciplina_Aluno(tud_id, alu_id, mtu_id, mtd_id);
        }

        /// <summary>
        /// Retorna o lançamento de frequência dos alunos que não foram excluídos logicamente(Sem dados do aluno)
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tau_id">ID da aula da disciplina da turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sTurmaAulaAluno> SelecionaFrequenciaAulaTurmaDisciplina
        (
            long tud_id
            , int tau_id
            , string tur_ids = null
        )
        {
            CLS_TurmaAulaAlunoDAO dao = new CLS_TurmaAulaAlunoDAO();

            DataTable dtTurmas = TUR_Turma.TipoTabela_Turma();

            if (!string.IsNullOrEmpty(tur_ids))
            {
                tur_ids.Split(';').ToList().ForEach
                    (
                        p =>
                        {
                            DataRow dr = dtTurmas.NewRow();
                            dr["tur_id"] = p.ToString();
                            dtTurmas.Rows.Add(dr);
                        }
                    );
            }

            return dao.SelectBy_FrequenciaTurmaDisciplina(tud_id, tau_id, dtTurmas)
                      .Select()
                      .Select(p => (sTurmaAulaAluno)GestaoEscolarUtilBO.DataRowToEntity(p, new sTurmaAulaAluno()))
                      .ToList();
        }

        /// <summary>
        /// Retorna o lançamento de frequência dos alunos que não foram excluídos logicamente
        /// </summary>                
        /// <param name="tud_id">ID da disciplina da turma</param>        
        /// <param name="tau_id">ID da aula da disciplina da turma</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="ordenacao">Tipo de ordenação dos alunos 0-Número chamada, 1-Nome do aluno</param> 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaFrequenciaPorAulaTurmaDisciplina
        (
            long tud_id
            , int tau_id
            , Guid ent_id
            , byte ordenacao
        )
        {
            CLS_TurmaAulaAlunoDAO dao = new CLS_TurmaAulaAlunoDAO();
            return dao.SelectBy_TurmaDisciplina(tud_id, tau_id, ent_id, ordenacao);
        }

        /// <summary>
        /// Retorna os lançamentos de frequência dos alunos, filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>                
        /// <param name="tud_id">ID da disciplina da turma</param>        
        /// <param name="tau_id">ID da aula da disciplina da turma</param>
        /// <param name="tipoDocente">Tipo de docente</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="ordenacao">Tipo de ordenação dos alunos 0-Número chamada, 1-Nome do aluno</param> 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaFrequenciaPorAulaTurmaDisciplinaFiltroDeficiencia
        (
            long tud_id
            , int tau_id
            , EnumTipoDocente tipoDocente
            , Guid ent_id
            , byte ordenacao
        )
        {
            return new CLS_TurmaAulaAlunoDAO().SelectBy_TurmaDisciplinaFiltroDeficiencia(tud_id, tau_id, (byte)tipoDocente, ent_id, ordenacao);
        }

        /// <summary>
        /// Retorna datatable contendo todos os alunos que possuam anotações.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param> 
        /// <param name="tau_id">ID da aula da disciplina da turma</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaAnotacaoPorAulaTurmaDisciplina
        (
            long tud_id
            , int tau_id
            , Guid ent_id
            , string tur_ids = null
        )
        {
            CLS_TurmaAulaAlunoDAO dao = new CLS_TurmaAulaAlunoDAO();

            DataTable dtTurmas = TUR_Turma.TipoTabela_Turma();

            if (!string.IsNullOrEmpty(tur_ids))
            {
                tur_ids.Split(';').ToList().ForEach
                    (
                        p =>
                        {
                            DataRow dr = dtTurmas.NewRow();
                            dr["tur_id"] = p.ToString();
                            dtTurmas.Rows.Add(dr);
                        }
                    );
            }

            return dao.SelectBy_tud_id_Return_anotacao(tud_id, tau_id, ent_id, dtTurmas);
        }

        /// <summary>
        /// Retorna Turma, Disciplina, Professor, data da aula e anotações do Aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param> 
        /// <param name="cal_ano">ID do calendário</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaAnotacaoPorAlunoCalendario
        (
            long alu_id
            , int cal_ano
            , Guid usu_id
            , bool usuario_superior
        )
        {
            CLS_TurmaAulaAlunoDAO dao = new CLS_TurmaAulaAlunoDAO();
            return dao.SelectAulasAluno(alu_id, cal_ano, usu_id, usuario_superior);
        }

        /// <summary>
        /// Retorna as aulas da turma disciplina no período.
        /// </summary>
        /// <param name="tud_id">id da turma</param>
        /// <param name="tpc_id">id do período</param>
        /// <param name="data_inicio">data inicio</param>
        /// <param name="data_final">data final</param>
        /// <returns>
        /// Quando a data inicio e data final for zero retorna os dados das 5 aulas
        /// caso contrário retorna todas as aulas dentro do intervalo das datas.
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sTurmaAulaAluno> GetSelectBy_TurmaDisciplina
        (
            long tud_id
            , int tpc_id
            , DateTime data_inicio
            , DateTime data_final
        )
        {
            return GetSelectBy_TurmaDisciplina(tud_id, tpc_id, Guid.Empty, data_inicio, data_final, 0, false);
        }

        /// <summary>
        /// Retorna as aulas da turma disciplina no período.
        /// </summary>
        /// <param name="tud_id">id da turma</param>
        /// <param name="tpc_id">id do período</param>
        /// <param name="usu_id">id do usuario que corresponde ao docente logado</param>
        /// <param name="data_inicio">data inicio</param>
        /// <param name="data_final">data final</param>
        /// <param name="tdt_posicao">Inidica a posição do docente a ser filtrada</param>
        /// <param name="usuario_superior">Indica se é um usuário superior</param>
        /// <returns>
        /// Quando a data inicio e data final for zero retorna os dados das 5 aulas
        /// caso contrário retorna todas as aulas dentro do intervalo das datas.
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sTurmaAulaAluno> GetSelectBy_TurmaDisciplina
        (
            long tud_id
            , int tpc_id
            , Guid usu_id
            , DateTime data_inicio
            , DateTime data_final
            , byte tdt_posicao
            , bool usuario_superior
            , long tud_idRelacionada = -1
            , string tur_ids = null
        )
        {
            CLS_TurmaAulaAlunoDAO dao = new CLS_TurmaAulaAlunoDAO();

            DataTable dtTurmas = TUR_Turma.TipoTabela_Turma();

            if (!string.IsNullOrEmpty(tur_ids))
            {
                tur_ids.Split(';').ToList().ForEach
                    (
                        p =>
                        {
                            DataRow dr = dtTurmas.NewRow();
                            dr["tur_id"] = p.ToString();
                            dtTurmas.Rows.Add(dr);
                        }
                    );
            }

            return dao.SelectBy_TurmaDisciplina(tud_id, tpc_id, usu_id, data_inicio, data_final, tdt_posicao, usuario_superior, tud_idRelacionada, dtTurmas).Rows.Cast<DataRow>()
                   .Select(dr =>
                    new sTurmaAulaAluno 
                    {
                        tau_id = Convert.ToInt32(dr["tau_id"] != DBNull.Value ? dr["tau_id"] : 0)
                        ,
                        tud_id = Convert.ToInt64(dr["tud_id"] != DBNull.Value ? dr["tud_id"] : 0)
                        ,
                        alu_id = Convert.ToInt64(dr["alu_id"] != DBNull.Value ? dr["alu_id"] : 0)
                        ,
                        mtu_id = Convert.ToInt32(dr["mtu_id"] != DBNull.Value ? dr["mtu_id"] : 0)
                        ,
                        mtd_id = Convert.ToInt32(dr["mtd_id"] != DBNull.Value ? dr["mtd_id"] : 0)
                        ,
                        taa_frequencia = Convert.ToInt32(dr["taa_frequencia"] != DBNull.Value ? dr["taa_frequencia"] : 0)
                        ,
                        tau_sequencia = Convert.ToInt32(dr["tau_sequencia"] != DBNull.Value ? dr["tau_sequencia"] : 0)
                        ,
                        tau_data = Convert.ToDateTime(dr["tau_data"] != DBNull.Value ? dr["tau_data"] : new DateTime())
                        ,
                        tau_numeroAulas = Convert.ToInt32(dr["tau_numeroAulas"] != DBNull.Value ? dr["tau_numeroAulas"] : 0)
                        ,
                        tau_efetivado = Convert.ToBoolean(dr["tau_efetivado"] != DBNull.Value ? dr["tau_efetivado"] : false)
                        ,
                        falta_justificada = (dr["falta_justificada"] != DBNull.Value ? dr["falta_justificada"] : "").ToString()
                        ,
                        tdt_posicao = Convert.ToByte(dr["tdt_posicao"] != DBNull.Value ? dr["tdt_posicao"] : 0)
                        ,
                        taa_frequenciaBitMap = (dr["taa_frequenciaBitMap"] != DBNull.Value ? dr["taa_frequenciaBitMap"] : "").ToString()
                        ,
                        permissaoAlteracao = Convert.ToBoolean(dr["permissaoAlteracao"] != DBNull.Value ? dr["permissaoAlteracao"] : false)
                        ,
                        AlunoComCompensacao = Convert.ToBoolean(dr["AlunoComCompensacao"] != DBNull.Value ? dr["AlunoComCompensacao"] : false)
                        ,
                        usu_id = new Guid((dr["usu_id"] != DBNull.Value ? dr["usu_id"] : new Guid()).ToString())
                        ,
                        tau_reposicao = Convert.ToBoolean(dr["tau_reposicao"] != DBNull.Value ? dr["tau_reposicao"] : false)
                        ,
                        falta_abonada = Convert.ToBoolean(dr["falta_abonada"] != DBNull.Value ? dr["falta_abonada"] : false)
                    }).ToList();
        }

        /// <summary>
        /// Retorna as aulas da turma disciplina no período.
        /// </summary>
        /// <param name="tud_id">id da turma</param>
        /// <param name="tpc_id">id do período</param>
        /// <param name="usu_id">id do usuario que corresponde ao docente logado</param>
        /// <param name="data_inicio">data inicio</param>
        /// <param name="data_final">data final</param>
        /// <param name="tdt_posicao">Inidica a posição do docente a ser filtrada</param>
        /// <param name="usuario_superior">Indica se é um usuário superior</param>
        /// <returns>
        /// Quando a data inicio e data final for zero retorna os dados das 5 aulas
        /// caso contrário retorna todas as aulas dentro do intervalo das datas.
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sTurmaAulaAluno> GetSelectBy_TurmaDisciplinaTerritorio
        (
            long tud_id
            , int tpc_id
            , Guid usu_id
            , DateTime data_inicio
            , DateTime data_final
            , byte tdt_posicao
            , bool usuario_superior
            , long tud_idRelacionada = -1
            , string tur_ids = null
        )
        {
            CLS_TurmaAulaAlunoDAO dao = new CLS_TurmaAulaAlunoDAO();

            DataTable dtTurmas = TUR_Turma.TipoTabela_Turma();

            if (!string.IsNullOrEmpty(tur_ids))
            {
                tur_ids.Split(';').ToList().ForEach
                    (
                        p =>
                        {
                            DataRow dr = dtTurmas.NewRow();
                            dr["tur_id"] = p.ToString();
                            dtTurmas.Rows.Add(dr);
                        }
                    );
            }

            return dao.SelectBy_TurmaDisciplinaTerritorio(tud_id, tpc_id, usu_id, data_inicio, data_final, tdt_posicao, usuario_superior, tud_idRelacionada, dtTurmas).Rows.Cast<DataRow>()
                   .Select(dr =>
                    new sTurmaAulaAluno
                    {
                        tau_id = Convert.ToInt32(dr["tau_id"] != DBNull.Value ? dr["tau_id"] : 0)
                        ,
                        tud_id = Convert.ToInt64(dr["tud_id"] != DBNull.Value ? dr["tud_id"] : 0)
                        ,
                        alu_id = Convert.ToInt64(dr["alu_id"] != DBNull.Value ? dr["alu_id"] : 0)
                        ,
                        mtu_id = Convert.ToInt32(dr["mtu_id"] != DBNull.Value ? dr["mtu_id"] : 0)
                        ,
                        mtd_id = Convert.ToInt32(dr["mtd_id"] != DBNull.Value ? dr["mtd_id"] : 0)
                        ,
                        taa_frequencia = Convert.ToInt32(dr["taa_frequencia"] != DBNull.Value ? dr["taa_frequencia"] : 0)
                        ,
                        tau_sequencia = Convert.ToInt32(dr["tau_sequencia"] != DBNull.Value ? dr["tau_sequencia"] : 0)
                        ,
                        tau_data = Convert.ToDateTime(dr["tau_data"] != DBNull.Value ? dr["tau_data"] : new DateTime())
                        ,
                        tau_numeroAulas = Convert.ToInt32(dr["tau_numeroAulas"] != DBNull.Value ? dr["tau_numeroAulas"] : 0)
                        ,
                        tau_efetivado = Convert.ToBoolean(dr["tau_efetivado"] != DBNull.Value ? dr["tau_efetivado"] : false)
                        ,
                        falta_justificada = (dr["falta_justificada"] != DBNull.Value ? dr["falta_justificada"] : "").ToString()
                        ,
                        tdt_posicao = Convert.ToByte(dr["tdt_posicao"] != DBNull.Value ? dr["tdt_posicao"] : 0)
                        ,
                        taa_frequenciaBitMap = (dr["taa_frequenciaBitMap"] != DBNull.Value ? dr["taa_frequenciaBitMap"] : "").ToString()
                        ,
                        permissaoAlteracao = Convert.ToBoolean(dr["permissaoAlteracao"] != DBNull.Value ? dr["permissaoAlteracao"] : false)
                        ,
                        AlunoComCompensacao = Convert.ToBoolean(dr["AlunoComCompensacao"] != DBNull.Value ? dr["AlunoComCompensacao"] : false)
                        ,
                        usu_id = new Guid((dr["usu_id"] != DBNull.Value ? dr["usu_id"] : new Guid()).ToString())
                        ,
                        tau_reposicao = Convert.ToBoolean(dr["tau_reposicao"] != DBNull.Value ? dr["tau_reposicao"] : false)
                        ,
                        tud_tipo = Convert.ToInt16(dr["tud_tipo"] != DBNull.Value ? dr["tud_tipo"] : 0)
                        ,
                        tud_idExperiencia = Convert.ToInt64(dr["tud_idExperiencia"] != DBNull.Value ? dr["tud_idExperiencia"] : null)
                        ,
                        tau_idExperiencia = Convert.ToInt32(dr["tau_idExperiencia"] != DBNull.Value ? dr["tau_idExperiencia"] : 0)
                        ,
                        falta_abonada = Convert.ToBoolean(dr["falta_abonada"] != DBNull.Value ? dr["falta_abonada"] : false)
                    }).ToList();
        }

        /// <summary>
        /// Salva as entidades turmaAula e TurmaAulaAluno nas listas.
        /// </summary>
        /// <param name="listTurmaAulaAluno">Lista de entidades CLS_TurmaAulaAluno</param>
        /// <param name="listTurmaAula">LIsta de entidades CLS_TurmaAula</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tud_id">ID da disciplina que está sendo salva as frequências</param>
        /// <param name="tdt_posicao">Posição do docente logado no sistema</param>
        /// <param name="entityTurma">Turma.</param>
        /// <param name="entityFormatoAvaliacao">Formato de avaliação.</param>
        /// <param name="entityCurriculoPeriodo">CurriculoPeriodo.</param>
        /// <returns></returns>
        public static bool Save
        (
            List<CLS_TurmaAulaAluno> listTurmaAulaAluno
            , List<CLS_TurmaAula> listTurmaAula
            , long tur_id
            , long tud_id
            , byte tdt_posicao
            , TUR_Turma entityTurma = null
            , ACA_FormatoAvaliacao entityFormatoAvaliacao = null
            , ACA_CurriculoPeriodo entityCurriculoPeriodo = null
            , Guid usu_id = new Guid()
            , byte origemLogAula = 0
            , byte tipoLogAula = 0
            , Guid ent_id = new Guid()
        )
        {
            TalkDBTransaction banco = new CLS_TurmaAulaAlunoDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                return Save(listTurmaAulaAluno, listTurmaAula, tur_id, tud_id, tdt_posicao, entityTurma, entityFormatoAvaliacao, entityCurriculoPeriodo, banco, usu_id, origemLogAula, tipoLogAula, ent_id);
            }
            catch (Exception err)
            {
                banco.Close(err);
                throw;
            }
            finally
            {
                banco.Close();
            }
        }

        /// <summary>
        /// Salva as entidades turmaAula e TurmaAulaAluno nas listas - com transação.
        /// </summary>
        /// <param name="listTurmaAulaAluno">Lista de entidades CLS_TurmaAulaAluno</param>
        /// <param name="listTurmaAula">LIsta de entidades CLS_TurmaAula</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tud_id">ID da disciplina que está sendo salva as frequências</param>
        /// <param name="tdt_posicao">Posição do docente logado no sistema</param>
        /// <param name="entityTurma">Turma.</param>
        /// <param name="entityFormatoAvaliacao">Formato de avaliação.</param>
        /// <param name="entityCurriculoPeriodo">CurriculoPeriodo.</param>
        /// <param name="banco">Transação com banco de dados aberta</param>
        /// <returns></returns>
        internal static bool Save
        (
            List<CLS_TurmaAulaAluno> listTurmaAulaAluno
            , List<CLS_TurmaAula> listTurmaAula
            , long tur_id
            , long tud_id
            , byte tdt_posicao
            , TUR_Turma entityTurma
            , ACA_FormatoAvaliacao entityFormatoAvaliacao
            , ACA_CurriculoPeriodo entityCurriculoPeriodo
            , TalkDBTransaction banco
            , Guid usu_id = new Guid()
            , byte origemLogAula = 0
            , byte tipoLogAula = 0
            , Guid ent_id = new Guid()
        )
        {
            string tau_ids = string.Join(",",
                                         (from CLS_TurmaAula item in listTurmaAula select item.tau_id.ToString()).
                                             ToArray());

            // Recupera a lista de entidades CLS_TurmaAulaAluno para verificar se ela já existe.
            List<CLS_TurmaAulaAluno> listaTurmaAulaAluno = new CLS_TurmaAulaAlunoDAO { _Banco = banco }
                .SelectBy_Disciplina_Aulas(tud_id, tau_ids);

            DataTable dtTurmaAulaAluno = CLS_TurmaAulaAluno.TipoTabela_TurmaAulaAluno();
            List<LOG_TurmaAula_Alteracao> listLogAula = new List<LOG_TurmaAula_Alteracao>();

            object lockObject = new object();

            Parallel.ForEach
            (
                listTurmaAulaAluno,
                entityTurmaAulaAluno =>
                {
                    // Busca se a entidade já existe na lista.
                    CLS_TurmaAulaAluno entAux =
                        listaTurmaAulaAluno.Find(p =>
                                                 p.tud_id == entityTurmaAulaAluno.tud_id
                                                 && p.tau_id == entityTurmaAulaAluno.tau_id
                                                 && p.alu_id == entityTurmaAulaAluno.alu_id
                                                 && p.mtu_id == entityTurmaAulaAluno.mtu_id
                                                 && p.mtd_id == entityTurmaAulaAluno.mtd_id
                            );

                    if (entAux != null)
                    {
                        entityTurmaAulaAluno.IsNew = entAux.IsNew;
                        entityTurmaAulaAluno.taa_anotacao = entAux.taa_anotacao;

                        entityTurmaAulaAluno.usu_idDocenteAlteracao = entAux.usu_idDocenteAlteracao;
                    }

                    Validate(entityTurmaAulaAluno, listTurmaAula);

                    if (entityTurmaAulaAluno.Validate())
                    {
                        lock (lockObject)
                        {
                            DataRow dr = dtTurmaAulaAluno.NewRow();
                            dtTurmaAulaAluno.Rows.Add(TurmaAulaAlunoToDataRow(entityTurmaAulaAluno, dr));
                        }
                    }
                    else
                    {
                        throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entityTurmaAulaAluno));
                    }
                }
            );

            // Salva os dados de todos os alunos na tabela CLS_TurmaAulaAluno
            SalvaFrequenciaAlunos(dtTurmaAulaAluno, banco);

            // Verifica se a entidade recebida por parâmetro foi alimentada, se não foi, dá o GetEntity.
            TUR_Turma turma = entityTurma ?? TUR_TurmaBO.GetEntity(new TUR_Turma { tur_id = tur_id }, banco);
            ACA_FormatoAvaliacao formatoAvaliacao = entityFormatoAvaliacao ?? ACA_FormatoAvaliacaoBO.GetEntity(new ACA_FormatoAvaliacao { fav_id = turma.fav_id }, banco);
            ACA_CurriculoPeriodo entityCrp = entityCurriculoPeriodo ?? ACA_CurriculoPeriodoBO.SelecionaPorTurmaTipoNormal(turma.tur_id, GestaoEscolarUtilBO.MinutosCacheLongo);

            List<TUR_TurmaDisciplina> listaDisciplinas = TUR_TurmaDisciplinaBO.GetSelectBy_Turma(tur_id, banco, GestaoEscolarUtilBO.MinutosCacheLongo);

            TUR_TurmaDisciplina entDisciplinarincipal =
                listaDisciplinas.Find(p => p.tud_tipo == (byte)TurmaDisciplinaTipo.DisciplinaPrincipal);

            // Se não for para lançar na disciplina global, e a turma possuir uma disc. principal,
            // só poderá salvar na disciplina principal.
            bool validarDiscPrincipal =
                (!(turma.tur_docenteEspecialista && formatoAvaliacao.fav_planejamentoAulasNotasConjunto)) &&
                (entDisciplinarincipal != null) && (formatoAvaliacao.fav_tipoApuracaoFrequencia != (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.Dia &&
                                                    entityCrp.crp_controleTempo != (byte)ACA_CurriculoPeriodoControleTempo.Horas);

            DateTime dataLogAula = DateTime.Now;
            foreach (CLS_TurmaAula entityTurmaAula in listTurmaAula)
            {
                // Se for pra validar a disc. principal, só pode lançar frequência nela.
                if (validarDiscPrincipal && (entDisciplinarincipal.tud_id != entityTurmaAula.tud_id))
                {
                    throw new ValidationException("A frequência dessa turma só pode ser lançada para o(a) " + CustomResource.GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + entDisciplinarincipal.tud_nome + ".");
                }

                if (origemLogAula > 0)
                {
                    LOG_TurmaAula_Alteracao entLogAula = new LOG_TurmaAula_Alteracao
                    {
                        tud_id = entityTurmaAula.tud_id,
                        tau_id = entityTurmaAula.tau_id,
                        usu_id = usu_id,
                        lta_origem = origemLogAula,
                        lta_tipo = tipoLogAula,
                        lta_data = dataLogAula
                    };

                    listLogAula.Add(entLogAula);
                }
            }

            //Salva os logs de alteração de aula
            LOG_TurmaAula_AlteracaoBO.SalvarEmLote(listLogAula, banco);

            // Atualiza o campo efetivado da aula.
            CLS_TurmaAulaBO.AtualizarEfetivado(listTurmaAula, banco);

            // Caso o fechamento seja automático, grava na fila de processamento.
            if (formatoAvaliacao.fav_fechamentoAutomatico && listTurmaAula.Count > 0 && listTurmaAula[0].tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, ent_id))
            {
                CLS_AlunoFechamentoPendenciaBO.SalvarFilaFrequencia(tud_id, listTurmaAula[0].tpc_id, banco);
            }

            if (listTurmaAula.Any() && dtTurmaAulaAluno.Rows.Count > 0 && HttpContext.Current != null)
            {
                // Limpa o cache do fechamento
                try
                {
                    string chave = string.Empty;
                    int tpc_id = listTurmaAula[0].tpc_id;
                    List<ACA_Avaliacao> avaliacao = ACA_AvaliacaoBO.GetSelectBy_FormatoAvaliacaoPeriodo(turma.fav_id, tpc_id);

                    if (avaliacao.Any())
                    {
                        int ava_id = avaliacao.First().ava_id;
                        if (tud_id > 0)
                        {
                            chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodo(tud_id, turma.fav_id, ava_id, string.Empty);
                            CacheManager.Factory.RemoveByPattern(chave);

                            chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia(tud_id, turma.fav_id, ava_id, string.Empty);
                            CacheManager.Factory.RemoveByPattern(chave);
                        }
                        else
                        {
                            chave = MTR_MatriculaTurmaBO.RetornaChaveCache_GetSelectBy_Turma_Periodo(tur_id, turma.fav_id, ava_id);
                            HttpContext.Current.Cache.Remove(chave);
                        }
                    }
                }
                catch
                { }
            }

            return true;
        }

        /// <summary>
        /// Salva as entidades turmaAula e TurmaAulaAluno nas listas - com transação.
        /// </summary>
        /// <param name="listTurmaAulaAluno">Lista de entidades CLS_TurmaAulaAluno</param>
        /// <param name="listTurmaAula">LIsta de entidades CLS_TurmaAula</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tud_id">ID da disciplina que está sendo salva as frequências</param>
        /// <param name="tdt_posicao">Posição do docente logado no sistema</param>
        /// <param name="banco">Transação com banco de dados aberta</param>
        /// <returns></returns>
        internal static bool Save
        (
            List<CLS_TurmaAulaAluno> listTurmaAulaAluno
            , List<CLS_TurmaAula> listTurmaAula
            , List<sDadosAulaProtocolo> ltDadosAulasValidacao
            , List<CLS_TurmaAula> ltAulasBanco
            , TalkDBTransaction banco
        )
        {

            // Recupera a lista de entidades CLS_TurmaAulaAluno para verificar se ela já existe.
            List<CLS_TurmaAulaAluno> listaTurmaAulaAluno = new List<CLS_TurmaAulaAluno>();

            ltDadosAulasValidacao.ForEach(p => listaTurmaAulaAluno.AddRange(p.ltTurmaAulaAluno));

            DataTable dtTurmaAulaAluno = CLS_TurmaAulaAluno.TipoTabela_TurmaAulaAluno();

            foreach (CLS_TurmaAulaAluno entityTurmaAulaAluno in listTurmaAulaAluno)
            {
                // Busca se a entidade já existe na lista.
                CLS_TurmaAulaAluno entAux =
                    listaTurmaAulaAluno.Find(p =>
                                             p.tud_id == entityTurmaAulaAluno.tud_id
                                             && p.tau_id == entityTurmaAulaAluno.tau_id
                                             && p.alu_id == entityTurmaAulaAluno.alu_id
                                             && p.mtu_id == entityTurmaAulaAluno.mtu_id
                                             && p.mtd_id == entityTurmaAulaAluno.mtd_id
                        );

                if (entAux != null)
                {
                    entityTurmaAulaAluno.IsNew = entAux.IsNew;
                    entityTurmaAulaAluno.taa_anotacao = entAux.taa_anotacao;
                }

                Validate(entityTurmaAulaAluno, listTurmaAula);

                if (entityTurmaAulaAluno.Validate())
                {
                    dtTurmaAulaAluno.Rows.Add(TurmaAulaAlunoToDataRow(entityTurmaAulaAluno, dtTurmaAulaAluno.NewRow()));
                }
                else
                {
                    throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entityTurmaAulaAluno));
                }
            }

            // Salva os dados de todos os alunos na tabela CLS_TurmaAulaAluno
            SalvaFrequenciaAlunos(dtTurmaAulaAluno, banco);

            CLS_TurmaAulaBO.SalvarAulas(listTurmaAula, ltDadosAulasValidacao, ltAulasBanco, banco);

            return true;
        }

        /// <summary>
        /// Retorna as frequências dos alunos matriculados na disciplina e períodos selecionados.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tpc_id">ID do tipo período calendário</param>
        /// <param name="dataInicio">Data de ínicio do período</param>
        /// <param name="dataFim">Data de fim do período</param>
        /// <returns>DataTable de frequências por disciplina dos alunos</returns>
        public static DataTable SelecionaFreqDisciplinaPorTurmaPeriodoData
        (
            long tur_id
            , int tpc_id
            , DateTime dataInicio
            , DateTime dataFim
        )
        {
            CLS_TurmaAulaAlunoDAO dao = new CLS_TurmaAulaAlunoDAO();
            return dao.SelectFreqDisciplinaByTurmaPeriodoData(tur_id, tpc_id, dataInicio, dataFim);
        }

        /// <summary>
        /// Retorna a frequência global dos alunos matriculados na turma e períodos selecionados.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tpc_id">ID do tipo período calendário</param>
        /// <param name="dataInicio">Data de ínicio do período</param>
        /// <param name="dataFim">Data de fim do período</param>
        /// <returns>DataTable de frequência global</returns>
        public static DataTable SelecionaFreqGlobalPorTurmaPeriodoData
        (
            long tur_id
            , int tpc_id
            , DateTime dataInicio
            , DateTime dataFim
        )
        {
            CLS_TurmaAulaAlunoDAO dao = new CLS_TurmaAulaAlunoDAO();
            return dao.SelectFreqGlobalByTurmaPeriodoData(tur_id, tpc_id, dataInicio, dataFim);
        }

        /// <summary>
        /// Retorna a frequência global dos alunos matriculados na turma e períodos selecionados.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <param name="tpc_id">ID do tipo período calendário</param>
        /// <param name="tau_id">ID da aula</param>
        /// <returns>DataTable de frequência global</returns>
        public static bool VerificaLancamentoFrequencia
        (
            long tud_id
            , int tpc_id
            , int tau_id
        )
        {
            CLS_TurmaAulaAlunoDAO dao = new CLS_TurmaAulaAlunoDAO();
            DataTable dt = dao.VerificaLancamentoFrequencia(tud_id, tpc_id, tau_id);
            return dt.Rows.Count > 0;
        }

        /// <summary>
        /// Salva os dados das frequências dos alunos.
        /// </summary>
        /// <param name="dtTurmaAulaAluno">DataTable de dados do listão de frequência.</param>
        /// <param name="banco">Transação.</param>
        /// <returns>True em caso de sucesso.</returns>
        public static bool SalvaFrequenciaAlunos(DataTable dtTurmaAulaAluno, TalkDBTransaction banco = null)
        {
            return banco == null ?
                   new CLS_TurmaAulaAlunoDAO().SalvaFrequenciaAlunos(dtTurmaAulaAluno) :
                   new CLS_TurmaAulaAlunoDAO { _Banco = banco }.SalvaFrequenciaAlunos(dtTurmaAulaAluno);
        }

        /// <summary>
        /// O método converte um registro da CLS_TurmaAulaAluno em um DataRow.
        /// </summary>
        /// <param name="turmaAulaAluno">Frequência do aluno.</param>
        /// <param name="dr">Layout do DataTable.</param>
        /// <returns>DataRow.</returns>
        public static DataRow TurmaAulaAlunoToDataRow(CLS_TurmaAulaAluno turmaAulaAluno, DataRow dr, DateTime taa_dataAlteracao = new DateTime())
        {
            if (turmaAulaAluno.idAula > 0)
                dr["idAula"] = turmaAulaAluno.idAula;
            else
                dr["idAula"] = DBNull.Value;

            dr["tud_id"] = turmaAulaAluno.tud_id;
            dr["tau_id"] = turmaAulaAluno.tau_id;
            dr["alu_id"] = turmaAulaAluno.alu_id;
            dr["mtu_id"] = turmaAulaAluno.mtu_id;
            dr["mtd_id"] = turmaAulaAluno.mtd_id;
            dr["taa_frequencia"] = turmaAulaAluno.taa_frequencia;

            if (!string.IsNullOrEmpty(turmaAulaAluno.taa_anotacao))
                dr["taa_anotacao"] = turmaAulaAluno.taa_anotacao;
            else
                dr["taa_anotacao"] = DBNull.Value;

            dr["taa_situacao"] = turmaAulaAluno.taa_situacao;

            if (!string.IsNullOrEmpty(turmaAulaAluno.taa_frequenciaBitMap))
                dr["taa_frequenciaBitMap"] = turmaAulaAluno.taa_frequenciaBitMap;
            else
                dr["taa_frequenciaBitMap"] = DBNull.Value;

            if (taa_dataAlteracao != new DateTime())
                dr["taa_dataAlteracao"] = taa_dataAlteracao;
            else
                dr["taa_dataAlteracao"] = DBNull.Value;

            if (turmaAulaAluno.usu_idDocenteAlteracao != Guid.Empty)
                dr["usu_idDocenteAlteracao"] = turmaAulaAluno.usu_idDocenteAlteracao;
            else
                dr["usu_idDocenteAlteracao"] = DBNull.Value;

            return dr;
        }

        /// <summary>
        /// Valida a quantidade de frequência na aula.
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaAulaAluno</param>
        /// <param name="listAulas">Lista de entidades CLS_TurmaAula</param>
        public static void Validate(CLS_TurmaAulaAluno entity, List<CLS_TurmaAula> listAulas)
        {
            CLS_TurmaAula aula = listAulas.Find(p => (p.tud_id == entity.tud_id && p.tau_id == entity.tau_id && p.tau_numeroAulas > 0));

            if (aula != null)
            {
                if (aula.tau_numeroAulas < entity.taa_frequencia)
                    throw new ValidationException("A frequência deve estar entre 0 e " + aula.tau_numeroAulas);
            }
        }

        /// <summary>
        /// Retorna os dados das anotações do aluno, tanto do docente como da equipe gestora.
        /// </summary>
        /// <param name="ano">Ano das anotações</param>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="mtu_id">Id da matrícula do aluno na turma</param>
        /// <returns></returns>
        public static DataTable SelecionaAnotacoesPorAluno(int ano, long alu_id, int mtu_id)
        {
            CLS_TurmaAulaAlunoDAO dao = new CLS_TurmaAulaAlunoDAO();
            return dao.SelectAnotacoesBy_Aluno(ano, alu_id, mtu_id);
        }
    }
}
