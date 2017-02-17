<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboMes.ascx.cs" Inherits="WebControls_Combos_UCComboMes" %>
<div id="divMes" runat="server">
    <asp:Label ID="lblMesInicio" runat="server" Text="Mês Inicial" AssociatedControlID="ddlMesInicio"></asp:Label>
    <asp:DropDownList ID="ddlMesInicio" runat="server" AppendDataBoundItems="True" SkinID="text30C" OnSelectedIndexChanged="ddlMesInicio_SelectedIndexChanged" AutoPostBack="True">
        <asp:ListItem Value="-1">-- Selecione um mês --</asp:ListItem>
        <asp:ListItem Value="1">Janeiro</asp:ListItem>
        <asp:ListItem Value="2">Fevereiro</asp:ListItem>
        <asp:ListItem Value="3">Março</asp:ListItem>
        <asp:ListItem Value="4">Abril</asp:ListItem>
        <asp:ListItem Value="5">Maio</asp:ListItem>
        <asp:ListItem Value="6">Junho</asp:ListItem>
        <asp:ListItem Value="7">Julho</asp:ListItem>
        <asp:ListItem Value="8">Agosto</asp:ListItem>
        <asp:ListItem Value="9">Setembro</asp:ListItem>
        <asp:ListItem Value="10">Outubro</asp:ListItem>
        <asp:ListItem Value="11">Novembro</asp:ListItem>
        <asp:ListItem Value="12">Dezembro</asp:ListItem>
    </asp:DropDownList>
    <asp:CompareValidator ID="cpvComboInicial" runat="server" ErrorMessage="Mês inicial é obrigatório."
    ControlToValidate="ddlMesInicio" Operator="NotEqual" ValueToCompare="-1"
    Display="Dynamic" Visible="false">*</asp:CompareValidator>
   <div id="divFinal" runat="server">
    <asp:Label ID="lblMesFim" runat="server" Text="Mês Final" AssociatedControlID="ddlMesFim"></asp:Label>
    <asp:DropDownList ID="ddlMesFim" runat="server" AppendDataBoundItems="True" SkinID="text30C" AutoPostBack="True">
        <asp:ListItem Value="-1">-- Selecione um mês --</asp:ListItem>
        <asp:ListItem Value="1">Janeiro</asp:ListItem>
        <asp:ListItem Value="2">Fevereiro</asp:ListItem>
        <asp:ListItem Value="3">Março</asp:ListItem>
        <asp:ListItem Value="4">Abril</asp:ListItem>
        <asp:ListItem Value="5">Maio</asp:ListItem>
        <asp:ListItem Value="6">Junho</asp:ListItem>
        <asp:ListItem Value="7">Julho</asp:ListItem>
        <asp:ListItem Value="8">Agosto</asp:ListItem>
        <asp:ListItem Value="9">Setembro</asp:ListItem>
        <asp:ListItem Value="10">Outubro</asp:ListItem>
        <asp:ListItem Value="11">Novembro</asp:ListItem>
        <asp:ListItem Value="12">Dezembro</asp:ListItem>
    </asp:DropDownList>
    <asp:CompareValidator ID="cpvComboFinal" runat="server" ErrorMessage="Mês final é obrigatório."
    ControlToValidate="ddlMesFim" Operator="NotEqual" ValueToCompare="-1"
    Display="Dynamic" Visible="false">*</asp:CompareValidator>
 </div>
</div>
