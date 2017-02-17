<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCBuscaEscolaOrigem.ascx.cs"
    Inherits="GestaoEscolar.WebControls.EscolaOrigem.UCBuscaEscolaOrigem" %>
<asp:UpdatePanel ID="updBuscaEscolaOrigemDestino" runat="server">
    <ContentTemplate>
        <asp:Label ID="lblEscolaOrigemDestino" runat="server" Text="Escola" AssociatedControlID="txtEscolaOrigemDestino"
            EnableViewState="False"></asp:Label>
        <asp:TextBox ID="txtEscolaOrigemDestino" runat="server" Width="445px" MaxLength="200"
            Enabled="false"></asp:TextBox>
        <asp:ImageButton ID="btnEscolaOrigemDestino" runat="server" CausesValidation="false"
            SkinID="btPesquisar" OnClick="btnEscolaOrigemDestino_Click" />
        <asp:ImageButton ID="btnLimpar" runat="server" SkinID="btLimpar" CausesValidation="false"
            OnClick="btnLimpar_Click" Visible="false" />
    </ContentTemplate>
</asp:UpdatePanel>
