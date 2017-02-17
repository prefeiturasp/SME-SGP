<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboTurmaDisciplinaRelacionada.ascx.cs" Inherits="WebControls_Combos_UCComboTurmaDisciplinaRelacionada" %>

<asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:UserControl, Combos.UCComboTurmaDisciplinaRelacionada.lblTitulo.Text %>" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="tud_nome" DataValueField="tud_id" SkinID="text30C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="<%$ Resources:UserControl, Combos.UCComboTurmaDisciplinaRelacionada.cpvCombo.ErrorMessage %>"
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" SelectMethod="SelectRelacionadaVigenteBy_DisciplinaCompartilhada"
    OnSelected="odsDados_Selected" DataObjectTypeName="MSTech.GestaoEscolar.Entities.TUR_TurmaDisciplina"
    TypeName="MSTech.GestaoEscolar.BLL.TUR_TurmaDisciplinaBO">
    <SelectParameters>
        <asp:Parameter Name="tud_id" DbType="Int64" />
        <asp:Parameter Name="AppMinutosCacheLongo" DbType="Int32" />
        <asp:Parameter Name="retornarComponentesRegencia" DbType="Boolean" />
        <asp:Parameter Name="doc_id" DbType="Int64" />
    </SelectParameters>
</asp:ObjectDataSource>
