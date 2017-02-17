<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs"
    Inherits="GestaoEscolar.Configuracao.DiarioClasse.ConsultaEquipamentos.Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UCLoader ID="UCLoader1" runat="server" />
    <%--<asp:UpdatePanel ID="updMsgErro" runat="server">--%>
    <%--<ContentTemplate>--%>
    <asp:Label ID="lblMensagemErro" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Group1" />
    <%--</ContentTemplate>--%>
    <%--</asp:UpdatePanel>--%>

    <asp:UpdatePanel ID="updPequisa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset class="msgInfo">
                <legend>Consulta de Equipamentos</legend>
                <uc3:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" EnableViewState="False" />
                <uc2:UCComboUAEscola ID="uccUaEscola" runat="server" CarregarEscolaAutomatico="true"
                MostrarMessageSelecioneEscola="true" AsteriscoObg="true" ObrigatorioEscola="true"
                ObrigatorioUA="true" MostrarMessageSelecioneUA="true" ValidationGroup="Group1" />
                <br />
                <asp:Label ID="lblDescricao" runat="server" Text="Descrição" AssociatedControlID="txtDescricao"></asp:Label>
                <asp:TextBox ID="txtDescricao" runat="server" MaxLength="30" SkinID="text30C"></asp:TextBox>

                <div class="right">
                    <asp:Button ID="btn_pesquisar" runat="server" Text="Pesquisar"
                        OnClick="btnPesquisar_Click" ValidationGroup="Group1"/>
                </div>
            </fieldset>

            <fieldset id="fdsResultados" runat="server" visible="false">
                <legend>Resultados</legend>
                <uc4:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />

                <asp:GridView ID="grvConsultaEquipamentos" runat="server" DataKeyNames="equ_descricao" AutoGenerateColumns="False"
                    DataSourceID="odsConsultaEquipamentos" AllowPaging="True" BorderStyle="None"
                    EmptyDataText="A pesquisa não encontrou resultados.">
                    <Columns>
                        <asp:BoundField DataField="esc_nome" HeaderText="Escola" SortExpression="esc_nome" />
                        <asp:BoundField DataField="equ_descricao" HeaderText="Descrição" SortExpression="equ_descricao" />
                        <asp:BoundField DataField="equ_situacao" HeaderText="Situação" SortExpression="equ_situacao" />
                        <asp:BoundField DataField="equ_appVersion" HeaderText="Versão APP" SortExpression="equ_appVersion" />
                        <asp:BoundField DataField="equ_soVersion" HeaderText="Versão SO" SortExpression="equ_soVersion" />
                    </Columns>
                </asp:GridView>

                <uc5:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvConsultaEquipamentos" />

                <asp:ObjectDataSource ID="odsConsultaEquipamentos" runat="server"
                    SelectMethod="SelectByEscolaEquipamento"
                    TypeName="MSTech.GestaoEscolar.BLL.SYS_EquipamentoBO"></asp:ObjectDataSource>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



