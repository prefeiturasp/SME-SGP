<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboCurriculoPeriodo"
    CodeBehind="UCComboCurriculoPeriodo.ascx.cs" %>
<asp:Label ID="lblTitulo" runat="server" Text="Período" AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" DataSourceID="odsDados" DataTextField="crp_descricao"
    DataValueField="cur_id_crr_id_crp_id" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged"
    SkinID="text60C">
</asp:DropDownList>
<asp:Label ID="lblFormatoPeriodo" runat="server" Text="(Ano/Série)" Visible="false"></asp:Label>
<asp:CompareValidator ID="cpvCombo" runat="server" ControlToValidate="ddlCombo" Display="Dynamic"
    ErrorMessage="Período é obrigatório." Operator="NotEqual" ValueToCompare="-1;-1;-1"
    Visible="false">*</asp:CompareValidator>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinMsgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_CurriculoPeriodo"
    SelectMethod="GetSelect" TypeName="MSTech.GestaoEscolar.BLL.ACA_CurriculoPeriodoBO"
    EnablePaging="false" OnSelecting="odsDados_Selecting">
    <SelectParameters>
        <asp:Parameter Name="cur_id" DbType="Int32" Size="4" />
        <asp:Parameter Name="crr_id" DbType="Int32" Size="4" />
        <asp:Parameter Name="ent_id" DbType="Guid" />
        <asp:Parameter Name="appMinutosCacheLongo" DbType="Int32" Size="4" />
    </SelectParameters>
</asp:ObjectDataSource>
