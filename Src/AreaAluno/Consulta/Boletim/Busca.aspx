<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAluno.Master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="AreaAluno.Consulta.Boletim.Busca" %>

<%@ Register Src="~/WebControls/AlunoBoletimEscolar/UCAlunoBoletimEscolar.ascx" TagPrefix="uc1" TagName="UCAlunoBoletimEscolar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    

    <asp:UpdatePanel ID="upnBusca" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
            <fieldset id="Fieldset1" runat="server">
                <legend>Boletim Online</legend>
                <uc1:UCAlunoBoletimEscolar runat="server" ID="UCAlunoBoletimEscolar" />
                <!--<div class="right">
                    <asp:Button ID="btnVoltar" runat="server" CausesValidation="False" Text="Voltar" PostBackUrl="~/Index.aspx" />
                </div>-->
            </fieldset>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

