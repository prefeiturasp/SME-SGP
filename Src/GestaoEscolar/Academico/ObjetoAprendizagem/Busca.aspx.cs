using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Academico.ObjetoAprendizagem
{
    public partial class Busca : MotherPageLogado
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
            {
                LoadPage(PreviousPage.tds_id);
            }
            else if(Session["tds_id_oap"] != null)
            {
                LoadPage(Convert.ToInt32(Session["tds_id_oap"]));
                Session["tds_id_oap"] = null;
            }
            else
            {
                Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/ObjetoAprendizagem/BuscaDisciplina.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        private void LoadPage(int tds_id)
        {
            var tds = new ACA_TipoDisciplina { tds_id = tds_id };
            ACA_TipoDisciplinaBO.GetEntity(tds);

            txtDisciplina.Text = tds.tds_nome;

            _grvObjetoAprendizagem.PageIndex = 0;
            _odsObjeto.SelectParameters.Clear();
            _odsObjeto.SelectParameters.Add("tds_id", tds_id.ToString());
            _grvObjetoAprendizagem.DataBind();
        }

        protected void _btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Academico/ObjetoAprendizagem/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void _grvObjetoAprendizagem_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int evt_id = Convert.ToInt32(_grvObjetoAprendizagem.DataKeys[index].Value);

                    ACA_Evento entity = new ACA_Evento { evt_id = evt_id };
                    ACA_EventoBO.GetEntity(entity);

                    if (ACA_EventoBO.Delete(entity))
                    {
                        _grvObjetoAprendizagem.PageIndex = 0;
                        _grvObjetoAprendizagem.DataBind();
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "evt_id: " + evt_id);
                        _lblMessage.Text = UtilBO.GetErroMessage("Evento excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao excluir o evento.", UtilBO.TipoMensagem.Erro);
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

            if ((!string.IsNullOrEmpty(_grvObjetoAprendizagem.SortExpression)) &&
               (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ObjetoAprendizagemDisciplina))
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                if (filtros.ContainsKey("VS_Ordenacao"))
                {
                    filtros["VS_Ordenacao"] = _grvObjetoAprendizagem.SortExpression;
                }
                else
                {
                    filtros.Add("VS_Ordenacao", _grvObjetoAprendizagem.SortExpression);
                }

                if (filtros.ContainsKey("VS_SortDirection"))
                {
                    filtros["VS_SortDirection"] = _grvObjetoAprendizagem.SortDirection.ToString();
                }
                else
                {
                    filtros.Add("VS_SortDirection", _grvObjetoAprendizagem.SortDirection.ToString());
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.ObjetoAprendizagemDisciplina
                    ,
                    Filtros = filtros
                };
            }
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