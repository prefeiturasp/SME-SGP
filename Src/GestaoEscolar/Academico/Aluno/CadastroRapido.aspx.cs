using System;
using System.Data;
using System.Threading;
using System.Web.UI;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System.Web;
using System.Web.UI.WebControls;

public partial class Academico_Aluno_CadastroRapido : MotherPageLogado
{
    #region METODOS

    /// <summary>
    /// Salva dados do aluno
    /// </summary>
    private void Salvar(bool novo)
    {
        try
        {
            string msgErro;
            if (lblMessage.Text == string.Empty && !UCGridContato1.SalvaConteudoGrid(out msgErro))
            {
                UCGridContato1._MensagemErro.Visible = false;
                lblMessage.Text = msgErro;
                return;
            }

            PES_Pessoa entityPessoa = new PES_Pessoa
            {
                pes_nome = txtNome.Text
                ,
                cid_idNaturalidade = string.IsNullOrEmpty(_txtCid_id.Value) ? Guid.Empty : new Guid(_txtCid_id.Value)
                ,
                pes_dataNascimento = string.IsNullOrEmpty(txtDataNasc.Text) ? new DateTime() : Convert.ToDateTime(txtDataNasc.Text)
                ,
                pes_racaCor = UCComboRacaCor1._Combo.SelectedValue == "-1" ? Convert.ToByte(null) : Convert.ToByte(UCComboRacaCor1._Combo.SelectedValue)
                ,
                pes_sexo = UCComboSexo1._Combo.SelectedValue == "-1" ? Convert.ToByte(null) : Convert.ToByte(UCComboSexo1._Combo.SelectedValue)
                ,
                pes_estadoCivil = UCComboEstadoCivil1._Combo.SelectedValue == "-1" ? Convert.ToByte(null) : Convert.ToByte(UCComboEstadoCivil1._Combo.SelectedValue)
                ,
                pes_situacao = 1
                ,
                IsNew = true
            };

            PES_PessoaDeficiencia entityPessoaDeficiencia = new PES_PessoaDeficiencia
            {
                tde_id = new Guid(UCComboTipoDeficiencia1._Combo.SelectedValue)
            };

            END_Cidade cid = new END_Cidade
            {
                cid_id = new Guid(_txtCid_idCertidao.Value)
            };
            END_CidadeBO.GetEntity(cid);

            PES_CertidaoCivil entityCertidaoCivil = new PES_CertidaoCivil
            {
                ctc_tipo = 1
                ,
                cid_idCartorio = new Guid(_txtCid_idCertidao.Value)
                ,
                unf_idCartorio = cid.unf_id
                ,
                ctc_distritoCartorio = txtDistritoCertidao.Text
                ,
                ctc_dataEmissao = string.IsNullOrEmpty(txtDataEmissao.Text) ? new DateTime() : Convert.ToDateTime(txtDataEmissao.Text)
                ,
                ctc_folha = txtFolha.Text
                ,
                ctc_livro = txtLivro.Text
                ,
                ctc_numeroTermo = txtNumeroTermo.Text
            };

            END_Endereco entityEndereco;
            string numero;
            string complemento;
            string msg;

            bool cadastraEndereco = UCEnderecos1.RetornaEnderecoCadastrado(out entityEndereco, out numero, out complemento, out msg);

            if (!cadastraEndereco)
                throw new ValidationException(msg);

            DataTable dtEndereco = string.IsNullOrEmpty(entityEndereco.end_cep) ? new DataTable() : UCEnderecos1._VS_enderecos;

            ACA_Aluno entityAluno = new ACA_Aluno
            {
                ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id
                ,
                alu_situacao = _ddlSituacao.SelectedValue == "-1" ? Convert.ToByte(0) : Convert.ToByte(_ddlSituacao.SelectedValue)
                ,
                IsNew = true
            };

            ACA_AlunoCurriculo entityAlunoCurriculo = new ACA_AlunoCurriculo
            {
                esc_id = UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue.Split(';')[0] == "-1" ? Convert.ToInt32(null) : Convert.ToInt32(UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue.Split(';')[0])
                ,
                uni_id = UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue.Split(';')[1] == "-1" ? Convert.ToInt32(null) : Convert.ToInt32(UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue.Split(';')[1])
                ,
                cur_id = UCComboCurriculoPeriodo1._Combo.SelectedValue.Split(';')[0] == "-1" ? Convert.ToInt32(null) : Convert.ToInt32(UCComboCurriculoPeriodo1._Combo.SelectedValue.Split(';')[0])
                ,
                crr_id = UCComboCurriculoPeriodo1._Combo.SelectedValue.Split(';')[1] == "-1" ? Convert.ToInt32(null) : Convert.ToInt32(UCComboCurriculoPeriodo1._Combo.SelectedValue.Split(';')[1])
                ,
                crp_id = UCComboCurriculoPeriodo1._Combo.SelectedValue.Split(';')[2] == "-1" ? Convert.ToInt32(null) : Convert.ToInt32(UCComboCurriculoPeriodo1._Combo.SelectedValue.Split(';')[2])
                ,
                alc_matriculaEstadual = txtMatriculaEstadual.Text
                ,
                alc_situacao = _ddlSituacao.SelectedValue == "-1" ? Convert.ToByte(0) : Convert.ToByte(_ddlSituacao.SelectedValue)
            };

            if (ACA_AlunoBO.Save_CadastroRapido(entityPessoa
                                                , entityPessoaDeficiencia
                                                , dtEndereco
                                                , UCGridContato1._VS_contatos
                                                , entityCertidaoCivil
                                                , txtMae.Text
                                                , txtCPFMae.Text
                                                , txtPai.Text
                                                , txtCPFPai.Text
                                                , rfvMatriculaEstadual.Enabled
                                                , entityAluno
                                                , entityAlunoCurriculo
                                                , null
                                                , null))
            {
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "alu_id: " + entityAluno.alu_id);
                __SessionWEB.PostMessages = UtilBO.GetErroMessage(String.Format("Aluno incluído com sucesso."), UtilBO.TipoMensagem.Sucesso);

                Response.Redirect("CadastroRapido.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();

                string redirect = novo ?
                "~/Academico/Aluno/CadastroRapido.aspx" :
                "~/Academico/Aluno/Busca.aspx";

                Response.Redirect(redirect, false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o aluno.", UtilBO.TipoMensagem.Erro);
            }
        }
        catch (ThreadAbortException)
        {

        }
        catch (ValidationException ex)
        {
            UCEnderecos1.AtualizaEnderecos();
            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (ArgumentException ex)
        {
            UCEnderecos1.AtualizaEnderecos();
            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (DuplicateNameException ex)
        {
            UCEnderecos1.AtualizaEnderecos();
            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            UCEnderecos1.AtualizaEnderecos();
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o aluno.", UtilBO.TipoMensagem.Erro);
        }
    }

    #region DELEGATES

    private void UCFiltroEscolas1__Selecionar()
    {
        try
        {
            if (UCFiltroEscolas1._VS_FiltroEscola)
                UCFiltroEscolas1._UnidadeEscola_LoadBy_uad_idSuperior(new Guid(UCFiltroEscolas1._ComboUnidadeAdministrativa.SelectedValue));

            if (UCFiltroEscolas1._ComboUnidadeAdministrativa.SelectedValue == Guid.Empty.ToString())
            {
                UCFiltroEscolas1._ComboUnidadeEscola.Enabled = false;
            }
            else
            {
                UCFiltroEscolas1._ComboUnidadeEscola.Enabled = true;
                UCFiltroEscolas1._ComboUnidadeEscola.Focus();
            }

            UCComboCursoCurriculo1.Obrigatorio = true;
            UCComboCursoCurriculo1.CarregarCursoCurriculo();
            UCComboCursoCurriculo1.PermiteEditar = false;

            UCComboCurriculoPeriodo1._Combo.Items.Clear();
            UCComboCurriculoPeriodo1._MostrarMessageSelecione = true;
            UCComboCurriculoPeriodo1._Load(-1, -1);
            UCComboCurriculoPeriodo1._Combo.Enabled = false;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessageMatricula.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
        finally
        {
            updMatricula.Update();
        }
    }

    private void UCFiltroEscolas1__SelecionarEscola()
    {
        try
        {
            if (UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue != "-1;-1")
            {
                UCComboCursoCurriculo1.Obrigatorio = true;
                UCComboCursoCurriculo1.CarregarCursoCurriculoPorEscola(UCFiltroEscolas1._UCComboUnidadeEscola_Esc_ID, UCFiltroEscolas1._UCComboUnidadeEscola_Uni_ID, 0);
                UCComboCursoCurriculo1.PermiteEditar = true;
                UCComboCursoCurriculo1.SetarFoco();               
            }
            else
            {
                UCComboCursoCurriculo1.Obrigatorio = true;
                UCComboCursoCurriculo1.CarregarCursoCurriculo();
                UCComboCursoCurriculo1.PermiteEditar = false;
            }

            UCComboCurriculoPeriodo1._Combo.Items.Clear();
            UCComboCurriculoPeriodo1._MostrarMessageSelecione = true;
            UCComboCurriculoPeriodo1._Load(-1, -1);
            UCComboCurriculoPeriodo1._Combo.Enabled = false;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessageMatricula.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
        finally
        {
            updMatricula.Update();
        }
    }

    private void UCComboCursoCurriculo1_IndexChanged()
    {
        try
        {
            if (UCComboCursoCurriculo1.Valor[0] > 0)
            {

                UCComboCurriculoPeriodo1._Combo.Items.Clear();
                UCComboCurriculoPeriodo1._MostrarMessageSelecione = true;
                UCComboCurriculoPeriodo1._LoadBy_cur_id_crr_id_esc_id_uni_id
                    (
                        UCComboCursoCurriculo1.Valor[0]
                        , UCComboCursoCurriculo1.Valor[1]
                        , Convert.ToInt32(UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue.Split(';')[0])
                        , Convert.ToInt32(UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue.Split(';')[1])

                    );
                UCComboCurriculoPeriodo1._Combo.Enabled = true;
                UCComboCurriculoPeriodo1._Combo.Focus();
            }
            else
            {
                UCComboCurriculoPeriodo1._Combo.Items.Clear();
                UCComboCurriculoPeriodo1._MostrarMessageSelecione = true;
                UCComboCurriculoPeriodo1._Load(-1, -1);
                UCComboCurriculoPeriodo1._Combo.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessageMatricula.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
        finally
        {
            updMatricula.Update();
        }
    }

    #endregion

    #endregion

    #region EVENTOS

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.Tabs));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroPessoa.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsUCCadastroEndereco.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroCertidaoCivil.js"));
            sm.Services.Add(new ServiceReference("~/WSServicos.asmx"));
        }

        if (!IsPostBack)
        {
            cvDataNascimento.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de nascimento do aluno");
            CustomValidator1.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de emissão da certidão de nascimento");

            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                lblMessage.Text = message;

            lblMatriculaEstadual.Text = GestaoEscolarUtilBO.nomePadraoMatriculaEstadual() + " *";
            rfvMatriculaEstadual.ErrorMessage = GestaoEscolarUtilBO.nomePadraoMatriculaEstadual() + " é obrigatório.";

            UCComboTipoDeficiencia1._MostrarMessageSelecione = true;
            UCComboTipoDeficiencia1._Load(Guid.Empty, 0);

            UCEnderecos1.Inicializar(false, true, string.Empty);

            UCFiltroEscolas1.SelecionaCombosAutomatico = false;
            UCFiltroEscolas1.UnidadeAdministrativaCampoObrigatorio = true;
            UCFiltroEscolas1.EscolaCampoObrigatorio = true;
            UCFiltroEscolas1._LoadInicial();            

            UCComboCursoCurriculo1.Obrigatorio = true;
            UCComboCursoCurriculo1.CarregarCursoCurriculo();
            UCComboCursoCurriculo1.PermiteEditar = false;            

            UCComboCurriculoPeriodo1._Label.Text += " *";
            UCComboCurriculoPeriodo1._MostrarMessageSelecione = true;
            UCComboCurriculoPeriodo1._Load(-1, -1);
            UCComboCurriculoPeriodo1._Combo.Enabled = false;
            UCComboCurriculoPeriodo1.ExibeFormatoPeriodo = true;
            cvCurriculoPeriodo.ErrorMessage = GestaoEscolarUtilBO.nomePadraoPeriodo() + " é obrigatório.";

            try
            {
                if (__SessionWEB._cid_id != Guid.Empty)
                {
                    END_Cidade cid = new END_Cidade { cid_id = __SessionWEB._cid_id };
                    END_CidadeBO.GetEntity(cid);

                    _txtCid_id.Value = cid.cid_id.ToString();
                    txtNaturalidade.Text = cid.cid_nome;

                    _txtCid_idCertidao.Value = cid.cid_id.ToString();
                    txtCidadeCertidao.Text = cid.cid_nome;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

            UCGridContato1._CarregarContato();

            UCComboTipoDeficiencia1._Label.Text = "Necessidade educacional especial";
            UCComboEstadoCivil1._Label.Text = "Estado civil *";
            UCComboRacaCor1._Label.Text = "Raça / cor *";
            UCComboSexo1._Label.Text = "Sexo *";

            UCComboEstadoCivil1._Combo.SelectedValue = "1";
            _ddlSituacao.SelectedValue = "1";

            Page.Form.DefaultFocus = txtMatriculaEstadual.ClientID;
            Page.Form.DefaultButton = btnSalvarNovo.UniqueID;
            btnSalvarNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }

        UCFiltroEscolas1._Selecionar += UCFiltroEscolas1__Selecionar;
        UCFiltroEscolas1._SelecionarEscola += UCFiltroEscolas1__SelecionarEscola;
        UCComboCursoCurriculo1.IndexChanged += UCComboCursoCurriculo1_IndexChanged;
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _btnSalvar_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
            Salvar(((Button)sender).ID.Equals(btnSalvarNovo.ID));
    }

    /// <summary>
    /// disparado ao selecionar um item do drop down list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void _ddlSituacao_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList _ddlSituacao = (DropDownList)sender;

        // situação = "Excedente"
        if (_ddlSituacao.SelectedValue == "8")
        {
            // remove '*' do label
            lblMatriculaEstadual.Text = lblMatriculaEstadual.Text.Replace("*", "");
            // desabilita o requiredfield
            rfvMatriculaEstadual.Enabled = false;
        }
        else
        {
            // coloca '*' do label
            if(!lblMatriculaEstadual.Text.Trim().EndsWith("*"))
                lblMatriculaEstadual.Text += "*";
            // habilita o requiredfield
            rfvMatriculaEstadual.Enabled = true;
        }
    }

    #endregion
}




