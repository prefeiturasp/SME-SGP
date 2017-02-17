using System;
using System.Data;
using System.Web;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class Configuracao_TipoTurno_Cadastro : MotherPageLogado
{
    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                _Carregar(PreviousPage.EditItem);
            else
            {
                _ckbBloqueado.Visible = false;
            }

            Page.Form.DefaultFocus = _txtTipoTurno.ClientID;
        }
    }

    #endregion

    #region PROPRIEDADES

    /// <summary>
    /// Propriedade em ViewState que armazena valor de ttn_id (ID de tipo de turno)
    /// no caso de atualização de um registro ja existente.
    /// </summary>
    private int _VS_ttn_id
    {
        get
        {
            if (ViewState["_VS_ttn_id"] != null)
                return Convert.ToInt32(ViewState["_VS_ttn_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_ttn_id"] = value;
        }
    }

    #endregion PROPRIEDADES

    #region METODOS
    
    /// <summary>
    /// Carrega os dados do Tipo de Turno nos controles caso seja alteração.
    /// </summary>
    /// <param name="ttn_id">Id do tipo de turno</param>
    private void _Carregar(int ttn_id)
    {
        try
        {
            ACA_TipoTurno _TipoTurno = new ACA_TipoTurno { ttn_id = ttn_id };
            ACA_TipoTurnoBO.GetEntity(_TipoTurno);
            _VS_ttn_id = _TipoTurno.ttn_id;
            _txtTipoTurno.Text = _TipoTurno.ttn_nome;
            _ddlTipoTurno.SelectedValue = _TipoTurno.ttn_tipo == 0 ? "-1" : _TipoTurno.ttn_tipo.ToString();
            if (_TipoTurno.ttn_situacao == 2)
            {
                _ckbBloqueado.Checked = true;
            }
        }
        catch (Exception e)
        {
            ApplicationWEB._GravaErro(e);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o tipo de turno.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion METODOS

    #region EVENTOS

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoTurno/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
    
    #endregion EVENTOS
}