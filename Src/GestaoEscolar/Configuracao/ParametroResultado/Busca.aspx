<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Configuracao.ParametroResultado.Busca" %>


<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagPrefix="uc3" TagName="UCCCursoCurriculo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Resultado" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset id="fdsResultado" runat="server">
        <legend>Consulta de parâmetros de resultado</legend>
        <asp:UpdatePanel ID="updPesquisa" runat="server">
            <ContentTemplate>
                <div id="divPesquisa" runat="server">
                    <uc3:UCCCursoCurriculo runat="server" ID="UCCCursoCurriculo" MostrarMensagemSelecione="true" PermiteEditar="true"
                        Obrigatorio="true" ValidationGroup="Resultado" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click"
                ValidationGroup="Resultado" />
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click"
                CausesValidation="False" />
            <asp:Button ID="btnNovo" runat="server" Text="Incluir novo parâmetro de resultado" OnClick="_btnNovo_Click"
                CausesValidation="False" />
        </div>
    </fieldset>

    <asp:UpdatePanel ID="updResultado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="fdsResultados" runat="server" visible="false">
                <legend>Resultados</legend>
                <uc4:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
                <asp:GridView ID="grvResultado" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    BorderStyle="None" DataKeyNames="tpr_id" DataSourceID="odsResultado"
                    EmptyDataText="A pesquisa não encontrou resultados." OnRowCommand="grvResultado_RowCommand"
                    OnRowDataBound="grvResultado_RowDataBound" AllowSorting="True" EnableModelValidation="True" OnDataBound="grvResultado_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Tipo de resultado" SortExpression="tipoResultado">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" PostBackUrl="Cadastro.aspx"
                                    CausesValidation="False" Text='<%# Bind("tipoResultado") %>' CssClass="wrap400px"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("tipoResultado") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="tpr_nomenclatura" HeaderText="Nomenclatura" SortExpression="tpr_nomenclatura" DataFormatString="{0:d}">
                            <HeaderStyle CssClass="thLeft" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="nomeCurriculoPeriodo" HeaderText="Séries" SortExpression="nomeCurriculoPeriodo" DataFormatString="{0:d}">
                            <HeaderStyle CssClass="thLeft" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Excluir">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Deletar" SkinID="btExcluir"
                                    CausesValidation="False" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <uc3:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvResultado" />
                <asp:ObjectDataSource ID="odsResultado" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="SELECT_By_Pesquisa" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoResultadoBO"></asp:ObjectDataSource>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
