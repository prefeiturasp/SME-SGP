<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboCalendario"
    CodeBehind="UCComboCalendario.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Calendário escolar" AssociatedControlID="ddlComboCalendario"></asp:Label>
<asp:DropDownList ID="ddlComboCalendario" runat="server" AppendDataBoundItems="True"
    DataSourceID="odsDados" DataTextField="cal_ano_desc" DataValueField="cal_id"
    SkinID="text60C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
<asp:CompareValidator ID="cpvCombo" runat="server" ErrorMessage="Calendário escolar é obrigatório."
    ControlToValidate="ddlComboCalendario" Operator="NotEqual" ValueToCompare="-1"
    Display="Dynamic" Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_CalendarioAnual"
    SelectMethod="SelecionaCalendarioAnual" TypeName="MSTech.GestaoEscolar.BLL.ACA_CalendarioAnualBO"
    OnSelected="odsDados_Selected">
    <SelectParameters>
        <asp:Parameter Name="ent_id" DbType="Guid" />
        <asp:Parameter Name="appMinutosCacheLongo" DbType="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
