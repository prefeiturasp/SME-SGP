<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboTipoCurriculoPeriodo.ascx.cs" Inherits="GestaoEscolar.WebControls.Combos.UCComboTipoCurriculoPeriodo" %>
<asp:Label ID="lblTitulo" runat="server" Text="Tipo currículo período" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataTextField="tcp_descricao"
    DataValueField="tcp_id" AutoPostBack="true" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Tipo currículo período é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>