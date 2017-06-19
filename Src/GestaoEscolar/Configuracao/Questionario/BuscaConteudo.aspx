<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="BuscaConteudo.aspx.cs" Inherits="GestaoEscolar.Configuracao.Questionario.BuscaConteudo" %>
<%@ PreviousPageType VirtualPath="~/Configuracao/Questionario/BuscaQuestionario.aspx" %>
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
            <fieldset id="fdsResultados" runat="server" visible="false">
                <legend>Consulta de conteúdos</legend>
                <uc4:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao1_IndexChanged" />
                <br />
                <div align="left">
                    <asp:Button ID="btnNovo" runat="server" Text="Incluir novo conteúdo" CommandName="Edit"
                        CausesValidation="False" PostBackUrl="~/Configuracao/Questionario/CadastroConteudo.aspx"/>
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                        CausesValidation="false" PostBackUrl="~/Configuracao/Questionario/BuscaQuestionario.aspx" />
                </div>
                <asp:GridView ID="grvResultado" runat="server" AllowPaging="true" AutoGenerateColumns="false"
                    BorderStyle="None" DataKeyNames="qst_id, qtc_id, qtc_tipo, qtc_tipoResposta" DataSourceID="odsResultado" AllowCustomPaging="true"
                    EmptyDataText="A pesquisa não encontrou resultados." OnRowCommand="grvResultado_RowCommand"
                    OnRowDataBound="grvResultado_RowDataBound" AllowSorting="true" EnableModelValidation="true" OnDataBound="grvResultado_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Texto" SortExpression="qtc_texto">
                            <ItemTemplate>
                                <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("qtc_texto") %>' CssClass="wrap400px"></asp:Label>
                                <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("qtc_texto") %>'
                                    PostBackUrl="~/Configuracao/Questionario/CadastroConteudo.aspx" CssClass="wrap400px"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo de conteúdo">
                            <ItemTemplate>
                                <asp:Label ID="lblTipoConteudo" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo de resposta">
                            <ItemTemplate>
                                <asp:Label ID="lblTipoResposta" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Incluir perguntas">
                            <ItemTemplate>
                                <asp:Button ID="btnIncluirRespostas" runat="server" Text="Incluir respostas"
                                    CausesValidation="False" PostBackUrl="~/Configuracao/Questionario/BuscaResposta.aspx" CommandName="Edit"/>
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" />
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
                <asp:ObjectDataSource ID="odsResultado" runat="server" SelectMethod="SelectByQuestionario" TypeName="MSTech.GestaoEscolar.BLL.CLS_QuestionarioConteudoBO"       
            SelectCountMethod="GetTotalRecords" ></asp:ObjectDataSource>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
