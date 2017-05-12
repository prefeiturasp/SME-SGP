<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboTipoTurma.ascx.cs" Inherits="GestaoEscolar.WebControls.Combos.UCComboTipoTurma" %>
<asp:Label ID="lblTitulo" runat="server" Text="Tipo de turma *" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" SkinID="text30C">
    <asp:ListItem Value="-1">-- Selecione um tipo de turma --</asp:ListItem>
    <asp:ListItem Value="1">Normal</asp:ListItem>
    <asp:ListItem Value="2">Eletiva de aluno</asp:ListItem>
    <asp:ListItem Value="3">Multisseriada</asp:ListItem>
    <asp:ListItem Value="4">Multisseriada de docente</asp:ListItem>
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Tipo de turma é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>