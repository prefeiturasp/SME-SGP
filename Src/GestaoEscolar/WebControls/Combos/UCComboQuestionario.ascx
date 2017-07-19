<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboQuestionario.ascx.cs" Inherits="GestaoEscolar.WebControls.Combos.UCComboQuestionario" %>
<asp:Label ID="lblTitulo" runat="server" Text="Questionário" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataTextField="qst_titulo"
    DataValueField="qst_id" SkinID="text30C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Questionário é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>