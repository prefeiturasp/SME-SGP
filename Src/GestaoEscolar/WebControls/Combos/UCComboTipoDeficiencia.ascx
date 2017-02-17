<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboTipoDeficiencia" Codebehind="UCComboTipoDeficiencia.ascx.cs" %>
<asp:Label ID="LabelTipoDeficiencia" runat="server" 
    Text="<%$ Resources:WebControls, Combos.UCComboTipoDeficiencia.LabelTipoDeficiencia.Text %>"
    AssociatedControlID="_ddlTipoDeficiencia"></asp:Label>
<asp:DropDownList ID="_ddlTipoDeficiencia" runat="server" DataSourceID="odsTipoDeficiencia"
    DataTextField="tde_nome" DataValueField="tde_id" OnSelectedIndexChanged="_ddlTipoDeficiencia_SelectedIndexChanged"
    SkinID="text30C">
</asp:DropDownList>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false" CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsTipoDeficiencia" runat="server" DataObjectTypeName="MSTech.CoreSSO.Entities.PES_TipoDeficiencia"
    SelectMethod="GetSelect" TypeName="MSTech.CoreSSO.BLL.PES_TipoDeficienciaBO"
    EnablePaging="false" onselected="odsTipoDeficiencia_Selected">
    <SelectParameters>
        <asp:Parameter Name="tde_id" DbType="Int32" Size="4" />
        <asp:Parameter Name="tde_nome" DbType="AnsiString" Size="100" />
        <asp:Parameter Name="tde_situacao" DbType="Byte" Size="1" />
        <asp:Parameter Name="paginado" Type="Boolean" DefaultValue="false" />
        <asp:Parameter Name="currentPage" Type="Int32" DefaultValue="1" />
        <asp:Parameter Name="pageSize" Type="Int32" DefaultValue="1" />
    </SelectParameters>
</asp:ObjectDataSource>
