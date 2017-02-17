<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboEntidadeGestao.ascx.cs" 
    Inherits="WebControls_Combos_UCComboEntidadeGestao" %>
<asp:Label ID="lblTitulo" runat="server" AssociatedControlID="ddlCombo" 
    Text="Entidade"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server"
    DataTextField="ent_razaoSocial" DataValueField="ent_id"  SkinID="text30C"
    AppendDataBoundItems="True" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged" AutoPostBack="true" >
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" 
    ControlToValidate="ddlCombo" Display="Dynamic" 
    ErrorMessage="{0} é obrigatório" Operator="NotEqual" ValueToCompare="00000000-0000-0000-0000-000000000000">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" visible="false" EnableViewState="false" 
    SkinID="SkinMsgErroCombo" ></asp:Label>