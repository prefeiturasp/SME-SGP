<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Relatorios.IndicadorFrequencia.Busca" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="False" />
    <asp:ValidationSummary runat="server" />
    <asp:Panel ID="pnlBusca" runat="server" GroupingText="">
        <asp:Label AssociatedControlID="ddlAgrupamento" runat="server" 
            Text="<%$ Resources: Relatorios, IndicadorFrequencia.Busca.lblAgrupamento.Text %>" />
        <asp:DropDownList ID="ddlAgrupamento" runat="server">
            <asp:ListItem Text="<%$ Resources: Relatorios, IndicadorFrequencia.Busca.ddlAgrupamento.Selecione %>" Value="-1" Selected="True" />
            <asp:ListItem Text="<%$ Resources: Relatorios, IndicadorFrequencia.Busca.ddlAgrupamento.DRE %>" Value="1" />
            <asp:ListItem Text="<%$ Resources: Relatorios, IndicadorFrequencia.Busca.ddlAgrupamento.PeriodoCurso %>" Value="2" />
        </asp:DropDownList>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlAgrupamento" InitialValue="-1" Display="Dynamic" Text="*"
            ErrorMessage="<%$ Resources: Relatorios, IndicadorFrequencia.Busca.rfvAgrupamento.ErrorMessage %>" />
        <div class="right">
            <asp:Button ID="btnGerar" runat="server" OnClick="btnGerar_Click"
                Text="<%$ Resources: Relatorios, IndicadorFrequencia.Busca.btnGerar.Text %>"
                ToolTip="<%$ Resources: Relatorios, IndicadorFrequencia.Busca.btnGerar.Text %>" />
        </div>
    </asp:Panel>
</asp:Content>
