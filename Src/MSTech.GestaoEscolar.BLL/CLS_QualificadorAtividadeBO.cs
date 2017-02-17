/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.ComponentModel;
    using System.Data;
    using MSTech.Validation.Exceptions;
    using System;
    using MSTech.GestaoEscolar.CustomResourceProviders;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Description: CLS_QualificadorAtividade Business Object. 
    /// </summary>
    public class CLS_QualificadorAtividadeBO : BusinessBase<CLS_QualificadorAtividadeDAO, CLS_QualificadorAtividade>
    {
        #region Enumeradores

        public enum EnumTipoQualificadorAtividade
        {
            AtividadeDiversificada = 1
            ,
            InstrumentoDeAvaliacao = 2
            ,
            RecuperacaoDaAtividadeDiversificada = 3
            ,
            RecuperacaoDoInstrumentoDeAvaliacao = 4
            ,
            LicaoDeCasa = 5
        }

        public enum EnumSituacaoQualificadorAtividade
        {
            Ativo = 1
            ,
            Excluido = 3
        }

        #endregion Enumeradores

        #region Metodos
        
        /// <summary>
        /// Retorna todos os qualificadores de atividade não excluídos logicamente
        /// Sem paginação
        /// </summary>            
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectAtivos()
        {
            CLS_QualificadorAtividadeDAO dao = new CLS_QualificadorAtividadeDAO();
            return dao.SelectAtivos();
        }

        /// <summary>
        /// Retorna os tipos de atividade para o qualificador.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetTipos()
        {
            DataTable dtQualificador = SelectAtivos();
            return (from DataRow row in dtQualificador.Rows
                    select new
                    {
                        chave = (int)row["qat_id"]
                        ,
                        valor = row["qat_nome"].ToString()
                    }).ToDictionary(p => p.chave, p => p.valor);
        }

        /// <summary>
        /// Retorna os qualificadores de atividade ativos configurados para a turma disciplina.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetTiposBy_TurmaDisciplina(long tud_id)
        {
            DataTable dtQualificador = new CLS_QualificadorAtividadeDAO().SelectAtivosBy_TurmaDisciplina(tud_id);
            return (from DataRow row in dtQualificador.Rows
                    select new
                    {
                        chave = (int)row["qat_id"]
                        ,
                        valor = row["qat_nome"].ToString()
                    }).ToDictionary(p => p.chave, p => p.valor);
        }
        
        #endregion Metodos
    }
}
