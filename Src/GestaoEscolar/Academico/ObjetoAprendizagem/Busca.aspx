<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Academico.ObjetoAprendizagem.Busca" %>

<%@ PreviousPageType VirtualPath="~/Academico/ObjetoAprendizagem/BuscaDisciplina.aspx" %>

<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc4" %>

<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc5" %>

<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="_updDadosBasicos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="fds" runat="server">
                <legend>Consulta de objetos de aprendizagem</legend>
                <div id="_divPesquisa" runat="server">
                    <asp:Label ID="_lblDisciplina" runat="server" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" AssociatedControlID="txtDisciplina"></asp:Label>
                    <asp:TextBox ID="txtDisciplina" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <div class="right">
                    <asp:Button ID="_btnNovo" runat="server" Text="Incluir novo objeto de aprendizagem" PostBackUrl="~/Academico/ObjetoAprendizagem/Cadastro.aspx" />
                    <asp:Button ID="_btnCancelar" runat="server" Text="Voltar" OnClick="_btnCancelar_Click" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset id="fdsResultados" runat="server">
        <legend>Resultados</legend>
        <asp:UpdatePanel ID="upd" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <uc5:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao1_IndexChanged" />
                <asp:GridView ID="_grvObjetoAprendizagem" runat="server" DataSourceID="_odsObjeto" AutoGenerateColumns="False"
                    AllowPaging="True" DataKeyNames="oap_id" HorizontalAlign="Center"
                    OnRowCommand="_grvObjetoAprendizagem_RowCommand" OnRowDataBound="_grvObjetoAprendizagem_RowDataBound"
                    OnDataBound="_grvObjetoAprendizagem_DataBound" AllowSorting="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Descrição">
                            <ItemTemplate>
                                <asp:LinkButton ID="_btnSelecionar" runat="server" CommandName="Edit" Text='<%# Bind("oap_descricao") %>'
                                    PostBackUrl="~/Academico/ObjetoAprendizagem/Cadastro.aspx"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ciclos" HeaderText="Tipos de ciclo" />
                        <asp:BoundField DataField="oap_situacao" HeaderText="Situação" />
                        <asp:TemplateField HeaderText="Excluir">
                            <ItemTemplate>
                                <asp:ImageButton ID="_btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:GridView>
                <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_grvObjetoAprendizagem" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
    <asp:ObjectDataSource ID="_odsObjeto" runat="server" SelectMethod="SelectBy_TipoDisciplina"
        TypeName="MSTech.GestaoEscolar.BLL.ACA_ObjetoAprendizagemTipoCicloBO"
        DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_ObjetoAprendizagem"></asp:ObjectDataSource>
</asp:Content>
