<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCCursoCurriculo.ascx.cs" 
    Inherits="GestaoEscolar.WebControls.Combos.Novos.UCCCursoCurriculo" %>

<asp:Label ID="lblTitulo" runat="server" Text="Curso" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" 
    DataTextField="cur_crr_nome" DataValueField="cur_crr_id" SkinID="text60C" 
    OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Curso é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" Display="Dynamic" Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>