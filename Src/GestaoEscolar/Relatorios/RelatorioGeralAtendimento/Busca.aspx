<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Relatorios.RelatorioGeralAtendimento.Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagPrefix="uc3" TagName="UCCamposObrigatorios" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="Relatorio" />
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlBusca" runat="server">
        <div class="area-form">
            <uc3:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
            <asp:UpdatePanel ID="updFiltros" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc4:UCComboAnoLetivo ID="UCComboAnoLetivo1" runat="server" Obrigatorio="true" ValidationGroup="Relatorio" />
                    <uc1:UCComboTipoNivelEnsino ID="UCComboTipoNivelEnsino1" runat="server" Obrigatorio="false" MostrarMessageSelecione="true" />
                    <uc2:UCComboTipoModalidadeEnsino ID="UCComboTipoModalidadeEnsino1" runat="server" Obrigatorio="false" MostrarMessageSelecione="true" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="right area-botoes-bottom">
            <asp:Button ID="btnGerar" runat="server" Text="<%$ Resources:Relatorios, QuantitativoSugestoes.Busca.btnGerarRel.Text %>" OnClick="btnGerar_Click" ValidationGroup="Relatorio" />
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="<%$ Resources:Relatorios, QuantitativoSugestoes.Busca.btnLimparPesquisa.Text %>" OnClick="btnLimparPesquisa_Click"
                CausesValidation="false" />
        </div>
    </asp:Panel>
</asp:Content>
