using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using MSTech.Data.Common;
using System.Data;
using MSTech.Validation.Exceptions;
using Newtonsoft.Json.Linq;
using MSTech.CoreSSO.Entities;
using MSTech.CoreSSO.BLL;
using System.Web;
using MSTech.CoreSSO.DAL;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// DCL_Protocolo Business Object 
    /// </summary>
    public class DCL_ProtocoloBO : BusinessBase<DCL_ProtocoloDAO, DCL_Protocolo>
    {
        /// <summary>
        /// Status do processamento do protocolo.
        /// </summary>
        public enum eStatus
        {
            NaoProcessado = 1,
            EmProcessamento = 2,
            ProcessadoComSucesso = 3,
            ProcessadoComErro = 4,
            ProcessadoComErrosValidacao = 5
        }

        public enum eTipo
        {
            Aula = 1,
            JustificativaFaltaAluno = 2,
            PlanejamentoAnual = 3,
            Logs = 4,
            Foto = 5,
            CompensacaoDeAula = 6
        }

        #region Consultas

        /// <summary>
        /// Retorna os protocolos de acordo com a entidade e o período.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="dtInicio">Data de início</param>
        /// <param name="dtFim">Data de fim</param>
        /// <param name="status">Situação do protocolo</param>
        /// <param name="tipoProtocolo">Tipo do Protocolo</param>
        /// <returns></returns>
        public static DataTable SelectBy_Entidade_Data(Guid ent_id, DateTime dtInicio, DateTime dtFim, byte status, byte tipoProtocolo)
        {
            DCL_ProtocoloDAO dao = new DCL_ProtocoloDAO();
            DataTable dt = dao.SelectBy_EntidadeData(ent_id, dtInicio, dtFim, status, tipoProtocolo);

            totalRecords = dt.Rows.Count;

            return dt;
        }

        /// <summary>
        /// retorna os protocolos vinculados a escola a partir de uma data especifica podendo filtrar pelo tipo do protocolo
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="dataBase">data base para seleção dos protocolos</param>
        /// <param name="pro_tipo">tipo do protocolo</param>
        /// <returns></returns>
        public static DataTable SelectBy_Escola(Int32 esc_id, DateTime dataBase, int pro_tipo)
        {
            DCL_ProtocoloDAO dao = new DCL_ProtocoloDAO();
            return dao.SelectBy_Escola(esc_id, dataBase, pro_tipo);
        }

        /// <summary>
        /// Carrega a lista de protocolos segundo os números de protocolo.
        /// </summary>
        /// <param name="pro_protocolo">Número do Protocolo</param>
        /// <returns></returns>
        public static List<DCL_Protocolo> SelectBy_Protocolos(string pro_protocolo)
        {
            DCL_ProtocoloDAO dao = new DCL_ProtocoloDAO();
            DataTable dt = dao.SelectBy_Protocolos(pro_protocolo);

            List<DCL_Protocolo> lista =
                (
                    from DataRow dr in dt.Rows
                    select dao.DataRowToEntity(dr, new DCL_Protocolo())
                ).ToList();

            return lista;
        }

        /// <summary>
        /// Carrega o protocolo segundo a entidade e o número de protocolo
        /// </summary>
        /// <param name="ent_id">Id da Entidade</param>
        /// <param name="pro_protocolo">Número do Protocolo</param>
        /// <returns></returns>
        public static DCL_Protocolo GetEntityBy_Protocolo(long pro_protocolo, Guid ent_id)
        {
            try
            {
                DCL_ProtocoloDAO dao = new DCL_ProtocoloDAO();
                return dao.CarregarBy_Protocolo(pro_protocolo, ent_id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna o próximo protocolo pendente de processamento, ordenando pela data de criação
        /// crescente.
        /// </summary>
        /// <returns></returns>
        public static DCL_Protocolo SelecionaProximoNaoProcessado()
        {
            DCL_ProtocoloDAO dao = new DCL_ProtocoloDAO();
            DataTable dt = dao.SelectNaoProcessados();

            return (from DataRow dr in dt.Rows
                    select dao.DataRowToEntity(dr, new DCL_Protocolo())
                    ).FirstOrDefault();
        }

        /// <summary>
        /// Seleciona uma quantidade de protocolos filtrados pelo tipo.
        /// </summary>
        /// <param name="pro_tipo">Tipo do protocolo.</param>
        /// <param name="qtdeProtocolo">Quantidade máxima de protocolos.</param>
        /// <returns></returns>
        public static List<DCL_Protocolo> SelecionaNaoProcessadosPorTipo(eTipo pro_tipo, int qtdeProtocolo)
        {
            return new DCL_ProtocoloDAO().SelecionaNaoProcessadosPorTipo((byte)pro_tipo, qtdeProtocolo);
        }

        #endregion

        #region Saves

        /// <summary>
        /// Salva a entidade DCL_Protocolo.
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <returns></returns>
        public new static bool Save(DCL_Protocolo entity)
        {
            TalkDBTransaction banco = new DCL_ProtocoloDAO()._Banco.CopyThisInstance();
            try
            {
                banco.Open();

                return Save(entity, banco);
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                {
                    banco.Close();
                }
            }
        }

        /// <summary>
        /// Salva a entidade DCL_Protocolo.
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <param name="banco">Transação com banco de dados</param>
        /// <returns></returns>
        public new static bool Save(DCL_Protocolo entity, TalkDBTransaction banco)
        {
            if (!entity.Validate())
                throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));

            DCL_ProtocoloDAO dao = new DCL_ProtocoloDAO() { _Banco = banco };
            bool ret = dao.Salvar(entity);
            return ret;
        }

        /// <summary>
        /// Colocar um protocolo executado com erro para reprocessar
        /// </summary>
        /// <param name="pro_id">Id do Protocolo</param>
        /// <param name="novoPacote">Novo pacote a ser processado</param>
        /// <param name="versao">Versão do aplicativo que enviou o protocolo</param>
        /// <returns></returns>
        public static bool Reprocessar(Guid pro_id, string novoPacote, string versao)
        {
            DCL_ProtocoloReprocessoDAO dao = new DCL_ProtocoloReprocessoDAO();
            TalkDBTransaction banco = dao._Banco;

            try
            {
                DCL_Protocolo protocolo = new DCL_Protocolo() { pro_id = pro_id };
                DCL_ProtocoloReprocesso entity = null;

                DCL_ProtocoloBO.GetEntity(protocolo);

                if (protocolo.IsNew)
                {
                    throw new ProtocoloNãoEncontrado();
                }

                //Verifica se está com status de "Processado com Erro"
                if (protocolo.pro_status == (byte)DCL_ProtocoloBO.eStatus.ProcessadoComErro)
                {
                    entity = new DCL_ProtocoloReprocesso()
                    {
                        pro_id = protocolo.pro_id,
                        prp_pacote = protocolo.pro_pacote,
                        prp_status = protocolo.pro_status,
                        prp_statusObervacao = protocolo.pro_statusObservacao,
                        prp_situacao = protocolo.pro_situacao,
                        prp_dataCriacao = DateTime.Now,
                        prp_dataAlteracao = DateTime.Now
                    };

                    if (!string.IsNullOrEmpty(versao))
                    {
                        protocolo.pro_versaoAplicativo = versao;
                    }

                    protocolo.pro_status = 1;
                    protocolo.pro_statusObservacao = string.Empty;

                    if (!string.IsNullOrEmpty(novoPacote))
                    {
                        protocolo.pro_pacote = novoPacote;
                    }

                    banco.Open();

                    DCL_ProtocoloReprocessoBO.Save(entity, banco);
                    DCL_ProtocoloBO.Save(protocolo, banco);

                    return true;
                }
                else
                {
                    throw new ReprocessarProtocoloSemErro();
                }
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
        /// Atualiza os protocolos passados por parâmetro em uma lista.
        /// </summary>
        /// <param name="dtProtocolo">Tabela de protocolos.</param>
        /// <returns></returns>
        public static bool AtualizaListaProtocolos(List<DCL_Protocolo> ltProtocolo, TalkDBTransaction banco = null)
        {
            DataTable dtProtocolo = DCL_Protocolo.TipoTabela_Protocolo();
            ltProtocolo.ForEach(p => dtProtocolo.Rows.Add(ProtocoloToDataRow(p, dtProtocolo.NewRow())));

            return AtualizaProtocolos(dtProtocolo, banco);
        }

        /// <summary>
        /// Atualiza os protocolos passados por parâmetro em uma lista.
        /// </summary>
        /// <param name="dtProtocolo">Tabela de protocolos.</param>
        /// <returns></returns>
        public static bool AtualizaProtocolos(DataTable dtProtocolo, TalkDBTransaction banco = null)
        {
            return banco == null ?
                new DCL_ProtocoloDAO().AtualizaProtocolos(dtProtocolo) :
                new DCL_ProtocoloDAO { _Banco = banco }.AtualizaProtocolos(dtProtocolo);
        }

        #region Sincronização com diário de classe (Log do sistema)

        /// <summary>
        /// Retorna inf. detalhadas da dt. aula/escola/turma/professor vinculadas ao protocolo (pro_id)
        /// </summary>
        /// <param name="pro_id">ID do Protocolo</param>
        /// <returns></returns>
        public static DataTable SelectBy_Protocolo_TurmaAula(Guid pro_id)
        {
            DCL_ProtocoloDAO dao = new DCL_ProtocoloDAO();
            DataTable dt = dao.SelectBy_Protocolo_TurmaAula(pro_id);

            totalRecords = dt.Rows.Count;

            return dt;
        }

        /// <summary>
        /// Retorna inf. detalhadas da Escola/Turma/Disciplina vinculadas ao protocolo (pro_id)
        /// </summary>
        /// <param name="pro_id">ID do Protocolo</param>
        /// <returns></returns>
        public static DataTable SelectBy_Protocolo_TurmaDisciplinaPlanejamento(Guid pro_id)
        {
            DCL_ProtocoloDAO dao = new DCL_ProtocoloDAO();
            DataTable dt = dao.SelectBy_Protocolo_TurmaDisciplinaPlanejamento(pro_id);

            totalRecords = dt.Rows.Count;

            return dt;
        }

        /// <summary>
        /// Retorna inf. detalhadas da Nome aluno/Matricula/Escola/Turma/Grupamento de ensino vinculadas ao protocolo (pro_id)
        /// </summary>
        /// <param name="pro_id">ID do Protocolo</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns></returns>
        public static DataTable SelectBy_Protocolo_ProtocoloAluno(Guid pro_id, Guid ent_id)
        {
            DCL_ProtocoloDAO dao = new DCL_ProtocoloDAO();

            bool matriculaEstadual = !string.IsNullOrEmpty(ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, ent_id));

            DataTable dt = dao.SelectBy_Protocolo_ProtocoloAluno(pro_id, matriculaEstadual);

            totalRecords = dt.Rows.Count;

            return dt;
        }

        /// <summary>
        /// Retorna inf. detalhadas Nome Professor, Disciplina, Qtde de aulas compensadas, ativ. desenvolvida no vinculadas ao protocolo (pro_id)
        /// </summary>
        /// <param name="pro_id">ID do Protocolo</param>
        /// <returns></returns>
        public static DataTable SelectBy_Protocolo_CompensacaoDeAula(Guid pro_id)
        {
            DCL_ProtocoloDAO dao = new DCL_ProtocoloDAO();

            DataTable dt = dao.SelectBy_Protocolo_CompensacaoDeAula(pro_id);

            totalRecords = dt.Rows.Count;

            return dt;
        }

        #endregion


        #endregion

        #region Validações

        /// <summary>
        /// O protocolo só poderá ser processado se a turma for ativa e pertencer ao 
        /// ano letivo corrente.
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <param name="tud_id">id da turmaDIsciplina</param>
        /// <returns></returns>
        public static bool PodeProcessarProtocolo(long tur_id, long tud_id)
        {
            DataTable dt = new DCL_ProtocoloDAO().SelecionarTurmaAtivaAnoCorrente(tur_id, tud_id);

            if (dt.Rows.Count > 0)
                return true;

            return false;
        }

        #endregion

        /// <summary>
        /// Retorna um datarow com os dados de um protocolo.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static DataRow ProtocoloToDataRow(DCL_Protocolo entity, DataRow dr)
        {
            if (entity.idAula > 0)
                dr["idAula"] = entity.idAula;
            else
                dr["idAula"] = DBNull.Value;

            dr["pro_id"] = entity.pro_id;
            dr["pro_status"] = entity.pro_status;
            dr["pro_statusObservacao"] = entity.pro_statusObservacao;

            if (entity.tur_id > 0)
                dr["tur_id"] = entity.tur_id;
            else
                dr["tur_id"] = DBNull.Value;

            if (entity.tud_id > 0)
                dr["tud_id"] = entity.tud_id;
            else
                dr["tud_id"] = DBNull.Value;

            if (entity.tau_id > 0)
                dr["tau_id"] = entity.tau_id;
            else
                dr["tau_id"] = DBNull.Value;

            if (entity.pro_qtdeAlunos >= 0)
                dr["pro_qtdeAlunos"] = entity.pro_qtdeAlunos;
            else
                dr["pro_qtdeAlunos"] = DBNull.Value;

            dr["pro_situacao"] = entity.pro_situacao;
            dr["pro_tentativa"] = entity.pro_tentativa;

            return dr;
        }
    }
}
