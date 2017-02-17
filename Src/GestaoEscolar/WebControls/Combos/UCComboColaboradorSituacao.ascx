<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboColaboradorSituacao"
    CodeBehind="UCComboColaboradorSituacao.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Situação *" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" SkinID="text30C">
    <asp:ListItem Value="-1">-- Selecione uma situação --</asp:ListItem>
    <asp:ListItem Value="1">Ativo</asp:ListItem>
    <asp:ListItem Value="2">Bloqueado</asp:ListItem>
    <asp:ListItem Value="4">Demitido</asp:ListItem>
    <asp:ListItem Value="5">Afastado</asp:ListItem>
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Situação é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
