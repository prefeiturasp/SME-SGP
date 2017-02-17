<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ComboTipoDeficiencia.ascx.cs"
    Inherits="GestaoEscolar.WebControls.Combos.ComboTipoDeficiencia" %>
<asp:Label ID="lblTitulo" runat="server" 
    Text="<%$ Resources:WebControls, Combos.ComboTipoDeficiencia.lblTitulo.Text %>" EnableViewState="false"
    AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataTextField="dataTextField"
    DataValueField="dataValueField" SkinID="text30C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" 
    ErrorMessage="<%$ Resources:WebControls, Combos.ComboTipoDeficiencia.cpvCombo.ErrorMessage %>"
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="00000000-0000-0000-0000-000000000000"
    Visible="false" Display="Dynamic">*</asp:CompareValidator>
<asp:Label ID="lblAviso" runat="server" Style="display: inline;" 
    Text="<%$ Resources:WebControls, Combos.ComboTipoDeficiencia.lblAviso.Text %>"
    Visible="false" EnableViewState="false" AssociatedControlID="ddlCombo"></asp:Label>