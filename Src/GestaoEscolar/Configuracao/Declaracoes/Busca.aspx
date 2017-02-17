<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Configuracao.Declaracoes.Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="Declaracoes" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="_updDeclaracoes" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="fdsDeclaracoes" runat="server">
                <legend>Declarações</legend>
                <uc2:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
                <asp:GridView ID="grvDeclaracoes" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    BorderStyle="None" DataKeyNames="rda_id, rlt_id, pda_id" DataSourceID="OdsDeclaracoes"
                    EmptyDataText="A pesquisa não encontrou resultados."
                    OnDataBound="grvDeclaracoes_DataBound" AllowSorting="True">
                    <Columns>
                        <asp:TemplateField HeaderText="Tipo de declaração" SortExpression="rda_nomeDocumento">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" PostBackUrl="Cadastro.aspx"
                                    CausesValidation="False" Text='<%# Bind("rda_nomeDocumento") %>' CssClass="wrap400px">
                                </asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle CssClass="thLeft" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Situacao" HeaderText="Situação" SortExpression="Situacao">
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
                <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvDeclaracoes"
                    class="clTotalReg" />
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="OdsDeclaracoes" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="SelecionaDeclaracoesHTML" TypeName="MSTech.GestaoEscolar.BLL.CFG_RelatorioDocumentoAlunoBO">
    </asp:ObjectDataSource>
</asp:Content>
