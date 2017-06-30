<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboTipoTurma.ascx.cs" Inherits="WebControls_Combos_UCComboTipoTurma" %>
<asp:Label ID="lblTitulo" runat="server" Text="Tipo de turma" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" SkinID="text30C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
    <asp:ListItem Value="0">-- Selecione um tipo de turma --</asp:ListItem>
    <asp:ListItem Value="1">Normal</asp:ListItem>
    <asp:ListItem Value="2">Recuperação paralela</asp:ListItem>
    <asp:ListItem Value="3">Multisseriada</asp:ListItem>
    <asp:ListItem Value="5">Atendimento educacional especializado</asp:ListItem>
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Tipo de turma é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>