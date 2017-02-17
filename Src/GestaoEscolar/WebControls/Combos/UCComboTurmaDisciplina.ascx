<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboTurmaDisciplina"
    CodeBehind="UCComboTurmaDisciplina.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataSourceID="odsDados"
    DataTextField="tud_nome" DataValueField="tud_id" SkinID="text30C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="<%$ Resources:UserControl, Combos.UCComboTurmaDisciplina.cpvCombo.ErrorMessage %>"
    ControlToValidate="ddlCombo" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false"
    CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" SelectMethod="GetSelectBy_tur_id"
    OnSelected="odsDados_Selected" DataObjectTypeName="MSTech.GestaoEscolar.Entities.TUR_TurmaDisciplina"
    TypeName="MSTech.GestaoEscolar.BLL.TUR_TurmaDisciplinaBO">
    <SelectParameters>
        <asp:Parameter Name="tur_id" DbType="Int64" />
        <asp:Parameter Name="ent_id" DbType="Guid" />
        <asp:Parameter Name="mostraFilhosRegencia" DbType="Boolean" />
        <asp:Parameter Name="mostraRegencia" DbType="Boolean" />
        <asp:Parameter Name="AppMinutosCacheLongo" DbType="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
