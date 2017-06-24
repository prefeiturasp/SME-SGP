using System;
using System.Collections.Generic;
using System.Data;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_BuscaAluno_UCCamposBuscaAluno : MotherUserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cvDataNascimento.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de nascimento do aluno");
    }

    #region ENUM

    protected enum eTipoDoctoID { CPF, RG }

    #endregion

    #region PROPRIEDADES

    /// <summary>
    /// Checa se todos os validation do usercontrol são válidos.
    /// </summary>
    public bool IsValid
    {
        get
        {
            cvDataNascimento.Validate();
            return cvDataNascimento.IsValid;
        }
    }
    /// Retorno o valor do text do nome do aluno.
    /// </summary>
    public string TipoBuscaNomeAluno
    {
        get { return _rblEscolhaBusca.SelectedValue; }
        set { _rblEscolhaBusca.SelectedValue = value; }
    }
    /// <summary>
    /// Retorno o valor do text do nome do aluno.
    /// </summary>
    public string NomeAluno
    {
        get { return _txtNome.Text; }
        set { _txtNome.Text = value; }
    }
    public string NomeMaeAluno
    {
        get { return _txtMae.Text; }
        set { _txtMae.Text = value; }
    }
    public string DataNascAluno
    {
        get { return _txtDataNascimento.Text; }
        set { _txtDataNascimento.Text = value; }
    }
    public string MatriculaAluno
    {
        get { return _txtMatricula.Text; }
        set { _txtMatricula.Text = value; }
    }
    public string MatriculaEstadualAluno
    {
        get { return _txtMatriculaEstadual.Text; }
        set { _txtMatriculaEstadual.Text = value; }
    }

    /// <summary>
    /// Configura os campos de matrícula estadual/matrícula.
    /// </summary>
    public bool MostrarMatriculaEstadual
    {
        set
        {
        _lblMatricula.Visible = !value;
        _txtMatricula.Visible = !value;
        _lblMatrEst.Visible = value;
        _txtMatriculaEstadual.Visible = value;
        }
    }

    /// <summary>
    /// Configura título do campo matrícula estadual
    /// </summary>
    public string TituloMatriculaEstadual
    {
        set
        {
            _lblMatrEst.Text = value;
        }
    }

    /// <summary>
    /// Propriedade que seta a visibilidade do campo nome
    /// </summary>
    public bool VisibleNome
    {
        set
        {
            divNomeAluno.Visible = value;
        }
    }

    /// <summary>
    /// Propriedade que seta a visibilidade do campo data de nascimento
    /// </summary>
    public bool VisibleDataNascimento
    {
        set
        {
            divDtNascAluno.Visible = value;
        }
    }

    /// <summary>
    /// Propriedade que seta a visibilidade do campo nome mae
    /// </summary>
    public bool VisibleNomeMae
    {
        set
        {
            divNomeMaeAluno.Visible = value;
        }
    }

    /// <summary>
    /// Propriedade que seta a visibilidade do campo matricula
    /// </summary>
    public bool VisibleMatricula
    {
        set
        {
            divMatriculaAluno.Visible = value;
        }
    }

    /// <summary>
    /// Propriedade que seta a visibilidade do campo matricula estadual
    /// </summary>
    public bool VisibleMatriculaEstadual
    {
        set
        {
            divMatriculaEstAluno.Visible = value;
        }
    }


    #endregion

    #region METODOS

    /// <summary>
    /// Limpa todos os txts do user control.
    /// </summary>
    public void LimpaCampos()
    { 
        _txtDataNascimento.Text = "";
        _txtMae.Text = "";
        _txtMatricula.Text = "";
        _txtMatriculaEstadual.Text = "";
        _txtNome.Text = "";
        _rblEscolhaBusca.SelectedValue = "2";
    }

    #endregion
}
