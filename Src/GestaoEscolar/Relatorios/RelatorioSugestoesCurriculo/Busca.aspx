<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Relatorios.RelatorioSugestoesCurriculo.Busca" %>

<%@ Register src="~/WebControls/Combos/UCComboTipoDisciplina.ascx" tagname="UCComboTipoDisciplina" tagprefix="uc1" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagPrefix="uc2" TagName="UCCamposObrigatorios" %>

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
            <uc2:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
            <asp:UpdatePanel ID="updFiltros" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:UCComboTipoDisciplina ID="UCComboTipoDisciplina1" runat="server" _MostrarMessageSelecione="true" ValidationGroup="Relatorio"
                        Obrigatorio="true" PermiteEditar="true" />
                    <div runat="server" id="divData">
                        <div style="display: inline-block">
                            <asp:Label ID="lblDataInicio" runat="server" Text="Data inicial *" AssociatedControlID="txtDataInicio"></asp:Label>
                            <asp:TextBox runat="server" ID="txtDataInicio" SkinID="Data"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDataInicio" runat="server" ErrorMessage="Data inicial é obrigatória."
                                Display="Dynamic" ControlToValidate="txtDataInicio" ValidationGroup="Relatorio">*</asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvDataInicio" runat="server" ControlToValidate="txtDataInicio"
                                Display="Dynamic" ErrorMessage="Data início deve estar no formato DD/MM/AAAA." OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                        </div>
                        &nbsp;
                        <div style="display: inline-block">
                            <asp:Label ID="lblDataFim" runat="server" Text="Data final *" AssociatedControlID="txtDataFim"></asp:Label>
                            <asp:TextBox runat="server" ID="txtDataFim" SkinID="Data"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDataFim" runat="server" ErrorMessage="Data final é obrigatória."
                                Display="Dynamic" ControlToValidate="txtDataFim" ValidationGroup="Relatorio">*</asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvDataFim" runat="server" ControlToValidate="txtDataFim"
                                ValidationGroup="Relatorio" Display="Dynamic" ErrorMessage="Data fim deve estar no formato DD/MM/AAAA." OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                            <asp:CompareValidator ID="cpvDataFim" runat="server" ControlToValidate="txtDataFim"
                                ValidationGroup="Relatorio" Display="Dynamic" Type="Date" Operator="GreaterThanEqual"
                                ControlToCompare="txtDataInicio" ErrorMessage="Data fim deve ser maior ou igual à data início.">*</asp:CompareValidator>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="right area-botoes-bottom">
            <asp:Button ID="btnGerar" runat="server" Text="<%$ Resources:Relatorios, RelatorioSugestoesCurriculo.Busca.btnGerarRel.Text %>" OnClick="btnGerar_Click" ValidationGroup="Relatorio" />
                        <asp:Button ID="btnLimparPesquisa" runat="server" Text="<%$ Resources:Relatorios, RelatorioSugestoesCurriculo.Busca.btnLimparPesquisa.Text %>" OnClick="btnLimparPesquisa_Click"
                            CausesValidation="false" />
        </div>
    </asp:Panel>
</asp:Content>
