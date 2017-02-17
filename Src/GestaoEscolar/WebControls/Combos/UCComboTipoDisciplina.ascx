<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboTipoDisciplina" CodeBehind="UCComboTipoDisciplina.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:UserControl, Combos.UCComboTipoDisciplina.lblTitulo.Text %>" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="tds_nome" DataValueField="tds_id" CssClass="text60C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="<%$ Resources:UserControl, Combos.UCComboTipoDisciplina.cpvCombo.ErrorMessage %>"
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoDisciplina"
    SelectMethod="SelecionaTipoDisciplina" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoDisciplinaBO"
    OnSelected="odsDados_Selected">
    <SelectParameters>
        <asp:Parameter Name="ent_id" DbType="Guid" />
        <asp:Parameter Name="AppMinutosCacheLongo" DbType="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
