<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboParametroGrupoPerfil.ascx.cs"
    Inherits="GestaoEscolar.WebControls.Combos.UCComboParametroGrupoPerfil" %>
<asp:Label ID="lblTitulo" runat="server" Text="Grupo padrão" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataTextField="pgs_chave"
    DataValueField="pgs_chave" SkinID="text60C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Grupo padrão é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>
