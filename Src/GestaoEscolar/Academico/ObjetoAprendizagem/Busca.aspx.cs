using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Academico.ObjetoAprendizagem
{
    public partial class Busca : MotherPageLogado
    {
        public int oap_id
        {
            get
            {
                if(_grvObjetoAprendizagem.EditIndex >= 0)
                    return Convert.ToInt32(_grvObjetoAprendizagem.DataKeys[_grvObjetoAprendizagem.EditIndex].Value);

                return -1;
            }
        }

        public int tds_id
        {
            get
            {
                return _VS_tds_id;
            }
        }

        private int _VS_tds_id
        {
            get
            {
                if (ViewState["_VS_tds_id"] != null)
                {
                    return Convert.ToInt32(ViewState["_VS_tds_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["_VS_tds_id"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                }

                string message = __SessionWEB.PostMessages;
                if (!string.IsNullOrEmpty(message))
                    _lblMessage.Text = message;

                if (!IsPostBack)
                {
                    _grvObjetoAprendizagem.EmptyDataText = string.Format("Não existe objeto de aprendizagem associado a este {0}.", GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL_MIN"));

                    if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                    {
                        Session["tds_id_oap"] = PreviousPage.tds_id;
                        LoadPage(PreviousPage.tds_id);
                    }
                    else if (Session["tds_id_oap"] != null)
                    {
                        LoadPage(Convert.ToInt32(Session["tds_id_oap"]));
                        Session["tds_id_oap"] = null;
                    }
                    else
                    {
                        _odsObjeto.SelectParameters.Add("tds_id", string.Empty);
                        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/ObjetoAprendizagem/BuscaDisciplina.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void LoadPage(int tds_id)
        {
            try
            {
                _VS_tds_id = tds_id;
                var tds = new ACA_TipoDisciplina { tds_id = tds_id };
                ACA_TipoDisciplinaBO.GetEntity(tds);

                txtDisciplina.Text = tds.tds_nome;

                _grvObjetoAprendizagem.PageIndex = 0;
                _odsObjeto.SelectParameters.Clear();
                _odsObjeto.SelectParameters.Add("tds_id", tds_id.ToString());
                _grvObjetoAprendizagem.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar página.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void _grvObjetoAprendizagem_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int id = Convert.ToInt32(_grvObjetoAprendizagem.DataKeys[index].Value);

                    var entity = new ACA_ObjetoAprendizagem { oap_id = id };
                    ACA_ObjetoAprendizagemBO.GetEntity(entity);


                    if (ACA_ObjetoAprendizagemBO.Delete(entity))
                    {
                        _grvObjetoAprendizagem.PageIndex = 0;
                        _grvObjetoAprendizagem.DataBind();
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "oap_id: " + id);
                        _lblMessage.Text = UtilBO.GetErroMessage("Objeto de aprendizagem excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                        _updDadosBasicos.Update();
                    }
                }
                catch (ValidationException ex)
                {
                    _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao excluir o objeto de aprendizagem.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void _grvObjetoAprendizagem_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");
                if (_btnExcluir != null)
                {
                    _btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                    _btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }

        protected void _grvObjetoAprendizagem_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = ACA_ObjetoAprendizagemBO.GetTotalRecords();
            // Seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(_grvObjetoAprendizagem);
        }

        protected void UCComboQtdePaginacao1_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            _grvObjetoAprendizagem.PageSize = UCComboQtdePaginacao1.Valor;
            _grvObjetoAprendizagem.PageIndex = 0;
            // atualiza o grid
            _grvObjetoAprendizagem.DataBind();
        }

        protected void _btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Academico/ObjetoAprendizagem/BuscaDisciplina.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}