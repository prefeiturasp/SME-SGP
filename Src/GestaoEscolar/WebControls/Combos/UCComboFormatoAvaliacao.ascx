<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboFormatoAvaliacao" Codebehind="UCComboFormatoAvaliacao.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Formato de avaliação"
    AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True"
    DataTextField="fav_nome" DataValueField="fav_id"
    SkinID="text60C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Formato de avaliação é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" 
    ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" visible="false" EnableViewState="false" 
    SkinID="SkinMsgErroCombo" ></asp:Label>