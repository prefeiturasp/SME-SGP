<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboTipoEscolaridade" Codebehind="UCComboTipoEscolaridade.ascx.cs" %>
<asp:Label ID="LabelTipoEscolaridade" runat="server" Text="Escolaridade" AssociatedControlID="_ddlTipoEscolaridade"></asp:Label>
<asp:DropDownList ID="_ddlTipoEscolaridade" runat="server" AppendDataBoundItems="True"
    DataSourceID="odsTipoEscolaridade" DataTextField="tes_nome" DataValueField="tes_id"
    SkinID="text30C">
</asp:DropDownList>
<asp:Label ID="lblMessage" runat="server" Visible="false" Text="" EnableViewState="false" CssClass="msgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsTipoEscolaridade" runat="server" DataObjectTypeName="MSTech.CoreSSO.Entities.PES_TipoEscolaridade"
    SelectMethod="GetSelect" TypeName="MSTech.CoreSSO.BLL.PES_TipoEscolaridadeBO"
    EnablePaging="false" onselected="odsTipoEscolaridade_Selected"></asp:ObjectDataSource>
