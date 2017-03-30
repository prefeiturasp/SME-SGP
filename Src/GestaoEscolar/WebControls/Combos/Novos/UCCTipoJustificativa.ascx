<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCTipoJustificativa.ascx.cs" 
    Inherits="GestaoEscolar.WebControls.Combos.Novos.UCCTipoJustificativa" %>

<asp:Label ID="lblTitulo" runat="server" AssociatedControlID="ddlCombo"
    Text="Tipo de Justificativa" />
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True"
    DataTextField="tjf_nome" DataValueField="tjf_id" SkinID="text60C"
    OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ControlToValidate="ddlCombo"
    Operator="NotEqual" Display="Dynamic" Text="*" Visible="false"
    ErrorMessage="Tipo de Justificativa é obrigatório." />
<asp:Label ID="lblMessage" runat="server" Visible="false" EnableViewState="false" SkinID="SkinMsgErroCombo" />