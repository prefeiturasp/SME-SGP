using System;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using System.Data;
using System.Web;

public partial class Configuracao_TipoEvento_Cadastro : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Propriedade em ViewState que armazena valor de tev_id
    /// no caso de atualização de um registro ja existente.
    /// </summary>
    private int _VS_tev_id
    {
        get
        {
            if (ViewState["_VS_tev_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_tev_id"]);
            }
            return -1;
        }
        set
        {
            ViewState["_VS_tev_id"] = value;
        }
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Carrega dados do tipo de evento
    /// </summary>
    /// <param name="tev_id">ID do tipo de evento</param>
    private void _LoadFromEntity(int tev_id)
    {
        try
        {
            ACA_TipoEvento TipoEvento = new ACA_TipoEvento { tev_id = tev_id };
            ACA_TipoEventoBO.GetEntity(TipoEvento);
            _VS_tev_id = TipoEvento.tev_id;
            _txtTipoEvento.Text = TipoEvento.tev_nome;
            _ckbPeriodoCalendario.Checked = TipoEvento.tev_periodoCalendario;
            _rdlLiberacao.SelectedValue = (TipoEvento.tev_liberacao > 0 ? TipoEvento.tev_liberacao : 1).ToString();
            _ckbBloqueado.Checked = !TipoEvento.tev_situacao.Equals(1);
            _ckbBloqueado.Visible = true;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o tipo de evento.", UtilBO.TipoMensagem.Erro);
        }

    }

    /// <summary>
    /// Insere ou altera o tipo de evento
    /// </summary>
    public void Salvar()
    {
        try
        {
            ACA_TipoEvento TipoEvento = new ACA_TipoEvento
            {
                tev_id = _VS_tev_id
                ,
                tev_nome = _txtTipoEvento.Text
                ,
                tev_situacao = (_ckbBloqueado.Checked ? Convert.ToByte(2) : Convert.ToByte(1))
                ,
                tev_dataCriacao = DateTime.Now
                ,
                tev_dataAlteracao = DateTime.Now
                ,
                tev_periodoCalendario = _ckbPeriodoCalendario.Checked
                ,
                tev_liberacao = Convert.ToByte(_rdlLiberacao.SelectedValue)
                ,
                IsNew = (_VS_tev_id > 0) ? false : true
            };

            if (ACA_TipoEventoBO.Save(TipoEvento))
            {
                if (_VS_tev_id <= 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "tev_id: " + TipoEvento.tev_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Tipo de evento incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tev_id: " + TipoEvento.tev_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Tipo de evento alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoEvento/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o tipo de evento.", UtilBO.TipoMensagem.Erro);
            }
        }
        catch (MSTech.Validation.Exceptions.ValidationException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (DuplicateNameException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (ArgumentException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o tipo de evento.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                {
                    _LoadFromEntity(PreviousPage.EditItem);
                    _ckbPeriodoCalendario.Enabled = false;
                }
                else
                {
                    _bntSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                    _ckbBloqueado.Visible = false;
                }

                Page.Form.DefaultFocus = _txtTipoEvento.ClientID;
                Page.Form.DefaultButton = _bntSalvar.UniqueID;
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
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoEvento/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _bntSalvar_Click(object sender, EventArgs e)
    {
        Salvar();
    }

    #endregion
}
