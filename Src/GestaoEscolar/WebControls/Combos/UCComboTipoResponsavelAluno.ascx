<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboTipoResponsavelAluno"
    CodeBehind="UCComboTipoResponsavelAluno.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Tipo de responsável" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataTextField="tra_nome"
    DataValueField="tra_id" CssClass="text60C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Tipo de responsável é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
