<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Academico_Calendario_Anual_Busca" Codebehind="Busca.aspx.cs" %>

<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc2" %>   
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsCalendario" runat="server">
        <legend>Consulta de calendário escolar</legend>
        <div id="_divPesquisa" runat="server">
            <asp:Label ID="_lblAno" runat="server" Text="Ano letivo" AssociatedControlID="_txtAno"></asp:Label>
            <asp:TextBox ID="_txtAno" runat="server" CssClass="numeric" SkinID="Numerico" MaxLength="4"></asp:TextBox>
            <asp:Label ID="_lblDescricao" runat="server" Text="Descrição do calendário escolar"
                AssociatedControlID="_txtDescricao"></asp:Label>
            <asp:TextBox ID="_txtDescricao" runat="server" SkinID="text60C"></asp:TextBox>
        </div>
        <div class="right">
            <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click" />
            <asp:Button ID="_btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click" />
            <span class="area-botoes-bottom">
                <asp:Button ID="_btnNovo" runat="server" Text="Incluir novo calendário escolar" 
                    OnClick="_btnNovo_Click" />
            </span>
        </div>
    </fieldset>
    <div class="area-form">
        <fieldset id="fdsResultados" runat="server">
            <legend>Resultados</legend>
            <uc2:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
            <asp:GridView ID="_dgvCalendarioAnual" runat="server" DataKeyNames="cal_id" AutoGenerateColumns="False"
                DataSourceID="_odsCalendarioAnual" AllowPaging="True" OnRowDataBound="_dgvCalendarioAnual_RowDataBound"
                EmptyDataText="A pesquisa não encontrou resultados." OnRowCommand="_dgvCalendarioAnual_RowCommand"
                OnDataBound="_dgvCalendarioAnual_DataBound" AllowSorting="true" SkinID="GridResponsive">
                <Columns>
                    <asp:TemplateField HeaderText="Descrição do calendário escolar" SortExpression="cal_descricao">
                        <ItemTemplate>
                            <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("cal_descricao") %>'
                                PostBackUrl="../CalendarioAnual/Cadastro.aspx" CssClass="wrap600px"></asp:LinkButton>
                            <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("cal_descricao") %>' CssClass="wrap600px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="cal_ano" HeaderText="Ano" SortExpression="cal_ano">
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="cal_descricao" HeaderText="cal_descricao" SortExpression="cal_descricao"
                        Visible="False" />
                    <asp:BoundField DataField="cal_periodoLetivo" HeaderText="Período letivo" SortExpression="cal_dataInicio" ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Visualizar">
                        <ItemTemplate>
                            <asp:ImageButton ID="_btnVIsualizar" runat="server" SkinID="btDetalhar" PostBackUrl="~/Academico/CalendarioAnual/Visualizar.aspx"
                                CommandName="Edit" ToolTip="Visualizar o calendário escolar" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Limites para eventos no calendário">
                        <ItemTemplate>
                            <asp:ImageButton ID="_btnCadastrarLimites" runat="server" SkinID="btDetalhar" 
                                PostBackUrl="~/Academico/CalendarioAnual/CadastroLimites.aspx"
                                CommandName="Select" ToolTip="Limites para eventos no calendário" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Excluir">
                        <ItemTemplate>
                            <asp:ImageButton ID="_btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:GridView>
            <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_dgvCalendarioAnual" />
        </fieldset>
        <asp:ObjectDataSource ID="_odsCalendarioAnual" runat="server" EnablePaging="True"
            MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords" StartRowIndexParameterName="currentPage"
            DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_CalendarioAnual" SelectMethod="SelecionaCalendarioAnualPaginado"
            TypeName="MSTech.GestaoEscolar.BLL.ACA_CalendarioAnualBO" OnSelecting="_odsCalendarioAnual_Selecting">
        </asp:ObjectDataSource>
    </div>
</asp:Content>
