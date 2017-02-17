<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboTipoPeriodoCalendario.ascx.cs" Inherits="AreaAluno.WebControls.Combos.UCComboTipoPeriodoCalendario" %>

<asp:Label ID="lblTitulo" runat="server" Text="Tipo período calendário" AssociatedControlID="ddlTipoPeriodoCalendario"></asp:Label>
<asp:DropDownList ID="ddlTipoPeriodoCalendario" runat="server" AppendDataBoundItems="True" DataTextField="cap_descricao"
    DataValueField="tpc_id" CssClass="text30C">
</asp:DropDownList>
<asp:CompareValidator ID="cpvTipoPeriodoCalendario" runat="server" ErrorMessage="Bimestre é obrigatório."
    ControlToValidate="ddlTipoPeriodoCalendario" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>