<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCCalendario.ascx.cs" 
    Inherits="GestaoEscolar.WebControls.Combos.Novos.UCCCalendario" %>

<asp:Label ID="lblTitulo" runat="server" Text="Calendário escolar" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" 
    DataTextField="cal_ano_desc" DataValueField="cal_id" SkinID="text60C" 
    OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Calendário escolar é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" Display="Dynamic" Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>