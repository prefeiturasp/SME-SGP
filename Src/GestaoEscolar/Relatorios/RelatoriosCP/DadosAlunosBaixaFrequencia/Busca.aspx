<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Relatorios.RelatoriosCP.DadosAlunosBaixaFrequencia.Busca" %>

<%@ Register Src="~/WebControls/BuscaDocente/UCBuscaDocenteTurma.ascx" TagPrefix="uc1" TagName="UCBuscaDocenteTurma" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagPrefix="uc2" TagName="UCCamposObrigatorios" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCPeriodoCalendario.ascx" TagPrefix="uc3" TagName="UCCPeriodoCalendario" %>
<%@ Register Src="~/WebControls/Combos/UCComboTurmaDisciplina.ascx" TagPrefix="uc4" TagName="UCComboTurmaDisciplina" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary" runat="server" />
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Panel ID="pnlBusca" runat="server">
        <div class="area-form">
            <uc2:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
            <asp:UpdatePanel ID="updFiltros" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:UCBuscaDocenteTurma ID="UCBuscaDocenteTurma" runat="server" _VS_CarregarApenasTurmasNormais="true" />
                    <uc3:UCCPeriodoCalendario ID="UCCPeriodoCalendario" runat="server" MostrarMensagemSelecione="true" Obrigatorio="false" PermiteEditar="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="right area-botoes-bottom">
            <asp:Button ID="btnGerar" runat="server" Text="<%$ Resources:Relatorios, RelatoriosCP.DadosAlunosBaixaFrequencia.Busca.btnGerarRel.Text %>" OnClick="btnGerar_Click" />
        </div>
    </asp:Panel>
</asp:Content>
