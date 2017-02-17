<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboAreaConhecimento.ascx.cs" Inherits="GestaoEscolar.WebControls.Combos.UCComboAreaConhecimento" %>
<asp:Label ID="lblTitulo" runat="server" Text="Área de conhecimento" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataTextField="aco_nome"
    DataValueField="aco_id">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Área de conhecimento é obrigatória."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>