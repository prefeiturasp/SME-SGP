<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboMeioTransporte.ascx.cs" Inherits="GestaoEscolar.WebControls.Combos.UCComboMeioTransporte" %>
<asp:Label ID="lblMeioTransporte" runat="server" Text="Meio de transporte" AssociatedControlID="ddlMeioTransporte"
    EnableViewState="True"></asp:Label>
<asp:DropDownList ID="ddlMeioTransporte" runat="server" AppendDataBoundItems="True"
    SkinID="text30C">
    <asp:ListItem Value="-1">-- Selecione um meio de transporte --</asp:ListItem>
    <asp:ListItem Value="1">Pedestre</asp:ListItem>
    <asp:ListItem Value="2">Ônibus</asp:ListItem>
    <asp:ListItem Value="3">Trem</asp:ListItem>
    <asp:ListItem Value="4">Carro </asp:ListItem>
    <asp:ListItem Value="5">Metrô </asp:ListItem>
    <asp:ListItem Value="6">Outros </asp:ListItem>
    <asp:ListItem Value="7">Transporte escolar</asp:ListItem>
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Meio de transporte é obrigatório."
    ControlToValidate="ddlMeioTransporte" Operator="NotEqual" ValueToCompare="-1"
    Display="Dynamic" Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>