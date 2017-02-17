using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Configuracao.AreaConhecimento
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Recebe o Id para enviar os dados para edição.
        /// </summary>
        public int EditItem
        {
            get
            {
                return Convert.ToInt32(_dgvAreaConhecimento.DataKeys[_dgvAreaConhecimento.EditIndex].Value);
            }
        }

        #endregion

        #region Eventos Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            }

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    _lblMessage.Text = message;

                bool controlarOrdem = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                _dgvAreaConhecimento.Columns[1].Visible = controlarOrdem;

                if (_dgvAreaConhecimento.Rows.Count > 0)
                {
                    ((ImageButton)_dgvAreaConhecimento.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                    ((ImageButton)_dgvAreaConhecimento.Rows[_dgvAreaConhecimento.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                }

                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                _dgvAreaConhecimento.PageSize = itensPagina;
                _dgvAreaConhecimento.DataBind();

                _dgvAreaConhecimento.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                UCTotalRegistros1.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                _btnNovoAreaConhecimento.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }
        }

        #endregion

        #region Eventos

        protected void _dgvAreaConhecimento_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = ACA_AreaConhecimentoBO.GetTotalRecords();

            if (_dgvAreaConhecimento.Rows.Count > 0)
            {
                ((ImageButton)_dgvAreaConhecimento.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                ((ImageButton)_dgvAreaConhecimento.Rows[_dgvAreaConhecimento.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
            }
        }

        protected void odsAreaConhecimento_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
                e.InputParameters.Clear();
        }

        protected void _dgvAreaConhecimento_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int aco_id = Convert.ToInt32(_dgvAreaConhecimento.DataKeys[index].Value.ToString());

                    ACA_AreaConhecimento entity = new ACA_AreaConhecimento { aco_id = aco_id };
                    ACA_AreaConhecimentoBO.GetEntity(entity);

                    if (ACA_AreaConhecimentoBO.Delete(entity))
                    {
                        _dgvAreaConhecimento.DataBind();

                        if (_dgvAreaConhecimento.Rows.Count > 0)
                        {
                            ((ImageButton)_dgvAreaConhecimento.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                            ((ImageButton)_dgvAreaConhecimento.Rows[_dgvAreaConhecimento.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                        }

                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "aco_id: " + aco_id);
                        _lblMessage.Text = UtilBO.GetErroMessage(
                                            "Área de conhecimento excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir área de conhecimento.", UtilBO.TipoMensagem.Erro);
                }
            }
            if (e.CommandName == "Subir")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int aco_idDescer = Convert.ToInt32(_dgvAreaConhecimento.DataKeys[index - 1]["aco_id"]);
                    int aco_ordemDescer = Convert.ToInt32(_dgvAreaConhecimento.DataKeys[index]["aco_ordem"]);
                    ACA_AreaConhecimento entityDescer = new ACA_AreaConhecimento { aco_id = aco_idDescer };
                    ACA_AreaConhecimentoBO.GetEntity(entityDescer);
                    entityDescer.aco_ordem = aco_ordemDescer;

                    int aco_idSubir = Convert.ToInt32(_dgvAreaConhecimento.DataKeys[index]["aco_id"]);
                    int aco_ordemSubir = Convert.ToInt32(_dgvAreaConhecimento.DataKeys[index - 1]["aco_ordem"]);
                    ACA_AreaConhecimento entitySubir = new ACA_AreaConhecimento { aco_id = aco_idSubir };
                    ACA_AreaConhecimentoBO.GetEntity(entitySubir);
                    entitySubir.aco_ordem = aco_ordemSubir;

                    if (ACA_AreaConhecimentoBO.SaveOrdem(entityDescer, entitySubir))
                    {
                        _dgvAreaConhecimento.DataBind();


                        if (_dgvAreaConhecimento.Rows.Count > 0)
                        {
                            ((ImageButton)_dgvAreaConhecimento.Rows[0].Cells[2].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                            ((ImageButton)_dgvAreaConhecimento.Rows[_dgvAreaConhecimento.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                        }
                    }

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "aco_id: " + aco_idSubir);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "aco_id: " + aco_idDescer);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }

            if (e.CommandName == "Descer")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int aco_idDescer = Convert.ToInt32(_dgvAreaConhecimento.DataKeys[index]["aco_id"]);
                    int aco_ordemDescer = Convert.ToInt32(_dgvAreaConhecimento.DataKeys[index + 1]["aco_ordem"]);
                    ACA_AreaConhecimento entityDescer = new ACA_AreaConhecimento { aco_id = aco_idDescer };
                    ACA_AreaConhecimentoBO.GetEntity(entityDescer);
                    entityDescer.aco_ordem = aco_ordemDescer;

                    int aco_idSubir = Convert.ToInt32(_dgvAreaConhecimento.DataKeys[index + 1]["aco_id"]);
                    int aco_ordemSubir = Convert.ToInt32(_dgvAreaConhecimento.DataKeys[index]["aco_ordem"]);
                    ACA_AreaConhecimento entitySubir = new ACA_AreaConhecimento { aco_id = aco_idSubir };
                    ACA_AreaConhecimentoBO.GetEntity(entitySubir);
                    entitySubir.aco_ordem = aco_ordemSubir;

                    if (ACA_AreaConhecimentoBO.SaveOrdem(entityDescer, entitySubir))
                    {
                        _dgvAreaConhecimento.DataBind();

                        if (_dgvAreaConhecimento.Rows.Count > 0)
                        {
                            ((ImageButton)_dgvAreaConhecimento.Rows[0].Cells[2].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                            ((ImageButton)_dgvAreaConhecimento.Rows[_dgvAreaConhecimento.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                        }
                    }

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "aco_id: " + aco_idSubir);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "aco_id: " + aco_idDescer);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }

        }

        protected void _dgvAreaConhecimento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label _lblAlterar = (Label)e.Row.FindControl("_lblAlterar");
                if (_lblAlterar != null)
                {
                    _lblAlterar.Visible = !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }

                LinkButton _btnAlterar = (LinkButton)e.Row.FindControl("_btnAlterar");
                if (_btnAlterar != null)
                {
                    _btnAlterar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
                ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");
                if (_btnExcluir != null)
                {
                    _btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                    _btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }
                ImageButton _btnSubir = (ImageButton)e.Row.FindControl("_btnSubir");
                if (_btnSubir != null)
                {
                    _btnSubir.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "cima.png";
                    _btnSubir.CommandArgument = e.Row.RowIndex.ToString();
                    _btnSubir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }

                ImageButton _btnDescer = (ImageButton)e.Row.FindControl("_btnDescer");
                if (_btnDescer != null)
                {
                    _btnDescer.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "baixo.png";
                    _btnDescer.CommandArgument = e.Row.RowIndex.ToString();
                    _btnDescer.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
            }
        }

        protected void _btnNovoAreaConhecimento_Click(object sender, EventArgs e)
        {
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/AreaConhecimento/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion
    }
}