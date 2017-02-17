<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Configuracao.Recomendacoes.Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Recomendacoes" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updResultado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="fdsResultados" runat="server" visible="false">
                <legend>Listagem de recomendações a alunos e resposáveis</legend>
                <div class="left">
                    <asp:Button ID="btnNovo" runat="server" Text="Incluir nova recomendação" OnClick="_btnNovo_Click"
                        CausesValidation="False" />
                </div>
                <div id="divResultado" runat="server">
                    <uc4:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
                    <asp:GridView ID="grvRecomendacoes" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        BorderStyle="None" DataKeyNames="rar_id" DataSourceID="odsRecomendacoes"
                        EmptyDataText="A pesquisa não encontrou resultados." OnRowCommand="_grvRecomendacoes_RowCommand"
                        OnRowDataBound="_grvRecomendacoes_RowDataBound" OnDataBound="_grvRecomendacoes_DataBound" AllowSorting="True" EnableModelValidation="True">
                        <Columns>
                            <asp:TemplateField HeaderText="Recomendação" SortExpression="rar_id">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" PostBackUrl="Cadastro.aspx"
                                        CausesValidation="False" Text='<%# Bind("rar_descricao") %>' CssClass="wrap400px"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="rar_tipo" HeaderText="A quem se destina" SortExpression="rar_tipo" DataFormatString="{0:d}">
                                <HeaderStyle CssClass="thLeft" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Excluir">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Deletar" SkinID="btExcluir"
                                        CausesValidation="False" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle CssClass="center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <uc3:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvRecomendacoes" />
                    <asp:ObjectDataSource ID="odsRecomendacoes" runat="server" OldValuesParameterFormatString="original_{0}"
                        SelectMethod="SelecionaAtivos" TypeName="MSTech.GestaoEscolar.BLL.ACA_RecomendacaoAlunoResponsavelBO"></asp:ObjectDataSource>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
