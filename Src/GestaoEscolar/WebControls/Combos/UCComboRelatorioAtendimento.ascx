<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboRelatorioAtendimento.ascx.cs" Inherits="GestaoEscolar.WebControls.Combos.UCComboRelatorioAtendimento" %>

<asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:GestaoEscolar.WebControls.Combos.UCComboRelatorioAtendimento, lblTitulo.Text %>" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" DataTextField="rea_titulo" DataValueField="rea_id" SkinID="text60C"
    AppendDataBoundItems="True" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="<%$ Resources:GestaoEscolar.WebControls.Combos.UCComboRelatorioAtendimento, cpvCombo.ErrorMessage %>"
    ControlToValidate="ddlCombo" Operator="NotEqual" Display="Dynamic" Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>
