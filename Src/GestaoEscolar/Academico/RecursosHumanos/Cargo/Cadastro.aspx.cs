using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class Academico_RecursosHumanos_Cargo_Cadastro : MotherPageLogado
{
    #region PROPRIEDADES

    private int _VS_crg_id
    {
        get
        {
            if (ViewState["_VS_crg_id"] != null)
                return Convert.ToInt32(ViewState["_VS_crg_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_crg_id"] = value;
        }
    }

    /// <summary>
    /// Propriedade que guarda a tabela de cargos/disciplinas
    /// no ViewState.
    /// </summary>
    public DataTable _VS_cargosDisciplinas
    {
        get
        {
            if (ViewState["_VS_cargosDisciplinas"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("crg_id");
                dt.Columns.Add("tds_id");

                dt.PrimaryKey = new[] { dt.Columns["crg_id"], dt.Columns["tds_id"] };

                ViewState["_VS_cargosDisciplinas"] = dt;
            }
            return (DataTable)ViewState["_VS_cargosDisciplinas"];
        }
        set
        {
            ViewState["_VS_cargosDisciplinas"] = value;
        }
    }

    #endregion PROPRIEDADES

    #region METODOS

    private void _LoadFromEntity(int crg_id)
    {
        try
        {
            RHU_Cargo Cargo = new RHU_Cargo { crg_id = crg_id };
            RHU_CargoBO.GetEntity(Cargo);

            CarregarDisciplinas(Cargo);

            if (Cargo.ent_id != __SessionWEB.__UsuarioWEB.Usuario.ent_id)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("O cargo não pertence à entidade na qual você está logado.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            _VS_crg_id = Cargo.crg_id;
            _UCComboTipoVinculo.Valor = Cargo.tvi_id;
            _UCComboTipoVinculo.PermiteEditar = false;
            _ckbCargoDocente.Enabled = false;
            _ckbEspecialista.Enabled = false;
            _txtNome.Text = Cargo.crg_nome;
            _txtCodigo.Text = Cargo.crg_codigo;
            _txtDescricao.Text = Cargo.crg_descricao;
            _txtCodIntegracao.Text = Cargo.crg_codIntegracao;
            _txtMaxAulaSemana.Text = Cargo.crg_maxAulaSemana.ToString();
            _txtMaxAulaDia.Text = Cargo.crg_maxAulaDia.ToString();
            ckbControleIntegracao.Checked = Cargo.crg_controleIntegracao;

            ddlTipo.SelectedValue = Cargo.crg_tipo.ToString();

            if (!string.IsNullOrEmpty(Cargo.pgs_chave))
            {
                if (UCComboParametroGrupoPerfil1.ExisteItem(Cargo.pgs_chave))
                    UCComboParametroGrupoPerfil1.Valor = Cargo.pgs_chave;
            }

            _ckbBloqueado.Visible = true;
            _ckbBloqueado.Checked = !Cargo.crg_situacao.Equals(1);

            if (Cargo.crg_cargoDocente.Equals(false))
            {
                _ckbCargoDocente.Checked = false;
                divDisciplinas.Visible = false;
            }
            else
            {
                _ckbCargoDocente.Checked = true;

                if (_txtMaxAulaDia.Text == "0")
                    _txtMaxAulaDia.Text = string.Empty;
                if (_txtMaxAulaSemana.Text == "0")
                    _txtMaxAulaSemana.Text = string.Empty;
                divDisciplinas.Visible = true;

                divCargoDocente.Visible = true;

                _ckbEspecialista.Visible = true;

                if (Cargo.crg_especialista.Equals(true))
                {
                    _ckbEspecialista.Checked = true;
                    divDisciplinas.Visible = false;
                }
                else
                {
                    divDisciplinas.Visible = true;
                }

                if (GestaoEscolarUtilBO.VerificarIntegridade("crg_id", _VS_crg_id.ToString(), "RHU_Cargo", null))
                {
                    _txtMaxAulaDia.Enabled = false;
                    _txtMaxAulaSemana.Enabled = false;
                }
                else
                {
                    _txtMaxAulaDia.Enabled = true;
                    _txtMaxAulaSemana.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o cargo.", UtilBO.TipoMensagem.Erro);
        }
    }

    //Carrega as disciplinas do cargo
    public void CarregarDisciplinas(RHU_Cargo cargo)
    {
        odsDisciplinas.SelectParameters.Clear();
        odsDisciplinas.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDisciplinas.SelectParameters.Add("AppMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        // Forçar atualização do checkboxList
        cblDisciplinasPossiveis.DataBind();

        if (cargo.crg_cargoDocente)
        {
            if (!cargo.crg_especialista)
            {
                List<RHU_CargoDisciplina> listDis = new List<RHU_CargoDisciplina>();

                listDis = RHU_CargoDisciplinaBO.RetornaDisciplinasCargo(cargo.crg_id);

                if (listDis.Count > 0)
                {
                    foreach (ListItem li in cblDisciplinasPossiveis.Items)
                    {
                        int tds_id = Convert.ToInt32(li.Value);

                        if (listDis.Exists(p => p.tds_id == tds_id))
                            li.Selected = true;
                    }
                }
            }
        }
    }

    public void Salvar()
    {
        try
        {
            if (_ckbCargoDocente.Checked)
            {
                if (!string.IsNullOrEmpty(_txtMaxAulaDia.Text) && (Convert.ToInt32(_txtMaxAulaDia.Text) > 24))
                    throw new ArgumentException("Máximo de aulas por dia não pode ser maior do que 24.");

                if (!string.IsNullOrEmpty(_txtMaxAulaSemana.Text) && (Convert.ToInt32(_txtMaxAulaSemana.Text) > 168))
                    throw new ArgumentException("Máximo de aulas por semana não pode ser maior do que 168.");
            }

            RHU_Cargo Cargo = new RHU_Cargo
            {
                crg_id = _VS_crg_id
                ,
                crg_nome = _txtNome.Text
                ,
                crg_codigo = _txtCodigo.Text
                ,
                crg_descricao = _txtDescricao.Text
                ,
                crg_codIntegracao = _txtCodIntegracao.Text
                ,
                tvi_id = _UCComboTipoVinculo.Valor
                ,
                ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id
                ,
                crg_maxAulaSemana = Convert.ToByte(string.IsNullOrEmpty(_txtMaxAulaSemana.Text) ? "0" : _txtMaxAulaSemana.Text)
                ,
                crg_maxAulaDia = Convert.ToByte(string.IsNullOrEmpty(_txtMaxAulaDia.Text) ? "0" : _txtMaxAulaDia.Text)
                ,
                crg_cargoDocente = _ckbCargoDocente.Checked
                ,
                crg_especialista = _ckbEspecialista.Checked
                ,
                pgs_chave = UCComboParametroGrupoPerfil1.Valor == "-1" ? string.Empty : UCComboParametroGrupoPerfil1.Valor
                ,
                crg_situacao = Convert.ToByte(_ckbBloqueado.Checked ? 2 : 1)
                ,
                crg_tipo = Convert.ToByte(ddlTipo.SelectedValue)
                ,
                crg_controleIntegracao = ckbControleIntegracao.Checked
                ,
                IsNew = (_VS_crg_id > 0) ? false : true
            };

            //Lista para salvar as disciplinas do cargo
            List<RHU_CargoDisciplina> listDis = new List<RHU_CargoDisciplina>();

            if (fsDisciplinas.Visible)
            {
                foreach (ListItem li in cblDisciplinasPossiveis.Items)
                {
                    if (li.Selected)
                    {
                        RHU_CargoDisciplina entityDis = new RHU_CargoDisciplina
                        {
                            tds_id = Convert.ToInt32(li.Value)
                        };

                        listDis.Add(entityDis);
                    }
                }
            }

            if (RHU_CargoBO.Save(Cargo, listDis))
            {
                if (_VS_crg_id <= 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "crg_id: " + Cargo.crg_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Cargo incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "crg_id: " + Cargo.crg_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Cargo alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                Response.Redirect("Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o cargo.", UtilBO.TipoMensagem.Erro);
            }
        }
        catch (MSTech.Validation.Exceptions.ValidationException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (ArgumentException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (DuplicateNameException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o cargo." + ex.Message, UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion METODOS

    #region EVENTOS

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    /// <author>juliano.real</author>
    /// <datetime>19/11/2013-10:41</datetime>
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
        }

        if (!IsPostBack)
        {
            _ckbBloqueado.Visible = false;
            divCargoDocente.Visible = false;
            divDisciplinas.Visible = false;

            try
            {
                _UCComboTipoVinculo.ValidationGroup = "Cargo";
                _UCComboTipoVinculo.Obrigatorio = true;
                _UCComboTipoVinculo.CarregarTipoVinculo();

                UCComboParametroGrupoPerfil1.CarregarGrupoPadrao();
                UCComboParametroGrupoPerfil1.Obrigatorio = true;
                UCComboParametroGrupoPerfil1.ValidationGroup = "Cargo";
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

            if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
            {
                _LoadFromEntity(PreviousPage.EditItem_crg_id);
                Page.Form.DefaultFocus = _txtNome.ClientID;
            }
            else
            {
                Page.Form.DefaultFocus = _UCComboTipoVinculo.Combo_ClientID;
                _btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }

            Page.Form.DefaultButton = _btnSalvar.UniqueID;

            bool podeEditarCargo = ((__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && _VS_crg_id > 0) ||
                                      (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && _VS_crg_id <= 0));

            if (!podeEditarCargo)
            {
                HabilitaControles(fdsCargos.Controls, false);
                _btnCancelar.Enabled = true;
            }
        }
    }

    protected void _cbkCargoDocente_CheckedChanged(object sender, EventArgs e)
    {
        if (_ckbCargoDocente.Checked.Equals(true))
        {
            _txtMaxAulaDia.Focus();

            _txtMaxAulaDia.Text = string.Empty;
            _txtMaxAulaSemana.Text = string.Empty;
            _ckbEspecialista.Checked = false;
            divDisciplinas.Visible = true;
            divCargoDocente.Visible = true;

            CarregarDisciplinas(new RHU_Cargo());
        }
        else
        {
            _txtMaxAulaDia.Text = string.Empty;
            _txtMaxAulaSemana.Text = string.Empty;
            _ckbEspecialista.Checked = false;
            divCargoDocente.Visible = false;
            divDisciplinas.Visible = false;
        }
    }

    protected void _cbkCargoEspecialista_CheckedChanged(object sender, EventArgs e)
    {
        //Quando especialista estiver checado não será exibida as disciplinas
        if (_ckbEspecialista.Checked.Equals(true))
            divDisciplinas.Visible = false;
        else
            divDisciplinas.Visible = true;
    }

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _btnSalvar_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
            Salvar();
    }

    #endregion EVENTOS
}