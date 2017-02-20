<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Classe.LancamentoFrequenciaExterna.Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/FiltroEscolas/UCFiltroEscolas.ascx" TagName="UCFiltroEscolas" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCCCalendario" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurma.ascx" TagName="UCCTurma" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ValidationSummary ID="vsValidacao" runat="server" EnableViewState="false" />
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlPesquisa" runat="server" GroupingText="Lançamento de ausência em outras redes">
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
        <asp:UpdatePanel ID="updPesquisa" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:UCFiltroEscolas ID="UCFiltroEscolas" runat="server" EscolaCampoObrigatorio="true" UnidadeAdministrativaCampoObrigatorio="true" />
                <uc3:UCCCalendario ID="UCCCalendario" runat="server" Obrigatorio="true" MostrarMensagemSelecione="true" />
                <uc4:UCCTurma ID="UCCTurma" runat="server" Obrigatorio="true" MostrarMensagemSelecione="true" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" />
        </div>
    </asp:Panel>
</asp:Content>
