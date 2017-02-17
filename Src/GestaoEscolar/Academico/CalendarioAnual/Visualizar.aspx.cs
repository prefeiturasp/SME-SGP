using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class Academico_CalendarioAnual_Visualizar : MotherPageLogado
{
    #region ATRIBUTOS/PROPRIEDADES

    /// <summary>
    /// Propriedade na qual cria variavel em ViewState armazenando valor de cal_id
    /// </summary>
    private int _VS_cal_id
    {
        get
        {
            if (ViewState["_VS_cal_id"] != null)
                return Convert.ToInt32(ViewState["_VS_cal_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_cal_id"] = value;
        }
    }

    /// <summary>
    /// Armazena os períodos do calendário
    /// </summary>
    private DataTable periodos;

    private DataTable Periodos
    {
        get
        {
            if (periodos == null)
                periodos = new DataTable();

            return periodos;
        }

        set { periodos = value; }
    }

    /// <summary>
    /// Armazena os eventos do calendário
    /// </summary>
    /// </summary>
    private DataTable eventos;

    private DataTable Eventos
    {
        get
        {
            if (eventos == null)
                eventos = new DataTable();

            return eventos;
        }

        set { eventos = value; }
    }

    /// <summary>
    /// Armazena os dias não úteis
    /// </summary>
    private List<SYS_DiaNaoUtil> diasNaoUteis;

    private List<SYS_DiaNaoUtil> DiasNaoUteis
    {
        get
        {
            if (diasNaoUteis == null)
                diasNaoUteis = new List<SYS_DiaNaoUtil>();

            return diasNaoUteis;
        }

        set { diasNaoUteis = value; }
    }

    /// <summary>
    /// Armazena as informações do calendário
    /// </summary>
    private ACA_CalendarioAnual calendarioanual;

    private ACA_CalendarioAnual CalendarioAnual
    {
        get
        {
            if (calendarioanual == null)
                calendarioanual = new ACA_CalendarioAnual();

            return calendarioanual;
        }
        set { calendarioanual = value; }
    }

    /// <summary>
    /// Cidade do usuario logado.
    /// </summary>
    private END_Cidade cidade;

    private END_Cidade Cidade
    {
        get
        {
            if (cidade == null)
            {
                cidade = new END_Cidade
                {
                    cid_id = CarregarCidadeUsuarioLogado()
                };
                END_CidadeBO.GetEntity(cidade);
            }

            return cidade;
        }
    }

    /// <summary>
    /// Cor que indica inicio/fim do ano letivo
    /// </summary>
    protected string CorInicioAnoLetivo = ApplicationWEB.CalendarioCorInicioAnoLetivo;

    /// <summary>
    /// Cor que indica inicio/fim dos períodos/eventos(padrões) do calendário)
    /// </summary>
    protected string CorPeriodosEventos = ApplicationWEB.CalendarioCorPeriodosEventos;

    /// <summary>
    /// Cor que indica os dias não úteis
    /// </summary>
    protected string CorDiasNaoUteis = ApplicationWEB.CalendarioCorDiasNaoUteis;

    /// <summary>
    /// Cor que indica os dias sem atividade discente
    /// </summary>
    protected string CorDiasSemAtividadeDiscente = ApplicationWEB.CalendarioCorDiasSemAtividadeDiscente;

    #endregion ATRIBUTOS/PROPRIEDADES

    #region MÉTODOS

    /// <summary>
    /// Carrega os dados da pagina.
    /// </summary>
    private void CarregarDados()
    {
        ACA_CalendarioAnual calendario = new ACA_CalendarioAnual { cal_id = _VS_cal_id };
        ACA_CalendarioAnualBO.GetEntity(calendario);
        CalendarioAnual = calendario;

        // busca os periodos referentes ao calendário
        Periodos = ACA_CalendarioPeriodoBO.Seleciona_cal_id(CalendarioAnual.cal_id, false, 1, 1);

        // busca os dias não úteis
        DiasNaoUteis = SYS_DiaNaoUtilBO.SelecionaTodosPorCidade(CarregarCidadeUsuarioLogado());

        // mostra o nome da calendário
        lblCalendario.Text = "Calendário escolar: <b>" + CalendarioAnual.cal_descricao + "</b>";

        // mostra o ano letivo
        lblAnoLetivo.Text = "Ano Letivo: <b>" + CalendarioAnual.cal_ano + "</b>";

        if (ddlComboTipoEvento.SelectedValue.Equals("2") && ucComboUAEscola.Esc_ID <= 0)
        {
            fdsVisualizacao.Visible = false;
            return;
        }

        Eventos = ACA_CalendarioEventoBO.BuscaEventosCalendario(
            CalendarioAnual.cal_id,
            ddlComboTipoEvento.SelectedValue.Equals("0"),
            ddlComboTipoEvento.SelectedValue.Equals("2") ? ucComboUAEscola.Esc_ID : 0
        );

        IniciaRepeater();

        IniciaRepeaterDiasLetivos();

        CriaTabelaLegenda();

        fdsVisualizacao.Visible = true;

        // mostra a quantidade de dias letivos no ano
        int totalDeDiasLetivos = NumeroDeDiasUteis(CalendarioAnual.cal_dataInicio, CalendarioAnual.cal_dataFim);
        //- NumeroDeDiasSemAtividadeDiscente(CalendarioAnual.cal_dataInicio, CalendarioAnual.cal_dataFim);
        lblDiasLetivosNoAno.Text = "Dias letivos do ano: <b>" + totalDeDiasLetivos + "</b>";
    }

    /// <summary>
    /// Inicia o repeater externo com os meses do calendário
    /// </summary>
    private void IniciaRepeater()
    {
        DataTable dtMeses = new DataTable();
        dtMeses.Columns.Add("mes", typeof(DateTime));

        // calcula o número de meses que compõe o calendário
        int totalMeses = (CalendarioAnual.cal_dataFim.Year - CalendarioAnual.cal_dataInicio.Year) * 12
            + (CalendarioAnual.cal_dataFim.Month - CalendarioAnual.cal_dataInicio.Month);

        // adiciona cada mês no data table
        for (int i = 0; i <= totalMeses; i++)
        {
            DataRow dr = dtMeses.NewRow();
            dr["mes"] = CalendarioAnual.cal_dataInicio.AddMonths(i);
            dtMeses.Rows.Add(dr);
        }

        // associa o data table com o repeater
        rptCalendarios.DataSource = dtMeses;
        rptCalendarios.DataBind();
    }

    /// <summary>
    ///
    /// </summary>
    private void IniciaRepeaterDiasLetivos()
    {
        DataTable dtDiasLetivos = new DataTable();
        dtDiasLetivos.Columns.Add("periodo", typeof(string));
        dtDiasLetivos.Columns.Add("dias", typeof(Int32));

        // Adiciona cada período e seu respectivo número de dias letivos ao data table.
        foreach (DataRow row in Periodos.Rows)
        {
            DateTime dataInicial = DateTime.Parse((row["cap_periodo"].ToString().Split('-'))[0]);
            DateTime dataFinal = DateTime.Parse((row["cap_periodo"].ToString().Split('-'))[1]);

            DataRow dr = dtDiasLetivos.NewRow();
            dr["periodo"] = row["cap_descricao"];
            dr["dias"] = NumeroDeDiasUteis(dataInicial, dataFinal);
            dtDiasLetivos.Rows.Add(dr);
        }

        // Associa o data table com o repeater
        rptDiasLetivos.DataSource = dtDiasLetivos;
        rptDiasLetivos.DataBind();
    }

    /// <summary>
    /// Calcula o número de dias úteis entre duas datas.
    /// </summary>
    /// <param name="dataInicial">Data inicial</param>
    /// <param name="dataFinal">Data final</param>
    /// <returns>Número de dias úteis</returns>
    private int NumeroDeDiasUteis(DateTime dataInicial, DateTime dataFinal)
    {
        int totalDiasLetivos = 0;
        int dias = dataFinal.Subtract(dataInicial).Days;
        dias++;
         for (int i = 1; i <= dias; i++)
        {
            //Verifica se o dia não é util
            //      não é um sabado ou domingo, que nao contam como dias letivos
            //      um dia nao util do sistema, configurado pelo core
            //      um dia de evento sem atividade dicente cadastrado no sistema
            bool diaUtil =
                    (
                        dataInicial.DayOfWeek != DayOfWeek.Sunday
                        && dataInicial.DayOfWeek != DayOfWeek.Saturday
                        && !DiaDeEvento(dataInicial) 
                        && !DiaNaoUtil(dataInicial)
                    );

            //Caso o dia não seria util mas como tem uma atividade discente cadastrada, ele passa a ser util
            if (!diaUtil)
                diaUtil = DiaAtividadeDiversificada(dataInicial);

            //Conta apenas os dias da semana que não são nem dia de evento nem dia não útil e nem dia de atividade diversificada
            if (diaUtil)
                totalDiasLetivos++;
            
            dataInicial = dataInicial.AddDays(1);
        }

        return totalDiasLetivos;
    }

    /// <summary>
    /// Verifica se o dia ie marcado como dia não útil.
    /// </summary>
    /// <param name="dia">Dia do ano</param>
    /// <returns>Verdadeiro se é marcado como não útil.</returns>
    private bool DiaNaoUtil(DateTime dia)
    {
        foreach (SYS_DiaNaoUtil li in DiasNaoUteis)
        {
            if ((li.dnu_data.Day == dia.Date.Day
                && li.dnu_data.Month == dia.Date.Month
                && (li.dnu_data.Year == dia.Date.Year
                    || (li.dnu_recorrencia
                        && li.dnu_vigenciaInicio <= DateTime.Now
                        && (DateTime.Now <= li.dnu_vigenciaFim || li.dnu_vigenciaFim == new DateTime())
                        )
                     )
                )
                && (li.unf_id == Guid.Empty || Cidade.unf_id == li.unf_id)
                )
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Verifica se a data esta dentro de um evento de atividade diversificada
    /// </summary>
    /// <param name="dia">Dia do ano</param>
    /// <returns>Verdadeiro se ele pertencer a um evento de atividade diversificada.</returns>
    private bool DiaAtividadeDiversificada(DateTime dia)
    {
        int parametroAtivDiversificada = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(
            eChaveAcademico.TIPO_EVENTO_ATIVIDADE_DIVERSIFICADA
            , __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        
        // altera a cor da célula referente a data de cada evento
        DateTime dateAux;

        return (
                from DataRow row in Eventos.Rows
                where
                    DateTime.TryParse(row["evt_dataInicio"].ToString(), out dateAux)
                    && DateTime.TryParse(row["evt_dataFim"].ToString(), out dateAux)
                    && DateTime.Parse(row["evt_dataInicio"].ToString()) <= dia
                    && dia <= DateTime.Parse(row["evt_dataFim"].ToString())
                    //Possui atividade discente
                    && !bool.Parse(row["evt_semAtividadeDiscente"].ToString())
                    //For atividade diversificada
                    && Convert.ToInt32(row["tev_id"]) == parametroAtivDiversificada
                select new
                {
                    evt_dataInicio = DateTime.Parse(row["evt_dataInicio"].ToString()),
                    evt_dataFim = DateTime.Parse(row["evt_dataFim"].ToString()),                   
                    esc_nome = row["esc_nome"].ToString()
                }
            ).Any();
    }

    /// <summary>
    /// Verifica se o dia pertence ao período de algum evento
    /// </summary>
    /// <param name="dia">Dia do ano</param>
    /// <returns>Verdadeiro se pertence a algum evento.</returns>
    private bool DiaDeEvento(DateTime dia)
    {
        // altera a cor da célula referente a data de cada evento
        DateTime dateAux;
        var listEventos = (
                from DataRow row in Eventos.Rows
                where
                    DateTime.TryParse(row["evt_dataInicio"].ToString(), out dateAux)
                    && DateTime.TryParse(row["evt_dataFim"].ToString(), out dateAux)
                    && DateTime.Parse(row["evt_dataInicio"].ToString()) <= dia
                    && dia <= DateTime.Parse(row["evt_dataFim"].ToString())
                orderby
                    row["esc_nome"].ToString() descending

                select new
                {
                    evt_dataInicio = DateTime.Parse(row["evt_dataInicio"].ToString()),
                    evt_dataFim = DateTime.Parse(row["evt_dataFim"].ToString()),
                    evt_semAtividadeDiscente = bool.Parse(row["evt_semAtividadeDiscente"].ToString()),
                    esc_nome = row["esc_nome"].ToString()
                }                
            // Alterado para considerar primeiro os eventos sem atividades discentes
            ).Distinct().OrderByDescending(p => p.evt_semAtividadeDiscente).ToList();
            //).Distinct().OrderByDescending(p => p.esc_nome).ThenByDescending(p => p.evt_semAtividadeDiscente).ToList();

        return (listEventos.Any() && listEventos.First().evt_semAtividadeDiscente);
    }

    private bool DiaDeEventoAtividadeDiversificada(DateTime dia, int parametroAtivDiv)
    {
        return Eventos.Rows.Cast<DataRow>().Any(row => Convert.ToDateTime(row["evt_dataInicio"]) <= dia && Convert.ToDateTime(row["evt_dataFim"]) >= dia && Convert.ToInt32(row["tev_id"]) == parametroAtivDiv);
    }

    /// <summary>
    /// Cria uma tabela HTML contendo a legenda das cores utilizadas
    /// </summary>
    private void CriaTabelaLegenda()
    {
        HtmlTable tabelalegenda = new HtmlTable();
        HtmlTableCell cell;

        HtmlTableRow row1 = new HtmlTableRow();

        // configura célula
        cell = new HtmlTableCell { BgColor = CorInicioAnoLetivo, Height = "15px", Width = "25px" };

        // insere célula na linha
        row1.Cells.Add(cell);

        // configura célula
        cell = new HtmlTableCell { InnerText = "Início/Fim ano letivo" };

        // insere célula na linha
        row1.Cells.Add(cell);

        HtmlTableRow row2 = new HtmlTableRow();

        // configura célula
        cell = new HtmlTableCell { BgColor = CorPeriodosEventos, Height = "15px", Width = "25px" };

        // insere célula na linha
        row2.Cells.Add(cell);

        // configura célula
        cell = new HtmlTableCell { InnerText = "Períodos/Eventos" };

        // insere célula na linha
        row2.Cells.Add(cell);

        HtmlTableRow row3 = new HtmlTableRow();

        // configura célula
        cell = new HtmlTableCell { BgColor = CorDiasNaoUteis, Height = "15px", Width = "25px" };

        // insere célula na linha
        row3.Cells.Add(cell);

        // configura célula
        cell = new HtmlTableCell { InnerText = "Dias não úteis" };

        // insere célula na linha
        row3.Cells.Add(cell);

        HtmlTableRow row4 = new HtmlTableRow();

        // configura célula
        cell = new HtmlTableCell { BgColor = CorDiasSemAtividadeDiscente, Height = "15px", Width = "25px" };

        // insere célula na linha
        row4.Cells.Add(cell);

        // configura célula
        cell = new HtmlTableCell { InnerText = "Dias sem atividade discente" };

        // insere célula na linha
        row4.Cells.Add(cell);

        // insere as linhas na tabela
        tabelalegenda.Rows.Add(row1);
        tabelalegenda.Rows.Add(row2);
        tabelalegenda.Rows.Add(row3);
        tabelalegenda.Rows.Add(row4);

        // adiciona tabela na div
        divLegenda.Controls.Add(tabelalegenda);
    }

    /// <summary>
    /// Retorna a cidade pelo endereço da entidade do usuário logado.
    /// </summary>
    private Guid CarregarCidadeUsuarioLogado()
    {
        // Setar a cidade pelo endereço da Entidade do usuário logado.
        Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

        Guid ene_id = SYS_EntidadeEnderecoBO.Select_ene_idBy_ent_id(ent_id);

        SYS_EntidadeEndereco entEndereco = new SYS_EntidadeEndereco
        {
            ent_id = ent_id
            ,
            ene_id = ene_id
        };
        SYS_EntidadeEnderecoBO.GetEntity(entEndereco);

        // Recuperando entidade Endereço do usuário logado.
        END_Endereco endereco = new END_Endereco
        {
            end_id = entEndereco.end_id
        };
        END_EnderecoBO.GetEntity(endereco);

        return endereco.cid_id;
    }

    #endregion MÉTODOS

    #region EVENTOS

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                {
                    // carrega o calendário
                    _VS_cal_id = PreviousPage.EditItem;
                    if (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Individual || __SessionWEB.__UsuarioWEB.Docente.doc_id == 0)
                        ucComboUAEscola.Inicializar();
                    else
                        ucComboUAEscola.InicializarVisaoIndividual(__SessionWEB.__UsuarioWEB.Docente.doc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, 1);

                    ucComboUAEscola.ObrigatorioEscola = true;
                    ucComboUAEscola.MostrarMessageSelecioneEscola = true;

                    if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa ||
                        __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                    {
                        ddlComboTipoEvento.SelectedValue = "2";
                        divEscolas.Visible = divBtnPesquisar.Visible = true;
                    }

                    CarregarDados();
                }
                else
                {
                    Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/CalendarioAnual/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        ucComboUAEscola.IndexChangedUA += UCFiltroEscolas1__Selecionar;
    }

    /// <summary>
    /// Evento change do combo de UA Superior.
    /// </summary>
    private void UCFiltroEscolas1__Selecionar()
    {
        try
        {
            ucComboUAEscola.CarregaEscolaPorUASuperiorSelecionada();

            if (ucComboUAEscola.Uad_ID != Guid.Empty)
            {
                ucComboUAEscola.FocoEscolas = true;
                ucComboUAEscola.PermiteAlterarCombos = true;
            }

            ucComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Evento disparado ao renderizar cada dia do calendário
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void calMesPeriodo_DayRender(object sender, DayRenderEventArgs e)
    {
        // coloca os dias referentes a outros meses em uma cor diferente
        if (e.Day.IsOtherMonth)
        {
            e.Cell.ForeColor = Color.Silver;
        }

        // altera a cor da célula referente a data de cada período
        foreach (DataRow row in Periodos.Rows)
        {
            DateTime dataPeriodo;
            if (DateTime.TryParse((row["cap_periodo"].ToString().Split('-'))[0], out dataPeriodo)
                && dataPeriodo == e.Day.Date && !e.Day.IsOtherMonth)
            {
                e.Cell.Font.Bold = true;
                e.Cell.ForeColor = Color.FromName(CorPeriodosEventos);
            }

            if (DateTime.TryParse((row["cap_periodo"].ToString().Split('-'))[1], out dataPeriodo)
                && dataPeriodo == e.Day.Date && !e.Day.IsOtherMonth)
            {
                e.Cell.Font.Bold = true;
                e.Cell.ForeColor = Color.FromName(CorPeriodosEventos);
            }
        }

        // altera a cor da célula referente a data de cada evento
        DateTime dateAux;
        var listEventos = (
                from DataRow row in Eventos.Rows
                where
                    DateTime.TryParse(row["evt_dataInicio"].ToString(), out dateAux)
                    && DateTime.TryParse(row["evt_dataFim"].ToString(), out dateAux)
                    && DateTime.Parse(row["evt_dataInicio"].ToString()) <= e.Day.Date
                    && e.Day.Date <= DateTime.Parse(row["evt_dataFim"].ToString())
                select new
                {
                    evt_dataInicio = DateTime.Parse(row["evt_dataInicio"].ToString()),
                    evt_dataFim = DateTime.Parse(row["evt_dataFim"].ToString()),
                    evt_semAtividadeDiscente = bool.Parse(row["evt_semAtividadeDiscente"].ToString()),
                    esc_nome = row["esc_nome"].ToString()
                }

            ).Distinct().ToList();

        if ((listEventos.Any(p => p.evt_dataInicio == e.Day.Date) && !e.Day.IsOtherMonth)
                || (listEventos.Any(p => p.evt_dataFim == e.Day.Date) && !e.Day.IsOtherMonth))
        {
            e.Cell.Font.Bold = true;
            e.Cell.ForeColor = Color.FromName(CorPeriodosEventos);
        }

        foreach (DataRow row in Eventos.Rows)
        {
            DateTime dataInicioEvento, dataFimEvento;
            DateTime.TryParse(row["evt_dataInicio"].ToString(), out dataInicioEvento);
            DateTime.TryParse(row["evt_dataFim"].ToString(), out dataFimEvento);

            if (bool.Parse(row["evt_semAtividadeDiscente"].ToString())
                && dataInicioEvento <= e.Day.Date
                && e.Day.Date <= dataFimEvento)
            {
                e.Cell.BackColor = Color.FromName(CorDiasSemAtividadeDiscente);
            }
        }


        //foreach (DataRow row in Eventos.Rows)
        //{
        //    DateTime dataInicioEvento, dataFimEvento;
        //    if (DateTime.TryParse(row["evt_dataInicio"].ToString(), out dataInicioEvento)
        //        && dataInicioEvento == e.Day.Date && !e.Day.IsOtherMonth)
        //    {
        //        e.Cell.Font.Bold = true;
        //        e.Cell.ForeColor = Color.FromName(CorPeriodosEventos);
        //    }

        //    if (DateTime.TryParse(row["evt_dataFim"].ToString(), out dataFimEvento)
        //        && dataFimEvento == e.Day.Date && !e.Day.IsOtherMonth)
        //    {
        //        e.Cell.Font.Bold = true;
        //        e.Cell.ForeColor = Color.FromName(CorPeriodosEventos);
        //    }

        //    if (bool.Parse(row["evt_semAtividadeDiscente"].ToString())
        //        && dataInicioEvento <= e.Day.Date
        //        && e.Day.Date <= dataFimEvento)
        //    {
        //        e.Cell.BackColor = Color.FromName(CorDiasSemAtividadeDiscente);
        //    }
        //}

        int parametroAtivDiversificada = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_ATIVIDADE_DIVERSIFICADA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        if (DiaNaoUtil(e.Day.Date) && !DiaDeEventoAtividadeDiversificada(e.Day.Date, parametroAtivDiversificada) && !e.Day.IsOtherMonth)
        {
            e.Cell.Font.Bold = true;
            e.Cell.ForeColor = Color.FromName(CorDiasNaoUteis);
            e.Cell.BackColor = Color.FromName(CorDiasSemAtividadeDiscente);
        }

        //// altera a cor da célula referente a data de cada dia não útil
        //foreach (SYS_DiaNaoUtil dia in DiasNaoUteis)
        //{
        //    // Verifica se é o mês certo e (o ano certo ou é (recorrente e se a data de hoje está dentro do período vigente))
        //    if ((dia.dnu_data.Day == e.Day.Date.Day
        //            && dia.dnu_data.Month == e.Day.Date.Month
        //            && (dia.dnu_data.Year == e.Day.Date.Year
        //                || (dia.dnu_recorrencia
        //                    && dia.dnu_vigenciaInicio <= DateTime.Now
        //                    && (DateTime.Now <= dia.dnu_vigenciaFim || dia.dnu_vigenciaFim == new DateTime())
        //                    )
        //                 )
        //         )
        //         && !e.Day.IsOtherMonth
        //         && (dia.unf_id == Guid.Empty
        //            || Cidade.unf_id == dia.unf_id)
        //        )
        //    {
        //        e.Cell.Font.Bold = true;
        //        e.Cell.ForeColor = Color.FromName(CorDiasNaoUteis);
        //        e.Cell.BackColor = Color.FromName(CorDiasSemAtividadeDiscente);
        //    }
        //}

        // altera a cor da célula referente ao início e fim do calendário
        if (CalendarioAnual.cal_dataInicio == e.Day.Date && !e.Day.IsOtherMonth)
        {
            e.Cell.Font.Bold = true;
            e.Cell.ForeColor = Color.FromName(CorInicioAnoLetivo);
        }
        else
            if (CalendarioAnual.cal_dataFim == e.Day.Date && !e.Day.IsOtherMonth)
            {
                e.Cell.Font.Bold = true;
                e.Cell.ForeColor = Color.FromName(CorInicioAnoLetivo);
            }
    }

    /// <summary>
    /// Evento disparado ao dar bind no repeater externo
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptCalendarios_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataTable dtEventos = new DataTable();
            dtEventos.Columns.Add("dia", typeof(int));
            dtEventos.Columns.Add("periodo", typeof(string));
            DataRow dr;

            // insere inicio do calendário no data table
            if (CalendarioAnual.cal_dataInicio.Month == ((Calendar)e.Item.FindControl("calMesPeriodo")).VisibleDate.Month
                && CalendarioAnual.cal_dataInicio.Year == ((Calendar)e.Item.FindControl("calMesPeriodo")).VisibleDate.Year)
            {
                dr = dtEventos.NewRow();
                dr["dia"] = CalendarioAnual.cal_dataInicio.Day;
                dr["periodo"] = "<font color=" + CorInicioAnoLetivo + "><b>" + CalendarioAnual.cal_dataInicio.Day + "</b></font>: " +
                    "Início ano letivo" + "<br />";
                dtEventos.Rows.Add(dr);
            }

            // insere fim do calendário no data table
            if (CalendarioAnual.cal_dataFim.Month == ((Calendar)e.Item.FindControl("calMesPeriodo")).VisibleDate.Month
                && CalendarioAnual.cal_dataFim.Year == ((Calendar)e.Item.FindControl("calMesPeriodo")).VisibleDate.Year)
            {
                dr = dtEventos.NewRow();
                dr["dia"] = CalendarioAnual.cal_dataFim.Day;
                dr["periodo"] = "<font color=" + CorInicioAnoLetivo + "><b>" + CalendarioAnual.cal_dataFim.Day + "</b></font>: " +
                    "Fim ano letivo" + "<br />";
                dtEventos.Rows.Add(dr);
            }

            // insere inicio e fim dos períodos referentos ao calendário no data table
            foreach (DataRow row in Periodos.Rows)
            {
                DateTime dataInicioFim;
                if (DateTime.TryParse((row["cap_periodo"].ToString().Split('-'))[0], out dataInicioFim)
                    && dataInicioFim.Month == ((Calendar)e.Item.FindControl("calMesPeriodo")).VisibleDate.Month
                    && dataInicioFim.Year == ((Calendar)e.Item.FindControl("calMesPeriodo")).VisibleDate.Year)
                {
                    dr = dtEventos.NewRow();
                    dr["dia"] = dataInicioFim.Day;
                    dr["periodo"] = "<font color=" + CorPeriodosEventos + "><b>" + dataInicioFim.Day + "</b></font>: " + "Início " + row["cap_descricao"] + "<br />";
                    dtEventos.Rows.Add(dr);
                }

                if (DateTime.TryParse((row["cap_periodo"].ToString().Split('-'))[1], out dataInicioFim)
                    && dataInicioFim.Month == ((Calendar)e.Item.FindControl("calMesPeriodo")).VisibleDate.Month
                    && dataInicioFim.Year == ((Calendar)e.Item.FindControl("calMesPeriodo")).VisibleDate.Year)
                {
                    dr = dtEventos.NewRow();
                    dr["dia"] = dataInicioFim.Day;
                    dr["periodo"] = "<font color=" + CorPeriodosEventos + "><b>" + dataInicioFim.Day + "</b></font>: " + "Fim " + row["cap_descricao"] + "<br />";
                    dtEventos.Rows.Add(dr);
                }
            }

            // insere os eventos (padrões) referentes ao calendário no data table
            foreach (DataRow row in Eventos.Rows)
            {
                DateTime dataInicioEvento, dataFimEvento;
                bool dataValida = DateTime.TryParse(row["evt_dataFim"].ToString(), out dataFimEvento);
                dataValida &= DateTime.TryParse(row["evt_dataInicio"].ToString(), out dataInicioEvento);

                if (dataValida)
                {
                    if (dataInicioEvento == dataFimEvento
                        && dataInicioEvento.Month == ((Calendar)e.Item.FindControl("calMesPeriodo")).VisibleDate.Month
                        && dataInicioEvento.Year == ((Calendar)e.Item.FindControl("calMesPeriodo")).VisibleDate.Year)
                    {
                        dr = dtEventos.NewRow();
                        dr["dia"] = dataInicioEvento.Day;
                        dr["periodo"] = bool.Parse(row["evt_semAtividadeDiscente"].ToString()) ?
                            "<font color=" + CorPeriodosEventos + "><b>" + dataInicioEvento.Day + "</b></font>: " +
                                             ((__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa &&
                                              __SessionWEB.__UsuarioWEB.GrupoUA.Count == 1) ||
                                             ucComboUAEscola.Esc_ID > 0 && ddlComboTipoEvento.SelectedValue.Equals("2") ? "" :
                                             row["esc_nome"]) + row["evt_nome"] + "<br />" :
                            "<font color=" + CorPeriodosEventos + "><b>" + dataInicioEvento.Day + "</b></font>: " +
                                             ((__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa &&
                                              __SessionWEB.__UsuarioWEB.GrupoUA.Count == 1) ||
                                             ucComboUAEscola.Esc_ID > 0 && ddlComboTipoEvento.SelectedValue.Equals("2") ? "" :
                                             row["esc_nome"]) + row["evt_nome"] + "<br />";
                        dtEventos.Rows.Add(dr);
                    }
                    else
                    {
                        if (dataInicioEvento.Month == ((Calendar)e.Item.FindControl("calMesPeriodo")).VisibleDate.Month
                            && dataInicioEvento.Year == ((Calendar)e.Item.FindControl("calMesPeriodo")).VisibleDate.Year)
                        {
                            dr = dtEventos.NewRow();
                            dr["dia"] = dataInicioEvento.Day;
                            dr["periodo"] = bool.Parse(row["evt_semAtividadeDiscente"].ToString()) ?
                                "<font color=" + CorPeriodosEventos + "><b>" + dataInicioEvento.Day + "</b></font>: " + "Início " +
                                                 ((__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa &&
                                                  __SessionWEB.__UsuarioWEB.GrupoUA.Count == 1) ||
                                                 ucComboUAEscola.Esc_ID > 0 && ddlComboTipoEvento.SelectedValue.Equals("2") ? "" :
                                                 row["esc_nome"]) + row["evt_nome"] + "<br />" :
                                "<font color=" + CorPeriodosEventos + "><b>" + dataInicioEvento.Day + "</b></font>: " + "Início " +
                                                 ((__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa &&
                                                  __SessionWEB.__UsuarioWEB.GrupoUA.Count == 1) ||
                                                 ucComboUAEscola.Esc_ID > 0 && ddlComboTipoEvento.SelectedValue.Equals("2") ? "" :
                                                 row["esc_nome"]) + row["evt_nome"] + "<br />";
                            dtEventos.Rows.Add(dr);
                        }

                        if (dataFimEvento.Month == ((Calendar)e.Item.FindControl("calMesPeriodo")).VisibleDate.Month
                            && dataFimEvento.Year == ((Calendar)e.Item.FindControl("calMesPeriodo")).VisibleDate.Year)
                        {
                            dr = dtEventos.NewRow();
                            dr["dia"] = dataFimEvento.Day;
                            dr["periodo"] = bool.Parse(row["evt_semAtividadeDiscente"].ToString()) ?
                                "<font color=" + CorPeriodosEventos + "><b>" + dataFimEvento.Day + "</b></font>: " + "Fim " +
                                                 ((__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa &&
                                                  __SessionWEB.__UsuarioWEB.GrupoUA.Count == 1) ||
                                                 ucComboUAEscola.Esc_ID > 0 && ddlComboTipoEvento.SelectedValue.Equals("2") ? "" :
                                                 row["esc_nome"]) + row["evt_nome"] + "<br />" :
                                "<font color=" + CorPeriodosEventos + "><b>" + dataFimEvento.Day + "</b></font>: " + "Fim " +
                                                ((__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa &&
                                                  __SessionWEB.__UsuarioWEB.GrupoUA.Count == 1) ||
                                                 ucComboUAEscola.Esc_ID > 0 && ddlComboTipoEvento.SelectedValue.Equals("2") ? "" :
                                                 row["esc_nome"]) + row["evt_nome"] + "<br />";
                            dtEventos.Rows.Add(dr);
                        }
                    }
                }
            }

            // insere os dias não úteis no data table
            foreach (SYS_DiaNaoUtil dia in DiasNaoUteis)
            {
                //Verifica se é o mês certo e (o ano certo ou é (recorrente e se a data de hoje está dentro do período vigente))
                if (dia.dnu_data.Month == ((Calendar)e.Item.FindControl("calMesPeriodo")).VisibleDate.Month
                                            && (dia.dnu_data.Year == ((Calendar)e.Item.FindControl("calMesPeriodo")).VisibleDate.Year
                                            || (dia.dnu_recorrencia
                                            && dia.dnu_vigenciaInicio <= DateTime.Now
                                            && (DateTime.Now <= dia.dnu_vigenciaFim
                                            || dia.dnu_vigenciaFim == new DateTime())))

                 && (dia.unf_id == Guid.Empty
                    || Cidade.unf_id == dia.unf_id))
                {
                    dr = dtEventos.NewRow();
                    dr["dia"] = dia.dnu_data.Day;
                    dr["periodo"] = "<font color=" + CorDiasNaoUteis + "><b>" + dia.dnu_data.Day + "</b></font>: " + dia.dnu_nome + "<br />";
                    dtEventos.Rows.Add(dr);
                }
            }

            // ordena os dados por ordem crescente de dia
            DataView dv = new DataView(dtEventos) { Sort = "dia ASC" };

            // associa data table (ordenado) com o repeater
            Repeater rptEventos = (Repeater)e.Item.FindControl("rptEventos");
            rptEventos.DataSource = dv;
            rptEventos.DataBind();
        }
    }

    /// <summary>
    /// Evento disparado ao clicar no botão voltar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void _btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/CalendarioAnual/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void ddlComboTipoEvento_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divEscolas.Visible = fdsVisualizacao.Visible = divBtnPesquisar.Visible = false;
            if (ddlComboTipoEvento.SelectedValue.Equals("2"))
                divEscolas.Visible = divBtnPesquisar.Visible = true;

            CarregarDados();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o calendário.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void _btnPesquisar_Click(object sender, EventArgs e)
    {
        try
        {
            CarregarDados();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o calendário.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion EVENTOS
}