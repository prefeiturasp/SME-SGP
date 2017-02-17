using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.CoreSSO.BLL;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MSTech.Validation.Exceptions;

public partial class Academico_Turno_Cadastro : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Propriedade na qual cria variavel em ViewState armazenando valor de trn_id
    /// </summary>
    private int _VS_trn_id
    {
        get
        {
            if (ViewState["_VS_trn_id"] != null)
                return Convert.ToInt32(ViewState["_VS_trn_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_trn_id"] = value;
        }
    }

    /// <summary>
    /// Armazena o sequencial para inclusão no DataTable quando for do tipo int
    /// </summary>
    public int _VS_seq
    {
        get
        {
            if (ViewState["_VS_seq"] != null)
                return Convert.ToInt32(ViewState["_VS_seq"]);
            return -1;
        }
        set
        {
            ViewState["_VS_seq"] = value;
        }
    }

    #endregion

    #region Métodos
    
    /// <summary>
    /// Metodo para carregar Turno e Turnos Horarios referente a este Turno
    /// </summary>
    /// <param name="trn_id">ID do turno</param>
    private void _Carregar(int trn_id)
    {
        try
        {
            // Carrega turno
            ACA_Turno _Turno = new ACA_Turno { trn_id = trn_id };
            ACA_TurnoBO.GetEntity(_Turno);

            if (_Turno.ent_id != __SessionWEB.__UsuarioWEB.Usuario.ent_id)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("O turno não pertence à entidade na qual você está logado.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Academico/Turno/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            _VS_trn_id = _Turno.trn_id;
            _txtDescricao.Text = _Turno.trn_descricao;

            _UCComboTipoTurno.Valor = _Turno.ttn_id;

            if (_Turno.trn_situacao == 2)
                _ckbBloqueado.Checked = true;

            ddlcontroleTempo.SelectedValue = Convert.ToString(_Turno.trn_controleTempo);
            if (_Turno.trn_controleTempo == 2)
            {
                MostraHorasTurno(true);
                txtHoraFim.Text = _Turno.trn_horaFim.ToString();
                txtHoraInicio.Text = _Turno.trn_horaInicio.ToString();
            }
            else
                MostraHorasTurno(false);

            CarregarHorariosDoBanco(trn_id);

            DataTable dt = ACA_TurnoHorarioBO.GetSelectDiasSemana(_VS_trn_id);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                foreach (ListItem chk in chkDiasSemana.Items)
                {
                    if (chk.Value == dt.Rows[i]["trh_diaSemana"].ToString())
                    {
                        chk.Selected = true;
                        break;
                    }
                }
            }

            _UCComboTipoTurno.PermiteEditar = false;
            _UCComboTipoTurno.Obrigatorio = false;

            ddlcontroleTempo.Enabled = false;

            if (TUR_TurmaBO.VerificaTurmaAssociada(trn_id))
            {
                HabilitaControles(_rptHorarios.Controls, false);
                txtHoraInicio.Enabled = false;
                txtHoraFim.Enabled = false;
                rfvHoraFim.Enabled = false;
                rfvHoraInicio.Enabled = false;
                chkDiasSemana.Enabled = false;                
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o turno.", UtilBO.TipoMensagem.Erro);
        }
    }
    
    /// <summary>
    /// Exibe ou nao os campos de hora incial (trn_horaInicio) e hora final (trn_horaFim)
    /// Se ControleTempo = Tempos de aula -> nao mostra
    /// Se ControleTempo = Horas -> exibe perioro
    /// </summary>
    /// <param name="bMostra">TRUE - Se ControleTempo = Tempos de aula // FALSE - Se ControleTempo = Horas</param>
    private void MostraHorasTurno(bool bMostra)
    {
        lblHoraInicio.Visible = bMostra;
        txtHoraInicio.Visible = bMostra;
        lblHoraFim.Visible = bMostra;
        txtHoraFim.Visible = bMostra;
        rfvHoraInicio.Enabled = bMostra;
        rfvHoraFim.Enabled = bMostra;

        if (!bMostra)
        {
            txtHoraInicio.Text = "";
            txtHoraFim.Text = "";
        }
    }

    /// <summary>
    /// Método que carrega o repeater de horários com os dados do banco.
    /// </summary>
    /// <param name="trn_id">Id do turno.</param>
    public void CarregarHorariosDoBanco(int trn_id)
    {
        try
        {
            DataTable dt = ACA_TurnoHorarioBO.GetSelectBy_trn_id(trn_id, false, 1, 1);

            if (dt.Rows.Count == 0)
            {
                AdicionarLinhaRepeater();
            }
            else
            {
                _rptHorarios.DataSource = dt;
                _rptHorarios.DataBind();
            }
        }
        catch (Exception ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Alerta);
            ApplicationWEB._GravaErro(ex);
        }
    }

    /// <summary>
    /// Método que retorna um data table contendo todas as informações do repeater de horários.
    /// </summary>
    /// <returns>Data table contendo todas as informações do repeater de horários.</returns>
    private DataTable CarregarDataTableComHorarios()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("trh_id");
        dt.Columns.Add("trh_horaInicio");
        dt.Columns.Add("trh_horaFim");
        dt.Columns.Add("trh_tipo");
        dt.Columns.Add("banco");

        foreach (RepeaterItem ri in _rptHorarios.Items)
        {
            if ((!String.IsNullOrEmpty(((TextBox)ri.FindControl("_txtHoraInicial")).Text))
                    && (!String.IsNullOrEmpty(((TextBox)ri.FindControl("_txtHoraFinal")).Text))
                    && (((DropDownList)ri.FindControl("_ddlTipoHorario")).SelectedIndex > 0))
            {
                DataRow dr = dt.NewRow();
                dr["trh_id"] = ((Label)ri.FindControl("trh_id")).Text;
                dr["trh_horaInicio"] = ((TextBox)ri.FindControl("_txtHoraInicial")).Text;
                dr["trh_horaFim"] = ((TextBox)ri.FindControl("_txtHoraFinal")).Text;
                dr["trh_tipo"] = ((DropDownList)ri.FindControl("_ddlTipoHorario")).SelectedIndex;
                dr["banco"] = ((Label)ri.FindControl("banco")).Text;

                dt.Rows.Add(dr);
            }
        }
        return dt;
    }

    /// <summary>
    /// Método que adiciona uma linha ao repeater de horários.
    /// </summary>
    private void AdicionarLinhaRepeater()
    {
        _lblMessageHorario.Text = "";
        _lblMessageHorario.Visible = false;

        DataTable dt = CarregarDataTableComHorarios();

        DataRow dr = dt.NewRow();

        dr["trh_horaInicio"] = "";
        dr["trh_horaFim"] = "";
        dr["trh_tipo"] = 0;
        dr["trh_id"] = "";
        dr["banco"] = "false";

        dt.Rows.Add(dr);

        _rptHorarios.DataSource = dt;
        _rptHorarios.DataBind();
    }
    
    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
        }

        if (!IsPostBack)
        {
            try
            {
                // por padrao Controle de horas/aula é 1 - Tempos de aula
                // e esse tipo não tem intervalo de horarios
                MostraHorasTurno(false);

                _UCComboTipoTurno.CarregarTipoTurno();
                _UCComboTipoTurno.Obrigatorio = true;
                _UCComboTipoTurno.ValidationGroup = "_ValidationTurno";

                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                {
                    _Carregar(PreviousPage.EditItem);
                    Page.Form.DefaultFocus = _txtDescricao.ClientID;
                }
                else
                {
                    AdicionarLinhaRepeater();

                    _ckbBloqueado.Visible = false;

                    Page.Form.DefaultFocus = _UCComboTipoTurno.Combo_ClientID;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Academico/Turno/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void ddlcontroleTempo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcontroleTempo.SelectedValue == "1")
        { MostraHorasTurno(false); }
        else if (ddlcontroleTempo.SelectedValue == "2")
        { MostraHorasTurno(true); }
    }

    protected void _rptTipoContato_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer && e.Item.ItemType != ListItemType.Pager)
        {
            ((DropDownList)e.Item.FindControl("_ddlTipoHorario")).SelectedIndex = Convert.ToInt32(((Label)e.Item.FindControl("trh_tipo")).Text);
        }
    }
    
    #endregion
}
