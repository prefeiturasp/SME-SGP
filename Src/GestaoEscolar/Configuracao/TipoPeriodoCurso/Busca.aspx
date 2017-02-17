<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_TipoPeriodoCurso_Busca" CodeBehind="Busca.aspx.cs" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoModalidadeEnsino.ascx" TagName="UCComboTipoModalidadeEnsino" 
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoNivelEnsino.ascx" TagName="UCComboTipoNivelEnsino" 
    TagPrefix="uc3" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="periodoCurso" />
    <%-- Pesquisa --%>
    <fieldset id="fdsEscolaPeriodoCurso" runat="server">
        <legend><asp:Label ID="lblLegendaBuscaPeriodoCurso" runat="server" ></asp:Label></legend>
        <uc5:uccamposobrigatorios ID="UCCamposObrigatorios1" runat="server" />
            <asp:UpdatePanel ID="uppPesquisa" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="divPesquisa" runat="server">
                        <uc3:UCComboTipoNivelEnsino runat="server" ID="UCComboTipoNivelEnsino" Obrigatorio="true" PermiteEditar="true"
                                ValidationGroup="periodoCurso" />
                        <uc2:UCComboTipoModalidadeEnsino runat="server" ID="UCComboTipoModalidadeEnsino" Obrigatorio="true" PermiteEditar="true"
                                ValidationGroup="periodoCurso" />
                    </div>
                </ContentTemplate> 
            </asp:UpdatePanel> 
            <div class="right">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                    CausesValidation="true" ValidationGroup="periodoCurso" />
                <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="btnLimparPesquisa_Click"
                    CausesValidation="False" />
            </div>
    </fieldset>
    <%-- Resultado da pesquisa --%>
            <fieldset id="fdsResultados" runat="server" visible="false">
                <legend>Resultados</legend>                        
                <asp:GridView ID="gvPeriodoCurso" runat="server" AutoGenerateColumns="False" DataSourceID="odsPeriodoCurso"
                    DataKeyNames="tcp_id, tcp_ordem, tne_id, tme_id, tcp_descricao, tcp_situacao" EmptyDataText="A pesquisa não encontrou resultados." 
                    OnDataBound="gvPeriodoCurso_DataBound" AllowSorting="False" OnRowDataBound="gvPeriodoCurso_RowDataBound" 
                    OnRowCommand="gvPeriodoCurso_RowCommand" style="margin-top: 0px">   
                  <Columns>
                    <asp:TemplateField HeaderText="Descrição">                                           
                        <ItemTemplate>
                            <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("tcp_descricao") %>'
                                PostBackUrl="~/Configuracao/TipoPeriodoCurso/Cadastro.aspx"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ordem">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("tcp_ordem") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:ImageButton ID="_btnSubir" runat="server" CausesValidation="false" CommandName="Subir"
                                Height="16" Width="16" ToolTip="Subir uma linha" />
                            <asp:ImageButton ID="_btnDescer" runat="server" CausesValidation="false" CommandName="Descer"
                                Height="16" Width="16" ToolTip="Descer uma linha" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                  </Columns>        
                </asp:GridView>
                <uc4:uctotalregistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="gvPeriodoCurso" />
                <asp:ObjectDataSource ID="odsPeriodoCurso" runat="server"
                    SelectMethod="SelectByPesquisa" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoCurriculoPeriodoBO"></asp:ObjectDataSource>
            </fieldset>   
</asp:Content>
