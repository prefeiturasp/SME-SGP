<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Academico.ConfiguracaoServicoPendencia.Busca" %>

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
    <fieldset id="fdsPesquisa" runat="server">
        <legend>Busca</legend>
        <div id="divPesquisa" runat="server">
            <uc1:UCComboTipoNivelEnsino ID="UCComboTipoNivelEnsino" runat="server"/>
            <uc2:UCComboTipoModalidadeEnsino ID="UCComboTipoModalidadeEnsino" runat="server"/>
            <uc3:UCComboTipoTurma ID="UCComboTipoTurma" runat="server"/>
        </div>
    </fieldset>
    <fieldset id="fdsConfiguracao" runat="server" visible="false">
        <legend>Configurar</legend>

        <asp:CheckBox ID="chkSemNota" runat="server" Text="Sem nota" CssClass="wrap150px"></asp:CheckBox>

        <asp:CheckBox ID="chkSemParecer" runat="server" Text="Sem parecer conclusivo" CssClass="wrap150px"></asp:CheckBox>

        <asp:CheckBox ID="chkDisciplinaSemAula" runat="server" Text="Disciplina sem aula" CssClass="wrap150px"></asp:CheckBox>

        <asp:CheckBox ID="chkSemResultadoFinal" runat="server" Text="Sem resultado final" CssClass="wrap150px"></asp:CheckBox>

        <asp:CheckBox ID="chkSemPlanejamento" runat="server" Text="Sem planejamento" CssClass="wrap150px"></asp:CheckBox>

        <asp:CheckBox ID="chkSemSintese" runat="server" Text="Sem síntese final" CssClass="wrap150px"></asp:CheckBox>

        <asp:CheckBox ID="chkSemPlanoAula" runat="server" Text="Aula sem plano de aula" CssClass="wrap150px"></asp:CheckBox>

        <div class="right">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                ToolTip="Salvar" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                ToolTip="Cancelar" />
        </div>
    </fieldset>
</asp:Content>
