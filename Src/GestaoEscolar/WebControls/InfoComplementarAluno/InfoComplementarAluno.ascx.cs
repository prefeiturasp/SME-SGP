using System;
using System.Data;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class InfoComplementarAluno : MotherUserControl
{
    #region Propriedades

    public int Esc_id
    {
        get
        {
            if (ViewState["Esc_id"] != null)
                return Convert.ToInt32(ViewState["Esc_id"]);
            return -1;
        }
        set
        {
            ViewState["Esc_id"] = value;
        }
    }

    public bool HistoricoEscolar
    {
        get
        {
            if (ViewState["HistoricoEscolar"] != null)
                return Convert.ToBoolean(ViewState["HistoricoEscolar"]);
            return false;
        }
        set
        {
            ViewState["HistoricoEscolar"] = value;
        }
    }
    #endregion Propriedades

    #region Métodos

    /// <summary>
    /// Metodo atribui a label informações sobre aluno(nome, escola, curso, ano, matricula, turma, avaliação, nºchamada).
    /// </summary>
    /// <param name="alu_id">ID do aluno</param>
    /// <param name="dtCurriculo">Último currículo do aluno (parâmetro opcional)</param>
    public void InformacaoComplementarAluno(long alu_id, DataTable dtCurriculo = null, bool documentoOficial = false)
    {
        try
        {
            if (alu_id > 0)
            {
                ACA_Aluno entityAluno = new ACA_Aluno();
                PES_Pessoa entityPessoa = new PES_Pessoa();

                // Carrega entidade ACA_Aluno
                entityAluno.alu_id = alu_id;
                ACA_AlunoBO.GetEntity(entityAluno);

                // Carrega entidade PES_Pessoa
                entityPessoa.pes_id = entityAluno.pes_id;
                PES_PessoaBO.GetEntity(entityPessoa);

                eExibicaoNomePessoa exibicaoNome = documentoOficial ? eExibicaoNomePessoa.NomeSocial | eExibicaoNomePessoa.NomeRegistro : eExibicaoNomePessoa.NomeSocial;

                string nomeAluno = entityPessoa.NomeFormatado(exibicaoNome);
                string turno = string.Empty;
                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                bool paramOrdenar = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

                //Nome
                lblInformacaoAluno.Text = "<b>Nome do aluno: </b>" + nomeAluno + "<br/>";

                //Idade
                if (entityPessoa.pes_dataNascimento != new DateTime() && entityPessoa.pes_dataNascimento < DateTime.Today)
                {
                    string dataExtenso = GestaoEscolarUtilBO.DiferencaDataExtenso(entityPessoa.pes_dataNascimento, DateTime.Today);
                    if (!string.IsNullOrEmpty(dataExtenso))
                        lblInformacaoAluno.Text += "<b>Idade: </b>" + dataExtenso + "<br/>";
                }

                // Caso estiver sendo chamada da tela de cadastro de aluno, o datatable com a ultima matricula já virá preenchido
                if (dtCurriculo == null)
                {
                    dtCurriculo = ACA_AlunoCurriculoBO.SelecionaDadosUltimaMatricula(alu_id);
                }

                if (dtCurriculo.Rows.Count > 0)
                {
                    #region Carrega os dados

                    Esc_id = (string.IsNullOrEmpty(dtCurriculo.Rows[0]["esc_id"].ToString())) ? -1 : Convert.ToInt32(dtCurriculo.Rows[0]["esc_id"]);
                    string nomeEscola = dtCurriculo.Rows[0]["esc_nome"].ToString();
                    string codigoEscola = dtCurriculo.Rows[0]["esc_codigo"].ToString();
                    string cursoNome = dtCurriculo.Rows[0]["cur_nome"].ToString();
                    string descricaoPeriodo = dtCurriculo.Rows[0]["crp_descricao"].ToString();
                    string matriculaEstadual = dtCurriculo.Rows[0]["alc_matriculaEstadual"].ToString();
                    string numeroMatricula = dtCurriculo.Rows[0]["alc_matricula"].ToString();
                    string turmaCodigo = dtCurriculo.Rows[0]["tur_codigo"].ToString();
                    string mtu_numeroChamada = dtCurriculo.Rows[0]["mtu_numeroChamada"].ToString();
                    string nomeAvaliacao = dtCurriculo.Rows[0]["crp_nomeAvaliacao"].ToString();
                    string numeroAvaliacao = dtCurriculo.Rows[0]["tca_numeroAvaliacao"].ToString();
                    string cal_ano = dtCurriculo.Rows[0]["cal_ano"].ToString();
                    turno = dtCurriculo.Rows[0]["ttn_nome"].ToString();

                    #endregion Carrega os dados

                    //Escola

                    lblInformacaoAluno.Text += "<b>Escola: </b>";
                    lblInformacaoAluno.Text += (paramOrdenar ? codigoEscola + " - " : "") + nomeEscola + "<br/>";

                    if (!HistoricoEscolar)
                    {
                    //Curso
                    lblInformacaoAluno.Text += " <b>" + GestaoEscolarUtilBO.nomePadraoCurso(ent_id) + ": </b>" + cursoNome;


                    //Periodo
                    lblInformacaoAluno.Text += "&nbsp;&nbsp;&nbsp;<b>" + GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id) + ": </b>" + descricaoPeriodo + "<br/>";

                    //Matricula
                    if (!string.IsNullOrEmpty(ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, ent_id)))
                    {
                        if (!string.IsNullOrEmpty(matriculaEstadual))
                            lblInformacaoAluno.Text += " <b>" + GestaoEscolarUtilBO.nomePadraoMatriculaEstadual(ent_id) + ": " + "</b>" + matriculaEstadual + "&nbsp;&nbsp;&nbsp;";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(numeroMatricula))
                                lblInformacaoAluno.Text += "<b> " + GetGlobalResourceObject("Mensagens", "MSG_NUMEROMATRICULA") + ": " + "</b>" + numeroMatricula + "&nbsp;&nbsp;&nbsp;";
                    }

                    //Turma
                    lblInformacaoAluno.Text += "<b>Turma: </b>" + turmaCodigo;

                    if (!string.IsNullOrEmpty(turno))
                    {
                        //Turno
                        lblInformacaoAluno.Text += "&nbsp;&nbsp;&nbsp;<b>Turno: </b>" + turno;
                    }

                    //Avaliação
                    if (!string.IsNullOrEmpty(nomeAvaliacao) && !string.IsNullOrEmpty(numeroAvaliacao))
                        lblInformacaoAluno.Text += "&nbsp;&nbsp;&nbsp;<b>" + nomeAvaliacao + ": </b>" + numeroAvaliacao;
                    }
                    else
                    {
                        //Turma
                        lblInformacaoAluno.Text += "<b>Ciclo de alfabetização: </b>" + turmaCodigo + "<br/>";

                        //Ano
                        lblInformacaoAluno.Text += "<b>Ano: </b>" + cal_ano;
                    }

                    //Número de chamada
                    int numeroChamada;
                    Int32.TryParse(mtu_numeroChamada, out numeroChamada);

                    if (numeroChamada > 0)
                        lblInformacaoAluno.Text += "&nbsp;&nbsp;&nbsp;<b>Nº chamada: </b>" + mtu_numeroChamada; 
                }
                
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            __SessionWEB.PostMessages = UtilBO.GetErroMessage("Erro ao tentar carregar as informações do aluno.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion Métodos
}