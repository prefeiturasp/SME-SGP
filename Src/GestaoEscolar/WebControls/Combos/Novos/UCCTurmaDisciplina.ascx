<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCTurmaDisciplina.ascx.cs" 
    Inherits="GestaoEscolar.WebControls.Combos.Novos.UCCTurmaDisciplina" %>

<asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:UserControl, UCCTurmaDisciplina.lblTitulo.Text %>" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" 
    DataTextField="tud_nome" DataValueField="tud_id" SkinID="text60C" 
    OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="<%$ Resources:UserControl, UCCTurmaDisciplina.cpvCombo.ErrorMessage %>"
    ControlToValidate="ddlCombo" Operator="NotEqual" Display="Dynamic" Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>
