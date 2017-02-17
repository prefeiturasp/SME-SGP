<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Combos_UCComboUnidadeEscola"
    CodeBehind="UCComboUnidadeEscola.ascx.cs" %>
<asp:Label ID="LabelEscola" runat="server" Text="Escola" AssociatedControlID="_ddlUnidadeEscola"></asp:Label>
<asp:DropDownList ID="_ddlUnidadeEscola" runat="server" AppendDataBoundItems="True"
    DataSourceID="odsUnidadeEscola" DataTextField="esc_uni_nome" DataValueField="esc_uni_id"
    CssClass="text60C" OnSelectedIndexChanged="_ddlUnidadeEscola_SelectedIndexChanged">
</asp:DropDownList>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
    SkinID="SkinmsgErroCombo"></asp:Label>
<asp:ObjectDataSource ID="odsUnidadeEscola" runat="server" SelectMethod="GetSelect"
    TypeName="MSTech.GestaoEscolar.BLL.ESC_UnidadeEscolaBO" EnablePaging="false"
    OnSelecting="odsUnidadeEscola_Selecting"></asp:ObjectDataSource>
