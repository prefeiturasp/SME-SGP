<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Configuracao.RelatorioAtendimento.Busca" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc1" %>   

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsFiltro" runat="server">
        <legend>Consulta de relatórios de atendimento</legend>
        <div id="divPesquisa" runat="server">
            <asp:Label ID="lbl" runat="server" Text="Tipo de relatório" AssociatedControlID="ddlTipoRelatorio"></asp:Label>
            <asp:DropDownList ID="ddlTipoRelatorio" runat="server" SkinID="text30C">
                <asp:ListItem Text="-- Selecione um tipo de relatório --" Value="0"></asp:ListItem>
                <asp:ListItem Text="AEE" Value="1"></asp:ListItem>
                <asp:ListItem Text="NAAPA" Value="2"></asp:ListItem>
                <asp:ListItem Text="Recuperação Paralela" Value="3"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" />
            <span class="area-botoes-bottom">
                <asp:Button ID="btnNovo" runat="server" Text="Incluir novo relatório de atendimento" OnClick="btnNovo_Click" />
            </span>
        </div>
    </fieldset>
    <fieldset id="fdsResultados" runat="server">
        <legend>Listagem de relatórios de atendimento</legend>
        <div>
            <uc1:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao1_IndexChanged" />
            <asp:GridView ID="grvDados" runat="server" AutoGenerateColumns="False" DataKeyNames="rea_id"
                OnRowCommand="grvDados_RowCommand" OnRowDataBound="grvDados_RowDataBound"
                DataSourceID="odsDados" AllowPaging="True" EmptyDataText="Não foram encontrados relatórios cadastrados." 
                OnDataBound="grvDados_DataBound"
                SkinID="GridResponsive">
                <Columns>
                    <asp:TemplateField HeaderText="Título do relatório">
                        <ItemTemplate>
                            <asp:LinkButton ID="_btnAlterar" runat="server" Text='<%# Eval("rea_titulo") %>' 
                                PostBackUrl="~/Configuracao/RelatorioAtendimento/Cadastro.aspx"
                                CommandName="Edit">
                            </asp:LinkButton>
                            <asp:Label ID="_lblAlterar" runat="server" Text='<%# Eval("rea_titulo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Tipo de relatório" DataField="Tipo" />
                    <asp:TemplateField HeaderText="Excluir" HeaderStyle-HorizontalAlign="Center" 
                        ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:ImageButton ID="_btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" ToolTip="Excluir relatório" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <uc2:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvDados" />
            <asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.CLS_RelatorioAtendimento"
                SelectMethod="PesquisaRelatorioPorTipo" TypeName="MSTech.GestaoEscolar.BLL.CLS_RelatorioAtendimentoBO"
                MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords" StartRowIndexParameterName="currentPage"
                EnablePaging="True"
                OnSelecting="odsDados_Selecting">
            </asp:ObjectDataSource>
        </div>
    </fieldset>
</asp:Content>
