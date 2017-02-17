<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboPeriodoCalendario"
    CodeBehind="UCComboPeriodoCalendario.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Período" 
    AssociatedControlID="ddlComboPeriodoCalendario"/>
<asp:DropDownList ID="ddlComboPeriodoCalendario" runat="server" AppendDataBoundItems="True" 
    DataTextField="tpc_nome" DataValueField="tpc_cap_id" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged"
    SkinID="text60C" >
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Período é obrigatório."
    ControlToValidate="ddlComboPeriodoCalendario" Operator="NotEqual" ValueToCompare="-1;-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
