using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.ComponentModel;
using System.Data;
using MSTech.Validation.Exceptions;
using System.Collections.Generic;
using System;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumerador

    public enum ResultadoHistoricoDisciplina
    {
        Aprovado = 1
        ,
        Reprovado = 2
        ,
        ReprovadoFrequencia = 3
    }

    #endregion Enumerador

    public class ACA_AlunoHistoricoDisciplinaBO : BusinessBase<ACA_AlunoHistoricoDisciplinaDAO, ACA_AlunoHistoricoDisciplina>
    {
        #region Consultas

        /// <summary>
        /// Retorna um datatable contendo todas Disciplinas dos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por 
        /// alu_id
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>      
        /// <returns>DataTable com as Disciplinas dos Historicos do Aluno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_alu_id
        (
            long alu_id
        )
        {
            ACA_AlunoHistoricoDisciplinaDAO dao = new ACA_AlunoHistoricoDisciplinaDAO();
            return dao.SelectBy_alu_id(alu_id, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todas Disciplinas dos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por 
        /// alu_id e ano letivo
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <param name="alh_anoLetivo">Ano letivo do histórico</param>
        /// <returns>DataTable com as Disciplinas dos Historicos do Aluno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_alu_id_alh_anoLetivo
        (
            long alu_id,
            int alh_anoLetivo
        )
        {
            ACA_AlunoHistoricoDisciplinaDAO dao = new ACA_AlunoHistoricoDisciplinaDAO();
            return dao.SelectBy_alu_id_alh_anoLetivo(alu_id, alh_anoLetivo, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todas Disciplinas dos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por 
        /// alu_id e tipo curriculo periodo
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <param name="tcp_id">Id da tabela ACA_TipoCurriculoPeriodo</param>
        /// <returns>DataTable com as Disciplinas dos Historicos do Aluno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_alu_id_tcp_id
        (
            long alu_id,
            int tcp_id
        )
        {
            ACA_AlunoHistoricoDisciplinaDAO dao = new ACA_AlunoHistoricoDisciplinaDAO();
            return dao.SelectBy_alu_id_tcp_id(alu_id, tcp_id, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todas Disciplinas dos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por 
        /// alu_id e historico id
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <param name="alh_id">Id do histórico do aluno</param>
        /// <returns>DataTable com as Disciplinas dos Historicos do Aluno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_alu_id_alh_id
        (
            long alu_id,
            int alh_id
        )
        {
            ACA_AlunoHistoricoDisciplinaDAO dao = new ACA_AlunoHistoricoDisciplinaDAO();
            return dao.SelectBy_alu_id_alh_id(alu_id, alh_id, out totalRecords);
        }

        /// <summary>
        /// Retorna uma lista com os históricos do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        public static List<ACA_AlunoHistoricoDisciplina> RetornaListaHistoricoDisciplinasAluno
        (
            long alu_id
            , TalkDBTransaction banco
        )
        {
            List<ACA_AlunoHistoricoDisciplina> lista = new List<ACA_AlunoHistoricoDisciplina>();

            ACA_AlunoHistoricoDisciplinaDAO dao = new ACA_AlunoHistoricoDisciplinaDAO
            {
                _Banco = banco
            };

            DataTable dt = GetSelectBy_alu_id(alu_id);

            foreach (DataRow dr in dt.Rows)
            {
                ACA_AlunoHistoricoDisciplina ent = new ACA_AlunoHistoricoDisciplina();
                lista.Add(dao.DataRowToEntity(dr, ent));
            }

            return lista;
        }
                       

        #endregion

        #region Inclusão ou alteração

        /// <summary>
        /// Inclui uma nova disciplina para o histórico do aluno
        /// </summary>
        /// <param name="entity">Entidade ACA_AlunoHistoricoDisciplina</param>        
        /// <param name="banco"></param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_AlunoHistoricoDisciplina entity
            , TalkDBTransaction banco
        )
        {
            if (entity.Validate())
            {
                ACA_AlunoHistoricoDisciplinaDAO dao = new ACA_AlunoHistoricoDisciplinaDAO { _Banco = banco };

                return dao.Salvar(entity);
            }

            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }

        #endregion

        #region Métodos em lote

        /// <summary>
        /// O método retorna um DataRow com as informações da entidade.
        /// </summary>
        /// <param name="entity">Entidade de historico por disciplina do aluno.</param>
        /// <param name="dr">DataRow de historico.</param>
        /// <returns></returns>
        public static DataRow EntityToDataRow(ACA_AlunoHistoricoDisciplina entity, DataRow dr)
        {
            dr["alu_id"] = entity.alu_id;
            dr["alh_id"] = entity.alh_id;
            dr["ahd_id"] = entity.ahd_id;

            if (entity.tds_id > 0)
                dr["tds_id"] = entity.tds_id;
            else
                dr["tds_id"] = DBNull.Value;

            dr["ahd_disciplina"] = entity.ahd_disciplina;

            if (entity.ahd_resultado > 0)
                dr["ahd_resultado"] = entity.ahd_resultado;
            else
                dr["ahd_resultado"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.ahd_resultadoDescricao))
                dr["ahd_resultadoDescricao"] = entity.ahd_resultadoDescricao;
            else
                dr["ahd_resultadoDescricao"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.ahd_avaliacao))
                dr["ahd_avaliacao"] = entity.ahd_avaliacao;
            else
                dr["ahd_avaliacao"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.ahd_frequencia))
                dr["ahd_frequencia"] = entity.ahd_frequencia;
            else
                dr["ahd_frequencia"] = DBNull.Value;

            dr["ahd_indicacaoDependencia"] = entity.ahd_indicacaoDependencia;
            dr["ahd_situacao"] = entity.ahd_situacao;

            if (entity.ahd_qtdeFaltas > 0)
                dr["ahd_qtdeFaltas"] = entity.ahd_qtdeFaltas;
            else
                dr["ahd_qtdeFaltas"] = DBNull.Value;

            if (entity.ahp_id > 0)
                dr["ahp_id"] = entity.ahp_id;
            else
                dr["ahp_id"] = DBNull.Value;

            return dr;
        }

        #endregion Métodos em lote
    }
}
