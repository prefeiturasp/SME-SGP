<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Academico.OrientacaoCurricular.Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagPrefix="uc1" TagName="UCCamposObrigatorios" %>
<%@ Register Src="~/WebControls/Combos/UCComboCursoCurriculo.ascx" TagName="UCComboCursoCurriculo" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboCurriculoPeriodo.ascx" TagName="UCComboCurriculoPeriodo" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoDisciplina.ascx" TagName="UCComboTipoDisciplina" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboCalendario.ascx" TagName="UCComboCalendario" TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Combos/UCComboMatrizHabilidades.ascx" TagName="UCComboMatrizHabilidades" TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional" EnableViewState="false">
        <ContentTemplate>
            <asp:ValidationSummary ID="vsOrientacao" runat="server" ValidationGroup="OrientacaoCurricular" />
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlPesquisa" runat="server" GroupingText="Busca de orientações curriculares">
        <uc1:UCCamposObrigatorios runat="server" ID="UCCamposObrigatorios" />
        <uc2:UCComboCursoCurriculo ID="UCComboCursoCurriculo1" runat="server" MostrarMessageSelecione="true" Obrigatorio="true" ValidationGroup="OrientacaoCurricular" />
        <uc3:UCComboCurriculoPeriodo ID="UCComboCurriculoPeriodo1" runat="server" MostrarMessageSelecione="true" Obrigatorio="true" ValidationGroup="OrientacaoCurricular" />        
        <uc4:UCComboTipoDisciplina ID="UCComboTipoDisciplina1" runat="server" MostrarMensagemSelecione="true" Obrigatorio="true" ValidationGroup="OrientacaoCurricular" />
        <uc5:UCComboCalendario ID="UCComboCalendario1" runat="server" Obrigatorio="true" MostrarMensagemSelecione="true" ValidationGroup="OrientacaoCurricular" />
        <uc6:UCComboMatrizHabilidades id="UCComboMatrizHabilidades" runat="server" mostrarmessageselecione="true" obrigatorio="true" ValidationGroup="OrientacaoCurricular"/>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" ValidationGroup="OrientacaoCurricular" PostBackUrl="~/Academico/OrientacaoCurricular/Cadastro.aspx" />
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" CausesValidation="false" OnClick="btnLimparPesquisa_Click" />
        </div>
    </asp:Panel>
</asp:Content>
