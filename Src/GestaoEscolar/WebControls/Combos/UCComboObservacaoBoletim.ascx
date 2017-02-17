<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboObservacaoBoletim.ascx.cs"
    Inherits="GestaoEscolar.WebControls.Combos.UCComboObservacaoBoletim" %>
<asp:Label ID="lblTitulo" runat="server" Text="Tipo de observação" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged" SkinID="text60C">
    <asp:ListItem Text="<%$ Resources:UserControl, Combos.UCComboObservacaoBoletim.ddlCombo.valor1 %>" Value="1"></asp:ListItem>
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ControlToValidate="ddlCombo" Display="Dynamic"
    ErrorMessage="Tipo de observação é obrigatório." Operator="NotEqual" ValueToCompare="-1"
    Visible="false">*</asp:CompareValidator>