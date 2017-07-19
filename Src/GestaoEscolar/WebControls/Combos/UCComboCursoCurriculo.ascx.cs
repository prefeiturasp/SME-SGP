using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboCursoCurriculo : MotherUserControl
{
    #region DELEGATES

    public delegate void SelectedIndexChanged();

    public event SelectedIndexChanged IndexChanged;

    public delegate void SelectedIndexChange_Sender(object sender, EventArgs e);

    public event SelectedIndexChange_Sender IndexChanged_Sender;

    #endregion DELEGATES

    #region PROPRIEDADES

    private bool cancelSelect;

    /// <summary>
    /// Cancela o consulta do ObjectDataSource ao carregar a página pela primeira vez.
    /// </summary>
    public bool CancelSelect
    {
        get { return cancelSelect; }
        set { cancelSelect = value; }
    }

    /// <summary>
    /// Propriedade que seta o Width do combo.
    /// </summary>
    public Int32 WidthCombo
    {
        set
        {
            ddlCombo.Width = value;
        }
    }

    /// <summary>
    /// Propriedade que retorna a quantidade de itens no Combo
    /// </summary>
    public int QuantidadeItensCombo
    {
        get
        {
            return ddlCombo.Items.Count;
        }
    }

    /// <summary>
    /// Propriedade que seta o SelectedIndex do combo
    /// </summary>
    public int SelectedIndex
    {
        set
        {
            ddlCombo.SelectedValue = ddlCombo.Items[value].Value;
        }
    }

    /// <summary>
    /// Retorna e seta o valor selecionado no combo.
    /// valor[0] = cur_id
    /// valor[1] = crr_id
    /// </summary>
    public Int32[] Valor
    {
        get
        {
            string[] s = ddlCombo.SelectedValue.Split(';');

            if (s.Length == 2)
                return new[] { Convert.ToInt32(s[0]), Convert.ToInt32(s[1]) };

            return new[] { -1, -1 };
        }
        set
        {
            string s;
            if (value.Length == 2)
                s = value[0] + ";" + value[1];
            else
                s = "-1;-1";

            ddlCombo.SelectedValue = s;
        }
    }

    /// <summary>
    /// Retorna o Cur_ID selecionado no combo.
    /// </summary>
    public int Cur_ID
    {
        get
        {
            return Valor[0];
        }
    }

    /// <summary>
    /// Retorna o Crr_ID selecionado no combo.
    /// </summary>
    public int Crr_ID
    {
        get
        {
            return Valor[1];
        }
    }

    /// <summary>
    /// Retorna o texto selecionado no combo
    /// </summary>
    public string Texto
    {
        get
        {
            return ddlCombo.SelectedItem.ToString();
        }
    }

    /// <summary>
    /// Atribui valor para o skin do combo
    /// </summary>
    public string SkinIDCombo
    {
        set
        {
            ddlCombo.SkinID = value;
        }
    }

    /// <summary>
    /// Propriedade visible da label do nome do combo
    /// </summary>
    public bool LabelVisible
    {
        set
        {
            lblTitulo.Visible = value;
        }
    }

    /// <summary>
    /// Propriedade que seta a label e a validação do combo
    /// </summary>
    public bool Obrigatorio
    {
        set
        {
            if (value)
            {
                AdicionaAsteriscoObrigatorio(lblTitulo);
            }
            else
            {
                RemoveAsteriscoObrigatorio(lblTitulo);
            }
            cpvCombo.Visible = value;
        }
    }

    /// <summary>
    /// Propriedade que seta asterísco no label, porém sem o Compare Validator
    /// </summary>
    public bool AsteriscoObg
    {
        set
        {
            if (value)
            {
                AdicionaAsteriscoObrigatorio(lblTitulo);
            }
            else
            {
                RemoveAsteriscoObrigatorio(lblTitulo);
            }
        }
    }

    /// <summary>
    /// Seta o validationGroup do combo.
    /// </summary>
    public string ValidationGroup
    {
        set
        {
            cpvCombo.ValidationGroup = value;
        }
    }

    /// <summary>
    /// ClientID do combo
    /// </summary>
    public string Combo_ClientID
    {
        get
        {
            return ddlCombo.ClientID;
        }
    }

    /// <summary>
    /// Deixa o combo habilitado de acordo com o valor passado
    /// </summary>
    public bool PermiteEditar
    {
        get
        {
            return ddlCombo.Enabled;
        }
        set
        {
            ddlCombo.Enabled = value;
        }
    }

    /// <summary>
    /// Exibe ou não o combo, o titulo e o validador de acordo com o valor passado
    /// </summary>
    public bool ExibeCombo
    {
        set
        {
            ddlCombo.Visible = value;
            lblTitulo.Visible = value;
            cpvCombo.Visible = value;
        }
    }

    /// <summary>
    /// Adciona e remove a mensagem "Selecione um Cruso" do dropdownlist.
    /// Por padrão é false e a mensagem "Selecione um Curso" não é exibida.
    /// </summary>
    public bool MostrarMessageSelecione
    {
        set
        {
            if (value && __SessionWEB != null && __SessionWEB.__UsuarioWEB != null && __SessionWEB.__UsuarioWEB.Usuario != null)
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1", true));
            ddlCombo.AppendDataBoundItems = value;
        }
    }

    /// <summary>
    /// Retorna o título do combo (nome do Curso no sistema).
    /// </summary>
    public string Titulo
    {
        get
        {
            return lblTitulo.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");
        }
    }

    public string ComboID
    {
        get { return ddlCombo.ID; }
    }

    /// <summary>
    /// Propriedade para controlar o viewstate dos controles:
    /// odsDados
    /// </summary>
    public bool GuardaViewState
    {
        get
        {
            return odsDados.EnableViewState;
        }
        set
        {
            odsDados.EnableViewState = value;
        }
    }

    #endregion PROPRIEDADES

    #region METODOS

    /// <summary>
    /// Seta o foco no combo
    /// </summary>
    public void SetarFoco()
    {
        ddlCombo.Focus();
    }

    /// <summary>
    /// Verifica se o combo possui somente um item e seleciona o primeiro.
    /// </summary>
    public bool SelecionaPrimeiroItem()
    {
        if ((QuantidadeItensCombo == 2) &&
            (Valor[0] == -1))
        {
            // Seleciona o primeiro item.
            ddlCombo.SelectedValue = ddlCombo.Items[1].Value;

            if (IndexChanged != null)
                IndexChanged();

            return true;
        }

        return false;
    }

    /// <summary>
    /// Verifica se existe o valor no dropdownlist
    /// </summary>
    /// <param name="crr_id">ID do curso</param>
    /// <param name="cur_id">ID do curriculo</param>
    public bool ExisteItem
    (
        int cur_id
        , int crr_id
    )
    {
        return (ddlCombo.Items.FindByValue(cur_id + ";" + crr_id) != null);
    }

    /// <summary>
    /// Carrega todos os cursos/currículos não excluídos logicamente no combo que permitem efetivação semestral
    /// </summary>
    public void CarregarCursoCurriculoEfetivacaoSemestral()
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_Curso";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_CursoBO";
        odsDados.SelectMethod = "CursoCurriculoEfetivacaoSemestral";
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1", true));
        ddlCombo.DataBind();
    }


    /// <summary>
    /// Carrega todos os cursos/currículos não excluídos logicamente no combo
    /// </summary>
    public void CarregarCursoCurriculo(bool mostraEJAModalidades = false)
    {
        CarregarCursoCurriculoPorEscola(-1, -1, 0, mostraEJAModalidades);
    }

    /// <summary>
    /// Carrega cursos/currículos não excluídos logicamente no combo
    /// com a situação informada
    /// </summary>
    public void CarregarCursoCurriculoSituacao(byte cur_situacao, bool mostraEJAModalidades = false)
    {
        CarregarCursoCurriculoPorEscola(-1, -1, cur_situacao, mostraEJAModalidades);
    }

    /// <summary>
    /// Carrega todos os cursos/currículos não excluídos logicamente no combo
    /// Filtrando por escola do curso, unidade da escola e situação do curso
    /// </summary>
    /// <param name="esc_id">Escola do curso</param>
    /// <param name="uni_id">Unidade da escola </param>
    /// <param name="cur_situacao">Situação do curso</param>
    public void CarregarCursoCurriculoPorEscola
    (
        int esc_id
        , int uni_id
        , byte cur_situacao
        , bool mostraEJAModalidades = false
    )
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_Curso";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_CursoBO";
        odsDados.SelectMethod = "SelecionaCursoCurriculo";
        odsDados.SelectParameters.Add("esc_id", esc_id.ToString());
        odsDados.SelectParameters.Add("uni_id", uni_id.ToString());
        odsDados.SelectParameters.Add("cur_situacao", cur_situacao.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("mostraEJAModalidades", mostraEJAModalidades.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Carrega todos os cursos/currículos não excluídos logicamente no combo
    /// Filtrando pelo acesso do usuário
    /// </summary>
    /// <param name="cur_situacao">Situação do curso</param>
    public void CarregarCursoCurriculoPorUsuario
    (
        byte cur_situacao = 0
    )
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_Curso";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_CursoBO";
        odsDados.SelectMethod = "SelecionaCursoCurriculoPorUsuario";
        odsDados.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        odsDados.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
        odsDados.SelectParameters.Add("cur_situacao", cur_situacao.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Carrega todos os cursos/currículos não excluídos logicamente no combo
    /// Filtrando (ou não) por escola e situação do curso que possuem disciplina eletiva
    /// </summary>
    /// <param name="esc_id">Escola do curso</param>
    /// <param name="uni_id">Unidade da escola </param>
    public void CarregarCursoComDisciplinaEletiva
    (
        int esc_id
        , int uni_id
        , int cur_situacao = 0
        , bool mostraEJAModalidades = false
    )
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_Curso";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_CursoBO";
        odsDados.SelectMethod = "SelectCursoComDisciplinaEletiva";
        odsDados.SelectParameters.Add("esc_id", esc_id.ToString());
        odsDados.SelectParameters.Add("uni_id", uni_id.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("cur_situacao", cur_situacao.ToString());
        odsDados.SelectParameters.Add("mostraEJAModalidades", mostraEJAModalidades.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Carrega os cursos relacionados ao curso informado, que tenham ligação com a escola informada.
    /// </summary>
    /// <param name="cur_id">ID do curso</param>
    /// <param name="crr_id">ID do currículo do curso</param>
    /// <param name="esc_id">ID da escola</param>
    /// <param name="uni_id">ID da unidade da escola</param>
    /// <param name="somenteAtivos">True - Trazer os cursos relacionados ativos / False - Trazer os cursos relacionados não excluídos logicamente</param>
    /// <returns></returns>
    public void CarregaCursos_Relacionados_Por_Escola(int cur_id, int crr_id, int esc_id, int uni_id, bool somenteAtivos)
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_Curso";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_CursoBO";
        odsDados.SelectMethod = "Seleciona_CursosRelacionados_Por_Escola";
        odsDados.SelectParameters.Add("cur_id", cur_id.ToString());
        odsDados.SelectParameters.Add("crr_id", crr_id.ToString());
        odsDados.SelectParameters.Add("esc_id", esc_id.ToString());
        odsDados.SelectParameters.Add("uni_id", uni_id.ToString());
        odsDados.SelectParameters.Add("somenteAtivos", somenteAtivos.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Carrega os cursos relacionados ao curso informado, que tenham ligação com a escola informada
    /// e que estejam vigentes.
    /// </summary>
    /// <param name="cur_id">ID do curso</param>
    /// <param name="crr_id">ID do currículo do curso</param>
    /// <param name="esc_id">ID da escola</param>
    /// <param name="uni_id">ID da unidade da escola</param>
    /// <returns></returns>
    public void CarregaCursos_RelacionadosVigentes_Por_Escola(int cur_id, int crr_id, int esc_id, int uni_id)
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_Curso";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_CursoBO";
        odsDados.SelectMethod = "Seleciona_CursosRelacionadosVigentes_Por_Escola";
        odsDados.SelectParameters.Add("cur_id", cur_id.ToString());
        odsDados.SelectParameters.Add("crr_id", crr_id.ToString());
        odsDados.SelectParameters.Add("esc_id", esc_id.ToString());
        odsDados.SelectParameters.Add("uni_id", uni_id.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Carrega os cursos relacionados ao curso informado, que tenham ligação com a escola informada. (exceto ele mesmo)
    /// </summary>
    /// <param name="cur_id">ID do curso</param>
    /// <param name="crr_id">ID do currículo do curso</param>
    /// <param name="esc_id">ID da escola</param>
    /// <param name="uni_id">ID da unidade da escola</param>
    /// <param name="somenteAtivos">True - Trazer os cursos relacionados ativos / False - Trazer os cursos relacionados não excluídos logicamente</param>
    /// <returns></returns>
    public void CarregaApenasCursosRelacionadosPorEscola(int cur_id, int crr_id, int esc_id, int uni_id, bool somenteAtivos)
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_Curso";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_CursoBO";
        odsDados.SelectMethod = "SelecionaApenasCursosRelacionadosPorEscola";
        odsDados.SelectParameters.Add("cur_id", cur_id.ToString());
        odsDados.SelectParameters.Add("crr_id", crr_id.ToString());
        odsDados.SelectParameters.Add("esc_id", esc_id.ToString());
        odsDados.SelectParameters.Add("uni_id", uni_id.ToString());
        odsDados.SelectParameters.Add("somenteAtivos", somenteAtivos.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Carrega todos os cursos/currículos não excluídos logicamente no combo
    /// Filtrando por escola do curso, unidade da escola e situação do curso
    /// </summary>
    /// <param name="esc_id">Escola do curso</param>
    /// <param name="uni_id">Unidade da escola </param>
    /// <param name="cur_situacao">Situação do curso</param>
    /// <param name="cal_id">Calendario</param>
    public void CarregarCursoCurriculoPorEscolaCalendario
    (
        int esc_id
        , int uni_id
        , byte cur_situacao
        , int cal_id
        , bool mostraEJAModalidades = false
    )
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_Curso";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_CursoBO";
        odsDados.SelectMethod = "SelecionaCursoCurriculoCalendarioEscola";
        odsDados.SelectParameters.Add("esc_id", esc_id.ToString());
        odsDados.SelectParameters.Add("uni_id", uni_id.ToString());
        odsDados.SelectParameters.Add("cur_situacao", cur_situacao.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("cal_id", cal_id.ToString());
        odsDados.SelectParameters.Add("mostraEJAModalidades", mostraEJAModalidades.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Carrega todos os cursos/currículos não excluídos logicamente no combo
    /// Filtrando por escola do curso, unidade da escola e situação do curso
    /// </summary>
    /// <param name="esc_id">Escola do curso</param>
    /// <param name="uni_id">Unidade da escola </param>
    /// <param name="dis_id">Disciplina</param>
    /// <param name="cur_situacao">Situação do curso</param>
    /// <param name="cal_id">Calendario</param>
    public void CarregarCursoCurriculoPorEscolaCalendarioDisciplina
    (
        int esc_id
        , int uni_id
        , int dis_id
        , byte cur_situacao
        , int cal_id
    )
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_Curso";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_CursoBO";
        odsDados.SelectMethod = "SelecionaCursoCurriculoCalendarioEscolaDisciplina";
        odsDados.SelectParameters.Add("esc_id", esc_id.ToString());
        odsDados.SelectParameters.Add("uni_id", uni_id.ToString());
        odsDados.SelectParameters.Add("dis_id", dis_id.ToString());
        odsDados.SelectParameters.Add("cur_situacao", cur_situacao.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("cal_id", cal_id.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Carrega todos os cursos/currículos do PEJA não excluídos logicamente no combo
    /// Filtrando por escola do curso, unidade da escola e situação do curso
    /// </summary>
    /// <param name="esc_id">Escola do curso</param>
    /// <param name="uni_id">Unidade da escola </param>
    /// <param name="cur_situacao">Situação do curso</param>
    public void CarregarCursoCurriculoPorEscolaPEJA(int esc_id, int uni_id, byte cur_situacao)
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_Curso";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_CursoBO";
        odsDados.SelectMethod = "SelectCursoCurriculoPorEscolaPEJA";
        odsDados.SelectParameters.Add("esc_id", esc_id.ToString());
        odsDados.SelectParameters.Add("uni_id", uni_id.ToString());
        odsDados.SelectParameters.Add("cur_situacao", cur_situacao.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1", true));
        ddlCombo.DataBind();    
    }

    /// <summary>
    /// Carrega os cursos relacionados a  modalidade de ensino informada
    /// </summary>
    /// <param name="tme_id">ID do tipo da modalidade de ensino</param>
    /// <returns></returns>
    public void CarregaCursos_Por_TipoModalidadeEnsino(int tme_id, int esc_id, int uni_id)
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_Curso";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_CursoBO";
        odsDados.SelectMethod = "Seleciona_Cursos_Por_ModalidadeEnsino";
        odsDados.SelectParameters.Add("tme_id", tme_id.ToString());
        odsDados.SelectParameters.Add("esc_id", esc_id.ToString());
        odsDados.SelectParameters.Add("uni_id", uni_id.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1", true));
        ddlCombo.DataBind();
    }

    #endregion METODOS

    #region EVENTOS

    protected void Page_Init(object sender, EventArgs e)
    {
        bool obrigatorio = lblTitulo.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio) ||
                   lblTitulo.Text.EndsWith(" *");

        //Altera o Label para o nome padrão de curso no sistema
        lblTitulo.Text = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
        //Altera a mensagem de validação para o nome padrão de curso no sistema
        cpvCombo.ErrorMessage = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " é obrigatório.";

        Obrigatorio = obrigatorio;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ddlCombo.AutoPostBack = (IndexChanged != null) || (IndexChanged_Sender != null);
    }

    /// <summary>
    /// Evento na mudança de item no combo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IndexChanged != null)
            IndexChanged();

        if (IndexChanged_Sender != null)
            IndexChanged_Sender(sender, e);
    }

    protected void odsDados_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            // Grava o erro e mostr pro usuário.
            ApplicationWEB._GravaErro(e.Exception.InnerException);

            e.ExceptionHandled = true;
            lblMessage.Text = "Erro ao tentar carregar o(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + ".";
            lblMessage.Visible = true;
        }
    }

    protected void odsDados_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.Cancel = cancelSelect;
        if (e.Cancel)
            ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1", true));
    }

    #endregion EVENTOS
}