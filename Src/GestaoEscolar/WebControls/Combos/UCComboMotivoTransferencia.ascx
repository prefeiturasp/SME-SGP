<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboMotivoTransferencia.ascx.cs"
    Inherits="GestaoEscolar.WebControls.Combos.UCComboMotivoTransferencia" %>
<asp:Label ID="lblTitulo" runat="server" Text="Motivo de transferência" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True"
    DataTextField="mot_nome" DataValueField="mot_id" SkinID="text60C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Motivo de transferência é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
