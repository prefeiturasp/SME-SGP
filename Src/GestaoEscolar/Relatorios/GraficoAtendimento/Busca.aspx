<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Relatorios.GraficoAtendimento.Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCObrigatorios" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoRelatorioAtendimento.ascx" TagName="UCCTipoRelatorioAtendimento" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/UCComboGraficoAtendimento.ascx" TagName="UCCGraficoAtendimento" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCCUAEscola" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagName="UCCCursoCurriculo" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCurriculoPeriodo.ascx" TagName="UCCCurriculoPeriodo" TagPrefix="uc" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="summary" runat="server" />
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Panel ID="pnlBusca" runat="server" GroupingText="Gráficos de atendimento">
        <uc:UCCObrigatorios ID="UCCObrigatorios" runat="server"></uc:UCCObrigatorios>
        <asp:UpdatePanel ID="updFiltros" runat="server">
            <ContentTemplate>
                <uc:UCCTipoRelatorioAtendimento ID="UCCTipoRelatorioAtendimento" runat="server" Obrigatorio="true" MostrarMensagemSelecione="true" />
                <uc:UCCGraficoAtendimento ID="UCCGraficoAtendimento" runat="server" Obrigatorio="true" MostrarMensagemSelecione="true" />
                <uc:UCCUAEscola ID="UCCUAEscola" runat="server" CarregarEscolaAutomatico="true" MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true"
                    ObrigatorioUA="true" ObrigatorioEscola="true" />
                <uc:UCCCursoCurriculo ID="UCCCursoCurriculo" MostrarMensagemSelecione="true" Obrigatorio="false"
                    runat="server" />
                <uc:UCCCurriculoPeriodo ID="UCCCurriculoPeriodo" MostrarMensagemSelecione="true" Obrigatorio="false"
                    runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="right">
            <asp:Button ID="btnGerar" runat="server" Text="Gerar relatório" PostBackUrl="~/Relatorios/GraficoAtendimento/Relatorio.aspx" />
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="btnLimparPesquisa_Click" CausesValidation="false" />
        </div>
    </asp:Panel>
</asp:Content>
