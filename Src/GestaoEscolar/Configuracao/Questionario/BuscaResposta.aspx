<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="BuscaResposta.aspx.cs" Inherits="GestaoEscolar.Configuracao.Questionario.BuscaResposta" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/Questionario/BuscaConteudo.aspx" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Resultado" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updResultado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset>
                <div id="divInformacao" style="width: 60%; float: left; clear: none;">
                    <asp:Label runat="server" ID="lblInfo" Visible="true"></asp:Label>
                    <br />
                </div>
                <div class="right">
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"
                        CausesValidation="false" OnClick="btnCancelar_Click" />
                </div>
            </fieldset>
            <fieldset id="fdsResultado" runat="server" visible="false">
                <legend>Consulta de respostas</legend>
                <%--<div id="divInformacao" style="width: 60%; float: left; clear: none;">
                    <asp:Label runat="server" ID="lblInfo" Visible="true"></asp:Label>
                    <br />
                </div>--%>
                <uc4:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao1_IndexChanged" />
                <br />
                <div align="left">
                    <asp:Button ID="btnNovo" runat="server" Text="Incluir nova resposta"
                        CausesValidation="False" PostBackUrl="~/Configuracao/Questionario/CadastroResposta.aspx" />
                    <%--<asp:Button ID="btnCancelar" runat="server" Text="Cancelar"
                        CausesValidation="false" OnClick="btnCancelar_Click" />--%>
                </div>
                <asp:GridView ID="grvResultado" runat="server" AllowPaging="true" AutoGenerateColumns="false"
                    BorderStyle="None" DataKeyNames="qtr_id, qtc_id, qtr_texto, qtr_permiteAdicionarTexto, qtr_peso, qtr_ordem"
                    DataSourceID="odsResultado" AllowCustomPaging="true"
                    EmptyDataText="A pesquisa não encontrou resultados." OnRowCommand="grvResultado_RowCommand"
                    OnRowDataBound="grvResultado_RowDataBound" AllowSorting="true" EnableModelValidation="true" OnDataBound="grvResultado_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Texto da resposta">
                            <ItemTemplate>
                                <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("qtr_texto") %>' CssClass="wrap400px" Visible="false"></asp:Label>
                                <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("qtr_texto") %>'
                                    PostBackUrl="~/Configuracao/Questionario/CadastroResposta.aspx" CssClass="wrap400px" Visible="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Peso">
                            <ItemTemplate>
                                <asp:Label ID="lblPeso" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Permite adicionar texto">
                            <ItemTemplate>
                                <asp:Label ID="lblPermiteAdicionarTexto" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="Ordem">
                            <ItemTemplate>
                                <asp:ImageButton ID="_btnSubir" runat="server" CausesValidation="false" CommandName="Subir"
                                    Height="16" Width="16" />
                                <asp:ImageButton ID="_btnDescer" runat="server" CausesValidation="false" CommandName="Descer"
                                    Height="16" Width="16" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Excluir">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Deletar" SkinID="btExcluir"
                                    CausesValidation="False" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <uc3:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvResultado" />
                <asp:ObjectDataSource ID="odsResultado" runat="server" SelectMethod="SelectByConteudoPaginado"
                    TypeName="MSTech.GestaoEscolar.BLL.CLS_QuestionarioRespostaBO" SelectCountMethod="GetTotalRecords"
                    DataObjectTypeName="MSTech.GestaoEscolar.Entities.CLS_QuestionarioResposta" EnablePaging="True"
                    OnSelecting="odsResultado_Selecting" MaximumRowsParameterName="pageSize" StartRowIndexParameterName="currentPage"></asp:ObjectDataSource>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
