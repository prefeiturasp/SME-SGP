using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MSTech.Business.Common;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Linq;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Situações da matrícula do aluno
    /// </summary>
    public enum ACA_AlunoCurriculoSituacao : byte
    {
        Ativo = 1
        ,

        Excluido = 3
        ,

        Inativo = 4
        ,

        Formado = 5
        ,

        Cancelado = 6
        ,

        EmMatricula = 7
        ,

        Excedente = 8
        ,

        Evadido = 9
        ,

        EmMovimentacao = 10
    }

    /// <summary>
    /// Tipo de ingresso da matrícula do aluno
    /// </summary>
    public enum ACA_AlunoCurriculoTipoIngresso : byte
    {
        MatriculaInicial = 1
        ,

        Transferido = 2
    }

    #endregion Enumeradores

    #region Excessões

    /// <summary>
    /// Classe de excessão referente à entidade ACA_AlunoCurriculo.
    /// Utilizada nas telas de cadastro, para identificar se houve erro de validação
    /// na entidade do ACA_AlunoCurriculo.
    /// </summary>
    public class ACA_AlunoCurriculo_ValidationException : ValidationException
    {
        public ACA_AlunoCurriculo_ValidationException(string message) : base(message) { }
    }

    #endregion Excessões

    public class ACA_AlunoCurriculoBO : BusinessBase<ACA_AlunoCurriculoDAO, ACA_AlunoCurriculo>
    {
        #region Consultas

        /// <summary>
        /// Retorna os currículos ativos e em matricula do aluno.
        /// </summary>
        /// <param name="alu_id">Id do aluno do bd</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDadosUltimaMatriculaAtiva
        (
            long alu_id
        )
        {
            ACA_AlunoCurriculoDAO dao = new ACA_AlunoCurriculoDAO();
            return dao.SelecionaDadosUltimaMatriculaAtiva(alu_id);
        }

        /// <summary>
        /// Retorna a última matrícula ativa dos alunos informados.
        /// </summary>
        /// <param name="alu_id">IDs dos alunos</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        public static List<ACA_AlunoCurriculo> SelecionaUltimaMatriculaAtiva_Alunos
        (
            string alu_id
            , TalkDBTransaction banco
        )
        {
            ACA_AlunoCurriculoDAO dao = new ACA_AlunoCurriculoDAO { _Banco = banco };
            DataTable dt = dao.SelecionaUltimaMatriculaAtiva_Alunos(alu_id);

            return (from DataRow dr in dt.Rows
                    select dao.DataRowToEntity(dr, new ACA_AlunoCurriculo())).ToList();
        }

        /// <summary>
        /// Retorna o último currículo do aluno (Ativo ou inativo).
        /// </summary>
        /// <param name="alu_id">Id do aluno do bd</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDadosUltimaMatricula
        (
            long alu_id
        )
        {
            ACA_AlunoCurriculoDAO dao = new ACA_AlunoCurriculoDAO();
            return dao.SelecionaDadosUltimaMatricula(alu_id);
        }

        /// <summary>
        /// Retorna o último curriculo do aluno.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="entity">Entidade ACA_AlunoCurriculo de retorno</param>
        /// <returns></returns>
        public static bool CarregaUltimoCurriculo(long alu_id, out ACA_AlunoCurriculo entity, TalkDBTransaction banco = null)
        {
            ACA_AlunoCurriculoDAO dao = new ACA_AlunoCurriculoDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.SelectBy_UltimoCurriculo(alu_id, out entity);
        }

        /// <summary>
        /// Retorna o último curriculo INATIVO do aluno.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="entity">Entidade ACA_AlunoCurriculo de retorno</param>
        /// <returns></returns>
        public static bool CarregaUltimoCurriculoInativo(long alu_id, out ACA_AlunoCurriculo entity)
        {
            ACA_AlunoCurriculoDAO dao = new ACA_AlunoCurriculoDAO();
            return dao.SelectBy_UltimoCurriculoInativo(alu_id, out entity);
        }

        #endregion Consultas

        #region Saves

        /// <summary>
        /// Salva a entidade AlunoCurriculo no banco, realizando as validações necessárias.
        /// Verifica também a situação da entidade, e seta a data de saída, se o registro foi
        /// inativado.
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
           ACA_AlunoCurriculo entity
            , TalkDBTransaction banco
            , Guid ent_id
        )
        {
            if (entity.Validate())
            {
                // Valida dados necessários para salvar a entidade.
                ValidaDados(entity, banco, ent_id);

                if (!entity.IsNew)
                {
                    // Se for alteração, e estiver alterando para Inativo, setar a data de saída.
                    ACA_AlunoCurriculo entAux = new ACA_AlunoCurriculo
                                                    {
                                                        alu_id = entity.alu_id
                                                        ,
                                                        alc_id = entity.alc_id
                                                    };
                    GetEntity(entAux, banco);

                    if ((!entAux.IsNew) &&
                        (entity.alc_dataSaida == new DateTime()) &&
                        (entity.alc_situacao == (byte)ACA_AlunoCurriculoSituacao.Inativo) &&
                        (entity.alc_situacao != entAux.alc_situacao))
                    {
                        // Se a situação não era inativo e passou a ser.
                        entity.alc_dataSaida = DateTime.Now;
                    }
                }
                else
                {
                    if (entity.alc_dataPrimeiraMatricula == new DateTime())
                    {
                        // Se a entidade for nova e o campo estiver nulo, setar a data da primeira matrícula.
                        entity.alc_dataPrimeiraMatricula = DateTime.Now;
                    }
                }

                if (ValidarIdadeIdeal(entity, banco, null, true, 0, ent_id))
                {
                    ACA_AlunoCurriculoDAO alucurDAO = new ACA_AlunoCurriculoDAO { _Banco = banco };
                    return alucurDAO.Salvar(entity);
                }
            }

            throw new ACA_AlunoCurriculo_ValidationException(UtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// Override do método salvar, que faz a verificação da situação para setar a data de saída.
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <returns></returns>
        public new static bool Save(ACA_AlunoCurriculo entity)
        {
            // Se for alteração, e estiver alterando para Inativo, setar a data de saída.
            ACA_AlunoCurriculo entAux = new ACA_AlunoCurriculo
            {
                alu_id = entity.alu_id
                ,
                alc_id = entity.alc_id
            };
            GetEntity(entAux);

            if ((!entAux.IsNew) &&
                (entity.alc_dataSaida == new DateTime()) &&
                (entity.alc_situacao == (byte)ACA_AlunoCurriculoSituacao.Inativo) &&
                (entity.alc_situacao != entAux.alc_situacao))
            {
                // Se a situação não era inativo e passou a ser.
                entity.alc_dataSaida = DateTime.Now;
            }

            ACA_AlunoCurriculoDAO dao = new ACA_AlunoCurriculoDAO();
            return dao.Salvar(entity);
        }

        /// <summary>
        /// Altera somente os dados referentes à matrícula do aluno.
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <param name="banco">Transação com banco do Gestão - obrigatório</param>
        /// <returns></returns>
        internal static bool SaveDadosMatricula
        (
            ACA_AlunoCurriculo entity
            , TalkDBTransaction banco
        )
        {
            ACA_AlunoCurriculoDAO dao = new ACA_AlunoCurriculoDAO { _Banco = banco };
            return dao.AlterarDadosMatricula(entity);
        }

        /// <summary>
        /// Atualiza a situação do curriculo do aluno de acordo com o fechamento de matrícula
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool Atualiza_Situacao_Fechamento_Matricula
        (
            ACA_AlunoCurriculo entity
            , TalkDBTransaction banco
        )
        {
            ACA_AlunoCurriculoDAO dao = new ACA_AlunoCurriculoDAO { _Banco = banco };
            return dao.Update_Situacao_FechamentoMatricula(entity);
        }

        /// <summary>
        /// Atualiza a situação dos dados de matrícula do aluno para inativo.
        /// </summary>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="dataSaida"></param>
        /// <param name="bancoGestao">Transação de banco - obrigatório</param>
        public static void InativaDadosMatricula
        (
            long alu_id
            , DateTime dataSaida
            , TalkDBTransaction bancoGestao
        )
        {
            ACA_AlunoCurriculoDAO dao = new ACA_AlunoCurriculoDAO
            {
                _Banco = bancoGestao
            };

            dao.InativaDadosMatricula(alu_id, dataSaida);
        }


        /// <summary>
        /// Retorna os currículos do alunos (inativo) de acordo mtu_id.
        /// </summary>
        /// <param name="alu_id">Id do aluno do bd</param>
        /// <param name="mtu_id">id da MatriculaTurma do aluno</param>
        /// <param name="tpc_id">id da bimestre/coc</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDadosMatriculaTurma
        (
            long alu_id,
            int mtu_id,
            int tpc_id

        )
        {
            ACA_AlunoCurriculoDAO dao = new ACA_AlunoCurriculoDAO();
            return dao.SelecionaDadosMatriculaTurma(alu_id, mtu_id, tpc_id);
        }
        #endregion Saves

        #region Validações

        /// <summary>
        /// Validar idade do aluno de acordo com o CurriculoPeriodo no qual ele está inserido.
        /// Valida a idade máxima ideal + a amplitude do parâmetro acadêmico ou o valor informado
        /// no parâmetro amplitude.
        /// Valida a idade mínima ideal quando informado através da flag validarIdadeMinima - quando
        /// informada sempre valida usando a amplitudo passada por parâmetro na função.
        /// </summary>
        /// <param name="entity">Entidade AlunoCurriculo carregada</param>
        /// <param name="bancoGestao">Transação com banco Gestão - obrigatório</param>
        /// <param name="bancoCore">Transação com banco CoreSSO - opcional</param>
        /// <param name="validarIdadeMinima">Flag que indica se é pra validar idade mínima também</param>
        /// <param name="amplitude">Amplitude, se > 0, substitui a amplitude do parâmetro acadêmico</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        /// <returns>Se idade válida</returns>
        internal static bool ValidarIdadeIdeal
        (
            ACA_AlunoCurriculo entity
            , TalkDBTransaction bancoGestao
            , TalkDBTransaction bancoCore
            , bool validarIdadeMinima
            , int amplitude
            , Guid ent_id
        )
        {
            ACA_Aluno entAluno = new ACA_Aluno { alu_id = entity.alu_id };
            ACA_AlunoBO.GetEntity(entAluno, bancoGestao);

            if (entAluno.alu_situacao != Convert.ToByte(ACA_AlunoSituacao.Inativo))
            {
                if (entity.alc_situacao == Convert.ToByte(ACA_AlunoCurriculoSituacao.Ativo)
                    || entity.alc_situacao == Convert.ToByte(ACA_AlunoCurriculoSituacao.EmMatricula))
                {
                    ACA_CurriculoPeriodo entCurPer = new ACA_CurriculoPeriodo { cur_id = entity.cur_id, crr_id = entity.crr_id, crp_id = entity.crp_id };
                    ACA_CurriculoPeriodoBO.GetEntity(entCurPer, bancoGestao);

                    PES_Pessoa entPessoa = new PES_Pessoa { pes_id = entAluno.pes_id };
                    if (bancoCore == null)
                        PES_PessoaBO.GetEntity(entPessoa);
                    else
                        PES_PessoaBO.GetEntity(entPessoa, bancoCore);

                    // Quantidade de meses da idade máxima da criança cadastrada no CurrPeriodo.
                    int idadeMaxima = (entCurPer.crp_idadeIdealAnoFim * 12) + entCurPer.crp_idadeIdealMesFim;

                    if (amplitude > 0)
                    {
                        idadeMaxima += amplitude;
                    }
                    else
                    {
                        int pac_valor = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.AMPLITUDE_IDADE_ALERTA, ent_id);

                        // Soma quantidade em meses do parâmetro de amplitude.
                        idadeMaxima += (pac_valor > 0 ? (pac_valor * 12) : 0);
                    }

                    // Valida a quantidade de meses da idade da criança.
                    int anos, meses, dias;
                    GestaoEscolarUtilBO.CalculaAniversarioCompleto(DateTime.Now, entPessoa.pes_dataNascimento, out anos, out meses, out dias);

                    int idade = (anos * 12) + meses;

                    if (idade > idadeMaxima)
                    {
                        anos = idadeMaxima / 12;
                        meses = idadeMaxima % 12;

                        string sAnos = anos > 0 ? (anos + (anos > 1 ? " anos" : " ano")) : string.Empty;
                        string sMeses = meses > 0 ? (anos > 0 ? " e " : "") + meses + (meses > 1 ? " meses" : " mês") : string.Empty;
                        throw new ACA_Aluno_ValidationException("A idade do aluno não pode ser maior que " + sAnos + sMeses + ".");
                    }

                    ACA_Curriculo curso = ACA_CurriculoBO.GetEntity(new ACA_Curriculo { cur_id = entity.cur_id, crr_id = entity.crr_id });

                    if (validarIdadeMinima
                            && curso.crr_regimeMatricula == 3
                            && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.VALIDAR_IDADE_MINIMA_PEJA, ent_id))
                    {
                        // Quantidade de meses da idade máxima da criança cadastrada no CurrPeriodo.
                        int idadeMinima = (entCurPer.crp_idadeIdealAnoInicio * 12) + entCurPer.crp_idadeIdealMesInicio;

                        idadeMinima -= amplitude;

                        if (idade < idadeMinima)
                        {
                            anos = idadeMinima / 12;
                            meses = idadeMinima % 12;

                            string sAnos = anos > 0 ? (anos + (anos > 1 ? " anos" : " ano")) : string.Empty;
                            string sMeses = meses > 0 ? (anos > 0 ? " e " : "") + meses + (meses > 1 ? " meses" : " mês") : string.Empty;
                            throw new ACA_Aluno_ValidationException("A idade do aluno não pode ser menor que " + sAnos + sMeses + ".");
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Valida dados necessários para salvar a entidade. Dispara um ValidationException caso não
        /// esteja válida.
        /// </summary>
        /// <param name="entity">Entidade a validar</param>
        /// <param name="banco">Transação com banco do Gestão - obrigatório</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        private static void ValidaDados(ACA_AlunoCurriculo entity, TalkDBTransaction banco, Guid ent_id)
        {
            //Validação feita no BO devido a alteração de nome dos campos para os diferentes clientes
            if (entity.cur_id <= 0)
                throw new ACA_AlunoCurriculo_ValidationException(GestaoEscolarUtilBO.nomePadraoCurso(ent_id) + " é obrigatório.");

            if (entity.crr_id <= 0)
                throw new ACA_AlunoCurriculo_ValidationException(GestaoEscolarUtilBO.nomePadraoCurso(ent_id) + " é obrigatório.");

            if (entity.crp_id <= 0)
                throw new ACA_AlunoCurriculo_ValidationException(GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id) + " é obrigatório.");

            if (!string.IsNullOrEmpty(entity.alc_matriculaEstadual) && entity.alc_matriculaEstadual.Length > 50)
                throw new ACA_AlunoCurriculo_ValidationException(GestaoEscolarUtilBO.nomePadraoMatriculaEstadual(ent_id) + " pode conter até 50 caracteres.");

            //Se for uma nova matrícula e se a situação dela for 'Ativa' ou 'Em matrícula'
            //Verifica se já existe uma matrícula cadastrada com os mesmos dados
            if (entity.IsNew &&
                (entity.alc_situacao == Convert.ToByte(ACA_AlunoCurriculoSituacao.Ativo) ||
                 entity.alc_situacao == Convert.ToByte(ACA_AlunoCurriculoSituacao.EmMatricula)))
            {
                if (Existe_AlunoCurriculo(entity.alu_id, entity.cur_id, entity.crr_id, entity.crp_id, banco))
                {
                    ESC_Escola esc = new ESC_Escola { esc_id = entity.esc_id };
                    ESC_EscolaBO.GetEntity(esc, banco);

                    ACA_Curso cur = new ACA_Curso { cur_id = entity.cur_id };
                    ACA_CursoBO.GetEntity(cur, banco);

                    ACA_CurriculoPeriodo crp = new ACA_CurriculoPeriodo { cur_id = entity.cur_id, crr_id = entity.crr_id, crp_id = entity.crp_id };
                    ACA_CurriculoPeriodoBO.GetEntity(crp, banco);

                    throw new ACA_AlunoCurriculo_ValidationException("Já existe uma matrícula 'Ativa' ou 'Em matrícula' cadastrada com os mesmos dados: <BR/>" +
                                                                     "Escola: " + esc.esc_nome + "<BR/>" +
                                                                     GestaoEscolarUtilBO.nomePadraoCurso(ent_id) + ": " + cur.cur_nome + "<BR/>" +
                                                                     GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id) + ": " + crp.crp_descricao);
                }
            }
        }

        /// <summary>
        /// Verifica se o aluno curriculo está sendo utilizado na turma
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="alc_id"></param>
        /// <returns>True/False</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool Existe_MatriculaTurma
        (
            long alu_id
            , int alc_id
        )
        {
            ACA_AlunoCurriculoDAO dao = new ACA_AlunoCurriculoDAO();
            return dao.SelectBy_VerificaMatriculaTurma(alu_id, alc_id);
        }

        /// <summary>
        /// Verifica se já existe um AlunoCurriculo cadastrado com os mesmos dados
        /// (escola, curso, currículo e período)
        /// e com a situação "Ativo" ou "Em matrícula"
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        /// <param name="banco"></param>
        /// <returns>True/False</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool Existe_AlunoCurriculo
        (
            long alu_id
            , int cur_id
            , int crr_id
            , int crp_id
            , TalkDBTransaction banco
        )
        {
            ACA_AlunoCurriculoDAO dao = new ACA_AlunoCurriculoDAO();
            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            return dao.SelectBy_VerificaAlunoCurriculo(alu_id, cur_id, crr_id, crp_id);
        }

        /// <summary>
        /// Retorna um booleano informado se o aluno tem alguma matrícula
        /// na(s) escola(s) que o usuário informado tem permissão.
        /// </summary>
        public static bool AlunoPossuiMatricula_PorPermissaoUsuario
        (
            long alu_id
            , Guid usu_id
            , Guid gru_id
            , TalkDBTransaction banco
        )
        {
            ACA_AlunoCurriculoDAO dao = new ACA_AlunoCurriculoDAO { _Banco = banco };
            return dao.SelectBy_Aluno_PermissaoUsuario(alu_id, usu_id, gru_id).Rows.Count > 0;
        }

        /// <summary>
        /// Retorna os currículos ativos/inativos e em matricula do aluno
        /// usado no UCBoletimCompletoAluno
        /// </summary>
        /// <param name="alu_id">Id do aluno do bd</param>
        /// <param name="tpc_id">Id do bimestre/coc</param>        
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDadosUltimaMatriculaPeriodoAvaliacao
        (
            long alu_id,
            int tpc_id
        )
        {
            ACA_AlunoCurriculoDAO dao = new ACA_AlunoCurriculoDAO();
            return dao.SelecionaDadosUltimaMatriculaAtivaPeriodoAvaliacao(alu_id, tpc_id);
        }

        #endregion Validações
    }
}