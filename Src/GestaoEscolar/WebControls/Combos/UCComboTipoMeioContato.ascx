<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboTipoMeioContato" Codebehind="UCComboTipoMeioContato.ascx.cs" %>
<asp:Label ID="LabelTipoMeioContato" runat="server" Text="Tipo de contato *" AssociatedControlID="_ddlTipoMeioContato"></asp:Label>
<asp:DropDownList ID="_ddlTipoMeioContato" runat="server" AppendDataBoundItems="True" 
    DataSourceID="odsTipoMeioContato" DataTextField="tmc_nome" 
    DataValueField="tmc_id" 
    onselectedindexchanged="_ddlTipoMeioContato_SelectedIndexChanged">
</asp:DropDownList>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false" CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsTipoMeioContato" runat="server" 
    DataObjectTypeName="MSTech.CoreSSO.Entities.SYS_TipoMeioContato" 
    SelectMethod="GetSelect" 
    TypeName="MSTech.CoreSSO.BLL.SYS_TipoMeioContatoBO" 
    EnablePaging="false" MaximumRowsParameterName="pageSize" 
    SelectCountMethod="GetTotalRecords" 
    StartRowIndexParameterName="currentPage" 
    onselected="odsTipoMeioContato_Selected">   
</asp:ObjectDataSource>
