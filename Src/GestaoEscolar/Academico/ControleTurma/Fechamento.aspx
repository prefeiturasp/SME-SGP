<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Fechamento.aspx.cs" Inherits="GestaoEscolar.Academico.ControleTurma.Fechamento" %>
<%@ PreviousPageType VirtualPath="~/Academico/ControleTurma/Busca.aspx" %>

<%@ Register Src="~/WebControls/ControleTurma/UCControleTurma.ascx" TagName="UCControleTurma" TagPrefix="uc10" %>
<%@ Register Src="~/WebControls/NavegacaoTelaPeriodo/UCNavegacaoTelaPeriodo.ascx" TagName="UCNavegacaoTelaPeriodo" TagPrefix="uc13" %>
<%@ Register src="~/WebControls/ControleTurma/UCSelecaoDisciplinaCompartilhada.ascx" tagname="UCSelecaoDisciplinaCompartilhada" tagprefix="uc10" %>
<%@ Register Src="~/WebControls/Fechamento/UCFechamento.ascx" TagName="UCFechamento" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageName = "Fechamento.aspx";
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
    <fieldset>
        <uc10:UCControleTurma ID="UCControleTurma1" runat="server" />

        <div runat="server" id="divMessageTurmaAnterior"
            class="summaryMsgAnosAnteriores" style="<%$ Resources: Academico, ControleTurma.Busca.divMessageTurmaAnterior.Style %>">
            <asp:Label runat="server" ID="lblMessageTurmaAnterior" Text="<%$ Resources:Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Text %>"
                Style="<%$ Resources: Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Style %>"></asp:Label>
        </div>

        <!-- Botões de navegação -->
        <uc13:UCNavegacaoTelaPeriodo id="UCNavegacaoTelaPeriodo" runat="server" />

        <uc1:UCFechamento ID="UCFechamento" runat="server" />
    </fieldset>

    <%-- Disciplinas compartilhadas --%>
    <uc10:UCSelecaoDisciplinaCompartilhada ID="UCSelecaoDisciplinaCompartilhada1" runat="server"></uc10:UCSelecaoDisciplinaCompartilhada>
    <asp:HiddenField ID="hdnValorTurmas" runat="server" />
    
    <asp:HiddenField ID="hdnProcessarFilaFechamentoTela" runat="server" Value="false" />
</asp:Content>
