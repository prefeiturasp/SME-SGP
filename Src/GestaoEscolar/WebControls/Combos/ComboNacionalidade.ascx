<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ComboNacionalidade.ascx.cs"
    Inherits="GestaoEscolar.WebControls.Combos.ComboNacionalidade" %>
<asp:Label ID="lblTitulo" runat="server" Text="Nacionalidade" EnableViewState="false"
    AssociatedControlID="ddlCombo"></asp:Label>
<asp:DropDownList ID="ddlCombo" runat="server" AppendDataBoundItems="True" DataTextField="dataTextField"
    DataValueField="dataValueField" SkinID="text30C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged">
</asp:DropDownList>
