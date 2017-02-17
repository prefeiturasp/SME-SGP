using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;

public partial class WebControls_Combos_UCComboFormatoAvaliacao : MotherUserControl
{
    #region Delegates

    public delegate void SelectedIndexChanged();
    public event SelectedIndexChanged IndexChanged;

    #endregion

    #region Propriedades

    /// <summary>
    /// Retorna e seta o valor selecionado no combo.
    /// Referente ao campo fav_id.
    /// </summary>
    public int Valor
    {
        get
        {
            if (string.IsNullOrEmpty(ddlCombo.SelectedValue))
                return -1;

            return Convert.ToInt32(ddlCombo.SelectedValue);
        }
        set
        {
            ddlCombo.SelectedValue = value.ToString();
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
    /// Texto do título ao combo.
    /// </summary>
    public string Texto
    {
        set
        {
            lblTitulo.Text = value;
            cpvCombo.ErrorMessage = value + " é obrigatório.";
        }
    }

    /// <summary>
    /// Adciona e remove a mensagem "Selecione um Formato" do dropdownlist.  
    /// Por padrão é false e a mensagem "Selecione um Formato" não é exibida.
    /// </summary>
    public bool _MostrarMessageSelecione
    {
        set
        {
            if (value)
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um formato --", "-1", true));
            ddlCombo.AppendDataBoundItems = value;
        }
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Adiciona um item vazio no combo, com valor "Selecione um item".
    /// </summary>
    public void AdicionaItemVazio()
    {
        ddlCombo.Items.Clear();
        ddlCombo.Items.Add(new ListItem("-- Selecione um formato --", "-1", true));
    }

    /// <summary>
    /// Verifica se o combo tem somente um formato carregado, e seleciona o primeiro,
    /// caso positivo.
    /// </summary>
    public void SelecionaPrimeiroItem()
    {
        if (ddlCombo.Items.Count == 2)
        {
            ddlCombo.SelectedValue = ddlCombo.Items[1].Value;
        }
    }

    /// <summary>
    /// Seta o foco no combo.
    /// </summary>
    public new void Focus()
    {
        ddlCombo.Focus();
    }

    /// <summary>
    /// Carrega o combo com os formatos de avaliação padrão ativo ou de um formato específico.
    /// </summary>    
    /// <param name="fav_id">Id do formato de avaliação</param>
    public void CarregarFormatoPorFormatoPadraoAtivo(int fav_id)
    {
        try
        {
            ddlCombo.Items.Clear();
            ddlCombo.DataSource = ACA_FormatoAvaliacaoBO.SelecionaFormatosPorFormatoPadraoAtivo(fav_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            ddlCombo.Items.Insert(0, new ListItem("-- Selecione um formato de avaliação --", "-1", true));
            ddlCombo.AppendDataBoundItems = true;
            ddlCombo.DataBind();

        }
        catch (Exception e)
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.InnerException);

            lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
        }
    }

    /// <summary>
    /// Carrega o combo com os formatos de avaliação padrão ativo ou de um formato específico.
    /// Valida regras do curso
    /// </summary>    
    /// <param name="fav_id">Id do formato de avaliação</param>
    /// <param name="cur_id">ID do curso</param>
    /// <param name="crr_id">ID do currículo</param>
    /// <param name="crp_id">ID do curriculoPeriodo</param>
    public void CarregarPorRegrasCurso
    (int fav_id, int cur_id, int crr_id, int crp_id, Nullable<bool> tur_docenteEspecialista)
    {
        try
        {
            ACA_Curriculo crr = new ACA_Curriculo { cur_id = cur_id, crr_id = crr_id };
            ACA_CurriculoBO.GetEntity(crr);

            // Verifica se o curso tem regime de matrícula seriado por avaliações.
            if ((ACA_CurriculoRegimeMatricula)crr.crr_regimeMatricula == ACA_CurriculoRegimeMatricula.SeriadoPorAvaliacoes)
            {
                // Se curso seriado por avaliações, usa o outro método de carregar, para usar as regras específicas.
                CarregarPorRegrasCurso_SeriadoAvaliacoes(fav_id, crr.crr_qtdeAvaliacaoProgressao, cur_id, crr_id, crp_id, tur_docenteEspecialista);
            }
            else
            {
                ddlCombo.Items.Clear();
                object lista;

                string doc_especialista = tur_docenteEspecialista == null ? string.Empty : tur_docenteEspecialista.ToString();
                string chave = string.Format("FormatoAvaliacao_{0};{1};{2};{3};{4}", fav_id, cur_id, crr_id, crp_id, doc_especialista);
                object cache = Cache[chave];

                if (cache == null)
                {
                    // Carrega do banco para guardar em cache.
                    DataTable dt = ACA_FormatoAvaliacaoBO.SelecionaPor_RegrasCurriculoPeriodo
                        (fav_id, cur_id, crr_id, crp_id, tur_docenteEspecialista, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    lista = (from DataRow dr in dt.Rows
                             select new
                             {
                                 fav_nome = dr["fav_nome"].ToString()
                                 ,
                                 fav_id = dr["fav_id"].ToString()
                             }).ToList();

                    // Adiciona cache com validade de 1 hora.
                    Cache.Insert(chave, lista, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    lista = cache;
                }

                ddlCombo.DataSource = lista;

                _MostrarMessageSelecione = true;

                ddlCombo.DataBind();
            }
        }
        catch (Exception e)
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.InnerException);

            lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
        }
    }

    /// <summary>
    /// Carrega o combo com os formatos de avaliação padrão ativo 
    /// e que tenham a quantidade de avaliações periódica ou periódica+final.
    /// </summary>
    /// <param name="fav_id">Id do formato de avaliação</param>
    /// <param name="qtdeAvaliacaoPeriodica">Quantidade de avaliações periódicas e periódica+final</param>
    /// <param name="cur_id">ID do curso</param>
    /// <param name="crr_id">ID do currículo</param>
    /// <param name="crp_id">ID do curriculoPeriodo</param>
    private void CarregarPorRegrasCurso_SeriadoAvaliacoes
    (
        int fav_id
        , int qtdeAvaliacaoPeriodica
        , int cur_id
        , int crr_id
        , int crp_id
        , Nullable<bool> tur_docenteEspecialista
    )
    {
        try
        {
            ddlCombo.Items.Clear();

            ddlCombo.DataSource = ACA_FormatoAvaliacaoBO.SelecionaPor_RegrasCurriculoPeriodo_SeriadoAvaliacoes
                (fav_id, qtdeAvaliacaoPeriodica, cur_id, crr_id, crp_id, tur_docenteEspecialista, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            ddlCombo.Items.Insert(0, new ListItem("-- Selecione um formato de avaliação --", "-1", true));
            ddlCombo.AppendDataBoundItems = true;
            ddlCombo.DataBind();
        }
        catch (Exception e)
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.InnerException);

            lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
        }
    }

    /// <summary>
    /// Carrega o combo com os formatos de avaliação padrão ativo ou de um formato específico.
    /// </summary>    
    /// <param name="fav_id">Id do formato de avaliação</param>
    public void CarregarPorRegrasSemPeriodo_SeriadoAvaliacoes
    (
        int fav_id
        , int qtdeAvaliacaoPeriodica
        , bool seriadoAvaliacoes
    )
    {
        try
        {
            ddlCombo.Items.Clear();
            ddlCombo.DataSource = ACA_FormatoAvaliacaoBO.SelecionaPor_RegrasCurriculoPeriodo(fav_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, qtdeAvaliacaoPeriodica, seriadoAvaliacoes);
            ddlCombo.Items.Insert(0, new ListItem("-- Selecione um formato de avaliação --", "-1", true));
            ddlCombo.AppendDataBoundItems = true;
            ddlCombo.DataBind();

        }
        catch (Exception e)
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.InnerException);

            lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
        }
    }

    #endregion

    #region Eventos

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ddlCombo.AutoPostBack = IndexChanged != null;
    }

    protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IndexChanged != null)
            IndexChanged();
    }

    #endregion
}
