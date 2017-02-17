<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" 
Inherits="Configuracao_TipoEquipamentoDeficiente_Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/TipoEquipamentoDeficiente/Busca.aspx" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <fieldset>
        <legend>
           <asp:Label runat="server" ID="lblLegend" 
                      Text="<%$ Resources:Configuracao, TipoEquipamentoDeficiente.Cadastro.lblLegend.Text %>"></asp:Label>
        </legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:Label ID="lblTipoEquipamentoDeficiente" runat="server" 
            Text="<%$ Resources:Configuracao, TipoEquipamentoDeficiente.Cadastro.lblTipoEquipamentoDeficiente.Text %>"
            AssociatedControlID="txtTipoEquipamentoDeficiente"></asp:Label>
        <asp:TextBox ID="txtTipoEquipamentoDeficiente" runat="server" SkinID="text60C" MaxLength="100"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvTipoEquipamentoDeficiente" runat="server" 
            ErrorMessage="<%$ Resources:Configuracao, TipoEquipamentoDeficiente.Cadastro.rfvTipoEquipamentoDeficiente.ErrorMessage %>"
            Display="Dynamic" ControlToValidate="txtTipoEquipamentoDeficiente">*</asp:RequiredFieldValidator>      
        <div class="right">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                OnClick="btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
