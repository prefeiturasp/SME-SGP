<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboTipoRelatorioAtendimento.ascx.cs" Inherits="GestaoEscolar.WebControls.Combos.UCComboTipoRelatorioAtendimento" %>

<asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:GestaoEscolar.WebControls.Combos.UCComboTipoRelatorioAtendimento, lblTitulo.Text %>" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" SkinID="text60C" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="<%$ Resources:GestaoEscolar.WebControls.Combos.UCComboTipoRelatorioAtendimento, cpvCombo.ErrorMessage %>"
    ControlToValidate="ddlCombo" Operator="NotEqual" Display="Dynamic" Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>
