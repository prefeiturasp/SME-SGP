<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboSexo" Codebehind="UCComboSexo.ascx.cs" %>
<asp:Label ID="LabelSexo" runat="server" Text="Sexo" AssociatedControlID="_ddlSexo"></asp:Label>
<asp:DropDownList ID="_ddlSexo" runat="server" AppendDataBoundItems="True" SkinID="text30C">
    <asp:ListItem Value="-1">-- Selecione um sexo --</asp:ListItem>
    <asp:ListItem Value="1">Masculino</asp:ListItem>
    <asp:ListItem Value="2">Feminino</asp:ListItem>
</asp:DropDownList>
<asp:CompareValidator ID="cpvSexo" runat="server" ErrorMessage="Sexo é obrigatório."
    ControlToValidate="_ddlSexo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
