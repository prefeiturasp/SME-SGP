using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Configuracao.NivelOrientacaoCurricular
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Id do calendário.
        /// </summary>
        private int VS_cal_id
        {
            get
            {
                if (ViewState["VS_cal_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_cal_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_cal_id"] = value;
            }
        }

        /// <summary>
        /// Id do curso.
        /// </summary>
        private int VS_cur_id
        {
            get
            {
                if (ViewState["VS_cur_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_cur_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_cur_id"] = value;
            }
        }

        /// <summary>
        /// Id do curso currículo.
        /// </summary>
        private int VS_crr_id
        {
            get
            {
                if (ViewState["VS_crr_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_crr_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_crr_id"] = value;
            }
        }

        /// <summary>
        /// Id do currículo período.
        /// </summary>
        private int VS_crp_id
        {
            get
            {
                if (ViewState["VS_crp_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_crp_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_crp_id"] = value;
            }
        }

        /// <summary>
        /// ID do tipo de disciplina.
        /// </summary>
        private int VS_tds_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tds_id"] ?? -1);
            }

            set
            {
                ViewState["VS_tds_id"] = value;
            }
        }

        /// <summary>
        /// ID da matriz
        /// </summary>
        private long VS_mat_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_mat_id"] ?? -1);
            }

            set
            {
                ViewState["VS_mat_id"] = value;
            }
        }


        /// <summary>
        /// Ordem do último nível da orientação curricular.
        /// </summary>
        private int VS_ordem_ultimo
        {
            get
            {
                if (ViewState["VS_ordem_ultimo"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_ordem_ultimo"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_ordem_ultimo"] = value;
            }
        }

        /// <summary>
        /// DataTable dos Níveis de Orientação Curricular
        /// </summary>
        private List<sNivelOrientacaoCurricular> VS_Dt_Niveis
        {
            get
            {
                if (ViewState["VS_Dt_Niveis"] != null)
                    return (List<sNivelOrientacaoCurricular>)ViewState["VS_Dt_Niveis"];
                VS_Dt_Niveis = new List<sNivelOrientacaoCurricular>();
                return VS_Dt_Niveis;
            }
            set
            {
                ViewState["VS_Dt_Niveis"] = value;
            }
        }

        /// <summary>
        /// Se tiver dois níveis de orientação curricular, pra salvar os dois, pois é obrigatório salvar pelo menos dois.
        /// </summary>
        private int VS_Aux_Dois_Niveis
        {
            get
            {
                if (ViewState["VS_Aux_Dois_Niveis"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_Aux_Dois_Niveis"]);
                }

                return 0;
            }

            set
            {
                ViewState["VS_Aux_Dois_Niveis"] = value;
            }
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                }

                if ((PreviousPage != null) && PreviousPage.IsCrossPagePostBack)
                {
                    VS_cal_id = PreviousPage.EditItem_cal_id;
                    VS_cur_id = PreviousPage.EditItem_cur_id;
                    VS_crr_id = PreviousPage.EditItem_crr_id;
                    VS_crp_id = PreviousPage.EditItem_crp_id;
                    VS_tds_id = PreviousPage.EditItem_tds_id;
                    VS_mat_id = PreviousPage.EditItem_mat_id;

                    ORC_MatrizHabilidades entMatriz = new ORC_MatrizHabilidades();
                    entMatriz.mat_id = VS_mat_id;
                    ORC_MatrizHabilidadesBO.GetEntity(entMatriz);
                    
                    // Mostra o label com os dados selecionados.
                    lblInformacao.Text = "<b>Calendário escolar: </b>" + PreviousPage.EditItem_calendario + "<br/>";
                    lblInformacao.Text += "<b>" + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + PreviousPage.EditItem_curso + "<br/>";
                    lblInformacao.Text += "<b>" + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": </b>" + PreviousPage.EditItem_grupamento + "<br/>";
                    lblInformacao.Text += "<b>"+GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA")+": </b>" + PreviousPage.EditItem_tipoDisciplina + "<br />";
                    lblInformacao.Text += "<b>Nome da matriz: </b>" + entMatriz.mat_nome + "<br/>";
                    lblInformacao.Text += "<b>Matriz padrão: </b>" + (entMatriz.mat_padrao ? "Sim" : "Não") + "<br/>";

                    CarregaNiveis(false);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a página.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configuracao/NivelOrientacaoCurricular/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void btnIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                if (VS_ordem_ultimo < 0)
                    VS_ordem_ultimo = 1;
                else
                    VS_ordem_ultimo++;

                int indexUltimo = 0;
                int indexEdit = 0;
                string orientacao_curricular = "";

                if (VS_Dt_Niveis.Any())
                {
                    indexUltimo = VS_Dt_Niveis.Count() - 1;

                    // Se o último nível possuir orientação curricular, o novo registro deve ficar em penúltimo, para que o último se matenha em último.
                    orientacao_curricular = VS_Dt_Niveis.LastOrDefault().ocr_id.ToString();
                }

                if (VS_Dt_Niveis.Any() && !string.IsNullOrEmpty(orientacao_curricular)) 
                {
                    // penúltimo
                    sNivelOrientacaoCurricular rowNew = new sNivelOrientacaoCurricular();
                    rowNew.nvl_id = -1;
                    rowNew.cur_id = VS_cur_id;
                    rowNew.crr_id = VS_crr_id;
                    rowNew.crp_id = VS_crp_id;
                    rowNew.cal_id = VS_cal_id;
                    rowNew.tds_id = VS_tds_id;
                    rowNew.nvl_ordem = VS_ordem_ultimo - 1;
                    rowNew.nvl_nome = "";
                    rowNew.nvl_nomePlural = String.Empty;
                    rowNew.nvl_situacao = 0;
                    rowNew.nvl_dataCriacao = DateTime.Now;
                    rowNew.ocr_id = -1;

                    // último
                    sNivelOrientacaoCurricular rowHab = new sNivelOrientacaoCurricular();
                    rowHab.nvl_id = VS_Dt_Niveis.LastOrDefault().nvl_id;
                    rowHab.cur_id = VS_cur_id;
                    rowHab.crr_id = VS_crr_id;
                    rowHab.crp_id = VS_crp_id;
                    rowHab.cal_id = VS_cal_id;
                    rowHab.tds_id = VS_tds_id;
                    rowHab.nvl_ordem = VS_ordem_ultimo;
                    rowHab.nvl_nome = VS_Dt_Niveis.LastOrDefault().nvl_nome;
                    rowHab.nvl_nomePlural = VS_Dt_Niveis.LastOrDefault().nvl_nomePlural;
                    rowHab.nvl_situacao = VS_Dt_Niveis.LastOrDefault().nvl_situacao;
                    rowHab.nvl_dataCriacao = VS_Dt_Niveis.LastOrDefault().nvl_dataCriacao;
                    rowHab.ocr_id = VS_Dt_Niveis.LastOrDefault().ocr_id;

                    VS_Dt_Niveis.Remove(VS_Dt_Niveis.Last());
                    VS_Dt_Niveis.Add(rowNew);
                    VS_Dt_Niveis.Add(rowHab);

                    // Edita penúltima linha.
                    indexEdit = VS_Dt_Niveis.Count() - 2;
                }
                else if (VS_Dt_Niveis.Count() == 0)
                {
                    AdicionaLinhaDataTable();
                    //AdicionaLinhaDataTable();

                    lblObservacao.Text = UtilBO.GetErroMessage("É necessário incluir no mínimo dois níveis curriculares.", UtilBO.TipoMensagem.Informacao);
                    lblObservacao.Visible = true;
                    VS_Aux_Dois_Niveis++;
                    
                }
                else
                {
                    AdicionaLinhaDataTable();

                    // Edita última linha.
                    indexEdit = VS_Dt_Niveis.Count() - 1;
                }                

                lblMsgGrid.Visible = false;

                // Habilita textbox para alterar o nível.
                grvNiveis.EditIndex = indexEdit;
                grvNiveis.DataSource = VS_Dt_Niveis;
                grvNiveis.DataBind();
                grvNiveis.Rows[indexEdit].Focus();

                // Desabilita botões até salvar esse novo registro.
                btnIncluir.Visible = false;
                btnIncluir2.Visible = false;

                EscondeBotoes();                
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar incluir o nível de orientação curricular.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvNiveis_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int nvl_id = Convert.ToInt32(grvNiveis.DataKeys[index].Value);

                    ORC_Nivel entity = new ORC_Nivel { nvl_id = nvl_id };
                    ORC_NivelBO.GetEntity(entity);
                    entity.nvl_situacao = 3;
                    ORC_NivelBO.Save(entity);

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "nvl_id: " + entity.nvl_id);
                    lblMensagem.Text = UtilBO.GetErroMessage("Nível da orientação curricular excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    CarregaNiveis(true);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao excluir o nível da orientação curricular.", UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Subir")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int nvl_idDescer = Convert.ToInt32(grvNiveis.DataKeys[index - 1]["nvl_id"]);
                    int nvl_ordemDescer = Convert.ToInt32(grvNiveis.DataKeys[index]["nvl_ordem"]);
                    int nvl_idSubir = Convert.ToInt32(grvNiveis.DataKeys[index]["nvl_id"]);
                    int nvl_ordemSubir = Convert.ToInt32(grvNiveis.DataKeys[index - 1]["nvl_ordem"]);

                    if (AlteraOrdem(nvl_idDescer, nvl_ordemDescer, nvl_idSubir, nvl_ordemSubir))
                    {
                        CarregaNiveis(true);
                    }

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "nvl_id: " + nvl_idDescer);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "nvl_id: " + nvl_idSubir);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Descer")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int nvl_idDescer = Convert.ToInt32(grvNiveis.DataKeys[index]["nvl_id"]);
                    int nvl_ordemDescer = Convert.ToInt32(grvNiveis.DataKeys[index + 1]["nvl_ordem"]);
                    int nvl_idSubir = Convert.ToInt32(grvNiveis.DataKeys[index + 1]["nvl_id"]);
                    int nvl_ordemSubir = Convert.ToInt32(grvNiveis.DataKeys[index]["nvl_ordem"]);

                    if (AlteraOrdem(nvl_idDescer, nvl_ordemDescer, nvl_idSubir, nvl_ordemSubir))
                    {
                        CarregaNiveis(true);
                    }

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "nvl_id: " + nvl_idDescer);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "nvl_id: " + nvl_idSubir);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void grvNiveis_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grv = ((GridView)sender);
            grv.EditIndex = e.NewEditIndex;
            grv.DataSource = VS_Dt_Niveis;
            grv.DataBind();

            grv.Rows[e.NewEditIndex].Focus();

            // Desabilita botões até salvar esse registro.
            btnIncluir.Visible = false;
            btnIncluir2.Visible = false;

            EscondeBotoes();
        }

        protected void grvNiveis_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnUp = (ImageButton)e.Row.FindControl("btnSubir");
                if (btnUp != null)
                {
                    btnUp.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "cima.png";
                    btnUp.CommandArgument = e.Row.RowIndex.ToString();
                    //btnUp.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;                    
                }

                ImageButton btnDown = (ImageButton)e.Row.FindControl("btnDescer");
                if (btnDown != null)
                {
                    btnDown.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "baixo.png";
                    btnDown.CommandArgument = e.Row.RowIndex.ToString();
                    //btnDown.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }

                ImageButton btnExclui = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExclui != null)
                {
                    btnExclui.CommandArgument = e.Row.RowIndex.ToString();
                    //btnDown.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }

                TextBox txtNomeNivel = (TextBox)e.Row.FindControl("txtNomeNivel");
                if (txtNomeNivel != null)
                {
                    txtNomeNivel.Text = "";
                }

                TextBox txtNomePlural = (TextBox)e.Row.FindControl("txtNomePlural");
                if (txtNomePlural != null)
                {
                    txtNomePlural.Text = "";
                }
            }
        }

        protected void grvNiveis_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView grv = ((GridView)sender);

            // Se estiver cancelando o registro que acabou de ser incluído, exclui do DataTable.
            if (VS_Dt_Niveis[grv.EditIndex].nvl_id <= 0)
            {
                VS_Dt_Niveis.RemoveAt(grv.EditIndex);
            }

            grv.EditIndex = -1;

            if (lblObservacao.Visible)
            {
                lblObservacao.Visible = false;

                VS_Aux_Dois_Niveis = 0;
                VS_Dt_Niveis.Clear();
            }

            if (VS_Dt_Niveis.Any())
            {
                VS_ordem_ultimo = VS_Dt_Niveis.LastOrDefault().nvl_ordem;
            }
            
            CarregaNiveis(true);
        }

        protected void grvNiveis_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string nvl_nome = "";
            string nvl_nomePlural = String.Empty;
            bool novo = false;
            ORC_Nivel entityNivel;

            GridView grv = ((GridView)sender);
            try
            {
                GridViewRow row = grv.Rows[e.RowIndex];

                // Adiciona a segunda linha obrigatória pra poder salvar no mínimo 2 níveis de orientação curricular.
                if (lblObservacao.Visible && VS_Aux_Dois_Niveis == 1)
                {
                    string nome = string.Empty;
                    TextBox txtNome = (TextBox)row.FindControl("txtNomeNivel");
                    if (txtNome != null)
                        nome = txtNome.Text;

                    string nomePlural = string.Empty;
                    TextBox txtNomePlural = (TextBox)row.FindControl("txtNomePlural");
                    if (txtNomePlural != null)
                        nomePlural = txtNomePlural.Text;

                    List<sNivelOrientacaoCurricular> Dt_Niveis_Aux = VS_Dt_Niveis;
                    sNivelOrientacaoCurricular primeiroNivel = VS_Dt_Niveis[0];
                    Dt_Niveis_Aux.RemoveAt(0);

                    //altera o nome da primeira linha que foi adicionada
                    primeiroNivel.nvl_nome = nome;
                    primeiroNivel.nvl_nomePlural = nomePlural;

                    VS_Dt_Niveis.Clear();

                    VS_Dt_Niveis.Add(primeiroNivel);
                    VS_Dt_Niveis.AddRange(Dt_Niveis_Aux);

                    VS_ordem_ultimo++;
                    AdicionaLinhaDataTable();

                    // Habilita textbox para alterar o nível.
                    grvNiveis.EditIndex = 1;
                    grvNiveis.DataSource = VS_Dt_Niveis;
                    grvNiveis.DataBind();
                    grvNiveis.Rows[1].Focus();

                    //esconde todos os botões
                    EscondeBotoes2Niveis();

                    VS_Aux_Dois_Niveis++;
                }
                else if (lblObservacao.Visible && VS_Aux_Dois_Niveis == 2)
                {
                    // salva linha 1
                    entityNivel = new ORC_Nivel
                    {
                        nvl_id = -1,
                        cur_id = VS_cur_id,
                        crr_id = VS_crr_id,
                        crp_id = VS_crp_id,
                        cal_id = VS_cal_id,
                        tds_id = VS_tds_id,
                        mat_id = VS_mat_id,
                        nvl_ordem = 1,
                        nvl_nome = VS_Dt_Niveis[0].nvl_nome,
                        nvl_nomePlural = VS_Dt_Niveis[0].nvl_nomePlural,
                        nvl_dataCriacao = DateTime.Now,
                        nvl_dataAlteracao = DateTime.Now,
                        nvl_situacao = 1,
                        IsNew = true
                    };

                    ORC_NivelBO.Save(entityNivel);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "nvl_id: " + entityNivel.nvl_id);

                    // salva linha 2
                    TextBox txtNome = (TextBox)row.FindControl("txtNomeNivel");
                    if (txtNome != null)
                        nvl_nome = txtNome.Text;

                    TextBox txtNomePlural = (TextBox)row.FindControl("txtNomePlural");
                    if(txtNomePlural != null)
                        nvl_nomePlural = txtNomePlural.Text;

                    entityNivel = new ORC_Nivel
                    {
                        nvl_id = VS_Dt_Niveis[e.RowIndex].nvl_id,
                        cur_id = VS_Dt_Niveis[e.RowIndex].cur_id,
                        crr_id = VS_Dt_Niveis[e.RowIndex].crr_id,
                        crp_id = VS_Dt_Niveis[e.RowIndex].crp_id,
                        cal_id = VS_Dt_Niveis[e.RowIndex].cal_id,
                        tds_id = VS_Dt_Niveis[e.RowIndex].tds_id,
                        mat_id = VS_mat_id,
                        nvl_ordem = VS_Dt_Niveis[e.RowIndex].nvl_ordem,
                        nvl_nome = nvl_nome,
                        nvl_nomePlural = nvl_nomePlural,
                        nvl_dataCriacao = VS_Dt_Niveis[e.RowIndex].nvl_dataCriacao,
                        nvl_dataAlteracao = DateTime.Now,
                        nvl_situacao = 1,
                        IsNew = true
                    };

                    ORC_NivelBO.Save(entityNivel);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "nvl_id: " + entityNivel.nvl_id);

                    lblObservacao.Visible = false;
                    lblMensagem.Text = UtilBO.GetErroMessage("Os dois níveis de orientação curricular foram incluídos com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    grv.EditIndex = -1;

                    CarregaNiveis(true);
                }
                else
                {
                    TextBox txtNome = (TextBox)row.FindControl("txtNomeNivel");
                    if (txtNome != null)
                        nvl_nome = txtNome.Text;

                    TextBox txtNomePlural = (TextBox)row.FindControl("txtNomePlural");
                    if(txtNomePlural != null)
                        nvl_nomePlural = txtNomePlural.Text;

                    entityNivel = new ORC_Nivel
                    {
                        nvl_id = VS_Dt_Niveis[e.RowIndex].nvl_id,
                        cur_id = VS_Dt_Niveis[e.RowIndex].cur_id,
                        crr_id = VS_Dt_Niveis[e.RowIndex].crr_id,
                        crp_id = VS_Dt_Niveis[e.RowIndex].crp_id,
                        cal_id = VS_Dt_Niveis[e.RowIndex].cal_id,
                        tds_id = VS_Dt_Niveis[e.RowIndex].tds_id,
                        mat_id = VS_mat_id,
                        nvl_ordem = VS_Dt_Niveis[e.RowIndex].nvl_ordem,
                        nvl_nome = nvl_nome,
                        nvl_nomePlural = nvl_nomePlural,
                        nvl_dataCriacao = VS_Dt_Niveis[e.RowIndex].nvl_dataCriacao,
                        nvl_dataAlteracao = DateTime.Now,
                        nvl_situacao = 1
                    };

                    if (VS_Dt_Niveis[e.RowIndex].nvl_situacao == 0)
                    {
                        entityNivel.IsNew = true;
                        novo = true;
                    }
                    else
                    {
                        entityNivel.IsNew = false;
                    }

                    ORC_NivelBO.Save(entityNivel);
                    if (novo)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "nvl_id: " + entityNivel.nvl_id);
                        lblMensagem.Text = UtilBO.GetErroMessage("Nível de orientação curricular incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "nvl_id: " + entityNivel.nvl_id);
                        lblMensagem.Text = UtilBO.GetErroMessage("Nível de orientação curricular alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }

                    grv.EditIndex = -1;

                    // Se é um registro novo, e o último possuir orientação curricular, altera a ordem dos dois registros.
                    int indexUltimo = VS_Dt_Niveis.Count() - 1;

                    // Se o último nível possuir orientação curricular, o novo registro deve ficar em penúltimo, para que o último se matenha em último.
                    string orientacao_curricular = VS_Dt_Niveis.LastOrDefault().ocr_id.ToString();

                    if (novo && !string.IsNullOrEmpty(orientacao_curricular))
                    {
                        int nvl_idDescer = Convert.ToInt32(grvNiveis.DataKeys[indexUltimo]["nvl_id"]);
                        int nvl_ordemDescer = VS_Dt_Niveis.Count();
                        int nvl_idSubir = entityNivel.nvl_id;
                        int nvl_ordemSubir = VS_Dt_Niveis.Count() - 1;

                        bool returnAlteracao = AlteraOrdem(nvl_idDescer, nvl_ordemDescer, nvl_idSubir, nvl_ordemSubir);
                    }

                    CarregaNiveis(true);                    
                }                
            }
            catch (MSTech.Validation.Exceptions.ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar salvar nível da orientação curricular.", UtilBO.TipoMensagem.Erro);
            }

        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega os níveis de orientação curricular desse Curso, Currículo, CurrículoPeríodo e Calendário
        /// </summary>
        private void CarregaNiveis(bool habilitaBotoesIncluir)
        {
            try
            {
                ORC_NivelBO.CorrigirOrdem(VS_cur_id, VS_crr_id, VS_crp_id, VS_cal_id, VS_tds_id);

                VS_Dt_Niveis = ORC_NivelBO.SelecionaPorCursoGrupamentoCalendarioTipoDisciplina(VS_cur_id, VS_crr_id, VS_crp_id, VS_cal_id, VS_tds_id, VS_mat_id, 0); //Não colocar cache nessa consulta.

                VS_ordem_ultimo = VS_Dt_Niveis.LastOrDefault().nvl_ordem;

                grvNiveis.DataSource = VS_Dt_Niveis;
                grvNiveis.DataBind();

                if (grvNiveis.Rows.Count == 0)
                {
                    lblMsgGrid.Text = "Não foram encontrados níveis da orientação curricular.";
                    lblMsgGrid.Visible = true;
                }
                else
                {
                    // Esconde botão subir na primeira linha da grid, e botão descer da última linha.
                    ((ImageButton)grvNiveis.Rows[0].FindControl("btnSubir")).Visible = false;
                    ((ImageButton)grvNiveis.Rows[grvNiveis.Rows.Count - 1].FindControl("btnDescer")).Visible = false;

                    // Se o último nível possuir orientação curricular, o novo registro deve ficar em penúltimo, para que o último se matenha em último.
                    string orientacao_curricular = VS_Dt_Niveis[grvNiveis.Rows.Count - 1].ocr_id.ToString();

                    if (!string.IsNullOrEmpty(orientacao_curricular) &&
                        Convert.ToInt32(orientacao_curricular) > 0)
                    {
                        ((ImageButton)grvNiveis.Rows[grvNiveis.Rows.Count - 1].FindControl("btnSubir")).Visible = false;
                        // Esconde botão de editar da última linha.
                        ((ImageButton)grvNiveis.Rows[grvNiveis.Rows.Count - 1].FindControl("btnAlterar")).Visible = false;
                        // Esconde botão de excluir da última linha.
                        ((ImageButton)grvNiveis.Rows[grvNiveis.Rows.Count - 1].FindControl("btnExcluir")).Visible = false;

                        if (grvNiveis.Rows.Count > 1)
                        {
                            // Esconde botão descer da penúltima linha.
                            ((ImageButton)grvNiveis.Rows[grvNiveis.Rows.Count - 2].FindControl("btnDescer")).Visible = false;
                        }
                    }

                    //Se tiver apenas 2 registros não permite excluir nenhum.
                    if (grvNiveis.Rows.Count == 2)
                    {
                        // Esconde botão de excluir das linhas.
                        ((ImageButton)grvNiveis.Rows[0].FindControl("btnExcluir")).Visible = false;
                        ((ImageButton)grvNiveis.Rows[1].FindControl("btnExcluir")).Visible = false;
                    }
                }

                if (habilitaBotoesIncluir)
                {
                    // Habilita botões.
                    btnIncluir.Visible = habilitaBotoesIncluir;
                    btnIncluir2.Visible = habilitaBotoesIncluir;
                }

                updNiveis.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os níveis da orientação curricular.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Esconde botões Subir, Descer, Editar e Excluir
        /// </summary>
        private void EscondeBotoes()
        {
            try
            {
                for (int i = 0; i < grvNiveis.Rows.Count;i++ )
                {
                    ImageButton btnAlterar = ((ImageButton)grvNiveis.Rows[i].FindControl("btnAlterar"));
                    if (btnAlterar != null)
                        ((ImageButton)grvNiveis.Rows[i].FindControl("btnAlterar")).Visible = false;

                    ((ImageButton)grvNiveis.Rows[i].FindControl("btnSubir")).Visible = false;
                    ((ImageButton)grvNiveis.Rows[i].FindControl("btnDescer")).Visible = false;                    
                    ((ImageButton)grvNiveis.Rows[i].FindControl("btnExcluir")).Visible = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os níveis da orientação curricular.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Mostra botões Subir, Descer, Editar e Excluir
        /// </summary>
        private void MostraBotoes()
        {
            try
            {
                for (int i = 0; i < grvNiveis.Rows.Count; i++)
                {
                    ImageButton btnAlterar = ((ImageButton)grvNiveis.Rows[i].FindControl("btnAlterar"));
                    if (btnAlterar != null)
                        ((ImageButton)grvNiveis.Rows[i].FindControl("btnAlterar")).Style.Add("visibility", "visible");

                    ((ImageButton)grvNiveis.Rows[i].FindControl("btnSubir")).Style.Add("visibility", "visible");
                    ((ImageButton)grvNiveis.Rows[i].FindControl("btnDescer")).Style.Add("visibility", "visible");
                    ((ImageButton)grvNiveis.Rows[i].FindControl("btnExcluir")).Style.Add("visibility", "visible");
                }

                if (grvNiveis.Rows.Count > 0)
                {
                    // Esconde botão subir na primeira linha da grid, e botão descer da última linha.
                    ((ImageButton)grvNiveis.Rows[0].FindControl("btnSubir")).Visible = false;
                    ((ImageButton)grvNiveis.Rows[grvNiveis.Rows.Count - 1].FindControl("btnDescer")).Visible = false;

                    // Se o último nível for Habilidades, esconde botão subir.
                    string nome_ultimo_nivel = VS_Dt_Niveis.LastOrDefault().nvl_nome;

                    if ((nome_ultimo_nivel.Length >= 10 && nome_ultimo_nivel.ToLower().Substring(0, 10).Equals("habilidade")) ||
                        nome_ultimo_nivel.Length >= 11 && nome_ultimo_nivel.ToLower().Substring(0, 11).Equals("habilidades"))
                    {
                        ((ImageButton)grvNiveis.Rows[grvNiveis.Rows.Count - 1].FindControl("btnSubir")).Visible = false;
                        // Esconde botão descer da penúltima linha.
                        ((ImageButton)grvNiveis.Rows[grvNiveis.Rows.Count - 2].FindControl("btnDescer")).Visible = false;
                    }
                }               

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os níveis da orientação curricular.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Altera ordem dos níveis.
        /// </summary>
        private bool AlteraOrdem(int nvl_idDescer, int nvl_ordemDescer, int nvl_idSubir, int nvl_ordemSubir)
        {
            ORC_Nivel entityDescer = new ORC_Nivel { nvl_id = nvl_idDescer };
            ORC_NivelBO.GetEntity(entityDescer);
            entityDescer.nvl_ordem = nvl_ordemDescer;
            entityDescer.nvl_dataAlteracao = DateTime.Now;

            ORC_Nivel entitySubir = new ORC_Nivel { nvl_id = nvl_idSubir };
            ORC_NivelBO.GetEntity(entitySubir);
            entitySubir.nvl_ordem = nvl_ordemSubir;
            entitySubir.nvl_dataAlteracao = DateTime.Now;

            return ORC_NivelBO.SaveOrdem(entityDescer, entitySubir);
        }

        private void AdicionaLinhaDataTable()
        {
            sNivelOrientacaoCurricular row = new sNivelOrientacaoCurricular();
            row.nvl_id = -1;
            row.cur_id = VS_cur_id;
            row.crr_id = VS_crr_id;
            row.crp_id = VS_crp_id;
            row.cal_id = VS_cal_id;
            row.tds_id = VS_tds_id;
            row.nvl_ordem = VS_ordem_ultimo;
            row.nvl_nome = "";
            row.nvl_nomePlural = String.Empty;
            row.nvl_situacao = 0;
            row.nvl_dataCriacao = DateTime.Now;
            row.ocr_id = -1;
            VS_Dt_Niveis.Add(row);
        }

        /// <summary>
        /// Mostra botões Subir, Descer, Editar e Excluir
        /// </summary>
        private void EscondeBotoes2Niveis()
        {
            // esconde da primeira linha
            ImageButton btnAlterar = ((ImageButton)grvNiveis.Rows[0].FindControl("btnAlterar"));
            if (btnAlterar != null)
                ((ImageButton)grvNiveis.Rows[0].FindControl("btnAlterar")).Visible = false;

            ((ImageButton)grvNiveis.Rows[0].FindControl("btnSubir")).Visible = false;
            ((ImageButton)grvNiveis.Rows[0].FindControl("btnDescer")).Visible = false;
            ((ImageButton)grvNiveis.Rows[0].FindControl("btnExcluir")).Visible = false;

            // esconde da segunda linha
            ((ImageButton)grvNiveis.Rows[1].FindControl("btnSubir")).Visible = false;
            ((ImageButton)grvNiveis.Rows[1].FindControl("btnDescer")).Visible = false;
            ((ImageButton)grvNiveis.Rows[1].FindControl("btnExcluir")).Visible = false;
        }

        #endregion
    }
}