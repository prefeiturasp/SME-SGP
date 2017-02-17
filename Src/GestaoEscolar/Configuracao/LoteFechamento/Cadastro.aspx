<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.LoteFechamento.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/LoteFechamento/Busca.aspx" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional" EnableViewState="false">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup='<%=ValidationGroup %>'/>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset>
        <asp:Label ID="lblEscola" runat="server"></asp:Label><br />
        <asp:Label ID="lblTurma" runat="server"></asp:Label><br />
        <asp:Label ID="lblBimestre" runat="server"></asp:Label>
    </fieldset>
    <asp:Panel ID="pnlImportacao" runat="server" GroupingText='<%# RetornaLegendaPagina() %>'>
         <!-- Map de passos da importação -->
        <div class="map">
            <!-- Seleção de arquivo -->
            <asp:Label ID="lblSelecaoArquivo" runat="server" Text="Seleção do arquivo" CssClass="passo_atual"
                Style="z-index: 3;"></asp:Label>
            <!-- Análise do arquivo -->
            <asp:Label ID="lblAnaliseArquivo" runat="server" Text="Análise do arquivo" CssClass="passo"
                Style="z-index: 2;"></asp:Label>
            <!-- Importação do arquivo -->
            <asp:Label ID="lblImportacaoArquivo" runat="server" Text="Importação do arquivo" CssClass="passo"
                Style="z-index: 1;"></asp:Label>
        </div>
        <asp:Panel ID="pnlSelecaoArquivo" runat="server" GroupingText="Seleção do arquivo de importação">
            <div>
                <p class="msgInformacao">
                    Ao clicar em &quot;Avançar&quot;, será feita uma análise para validação dos registros
                    do arquivo selecionado.
                </p>
            </div>
            <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
            <asp:Label ID="lblArquivo" runat="server" Text="Arquivo (.csv) *" AssociatedControlID="fupArquivo"></asp:Label>
            <asp:FileUpload ID="fupArquivo" runat="server" ToolTip="Procurar arquivo" />
            <asp:RequiredFieldValidator ID="rfvArquivo" runat="server" ControlToValidate="fupArquivo" Display="Dynamic"
                ErrorMessage="Arquivo é obrigatório." ValidationGroup='<%=ValidationGroup %>' SetFocusOnError="True">*</asp:RequiredFieldValidator>
            <div class="right">
                <asp:Button ID="btnAnalisar" runat="server" OnClick="btnAnalisar_Click" Text="Avançar" ValidationGroup='<%=ValidationGroup %>' />
                <asp:Button ID="btnCancelar" runat="server" OnClick="btnCancelar_Click" Text="Cancelar" CausesValidation="false" />
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlAnaliseImportacaoArquivo" runat="server" GroupingText="Análise dos registro do arquivo">
            <div>
                <p id="msgImportacao" runat="server" class="msgInformacao">
                    Ao clicar em &quot;Avançar&quot;, será realizada a importação somente dos registros
                    processados com sucesso.
                </p>
            </div>
            <table style="font-size: 1.2em;">
                <tr>
                    <td>
                        <asp:Label ID="lblTituloSucesso" runat="server"></asp:Label>
                    </td>
                    <td style="padding-left: 20px; text-align: right;">
                        <asp:Label ID="lblSucesso" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTituloErro" runat="server"></asp:Label>
                    </td>
                    <td style="padding-left: 20px; text-align: right;">
                        <asp:Label ID="lblErro" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="font-weight: bold; border-top: solid 1px;">
                        <asp:Label ID="lblTituloTotal" runat="server"></asp:Label>
                    </td>
                    <td style="font-weight: bold; border-top: solid 1px; padding-left: 20px; text-align: right;">
                        <asp:Label ID="lblTotal" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <div class="right">
                <asp:Button ID="btnImportar" runat="server" Text="Avançar" CausesValidation="false"  OnClick="btnImportar_Click"/>
                <asp:Button ID="btnVoltar" runat="server" Text="Voltar" CausesValidation="false" OnClick="btnVoltar_Click" />
            </div><br />

            <asp:UpdatePanel ID="updArquivo" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:UCComboQtdePaginacao ID="UCComboQtdePaginacao" runat="server" />
                    <asp:GridView ID="grvArquivo" runat="server" AutoGenerateColumns="false" OnDataBinding="grvArquivo_DataBinding" 
                        AllowPaging="true" OnPageIndexChanging="grvArquivo_PageIndexChanging"
                        OnDataBound="grvArquivo_DataBound">
                        <Columns>
                            <asp:BoundField DataField="nomeEscola" HeaderText="Escola" />
                            <asp:BoundField DataField="codigoTurma" HeaderText="Turma" />
                            <asp:BoundField DataField="nomeAluno" HeaderText="Aluno" />
                            <asp:BoundField DataField="disciplina" HeaderText="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" />
                            <asp:BoundField DataField="docente" HeaderText="Docente" />
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Image ID="imgStatus" runat="server" Width="20px" ImageUrl='<%# RetornaImagemStatus(Eval("status").ToString()) %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Erros">
                                <ItemTemplate>
                                    <asp:Label ID="lblErro" runat="server" Text='<%# Bind("mensagem") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <uc3:UCTotalRegistros ID="UCTotalRegistros" runat="server" AssociatedGridViewID="grvArquivo" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
