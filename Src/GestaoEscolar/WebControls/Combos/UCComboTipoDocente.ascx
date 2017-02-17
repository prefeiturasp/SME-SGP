<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboTipoDocente.ascx.cs" 
    Inherits="GestaoEscolar.WebControls.Combos.UCComboTipoDocente" %>
<asp:Label ID="lblTitulo" runat="server" Text="Tipo de docente" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" SkinID="text30C"
    OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Tipo de docente é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" Display="Dynamic" Visible="false">*</asp:CompareValidator>