using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using System.Data;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using System.Web;
using System.Reflection;

public partial class Academico_Calendario_Anual_Cadastro : MotherPageLogado
{
    #region Constantes

    /// <summary>
    /// Posição da coluna excluir e adicionar do grid de períodos
    /// </summary>
    private const int posExcluirAdicionarPeriodo = 5;

    #endregion

    #region Propriedades

    /// <summary>
    /// Retorna e seta o ID do calendário 
    /// </summary>
    private int _VS_cal_id
    {
        get
        {
            if (ViewState["_VS_cal_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_cal_id"]);
            }
            return -1;
        }
        set
        {
            ViewState["_VS_cal_id"] = value;
        }
    }

    #endregion

    #region Métodos

    #region Load/Save

    /// <summary>
    /// Carrega o calendario selecionado nos campos da tela.
    /// </summary>
    /// <param name="cal_id">The cal_id.</param>
    private void _LoadFromEntity(int cal_id)
    {
        try
        {
            ACA_CalendarioAnual _calendarioAnual = new ACA_CalendarioAnual { cal_id = cal_id };
            ACA_CalendarioAnualBO.GetEntity(_calendarioAnual);

            if (_calendarioAnual.ent_id != __SessionWEB.__UsuarioWEB.Usuario.ent_id)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("O calendário não pertence à entidade na qual você está logado.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            _VS_cal_id = _calendarioAnual.cal_id;            
            _txtAno.Text = Convert.ToString(_calendarioAnual.cal_ano);
            _txtAno.Enabled = false;
            _txtDescricao.Text = _calendarioAnual.cal_descricao;
            _txtDataInicio.Text = _calendarioAnual.cal_dataInicio.ToString("dd/MM/yyyy");
            _txtDataFim.Text = _calendarioAnual.cal_dataFim.ToString("dd/MM/yyyy");
            ckbPermiteRecesso.Checked = _calendarioAnual.cal_permiteLancamentoRecesso;

            CarregaPeriodo();

            _dgvCalendarioPeriodo.Columns[posExcluirAdicionarPeriodo].Visible = !(ACA_CalendarioAnualBO.VerificaTurmaPejaExistente(_VS_cal_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o calendário escolar.", UtilBO.TipoMensagem.Erro);
        }

    }

    /// <summary>
    /// Salva o calendário anual
    /// </summary>
    public void Salvar()
    {
        
        try
        {
            DataTable CalendarioPeriodico = new DataTable();
            int valida = ValidaCampos();
            if (valida != 1)
            {
                if (valida == 0)
                    CalendarioPeriodico = dtCalendarioPeriodo(listCalendarioPeriodo());

                ACA_CalendarioAnual _calendarioAnual = new ACA_CalendarioAnual
                {
                    ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id
                    ,
                    cal_id = _VS_cal_id
                    ,
                    cal_padrao = true
                    ,
                    cal_ano = Convert.ToInt32(_txtAno.Text)
                    ,
                    cal_descricao = _txtDescricao.Text
                    ,
                    cal_dataInicio = Convert.ToDateTime(_txtDataInicio.Text)
                    ,
                    cal_dataFim = Convert.ToDateTime(_txtDataFim.Text)
                    ,
                    cal_permiteLancamentoRecesso = ckbPermiteRecesso.Checked
                    ,
                    cal_situacao = 1
                    ,
                    IsNew = (_VS_cal_id > 0) ? false : true
                };

                List<ACA_CalendarioCurso> ListaCursoCurriculo = CriarListaCalendarioCurso();

                if (ACA_CalendarioAnualBO.Save(_calendarioAnual, CalendarioPeriodico, ListaCursoCurriculo, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    if (_VS_cal_id <= 0)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "cal_id: " + _calendarioAnual.cal_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Calendário escolar incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "cal_id: " + _calendarioAnual.cal_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Calendário escolar alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }

                    Response.Redirect("Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o calendário escolar.", UtilBO.TipoMensagem.Erro);
                }
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
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o calendário escolar.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion

    #region Períodos

    /// <summary>
    /// Atualiza o grid
    /// </summary>m
    private void AtualizaGrid(List<ACA_CalendarioPeriodo> list)
    {
        _dgvCalendarioPeriodo.DataSource = list;
        _dgvCalendarioPeriodo.DataBind();
        if (list.Count <= 0)
            AdicionaItemGrid();
    }

    /// <summary>
    /// Cria uma lista de períodos do calendário
    /// </summary>
    /// <returns>List de períodos do calendário</returns>
    private List<ACA_CalendarioPeriodo> listCalendarioPeriodo()
    {
        List<ACA_CalendarioPeriodo> list = new List<ACA_CalendarioPeriodo>();

        try
        {
            foreach (GridViewRow row in _dgvCalendarioPeriodo.Rows)
            {
                ACA_CalendarioPeriodo campos = new ACA_CalendarioPeriodo();

                string cap_dataInicio = ((TextBox)row.FindControl("txtInicioPeriodo")).Text
                        , cap_dataFim = ((TextBox)row.FindControl("txtFimPeriodo")).Text
                        , cap_descricao = ((TextBox)row.FindControl("txtDescricao")).Text
                        , cap = ((Label)row.FindControl("lblCap_id")).Text;

                int cap_id = string.IsNullOrEmpty(cap) ? 0 : Convert.ToInt32(cap)
                    , tpc_id = Convert.ToInt32(((DropDownList)row.FindControl("ddlTipoPeriodo")).SelectedValue);

                campos.cal_id = _VS_cal_id;
                campos.cap_id = cap_id;
                campos.cap_dataInicio = string.IsNullOrEmpty(cap_dataInicio) ? new DateTime() : Convert.ToDateTime(cap_dataInicio);
                campos.cap_dataFim = string.IsNullOrEmpty(cap_dataFim) ? new DateTime() : Convert.ToDateTime(cap_dataFim);
                campos.cap_descricao = cap_descricao;
                campos.cap_situacao = 1;
                campos.tpc_id = tpc_id;

                list.Add(campos);
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar ler os dados na tabela.", UtilBO.TipoMensagem.Erro);
        }

        return list;
    }

    /// <summary>
    /// Pega os itens do grid e adiciona uma linha vazia.
    /// </summary>
    private void AdicionaItemGrid()
    {
        List<ACA_CalendarioPeriodo> list = listCalendarioPeriodo();
        ACA_CalendarioPeriodo cap = new ACA_CalendarioPeriodo();
        list.Add(cap);

        AtualizaGrid(list);
    }

    /// <summary>
    /// Carrega o grid de períodos do calendário
    /// </summary>
    private void CarregaPeriodo()
    {
        DataTable periodo = ACA_CalendarioPeriodoBO.Seleciona_cal_id(_VS_cal_id, false, 0, 0);
        _dgvCalendarioPeriodo.DataSource = periodo;
        _dgvCalendarioPeriodo.DataBind();

        if (_dgvCalendarioPeriodo.Rows.Count <= 0)
            AdicionaItemGrid();


        DataTable dtCursos = ACA_CalendarioCursoBO.SelecionaCursosNaoAssociados(_VS_cal_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        DataTable dtAssociados = ACA_CalendarioCursoBO.SelecionaCursosAssociados(_VS_cal_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        dtCursos.Merge(dtAssociados);

        rptCursos.DataSource = dtCursos.AsEnumerable().OrderBy(r => r["cur_nome"])
                               .Select(p => new { cur_id = p["cur_id"], cur_nome = p["cur_nome"] });
        rptCursos.DataBind();

        foreach (RepeaterItem item in rptCursos.Items)
        {
            CheckBox ckbCurso = (CheckBox)item.FindControl("ckbCurso");
            HiddenField hdnId = (HiddenField)item.FindControl("hdnId");

            if (ckbCurso != null && hdnId != null)
                ckbCurso.Checked = dtAssociados.AsEnumerable().Any(r => Convert.ToInt32(r["cur_id"]) == Convert.ToInt32(hdnId.Value));
        }
    }

    /// <summary>
    /// Valida os campos do grid
    /// </summary>
    /// <returns>0 - deu certo / 1 - deu errado / 2 - Se tiver apenas 
    /// uma linha com dados vazios (grava como se não houvessem dados) </returns>
    private int ValidaCampos()
    {
        bool flag = true            //Flag final - é true apenas se todas as outras flags de verificação são true
             , DescVazia = true     //Verifica se ja foi mostrada mensagem de validação "Descrição é obrigatória."
             , TipoVazio = true     //Verifica se ja foi mostrada mensagem de validação "Tipo de período é obrigatório."
             , DataIVazia = true    //Verifica se ja foi mostrada mensagem de validação "Data de início é obrigatória."
             , DataFVazia = true;   //Verifica se ja foi mostrada mensagem de validação "Data de fim é obrigatória."

        DateTime InicioAno = Convert.ToDateTime(_txtDataInicio.Text)
                 , FimAno = Convert.ToDateTime(_txtDataFim.Text);

        List<ACA_CalendarioPeriodo> list = listCalendarioPeriodo();

        //Arrays que armazenam os valores que já passaram pela verificação de igualdade
        ArrayList tipoIgual = new ArrayList();
        ArrayList descricaoIgual = new ArrayList();

        //Verifica se existe apenas uma linha com dados em branco
        if (list.Count == 1 &&
           (string.IsNullOrEmpty(list[0].cap_descricao) &&
           list[0].tpc_id == -1 &&
           list[0].cap_dataInicio == new DateTime() &&
           list[0].cap_dataFim == new DateTime()))
            return 2;
        else
        {
            //Percorre o grid verificando se existem erros
            foreach (GridViewRow row in _dgvCalendarioPeriodo.Rows)
            {
                bool flagFim = true         //Flag de verificação de erros na data de fim
                     , flagInicio = true    //Flag de verificação de erros na data de início
                     , flagPeriodo = true   //Flag de verificação de erros no tipo do período
                     , flagDesc = true;     //Flag de verificação de erros na descrição

                string descricao = ((TextBox)row.FindControl("txtDescricao")).Text;

                string dataI = ((TextBox)row.FindControl("txtInicioPeriodo")).Text
                       , dataF = ((TextBox)row.FindControl("txtFimPeriodo")).Text;

                CustomValidator cvInicio = (CustomValidator)row.FindControl("cvDataInicio")
                                 , cvFim = (CustomValidator)row.FindControl("cvDataFim")
                                 , cvTipoPeriodo = (CustomValidator)row.FindControl("cvTipoPeriodo")
                                 , cvDescricao = (CustomValidator)row.FindControl("cvDescricao");

                int tpc_id = Convert.ToInt32(((DropDownList)row.FindControl("ddlTipoPeriodo")).SelectedValue);

                ACA_TipoPeriodoCalendario tpc = new ACA_TipoPeriodoCalendario { tpc_id = tpc_id };
                ACA_TipoPeriodoCalendarioBO.GetEntity(tpc);

                #region Valida Campo Vazio

                if (string.IsNullOrEmpty(descricao))
                {
                    //Verifica se a mensagem ja foi mostrada
                    if (DescVazia)
                    {
                        cvDescricao.ErrorMessage = "Descrição é obrigatória.";
                        DescVazia = false;
                    }
                    cvDescricao.IsValid = false;
                    flagDesc = false;
                }
                if (tpc_id == -1)
                {
                    //Verifica se a mensagem já foi mostrada
                    if (TipoVazio)
                    {
                        cvTipoPeriodo.ErrorMessage = "Tipo de período é obrigatório.";
                        TipoVazio = false;
                    }
                    cvTipoPeriodo.IsValid = false;
                    flagPeriodo = false;
                }
                if (string.IsNullOrEmpty(dataI))
                {
                    //Verifica se a mensagem já foi mostrada
                    if (DataIVazia)
                    {
                        cvInicio.ErrorMessage = "Data de início é obrigatória.";
                        DataIVazia = false;
                    }
                    cvInicio.IsValid = false;
                    flagInicio = false;
                }
                if (string.IsNullOrEmpty(dataF))
                {
                    //Verifica se a mensagem já foi mostrada
                    if (DataFVazia)
                    {
                        cvFim.ErrorMessage = "Data de fim é obrigatória.";
                        DataFVazia = false;
                    }
                    cvFim.IsValid = false;
                    flagFim = false;
                }

                #endregion

                #region Valida Campos Iguais

                //Se o campo TipoPeriodo não estiver vazio valiada se existem dois ou mais períodos com o mesmo tpc_id
                if (flagPeriodo && list.FindAll(p => p.tpc_id == tpc_id).Count > 1)
                {
                    //Verifica se a mensagem já foi mostrada para um campo com este mesmo tpc_id
                    if (tipoIgual.IndexOf(tpc_id) < 0)
                    {
                        //Adiciona o tpc_id ao array dos tipos que ja mostraram mensagem
                        tipoIgual.Add(tpc_id);
                        cvTipoPeriodo.ErrorMessage = "Existem dois ou mais períodos cadastrados com o tipo de período: " + tpc.tpc_nome + ".";
                    }
                    cvTipoPeriodo.IsValid = false;
                    flagPeriodo = false;
                }
                //Se o campo Descrição não estiver vazio valiada se existem dois ou mais períodos com a mesma descrição
                if (flagDesc && list.FindAll(p => p.cap_descricao == descricao).Count > 1)
                {
                    //Verifica se a mensagem já foi mostrada para um campo com esta mesma descrição
                    if (descricaoIgual.IndexOf(descricao) < 0)
                    {
                        //Adiciona a descrição ao array das descrições que ja mostraram mensagem
                        descricaoIgual.Add(descricao);
                        cvDescricao.ErrorMessage = "Existem dois ou mais períodos cadastrados com a descrição: " + descricao + ".";
                    }
                    cvDescricao.IsValid = false;
                    flagDesc = false;
                }

                #endregion

                #region Valida Datas

                //Verifica se os campos de datas não estão vazios
                if (flagInicio && flagFim)
                {
                    DateTime dataInicio = Convert.ToDateTime(dataI)
                            , dataFim = Convert.ToDateTime(dataF);

                    //Valida se a data de início está dentro do intervalo permitido
                    if (dataInicio < InicioAno || dataInicio > FimAno)
                    {
                        cvInicio.ErrorMessage = "Data de início do período " + descricao + " deve estar entre  " + InicioAno.ToShortDateString() + " - " + FimAno.ToShortDateString() + ".";
                        cvInicio.IsValid = false;
                        flagInicio = false;
                    }
                    //Valida se a data de fim está dentro do intervalo permitido
                    if (dataFim < InicioAno || dataFim > FimAno)
                    {
                        cvFim.ErrorMessage = "Data de fim do período " + descricao + " deve estar entre  " + InicioAno.ToShortDateString() + " - " + FimAno.ToShortDateString() + ".";
                        cvFim.IsValid = false;
                        flagFim = false;
                    }
                    //Se as datas ainda não foram validadas, valida se ela é maior que a data de início
                    if (flagFim && flagInicio && dataFim <= dataInicio)
                    {
                        cvFim.ErrorMessage = "Data de fim do período " + descricao + " deve ser maior que a data de início.";
                        cvFim.IsValid = false;
                        flagFim = false;
                    }
                }

                #endregion

                if (flag)
                    flag = flagFim && flagInicio && flagPeriodo && flagDesc;
            }
        }
        return flag ? 0 : 1;
    }

    /// <summary>
    /// Converte a List em DataTable
    /// </summary>
    /// <param name="list">List de períodos do calendário</param>
    /// <returns>DataTable de períodos do calendário</returns>
    public static DataTable dtCalendarioPeriodo(List<ACA_CalendarioPeriodo> list)
    {
        using (DataTable dt = new DataTable())
        {
            foreach (PropertyInfo info in typeof(ACA_CalendarioPeriodo).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }
            foreach (ACA_CalendarioPeriodo cap in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(ACA_CalendarioPeriodo).GetProperties())
                {
                    row[info.Name] = info.GetValue(cap, null);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
    }

    #endregion

    #region Cursos

    /// <summary>
    /// Cria lista de entidades de CalendarioCurso de acordo com os cursos selecionados
    /// </summary>
    private List<ACA_CalendarioCurso> CriarListaCalendarioCurso()
    {
        List<ACA_CalendarioCurso> lt = new List<ACA_CalendarioCurso>();

        foreach (RepeaterItem item in rptCursos.Items)
        {
            CheckBox ckbCurso = (CheckBox)item.FindControl("ckbCurso");
            if (ckbCurso != null && ckbCurso.Checked)
            {
                HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                if (hdnId != null)
                    lt.Add(new ACA_CalendarioCurso { cur_id = Convert.ToInt32(hdnId.Value) });
            }
        }

        return lt;
    }
    
    #endregion

    /// <summary>
    /// Verifica permissão para visualizar a tela
    /// </summary>
    private void VerificaPermissao()
    {
        try
        {
            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Administracao)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Você não tem permissão para acessar essa tela.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("Busca.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {       
        if (!IsPostBack)
        {
            cvData.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Início do ano letivo");
            cvData2.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Fim do ano letivo");
            _revAno.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoAno("Ano letivo");

            VerificaPermissao();

            if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
            {
                _LoadFromEntity(PreviousPage.EditItem);
                Page.Form.DefaultFocus = _txtDescricao.ClientID;
            }
            else
            {
                Page.Form.DefaultFocus = _txtAno.ClientID;
                _btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }

            try
            {
                CarregaPeriodo();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

            pnlCursos.GroupingText = "Cadastro de " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " do calendário";

            Page.Form.DefaultButton = _btnSalvar.UniqueID;
        }

        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroCalendarioAnual.js"));
        }
    }

    protected void _dgvCalendarioPeriodo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DateTime dtInicio = (DateTime)DataBinder.Eval(e.Row.DataItem, "cap_dataInicio");
            DateTime dtFim = (DateTime)DataBinder.Eval(e.Row.DataItem, "cap_dataFim");

            ((TextBox)e.Row.FindControl("txtInicioPeriodo")).Text = dtInicio != new DateTime() ? dtInicio.ToString("dd/MM/yyyy") : "";
            ((TextBox)e.Row.FindControl("txtFimPeriodo")).Text = dtFim != new DateTime() ? dtFim.ToString("dd/MM/yyyy") : "";

            ImageButton btnDelete = (ImageButton)e.Row.FindControl("_btnExcluir");
            if (btnDelete != null)
            {
                btnDelete.CommandArgument = e.Row.RowIndex.ToString();
            }

            DropDownList ddlTipoPeriodo = (DropDownList)e.Row.FindControl("ddlTipoPeriodo");
            int cap_id = Convert.ToInt32(((Label)e.Row.FindControl("lblCap_id")).Text);

            if (ddlTipoPeriodo != null)
            {
                ddlTipoPeriodo.DataSource = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendario();
                ddlTipoPeriodo.DataBind();
                ddlTipoPeriodo.Items.Insert(0, new ListItem("-- Selecione um tipo de período --", "-1", true));
                ddlTipoPeriodo.SelectedValue = ((Label)e.Row.FindControl("tpc_id")).Text;
                ddlTipoPeriodo.Enabled = (cap_id == 0);
            }
        }
        if (_dgvCalendarioPeriodo.Rows.Count == 1)
        {
            _dgvCalendarioPeriodo.Rows[_dgvCalendarioPeriodo.Rows.Count - 1].FindControl("_btnExcluir").Visible = true;
            _dgvCalendarioPeriodo.Rows[_dgvCalendarioPeriodo.Rows.Count - 1].FindControl("ibtnAdd").Visible = true;
        }
        else
            if (_dgvCalendarioPeriodo.Rows.Count > 1)
            {
                _dgvCalendarioPeriodo.Rows[_dgvCalendarioPeriodo.Rows.Count - 1].FindControl("_btnExcluir").Visible = true;
                _dgvCalendarioPeriodo.Rows[_dgvCalendarioPeriodo.Rows.Count - 1].FindControl("ibtnAdd").Visible = true;

                _dgvCalendarioPeriodo.Rows[_dgvCalendarioPeriodo.Rows.Count - 2].FindControl("_btnExcluir").Visible = true;
                _dgvCalendarioPeriodo.Rows[_dgvCalendarioPeriodo.Rows.Count - 2].FindControl("ibtnAdd").Visible = false;
            }
    }

    protected void _dgvCalendarioPeriodo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            try
            {
                List<ACA_CalendarioPeriodo> list = listCalendarioPeriodo();

                list.RemoveAt(Convert.ToInt32(e.CommandArgument));

                // Atualiza Grid.
                AtualizaGrid(list);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir.", UtilBO.TipoMensagem.Erro);
            }
        }
        else if (e.CommandName == "Adicionar")
        {
            AdicionaItemGrid();
        }
    }

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _btnSalvar_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            cvDatasAnoLetivo.Visible = true;
            cvDatasAnoLetivo.Validate();
            if (cvDatasAnoLetivo.IsValid)
            {
                Salvar();
                cvDatasAnoLetivo.Visible = false;
            }
        }
    }

    protected void ValidarDatasPeriodoLetivo_ServerValidate(object source, ServerValidateEventArgs args)
    {
        bool flag = true;

        DateTime dtIni = Convert.ToDateTime(_txtDataInicio.Text);
        DateTime dtFim = Convert.ToDateTime(_txtDataFim.Text);

        if (dtIni > dtFim)
            flag = false;

        args.IsValid = flag;
    }

    #endregion
}
