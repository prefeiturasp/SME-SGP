<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboTipoClassificacaoEscola.ascx.cs" Inherits="GestaoEscolar.WebControls.Combos.UCComboTipoClassificacaoEscola" %>
<div ID="divUCCTipoClassificacao" runat="server" visible="false">
<asp:Label ID="lblTipoClassificacao" runat="server" Text="Tipo de classificação da escola" AssociatedControlID="ddlTipoClassificacao"></asp:Label>
<asp:DropDownList ID="ddlTipoClassificacao" runat="server" AppendDataBoundItems="True"
    DataTextField="tce_nome" DataValueField="tce_id"></asp:DropDownList>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false" SkinID="SkinMsgErroCombo"></asp:Label>
</div>