<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCEscalaAvaliacaoParecer.ascx.cs" 
    Inherits="WebControls_Combos_Novos_UCCEscalaAvaliacaoParecer" %>

<asp:Label ID="lblTitulo" runat="server" Text="Escala avaliação parecer" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" 
    DataTextField="descricao" DataValueField="esa_eap_ordem">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Escala avaliação parecer escolar é obrigatória."
    ControlToValidate="ddlCombo" Operator="NotEqual" Display="Dynamic" Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>