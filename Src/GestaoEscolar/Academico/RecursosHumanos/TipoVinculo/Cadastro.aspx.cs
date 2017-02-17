using System;
using System.Data;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using System.Web.UI;
using System.Web;

public partial class Academico_RecursosHumanos_TipoVinculo_Cadastro : MotherPageLogado
{    
    #region PROPRIEDADES

    private int _VS_tvi_id
    {
        get
        {
            if (ViewState["_VS_vti_id"] != null)
                return Convert.ToInt32(ViewState["_VS_vti_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_vti_id"] = value;
        }
    }

    #endregion

    #region METODOS

    /// <summary>
    /// Carrega os dados do grupo de usuário nos controles caso seja alteração.
    /// </summary>
    /// <param name="tvi_id"></param>
    private void _LoadFromEntity(int tvi_id)
    {
        try
        {
            RHU_TipoVinculo TipoVinculo = new RHU_TipoVinculo { tvi_id = tvi_id };
            RHU_TipoVinculoBO.GetEntity(TipoVinculo);

            if (TipoVinculo.ent_id != __SessionWEB.__UsuarioWEB.Usuario.ent_id)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("O tipo de vínculo não pertence à entidade na qual você está logado.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Academico/RecursosHumanos/TipoVinculo/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }


            _VS_tvi_id = TipoVinculo.tvi_id;
            _txtNome.Text = TipoVinculo.tvi_nome;
            _txtDescricao.Text = TipoVinculo.tvi_descricao;
            _txtHrsSemanais.Text = Convert.ToString(TipoVinculo.tvi_horasSemanais);
            _txtHrsSemanais.Enabled = false;
            _txtMinAlmoco.Text = Convert.ToString(TipoVinculo.tvi_minutosAlmoco);
            _txtMinAlmoco.Enabled = false;
            _txtHrMinEntrada.Text = Convert.ToString(TipoVinculo.tvi_horarioMinEntrada).Remove(TipoVinculo.tvi_horarioMinEntrada.ToString().Length - 3).Equals("00:00") ? null : Convert.ToString(TipoVinculo.tvi_horarioMinEntrada).Remove(TipoVinculo.tvi_horarioMinEntrada.ToString().Length - 3);
            _txtHrMaxSaida.Text = Convert.ToString(TipoVinculo.tvi_horarioMaxSaida).Remove(TipoVinculo.tvi_horarioMaxSaida.ToString().Length - 3).Equals("00:00") ? null : Convert.ToString(TipoVinculo.tvi_horarioMaxSaida).Remove(TipoVinculo.tvi_horarioMaxSaida.ToString().Length - 3);
            _txtCodigoIntegracao.Text = TipoVinculo.tvi_codIntegracao;
            _ckbBloqueado.Visible = true;
            
            _ckbBloqueado.Checked = !TipoVinculo.tvi_situacao.Equals(1);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o tipo de vínculo.", UtilBO.TipoMensagem.Erro);
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

            if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                _LoadFromEntity(PreviousPage.EditItem);
            else
            {
                _ckbBloqueado.Visible = false;
            }

            Page.Form.DefaultFocus = _txtNome.ClientID;

            bool podeEditarVinculo = ((__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && _VS_tvi_id > 0) ||
                                      (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && _VS_tvi_id <= 0));

            if (!podeEditarVinculo)
            {
                HabilitaControles(fdsVinculo.Controls, false);
                _btnCancelar.Enabled = true;
            }
        }
    }

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/TipoVinculo/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
    
    #endregion

    protected void CvValidaIntervalo_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
    {
        DateTime horaEntrada = Convert.ToDateTime(args.Value);
        DateTime horaSaida = Convert.ToDateTime(_txtHrMaxSaida.Text);
        if (horaEntrada < horaSaida)
        {
            args.IsValid = true;
        }
        else
        {
            args.IsValid = false;
        }
    }
}
