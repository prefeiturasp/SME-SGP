using System;
using System.Web;
using MSTech.CoreSSO.BLL;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Web.UI;

public partial class Academico_RecursosHumanos_CargaHoraria_Cadastro : MotherPageLogado
{
    #region PROPRIEDADES

    private int _VS_chr_id
    {
        get
        {
            if (ViewState["_VS_chr_id"] != null)
                return Convert.ToInt32(ViewState["_VS_chr_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_chr_id"] = value;
        }
    }

    #endregion

    #region METODOS
    
    /// <summary>
    /// Carrega as informações da carga horária.
    /// </summary>
    /// <param name="chr_id">id da carga horária</param>
    private void _Carregar(int chr_id)
    {
        try
        {
            RHU_CargaHoraria _CargaHoraria = new RHU_CargaHoraria { chr_id = chr_id };
            RHU_CargaHorariaBO.GetEntity(_CargaHoraria);

            if (_CargaHoraria.ent_id != __SessionWEB.__UsuarioWEB.Usuario.ent_id)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("A carga horária não pertence à entidade na qual você está logado.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            _VS_chr_id = _CargaHoraria.chr_id;
            _txtDescricao.Text = _CargaHoraria.chr_descricao;
            _chkPadrao.Checked = _CargaHoraria.chr_padrao;

            _chkEspecialista.Checked = _CargaHoraria.chr_especialista ?? false;

            _UCComboCargo.Valor = (_CargaHoraria.crg_id.Equals(0)) ? -1 : Convert.ToInt32(_CargaHoraria.crg_id);
            _txtCargaHrsSemanais.Text = _CargaHoraria.chr_cargaHorariaSemanal.ToString();
            _txtTemposAulas.Text = _CargaHoraria.chr_temposAula.ToString();
            _txtHorasAulas.Text = _CargaHoraria.chr_horasAula.ToString();
            _txtHorasComplementares.Text = _CargaHoraria.chr_horasComplementares.ToString();

            if (_chkPadrao.Checked)
            {
                _chkEspecialista.Visible = true;
                _UCComboCargo.Visible = false;
                _UCComboCargo.Obrigatorio = false;
            }
            else
            {
                _chkEspecialista.Visible = false;
                _UCComboCargo.Visible = true;
                _UCComboCargo.Obrigatorio = true;
            }

            _ckbBloqueado.Checked = !_CargaHoraria.chr_situacao.Equals(1);

        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a carga horária.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion

    #region EVENTOS

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
            try
            {
                _UCComboCargo.CarregarCargo();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

            if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                _Carregar(PreviousPage.EditItem);
            else
            {
                _ckbBloqueado.Visible = false;
            }

            Page.Form.DefaultFocus = _txtDescricao.ClientID;
        }
    }

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _chkPadrao_CheckedChanged(object sender, EventArgs e)
    {
        _chkEspecialista.Checked = false;
        _UCComboCargo.Valor = -1;

        if (_chkPadrao.Checked)
        {
            _chkEspecialista.Visible = true;
            _UCComboCargo.Visible = false;
            _UCComboCargo.Obrigatorio = false;
        }
        else
        {
            _chkEspecialista.Visible = false;
            _UCComboCargo.Visible = true;
            _UCComboCargo.Obrigatorio = true;
        }
    }

    #endregion
}