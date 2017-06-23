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
                        select new Struct_PreenchimentoAluno { alu_id = Convert.ToInt64(dr["alu_id"]), tds_id = Convert.ToInt32(dr["tds_id"]) }).ToList();
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

        public static new bool Save(CLS_RelatorioPreenchimentoAlunoTurmaDisciplina entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                return new CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaDAO { _Banco = banco }.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }
    }
}