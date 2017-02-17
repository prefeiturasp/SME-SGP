<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCTipoEvento.ascx.cs" 
    Inherits="GestaoEscolar.WebControls.Combos.Novos.UCCTipoEvento" %>

<asp:Label ID="lblTitulo" runat="server" AssociatedControlID="ddlCombo"
    Text="<%$ Resources:UserControl, UCCTipoEvento.lblTitulo.Text %>" />
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True"
    DataTextField="tev_nome" DataValueField="tev_id" SkinID="text60C"
    OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ControlToValidate="ddlCombo"
    Operator="NotEqual" Display="Dynamic" Text="*" Visible="false"
    ErrorMessage="<%$ Resources:UserControl, UCCTipoEvento.RequiredFieldValidator.ErrorMessage %>" />
<asp:Label ID="lblMessage" runat="server" Visible="false" EnableViewState="false" SkinID="SkinMsgErroCombo" />