using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_TurmaDisciplina_UCRepeaterDisciplina : MotherUserControl
{
    #region Propriedades

    /// <summary>
    /// Verifica se a turma tem acesso ao controle semestral
    /// </summary>
    public bool VS_AcessoControleSemestral
    {
        get
        {
            if (ViewState["VS_AcessoControleSemestral"] == null)
                return false;
            return Convert.ToBoolean(ViewState["VS_AcessoControleSemestral"]);
        }
        set
        {
            ViewState["VS_AcessoControleSemestral"] = value;
        }
    }

    public DataTable dtDocentesEscola;
    private DataTable DtDisciplinaNaoAvaliado;
    private DataTable DtAvaliacoesFormato;
    private DataTable DtVigenciasDocentes;
    public List<TUR_Turma_Docentes_Disciplina> listTurmaDocentes;

    private Int32 cde_idAnterior;
    private Int32 Tud_ID;
    private Int32 escola_esc_id;
    private Int32 escola_uni_id;
    private bool escola_bloqueioAtribuicaoDocente;
    private bool professorEspecialista;
    private int indiceRepeater;
    private bool aplicarNovaRegraDocenciaCompartilhada = false;

    private readonly List<string> periodosIds = new List<string>();
    public string Script = "";

    /// <summary>
    /// Propriedade que recebe o nome do grid.
    /// </summary>
    public String nomeTipoDisciplina
    {
        get
        {
            return lblTipoDisciplina.Text;
        }
        set
        {
            lblTipoDisciplina.Text = value;
        }
    }

    /// <summary>
    /// Retorna o texto para o label de mensagem sobre o campo Controle Semestral
    /// </summary>
    protected string TextoControleSemestral
    {
        get
        {
            return UtilBO.GetErroMessage(
                "Em \"Controle semestral\", marque quando os(as) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL_MIN") + " não serão avaliadas.",
                UtilBO.TipoMensagem.Informacao);
        }
    }

    /// <summary>
    /// Tabela com períodos do calendário da turma.
    /// </summary>
    public DataTable dtPeriodosCalendario;

    public bool PermiteEditar
    {
        set
        {
            if (!value)
            {
                foreach (RepeaterItem row in rptDisciplinasEletivas.Items)
                {
                    TextBox txtAulaSemanal = (TextBox)row.FindControl("txtAulaSemanal");
                    if (txtAulaSemanal != null)
                        txtAulaSemanal.Enabled = value;
                    CheckBox chkSemDocente = (CheckBox)row.FindControl("chkSemDocente");
                    if (chkSemDocente != null)
                        chkSemDocente.Enabled = value;
                    CheckBox chkAlunoDef = (CheckBox)row.FindControl("chkAlunoDef");
                    if (chkAlunoDef != null)
                        chkAlunoDef.Enabled = value;
                    Repeater rptDocentes = (Repeater)row.FindControl("rptDocentes");
                    if (rptDocentes != null)
                    {
                        foreach (RepeaterItem item in rptDocentes.Items)
                        {
                            GestaoEscolar.WebControls.ControleVigenciaDocentes.ControleVigenciaDocentes UCControleVigenciaDocentes = (GestaoEscolar.WebControls.ControleVigenciaDocentes.ControleVigenciaDocentes)item.FindControl("UCControleVigenciaDocentes");
                            if (UCControleVigenciaDocentes != null)
                            {
                                UCControleVigenciaDocentes.PermiteEditar = value;
                            }
                        }
                    }
                    CheckBoxList chkAvaliacoesPeriodicas = (CheckBoxList)row.FindControl("chkAvaliacoesPeriodicas");
                    if (chkAvaliacoesPeriodicas != null)
                        chkAvaliacoesPeriodicas.Enabled = value;
                    Repeater rptPeriodos = (Repeater)row.FindControl("rptPeriodos");
                    if (rptPeriodos != null)
                    {
                        foreach (RepeaterItem item in rptPeriodos.Items)
                        {
                            CheckBox chkPeriodo = (CheckBox)item.FindControl("chkPeriodo");
                            if (chkPeriodo != null)
                                chkPeriodo.Enabled = value;
                        }
                    }
                }
            }
        }
    }

    #endregion Propriedades

    #region Métodos

    /// <summary>
    /// Carrega no repeater de disciplinas da turma conforme a nova matriz curricular.
    /// </summary>
    /// <param name="cur_id">id do curso</param>
    /// <param name="crr_id">id do currículo</param>
    /// <param name="crp_id">id do currículo período</param>
    /// <param name="tipo">tipo de disciplina (ex: 1–Obrigatória,3–Optativa...)</param>
    /// <param name="tur_id">id da turma</param>
    /// <param name="cal_id">Calendário</param>
    /// <param name="esc_id">Id da escola</param>
    /// <param name="uni_id">Unidade da escola</param>
    /// <param name="ProfessorEspecialista">Flag que indica se turma é para professor especialista</param>
    /// <param name="dtDocentes">Tabela de docentes da escola</param>
    /// <param name="dtAvaliacoesFormato">Tabela com avaliações periódicas do formato - será mostrada na coluna controle semestral</param>
    /// <param name="dtDisciplinaNaoAvaliado">Tabela de disciplinas não avaliadas - todas as disciplinas da turma</param>
    /// <param name="bloqueioAtribuicaoDocente">Flag que indica se é pra bloquear a atribuição de docente para a escola</param>
    /// <param name="tabelaPeriodosCalendario">Tabela com períodos do calendário da turma</param>
    /// <param name="cur_idDestino">Id do curso da nova matriz curricular.</param>
    /// <param name="crr_idDestino">Id do currículo da nova matriz curricular.</param>
    public bool CarregaRepeaterDisciplinasNovaMatrizCurricular
    (
        Int32 cur_id
        , Int32 crr_id
        , Int32 crp_id
        , ACA_CurriculoDisciplinaTipo tipo
        , Int64 tur_id
        , Int32 cal_id
        , Int32 esc_id
        , Int32 uni_id
        , bool ProfessorEspecialista
        , ref DataTable dtDocentes
        , DataTable dtAvaliacoesFormato
        , ref DataTable dtDisciplinaNaoAvaliado
        , bool bloqueioAtribuicaoDocente
        , DataTable tabelaPeriodosCalendario
        , Int32 cur_idDestino
        , Int32 crr_idDestino
        , ref DataTable dtVigenciasDocentes
        , bool aplicarNovaRegraDocenciaCompartilhada
    )
    {
        dtDocentesEscola = dtDocentes;
        DtAvaliacoesFormato = dtAvaliacoesFormato;
        DtDisciplinaNaoAvaliado = dtDisciplinaNaoAvaliado;
        dtPeriodosCalendario = tabelaPeriodosCalendario;
        DtVigenciasDocentes = dtVigenciasDocentes;

        if ((DtDisciplinaNaoAvaliado == null) && (tur_id > 0))
        {
            // Carregar avaliações que devem ser desconsideradas para a disciplina.
            DtDisciplinaNaoAvaliado = TUR_TurmaDisciplinaNaoAvaliadoBO.GetSelectBy_Turma(tur_id);
        }

        // Variáveis que carregam o combo de professor.
        escola_esc_id = esc_id;
        escola_uni_id = uni_id;
        escola_bloqueioAtribuicaoDocente = bloqueioAtribuicaoDocente;
        professorEspecialista = ProfessorEspecialista;

        VS_AcessoControleSemestral = TUR_TurmaBO.VerificaAcessoControleSemestral(tur_id);

        bool mostraAvaliacoes = (DtAvaliacoesFormato != null && DtAvaliacoesFormato.Rows.Count > 0) && VS_AcessoControleSemestral;

        // Carregando as disciplinas de acordo como o tipo dado.
        DataTable dtDisciplina = ACA_CurriculoDisciplinaBO.SelecionaCursosPorNovaMatrizCurricularTipo(cur_id, crr_id, crp_id, tipo, tur_id, cur_idDestino, crr_idDestino);

        lblMensagemControleSemestral.Visible = mostraAvaliacoes && dtDisciplina.Rows.Count > 0;
        lblMensagemControleSemestral.Text = TextoControleSemestral;

        // Guarda todas as disciplinas da turma
        string tud_ids = string.Join(",", (from DataRow dr in dtDisciplina.Rows
                                           select dr["tud_id"].ToString()).ToArray());

        listTurmaDocentes = TUR_TurmaDocenteBO.SelecionaDocentesDisciplina(tud_ids);

        // Se existir disciplina eletiva
        if (dtDisciplina.Rows.Count > 0)
        {
            lblSemDisciplinasEletivas.Visible = false;
            rptDisciplinasEletivas.Visible = true;

            this.aplicarNovaRegraDocenciaCompartilhada = aplicarNovaRegraDocenciaCompartilhada;
            rptDisciplinasEletivas.DataSource = dtDisciplina;
            rptDisciplinasEletivas.DataBind();

            foreach (string id in periodosIds)
            {
                Script += "setaCheckBoxPeriodo('." + id + "');";
            }

            return true;
        }

        lblSemDisciplinasEletivas.Visible = true;
        rptDisciplinasEletivas.Visible = false;

        // Volta o valor das tabelas (caso tenham sido carregados no DataBind do grid).
        dtDocentes = dtDocentesEscola;
        dtDisciplinaNaoAvaliado = DtDisciplinaNaoAvaliado;

        return false;
    }

    /// <summary>
    /// Carrega no repeater de disciplinas eletivas da turma
    /// </summary>
    /// <param name="cur_id">id do curso</param>
    /// <param name="crr_id">id do currículo</param>
    /// <param name="crp_id">id do currículo período</param>
    /// <param name="tipo">tipo de disciplina (ex: 1–Obrigatória,3–Optativa...)</param>
    /// <param name="tur_id">id da turma</param>
    /// <param name="cal_id">Calendário</param>
    /// <param name="esc_id">Id da escola</param>
    /// <param name="uni_id">Unidade da escola</param>
    /// <param name="ProfessorEspecialista">Flag que indica se turma é para professor especialista</param>
    /// <param name="dtDocentes">Tabela de docentes da escola</param>
    /// <param name="dtAvaliacoesFormato">Tabela com avaliações periódicas do formato - será mostrada na coluna controle semestral</param>
    /// <param name="dtDisciplinaNaoAvaliado">Tabela de disciplinas não avaliadas - todas as disciplinas da turma</param>
    /// <param name="bloqueioAtribuicaoDocente">Flag que indica se é pra bloquear a atribuição de docente para a escola</param>
    /// <param name="tabelaPeriodosCalendario">Tabela com períodos do calendário da turma</param>
    public bool CarregaRepeaterDisciplinas
    (
        Int32 cur_id
        , Int32 crr_id
        , Int32 crp_id
        , ACA_CurriculoDisciplinaTipo tipo
        , Int64 tur_id
        , Int32 cal_id
        , Int32 esc_id
        , Int32 uni_id
        , bool ProfessorEspecialista
        , ref DataTable dtDocentes
        , DataTable dtAvaliacoesFormato
        , ref DataTable dtDisciplinaNaoAvaliado
        , bool bloqueioAtribuicaoDocente
        , DataTable tabelaPeriodosCalendario
        , ref DataTable dtVigenciasDocentes
        , bool aplicarNovaRegraDocenciaCompartilhada
    )
    {
        dtDocentesEscola = dtDocentes;
        DtAvaliacoesFormato = dtAvaliacoesFormato;
        DtDisciplinaNaoAvaliado = dtDisciplinaNaoAvaliado;
        dtPeriodosCalendario = tabelaPeriodosCalendario;
        DtVigenciasDocentes = dtVigenciasDocentes;

        if ((DtDisciplinaNaoAvaliado == null) && (tur_id > 0))
        {
            // Carregar avaliações que devem ser desconsideradas para a disciplina.
            DtDisciplinaNaoAvaliado = TUR_TurmaDisciplinaNaoAvaliadoBO.GetSelectBy_Turma(tur_id);
        }

        // Variáveis que carregam o combo de professor.
        escola_esc_id = esc_id;
        escola_uni_id = uni_id;
        escola_bloqueioAtribuicaoDocente = bloqueioAtribuicaoDocente;
        professorEspecialista = ProfessorEspecialista;

        VS_AcessoControleSemestral = TUR_TurmaBO.VerificaAcessoControleSemestral(tur_id);

        bool mostraAvaliacoes = (DtAvaliacoesFormato != null && DtAvaliacoesFormato.Rows.Count > 0) && VS_AcessoControleSemestral;

        // Carregando as disciplinas de acordo como o tipo dado.
        DataTable dtDisciplina = ACA_CurriculoDisciplinaBO.GetSelectBy_Curso_Tipo
        (
            cur_id
            , crr_id
            , crp_id
            , tipo
            , tur_id
        );

        lblMensagemControleSemestral.Visible = mostraAvaliacoes && dtDisciplina.Rows.Count > 0;
        lblMensagemControleSemestral.Text = TextoControleSemestral;

        // Guarda todas as disciplinas da turma
        string tud_ids = string.Join(",", (from DataRow dr in dtDisciplina.Rows
                                           select dr["tud_id"].ToString()).ToArray());

        listTurmaDocentes = TUR_TurmaDocenteBO.SelecionaDocentesDisciplina(tud_ids);

        // Se existir disciplina eletiva
        if (dtDisciplina.Rows.Count > 0)
        {
            lblSemDisciplinasEletivas.Visible = false;
            rptDisciplinasEletivas.Visible = true;

            this.aplicarNovaRegraDocenciaCompartilhada = aplicarNovaRegraDocenciaCompartilhada;
            rptDisciplinasEletivas.DataSource = dtDisciplina;
            rptDisciplinasEletivas.DataBind();

            foreach (string id in periodosIds)
            {
                Script += "setaCheckBoxPeriodo('." + id + "');";
            }

            return true;
        }

        lblSemDisciplinasEletivas.Visible = true;
        rptDisciplinasEletivas.Visible = false;

        // Volta o valor das tabelas (caso tenham sido carregados no DataBind do grid).
        dtDocentes = dtDocentesEscola;
        dtDisciplinaNaoAvaliado = DtDisciplinaNaoAvaliado;

        return false;
    }

    /// <summary>
    /// Retorna os lists necessários para salvar a turma.
    /// </summary>
    /// <param name="tud_codigo">código da turma disciplina</param>
    /// <param name="tud_vagas">quantidade de vagas turma disciplina</param>
    /// <param name="tud_minimoMatriculados">quantidade mínima de vagas turma disciplina</param>
    /// <param name="tud_duracao">Disciplina duração</param>
    /// <returns>As listas de entidades com as discplinas a serem salvas</returns>
    public List<CadastroTurmaDisciplina> RetornaDisciplinas
    (
        string tud_codigo
        , int tud_vagas
        , int tud_minimoMatriculados
        , byte tud_duracao
    )
    {
        List<CadastroTurmaDisciplina> listTurmaDisciplina = new List<CadastroTurmaDisciplina>();

        // Disciplinas eletivas.
        foreach (RepeaterItem item in rptDisciplinasEletivas.Items)
        {
            CadastroTurmaDisciplina ent = AdicionaDisciplina(item,
                                                             tud_codigo,
                                                             tud_vagas,
                                                             tud_minimoMatriculados,
                                                             tud_duracao);

            listTurmaDisciplina.Add(ent);
        }

        return listTurmaDisciplina;
    }

    /// <summary>
    /// Adiciona nas listas as entidades da disciplina da linha atual.
    /// </summary>
    /// <param name="rptItem">recebe o Item</param>
    /// <param name="tud_codigo">código da turma disciplina</param>
    /// <param name="tud_vagas">quantidade de vagas turma disciplina</param>
    /// <param name="tud_minimoMatriculados">quantidade mínima de vagas turma disciplina</param>
    /// <param name="tud_duracao">Disciplina duração</param>
    /// <returns>As listas de entidades com as discplinas a serem salvas</returns>
    private CadastroTurmaDisciplina AdicionaDisciplina
    (
        RepeaterItem rptItem
        , string tud_codigo
        , int tud_vagas
        , int tud_minimoMatriculados
        , byte tud_duracao
    )
    {
        bool tud_disciplinaEspecial = false;

        CheckBox chkAlunoDef = (CheckBox)rptItem.FindControl("chkAlunoDef");
        if (chkAlunoDef != null)
        {
            tud_disciplinaEspecial = chkAlunoDef.Checked;
        }

        // Declarando variáveis que receberá dados do grid
        Int32 tud_id = String.IsNullOrEmpty(((Label)rptItem.FindControl("lblTud_ID")).Text) ? -1 : Convert.ToInt32(((Label)rptItem.FindControl("lblTud_ID")).Text);

        // Adicionando na entidades os valores a ser salvo.
        TUR_TurmaDisciplina ent = new TUR_TurmaDisciplina
        {
            tud_tipo = String.IsNullOrEmpty(((Label)rptItem.FindControl("lblCrd_Tipo")).Text) ? new byte() : Convert.ToByte(((Label)rptItem.FindControl("lblCrd_Tipo")).Text)
            ,
            tud_global = false
            ,
            tud_nome = ((Label)rptItem.FindControl("lblDis_Nome")).Text
            ,
            tud_codigo = tud_codigo
            ,
            tud_id = tud_id
            ,
            tud_situacao = (byte)TurmaDisciplinaSituacao.Ativo
            ,
            tud_cargaHorariaSemanal = String.IsNullOrEmpty(((TextBox)rptItem.FindControl("txtAulaSemanal")).Text) ? 0 : Convert.ToInt32(((TextBox)rptItem.FindControl("txtAulaSemanal")).Text)
            ,
            tud_multiseriado = false
            ,
            tud_vagas = tud_vagas
            ,
            tud_minimoMatriculados = tud_minimoMatriculados
            ,
            tud_duracao = tud_duracao
            ,
            tud_modo = (byte)TurmaDisciplinaModo.Normal
            ,
            tud_aulaForaPeriodoNormal = false
            ,
            tud_disciplinaEspecial = tud_disciplinaEspecial
            ,
            tud_semProfessor = ((CheckBox)rptItem.FindControl("chkSemDocente")).Checked
            ,
            IsNew = tud_id <= 0
        };

        // Adicionando valores na entidade de relacionemento.
        TUR_TurmaDisciplinaRelDisciplina relDis = new TUR_TurmaDisciplinaRelDisciplina
        {
            dis_id = Convert.ToInt32(((Label)rptItem.FindControl("lblDis_ID")).Text)
            ,
            tud_id = ent.tud_id
            ,
            IsNew = tud_id <= 0
        };

        List<TUR_TurmaDisciplinaCalendario> turCal = AdicionaPeriodos(rptItem, ent);
        // TUR_TurmaDocente doc = AdicionaDocentes(rptItem, ent);

        // Setar cde_id = id do grupo de disciplinas eletivas.
        string id = ((Label)rptItem.FindControl("lblCde_id")).Text;
        Int32 cde_id = -1;
        if (!String.IsNullOrEmpty(id))
        {
            cde_id = Convert.ToInt32(id);
        }

        // Avaliações que não serão avaliadas.
        CheckBoxList chkList = (CheckBoxList)rptItem.FindControl("chkAvaliacoesPeriodicas");
        List<TUR_TurmaDisciplinaNaoAvaliado> lista =
            (from ListItem it in chkList.Items
             where it.Selected
             select new TUR_TurmaDisciplinaNaoAvaliado
                        {
                            tud_id = ent.tud_id
                            ,
                            fav_id = Convert.ToInt32(it.Value.Split(';')[0])
                            ,
                            ava_id = Convert.ToInt32(it.Value.Split(';')[1])
                        }
            ).ToList();

        Repeater rptDocentes = (Repeater)rptItem.FindControl("rptDocentes");
        List<TUR_Turma_Docentes_Disciplina> listDocentesPosicoes = new List<TUR_Turma_Docentes_Disciplina>();

        foreach (RepeaterItem itemD in rptDocentes.Items)
        {
            GestaoEscolar.WebControls.ControleVigenciaDocentes.ControleVigenciaDocentes UCControleVigenciaDocentes =
                                                (GestaoEscolar.WebControls.ControleVigenciaDocentes.ControleVigenciaDocentes)itemD.FindControl("UCControleVigenciaDocentes");
            byte posicao = Convert.ToByte(((Label)itemD.FindControl("lblposicao")).Text);

            UCControleVigenciaDocentes.RetornaDocentesPosicao(ref listDocentesPosicoes, posicao, ent.tud_id);
        }

        return new CadastroTurmaDisciplina
        {
            entTurmaDisciplina = ent
            ,
            entTurmaDiscRelDisciplina = relDis
            ,
            listaTurmaDocente = listDocentesPosicoes
            ,
            entTurmaCalendario = turCal
            ,
            cde_id = cde_id
            ,
            listaAvaliacoesNaoAvaliar = lista
        };
    }

    /// <summary>
    /// Adiciona na lista as entidades de PeriodoCalendario de acordo com os dados na linha.
    /// </summary>
    private List<TUR_TurmaDisciplinaCalendario> AdicionaPeriodos(RepeaterItem rptItem, TUR_TurmaDisciplina ent)
    {
        List<TUR_TurmaDisciplinaCalendario> lista = new List<TUR_TurmaDisciplinaCalendario>();

        if (rptItem != null)
        {
            Repeater rptPeriodos = (Repeater)rptItem.FindControl("rptPeriodos");

            foreach (RepeaterItem itemPeriodo in rptPeriodos.Items)
            {
                CheckBox chkPeriodo = (CheckBox)itemPeriodo.FindControl("chkPeriodo");

                if (chkPeriodo.Checked)
                {
                    Int32 tpc_id = Convert.ToInt32(((Label)itemPeriodo.FindControl("lbl_tpc_id")).Text);

                    TUR_TurmaDisciplinaCalendario turCal = new TUR_TurmaDisciplinaCalendario
                    {
                        tud_id = ent.tud_id,
                        tpc_id = tpc_id
                    };

                    lista.Add(turCal);
                }
            }
        }

        return lista;
    }

    /// <summary>
    /// Carrega o repeater com a quantidade de docentes definida no parâmetro acadêmico,
    /// com o controle de vigência de cada um deles.
    /// </summary>
    /// <param name="Row">Linha do repeater de disciplinas</param>
    /// <param name="tud_id">Id da disciplina</param>
    /// <param name="tds_id">Id do tipo de disciplina para carregar o docente por especialidade</param>
    private void CarregarControleDocentes(RepeaterItem Row, long tud_id, int tds_id)
    {
        int qtdeDocentes = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.QUANTIDADE_DOCENTES_VIGENTES_DISCIPLINA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

        DataTable dtDocentes = new DataTable();
        dtDocentes.Columns.Add("posicao");
        dtDocentes.Columns.Add("tud_id");
        dtDocentes.Columns.Add("qtdedocentes");
        dtDocentes.Columns.Add("tds_id");

        for (int i = 1; i <= qtdeDocentes; i++)
        {
            EnumTipoDocente tipoDocente = ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao((byte)i, ApplicationWEB.AppMinutosCacheLongo);
            if (!aplicarNovaRegraDocenciaCompartilhada
                || (tipoDocente != EnumTipoDocente.Compartilhado && tipoDocente != EnumTipoDocente.Projeto))
            {
                DataRow dr = dtDocentes.NewRow();
                dr["posicao"] = i;
                dr["tud_id"] = tud_id;
                dr["qtdedocentes"] = qtdeDocentes;
                dr["tds_id"] = tds_id;
                dtDocentes.Rows.Add(dr);
            }
        }

        Repeater rptDocentes = (Repeater)Row.FindControl("rptDocentes");
        rptDocentes.DataSource = dtDocentes;
        rptDocentes.DataBind();
    }

    #endregion Métodos

    #region Eventos

    protected void rptDocentes_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        long tud_id = Convert.ToInt64(((Label)e.Item.FindControl("lbltud_id")).Text);
        byte posicao = Convert.ToByte(((Label)e.Item.FindControl("lblposicao")).Text);
        int qtdedocentes = Convert.ToInt32(((Label)e.Item.FindControl("lblqtdedocentes")).Text);
        int tds_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tds_id"));

        GestaoEscolar.WebControls.ControleVigenciaDocentes.ControleVigenciaDocentes UCControleVigenciaDocentes =
                                                (GestaoEscolar.WebControls.ControleVigenciaDocentes.ControleVigenciaDocentes)e.Item.FindControl("UCControleVigenciaDocentes");

        TUR_Turma_Docentes_Disciplina entity = listTurmaDocentes.Find(p => (p.entDocente.tud_id == tud_id && p.entDocente.tdt_posicao == posicao && p.entDocente.tdt_situacao == 1));

        UCControleVigenciaDocentes.CarregarDocente
            (entity.doc_nome, posicao, qtdedocentes, tud_id, ref dtDocentesEscola, tds_id
            , escola_esc_id, escola_uni_id, professorEspecialista, escola_bloqueioAtribuicaoDocente, ref DtVigenciasDocentes);
    }

    protected void rptDisciplinasEletivas_DataBound(object source, RepeaterItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.Header) ||
            (e.Item.ItemType == ListItemType.Item) ||
            (e.Item.ItemType == ListItemType.AlternatingItem))
        {
            // Seta o tud_id para ser usado no Repeater de períodos.
            Tud_ID = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tud_id"));

            // Atualiza o repeater de periodos para o calendário selecionado no combo.
            Repeater rptPeriodos = (Repeater)e.Item.FindControl("rptPeriodos");

            if (e.Item.ItemType != ListItemType.Header)
            {
                // Recebendo o indice da linha do repeater Disciplina eletiva.
                indiceRepeater = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "cde_id"));
            }

            var dataSourcePeriodos = from DataRow dr in dtPeriodosCalendario.Rows
                                     group dr by Convert.ToInt32(dr["tpc_id"])
                                         into g
                                         select new
                                                    {
                                                        tpc_id = g.Key
                                                        ,
                                                        tpc_nome = g.First()["tpc_nome"].ToString()
                                                    };

            rptPeriodos.DataSource = dataSourcePeriodos;
            rptPeriodos.DataBind();

            string idTd = (e.Item.ItemType == ListItemType.Header
                               ? "tdAvaliacoesPeriodicasHeader"
                               : "tdAvaliacoesPeriodicas");

            HtmlTableCell tdAvaliacoesPeriodicas = (HtmlTableCell)e.Item.FindControl(idTd);
            bool visibleAvaliacoes = (DtAvaliacoesFormato != null && DtAvaliacoesFormato.Rows.Count > 0) && VS_AcessoControleSemestral;
            tdAvaliacoesPeriodicas.Visible = visibleAvaliacoes;

            if (e.Item.ItemType != ListItemType.Header)
            {
                int tds_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tds_id"));
                long tud_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "tud_id"));
                Int16 separaDispEscpecial = Convert.ToInt16(DataBinder.Eval(e.Item.DataItem, "tds_separaDispEspecial"));
                bool disciplinaEspecial = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "tud_disciplinaEspecial"));

                CheckBoxList list = (CheckBoxList)e.Item.FindControl("chkAvaliacoesPeriodicas");
                if (list != null)
                {
                    list.DataSource = DtAvaliacoesFormato;
                    list.DataBind();
                }

                if ((tud_id > 0) && (DtDisciplinaNaoAvaliado != null))
                {
                    // Selecionar os itens que já estão marcados para não avaliar.
                    foreach (ListItem item in list.Items)
                    {
                        item.Selected =
                            (from DataRow dr in DtDisciplinaNaoAvaliado.Rows
                             where dr["fav_id"] + ";" + dr["ava_id"] == item.Value
                             && Convert.ToInt64(dr["tud_id"]) == tud_id
                             select new { value = item.Value }
                            ).Count() > 0;
                    }
                }

                // Carrega o user control de controle de docentes.
                CarregarControleDocentes(e.Item, tud_id, tds_id);

                // Seta visible combo deficiente
                if (separaDispEscpecial == 1)
                {
                    CheckBox chkAlunoDef = (CheckBox)e.Item.FindControl("chkAlunoDef");
                    if (chkAlunoDef != null)
                    {
                        chkAlunoDef.Visible = true;
                        chkAlunoDef.Checked = disciplinaEspecial;
                    }
                }

                // Verifica se é eletiva de alguma outra disciplina.
                Int32 cde_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "cde_id"));

                if (cde_id != cde_idAnterior)
                {
                    // Linha da tabela.
                    HtmlTableRow trSeparator = (HtmlTableRow)e.Item.FindControl("trSeparator");
                    HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("tdSeparator");
                    td.ColSpan = 4 + dtPeriodosCalendario.Rows.Count;

                    trSeparator.Visible = true;
                }

                cde_idAnterior = cde_id;
            }
        }
        else if (e.Item.ItemType == ListItemType.Separator)
        {
            e.Item.Visible = false;
        }
    }

    protected void rptPeriodos_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.AlternatingItem) ||
            (e.Item.ItemType == ListItemType.Item))
        {
            CheckBox chkPeriodo = (CheckBox)e.Item.FindControl("chkPeriodo");

            string classeCss = rptDisciplinasEletivas.ClientID + "-" + indiceRepeater + "-" + DataBinder.Eval(e.Item.DataItem, "tpc_id");

            periodosIds.Add(classeCss);
            chkPeriodo.CssClass += " " + classeCss;

            if (Tud_ID > 0)
            {
                // Verifica se a disciplina está marcada nesse período
                var linhaDisciplina = from DataRow dr in dtPeriodosCalendario.Rows
                                      where Convert.ToInt64(dr["tud_id"]) == Tud_ID
                                            && Convert.ToInt32(dr["tpc_id"]) ==
                                            Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tpc_id"))
                                      select new
                                                 {
                                                     DisciplinaMarcada = Convert.ToBoolean(dr["DisciplinaMarcada"])
                                                 };

                if (linhaDisciplina.Count() > 0)
                {
                    // Checar checkBox do repeater.
                    chkPeriodo.Checked = linhaDisciplina.FirstOrDefault().DisciplinaMarcada;
                }
            }
        }
    }

    #endregion Eventos
}