<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Documentos_Efetivacao_Busca" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="~/WebControls/Combos/UCComboCursoCurriculo.ascx" TagName="UCComboCursoCurriculo"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboTurma.ascx" TagName="UCComboTurma"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/FiltroEscolas/UCFiltroEscolas.ascx" TagName="UCFiltroEscolas"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboCalendario.ascx" TagName="UCComboCalendario"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Combos/UCComboCurriculoPeriodo.ascx" TagName="UCComboCurriculoPeriodo"
    TagPrefix="uc7" %>
<%@ Register Src="~/WebControls/Combos/UCComboTurmaDisciplina.ascx" TagName="UCComboTurmaDisciplina"
    TagPrefix="uc8" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:UCLoader ID="UCLoader1" runat="server" />
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset id="fdsBusca" runat="server" style="width: auto">
        <legend>Parâmetros de busca</legend>
        <uc9:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <div id="_divPesquisa" runat="server">
            <asp:UpdatePanel ID="_updFiltros" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc3:UCFiltroEscolas ID="UCFiltroEscolas1" runat="server" />
                    <uc2:UCComboCursoCurriculo ID="UCComboCursoCurriculo1" runat="server" />
                    <uc7:UCComboCurriculoPeriodo ID="UCComboCurriculoPeriodo1" runat="server" />
                    <uc5:UCComboCalendario ID="UCComboCalendario1" runat="server" />
                    <uc4:UCComboTurma ID="UCComboTurma1" runat="server" />
                    <uc8:UCComboTurmaDisciplina ID="UCComboTurmaDisciplina1" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="right">
                <asp:Button ID="_btnGerarRelatorio" runat="server" Text="Gerar documento" OnClick="_btnGerarRelatorio_Click" />
            </div>
        </div>
    </fieldset>
</asp:Content>
