<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboServico.ascx.cs"
    Inherits="GestaoEscolar.WebControls.Combos.UCComboServico" %>
<asp:Label ID="lblServico" runat="server" Text="Serviço *" AssociatedControlID="ddlServico"></asp:Label>
<asp:DropDownList SkinID="text60C" ID="ddlServico" runat="server" 
    AutoPostBack="true" AppendDataBoundItems="True"
DataTextField="ser_nome" DataValueField="ser_id" 
    onselectedindexchanged="ddlServico_SelectedIndexChanged">
</asp:DropDownList>
