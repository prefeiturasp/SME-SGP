<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboTipoHorario.ascx.cs" Inherits="GestaoEscolar.WebControls.Combos.UCComboTipoHorario" %>
<asp:Label ID="lblTipoHorario" runat="server" Text="<%$ Resources:UserControl, Combos.UCComboTipoHorario.lblTipoHorario.Text %>"
    AssociatedControlID="ddlTipoHorario"></asp:Label>
<asp:DropDownList ID="ddlTipoHorario" runat="server" OnSelectedIndexChanged="ddlTipoHorario_SelectedIndexChanged" CssClass="text30C ddlTipoHorario">
    <asp:ListItem Value="0" Text="<%$ Resources:UserControl, Combos.UCComboTipoHorario.ddlTipoHorario.MensagemSelecioneItem %>"></asp:ListItem>
</asp:DropDownList>
<asp:CompareValidator ID="cvTipoHorario" runat="server" ErrorMessage="<%$ Resources:UserControl, Combos.UCComboUnidadeTempo.cvTipoHorario.ErrorMessage %>"
    ControlToValidate="ddlTipoHorario" Operator="NotEqual" ValueToCompare="0"
    Display="Dynamic" Visible="True">*</asp:CompareValidator>