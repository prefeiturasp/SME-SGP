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

namespace GestaoEscolar.Configuracao.TipoCiclo
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades
        
        /// <summary>
        /// Propriedade em ViewState que armazena valor de atq_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_tci_id
        {
            get
            {
                if (ViewState["VS_tci_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_tci_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_tci_id"] = value;
            }
        }
        
        #endregion Propriedades

        #region Delegates

        protected void UCComboQtdePaginacao_IndexChanged()
        {            
            // atribui nova quantidade itens por página para o grid
            grvTpCiclo.PageSize = UCComboQtdePaginacao1.Valor;
            grvTpCiclo.PageIndex = 0;            
            // atualiza o grid
            grvTpCiclo.DataBind();
            //faz com que apareça somente uma seta (para cima ou para baixo), qdo for primeiro ou último registro.
            if (grvTpCiclo.Rows.Count > 0)
            {
                ((ImageButton)grvTpCiclo.Rows[0].Cells[2].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                ((ImageButton)grvTpCiclo.Rows[grvTpCiclo.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
            }
        }

        #endregion Delegates

        #region Eventos

        protected void chkObjetoAprendizagem_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gr = (GridViewRow)chk.Parent.Parent;
            var tci_id = grvTpCiclo.DataKeys[gr.RowIndex].Value.ToString();

            ACA_TipoCicloBO.AtualizaObjetoAprendizagem(int.Parse(tci_id), chk.Checked);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            }

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                grvTpCiclo.PageSize = ApplicationWEB._Paginacao;

                try
                {
                    if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), fdsResultados.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsResultados.ClientID)), true);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
                
                Pesquisar();

                if (grvTpCiclo.Rows.Count > 0)
                {
                    ((ImageButton)grvTpCiclo.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                    ((ImageButton)grvTpCiclo.Rows[grvTpCiclo.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                }
            }
        }
        
        protected void grvTpCiclo_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = ACA_TipoCicloBO.GetTotalRecords();

            // seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(grvTpCiclo);
        }

        protected void grvTpCiclo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
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

                HiddenField field = e.Row.FindControl("hdfObjetoAprendizagem") as HiddenField;
                CheckBox chkObjetoAprendizagem = (CheckBox)e.Row.FindControl("chkObjetoAprendizagem");
                if(field != null && chkObjetoAprendizagem != null)
                {
                    var check = Convert.ToBoolean(Convert.ToInt16(field.Value));

                    chkObjetoAprendizagem.Checked = check;
                }
            }
        }

        protected void grvTpCiclo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Subir")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int tci_idDescer = Convert.ToInt32(grvTpCiclo.DataKeys[index - 1]["tci_id"]);
                    int tci_ordemDescer = Convert.ToInt32(grvTpCiclo.DataKeys[index]["tci_ordem"]);
                    ACA_TipoCiclo entityDescer = new ACA_TipoCiclo { tci_id = tci_idDescer };
                    ACA_TipoCicloBO.GetEntity(entityDescer);
                    entityDescer.tci_ordem = tci_ordemDescer;

                    int tes_idSubir = Convert.ToInt32(grvTpCiclo.DataKeys[index]["tci_id"]);
                    int tes_ordemSubir = Convert.ToInt32(grvTpCiclo.DataKeys[index - 1]["tci_ordem"]);
                    ACA_TipoCiclo entitySubir = new ACA_TipoCiclo { tci_id = tes_idSubir };
                    ACA_TipoCicloBO.GetEntity(entitySubir);
                    entitySubir.tci_ordem = tes_ordemSubir;

                    if (ACA_TipoCicloBO.SaveOrdem(entityDescer, entitySubir))
                    {
                        odsTpCiclo.DataBind();
                        grvTpCiclo.PageIndex = 0;
                        grvTpCiclo.DataBind();
                        updResultado.Update();

                        if (grvTpCiclo.Rows.Count > 0)
                        {
                            ((ImageButton)grvTpCiclo.Rows[0].Cells[2].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                            ((ImageButton)grvTpCiclo.Rows[grvTpCiclo.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                        }
                    }

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tes_id: " + tes_idSubir);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tes_id: " + tci_idDescer);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }

            if (e.CommandName == "Descer")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int tci_idDescer = Convert.ToInt32(grvTpCiclo.DataKeys[index]["tci_id"]);
                    int tci_ordemDescer = Convert.ToInt32(grvTpCiclo.DataKeys[index + 1]["tci_ordem"]);
                    ACA_TipoCiclo entityDescer = new ACA_TipoCiclo { tci_id = tci_idDescer };
                    ACA_TipoCicloBO.GetEntity(entityDescer);
                    entityDescer.tci_ordem = tci_ordemDescer;

                    int tes_idSubir = Convert.ToInt32(grvTpCiclo.DataKeys[index + 1]["tci_id"]);
                    int tes_ordemSubir = Convert.ToInt32(grvTpCiclo.DataKeys[index]["tci_ordem"]);
                    ACA_TipoCiclo entitySubir = new ACA_TipoCiclo { tci_id = tes_idSubir };
                    ACA_TipoCicloBO.GetEntity(entitySubir);
                    entitySubir.tci_ordem = tes_ordemSubir;

                    if (ACA_TipoCicloBO.SaveOrdem(entityDescer, entitySubir))
                    {
                        odsTpCiclo.DataBind();
                        grvTpCiclo.PageIndex = 0;
                        grvTpCiclo.DataBind();
                        updResultado.Update();

                        if (grvTpCiclo.Rows.Count > 0)
                        {
                            ((ImageButton)grvTpCiclo.Rows[0].Cells[2].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                            ((ImageButton)grvTpCiclo.Rows[grvTpCiclo.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                        }
                    }

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tes_id: " + tes_idSubir);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tes_id: " + tci_idDescer);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void odsTipoCiclo_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
                e.InputParameters.Clear();
        }  

        #endregion Eventos

        #region Métodos

        /// <summary>
        /// Realiza a consulta pelos filtros informados.
        /// </summary>
        private void Pesquisar()
        {
            try
            {
                fdsResultados.Visible = true;

                Dictionary<string, string> filtros = new Dictionary<string, string>();

                grvTpCiclo.PageIndex = 0;
                odsTpCiclo.SelectParameters.Clear();
                odsTpCiclo.DataBind();

                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;
                // atribui essa quantidade para o grid
                grvTpCiclo.PageSize = itensPagina;
                // atualiza o grid
                grvTpCiclo.DataBind();

                updResultado.Update(); 
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar tipos de ciclo.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion
    }
}