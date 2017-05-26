<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Relatorios.DivergenciasAulasPrevistas.Busca" %>

<%@ Register Src="../../WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCCCalendario" TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/Combos/Novos/UCCPeriodoCalendario.ascx" TagName="UCCPeriodoCalendario" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagPrefix="uc2" TagName="UCComboUAEscola" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagens" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="ValidaCampos" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlBusca" runat="server" GroupingText="">
        <div class="area-form">
            <uc2:UCComboUAEscola runat="server" ID="UCComboUAEscola" MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true"
                CarregarEscolaAutomatico="true" ValidationGroup="ValidaCampos" FiltroEscolasControladas="true"/>
            <uc2:UCCCalendario ID="UCCCalendario1" runat="server" MostrarMensagemSelecione="true" SelecionarAnoCorrente="true" PermiteEditar="false" Obrigatorio="true"
                ValidationGroup="ValidaCampos" />
            <uc3:UCCPeriodoCalendario ID="UCCPeriodoCalendario1" runat="server" MostrarMensagemSelecione="true" TrazerComboCarregado="true" PermiteEditar="false"
                Obrigatorio="true" ValidationGroup="ValidaCampos" MostrarOpcaoFinal="false" SelecionaPeriodoAtualAoCarregar="true" />
        </div>
        <div class="right area-botoes-bottom">
            <asp:Button ID="btnGerar" runat="server" Text="<%$ Resources:Relatorios, ComponentesFinalizados.Busca.btnGerarRel.Text %>" ValidationGroup="ValidaCampos" OnClick="btnGerar_Click"/>
        </div>
    </asp:Panel>
</asp:Content>
