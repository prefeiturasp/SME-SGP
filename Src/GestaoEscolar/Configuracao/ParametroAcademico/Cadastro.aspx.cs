using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class Configuracao_ParametroAcademico_Cadastro : MotherPageLogado
{
    #region Constantes

    private const int indiceColunaPacValorNome = 1;

    #endregion Constantes

    #region Propriedades

    private int _VS_pac_id
    {
        get
        {
            if (ViewState["_VS_pac_id"] != null)
                return Convert.ToInt32(ViewState["_VS_pac_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_pac_id"] = value;
        }
    }

    private string _VS_pac_chave
    {
        get
        {
            if (ViewState["_VS_pac_chave"] != null)
                return Convert.ToString(ViewState["_VS_pac_chave"]);
            return null;
        }
        set
        {
            ViewState["_VS_pac_chave"] = value;
        }
    }

    private string _VS_pac_descricao
    {
        get
        {
            if (ViewState["_VS_pac_descricao"] != null)
                return Convert.ToString(ViewState["_VS_pac_descricao"]);
            return null;
        }
        set
        {
            ViewState["_VS_pac_descricao"] = value;
        }
    }

    private bool _VS_pac_obrigatorio
    {
        get
        {
            if (ViewState["_VS_pac_obrigatorio"] != null)
                return Convert.ToBoolean(ViewState["_VS_pac_obrigatorio"]);
            return false;
        }
        set
        {
            ViewState["_VS_pac_obrigatorio"] = value;
        }
    }

    /// <summary>
    /// Guarda em ViewState o tipo de parâmetro que está sendo editado.
    /// </summary>
    private TipoParametroAcademico VS_TipoParametro
    {
        get
        {
            if (ViewState["VS_TipoParametro"] != null)
                return (TipoParametroAcademico)(ViewState["VS_TipoParametro"]);
            return TipoParametroAcademico.Unico;
        }
        set
        {
            ViewState["VS_TipoParametro"] = value;
        }
    }

    private int qtEscolaCadastrada = -1;

    /// <summary>
    /// Retorna se existe alguma escola cadastrada no sistema.
    /// </summary>
    private bool ExisteEscolaCadastrada
    {
        get
        {
            if (qtEscolaCadastrada == -1)
            {
                DataTable dt = ESC_EscolaBO.GetSelect
                    (0
                    , string.Empty
                    , string.Empty
                    , 0
                    , Guid.Empty
                    , string.Empty
                    , 0
                    , 0
                    , false
                    , 0
                    , 1
                    );

                qtEscolaCadastrada = dt.Rows.Count;
            }

            return qtEscolaCadastrada > 0;
        }
    }

    /// <summary>
    /// ViewState com datatable de Parametros
    /// Retorno e atribui valores para o DataTable de Parametros
    /// </summary>
    public DataTable _VS_ParametrosAcademico
    {
        get
        {
            if (ViewState["_VS_ParametrosAcademico"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("pac_id");
                dt.Columns.Add("pac_chave");
                dt.Columns.Add("pac_obrigatorio");
                dt.Columns.Add("pac_descricao");
                dt.Columns.Add("pac_valor_nome");
                dt.Columns.Add("pac_vigenciaFim");
                dt.Columns.Add("pac_vigenciaInicio");
                dt.Columns.Add("pac_vigencia");
                dt.Columns.Add("tipo", typeof(TipoParametroAcademico));
                dt.Columns.Add("integridadeEscolas", typeof(Boolean));

                ViewState["_VS_ParametrosAcademico"] = dt;
            }
            return (DataTable)ViewState["_VS_ParametrosAcademico"];
        }
        set
        {
            ViewState["_VS_ParametrosAcademico"] = value;
        }
    }

    /// <summary>
    /// Flag guardada em ViewState que indica se existe alguma escola cadastrada no sistema.
    /// </summary>
    public bool VS_ExisteEscolaCadastrada
    {
        get
        {
            if (ViewState["VS_ExisteEscolaCadastrada"] == null)
                return false;

            return Convert.ToBoolean(ViewState["VS_ExisteEscolaCadastrada"]);
        }
        set
        {
            ViewState["VS_ExisteEscolaCadastrada"] = value;
        }
    }

    #endregion Propriedades

    #region Métodos

    private void SetaBuscaEspecifico(string pac_chave)
    {
        #region Parametro - TIPO_UNIDADE_ADMINISTRATIVA_ESCOLA

        if (pac_chave == "TIPO_UNIDADE_ADMINISTRATIVA_ESCOLA")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo unidade administrativa é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tua_nome";
            _ddlParametroAcademicoValor.DataValueField = "tua_id";
            _ddlParametroAcademicoValor.DataSource = SYS_TipoUnidadeAdministrativaBO.GetSelect(Guid.Empty, string.Empty, 0, false);
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo unidade administrativa --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_UNIDADE_ADMINISTRATIVA_ESCOLA

        #region Parametro - PROG_SOCIAL_DESEMPENHO_DISCIPLINAS_MEDIA_2_ANO

        else if (pac_chave == "PROG_SOCIAL_DESEMPENHO_DISCIPLINAS_MEDIA_2_ANO")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Programas sociais - relatório de bônus por desempenho - " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL_MIN") + " da média do 2º ano é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tne_tds_nome";
            _ddlParametroAcademicoValor.DataValueField = "tds_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoDisciplinaBO.SelecionaTipoDisciplina(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - PROG_SOCIAL_DESEMPENHO_DISCIPLINAS_MEDIA_2_ANO

        #region Parametro - PROG_SOCIAL_DESEMPENHO_DISCIPLINAS_MEDIA_3_9_ANO

        else if (pac_chave == "PROG_SOCIAL_DESEMPENHO_DISCIPLINAS_MEDIA_3_9_ANO")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Programas sociais - relatório de bônus por desempenho - " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL_MIN") + " da média do 3º ao 9º anos é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tne_tds_nome";
            _ddlParametroAcademicoValor.DataValueField = "tds_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoDisciplinaBO.SelecionaTipoDisciplina(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - PROG_SOCIAL_DESEMPENHO_DISCIPLINAS_MEDIA_3_9_ANO

        #region Parametro - PAR_GRUPO_PERFIL_DOCENTE

        else if (pac_chave == "PAR_GRUPO_PERFIL_DOCENTE")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Grupo perfil docente é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "pgs_chave";
            _ddlParametroAcademicoValor.DataValueField = "pgs_chave";
            _ddlParametroAcademicoValor.DataSource = SYS_ParametroGrupoPerfilBO.GetSelect2(Guid.Empty, false, 1, 1);
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um grupo perfil --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - PAR_GRUPO_PERFIL_DOCENTE

        #region Parametro - PAR_GRUPO_PERFIL_ALUNO

        else if (pac_chave == "PAR_GRUPO_PERFIL_ALUNO")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Grupo perfil aluno é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "pgs_chave";
            _ddlParametroAcademicoValor.DataValueField = "pgs_chave";
            _ddlParametroAcademicoValor.DataSource = SYS_ParametroGrupoPerfilBO.GetSelect2(Guid.Empty, false, 1, 1);
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um grupo perfil --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - PAR_GRUPO_PERFIL_ALUNO

        #region Parametro - TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA

        else if (pac_chave == "TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de unidade administrativa é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tua_nome";
            _ddlParametroAcademicoValor.DataValueField = "tua_id";
            _ddlParametroAcademicoValor.DataSource = SYS_TipoUnidadeAdministrativaBO.GetSelect(Guid.Empty, string.Empty, 0, false);
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de unidade administrativa --", Guid.Empty.ToString(), true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA

        #region Parametro - TIPO_RESPONSAVEL_MAE

        else if (pac_chave == "TIPO_RESPONSAVEL_MAE")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo aluno responsável é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tra_nome";
            _ddlParametroAcademicoValor.DataValueField = "tra_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoResponsavelAlunoBO.SelecionaTipoResponsavelAluno();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo aluno responsável --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_RESPONSAVEL_MAE

        #region Parametro - TIPO_RESPONSAVEL_PAI

        else if (pac_chave == "TIPO_RESPONSAVEL_PAI")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo aluno responsável é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tra_nome";
            _ddlParametroAcademicoValor.DataValueField = "tra_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoResponsavelAlunoBO.SelecionaTipoResponsavelAluno();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo aluno responsável --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_RESPONSAVEL_PAI

        #region Parametro - TIPO_RESPONSAVEL_O_PROPRIO

        else if (pac_chave == "TIPO_RESPONSAVEL_O_PROPRIO")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo aluno responsável é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tra_nome";
            _ddlParametroAcademicoValor.DataValueField = "tra_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoResponsavelAlunoBO.SelecionaTipoResponsavelAluno();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo aluno responsável --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_RESPONSAVEL_O_PROPRIO

        #region Parametro - TIPO_RESPONSAVEL_OUTRO

        else if (pac_chave == "TIPO_RESPONSAVEL_OUTRO")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo aluno responsável é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tra_nome";
            _ddlParametroAcademicoValor.DataValueField = "tra_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoResponsavelAlunoBO.SelecionaTipoResponsavelAluno();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo aluno responsável --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_RESPONSAVEL_OUTRO

        #region Parametro - PAR_GRUPO_PERFIL_COLAB

        else if (pac_chave == "PAR_GRUPO_PERFIL_COLAB")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Grupo perfil colaborador é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "pgs_chave";
            _ddlParametroAcademicoValor.DataValueField = "pgs_chave";
            _ddlParametroAcademicoValor.DataSource = SYS_ParametroGrupoPerfilBO.GetSelect2(Guid.Empty, false, 1, 1);
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um grupo perfil --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - PAR_GRUPO_PERFIL_COLAB

        #region Parametro - PAR_GRUPO_PERFIL_ALUNO_RESPONSAVEL

        else if (pac_chave == "PAR_GRUPO_PERFIL_ALUNO_RESPONSAVEL")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Grupo perfil aluno responsável é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "pgs_chave";
            _ddlParametroAcademicoValor.DataValueField = "pgs_chave";
            _ddlParametroAcademicoValor.DataSource = SYS_ParametroGrupoPerfilBO.GetSelect2(Guid.Empty, false, 1, 1);
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um grupo perfil --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - PAR_GRUPO_PERFIL_ALUNO_RESPONSAVEL

        #region Parametro - PAR_REDE_ENSINO_PADRAO

        else if (pac_chave == "PAR_REDE_ENSINO_PADRAO")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de rede de ensino é obrigatório";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tre_nome";
            _ddlParametroAcademicoValor.DataValueField = "tre_id";
            _ddlParametroAcademicoValor.DataSource = ESC_TipoRedeEnsinoBO.SelecionaTipoRedeEnsino();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de rede de ensino --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - PAR_REDE_ENSINO_PADRAO

        #region Parametro - TIPO_DOCUMENTACAO_NIS

        else if (pac_chave == "TIPO_DOCUMENTACAO_NIS")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de documentação NIS é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tdo_nome";
            _ddlParametroAcademicoValor.DataValueField = "tdo_id";
            _ddlParametroAcademicoValor.DataSource = SYS_TipoDocumentacaoBO.GetSelect(Guid.Empty, string.Empty, string.Empty, 0, false, 1, 1);
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de documentação --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_DOCUMENTACAO_NIS

        #region Parametro - TIPO_EVENTO_EFETIVACAO_NOTAS

        else if (pac_chave == "TIPO_EVENTO_EFETIVACAO_NOTAS")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de evento de efetivação de notas do período é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tev_nome";
            _ddlParametroAcademicoValor.DataValueField = "tev_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoEventoBO.SelecionaTodosRelacionados();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de evento --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_EVENTO_EFETIVACAO_NOTAS

        #region Parametro - TIPO_EVENTO_EFETIVACAO_RECUPERACAO

        else if (pac_chave == "TIPO_EVENTO_EFETIVACAO_RECUPERACAO")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de evento de efetivação da recuperação é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tev_nome";
            _ddlParametroAcademicoValor.DataValueField = "tev_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoEventoBO.SelecionaTodosRelacionados();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de evento --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_EVENTO_EFETIVACAO_RECUPERACAO

        #region Parametro - SOLICITACAO_VAGA_VALIDADE_TIPO

        else if (pac_chave == "SOLICITACAO_VAGA_VALIDADE_TIPO")
        {
            parametroTextBox.Visible = false;
            parametroCombo.Visible = true;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de validade para solicitação de vaga é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("Data", "3", true));
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("Dias", "2", true));
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("Horas", "1", true));
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione uma opção --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
        }

        #endregion Parametro - SOLICITACAO_VAGA_VALIDADE_TIPO

        #region Parametro - TIPO_REDE_ENSINO_PARTICULAR

        else if (pac_chave == "TIPO_REDE_ENSINO_PARTICULAR")
        {
            parametroTextBox.Visible = false;
            parametroCombo.Visible = true;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de rede de ensino é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tre_nome";
            _ddlParametroAcademicoValor.DataValueField = "tre_id";
            _ddlParametroAcademicoValor.DataSource = ESC_TipoRedeEnsinoBO.SelecionaTipoRedeEnsino();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de rede de ensino --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
        }

        #endregion Parametro - TIPO_REDE_ENSINO_PARTICULAR

        #region Parametro - TIPO_REDE_ENSINO_PUBLICA_OUTROS_MUNICIPIOS

        else if (pac_chave == "TIPO_REDE_ENSINO_PUBLICA_OUTROS_MUNICIPIOS")
        {
            parametroTextBox.Visible = false;
            parametroCombo.Visible = true;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de rede de ensino é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tre_nome";
            _ddlParametroAcademicoValor.DataValueField = "tre_id";
            _ddlParametroAcademicoValor.DataSource = ESC_TipoRedeEnsinoBO.SelecionaTipoRedeEnsino();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de rede de ensino --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
        }

        #endregion Parametro - TIPO_REDE_ENSINO_PUBLICA_OUTROS_MUNICIPIOS

        #region Parametro - TIPO_REDE_ENSINO_PUBLICA_OUTROS_ESTADOS_FEDERAL

        else if (pac_chave == "TIPO_REDE_ENSINO_PUBLICA_OUTROS_ESTADOS_FEDERAL")
        {
            parametroTextBox.Visible = false;
            parametroCombo.Visible = true;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de rede de ensino é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tre_nome";
            _ddlParametroAcademicoValor.DataValueField = "tre_id";
            _ddlParametroAcademicoValor.DataSource = ESC_TipoRedeEnsinoBO.SelecionaTipoRedeEnsino();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de rede de ensino --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
        }

        #endregion Parametro - TIPO_REDE_ENSINO_PUBLICA_OUTROS_ESTADOS_FEDERAL

        #region Parametro - CURSO_ENSINO_FUNDAMENTAL

        else if (pac_chave == "CURSO_ENSINO_FUNDAMENTAL")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Curso de ensino fundamental regular é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "cur_nome";
            _ddlParametroAcademicoValor.DataValueField = "cur_id";
            _ddlParametroAcademicoValor.DataSource = ACA_CursoBO.SelecionaCursoNaoExcluido(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione uma " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - CURSO_ENSINO_FUNDAMENTAL

        #region Parametro - TIPO_DISCIPLINA_ELETIVA_ALUNO

        else if (pac_chave == "TIPO_DISCIPLINA_ELETIVA_ALUNO")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " eletivo(a) é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tne_tds_nome";
            _ddlParametroAcademicoValor.DataValueField = "tds_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoDisciplinaBO.SelecionaTipoDisciplinaTodasEletivaAluno(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione uma " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_DISCIPLINA_ELETIVA_ALUNO

        #region Parametro - TIPO_EVENTO_EFETIVACAO_FINAL

        else if (pac_chave == "TIPO_EVENTO_EFETIVACAO_FINAL")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de evento da efetivação da nota final é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tev_nome";
            _ddlParametroAcademicoValor.DataValueField = "tev_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoEventoBO.SelecionaTodosNaoRelacionados();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de evento --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_EVENTO_EFETIVACAO_FINAL
        
        #region Parametro - TIPO_CLASSIFICACAO_ESCOLAS_AMANHA

        else if (pac_chave == "TIPO_CLASSIFICACAO_ESCOLAS_AMANHA")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de classificação das Escolas do Amanhã é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tce_nome";
            _ddlParametroAcademicoValor.DataValueField = "tce_id";
            _ddlParametroAcademicoValor.DataSource = ESC_TipoClassificacaoEscolaBO.SelecionaTipoClassificacaoEscola();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de classificação da escola --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_CLASSIFICACAO_ESCOLAS_AMANHA

        #region Parametro - TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL

        else if (pac_chave == "TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL")
        {
            parametroTextBox.Visible = false;
            parametroCombo.Visible = true;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de nível de ensino de educação infantil é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tne_nome";
            _ddlParametroAcademicoValor.DataValueField = "tne_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoNivelEnsinoBO.SelecionaTipoNivelEnsino();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de nível de ensino --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
        }

        #endregion Parametro - TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL

        #region Parametro - TIPO_MODALIDADE_ENSINO_REGULAR

        else if (pac_chave == "TIPO_MODALIDADE_ENSINO_REGULAR")
        {
            parametroTextBox.Visible = false;
            parametroCombo.Visible = true;
            _cvParametroAcademicoValor.ErrorMessage = "Modalidade de ensino que se refere a modalidade regular do ensino fundamental é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataValueField = "tme_id";
            _ddlParametroAcademicoValor.DataTextField = "tme_nome";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoModalidadeEnsinoBO.SelecionaTipoModalidadeEnsino();
            _ddlParametroAcademicoValor.DataBind();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione uma opção --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
        }

        #endregion Parametro - TIPO_MODALIDADE_ENSINO_REGULAR

        #region Parametro - TIPO_NIVEL_ENSINO_FUNDAMENTAL

        else if (pac_chave == "TIPO_NIVEL_ENSINO_FUNDAMENTAL")
        {
            parametroTextBox.Visible = false;
            parametroCombo.Visible = true;
            _cvParametroAcademicoValor.ErrorMessage = "Nível de ensino que se refere ao curso ensino fundamental é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataValueField = "tne_id";
            _ddlParametroAcademicoValor.DataTextField = "tne_nome";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoNivelEnsinoBO.SelecionaTipoNivelEnsino();
            _ddlParametroAcademicoValor.DataBind();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione uma opção --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
        }

        #endregion Parametro - TIPO_NIVEL_ENSINO_FUNDAMENTAL

        #region Parametro - TIPO_EVENTO_EFETIVACAO_RECUPERACAO_FINAL

        else if (pac_chave == "TIPO_EVENTO_EFETIVACAO_RECUPERACAO_FINAL")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de evento da efetivação da nota de recuperação final é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tev_nome";
            _ddlParametroAcademicoValor.DataValueField = "tev_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoEventoBO.SelecionaTodosNaoRelacionados();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de evento --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_EVENTO_EFETIVACAO_RECUPERACAO_FINAL

        #region Parametro - ESCOLA_RESPONSAVEL_EFETIVACAO_TRANSFERENCIA_ENTRE_ESCOLAS

        else if (pac_chave == "ESCOLA_RESPONSAVEL_EFETIVACAO_TRANSFERENCIA_ENTRE_ESCOLAS")
        {
            parametroTextBox.Visible = false;
            parametroCombo.Visible = true;
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("Destino", "2", true));
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("Origem", "1", true));
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("Nenhuma", "-1", true));
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione uma opção --", "0", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
        }

        #endregion Parametro - ESCOLA_RESPONSAVEL_EFETIVACAO_TRANSFERENCIA_ENTRE_ESCOLAS

        #region Parametro - TIPO_CONTATO_CELULAR

        else if (pac_chave == "TIPO_CONTATO_CELULAR")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de contato celular é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tmc_nome";
            _ddlParametroAcademicoValor.DataValueField = "tmc_id";
            _ddlParametroAcademicoValor.DataSource = SYS_TipoMeioContatoBO.GetSelect(Guid.Empty, string.Empty, 0, false, 1, 1);
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de contato --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_CONTATO_CELULAR

        #region Parametro - TIPO_CONTATO_TELEFONE_RECADO

        else if (pac_chave == "TIPO_CONTATO_TELEFONE_RECADO")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de contato telefone de recado é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tmc_nome";
            _ddlParametroAcademicoValor.DataValueField = "tmc_id";
            _ddlParametroAcademicoValor.DataSource = SYS_TipoMeioContatoBO.GetSelect(Guid.Empty, string.Empty, 0, false, 1, 1);
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de contato --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_CONTATO_TELEFONE_RECADO

        #region Parametro - TIPO_DISCIPLINA_CONTROLE_FREQUENCIA

        else if (pac_chave == "TIPO_DISCIPLINA_CONTROLE_FREQUENCIA")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tds_nome";
            _ddlParametroAcademicoValor.DataValueField = "tds_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoDisciplinaBO.SelecionaTipoDisciplina(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_DISCIPLINA_CONTROLE_FREQUENCIA

        #region Parametro - ORDENACAO_COMBO_ALUNO

        else if (pac_chave == "ORDENACAO_COMBO_ALUNO")
        {
            parametroTextBox.Visible = false;
            parametroCombo.Visible = true;
            _cvParametroAcademicoValor.ErrorMessage = "Ordenação padrão nos combos de ordenação de alunos é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("Número de chamada", "0", true));
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("Nome do aluno", "1", true));
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione uma opção --", "", true));

            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
        }

        #endregion Parametro - ORDENACAO_COMBO_ALUNO

        #region Parametro - TIPO_EVENTO_LIBERACAO_BOLETIM_ONLINE

        else if (pac_chave == "TIPO_EVENTO_LIBERACAO_BOLETIM_ONLINE")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de evento de liberação do boletim online é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tev_nome";
            _ddlParametroAcademicoValor.DataValueField = "tev_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoEventoBO.SelecionaTodosRelacionados();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de evento --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_EVENTO_LIBERACAO_BOLETIM_ONLINE

        #region Parametro - TIPO_EVENTO_EDICAO_DADOS_AREA_ALUNO

        else if (pac_chave == "TIPO_EVENTO_EDICAO_DADOS_AREA_ALUNO")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de evento de liberação da edição dos dados cadastrais na área do aluno é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tev_nome";
            _ddlParametroAcademicoValor.DataValueField = "tev_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoEventoBO.SelecionaTodosRelacionados();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de evento --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_EVENTO_EDICAO_DADOS_AREA_ALUNO

        #region Parametro -  GRUPO_RESPONSAVEL_BOLETIMONLINE

        else if (pac_chave == "GRUPO_RESPONSAVEL_BOLETIMONLINE")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Grupo responsável é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "gru_nome";
            _ddlParametroAcademicoValor.DataValueField = "gru_id";
            _ddlParametroAcademicoValor.DataSource = SYS_GrupoBO.GetSelect(174, false, 0, 0);
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um grupo responsável --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro -  GRUPO_RESPONSAVEL_BOLETIMONLINE

        #region Parametro - TIPO_ENTIDADE_EMPRESA

        else if (pac_chave == "TIPO_ENTIDADE_EMPRESA")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de entidade empresa é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "ten_nome";
            _ddlParametroAcademicoValor.DataValueField = "ten_id";
            _ddlParametroAcademicoValor.DataSource = SYS_TipoEntidadeBO.GetSelect();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione uma entidade --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_ENTIDADE_EMPRESA

        #region Parametro - TIPO_DISCIPLINA_ENRIQUECIMENTO_CURRICULAR

        else if (pac_chave == "TIPO_DISCIPLINA_ENRIQUECIMENTO_CURRICULAR")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " de enriquecimento curricular é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tds_nome";
            _ddlParametroAcademicoValor.DataValueField = "tds_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoDisciplinaBO.SelecionaTipoDisciplina(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_DISCIPLINA_ENRIQUECIMENTO_CURRICULAR

        #region Parametro - TIPO_RESPONSAVEL_NAO_EXISTE

        else if (pac_chave == "TIPO_RESPONSAVEL_NAO_EXISTE")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo aluno responsável é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tra_nome";
            _ddlParametroAcademicoValor.DataValueField = "tra_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoResponsavelAlunoBO.SelecionaTipoResponsavelAluno();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo aluno responsável --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_RESPONSAVEL_NAO_EXISTE

        #region Parametro - TIPO_DISCIPLINA_ENRIQUECIMENTO_CURRICULAR

        else if (pac_chave == "TIPO_DISCIPLINA_ENRIQUECIMENTO_CURRICULAR")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " de enriquecimento curricular é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tds_nome";
            _ddlParametroAcademicoValor.DataValueField = "tds_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoDisciplinaBO.SelecionaTipoDisciplina(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_DISCIPLINA_ENRIQUECIMENTO_CURRICULAR

        #region Parametro - TIPO_EVENTO_ATIVIDADE_DIVERSIFICADA

        else if (pac_chave == "TIPO_EVENTO_ATIVIDADE_DIVERSIFICADA")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de evento de atividade diversificada é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tev_nome";
            _ddlParametroAcademicoValor.DataValueField = "tev_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoEventoBO.SelecionaTodosRelacionados();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de evento --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_EVENTO_ATIVIDADE_DIVERSIFICADA

        #region Parametro - TIPO_MODALIDADE_EJA

        else if (pac_chave == "TIPO_MODALIDADE_EJA")
        {
            parametroTextBox.Visible = false;
            parametroCombo.Visible = true;
            _cvParametroAcademicoValor.ErrorMessage = "Modalidade de ensino que se refere a modalidade EJA é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataValueField = "tme_id";
            _ddlParametroAcademicoValor.DataTextField = "tme_nome";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoModalidadeEnsinoBO.SelecionaTipoModalidadeEnsino();
            _ddlParametroAcademicoValor.DataBind();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione uma opção --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
        }

        #endregion Parametro - TIPO_MODALIDADE_EJA
        
        #region Parametro - TIPO_ANOTACAO_AVALIACAO_AUTOMATICA

        else if (pac_chave == "TIPO_ANOTACAO_AVALIACAO_AUTOMATICA")
        {
            parametroTextBox.Visible = false;
            parametroCombo.Visible = true;
            _cvParametroAcademicoValor.ErrorMessage = "É obrigatório selecionar uma opção.";
            DataTable dtTipoAnotacao = ACA_TipoAnotacaoAlunoBO.SelecionarTipoAnotacaoAluno_ent_id(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            _ddlParametroAcademicoValor.Items.Clear();
            foreach (DataRow drTipoAnotacao in dtTipoAnotacao.Rows)
            {
                _ddlParametroAcademicoValor.Items.Insert(0, new ListItem(drTipoAnotacao["tia_nome"].ToString(), drTipoAnotacao["tia_id"].ToString(), true));
            }
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione uma opção --", "", true));
            _cvParametroAcademicoValor.ValueToCompare = "";
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
        }

        #endregion Parametro - TIPO_ANOTACAO_AVALIACAO_AUTOMATICA

        #region Parametro - TIPO_EVENTO_PLANEJAMENTO_ANUAL

        else if (pac_chave == "TIPO_EVENTO_PLANEJAMENTO_ANUAL")
        {
            parametroTextBox.Visible = false;
            _cvParametroAcademicoValor.ErrorMessage = "Tipo de evento de planejamento anual é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataTextField = "tev_nome";
            _ddlParametroAcademicoValor.DataValueField = "tev_id";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoEventoBO.SelecionaTodosNaoRelacionados();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione um tipo de evento --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
            _ddlParametroAcademicoValor.DataBind();
            parametroCombo.Visible = true;
        }

        #endregion Parametro - TIPO_EVENTO_PLANEJAMENTO_ANUAL

        #region Parametro - PROCESSAR_FILA_FECHAMENTO_TELA_SERVICO

        else if (pac_chave == "PROCESSAR_FILA_FECHAMENTO_TELA_SERVICO")
        {
            parametroTextBox.Visible = false;
            parametroCombo.Visible = true;
            _cvParametroAcademicoValor.ErrorMessage = "É obrigatório selecionar uma opção.";

            _ddlParametroAcademicoValor.Items.Clear();

            string ser_ids = String.Format("{0};{1}", ((byte)eChaveServicos.ProcessamentoNotaFrequenciaFechamento).ToString(), ((byte)eChaveServicos.ProcessamentoDadosFechamento).ToString());

            List<SYS_Servicos> ltServico = MSTech.GestaoEscolar.BLL.SYS_ServicosBO.SelecionaServicosIds(ser_ids);

            foreach (SYS_Servicos servico in ltServico)
            {
                _ddlParametroAcademicoValor.Items.Insert(0, new ListItem(servico.ser_nome, servico.ser_id.ToString(), true));
            }

            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione uma opção --", "", true));
            _cvParametroAcademicoValor.ValueToCompare = "";
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
        }

        #endregion Parametro - PROCESSAR_FILA_FECHAMENTO_TELA_SERVICO

        #region Parametro - EXIBE_DISCIPLINA_FORA_GRADE_HISTORICO

        else if (pac_chave == "EXIBE_DISCIPLINA_FORA_GRADE_HISTORICO")
        {
            parametroTextBox.Visible = false;
            parametroCombo.Visible = true;
            _cvParametroAcademicoValor.ErrorMessage = "Área de conhecimento para disciplinas de fora da rede é obrigatório.";
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataValueField = "aco_id";
            _ddlParametroAcademicoValor.DataTextField = "aco_nome";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoNivelEnsinoBO.SelecionaAreaConhecimento();
            _ddlParametroAcademicoValor.DataBind();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione uma opção --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
        }

        #endregion Parametro - EXIBE_DISCIPLINA_FORA_GRADE_HISTORICO

        #region Parametro - TIPO_PERIODO_CALENDARIO_RECESSO

        else if (pac_chave == "TIPO_PERIODO_CALENDARIO_RECESSO")
        {
            parametroTextBox.Visible = false;
            parametroCombo.Visible = true;
            _cvParametroAcademicoValor.Visible = false;
            _ddlParametroAcademicoValor.Items.Clear();
            _ddlParametroAcademicoValor.DataValueField = "tpc_id";
            _ddlParametroAcademicoValor.DataTextField = "tpc_nome";
            _ddlParametroAcademicoValor.DataSource = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendario(0, false, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            _ddlParametroAcademicoValor.DataBind();
            _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione uma opção --", "-1", true));
            _ddlParametroAcademicoValor.AppendDataBoundItems = true;
        }

        #endregion Parametro - TIPO_PERIODO_CALENDARIO_RECESSO
    }

    private void SetaBuscaLogica(parametroAttributes parametroattributes)
    {
        parametroTextBox.Visible = false;
        parametroCombo.Visible = true;
        _cvParametroAcademicoValor.ErrorMessage = parametroattributes.ErrorMessage;
        _ddlParametroAcademicoValor.Items.Clear();
        _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("Sim", "True", true));
        _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("Não", "False", true));
        _ddlParametroAcademicoValor.Items.Insert(0, new ListItem("-- Selecione uma opção --", "-1", true));
        _ddlParametroAcademicoValor.AppendDataBoundItems = true;

    }

    private void SetaBuscaTexto(parametroAttributes parametroattributes)
    {
        parametroTextBox.Visible = true;
        parametroCombo.Visible = false;

        ACA_ParametroAcademico Param = ACA_ParametroAcademicoBO.GetEntityByChave(__SessionWEB.__UsuarioWEB.Usuario.ent_id, parametroattributes.Parametro);
        if (!string.IsNullOrEmpty(Param.pac_descricao))
        {
            _lblNome_Par.Text = Param.pac_descricao;
        }
        else
        {
            _lblNome_Par.Text = parametroattributes.Description;
        }
        if (!string.IsNullOrEmpty(Param.pac_valor))
        {
            _txtValor.Text = Param.pac_valor;
        }
    }

    /// <summary>
    /// Monta o DropDownList usada na DIV conforme o valor da chave.
    /// Retornando a lista com valores não excluidos logicamente.
    /// (ativo e bloqueado)
    /// </summary>
    /// <param name="pac_chave"></param>
    private void SetaBuscaComboValor(string pac_chave)
    {
        parametroAttributes parametroattributes = parametroAcademicoAttributes.Get(pac_chave);

        switch (parametroattributes.DataType)
        {
            case DataTypeParametroAcademico.specific:
                SetaBuscaEspecifico(pac_chave);
                break;
            case DataTypeParametroAcademico.logic:
                SetaBuscaLogica(parametroattributes);
                break;
            case DataTypeParametroAcademico.text:
            case DataTypeParametroAcademico.integer:
                SetaBuscaTexto(parametroattributes);
                break;
        }
    }

    /// <summary>
    /// Seta propriedades dos campos e grid, de acordo com as regras para edição
    /// do parâmetro selecionado.
    /// </summary>
    private void MontaDivEdicaoParametro()
    {
        try
        {
            _VS_pac_id = -1;
            _lblNome_Par.Text = _VS_pac_descricao;

            switch (VS_TipoParametro)
            {
                case TipoParametroAcademico.Unico:
                    {
                        // Abre tela direto pra edição de 1 único valor, ou para inserção,
                        // caso não tenha nenhum parâmetro cadastrado.

                        divVigencia.Visible = false;
                        fsValoresParametros.Visible = false;

                        ACA_ParametroAcademico entity = ACA_ParametroAcademicoBO.GetEntityByChave(
                            __SessionWEB.__UsuarioWEB.Usuario.ent_id
                            , (eChaveAcademico)Enum.Parse(typeof(eChaveAcademico), _VS_pac_chave));

                        if (entity.IsNew)
                        {
                            if (parametroCombo.Visible)
                            {
                                _ddlParametroAcademicoValor.SelectedIndex = 0;
                            }
                            else if (parametroTextBox.Visible)
                            {
                                _txtValor.Text = string.Empty;

                                //Limita o número máximo de caracteres permitidos em 2 para inclusão do valor do parametro AMPLITUDE_IDADE_ALERTA
                                if (entity.pac_chave == eChaveAcademico.AMPLITUDE_IDADE_ALERTA.ToString())
                                {
                                    _txtValor.MaxLength = 2;
                                }
                                else if (entity.pac_chave == eChaveAcademico.NOME_WS_RETORNA_IMAGEM_CARTEIRA_VACINACAO.ToString())
                                {
                                    _txtValor.MaxLength = 1000;
                                }
                                else
                                {
                                    _txtValor.MaxLength = 50;
                                }
                            }
                            else
                            {
                                txtData.Text = string.Empty;
                            }
                        }
                        else
                        {
                            _CarregarParametro(entity);
                        }

                        _cvParametroAcademicoValor.Visible = _VS_pac_obrigatorio;

                        break;
                    }
                case TipoParametroAcademico.Multiplo:
                    {
                        LimpaCampos();

                        // Pode cadastrar vários valores, porém sem vigência.
                        divVigencia.Visible = false;
                        fsValoresParametros.Visible = true;

                        // Carrega o grid e esconde a coluna de vigência.
                        CarregaGridValores(false, false, !ExisteEscolaCadastrada);

                        break;
                    }
                case TipoParametroAcademico.VigenciaObrigatorio:
                    {
                        LimpaCampos();

                        fsValoresParametros.Visible = true;
                        divVigencia.Visible = true;
                        divVigenciaInicio.Visible = true;
                        divVigenciaFim.Visible = false;

                        // Unificar o nome do campo com o informado na mensagem.
                        _lblVigencia.Text = "Vigência inicial";
                        _rfvVigenciaIni.ErrorMessage = "Vigência inicial é obrigatório.";

                        // Carrega grid com valores cadastrados.
                        CarregaGridValores(true, false, true);

                        break;
                    }
                case TipoParametroAcademico.VigenciaOpcional:
                    {
                        LimpaCampos();

                        fsValoresParametros.Visible = true;
                        divVigencia.Visible = true;
                        divVigenciaInicio.Visible = true;
                        divVigenciaFim.Visible = true;

                        // Unificar o nome do campo com o informado na mensagem.
                        _lblVigencia.Text = "Vigência";
                        _rfvVigenciaIni.ErrorMessage = "Data de vigência inicial é obrigatório.";

                        // Carrega grid com valores cadastrados.
                        CarregaGridValores(true, true, true);

                        break;
                    }
            }

            Form.DefaultFocus = _ddlParametroAcademicoValor.ClientID;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessageInsert.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Carrega o grid de valores dos parâmetros já cadastrados.
    /// </summary>
    private void CarregaGridValores(bool mostraVigencia, bool mostraAlterar, bool mostraExcluir)
    {
        mostraExcluir |= ((_VS_pac_chave == "TIPO_UNIDADE_ADMINISTRATIVA_ESCOLA")
                        || (_VS_pac_chave == "PROG_SOCIAL_DESEMPENHO_DISCIPLINAS_MEDIA_2_ANO")
                        || (_VS_pac_chave == "PROG_SOCIAL_DESEMPENHO_DISCIPLINAS_MEDIA_3_9_ANO")
                        || (_VS_pac_chave == "TIPO_DISCIPLINA_CONTROLE_FREQUENCIA")
                        || (_VS_pac_chave == "TIPO_DISCIPLINA_ENRIQUECIMENTO_CURRICULAR")
                        || (_VS_pac_chave == "GRUPO_RESPONSAVEL_BOLETIMONLINE")
                        || (_VS_pac_chave == "PROG_SOCIAL_PROCESSAR_FREQUENCIAS")
                        || (_VS_pac_chave == "CARGO_DOCENTE_JEX"));
        gvValoresParametrosAcademicos.Columns[2].Visible = mostraVigencia;
        gvValoresParametrosAcademicos.Columns[3].Visible = mostraAlterar;
        gvValoresParametrosAcademicos.Columns[4].Visible = mostraExcluir;

        gvValoresParametrosAcademicos.PageIndex = 0;
        gvValoresParametrosAcademicos.DataSource = ACA_ParametroAcademicoBO.SelecionaParametroValores(__SessionWEB.__UsuarioWEB.Usuario.ent_id, _VS_pac_chave, true, 0, 10);
        gvValoresParametrosAcademicos.DataBind();
    }

    /// <summary>
    /// Método para carga inicial dos parametros Default do sistema
    /// </summary>    
    private void Carrega_VS_ParametrosAcademico()
    {
        try
        {
            _VS_ParametrosAcademico = null;

            //List<ACA_ParametroAcademicoBO.tmpParametros> lista = ACA_ParametroAcademicoBO.CriaListaParametroPadrao(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            List<ACA_ParametroAcademicoBO.tmpParametros> lista = ACA_ParametroAcademicoBO.CriaListaParametroPadrao();

            foreach (ACA_ParametroAcademicoBO.tmpParametros item in lista)
            {
                DataRow dr = _VS_ParametrosAcademico.NewRow();
                dr["pac_chave"] = item.parametro.pac_chave;
                dr["pac_obrigatorio"] = item.parametro.pac_obrigatorio;
                dr["pac_descricao"] = TrocaParametroMensagem(item.parametro.pac_descricao);
                dr["tipo"] = item.tipo;
                dr["integridadeEscolas"] = item.integridadeEscolas;

                _VS_ParametrosAcademico.Rows.Add(dr);
            }

            DataTable dt = ACA_ParametroAcademicoBO.GetSelect(__SessionWEB.__UsuarioWEB.Usuario.ent_id, false, 1, 1);

            for (int i = 0; i < _VS_ParametrosAcademico.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (Convert.ToString(_VS_ParametrosAcademico.Rows[i]["pac_chave"]) == Convert.ToString(dt.Rows[j]["pac_chave"]))
                    {
                        _VS_ParametrosAcademico.Rows[i]["pac_id"] = Convert.ToString(dt.Rows[j]["pac_id"]);
                        _VS_ParametrosAcademico.Rows[i]["pac_valor_nome"] = Convert.ToString(dt.Rows[j]["pac_valor_nome"]);
                        _VS_ParametrosAcademico.Rows[i]["pac_vigenciaFim"] = Convert.ToString(dt.Rows[j]["pac_vigenciaFim"]);
                        _VS_ParametrosAcademico.Rows[i]["pac_vigenciaInicio"] = Convert.ToString(dt.Rows[j]["pac_vigenciaInicio"]);
                        _VS_ParametrosAcademico.Rows[i]["pac_vigencia"] = Convert.ToString(dt.Rows[j]["pac_vigencia"]);
                    }
                }
            }

            //Ordena o datatable pela descriçao
            _VS_ParametrosAcademico.DefaultView.Sort = "pac_descricao";
            gvParametrosAcademicos.DataSource = _VS_ParametrosAcademico.DefaultView;
            gvParametrosAcademicos.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os parâmetros acadêmicos.", UtilBO.TipoMensagem.Erro);
            _updMessage.Update();
        }

        _PesquisarParametrosBuscaAluno();
    }

    /// <summary>
    /// Carrega os dados do Parametro Academico nos controles caso seja alteração.
    /// </summary>
    /// <param name="pac_id"></param>
    private void _CarregarParametro(Int32 pac_id)
    {
        try
        {
            ACA_ParametroAcademico _ParametroAcademico = new ACA_ParametroAcademico { ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id, pac_id = pac_id };
            ACA_ParametroAcademicoBO.GetEntity(_ParametroAcademico);

            _CarregarParametro(_ParametroAcademico);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os parâmetros acadêmicos.", UtilBO.TipoMensagem.Erro);
            _updMessage.Update();
        }
    }

    /// <summary>
    /// Limpa campos do cadastro de parâmetros acadêmicos.
    /// </summary>
    private void LimpaCampos()
    {
        if (parametroCombo.Visible)
        {
            _ddlParametroAcademicoValor.SelectedIndex = 0;
            _txtVigenciaFim.Text = string.Empty;
            _txtVigenciaIni.Text = string.Empty;
        }
    }

    /// <summary>
    /// Carrega os dados do Parametro Academico nos controles caso seja alteração.
    /// </summary>
    /// <param name="_ParametroAcademico"></param>
    private void _CarregarParametro(ACA_ParametroAcademico _ParametroAcademico)
    {
        _VS_pac_id = _ParametroAcademico.pac_id;
        _VS_pac_chave = _ParametroAcademico.pac_chave;
        _VS_pac_obrigatorio = _ParametroAcademico.pac_obrigatorio;
        _VS_pac_descricao = _ParametroAcademico.pac_descricao;
        _lblNome_Par.Text = _VS_pac_descricao;

        if (_ParametroAcademico.pac_chave == eChaveAcademico.QUANTIDADE_DOCENTES_VIGENTES_DISCIPLINA.ToString())
            _lblNome_Par.Text += " (no máximo " + Enum.GetValues(typeof(EnumTipoDocente)).Length.ToString() + ")";

        if (_VS_pac_obrigatorio)
        {
            _lblNome_Par.Text += " *";
        }

        if (parametroCombo.Visible)
        {
            if (!string.IsNullOrEmpty(_ParametroAcademico.pac_valor))
            {
                _ddlParametroAcademicoValor.SelectedValue = _ParametroAcademico.pac_valor;
            }

            _txtVigenciaIni.Text = _ParametroAcademico.pac_vigenciaInicio.ToString("dd/MM/yyyy");
            _txtVigenciaFim.Text = (_ParametroAcademico.pac_vigenciaFim != new DateTime()) ? _ParametroAcademico.pac_vigenciaFim.ToString("dd/MM/yyyy") : "";
        }
        else if (parametroTextBox.Visible)
        {
            _txtValor.Text = _ParametroAcademico.pac_valor;

            //Limita o número máximo de caracteres permitidos em 2 para inclusão do valor do parametro AMPLITUDE_IDADE_ALERTA
            if (_ParametroAcademico.pac_chave == eChaveAcademico.AMPLITUDE_IDADE_ALERTA.ToString())
            {
                _txtValor.MaxLength = 2;
            }
            else if (_ParametroAcademico.pac_chave == eChaveAcademico.QUANTIDADE_DOCENTES_VIGENTES_DISCIPLINA.ToString())
            {
                _txtValor.MaxLength = 1;
                _txtValor.CssClass += " numeric";

                ScriptManager.RegisterStartupScript(this, GetType(), "PermiteNoMaximo5", "$('#" + _txtValor.ClientID + "').bind('keyup blur', function(e){e.preventDefault();if(parseInt(this.value) > " + Enum.GetValues(typeof(EnumTipoDocente)).Length.ToString() + ")this.value = '';})", true);
            }
            else if (_ParametroAcademico.pac_chave == eChaveAcademico.NOME_WS_RETORNA_IMAGEM_CARTEIRA_VACINACAO.ToString())
            {
                _txtValor.MaxLength = 1000;
            }
            else
            {
                _txtValor.MaxLength = 50;
            }
        }
        else if (parametroData.Visible)
        {
            txtData.Text = _ParametroAcademico.pac_valor;
        }
    }

    /// <summary>
    /// Insere e altera um Parametro Academico.
    /// </summary>
    private void _SalvarParametro()
    {
        try
        {
            _lblNome_Par.Text = _VS_pac_descricao;
            ACA_ParametroAcademico _ParametroAcademico = new ACA_ParametroAcademico
            {
                ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id
                ,
                pac_id = _VS_pac_id
                ,
                pac_chave = _VS_pac_chave
                ,
                pac_obrigatorio = _VS_pac_obrigatorio
                ,
                pac_valor = (parametroCombo.Visible)
                    ? _ddlParametroAcademicoValor.SelectedValue
                    : (parametroTextBox.Visible) ? _txtValor.Text : txtData.Text
                ,
                pac_descricao = _VS_pac_descricao
                ,
                pac_vigenciaInicio = String.IsNullOrEmpty(_txtVigenciaIni.Text) ? DateTime.Now : Convert.ToDateTime(_txtVigenciaIni.Text)
                ,
                pac_vigenciaFim = String.IsNullOrEmpty(_txtVigenciaFim.Text) ? new DateTime() : Convert.ToDateTime(_txtVigenciaFim.Text)
                ,
                pac_situacao = 1
                ,
                IsNew = !(_VS_pac_id > 0)
            };

            if (ACA_ParametroAcademicoBO.Save(_ParametroAcademico, VS_TipoParametro))
            {
                // Recarrega a lista de parâmetros do sistema.
                ACA_ParametroAcademicoBO.RecarregaListaParametrosVigente();

                //Ordena a tabela tipo disciplina atraves do campo tds_ordem se o parametro academico for CONTROLAR_ORDEM_DISCIPLINAS
                if ((_VS_pac_chave == "CONTROLAR_ORDEM_DISCIPLINAS") && (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id)))
                    ACA_TipoDisciplinaBO.OrdenaTiposDisciplinas();

                _lblMessageInsert.Text = _VS_pac_id > 0 ? UtilBO.GetErroMessage("Parâmetro acadêmico alterado com sucesso.", UtilBO.TipoMensagem.Sucesso) : UtilBO.GetErroMessage("Parâmetro acadêmico incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                _VS_pac_id = -1;
                LimpaCampos();

                if (VS_TipoParametro == TipoParametroAcademico.Unico)
                {
                    // Fecha janela.
                    Carrega_VS_ParametrosAcademico();
                    _updGridParametrosAcademicos.Update();
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "ParametroAcademicoCancelar", "$('#divParametroAcademico').dialog('close');", true);
                    _lblMessage.Text = UtilBO.GetErroMessage("Parâmetro acadêmico alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    _updMessage.Update();
                }
                else
                {
                    MontaDivEdicaoParametro();
                }
            }
            else
            {
                _lblMessageInsert.Text = UtilBO.GetErroMessage("Erro ao tentar salvar valor para o parâmetro acadêmico.", UtilBO.TipoMensagem.Erro);
            }
        }
        catch (ValidationException ex)
        {
            _lblMessageInsert.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (DuplicateNameException ex)
        {
            _lblMessageInsert.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (ArgumentException ex)
        {
            _lblMessageInsert.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessageInsert.Text = UtilBO.GetErroMessage("Erro ao tentar salvar valor para o parâmetro acadêmico.", UtilBO.TipoMensagem.Erro);
        }
        finally
        {
            _updCadastroParametroAcademico.Update();
        }
    }

    /// <summary>
    /// Valida o valor no combo e no textbox caso o parametro valor da validade de solicitação de vaga esteja sendo alterado.
    /// </summary>
    /// <returns></returns>
    private bool _Valida()
    {
        if (parametroCombo.Visible)
        {
            if ((_ddlParametroAcademicoValor.SelectedValue.Equals("-1")) && (_VS_pac_obrigatorio))
            {
                _lblMessageInsert.Text = UtilBO.GetErroMessage(_VS_pac_descricao + " é obrigatório.", UtilBO.TipoMensagem.Alerta);
                return false;
            }
        }
        if (parametroTextBox.Visible)
        {
            if ((string.IsNullOrEmpty(_txtValor.Text)) && (_VS_pac_obrigatorio))
            {
                _lblMessageInsert.Text = UtilBO.GetErroMessage(_VS_pac_descricao + " é obrigatório.", UtilBO.TipoMensagem.Alerta);
                return false;
            }
        }

        //Validaçao do campo de valor da validade da vaga, que varia de acordo com seu tipo

        #region Validacao_SOLICITACAO_VAGA_VALIDADE_VALOR

        if (_VS_pac_chave == "SOLICITACAO_VAGA_VALIDADE_VALOR")
        {
            if (!string.IsNullOrEmpty(_txtValor.Text))
            {
                //Retorna o parametro SOLICITACAO_VAGA_VALIDADE_TIPO para validar o mesmo em relaçao ao parametro SOLICITACAO_VAGA_VALIDADE_VALOR
                Int32 tipo = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.SOLICITACAO_VAGA_VALIDADE_TIPO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                //Valida o valor informado de acordo com o tipo que foi definido(hora(s) ou dia(s))
                if (tipo == 1 || tipo == 2)
                {
                    Regex regex = new Regex(@"^[0-9]+$");
                    if (!regex.IsMatch(_txtValor.Text))
                    {
                        _lblMessageInsert.Text = UtilBO.GetErroMessage(_VS_pac_descricao + " inválido.", UtilBO.TipoMensagem.Alerta);
                        return false;
                    }
                }

                //Valida o valor informado de acordo com o tipo definido como data
                else
                {
                    Regex regex = new Regex(@"^([0-9]|[0,1,2][0-9]|3[0,1])/([\d]|1[0,1,2])/\d{4}$");
                    if (!regex.IsMatch(_txtValor.Text))
                    {
                        _lblMessageInsert.Text = UtilBO.GetErroMessage(_VS_pac_descricao + " não está no formato dd/mm/aaaa ou é inexistente.", UtilBO.TipoMensagem.Alerta);
                        return false;
                    }
                }
            }
            else
            {
                _lblMessageInsert.Text = UtilBO.GetErroMessage(_VS_pac_descricao + " é obrigatório.", UtilBO.TipoMensagem.Alerta);
                return false;
            }
        }

        #endregion Validacao_SOLICITACAO_VAGA_VALIDADE_VALOR

        return true;
    }

    #region Parâmetros de busca de alunos

    private void _PesquisarParametrosBuscaAluno()
    {
        try
        {
            _grvParametroBuscaAluno.PageIndex = 0;
            odsParametroBuscaAluno.SelectParameters.Clear();
            odsParametroBuscaAluno.SelectParameters.Add("pba_id", "0");
            odsParametroBuscaAluno.SelectParameters.Add("pba_tipo", "0");
            odsParametroBuscaAluno.SelectParameters.Add("tdo_id", Guid.Empty.ToString());
            odsParametroBuscaAluno.SelectParameters.Add("pba_integridade", "false");
            odsParametroBuscaAluno.SelectParameters.Add("pba_situacao", "0");
            odsParametroBuscaAluno.SelectParameters.Add("paginado", "true");
            odsParametroBuscaAluno.DataBind();
            _grvParametroBuscaAluno.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os parâmetros de busca de aluno.", UtilBO.TipoMensagem.Erro);
            _updMessage.Update();
        }
    }

    private bool _ValidarParametroBuscaAluno()
    {
        if (_ddlTipoParametroBuscaAluno.SelectedValue == "-1")
        {
            _lblMessageParametroBuscaAluno.Text = UtilBO.GetErroMessage("Tipo é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (_ddlTipoParametroBuscaAluno.SelectedValue == "1")
        {
            if (UCComboTipoDocumentacao1._Combo.SelectedValue == Guid.Empty.ToString())
            {
                _lblMessageParametroBuscaAluno.Text = UtilBO.GetErroMessage("Tipo de documentação é obrigatório.", UtilBO.TipoMensagem.Alerta);
                return false;
            }
        }

        return true;
    }

    private void _SalvarParametroBuscaAluno()
    {
        try
        {
            ACA_ParametroBuscaAluno _ParametroBuscaAluno = new ACA_ParametroBuscaAluno
            {
                pba_id = -1
                ,
                pba_tipo = _ddlTipoParametroBuscaAluno.SelectedValue != "-1" ? Convert.ToByte(_ddlTipoParametroBuscaAluno.SelectedValue) : Convert.ToByte(0)
                ,
                tdo_id = _ddlTipoParametroBuscaAluno.SelectedValue == Guid.Empty.ToString() ? new Guid() : new Guid(UCComboTipoDocumentacao1._Combo.SelectedValue)
                ,
                pba_integridade = false
                ,
                pba_situacao = 1
                ,
                IsNew = true
            };

            if (ACA_ParametroBuscaAlunoBO.Save(_ParametroBuscaAluno))
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Parâmetro de busca de aluno incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                _updMessage.Update();
            }
            else
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar incluir o parâmetro de busca de aluno.", UtilBO.TipoMensagem.Erro);
            }
        }
        catch (ValidationException ex)
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
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar incluir o parâmetro de busca de aluno.", UtilBO.TipoMensagem.Erro);
        }
        finally
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "CadastroParametroBuscaAluno", "$('#divParametroBuscaAluno').dialog('close');", true);
            _PesquisarParametrosBuscaAluno();
            _updGridParametroBuscaAluno.Update();
            _updMessage.Update();
        }
    }

    #endregion Parâmetros de busca de alunos

    #endregion Métodos

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            sm.Scripts.Add(new ScriptReference("~/Includes/JS-ModuloAcademico.js"));
        }

        if (!IsPostBack)
        {
            _revVigenciaIni.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de vigência inicial");
            _revVigenciaFim.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de vigência final");

            _grvParametroBuscaAluno.PageSize = ApplicationWEB._Paginacao;
            gvValoresParametrosAcademicos.PageSize = ApplicationWEB._Paginacao;

            try
            {
                Carrega_VS_ParametrosAcademico();

                UCComboTipoDocumentacao1._Label.Text = "Tipo de documentação";
                UCComboTipoDocumentacao1._MostrarMessageSelecione = true;
                UCComboTipoDocumentacao1._Load(Guid.Empty, 0);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                _updMessage.Update();
            }

            _btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            _btnSalvarParametroBuscaAluno.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            _btnNovoParametroBuscaAluno.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }
    }

    protected void gvParametrosAcademicos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TipoParametroAcademico tipo = (TipoParametroAcademico)DataBinder.Eval(e.Row.DataItem, "tipo");
            bool integridadeEscolas = (bool)DataBinder.Eval(e.Row.DataItem, "integridadeEscolas");

            ImageButton _btnAlterar = (ImageButton)e.Row.FindControl("_btnAlterar");
            if (_btnAlterar != null)
            {
                _btnAlterar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }

            string pac_valor = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "pac_valor_nome"));

            // Item do tipo Único com a flag integridadeEscolas não pode ser alterado
            // caso já tenha sido cadastrada uma escola no sistema.
            if ((tipo == TipoParametroAcademico.Unico) &&
                (integridadeEscolas) &&
                (!String.IsNullOrEmpty(pac_valor)) &&
                (ExisteEscolaCadastrada))
            {
                if (_btnAlterar != null)
                    _btnAlterar.Visible = false;
            }

            if (tipo == TipoParametroAcademico.Unico || tipo == TipoParametroAcademico.Multiplo)
            {
                // Não tem vigência.
                e.Row.Cells[2].Text = "*";
            }

            string pac_chave = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "pac_chave"));

            // Se existe mais de um valor cadastrado
            // exibe um '-' como valor.
            if (pac_chave == eChaveAcademico.TIPO_UNIDADE_ADMINISTRATIVA_ESCOLA.ToString()
                || pac_chave == eChaveAcademico.PROG_SOCIAL_DESEMPENHO_DISCIPLINAS_MEDIA_2_ANO.ToString()
                || pac_chave == eChaveAcademico.PROG_SOCIAL_DESEMPENHO_DISCIPLINAS_MEDIA_3_9_ANO.ToString()
                || pac_chave == eChaveAcademico.TIPO_DISCIPLINA_CONTROLE_FREQUENCIA.ToString()
                || pac_chave == eChaveAcademico.GRUPO_RESPONSAVEL_BOLETIMONLINE.ToString()
                || pac_chave == eChaveAcademico.TIPO_DISCIPLINA_ENRIQUECIMENTO_CURRICULAR.ToString()
                || pac_chave == eChaveAcademico.PROG_SOCIAL_PROCESSAR_FREQUENCIAS.ToString()
                || pac_chave == String.Empty
                )
            {
                DataTable dtParametrosAcademicos = ACA_ParametroAcademicoBO.SelecionaParametroValores(__SessionWEB.__UsuarioWEB.Usuario.ent_id, pac_chave, false, 1, 1);
                if (dtParametrosAcademicos.Rows.Count > 0)
                {
                    e.Row.Cells[indiceColunaPacValorNome].Text = "-";
                }
            }
        }
    }

    protected void gvParametrosAcademicos_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            e.Cancel = true;

            _VS_pac_chave = Convert.ToString((gvParametrosAcademicos.DataKeys[e.NewEditIndex].Values[1]));
            _VS_pac_obrigatorio = Convert.ToBoolean((gvParametrosAcademicos.DataKeys[e.NewEditIndex].Values[2]));
            _VS_pac_descricao = Convert.ToString((gvParametrosAcademicos.DataKeys[e.NewEditIndex].Values[3]));
            VS_TipoParametro = (TipoParametroAcademico)gvParametrosAcademicos.DataKeys[e.NewEditIndex].Values[4];

            SetaBuscaComboValor(_VS_pac_chave);

            MontaDivEdicaoParametro();

            _updCadastroParametroAcademico.Update();
            ScriptManager.RegisterStartupScript(this, GetType(), "CadastroParametroAcademico", "$('#divParametroAcademico').dialog('open');", true);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            _updMessage.Update();
        }
    }

    protected void gvValoresParametrosAcademicos_RowEditing(object sender, GridViewEditEventArgs e)
    {
        e.Cancel = true;

        _CarregarParametro(Convert.ToInt32(gvValoresParametrosAcademicos.DataKeys[e.NewEditIndex].Value));

        _updCadastroParametroAcademico.Update();
    }

    protected void gvValoresParametrosAcademicos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Deletar")
        {
            try
            {
                ACA_ParametroAcademico entity = new ACA_ParametroAcademico
                {
                    ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id
                    ,
                    pac_id = Convert.ToInt32(e.CommandArgument)
                };

                if (ACA_ParametroAcademicoBO.Delete(entity, VS_TipoParametro))
                {
                    _lblMessageInsert.Text = UtilBO.GetErroMessage("Parâmetro acadêmico excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    gvValoresParametrosAcademicos.PageIndex = 0;
                    // Recarrega a lista de parâmetros do sistema.
                    ACA_ParametroAcademicoBO.RecarregaListaParametrosVigente();
                }
                else
                {
                    _lblMessageInsert.Text = UtilBO.GetErroMessage("Não foi possível excluir o parâmetro.", UtilBO.TipoMensagem.Erro);
                }
            }
            catch (ValidationException ex)
            {
                _lblMessageInsert.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessageInsert.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o parâmetro.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                gvParametrosAcademicos.PageIndex = 0;
                _VS_pac_id = -1;
                MontaDivEdicaoParametro();
                _updCadastroParametroAcademico.Update();
            }
        }
    }

    protected void gvValoresParametrosAcademicos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (VS_TipoParametro == TipoParametroAcademico.VigenciaObrigatorio)
            {
                DateTime vigenciaInicio = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "pac_vigenciaInicio"));

                string vigenciafim = DataBinder.Eval(e.Row.DataItem, "pac_vigenciaFim").ToString();

                if ((!String.IsNullOrEmpty(vigenciafim) && Convert.ToDateTime(vigenciafim) != new DateTime())
                    || (vigenciaInicio.Date <= DateTime.Now.Date))
                {
                    // Esconde o botão de excluir caso existe vigência fim.
                    // Parâmetros obrigatórios só pode excluir o último registro cadastrado
                    // (que está sem vigência fim).
                    ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");

                    if (_btnExcluir != null)
                        _btnExcluir.Visible = false;
                }
            }

            if (_VS_pac_chave == "TIPO_UNIDADE_ADMINISTRATIVA_ESCOLA")
            {
                // Esconde o botão excluir caso o tipo de unidade administrativa
                // já esteja relacionado com alguma escola. Neste caso, não é
                // possível excluí-lo.
                int pac_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "pac_id"));
                ACA_ParametroAcademico entityParametroAcademico = new ACA_ParametroAcademico
                {
                    ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id
                    ,
                    pac_id = pac_id
                };
                ACA_ParametroAcademicoBO.GetEntity(entityParametroAcademico);

                if (ESC_EscolaBO.VerificaEscolaComTipo(new Guid(entityParametroAcademico.pac_valor)))
                {
                    ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");

                    if (_btnExcluir != null)
                        _btnExcluir.Visible = false;
                }
            }
        }
    }

    protected void _btnSalvar_Click(object sender, EventArgs e)
    {
        if (_Valida())
            _SalvarParametro();
    }

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Carrega_VS_ParametrosAcademico();
        _updGridParametrosAcademicos.Update();
        _updMessage.Update();
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "ParametroAcademicoCancelar", "$('#divParametroAcademico').dialog('close');", true);
    }

    #region Parâmetros de busca de alunos

    protected void _btnNovoParametroBuscaAluno_Click(object sender, EventArgs e)
    {
        _ddlTipoParametroBuscaAluno.SelectedValue = "-1";
        UCComboTipoDocumentacao1._Combo.SelectedValue = Guid.Empty.ToString();

        UCComboTipoDocumentacao1._Combo.Visible = false;
        UCComboTipoDocumentacao1._Label.Visible = false;

        _ddlTipoParametroBuscaAluno.Focus();
        _updCadastroParametroBuscaAluno.Update();
        ScriptManager.RegisterStartupScript(this, GetType(), "CadastroParametroBuscaAlunoNovo", "$('#divParametroBuscaAluno').dialog('open');", true);
    }

    protected void _btnSalvarParametroBuscaAluno_Click(object sender, EventArgs e)
    {
        if (_ValidarParametroBuscaAluno())
            _SalvarParametroBuscaAluno();
        else
        {
            _updCadastroParametroAcademico.Update();
        }
    }

    protected void _ddlTipoParametroBuscaAluno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_ddlTipoParametroBuscaAluno.SelectedValue == "1")
        {
            UCComboTipoDocumentacao1._Combo.Visible = true;
            UCComboTipoDocumentacao1._Label.Visible = true;
            UCComboTipoDocumentacao1._Combo.Focus();
        }
        else
        {
            UCComboTipoDocumentacao1._Combo.Visible = false;
            UCComboTipoDocumentacao1._Label.Visible = false;
        }

        _updCadastroParametroBuscaAluno.Update();
    }

    protected void odsParametroBuscaAluno_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
    }

    protected void odsParametroBuscaAluno_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if ((bool)e.ReturnValue)
            {
                _grvParametroBuscaAluno.PageIndex = 0;
                _lblMessage.Text = UtilBO.GetErroMessage("Parâmetro excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                _updMessage.Update();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar exibir mensagem de exclusão de parâmetro.", UtilBO.TipoMensagem.Erro);
            _updMessage.Update();
        }
        finally
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
        }
    }

    protected void _grvParametroBuscaAluno_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");
            if (_btnExcluir != null)
            {
                _btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
            }
        }
    }

    #endregion Parâmetros de busca de alunos

    protected void gvValoresParametrosAcademicos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvValoresParametrosAcademicos.PageIndex = e.NewPageIndex;
        MontaDivEdicaoParametro();
    }

    #endregion Eventos
}