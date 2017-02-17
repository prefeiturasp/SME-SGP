<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboAvaliacao.ascx.cs" Inherits="GestaoEscolar.WebControls.Combos.UCComboAvaliacao" %>

<asp:Label ID="lblTitulo" runat="server" Text="Avaliação" AssociatedControlID="ddlAvaliacao"></asp:Label>
<asp:DropDownList ID="ddlAvaliacao" runat="server" AppendDataBoundItems="True" DataTextField="ava_nome"
    DataValueField="ava_id" CssClass="text60C" OnSelectedIndexChanged="ddlAvaliacao_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvAvaliacao" runat="server" ErrorMessage="Avaliação é obrigatório."
    ControlToValidate="ddlAvaliacao" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
