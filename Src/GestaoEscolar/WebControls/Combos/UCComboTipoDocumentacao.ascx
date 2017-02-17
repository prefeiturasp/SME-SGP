<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboTipoDocumento" Codebehind="UCComboTipoDocumentacao.ascx.cs" %>
<asp:Label ID="LabelTipoDocumentacao" runat="server" Text="Tipo de documento *" AssociatedControlID="_ddlTipoDocumentacao"></asp:Label>
<asp:DropDownList ID="_ddlTipoDocumentacao" runat="server" AppendDataBoundItems="True"
    DataSourceID="odsTipoDocumentacao" DataTextField="tdo_nome" DataValueField="tdo_id"
    OnSelectedIndexChanged="_ddlTipoDocumentacao_SelectedIndexChanged" SkinID="text30C">
</asp:DropDownList>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false" CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsTipoDocumentacao" runat="server" DataObjectTypeName="MSTech.CoreSSO.Entities.SYS_TipoDocumentacao"
    SelectMethod="GetSelect" TypeName="MSTech.CoreSSO.BLL.SYS_TipoDocumentacaoBO"
    EnablePaging="false" onselected="odsTipoDocumentacao_Selected"></asp:ObjectDataSource>
