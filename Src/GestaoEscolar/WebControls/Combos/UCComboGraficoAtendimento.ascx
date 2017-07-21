<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboGraficoAtendimento.ascx.cs" Inherits="GestaoEscolar.WebControls.Combos.UCComboGraficoAtendimento" %>

<asp:Label ID="lblTitulo" runat="server" Text="Gráfico de atendimento" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" DataTextField="gra_titulo" DataValueField="gra_id" SkinID="text60C"
    AppendDataBoundItems="True" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Gráfico de atendimento é obrigatório"
    ControlToValidate="ddlCombo" Operator="NotEqual" Display="Dynamic" Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>