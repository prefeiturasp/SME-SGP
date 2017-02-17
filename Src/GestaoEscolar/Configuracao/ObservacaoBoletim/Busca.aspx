<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Configuracao.ObservacaoBoletim.Busca" %>

<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc4" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset id="fdsEscola" runat="server">
        <legend>Consulta de observações do boletim</legend>
        <asp:UpdatePanel ID="updResultado" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc4:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
                <asp:GridView ID="grvObservacoes" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    BorderStyle="None" DataKeyNames="obb_id" DataSourceID="odsObservacoes"
                    EmptyDataText="A pesquisa não encontrou resultados." OnRowCommand="grvObservacoes_RowCommand"
                    OnRowDataBound="grvObservacoes_RowDataBound" OnDataBound="grvObservacoes_DataBound" AllowSorting="True">
                    <Columns>
                        <asp:TemplateField HeaderText="Nome" SortExpression="obb_nome">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" PostBackUrl="Cadastro.aspx"
                                    CausesValidation="False" Text='<%# Bind("obb_nome") %>' CssClass="wrap400px"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("obb_nome") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="obb_descricao" HeaderText="Descrição" SortExpression="obb_descricao">
                            <HeaderStyle CssClass="thLeft" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="obb_tipoObservacao" HeaderText="Tipo de observação" SortExpression="obb_tipoObservacao">
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
                <div class="right">
                    <asp:Button ID="btnNovo" runat="server" Text="Incluir nova observação" OnClick="btnNovo_Click"
                        CausesValidation="False" />
                </div>
                <uc3:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvAvisos" />
                <asp:ObjectDataSource ID="odsObservacoes" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="SelectAtivos" TypeName="MSTech.GestaoEscolar.BLL.CFG_ObservacaoBoletimBO"></asp:ObjectDataSource>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>

    </fieldset>
</asp:Content>
