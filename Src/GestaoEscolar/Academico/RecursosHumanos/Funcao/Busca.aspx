<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_RecursosHumanos_Funcao_Busca" Title="Untitled Page" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="../../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsFuncao" runat="server">
        <legend>Consulta de funções</legend>
        <div id="_divPesquisa" runat="server">
            <asp:Label ID="_lblFuncao" runat="server" Text="Nome da função" AssociatedControlID="_txtFuncao"></asp:Label>
            <asp:TextBox ID="_txtFuncao" runat="server" SkinID="text60C" MaxLength="100"></asp:TextBox>
            <asp:Label ID="_lblCodigo" runat="server" Text="Código da função" AssociatedControlID="_txtCodigo"></asp:Label>
            <asp:TextBox ID="_txtCodigo" runat="server" SkinID="text10C" MaxLength="20"></asp:TextBox>
        </div>
        <div class="right">
            <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click" />
            <asp:Button ID="_btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click" />
            <asp:Button ID="_btnNovo" runat="server" Text="Incluir nova função" OnClick="_btnNovo_Click" /></div>
    </fieldset>
    <fieldset id="fdsResultados" runat="server">
        <legend>Resultados</legend>
        <uc2:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="_dgvFuncao" runat="server" AutoGenerateColumns="False" AllowPaging="True"
            DataKeyNames="fun_id" DataSourceID="_odsFuncao" EmptyDataText="A pesquisa não encontrou resultados."
            OnRowCommand="_dgvFuncao_RowCommand" OnRowDataBound="_dgvFuncao_RowDataBound"
            OnDataBound="_dgvFuncao_DataBound" AllowSorting="true" >
            <Columns>
                <asp:TemplateField HeaderText="Nome da função" SortExpression="fun_nome"  >
                    <ItemTemplate>
                        <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("fun_nome") %>'
                            PostBackUrl="~/Academico/RecursosHumanos/Funcao/Cadastro.aspx"></asp:LinkButton>
                        <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("fun_nome") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="fun_codigo" HeaderText="Código da função" SortExpression="fun_codigo" />
                <asp:BoundField DataField="fun_situacao" HeaderText="Bloqueado" SortExpression="fun_situacao">
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Excluir" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="_btnExcluir" SkinID="btExcluir" CommandName="Deletar" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_dgvFuncao" />
    </fieldset>
    <asp:ObjectDataSource ID="_odsFuncao" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.RHU_Funcao"
         SelectMethod="SelecionaFuncao" TypeName="MSTech.GestaoEscolar.BLL.RHU_FuncaoBO"></asp:ObjectDataSource>
</asp:Content>
