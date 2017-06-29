/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using System;
    using Caching;
    using System.Linq;
    using System.Data;
    using System.ComponentModel;
    using Validation.Exceptions;
    using Data.Common;

    public enum RelatorioPreenchimentoAlunoSituacao : byte
    {
        [Description("CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.RelatorioPreenchimentoAlunoSituacao.Rascunho")]
        Rascunho = 1
        ,
        Excluido = 3
        ,
        [Description("CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.RelatorioPreenchimentoAlunoSituacao.Finalizado")]
        Finalizado = 4
        ,
        [Description("CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.RelatorioPreenchimentoAlunoSituacao.Aprovado")]
        Aprovado = 5
    }

    [Serializable]
    public class Struct_PreenchimentoAluno
    {
        public long alu_id { get; set; }

        public int tds_id { get; set; }

        public int tds_idRelacionada { get; set; }

        public long tur_id { get; set; }

        public string tur_codigo { get; set; }

        public long tud_id { get; set; }

        public string tud_nome { get; set; }
    }

    public class PermissaoRelatorioPreenchimentoValidationException : ValidationException
    {
        public PermissaoRelatorioPreenchimentoValidationException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Description: CLS_RelatorioPreenchimentoAlunoTurmaDisciplina Business Object. 
    /// </summary>
    public class CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO : BusinessBase<CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaDAO, CLS_RelatorioPreenchimentoAlunoTurmaDisciplina>
    {
        /// <summary>
        /// Retorna a chave do cache utilizada para retornar o id dos alunos que possuem registro de anotação da recuperação paralela.
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_AlunoPreenchimentoPorPeriodoDisciplina(int tpc_id, long tur_id, long tud_id)
        {
            return string.Format("Cache_AlunoPreenchimentoPorPeriodoDisciplina_{0}_{1}_{2}", tpc_id, tur_id, tud_id);
        }

        public static void LimpaCache_AlunoPreenchimentoPorPeriodoDisciplina(int tpc_id, long tur_id)
        {
            CacheManager.Factory.RemoveByPattern(string.Format("Cache_AlunoPreenchimentoPorPeriodoDisciplina_{0}_{1}", tpc_id, tur_id));
        }

        /// <summary>
        /// Retorna o id dos alunos que possuem registro de anotação da recuperação paralela.
        /// </summary>
        /// <param name="tpc_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="tud_id"></param>
        /// <param name="appMinutosCache"></param>
        /// <returns></returns>
        public static List<Struct_PreenchimentoAluno> SelecionaAlunoPreenchimentoPorPeriodoDisciplina(int tpc_id, long tur_id, long tud_id, int appMinutosCache = 0)
        {
            List<Struct_PreenchimentoAluno> lista = null;
            Func<List<Struct_PreenchimentoAluno>> retorno = delegate ()
            {
                CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaDAO dao = new CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaDAO();
                return (from dr in dao.SelecionaAlunoPreenchimentoPorPeriodoDisciplina(tpc_id, tur_id, tud_id).AsEnumerable()
                        select new Struct_PreenchimentoAluno { alu_id = Convert.ToInt64(dr["alu_id"])
                                                                , tds_id = Convert.ToInt32(dr["tds_id"])
                                                                , tds_idRelacionada = Convert.ToInt32(dr["tds_idRelacionada"])
                                                                , tur_id = Convert.ToInt64(dr["tur_id"])
                                                                , tur_codigo = dr["tur_codigo"].ToString()
                                                                , tud_id = Convert.ToInt64(dr["tud_id"])
                                                                , tud_nome = dr["tud_nome"].ToString() }).ToList();
            };

            if (appMinutosCache > 0)
            {
                string chave = RetornaChaveCache_AlunoPreenchimentoPorPeriodoDisciplina(tpc_id, tur_id, tud_id);
                lista = CacheManager.Factory.Get
                        (
                            chave,
                            retorno,
                            appMinutosCache
                        );
            }
            else
            {
                lista = retorno();
            }

            return lista;
        }

        /// <summary>
        /// Retorna os lançamentos feito para o aluno de acordo com os parâmetros.
        /// </summary>
        /// <returns></returns>
        public static DataTable SelecionaPorAlunoTurmaRelatorio(long alu_id, long tur_id, int rea_id)
        {
            return new CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaDAO().SelecionaPorAlunoTurmaDisciplinaRelatorioPeriodo(alu_id, tur_id, -1, false, rea_id, -1);
        }

        /// <summary>
        /// Retorna os lançamentos feito para o aluno de acordo com os parâmetros.
        /// </summary>
        /// <returns></returns>
        public static DataTable SelecionaPorAlunoTurmaDisciplinaRelatorioPeriodo(long alu_id, long tud_id, bool apenasComPreenchimento, int rea_id, int tpc_id)
        {
            return new CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaDAO().SelecionaPorAlunoTurmaDisciplinaRelatorioPeriodo(alu_id, -1, tud_id, apenasComPreenchimento, rea_id, tpc_id);
        }

        public static new bool Save(CLS_RelatorioPreenchimentoAlunoTurmaDisciplina entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                return new CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaDAO { _Banco = banco }.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        public static bool Delete(CLS_RelatorioPreenchimentoAlunoTurmaDisciplina entity, int rea_id)
        {
            CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaDAO dao = new CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                bool sucesso = Delete(entity, dao._Banco);
                if (sucesso)
                {
                    CLS_RelatorioAtendimento relatorioAtendimento = CLS_RelatorioAtendimentoBO.GetEntity(new CLS_RelatorioAtendimento { rea_id = rea_id });
                    if (relatorioAtendimento.rea_tipo == (byte)CLS_RelatorioAtendimentoTipo.RP)
                    {
                        ACA_CalendarioAnual calendario = ACA_CalendarioAnualBO.SelecionaPorTurma(entity.tur_id);
                        List<MTR_MatriculaTurma> matriculasAno = MTR_MatriculaTurmaBO.GetSelectMatriculasAlunoAno(entity.alu_id, calendario.cal_ano);
                        matriculasAno.ForEach(p => LimpaCache_AlunoPreenchimentoPorPeriodoDisciplina(entity.tpc_id, p.tur_id));

                        ACA_FormatoAvaliacao fav = TUR_TurmaBO.SelecionaFormatoAvaliacao(entity.tur_id, dao._Banco);
                        if (fav != null && fav.fav_fechamentoAutomatico)
                        {
                            CLS_AlunoFechamentoPendenciaBO.SalvarFilaPendencias(entity.tud_id, entity.tpc_id, dao._Banco);
                        }
                    }
                }
                return sucesso;
            }
            catch (Exception ex)
            {
                dao._Banco.Close(ex);
                throw;
            }
            finally
            {
                if (dao._Banco.ConnectionIsOpen)
                {
                    dao._Banco.Close();
                }
            }
        }
    }
}