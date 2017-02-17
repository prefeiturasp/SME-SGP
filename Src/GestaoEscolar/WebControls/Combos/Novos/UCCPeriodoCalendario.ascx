<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCPeriodoCalendario.ascx.cs" 
    Inherits="GestaoEscolar.WebControls.Combos.Novos.UCCPeriodoCalendario" %>

<asp:Label ID="lblTitulo" runat="server" Text="Período" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" 
    DataTextField="tpc_nome" DataValueField="tpc_cap_id" SkinID="text60C" 
    OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Período é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" Display="Dynamic" Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>