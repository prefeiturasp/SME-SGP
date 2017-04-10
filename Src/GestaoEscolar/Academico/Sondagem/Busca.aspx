<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Academico.Sondagem.Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc2" %>   
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsSondagem" runat="server">
        <legend><asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Academico, Sondagem.Busca.lblLegend.Text %>" /></legend>
        <div id="divPesquisa" runat="server">
            <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:Academico, Sondagem.Busca.lblTitulo.Text %>" AssociatedControlID="txtTitulo"></asp:Label>
            <asp:TextBox ID="txtTitulo" runat="server" SkinID="text60C" MaxLength="200"></asp:TextBox>
        </div>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="<%$ Resources:Academico, Sondagem.Busca.btnPesquisar.Text %>" OnClick="btnPesquisar_Click" />
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="<%$ Resources:Academico, Sondagem.Busca.btnLimparPesquisa.Text %>" OnClick="btnLimparPesquisa_Click" />
            <span class="area-botoes-bottom">
                <asp:Button ID="btnNovo" runat="server" Text="<%$ Resources:Academico, Sondagem.Busca.btnNovo.Text %>" 
                    OnClick="btnNovo_Click" />
            </span>
        </div>
    </fieldset>
    <div class="area-form">
        <fieldset id="fdsResultados" runat="server">
            <legend><asp:Label runat="server" ID="lblLegendResultados" Text="<%$ Resources:Academico, Sondagem.Busca.lblLegendResultados.Text %>" /></legend>
            <uc2:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
            <asp:GridView ID="dgvSondagem" runat="server" DataKeyNames="snd_id" AutoGenerateColumns="False"
                DataSourceID="odsSondagem" AllowPaging="True" OnRowDataBound="dgvSondagem_RowDataBound"
                EmptyDataText="<%$ Resources:Academico, Sondagem.Busca.dgvSondagem.EmptyDataText %>" OnRowCommand="dgvSondagem_RowCommand"
                OnDataBound="dgvSondagem_DataBound" AllowSorting="true" SkinID="GridResponsive">
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:Academico, Sondagem.Busca.dgvSondagem.HeaderTitulo %>" SortExpression="snd_titulo">
                        <ItemTemplate>
                            <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("snd_titulo") %>'
                                PostBackUrl="~/Academico/Sondagem/Cadastro.aspx" CssClass="wrap600px"></asp:LinkButton>
                            <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("snd_titulo") %>' CssClass="wrap600px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Academico, Sondagem.Busca.dgvSondagem.HeaderAgendamento %>" Visible="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnCadastrarAgendamento" runat="server" SkinID="btDetalhar" 
                                PostBackUrl="~/Academico/Sondagem/Agendamento.aspx" CommandName="Select" 
                                ToolTip="<%$ Resources:Academico, Sondagem.Busca.dgvSondagem.btnCadastrarAgendamento.ToolTip %>" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Academico, Sondagem.Busca.dgvSondagem.HeaderExcluir %>">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar"
                                ToolTip="<%$ Resources:Academico, Sondagem.Busca.dgvSondagem.btExcluir.ToolTip %>" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:GridView>
            <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="dgvSondagem" />
        </fieldset>
        <asp:ObjectDataSource ID="odsSondagem" runat="server" EnablePaging="True"
            MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords" StartRowIndexParameterName="currentPage"
            DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_Sondagem" SelectMethod="SelecionaSondagensPaginado"
            TypeName="MSTech.GestaoEscolar.BLL.ACA_SondagemBO" OnSelecting="odsSondagem_Selecting">
        </asp:ObjectDataSource>
    </div>
</asp:Content>
