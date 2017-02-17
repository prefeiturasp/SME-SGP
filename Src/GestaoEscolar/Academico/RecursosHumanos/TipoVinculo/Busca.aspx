<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Academico_RecursosHumanos_TipoVinculo_Busca"
    Title="Untitled Page" Codebehind="Busca.aspx.cs" %>

<%@ Register Src="../../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsTipoVinculo" runat="server">
        <legend>Consulta de tipos de vínculo</legend>
        <div id="_divPesquisa" runat="server">
            <asp:Label ID="_lblNome" runat="server" EnableViewState="False" Text="Nome do tipo de vínculo"
                AssociatedControlID="_txtNome"></asp:Label>
            <asp:TextBox ID="_txtNome" runat="server" SkinID="text60C"></asp:TextBox>
            <asp:Label ID="_lblDescricao" runat="server" EnableViewState="False" Text="Descrição do tipo de vínculo"
                AssociatedControlID="_txtDescricao"></asp:Label>
            <asp:TextBox ID="_txtDescricao" runat="server" SkinID="text60C"></asp:TextBox>
        </div>
        <div class="right">
            <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click" />
            <asp:Button ID="_btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click" />
        </div>
    </fieldset>
    <fieldset id="fdsResultados" runat="server">
        <legend>Resultados</legend>
        <uc2:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="_dgvTipoVinculo" runat="server" AutoGenerateColumns="False" DataKeyNames="tvi_id,ent_id"
            DataSourceID="_odsTipoVinculo" AllowPaging="True" EmptyDataText="A pesquisa não encontrou resultados."
            Style="margin-bottom: 0px" ondatabound="_dgvTipoVinculo_DataBound" AllowSorting="true">
            <Columns>
                <asp:TemplateField HeaderText="Nome do tipo de vínculo" SortExpression="tvi_nome">
                    <ItemTemplate>
                        <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("tvi_nome") %>'
                            PostBackUrl="~/Academico/RecursosHumanos/TipoVinculo/Cadastro.aspx"></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:BoundField DataField="tvi_horasSemanais" HeaderText="Horas semanais" SortExpression="tvi_horasSemanais">
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="tvi_minutosAlmoco" HeaderText="Minutos de almoço" SortExpression="tvi_minutosAlmoco">
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="tvi_situacao" HeaderText="Bloqueado" SortExpression="tvi_situacao">
                    <HeaderStyle HorizontalAlign="Center" CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_dgvTipoVinculo" />
    </fieldset>
    <asp:ObjectDataSource ID="_odsTipoVinculo" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.RHU_TipoVinculo"
        OnSelecting="_odsTipoVinculo_Selecting" SelectMethod="SelecionaTipoVinculo"
          TypeName="MSTech.GestaoEscolar.BLL.RHU_TipoVinculoBO">
    </asp:ObjectDataSource>
</asp:Content>
