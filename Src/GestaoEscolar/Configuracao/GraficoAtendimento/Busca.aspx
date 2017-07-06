<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Configuracao.GraficoAtendimento.Busca" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboRelatorioAtendimento.ascx" TagPrefix="uc1" TagName="UCComboRelatorioAtendimento" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsFiltro" runat="server">
        <legend>Consulta de gráficos de atendimento</legend>
        <div id="divPesquisa" runat="server">
            <asp:Label ID="lbl" runat="server" Text="Tipo de relatório" AssociatedControlID="ddlTipoRelatorio"></asp:Label>
            <asp:DropDownList ID="ddlTipoRelatorio" runat="server" SkinID="text30C" OnSelectedIndexChanged="ddlCombo_SelectedIndexChanged" AutoPostBack="True">
                <asp:ListItem Text="-- Selecione um tipo de relatório --" Value="0"></asp:ListItem>
                <asp:ListItem Text="AEE" Value="1"></asp:ListItem>
                <asp:ListItem Text="NAAPA" Value="2"></asp:ListItem>
                <asp:ListItem Text="Recuperação Paralela" Value="3"></asp:ListItem>

            </asp:DropDownList><uc1:UCComboRelatorioAtendimento runat="server" ID="UCComboRelatorioAtendimento" />
            <br />
            <br />
            <asp:Label ID="lblTitulo" runat="server" Text="Título do gráfico" />
            <br/>
            <asp:TextBox ID="txtTitulo" runat="server" SkinID="text60C" MaxLength="200"></asp:TextBox>

        </div>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" />
            <span class="area-botoes-bottom">
                <asp:Button ID="btnNovo" runat="server" Text="Incluir novo gráfico de atendimento" OnClick="btnNovo_Click" />
            </span>
        </div>
    </fieldset>
    <fieldset id="fdsResultados" runat="server">
        <legend>Listagem de gráficos de atendimento</legend>
        <div>
            <uc1:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao1_IndexChanged" />
            <asp:GridView ID="grvDados" runat="server" AutoGenerateColumns="False" DataKeyNames="gra_id"
                OnRowCommand="grvDados_RowCommand" OnRowDataBound="grvDados_RowDataBound"
                DataSourceID="odsDados" AllowPaging="True" EmptyDataText="Não foram encontrados gráficos cadastrados." 
                OnDataBound="grvDados_DataBound" OnPageIndexChanged="grvDados_PageIndexChanged"
                SkinID="GridResponsive">
                <Columns>
                    <asp:TemplateField HeaderText="Título do gráfico">
                        <ItemTemplate>
                            <asp:LinkButton ID="_btnAlterar" runat="server" Text='<%# Eval("gra_titulo") %>' 
                                PostBackUrl="~/Configuracao/GraficoAtendimento/Cadastro.aspx"
                                CommandName="Edit">
                            </asp:LinkButton>
                            <asp:Label ID="_lblAlterar" runat="server" Text='<%# Eval("gra_titulo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Relatório" DataField="rea_titulo" />
                    <asp:TemplateField HeaderText="Excluir" HeaderStyle-HorizontalAlign="Center" 
                        ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:ImageButton ID="_btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" ToolTip="Excluir gráfico" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <uc2:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvDados" />
            <asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.REL_GraficoAtendimento"
                SelectMethod="PesquisaGraficoPorRelatorio" TypeName="MSTech.GestaoEscolar.BLL.REL_GraficoAtendimentoBO"
                MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords" StartRowIndexParameterName="currentPage"
                EnablePaging="True"
                OnSelecting="odsDados_Selecting">
            </asp:ObjectDataSource>
        </div>
    </fieldset>
</asp:Content>
