using System;
using System.Data;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using System.Web;

public partial class Academico_RecursosHumanos_Funcao_Cadastro : MotherPageLogado
{   
    #region PROPRIEDADES

    private int _VS_fun_id
    {
        get
        {
            if (ViewState["_VS_fun_id"] != null)
                return Convert.ToInt32(ViewState["_VS_fun_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_fun_id"] = value;
        }
    }
    
    #endregion

    #region METODOS

    /// <summary>
    /// Salva as informações da Função
    /// </summary>
    private void _Salvar()
    {
        try
        {
            RHU_Funcao _Funcao = new RHU_Funcao
            {
                fun_id = _VS_fun_id
                ,
                fun_codigo = _txtCodigo.Text
                ,
                fun_nome = _txtFuncao.Text
                ,
                fun_descricao = _txtDescricao.Text
                ,
                fun_codIntegracao = _txtCodIntegracao.Text
                ,
                ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id
                ,
                fun_situacao = Convert.ToByte(_ckbBloqueado.Checked ? 2 :1)
                ,
                pgs_chave = UCComboParametroGrupoPerfil1.Valor == "-1" ? string.Empty : UCComboParametroGrupoPerfil1.Valor
                ,
                IsNew = (_VS_fun_id > 0) ? false : true
            };
            if(RHU_FuncaoBO.Save(_Funcao))
            {
                if (_VS_fun_id <= 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "fun_id: " + _Funcao.fun_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Função incluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "fun_id: " + _Funcao.fun_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Função alterada com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                Response.Redirect("Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
            else
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a função.", UtilBO.TipoMensagem.Erro);
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
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a função.", UtilBO.TipoMensagem.Erro);
        }  
    }

    /// <summary>
    /// Carrega as informações da função informada.
    /// </summary>
    /// <param name="fun_id">id da função</param>
    private void _Carregar(int fun_id)
    {
        try
        {
            RHU_Funcao _Funcao = new RHU_Funcao {fun_id = fun_id};
            RHU_FuncaoBO.GetEntity(_Funcao);

            if (_Funcao.ent_id != __SessionWEB.__UsuarioWEB.Usuario.ent_id)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("A função não pertence à entidade na qual você está logado.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            _VS_fun_id = _Funcao.fun_id;
            _txtCodigo.Text = _Funcao.fun_codigo;
            _txtFuncao.Text = _Funcao.fun_nome;
            _txtDescricao.Text = _Funcao.fun_descricao;
            _txtCodIntegracao.Text = _Funcao.fun_codIntegracao;

            if (!string.IsNullOrEmpty(_Funcao.pgs_chave))
            {
                if (UCComboParametroGrupoPerfil1.ExisteItem(_Funcao.pgs_chave))
                    UCComboParametroGrupoPerfil1.Valor = _Funcao.pgs_chave;
            }

            _ckbBloqueado.Checked = !_Funcao.fun_situacao.Equals(1);

        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a função.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion

    #region EVENTOS

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                UCComboParametroGrupoPerfil1.CarregarGrupoPadrao();
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
                _btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }

            Page.Form.DefaultFocus = _txtFuncao.ClientID;
            Page.Form.DefaultButton = _btnSalvar.UniqueID;
        }
    }

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _btnSalvar_Click(object sender, EventArgs e)
    {
        _Salvar();
    }

    #endregion
}
