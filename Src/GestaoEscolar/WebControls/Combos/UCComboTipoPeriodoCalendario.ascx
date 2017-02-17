<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboTipoPeriodoCalendario" Codebehind="UCComboTipoPeriodoCalendario.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Período" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="tpc_nome" DataValueField="tpc_id" CssClass="text60C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Período é obrigatório."
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoPeriodoCalendario"
    SelectMethod="SelecionaTipoPeriodoCalendario" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoPeriodoCalendarioBO"
    OnSelected="odsDados_Selected">
    <SelectParameters>
        <asp:Parameter DbType="Int32" Name="AppMinutosCacheLongo" />
        <asp:Parameter DbType="Boolean" Name="removerRecesso" />
        <asp:Parameter DbType="Guid" Name="ent_id" />
    </SelectParameters>
</asp:ObjectDataSource>
