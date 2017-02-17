<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Classe.JustificativaAbonoFalta.Busca" %>
<%@ Register Src="~/WebControls/BuscaDocente/UCBuscaDocenteTurma.ascx" TagPrefix="uc1" TagName="UCBuscaDocenteTurma" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagPrefix="uc2" TagName="UCCamposObrigatorios" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCPeriodoCalendario.ascx" TagPrefix="uc3" TagName="UCCPeriodoCalendario" %>
<%@ Register Src="~/WebControls/BuscaAluno/UCCamposBuscaAluno.ascx" TagName="UCCamposBuscaAluno"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurmaDisciplina.ascx" TagPrefix="uc6" TagName="UCComboTurmaDisciplina" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary" runat="server" />
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Panel ID="pnlBusca" runat="server">
        <uc2:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
        <asp:UpdatePanel ID="updFiltros" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:UCBuscaDocenteTurma ID="UCBuscaDocenteTurma" runat="server" _VS_CarregarApenasTurmasNormais="true" _VS_MostarComboTipoCiclo="false" />
                <uc6:UCComboTurmaDisciplina runat="server" ID="UCComboTurmaDisciplina" 
                    Obrigatorio="true" PermiteEditar="false" MostrarMensagemSelecione="true"
                    VS_MostraFilhosRegencia="false" />
                <uc4:UCCamposBuscaAluno ID="UCCamposBuscaAluno1" runat="server" />
                <div class="clear"></div>           
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" />
        </div>
    </asp:Panel>
    <div class="clear"></div>
    <asp:UpdatePanel ID="updResultado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="fdsResultados" runat="server" visible="false">
                <legend><asp:Label ID="lblLgdResultados" runat="server" Text="<%$ Resources:Classe, JustificativaAbonoFalta.lblLgdResultados.Text %>"></asp:Label></legend>
                <div>
                    <div align="right" id="divQtdPaginacao" runat="server">
                        <asp:Label ID="lblPag" runat="server" Text="<%$ Resources:Classe, JustificativaAbonoFalta.lblPag.Text %>"></asp:Label>
                        <asp:DropDownList ID="ddlQtPaginado" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlQtPaginado_SelectedIndexChanged">
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                            <asp:ListItem>100</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <br />
                <asp:GridView ID="grvAluno" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    BorderStyle="None" DataKeyNames="alu_id,mtu_id"
                    EmptyDataText="<%$ Resources:Classe, JustificativaAbonoFalta.grvAluno.EmptyDataText %>" AllowCustomPaging="true"
                    OnRowDataBound="grvAluno_RowDataBound" OnDataBound="grvAluno_DataBound"
                    AllowSorting="True" OnPageIndexChanging="grvAluno_PageIndexChanging" OnSorting="grvAluno_Sorting"
                    OnRowEditing="grvAluno_RowEditing" EnableModelValidation="True">
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:Classe, JustificativaAbonoFalta.grvAluno.alc_matriculaEstadual.HeaderText %>" SortExpression="alc_matriculaEstadual">
                            <ItemTemplate>
                                <asp:Label ID="lblMatriculaEstadual" runat="server" Text='<%# Bind("alc_matriculaEstadual") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Mensagens,MSG_NUMEROMATRICULA %>" SortExpression="alc_matricula">
                            <ItemTemplate>
                                <asp:Label ID="lblMatricula" runat="server" Text='<%# Bind("alc_matricula") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Classe, JustificativaAbonoFalta.grvAluno.pes_nome.HeaderText %>" SortExpression="pes_nome">
                            <ItemTemplate>
                                <asp:Label ID="lblNome" runat="server" Text='<%# Bind("pes_nome") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Classe, JustificativaAbonoFalta.grvAluno.tur_escolaUnidade.HeaderText %>" SortExpression="tur_escolaUnidade" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEscola" runat="server" Text='<%# Bind("tur_escolaUnidade") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="tur_codigo" HeaderText="<%$ Resources:Classe, JustificativaAbonoFalta.grvAluno.tur_codigo.HeaderText %>" SortExpression="tur_codigo" />
                        <asp:TemplateField HeaderText="<%$ Resources:Classe, JustificativaAbonoFalta.grvAluno.tur_curso.HeaderText %>" SortExpression="tur_curso" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCurso" runat="server" Text='<%# Bind("tur_curso") %>' CssClass="wrap150px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Classe, JustificativaAbonoFalta.grvAluno.tur_calendario.HeaderText %>" SortExpression="tur_calendario" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCalendario" runat="server" Text='<%# Bind("tur_calendario") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="alu_situacao" HeaderText="<%$ Resources:Classe, JustificativaAbonoFalta.grvAluno.alu_situacao.HeaderText %>" SortExpression="alu_situacao" />
                        <asp:TemplateField HeaderText="<%$ Resources:Classe, JustificativaAbonoFalta.grvAluno.btnAbonoFalta.HeaderText %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnAbonoFalta" runat="server" CommandName="Edit" SkinID="btEditar"
                                    PostBackUrl="~/Classe/JustificativaAbonoFalta/Cadastro.aspx" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
                <uc5:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_grvAluno" class="clTotalReg" />
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
