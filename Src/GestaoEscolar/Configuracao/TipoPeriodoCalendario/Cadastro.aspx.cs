using System;
using System.Web;
using MSTech.GestaoEscolar.Entities ;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using System.Data;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class Configuracao_PeriodoCalendario_Cadastro : MotherPageLogado
{
    
    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!IsPostBack)
        {
            _ckbBloqueado.Visible = false;

            if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                _LoadFromEntity(PreviousPage.EditItem);
            else
                _btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;

            Page.Form.DefaultFocus = _txtTipoPeriodoCalendario.ClientID;
            Page.Form.DefaultButton = _btnSalvar.UniqueID;            
        }
    }

    #endregion

    #region PROPRIEDADES

    /// <summary>
    /// Propriedade em ViewState que armazena valor de tpc_id (ID de tipo de periodo calendario)
    /// no caso de atualização de um registro ja existente.
    /// </summary>
    private int _VS_tpc_id
    {
        get
        {
            if (ViewState["_VS_tpc_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_tpc_id"]);
            }
            return -1;
        }
        set
        {
            ViewState["_VS_tpc_id"] = value;
        }
    }

    /// <summary>
    /// Propriedade em ViewState que armazena valor de tipo de ordem de período contrato
    /// </summary>
    private int _VS_tpc_ordem
    {
        get
        {
            if (ViewState["_VS_tpc_ordem"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_tpc_ordem"]);
            }
            return -1;
        }
        set
        {
            ViewState["_VS_tpc_ordem"] = value;
        }
    }

    #endregion

    #region METODOS

    /// <summary>
    /// Carrega os dados de tipo de período calendáro
    /// </summary>
    /// <param name="tne_id">Id de tipo de período calendário</param>
    private void _LoadFromEntity(int tpc_id)
    {
        try
        {
            ACA_TipoPeriodoCalendario tipoPeriodoCalendario = new ACA_TipoPeriodoCalendario { tpc_id = tpc_id };
            ACA_TipoPeriodoCalendarioBO.GetEntity(tipoPeriodoCalendario);
            _VS_tpc_id = tipoPeriodoCalendario.tpc_id;
            _VS_tpc_ordem = tipoPeriodoCalendario.tpc_ordem;
            _txtTipoPeriodoCalendario.Text = tipoPeriodoCalendario.tpc_nome;
            _txtTipoPeriodoCalendarioAbreviado.Text = tipoPeriodoCalendario.tpc_nomeAbreviado;
            _ckbForaPeriodoLetivo.Checked = tipoPeriodoCalendario.tpc_foraPeriodoLetivo;
            _ckbBloqueado.Checked = !tipoPeriodoCalendario.tpc_situacao.Equals(1);
            _ckbBloqueado.Visible = true;
            _ckbForaPeriodoLetivo.Enabled = !GestaoEscolarUtilBO.VerificarIntegridade("tpc_id", _VS_tpc_id.ToString(), "ACA_TipoPeriodoCalendario,REL_AlunosSituacaoFechamento", null);
        }
        catch (Exception e)
        {
            ApplicationWEB._GravaErro(e);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o tipo de período do calendário.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Insere ou altera o tipo de periodo calendário
    /// </summary>
    public void Salvar()
    {
        try
        {
            ACA_TipoPeriodoCalendario tipoPeriodoCalendario = new ACA_TipoPeriodoCalendario
            {
                tpc_id = _VS_tpc_id
                ,
                tpc_nome = _txtTipoPeriodoCalendario.Text
                ,
                tpc_nomeAbreviado = _txtTipoPeriodoCalendarioAbreviado.Text
                ,
                tpc_ordem = _VS_tpc_ordem
                ,
                tpc_situacao = (_ckbBloqueado.Checked ? Convert.ToByte(2) : Convert.ToByte(1))
                ,
                tpc_foraPeriodoLetivo = _ckbForaPeriodoLetivo.Checked
                ,
                IsNew = (_VS_tpc_id > 0) ? false : true
            };

            if (ACA_TipoPeriodoCalendarioBO.Save(tipoPeriodoCalendario))
            {
                if (_VS_tpc_id <= 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "tpc_id: " + tipoPeriodoCalendario.tpc_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Tipo de período do calendário incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tpc_id: " + tipoPeriodoCalendario.tpc_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Tipo de período do calendário alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoPeriodoCalendario/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o tipo período do calendário.", UtilBO.TipoMensagem.Erro);
            }
        }
        catch (MSTech.Validation.Exceptions.ValidationException e)
        {            
            _lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (DuplicateNameException e)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (ArgumentException e)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception e)
        {
            ApplicationWEB._GravaErro(e);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o tipo período do calendário.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion

    #region EVENTOS

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoPeriodoCalendario/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _btnSalvar_Click(object sender, EventArgs e)
    {
        Salvar();
    }

    #endregion
}
