<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Relatorios.LogNotificacoes.Busca" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagPrefix="uc3" TagName="UCCamposObrigatorios" %>

<%@ Register src="~/WebControls/Combos/UCComboAlerta.ascx" TagName="UCComboAlerta" 
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="Relatorio" />
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
        <asp:UpdatePanel ID="updFiltros" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset id="fdsRelatorioSondagem" runat="server" style="margin-left: 10px;">
                    <legend id="relTitulo" runat="server"></legend>
                    <uc3:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
                    <uc1:UCComboAlerta ID="UCComboAlerta1" runat="server" ValidationGroup="Relatorio" />
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
                    <div class="right area-botoes-bottom">
                        <asp:Button ID="btnGerar" runat="server" Text="<%$ Resources:Relatorios, LogNotificacoes.Busca.btnGerarRel.Text %>" OnClick="btnGerar_Click" ValidationGroup="Relatorio" />
                        <asp:Button ID="btnLimparPesquisa" runat="server" Text="<%$ Resources:Relatorios, LogNotificacoes.Busca.btnLimparPesquisa.Text %>" OnClick="btnLimparPesquisa_Click"
                            CausesValidation="false" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>