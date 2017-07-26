<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="Academico_ConfiguracaoServicoPendencia_Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Academico/ConfiguracaoServicoPendencia/Busca.aspx" %>

<%@ Register Src="~/WebControls/Combos/UCComboTipoNivelEnsino.ascx" TagName="UCComboTipoNivelEnsino"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoModalidadeEnsino.ascx" TagName="UCComboTipoModalidadeEnsino"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoTurma.ascx" TagName="UCComboTipoTurma"
    TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsCadastro" runat="server">
        <legend><asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Cadastro.lblLegend.Text %>" /></legend>
        <uc1:UCComboTipoNivelEnsino ID="UCComboTipoNivelEnsino" runat="server" />
        <uc2:UCComboTipoModalidadeEnsino ID="UCComboTipoModalidadeEnsino" runat="server" />
        <uc3:UCComboTipoTurma ID="UCComboTipoTurma" runat="server" />
        <asp:CheckBox ID="chkSemNota" runat="server" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Cadastro.chkSemNota.Text %>" CssClass="wrap150px"></asp:CheckBox>
        <asp:CheckBox ID="chkSemParecer" runat="server" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Cadastro.chkSemParecerConclusivo.Text %>" CssClass="wrap150px"></asp:CheckBox>
        <asp:CheckBox ID="chkDisciplinaSemAula" runat="server" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Cadastro.chkDisciplinaSemAula.Text %>" CssClass="wrap150px"></asp:CheckBox>
        <asp:CheckBox ID="chkSemResultadoFinal" runat="server" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Cadastro.chkSemResultadoFinal.Text %>" CssClass="wrap150px"></asp:CheckBox>
        <asp:CheckBox ID="chkSemPlanejamento" runat="server" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Cadastro.chkSemPlanejamento.Text %>" CssClass="wrap150px"></asp:CheckBox>
        <asp:CheckBox ID="chkSemSintese" runat="server" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Cadastro.chkSemSinteseFinal.Text %>" CssClass="wrap150px"></asp:CheckBox>
        <asp:CheckBox ID="chkSemPlanoAula" runat="server" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Cadastro.chkSemPlanoAula.Text %>" CssClass="wrap150px"></asp:CheckBox>
        <asp:CheckBox ID="chkSemObjetoConhecimento" runat="server" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Cadastro.chkSemObjetoConhecimento.Text %>" CssClass="wrap150px"></asp:CheckBox>
        <asp:CheckBoxList ID="cblSemRelatorioAtendimento" runat="server" RepeatDirection="Vertical"></asp:CheckBoxList>
        <div class="right">
            <asp:Button ID="btnSalvar" runat="server" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Cadastro.btnSalvar.Text %>" OnClick="btnSalvar_Click"
                ToolTip="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Cadastro.btnSalvar.Text %>" />
            <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Cadastro.btnCancelar.Text %>" OnClick="btnCancelar_Click"
                ToolTip="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Cadastro.btnCancelar.Text %>" />
        </div>
    </fieldset>
</asp:Content>
