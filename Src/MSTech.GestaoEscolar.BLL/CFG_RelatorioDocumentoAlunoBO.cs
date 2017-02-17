/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using MSTech.Business.Common;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using CFG_Relatorio = MSTech.GestaoEscolar.Entities.CFG_Relatorio;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// CFG_RelatorioDocumentoAluno Business Object
    /// </summary>
    public class CFG_RelatorioDocumentoAlunoBO : BusinessBase<CFG_RelatorioDocumentoAlunoDAO, CFG_RelatorioDocumentoAluno>
    {
        #region Propriedades

        private static IDictionary<ReportNameDocumentos, string[]> parametros;

        /// <summary>
        /// Retorna os parâmetros de mensagens do sistema.
        /// </summary>
        public static IDictionary<ReportNameDocumentos, string[]> Parametros
        {
            get
            {
                if ((parametros == null) || (parametros.Count == 0))
                    RecarregaDocumentosAtivos();

                return parametros;
            }
        }

        #endregion Propriedades

        #region Salvar e alterar

        /// <summary>
        /// O método salva ou altera um registro de CFG_RelatorioDocumentoAluno
        /// </summary>
        /// <param name="entity">Entidade de CFG_RelatorioDocumentoAluno</param>
        /// <returns></returns>
        public static bool Salvar(CFG_RelatorioDocumentoAluno entity)
        {
            if (VerificaRelatorioExistente(entity))
                throw new DuplicateNameException("Já existe um documento do aluno cadastrado com este relatorio.");

            if (VerificaNomeExistente(entity))
                throw new DuplicateNameException("Já existe um documento do aluno cadastrado com este nome.");

            if (VerificaOrdemExistente(entity))
                throw new DuplicateNameException("Já existe um documento do aluno cadastrado com esta ordem.");

            if (entity.Validate())
            {
                return new CFG_RelatorioDocumentoAlunoDAO().Salvar(entity);
            }

            throw new ValidationException(UtilBO.ErrosValidacao(entity));
        }

        #endregion Salvar e alterar

        #region Verificações

        /// <summary>
        /// Verifica se já existe um documento cadastrado com o relatório da entidade a ser salva.
        /// </summary>
        /// <param name="entity">Entidade CFG_RelatorioDocumentoAluno</param>
        /// <returns></returns>
        private static bool VerificaRelatorioExistente(CFG_RelatorioDocumentoAluno entity)
        {
            return new CFG_RelatorioDocumentoAlunoDAO().VerificaRelatorioExistente(entity.ent_id, entity.rlt_id, entity.rda_id);
        }

        /// <summary>
        /// Verifica se já existe um documento cadastrado com a ordem da entidade a ser salva.
        /// </summary>
        /// <param name="entity">Entidade CFG_RelatorioDocumentoAluno</param>
        /// <returns></returns>
        private static bool VerificaOrdemExistente(CFG_RelatorioDocumentoAluno entity)
        {
            return new CFG_RelatorioDocumentoAlunoDAO().VerificaOrdemExistente(entity.ent_id, entity.rda_id, entity.rda_ordem);
        }

        /// <summary>
        /// Verifica se existe um documento do aluno já cadastrado para determinado nome.
        /// </summary>
        /// <param name="entity">Entidade CFG_RelatorioDocumentoEscola</param>
        /// <returns></returns>
        private static bool VerificaNomeExistente(CFG_RelatorioDocumentoAluno entity)
        {
            return new CFG_RelatorioDocumentoAlunoDAO().VerificaNomeExistente(entity.ent_id, entity.rda_id, entity.rda_nomeDocumento);
        }

        #endregion Verificações

        #region Consultas

        /// <summary>
        /// Seleciona os documentos do aluno ativos, filtrados por entidade.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        public static DataTable SelecionaPorEntidade(Guid ent_id)
        {
            return new CFG_RelatorioDocumentoAlunoDAO().SelecionaPorEntidade(ent_id);
        }

        /// <summary>
        /// Lista os documento do aluno conforme a entidade e o sistema que o usuário está autenticado.
        /// </summary>
        /// <param name="idEntidade">id da entidade do usuário logado.</param>
        /// <returns>Lista dos documento do aluno conforme a entidade do sistema.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static IList<CFG_RelatorioDocumentoAluno> ListarDocumentosAluno(Guid idEntidade)
        {
            #region VALIDA PARAMETROS DE ENTRADA

            if (idEntidade.Equals(Guid.Empty))
                throw new ValidationException("Parâmetro idEntidade é obrigatório.");

            #endregion VALIDA PARAMETROS DE ENTRADA

            CFG_RelatorioDocumentoAlunoDAO dal = new CFG_RelatorioDocumentoAlunoDAO();
            return dal.SelectBy_EntidadeSistema(idEntidade);
        }

        /// <summary>
        /// Retorna os parâmetros de integração ativos.
        /// </summary>
        private static void SelecionaParametrosAtivos(out IDictionary<ReportNameDocumentos, string[]> dictionary)
        {
            IList<CFG_RelatorioDocumentoAluno> lt = GetSelect();

            dictionary = (from CFG_RelatorioDocumentoAluno ent in lt
                          where Enum.IsDefined(typeof(ReportNameDocumentos), ent.rlt_id)
                          group ent by ent.rlt_id.ToString() into t
                          select new
                          {
                              chave = t.Key
                              ,
                              valor = t.Select(p => p.rda_nomeDocumento).ToArray()
                          }).ToDictionary(
                                p => (ReportNameDocumentos)Enum.Parse(typeof(ReportNameDocumentos), p.chave)
                                , p => p.valor);
        }

        /// <summary>
        /// Metodo usado na declaração de matricula
        /// </summary>
        /// <param name="alu_ids">Id(s) dos alunos</param>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="situacao">situacao</param>
        /// <param name="ent_id">Entidade logada</param>
        /// <param name="MatriculaEstadual">Nome da matrícula estadual, caso exista</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaDados_DeclaracaoMatricula
            (
            string alu_ids
            , Int64 tur_id
            , Guid ent_id
            , bool situacao
            , string MatriculaEstadual
            )
        {
            CFG_RelatorioDocumentoAlunoDAO dao = new CFG_RelatorioDocumentoAlunoDAO();
            return dao.Select_DeclaracaoMatricula(alu_ids, tur_id, ent_id, situacao, MatriculaEstadual);
        }

        /// <summary>
        /// Seleciona declaracoes HTML.
        /// </summary>
        /// <param name="alu_ids">IDs alunos</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="rlt_id">ID do relatorio.</param>
        /// <param name="cal_id">ID do calendario.</param>
        /// <param name="situacao">situacao</param>
        /// <param name="MatriculaEstadual">Matricula estadual.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDeclaracoesHTML
            (
            string alu_ids
            , Int64 tur_id
            , Guid ent_id
            , int rlt_id
            , int cal_id
            , bool situacao
            , string MatriculaEstadual
            )
        {
            CFG_RelatorioDocumentoAlunoDAO dao = new CFG_RelatorioDocumentoAlunoDAO();
            return dao.SelecionaDeclaracoesHTML(alu_ids, tur_id, ent_id, rlt_id, cal_id, situacao, MatriculaEstadual);
        }

        /// <summary>
        ///  Metodo usado na declaração de conclusão de curso
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="cal_id"></param>
        /// <param name="situacao"></param>
        /// <param name="ent_id"></param>
        /// <param name="telefone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaDados_DeclaracaoConclusaoCurso
            (
            string alu_id
            , Int64 tur_id
            , int cal_id
            , bool situacao
            , Guid ent_id
            )
        {
            CFG_RelatorioDocumentoAlunoDAO dao = new CFG_RelatorioDocumentoAlunoDAO();
            return dao.Select_DeclaracaoConclusaoCurso(alu_id, tur_id, cal_id, situacao, ent_id);
        }

        /// <summary>
        /// Metodo usado na declaração de Ex-Aluno Unidade escolar
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="situacao"></param>
        /// <param name="ent_id"></param>
        /// <param name="telefone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaDados_DeclaracaoExAlunoUnidadeEscolar
            (
            string alu_id
            , Int64 tur_id
            , int uni_id
            , Int64 esc_id
            , bool situacao
            , Guid ent_id
            , Guid telefone
            , Guid email

            )
        {
            CFG_RelatorioDocumentoAlunoDAO dao = new CFG_RelatorioDocumentoAlunoDAO();
            return dao.Select_DeclaracaoExAlunoUnidadeEscolar(alu_id, tur_id, uni_id, esc_id, situacao, ent_id, telefone, email);
        }

        /// <summary>
        /// Metodo usado na declaração de Matricula Ex-Aluno
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="situacao"></param>
        /// <param name="ent_id"></param>
        /// <param name="telefone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaDados_DeclaracaoMatriculaExAluno
            (
            string alu_id
            , Int64 tur_id
            , int uni_id
            , int esc_id
            , bool situacao
            , Guid ent_id
            )
        {
            CFG_RelatorioDocumentoAlunoDAO dao = new CFG_RelatorioDocumentoAlunoDAO();
            return dao.Select_DeclaracaoMatriculaExAluno(alu_id, tur_id, uni_id, esc_id, situacao, ent_id);
        }

        /// <summary>
        /// Metodo usado na declaração de Pedido Transferencia
        /// </summary>
        /// <param name="alu_id">Ids dos alunos selecionados</param>
        /// <param name="tur_id">Turma pesquisada, caso selecionado</param>
        /// <param name="situacao">Ano atual:false || Ano anterior:true</param>
        /// <param name="ent_id">Entidade logada</param>
        /// <param name="telefone">Tipo contato telefone</param>
        /// <param name="email">Tipo contato e-mail</param>
        /// <param name="MatriculaEstadual">Nome da matricula estadual, caso exista</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaDados_DeclaracaoPedidoTransferencia
            (
            string alu_id
            , Int64 tur_id
            , bool situacao
            , Guid ent_id
            , string MatriculaEstadual
            )
        {
            CFG_RelatorioDocumentoAlunoDAO dao = new CFG_RelatorioDocumentoAlunoDAO();
            return dao.Select_DeclaracaoPedidoTransferencia(alu_id, tur_id, situacao, ent_id, MatriculaEstadual);
        }

        /// <summary>
        /// Metodo usado na declaração de Matricula Ex-Aluno
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="situacao"></param>
        /// <param name="ent_id"></param>
        /// <param name="telefone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaDados_DeclaracaoMatriculaPeriodo
            (
            string alu_id
            , Int64 tur_id
            , int uni_id
            , Int64 esc_id
            , string cal_ano
            , bool situacao
            , Guid ent_id
            , Guid telefone
            , Guid email
            )
        {
            CFG_RelatorioDocumentoAlunoDAO dao = new CFG_RelatorioDocumentoAlunoDAO();
            return dao.Select_DeclaracaoMatriculaExAlunoPeriodo(alu_id, tur_id, uni_id, esc_id, cal_ano, situacao, ent_id, telefone, email);
        }

        /// <summary>
        /// Método usado para geração de relatório/declaração HTML de solicitação de transferencia.
        /// </summary>
        /// <param name="alu_ids">Id(s) do(s) aluno(s)</param>
        /// <param name="ent_id">Entidade logada</param>
        /// <param name="telefone">Tipo contato telefone</param>
        /// <param name="email">Tipo contato email</param>
        /// <returns>Datatable para preencher controle</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaDados_DeclaracaoSolicitacaoTransferencia(string alu_ids, Guid ent_id, Guid telefone, Guid email)
        {
            CFG_RelatorioDocumentoAlunoDAO dao = new CFG_RelatorioDocumentoAlunoDAO();
            return dao.Select_DeclaracaoSolicitacaoTransferencia(alu_ids, ent_id, telefone, email);
        }

        /// <summary>
        /// Seleciona os relatórios dos documentos de escola.
        /// </summary>
        /// <returns></returns>
        public static IDictionary<ReportNameDocumentos, string[]> SelecionaRelatorios()
        {
            IList<CFG_Relatorio> lt = CFG_RelatorioBO.GetSelect().Where(p => p.rlt_situacao == 1).ToList();

            return (from CFG_Relatorio ent in lt
                    where Enum.IsDefined(typeof(ReportNameDocumentos), ent.rlt_id)
                    group ent by ent.rlt_id.ToString() into t
                    select new
                    {
                        chave = t.Key
                        ,
                        valor = t.Select(p => p.rlt_nome).ToArray()
                    }).ToDictionary(
                                p => (ReportNameDocumentos)Enum.Parse(typeof(ReportNameDocumentos), p.chave)
                                , p => p.valor);
        }

        /// <summary>
        /// Busca Declaracoes HTML
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDeclaracoesHTML()
        {
            CFG_RelatorioDocumentoAlunoDAO dao = new CFG_RelatorioDocumentoAlunoDAO();
            return dao.SelecionaDeclaracoesHTML
             (
                Enum.GetName(typeof(ChaveParametroDocumentoAluno), ChaveParametroDocumentoAluno.DECLARACAO_HTML),
                out totalRecords
            );
        }

        /// <summary>
        /// Retorna a entidade filtrada por ent_id e rlt_id
        /// </summary>
        /// <param name="ent_id"></param>
        /// <param name="rlt_id"></param>
        /// <returns></returns>
        public static DataTable SelecionaRelatorioDocumentoAluno(Guid ent_id, int rlt_id)
        {
            CFG_RelatorioDocumentoAlunoDAO dao = new CFG_RelatorioDocumentoAlunoDAO();
            return dao.SelecionaRelatorioDocumentoAluno(ent_id, rlt_id);
        }

        /// <summary>
        /// Seleciona a declaracao de comparecimento de responsáveis
        /// </summary>
        /// <param name="alu_ids">Ids dos alunos</param>
        /// <param name="mtu_ids">Ids das matrículas dos alunos</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="tra_id">Tipo de responsável na declaração</param>
        /// <param name="data">Data de comparecimento</param>
        /// <param name="horaInicio">Hora inicial de comparecimento</param>
        /// <param name="horaFim">Hora final de comparecimento</param>
        /// <returns></returns>
        public static DataTable SelecionaDeclaracaoComparecimentoHTML(string alu_ids, string mtu_ids, int esc_id, int tra_id, DateTime data, string horaInicio, string horaFim)
        {
            CFG_RelatorioDocumentoAlunoDAO dao = new CFG_RelatorioDocumentoAlunoDAO();
            return dao.SelecionaDeclaracaoComparecimentoHTML(alu_ids, mtu_ids, esc_id, tra_id, data, horaInicio, horaFim);
        }

        #endregion Consultas

        #region Metodos

        /// <summary>
        /// Recarrega os documentos ativos do aluno.
        /// </summary>
        public static void RecarregaDocumentosAtivos()
        {
            parametros = new Dictionary<ReportNameDocumentos, string[]>();
            lock (parametros)
            {
                SelecionaParametrosAtivos(out parametros);
            }
        }

        #endregion Metodos
    }
}