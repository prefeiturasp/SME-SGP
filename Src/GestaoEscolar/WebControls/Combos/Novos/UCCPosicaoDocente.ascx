<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCPosicaoDocente.ascx.cs"
    Inherits="GestaoEscolar.WebControls.Combos.Novos.UCCPosicaoDocente" %>
<asp:Label ID="lblTitulo" runat="server" Text="Posição do docente" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" SkinID="text30C"
    OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Posição do docente é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" Display="Dynamic" Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>