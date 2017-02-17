<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Combos_UCComboConfiguracao" Codebehind="UCComboConfiguracao.ascx.cs" %>
<asp:Label ID="LabelConfiguracao" runat="server" Text="Processo de matrícula" AssociatedControlID="_ddlConfiguracao"></asp:Label>
<asp:DropDownList ID="_ddlConfiguracao" runat="server" AppendDataBoundItems="True"
    DataSourceID="odsConfiguracao" DataTextField="cfg_nome" DataValueField="cfg_id"
    OnSelectedIndexChanged="_ddlConfiguracao_SelectedIndexChanged" 
    SkinID="text60C">
</asp:DropDownList>
<asp:Label ID="lblMessage" runat="server" Text="" visible="false" EnableViewState="false" SkinID="SkinMsgErroCombo" ></asp:Label>
<asp:ObjectDataSource ID="odsConfiguracao" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.MTR_Configuracao"
    SelectMethod="GetSelectBy_Situacao_evt_id_Vigente" TypeName="MSTech.GestaoEscolar.BLL.MTR_ConfiguracaoBO"
    EnablePaging="false" OnSelecting="odsConfiguracao_Selecting"></asp:ObjectDataSource>
