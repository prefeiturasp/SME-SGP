using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MSTech.Business.Common;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.DAL;
using MSTech.CoreSSO.Entities;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// Situações do responsável do aluno
    /// </summary>
    public enum ACA_AlunoResponsavelSituacao : byte
    {
        Ativo = 1,
        Excluido = 3,
        Falecido = 4
    }

    /// <summary>
    /// Tipo de responsável (1-Responsável financeiro, 2-Responsável pedagógico, 3-Responsável financeiro e pedagógico)
    /// </summary>
    public enum ACA_AlunoResponsavelTipoResponsavel : byte
    {
        Financeiro = 1
        ,
        Pedagogico = 2
        ,
        FinanceiroPedagogico = 3
        ,
        NaoResponsavel = 4
    }

    public class ACA_AlunoResponsavelBO : BusinessBase<ACA_AlunoResponsavelDAO, ACA_AlunoResponsavel>
    {
        /// <summary>
        /// Estrutura utilizada para cadastrar responsáveis para o aluno.
        /// </summary>
        [Serializable]
        public struct StructCadastro
        {
            /// <summary>
            /// Responsável do aluno.
            /// </summary>
            public ACA_AlunoResponsavel entAlunoResp;

            /// <summary>
            /// Pessoa do responsável do aluno.
            /// </summary>
            public PES_Pessoa entPessoa;

            /// <summary>
            /// Lista de documentos do responsável do aluno.
            /// </summary>
            public List<PES_PessoaDocumento> listPessoaDoc;

            /// <summary>
            /// Lista de contatos do responsável do aluno.
            /// </summary>
            public IList<PES_PessoaContato> listPessoaContato;

            /// <summary>
            /// Tipo de responsável do aluno.
            /// </summary>
            public int tipoResponsavelPadrao;
        }

        /// <summary>
        /// Retorna uma lista com as entidades AlunoResponsavel e Pes_pessoa, dos responsáveis
        /// cadastrados para o aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="banco">Banco - transação</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<StructCadastro> RetornaResponsaveisAluno
        (
            long alu_id
            , TalkDBTransaction banco
        )
        {
            List<StructCadastro> lista = new List<StructCadastro>();
            ACA_AlunoResponsavelDAO dao = new ACA_AlunoResponsavelDAO();

            if (banco != null)
            {
                dao._Banco = banco;
            }

            DataTable dt = dao.SelectBy_Aluno(alu_id);

            foreach (DataRow dr in dt.Rows)
            {
                StructCadastro item = new StructCadastro();

                item.entAlunoResp = dao.DataRowToEntity(dr, new ACA_AlunoResponsavel());
                item.entPessoa = new PES_PessoaDAO().DataRowToEntity(dr, new PES_Pessoa());
                item.listPessoaDoc = InserirListDocumentoResp(dr, item.entPessoa.pes_id);
                lista.Add(item);
            }

            return lista;
        }

        /// <summary>
        /// Retorna uma lista de responsáveis de um aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <returns></returns>
        public static List<ACA_AlunoResponsavel> SelecionaResponsaveisPorAluno(long alu_id)
        {
            List<ACA_AlunoResponsavel> lista = new List<ACA_AlunoResponsavel>();
            ACA_AlunoResponsavelDAO dao = new ACA_AlunoResponsavelDAO();
            DataTable dt = dao.SelectBy_Aluno(alu_id);

            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(dao.DataRowToEntity(dr, new ACA_AlunoResponsavel()));
            }

            return lista;
        }

        /// <summary>
        /// Retorna um datatable contendo todos os contatos dos responsaveis pelo aluno
        /// que não foram excluídos logicamente, filtrados por
        /// id do aluno
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <returns>DataTable com os contatos da entidade</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectContatosBy_alu_id
        (
            long alu_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
            {
                pageSize = 1;
            }

            totalRecords = 0;
            ACA_AlunoResponsavelDAO dao = new ACA_AlunoResponsavelDAO();
            try
            {
                return dao.SelectContatosBy_alu_id(alu_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Atualiza a profissão e a situação da pessoa (Responsável).
        /// Caso for alterado a profissão de um responsável
        /// Caso for do tipo 4 - (Falecido) situação será atualizado
        /// As atualizações serão feitas para todos a alunos no qual o mesmo é responsável
        /// </summary>
        /// <param name="pes_id">pes_id do responsável do aluno</param>
        /// <param name="alr_profissao">profissão do responsável</param>
        /// <param name="alr_situacao">situação do responsável</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool AtualizaResponsavel_Profissao_Situacao
        (
            Guid pes_id
            , string alr_profissao
            , byte alr_situacao
            , TalkDBTransaction banco
        )
        {
            return AtualizaResponsavel_Profissao_Situacao(pes_id, alr_profissao, "", alr_situacao, banco);
        }

        /// <summary>
        /// Atualiza a profissão e a situação da pessoa (Responsável).
        /// Caso for alterado a profissão de um responsável
        /// Caso for do tipo 4 - (Falecido) situação será atualizado
        /// As atualizações serão feitas para todos a alunos no qual o mesmo é responsável
        /// </summary>
        /// <param name="pes_id">pes_id do responsável do aluno</param>
        /// <param name="alr_profissao">profissão do responsável</param>
        /// <param name="alr_empresa">empresa do responsável</param>
        /// <param name="alr_situacao">situação do responsável</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool AtualizaResponsavel_Profissao_Situacao
        (
            Guid pes_id
            , string alr_profissao
            , string alr_empresa
            , byte alr_situacao
            , TalkDBTransaction banco
        )
        {
            ACA_AlunoResponsavelDAO dao = new ACA_AlunoResponsavelDAO();

            if (banco != null)
            {
                dao._Banco = banco;
            }

            try
            {
                return dao.Update_ProfissaoSituacao_AlunoResponsavel(pes_id, alr_profissao, alr_empresa, alr_situacao);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna a situação do responsável do aluno.
        /// passando apenas o pes_id por parâmetro
        /// </summary>
        /// <param name="pes_id">pes_id do responsável do aluno</param>
        /// <param name="alr_situacao"></param>
        /// <param name="alr_profissao"></param>
        /// <returns>retorna a situação do responsável do aluno</returns>
        public static void RetornaAlunoResponsavel_Situacao_Profissao
        (
            Guid pes_id
            , out byte alr_situacao
            , out string alr_profissao
        )
        {
            string alr_empresa = "";
            RetornaAlunoResponsavel_Situacao_Profissao(pes_id, out alr_situacao, out alr_profissao, out alr_empresa);
        }

        /// <summary>
        /// Retorna a situação do responsável do aluno.
        /// passando apenas o pes_id por parâmetro
        /// </summary>
        /// <param name="pes_id">pes_id do responsável do aluno</param>
        /// <param name="alr_situacao"></param>
        /// <param name="alr_profissao"></param>
        /// <param name="alr_profissao"></param>
        /// <returns>retorna a situação do responsável do aluno</returns>
        public static void RetornaAlunoResponsavel_Situacao_Profissao
        (
            Guid pes_id
            , out byte alr_situacao
            , out string alr_profissao
            , out string alr_empresa
        )
        {
            ACA_AlunoResponsavelDAO dao = new ACA_AlunoResponsavelDAO();
            try
            {
                DataTable dt = dao.RetornaAlunoResponsavel_Situacao_Profissao(pes_id);

                alr_situacao = 0;
                alr_profissao = string.Empty;
                alr_empresa = string.Empty;

                foreach (DataRow dr in dt.Rows)
                {
                    alr_situacao = Convert.ToByte(dr["alr_situacao"].ToString());
                    alr_profissao = dr["alr_profissao"].ToString();
                    alr_empresa = dr["alr_empresa"].ToString();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Salva os responsáveis do aluno, e o pai e a mãe na filiação do aluno.
        /// </summary>
        /// <param name="listaResponsavel">Lista da estrutura do cadastro</param>
        /// <param name="entAluno">Aluno</param>
        /// <param name="bancoGestao">Transação do Gestão</param>
        /// <param name="bancoCore">Transação do Core</param>
        /// <param name="tra_idPrincipal">Id do tipo de responsável principal</param>
        /// <param name="salvarMaiusculo">Indica se os nomes devem ser salvos em maiúsculos</param>
        /// <param name="entPessoaAluno">Pessoa referente ao aluno - seta o id do Pai e da Mãe</param>
        /// <param name="obrigatorioTipoResponsavel">Validar se foi informado o Id do tipo de responsável principal.</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        public static void SalvarResponsaveisAluno
        (
            List<StructCadastro> listaResponsavel,
            ACA_Aluno entAluno,
            TalkDBTransaction bancoGestao,
            TalkDBTransaction bancoCore,
            Int32 tra_idPrincipal,
            bool salvarMaiusculo,
            ref PES_Pessoa entPessoaAluno,
            bool obrigatorioTipoResponsavel,
            Guid ent_id
        )
        {
            List<StructCadastro> listCadastrados = new List<StructCadastro>();
            List<StructCadastro> listaInseridos = new List<StructCadastro>();
            Guid pes_idMae = Guid.Empty;
            Guid pes_idPai = Guid.Empty;

            if (tra_idPrincipal <= 0 && obrigatorioTipoResponsavel)
            {
                throw new ValidationException("É necessário informar o responsável do aluno.");
            }

            if (listaResponsavel.Count == 0)
            {
                throw new ValidationException("Responsável é obrigatório.");
            }

            if (!entAluno.IsNew)
            {
                // Guardar os responsáveis que já tinham sido cadastrados.
                listCadastrados = RetornaResponsaveisAluno(entAluno.alu_id, null);
            }

            // Buscando ids dos tipos de responsável dos parâmetros.
            Int32 tra_idMae = TipoResponsavelAlunoParametro.tra_idMae(ent_id);
            Int32 tra_idPai = TipoResponsavelAlunoParametro.tra_idPai(ent_id);
            Int32 tra_idProprio = TipoResponsavelAlunoParametro.tra_idProprio(ent_id);

            // ID do tipo de documento CPF.
            string docPadraoCPF = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF);
            Guid tdo_idCPF = String.IsNullOrEmpty(docPadraoCPF) ? Guid.Empty : new Guid(docPadraoCPF);

            // ID do tipo de documento RG.
            string docPadraoRG = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG);
            Guid tdo_idRG = String.IsNullOrEmpty(docPadraoRG) ? Guid.Empty : new Guid(docPadraoRG);

            // ID do tipo de documento NIS.
            Guid tdo_idNis = ACA_ParametroAcademicoBO.ParametroValorGuidPorEntidade(eChaveAcademico.TIPO_DOCUMENTACAO_NIS, ent_id);

            List<string> ListValidacoesDoc = new List<string>();

            foreach (StructCadastro resp in listaResponsavel)
            {
                //Responsáveis com o mesmo numero de documento
                foreach (PES_PessoaDocumento psd in resp.listPessoaDoc)
                {
                    if (listaResponsavel.Any(p => p.entAlunoResp.pes_id != resp.entAlunoResp.pes_id &&
                                             p.listPessoaDoc.Any(d => d.psd_numero == psd.psd_numero)) &&
                        !ListValidacoesDoc.Contains("Há mais de um responsável com o mesmo número de documento."))
                        ListValidacoesDoc.Add("Há mais de um responsável com o mesmo número de documento.");
                }

                // Responável tem data de nascimento maior que a data atual.
                if (resp.entPessoa.pes_dataNascimento > DateTime.Now)
                {
                    if (resp.entAlunoResp.tra_id == TipoResponsavelAlunoParametro.tra_idMae(ent_id))
                    {
                        ListValidacoesDoc.Add("A data de nascimento da mãe não pode ser maior que a data atual.");
                    }

                    if (resp.entAlunoResp.tra_id == TipoResponsavelAlunoParametro.tra_idPai(ent_id))
                    {
                        ListValidacoesDoc.Add("A data de nascimento do pai não pode ser maior que a data atual.");
                    }
                }

                // Responsavel do aluno é falecido e mora com ele -- erro
                if (resp.entAlunoResp.alr_moraComAluno && resp.entAlunoResp.alr_situacao == Convert.ToByte(ACA_AlunoResponsavelSituacao.Falecido))
                {
                    ListValidacoesDoc.Add("Responsável do aluno não pode morar com o aluno e ser falecido.");
                }

                // Responsavel tem data de nascimento mais nova que a do aluno
                if (resp.entPessoa.pes_dataNascimento > entPessoaAluno.pes_dataNascimento)
                {
                    if (resp.entAlunoResp.tra_id == TipoResponsavelAlunoParametro.tra_idMae(ent_id))
                    {
                        ListValidacoesDoc.Add("A data de nascimento da mãe não pode ser maior que a data de nascimento do aluno.");
                    }

                    if (resp.entAlunoResp.tra_id == TipoResponsavelAlunoParametro.tra_idPai(ent_id))
                    {
                        ListValidacoesDoc.Add("A data de nascimento do pai não pode ser maior que a data de nascimento do aluno.");
                    }
                }

                //Valida código NIS do aluno
                int index = resp.listPessoaDoc.FindIndex(p => p.tdo_id == tdo_idNis);

                if (index >= 0 && !(string.IsNullOrEmpty(resp.listPessoaDoc[index].psd_numero)))
                {
                    string TipoPessoa;

                    if (resp.entAlunoResp.tra_id == tra_idMae)
                    {
                        TipoPessoa = "A mãe";
                    }
                    else if (resp.entAlunoResp.tra_id == tra_idPai)
                    {
                        TipoPessoa = "O pai";
                    }
                    else
                    {
                        TipoPessoa = "O responsável";
                    }

                    if (ACA_AlunoBO.NISInvalido(resp.listPessoaDoc[index].psd_numero))
                    {
                        ListValidacoesDoc.Add(TipoPessoa + " possui o número do NIS inválido.");
                    }
                }

                // Adiciona na lista dos dados inseridos.
                listaInseridos.Add(
                    SalvarResponsavel(tra_idPrincipal,
                        entAluno,
                        resp,
                        tra_idProprio,
                        bancoCore,
                        tra_idMae,
                        tra_idPai,
                        bancoGestao,
                        tdo_idCPF,
                        tdo_idRG,
                        tdo_idNis,
                        ref pes_idMae,
                        ref pes_idPai,
                        salvarMaiusculo, 
                        ref ListValidacoesDoc,
                        listaResponsavel,
                        ent_id));
            }

            // Se não foi inserido nenhum item na lista como principal.
            if (!listaInseridos.Exists(p => p.entAlunoResp.alr_principal) && obrigatorioTipoResponsavel)
            {
                ListValidacoesDoc.Add("É necessário informar o responsável do aluno.");
            }

            if (ListValidacoesDoc.Count > 0)
                throw new ValidationException(string.Join("<BR/>", ListValidacoesDoc.ToArray()));

            // Salva na pessoa do aluno os Ids do pai e da mãe.
            entPessoaAluno = new PES_Pessoa
            {
                pes_id = entAluno.pes_id
            };
            PES_PessoaBO.GetEntity(entPessoaAluno, bancoCore);

            // Se mudou a mãe.
            if (entPessoaAluno.pes_idFiliacaoMae != pes_idMae)
            {
                entPessoaAluno.pes_idFiliacaoMae = pes_idMae;
                PES_PessoaBO.Save(entPessoaAluno, bancoCore);
            }

            // Se mudou o pai.
            if (entPessoaAluno.pes_idFiliacaoPai != pes_idPai)
            {
                entPessoaAluno.pes_idFiliacaoPai = pes_idPai;
                PES_PessoaBO.Save(entPessoaAluno, bancoCore);
            }

            // Percorrer os itens que existiam antes, para excluir os que não tem mais.
            foreach (StructCadastro item in listCadastrados)
            {
                if (item.entPessoa.pes_id == entAluno.pes_id)
                    continue;
                VerifcaItemCadastrado(listaInseridos, bancoCore, bancoGestao, item, tra_idProprio);
            }
        }

        /// <summary>
        /// Inseri os documentos do responsável em uma list
        /// </summary>
        /// <param name="dr">linha da tabela referente ao aluno</param>
        /// <param name="pes_id">id da pessoa (responsável)</param>
        /// <returns>uma list com os documentos do responsável</returns>
        private static List<PES_PessoaDocumento> InserirListDocumentoResp(DataRow dr, Guid pes_id)
        {
            List<PES_PessoaDocumento> listPessoaDoc = new List<PES_PessoaDocumento>();

            // Adiciona na list o documento RG do responsável.
            if (!string.IsNullOrEmpty(dr["numeroDocumentoRG"].ToString()))
            {
                PES_PessoaDocumento entityDocResp = new PES_PessoaDocumento
                {
                    pes_id = pes_id,
                    tdo_id = new Guid(dr["tipoDocumentoRG"].ToString()),
                    psd_numero = dr["numeroDocumentoRG"].ToString()
                };
                listPessoaDoc.Add(entityDocResp);
            }

            // Adiciona na list o documento CPF do responsável.
            if (!string.IsNullOrEmpty(dr["numeroDocumentoCPF"].ToString()))
            {
                PES_PessoaDocumento entityDocResp = new PES_PessoaDocumento
                {
                    pes_id = pes_id,
                    tdo_id = new Guid(dr["tipoDocumentoCPF"].ToString()),
                    psd_numero = dr["numeroDocumentoCPF"].ToString()
                };
                listPessoaDoc.Add(entityDocResp);
            }

            //Adiciona na list o documento NIS do responsável.
            if (!string.IsNullOrEmpty(dr["numeroDocumentoNIS"].ToString()))
            {
                PES_PessoaDocumento entityDocResp = new PES_PessoaDocumento
                {
                    pes_id = pes_id,
                    tdo_id = new Guid(dr["tipoDocumentoNIS"].ToString()),
                    psd_numero = dr["numeroDocumentoNIS"].ToString()
                };
                listPessoaDoc.Add(entityDocResp);
            }

            return listPessoaDoc;
        }

        /// <summary>
        /// Verifica se o item existia e deixou de existir, exclui caso positivo.
        /// </summary>
        /// <param name="listaInseridos">Lista de itens inseridos</param>
        /// <param name="bancoCore">Transação do Core</param>
        /// <param name="bancoGestao">Transação do Gestão</param>
        /// <param name="cadastrado">Item a ser verificado</param>
        /// <param name="tra_idProprio"></param>
        private static void VerifcaItemCadastrado
        (
            List<StructCadastro> listaInseridos
            , TalkDBTransaction bancoCore
            , TalkDBTransaction bancoGestao
            , StructCadastro cadastrado
            , Int32 tra_idProprio
        )
        {
            if (!listaInseridos.Exists
                (p =>
                 p.entAlunoResp.tra_id == cadastrado.entAlunoResp.tra_id
                ))
            {
                // Se o tipo de responsável não existir mais, excluir.
                ACA_AlunoResponsavel entResp = cadastrado.entAlunoResp;

                PES_Pessoa entPessoa = new PES_Pessoa
                {
                    pes_id = entResp.pes_id
                };
                PES_PessoaBO.GetEntity(entPessoa, bancoCore);

                // Verificar documento.
                if (cadastrado.listPessoaDoc.Count > 0)
                {
                    // Caso tiver documento para o responsável, será decrementado a integridade.
                    foreach (PES_PessoaDocumento documento in cadastrado.listPessoaDoc)
                    {
                        DecrementaIntegridadeDocumentoPessoa(bancoCore, documento.tdo_id, entPessoa.pes_id);
                    }
                }

                // Decrementa integridade da pessoa.
                DecrementaIntegridadePessoa(bancoCore, entPessoa);

                Delete(entResp, bancoGestao);
            }
            else
            {
                StructCadastro inserido = listaInseridos.Find
                   (p =>
                    p.entAlunoResp.tra_id == cadastrado.entAlunoResp.tra_id
                   );

                if ((inserido.listPessoaDoc.Count < cadastrado.listPessoaDoc.Count)
                    && (inserido.entAlunoResp.tra_id != tra_idProprio))
                {
                    // Se existia um documento e foi removido (quando for diferente do Próprio aluno), excluir o documento.
                    ExcluirDocumentoReponsavel(
                                                cadastrado.listPessoaDoc
                                                , inserido.listPessoaDoc
                                                , cadastrado.entPessoa.pes_id
                                                , bancoCore
                                                );
                }

                if (inserido.entPessoa.pes_id != cadastrado.entPessoa.pes_id)
                {
                    // Mudou a pessoa - decrementa integridade da pessoa anterior.
                    PES_Pessoa entPessoa = cadastrado.entPessoa;

                    // Decrementa integridade da pessoa.
                    DecrementaIntegridadePessoa(bancoCore, entPessoa);

                    if ((!inserido.entPessoa.IsNew) && (!inserido.entAlunoResp.IsNew))
                    {
                        // Se a pessoa não for nova, nem o responsável - incrementa
                        // integridade da pessoa inserida.
                        PES_PessoaBO.IncrementaIntegridade(inserido.entPessoa.pes_id, bancoCore);
                    }
                }
            }
        }

        /// <summary>
        /// Decrementa a integridade da pessoa, e exclui ela caso a integridade fique 0.
        /// </summary>
        /// <param name="bancoCore">Transação - obrigatório</param>
        /// <param name="entPessoa">entidade da Pessoa</param>
        private static void DecrementaIntegridadePessoa(TalkDBTransaction bancoCore, PES_Pessoa entPessoa)
        {
            entPessoa.pes_integridade--;
            PES_PessoaBO.DecrementaIntegridade(entPessoa.pes_id, bancoCore);

            if (entPessoa.pes_integridade <= 0)
            {
                // Se integridade for 0 - excluir pessoa.
                PES_PessoaBO.Delete(entPessoa, bancoCore);
                PES_PessoaBO.DecrementaIntegridade(entPessoa.pes_idFiliacaoMae, bancoCore);
                PES_PessoaBO.DecrementaIntegridade(entPessoa.pes_idFiliacaoPai, bancoCore);
            }
        }

        /// <summary>
        /// Decrementa a integridade do DocumentoPessoa no Core para a pessoa.
        /// </summary>
        /// <param name="bancoCore">Transação do Core</param>
        /// <param name="tdo_id">ID do tipo de documentação (NIS,CPF e RG)</param>
        /// <param name="pes_id">pes_id do responsável</param>
        private static void DecrementaIntegridadeDocumentoPessoa
        (
            TalkDBTransaction bancoCore,
            Guid tdo_id,
            Guid pes_id
        )
        {
            PES_PessoaDocumento doc = new PES_PessoaDocumento { pes_id = pes_id, tdo_id = tdo_id };

            // Decrementa integridade do tipo de documento.
            SYS_TipoDocumentacaoBO.DecrementaIntegridade(doc.tdo_id, bancoCore);
        }

        /// <summary>
        /// Excluindo documento do responsável
        /// </summary>
        /// <param name="listaRespDocumento">Busca do banco os documentos anteriores</param>
        /// <param name="listaDocumentoInseridos">Documentos atuais</param>
        /// <param name="pes_id">pes_id do responsável</param>
        /// <param name="bancoCore"></param>
        private static void ExcluirDocumentoReponsavel
        (
            List<PES_PessoaDocumento> listaRespDocumento
            , List<PES_PessoaDocumento> listaDocumentoInseridos
            , Guid pes_id
            , TalkDBTransaction bancoCore
        )
        {
            foreach (PES_PessoaDocumento item in listaRespDocumento)
            {
                if (!listaDocumentoInseridos.Exists
                (p =>
                 p.tdo_id == item.tdo_id
                ))
                {
                    // Se o tipo de documento não existir mais, excluir.
                    PES_PessoaDocumento doc = new PES_PessoaDocumento { pes_id = pes_id, tdo_id = item.tdo_id };
                    PES_PessoaDocumentoBO.Delete(doc, bancoCore);

                    // Decrementa integridade do tipo de documento.
                    SYS_TipoDocumentacaoBO.DecrementaIntegridade(doc.tdo_id, bancoCore);
                }
            }
        }

        /// <summary>
        /// Salva no banco os dados do item passado por parâmetro, retorna o item inserido.
        /// </summary>
        /// <param name="tra_idPrincipal">ID do tipo de responsável</param>
        /// <param name="entAluno">Aluno</param>
        /// <param name="item">Item a ser inserido</param>
        /// <param name="tra_idProprio">Id padrão do tipo de responsável "O próprio"</param>
        /// <param name="bancoCore">Transação do banco do Core</param>
        /// <param name="tra_idMae">Id padrão do tipo de responsável "Mãe"</param>
        /// <param name="tra_idPai">Id padrão do tipo de responsável "Pai"</param>
        /// <param name="bancoGestao">Transação do banco do Gestão</param>
        /// <param name="tdo_idCPF">ID padrão do tipo de documento CPF</param>
        /// <param name="tdo_idRG">ID padrão do tipo de documento RG</param>
        /// <param name="tdo_idNis">ID padrão do tipo de documento NIS</param>
        /// <param name="pes_idMae">ref - id da Mãe do aluno</param>
        /// <param name="pes_idPai">ref - id do Pai do aluno</param>
        /// <param name="salvarMaiusculo">Indica se os nomes devem ser salvos em maiúsculos</param>
        /// <param name="ListValidacoesDoc"></param>
        /// <param name="ListSalvar">Lista de responsaveis que estao sendo salvos, para nao considerar o mesmo documento</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns>Os dados inseridos</returns>
        private static StructCadastro SalvarResponsavel
        (
            Int32 tra_idPrincipal
            , ACA_Aluno entAluno
            , StructCadastro item
            , int tra_idProprio
            , TalkDBTransaction bancoCore
            , int tra_idMae
            , int tra_idPai
            , TalkDBTransaction bancoGestao
            , Guid tdo_idCPF
            , Guid tdo_idRG
            , Guid tdo_idNis
            , ref Guid pes_idMae
            , ref Guid pes_idPai
            , bool salvarMaiusculo
            , ref List<string> ListValidacoesDoc
            , List<StructCadastro> ListSalvar
            , Guid ent_id
        )
        {
            // Salvar os responsáveis.
            ACA_AlunoResponsavel entResp = item.entAlunoResp;
            PES_Pessoa entPessoa = item.entPessoa;
            List<PES_PessoaDocumento> lisDocResp;

            entResp.alu_id = entAluno.alu_id;

            if (entResp.tra_id == tra_idProprio)
            {
                // Se for o próprio, seta o id da Pessoa = o Id da pessoa do aluno.
                entResp.pes_id = entAluno.pes_id;

                // Salvar a integridade da pessoa.
                if (entResp.IsNew)
                {
                    PES_PessoaBO.IncrementaIntegridade(entAluno.pes_id, bancoCore);
                }
            }
            else
            {
                if ((salvarMaiusculo) && (!String.IsNullOrEmpty(entPessoa.pes_nome)))
                {
                    entPessoa.pes_nome = entPessoa.pes_nome.ToUpper();
                }

                // Só salva a pessoa se o nome estiver preenchido.
                if (!String.IsNullOrEmpty(entPessoa.pes_nome))
                {                    
                    // Faz a verificação caso exista a pessoa com o CPF passado por parâmetro.
                    Guid responsavel = Valida_Responsavel_CPF(item.listPessoaDoc, entResp.tra_id, ref ListValidacoesDoc, ent_id, entPessoa.pes_id, ListSalvar);
                    if (responsavel == new Guid())
                    {
                        // Se não existir, salva a entidade pessoa.
                        PES_PessoaBO.Save(entPessoa, bancoCore);
                    }
                    else
                    {
                        entPessoa.pes_id = responsavel;
                        entPessoa.IsNew = false;

                        #region Atualiza - Entidade Pessoa (Cria uma entidade auxiliar para atualizar apenas o nome)

                        //Atualiza apenas o nome do responsável, recebendo todas as informações ja cadastradas
                        PES_Pessoa pesAux = new PES_Pessoa
                        {
                            pes_id = entPessoa.pes_id
                        };
                        PES_PessoaBO.GetEntity(pesAux);

                        pesAux.pes_nome = entPessoa.pes_nome;
                        //// movimentado os valores por que esses campos não estavam sendo gravados
                        pesAux.pes_dataNascimento = entPessoa.pes_dataNascimento;
                        pesAux.pes_estadoCivil = entPessoa.pes_estadoCivil;
                        pesAux.tes_id = entPessoa.tes_id;
                        ////
                        entPessoa = pesAux;

                        #endregion Atualiza - Entidade Pessoa (Cria uma entidade auxiliar para atualizar apenas o nome)

                        // Se for matricula
                        if (entAluno.IsNew)
                        {
                            #region Atualiza - Entidade Aluno Responsavel (Caso existir a informações referente a profissão e situação)

                            //Faz verificação se existe as informações referente a profissão e situação do responsável
                            byte situacao_responsavel;
                            string profissao_responsavel;

                            RetornaAlunoResponsavel_Situacao_Profissao(entPessoa.pes_id, out situacao_responsavel, out profissao_responsavel);

                            if (situacao_responsavel == Convert.ToByte(ACA_AlunoResponsavelSituacao.Falecido))
                                entResp.alr_situacao = Convert.ToByte(ACA_AlunoResponsavelSituacao.Falecido);
                            else
                                entResp.alr_situacao = Convert.ToByte(ACA_AlunoResponsavelSituacao.Ativo);

                            entResp.alr_profissao = profissao_responsavel;

                            #endregion Atualiza - Entidade Aluno Responsavel (Caso existir a informações referente a profissão e situação)
                        }
                        else
                        {
                            // Caso já exista o responsável, será atualizado os campos: situção (Quando for falecido = 4) e o campo profissão.
                            AtualizaResponsavel_Profissao_Situacao(entPessoa.pes_id, entResp.alr_profissao, entResp.alr_empresa, entResp.alr_situacao, bancoGestao);
                        }

                        PES_PessoaBO.Save(entPessoa, bancoCore);
                    }
                }
                else
                    entPessoa.pes_id = Guid.Empty;

                if ((entPessoa.IsNew) || (entResp.IsNew))
                {
                    PES_PessoaBO.IncrementaIntegridade(entPessoa.pes_id, bancoCore);
                }

                entResp.pes_id = entPessoa.pes_id;

                // Seta o Id da mãe e do Pai para alterar na pessoa do aluno.
                if (entResp.tra_id == tra_idMae)
                {
                    pes_idMae = entPessoa.pes_id;
                }
                else if (entResp.tra_id == tra_idPai)
                {
                    pes_idPai = entPessoa.pes_id;
                }
            }

            // Seta responsável principal.
            entResp.alr_principal = (entResp.tra_id == tra_idPrincipal);

            Save(entResp, bancoGestao);

            if (entResp.tra_id != tra_idProprio)
            {
                // Salva os documentos dos responsáveis
                lisDocResp = SalvaDocumentoResponsavel(
                                    item.listPessoaDoc
                                    , entResp.pes_id
                                    , tra_idProprio
                                    , entResp.tra_id
                                    , tdo_idCPF
                                    , tdo_idRG
                                    , tdo_idNis
                                    , bancoCore
                                    , ref ListValidacoesDoc);
            }
            else
            {
                lisDocResp = new List<PES_PessoaDocumento>();
            }

            // Retorna item inserido.
            return (new StructCadastro
            {
                entAlunoResp = entResp,
                entPessoa = entPessoa,
                listPessoaDoc = lisDocResp
            });
        }

        /// <summary>
        /// Valida o CPF do responsável
        /// 1º verifica se o CPF é válido.
        /// 2º verifica se já existe uma pessoa cadastra com mesmo número de CPF.
        /// </summary>
        /// <param name="responsavelCPF">numero do documento - CPF</param>
        /// <param name="tra_id"></param>
        /// <param name="ListValidacoesDoc">Lista das validações dos reponsáveis</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <param name="ListSalvar">Lista de responsaveis que estao sendo salvos, para nao considerar o mesmo documento</param>
        /// <returns>
        /// Retorna o pes_id novo caso não tenha uma pessoa cadastrada com número do CPF.
        /// </returns>
        private static Guid Valida_Responsavel_CPF(List<PES_PessoaDocumento> responsavelCPF, Int32 tra_id, ref List<string> ListValidacoesDoc, Guid ent_id, Guid pes_id, List<StructCadastro> ListSalvar)
        {
            // Faz uma busca do id do tipo de documento (tdo_id).
            string docPadraoCPF = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF);
            Guid PadraoCPF = new Guid(docPadraoCPF);

            foreach (PES_PessoaDocumento item in responsavelCPF)
            {
                if (item.tdo_id == PadraoCPF)
                {
                    // Faz a validação do número do Cpf
                    if (!UtilBO._ValidaCPF(item.psd_numero))
                    {
                        string msg;
                        Int32 tra_idMae = TipoResponsavelAlunoParametro.tra_idMae(ent_id);
                        Int32 tra_idPai = TipoResponsavelAlunoParametro.tra_idPai(ent_id);

                        if (!(tra_id == tra_idMae) && !(tra_id == tra_idPai))
                        {
                            msg = "O responsável ";
                        }
                        else
                        {
                            msg = (tra_id == tra_idMae) ? "A mãe " : "O pai ";
                        }

                        ListValidacoesDoc.Add(msg + "possui o número do CPF inválido.");
                    }

                    // Verifica se existe uma pessoa cadastrada com o número de CPF informado.
                    PES_PessoaDocumento pessoaResponsavel = PES_PessoaDocumentoBO.GetEntityBy_Documento(item.psd_numero, PadraoCPF);

                    // Se encontrou uma pessoa diferente com o mesmo numero de CPF
                    // e nao eh nenhuma das outras pessoas que tambem estao sendo salvas...
                    if (pessoaResponsavel.pes_id != Guid.Empty
                        && pessoaResponsavel.pes_id != pes_id
                        && !ListSalvar.Any(p => p.entPessoa.pes_id == pessoaResponsavel.pes_id))
                    {
                        // Retorna a pessoa cadastrada com o mesmo numero de CPF
                        return pessoaResponsavel.pes_id;
                    }
                    else
                    {
                        // Retorno a propria pessoa
                        return pes_id;
                    }
                }
            }
            return pes_id;
        }

        /// <summary>
        /// Retorna ID da entidade de pessoa do responsável por CPF.
        /// </summary>
        /// <param name="cpf">Número de CPF.</param>
        /// <returns></returns>
        public static Guid RetornaIdPessoaResponsavelPorCPF(string cpf)
        {
            // Faz uma busca do id do tipo de documento (tdo_id).
            string docPadraoCPF = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF);
            Guid PadraoCPF = new Guid(docPadraoCPF);

            // Verifica se existe uma pessoa cadastro com o número de CPF informado.
            PES_PessoaDocumento pessoaResponsavel = PES_PessoaDocumentoBO.GetEntityBy_Documento(cpf, PadraoCPF);
            return pessoaResponsavel.pes_id;
        }

        /// <summary>
        /// Salva os tipos de documento do responsável
        /// </summary>
        /// <param name="pesRespDoc">entidade Pes_PessoaDocumento</param>
        /// <param name="pes_id"></param>        
        /// <param name="bancoCore"></param>
        private static void SalvaDocumento(PES_PessoaDocumento pesRespDoc, Guid pes_id, TalkDBTransaction bancoCore)
        {
            // Verifica se já existe esse documento para essa pessoa.
            PES_PessoaDocumento docAux = new PES_PessoaDocumento
            {
                pes_id = pes_id,
                tdo_id = pesRespDoc.tdo_id
            };
            PES_PessoaDocumentoBO.GetEntity(docAux, bancoCore);

            // Salvar documento.
            PES_PessoaDocumento doc = new PES_PessoaDocumento
            {
                pes_id = pes_id,
                tdo_id = pesRespDoc.tdo_id,
                psd_numero = pesRespDoc.psd_numero,
                IsNew = docAux.IsNew,
                psd_infoComplementares = docAux.psd_infoComplementares,
                psd_orgaoEmissao = docAux.psd_orgaoEmissao,
                psd_dataEmissao = docAux.psd_dataEmissao,
                unf_idEmissao = docAux.unf_idEmissao,
                psd_situacao = 1
            };

            PES_PessoaDocumentoBO.Save(doc, bancoCore);

            if (doc.IsNew)
            {
                // Incrementar a integridade do tipo de documento.
                SYS_TipoDocumentacaoBO.IncrementaIntegridade(doc.tdo_id, bancoCore);
            }
        }

        /// <summary>
        /// Salva os documentos do responsável,
        /// </summary>
        /// <param name="responsavelDocumento">List de documentos</param>
        /// <param name="pes_id">id do responsável</param>
        /// <param name="tra_idProprio">id do tipo de responsável "próprio" - definido no parâmetro.</param>
        /// <param name="tra_idResponsavel">id do do tipo de responsável principal - (definido na tela)</param>
        /// <param name="tdo_idCPF">id do tipo de documento "CPF" - definido no parâmetro.</param>
        /// <param name="tdo_idRG">id do tipo de documento "RG" - definido no parâmetro.</param>
        /// <param name="tdo_idNis">id do tipo de documento "NIS" - definido no parâmetro.</param>
        /// <param name="bancoCore"></param>
        /// <param name="ListValidacoesDoc"></param>
        /// <returns>list de documentos</returns>
        private static List<PES_PessoaDocumento> SalvaDocumentoResponsavel
        (
            List<PES_PessoaDocumento> responsavelDocumento
            , Guid pes_id
            , int tra_idProprio
            , int tra_idResponsavel
            , Guid tdo_idCPF
            , Guid tdo_idRG
            , Guid tdo_idNis
            , TalkDBTransaction bancoCore
            , ref List<string> ListValidacoesDoc
        )
        {
            List<PES_PessoaDocumento> list = new List<PES_PessoaDocumento>();

            foreach (PES_PessoaDocumento item in responsavelDocumento)
            {
                // Se for "do tipo de documento CPF".
                if (item.tdo_id == tdo_idCPF)
                {
                    SalvaDocumento(item, pes_id, bancoCore);
                }
                else
                {
                    // Se for "do tipo de documento RG".
                    if (item.tdo_id == tdo_idRG)
                    {
                        SalvaDocumento(item, pes_id, bancoCore);
                    }
                    else
                    { // Se for "do tipo de documento NIS".
                        if (item.tdo_id == tdo_idNis)
                        {
                            // Salvar o NIS - só quando não for o Próprio - pois o documento é inserido no UCDocumentos na tela.
                            if ((!String.IsNullOrEmpty(item.psd_numero)) &&
                                (tra_idResponsavel != tra_idProprio))
                            {
                                // Se o parâmetro não estiver definido, dispara uma excessão, porque
                                // não será possível salvar o NIS, mas o usuário não pode ver o motivo
                                // do problema.
                                if (tdo_idNis == Guid.Empty)
                                {
                                    ListValidacoesDoc.Add("O parâmetro do tipo de documento NIS não está definido nos parâmetros acadêmicos.");
                                }

                                SalvaDocumento(item, pes_id, bancoCore);
                            }
                        }
                    }
                }

                // Adiciona o item (entidade Pes_PessoaDocumento) salvo na list
                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// Retorna todos os alunos que o responsável possui.
        /// </summary>
        /// <param name="pes_id">ID do responsável do aluno.</param>
        /// <returns></returns>
        public static DataTable SelecionaAlunosPorResponsavel(Guid pes_id)
        {
            ACA_AlunoResponsavelDAO dao = new ACA_AlunoResponsavelDAO();
            return dao.SelecionaAlunosPorResponsavel(pes_id);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os responsaveis do sistema, de acordo com os filtros.
        /// </summary>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="nome">nome da pessoa</param>
        /// <param name="cpf">cpf da pessoa</param>
        /// <param name="rg">rg da pessoa</param>
        /// <param name="nis"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscarResponsaveis
        (
            Guid ent_id
            , string nome
            , string cpf
            , string rg
            , string nis
            , int currentPage
            , int pageSize
        )
        {
            ACA_AlunoResponsavelDAO dao = new ACA_AlunoResponsavelDAO();

            // ID do tipo de documento CPF.
            string docPadraoCPF = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF);
            Guid tdo_idCPF = String.IsNullOrEmpty(docPadraoCPF) ? Guid.Empty : new Guid(docPadraoCPF);

            // ID do tipo de documento RG.
            string docPadraoRG = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG);
            Guid tdo_idRG = String.IsNullOrEmpty(docPadraoRG) ? Guid.Empty : new Guid(docPadraoRG);

            // ID do tipo de documento NIS.
            Guid tdo_idNis = ACA_ParametroAcademicoBO.ParametroValorGuidPorEntidade(eChaveAcademico.TIPO_DOCUMENTACAO_NIS, ent_id);

            return dao.SelectBuscaResponsaveis(ent_id, nome, cpf, rg, nis, tdo_idCPF, tdo_idRG, tdo_idNis, currentPage, pageSize, out totalRecords);
        }
    }
}