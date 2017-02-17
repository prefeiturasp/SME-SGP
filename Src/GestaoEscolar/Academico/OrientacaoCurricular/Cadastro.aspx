<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Academico.OrientacaoCurricular.Cadastro" %>

<%@ Register Src="~/WebControls/Combos/UCComboCursoCurriculo.ascx" TagName="UCComboCursoCurriculo"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/Combos/UCComboCurriculoPeriodo.ascx" TagName="UCComboCurriculoPeriodo"
    TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc3" %>
<%@ Register Src="../../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoDisciplina.ascx" TagName="UCComboTipoDisciplina"
    TagPrefix="uc6" %>
<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc7" %>
<%@ Register Src="~/WebControls/Combos/UCComboCalendario.ascx" TagName="UCComboCalendario"
    TagPrefix="uc8" %>

<%@ PreviousPageType VirtualPath="~/Academico/OrientacaoCurricular/Busca.aspx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .grid ul li
        {
            list-style-type: circle;
            list-style-position: inside;
            line-height: 15px;
            padding-left: 3px;
        }
        .grid input[type="image"]
        {
            float: right;
        }
        .grid tr:hover
        {
            background: none;
        }
        .grid td
        {
            vertical-align: top;
        }
        textarea
        {
            width: 98% !important;
            height: 90px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="vsOrientacao" runat="server" ValidationGroup="OrientacaoCurricular" />
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
            <uc4:UCLoader ID="UCLoader1" runat="server" />
            <asp:Panel ID="pnlPesquisa" runat="server" GroupingText="Cadastro de orientações curriculares">
                <asp:Label ID="lblInformacao" runat="server" Visible="false"></asp:Label>
                <div id="divLimparPesquisa" runat="server" class="right" visible="false">
                    <asp:Button ID="btnCopiar" runat="server" Text="Copiar orientação curricular" OnClick="btnCopiar_Click"
                        CausesValidation="false" />
                    <asp:Button ID="btnLimparPesquisa" runat="server" Text="Voltar" OnClick="btnLimparPesquisa_Click" 
                        CausesValidation="false" />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlResultados" runat="server" GroupingText="Resultados">
                <asp:Button ID="btnAdicionarObjetivoCima" runat="server" Text="Adicionar objetivo"
                    OnClick="btnAdicionarObjetivo_Click" Visible="true" CausesValidation="true" ValidationGroup="OrientacaoCurricular" />
                <asp:Label ID="lblMsgRepeater" runat="server" CssClass="summary"></asp:Label>
                <asp:Repeater ID="rptObjetivos" runat="server" OnItemDataBound="rptObjetivos_ItemDataBound">
                    <HeaderTemplate>
                        <table id="tableHeader" class="grid" cellspacing="0" style="width: 100%">
                            <tr class="gridHeader">
                                <th style="width: 15%;">
                                    Objetivos <%=MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB.TextoAsteriscoObrigatorio %>
                                </th>
                                <th style="width: 30%;">
                                    Conteúdos <%=MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB.TextoAsteriscoObrigatorio %>
                                </th>
                                <th style="width: 30%;">
                                    Habilidades <%=MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB.TextoAsteriscoObrigatorio %>
                                </th>
                                <asp:Repeater ID="rptTiposPeriodo" runat="server">
                                    <ItemTemplate>
                                        <th class="tbGrid center">
                                            <asp:Literal ID="lbltpc_id" runat="server" Text='<%#Bind("tpc_id") %>' Visible="false"></asp:Literal>
                                            <asp:Literal ID="litNome" runat="server" Text='<%#Bind("tpc_nome") %>'></asp:Literal>
                                        </th>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                    </HeaderTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                    <ItemTemplate>
                        <tr class="gridRow">
                            <td id="tdObj" runat="server">
                                <asp:ImageButton ID="btnExcluirObjetivo" runat="server" OnClick="btnExcluirObjetivo_Click"
                                    ToolTip="Excluir objetivo." SkinID="btExcluir" />
                                <asp:Literal ID="litObj_id" runat="server" Text='<%#Bind("obj_id") %>' Visible="false"></asp:Literal>
                                <asp:LinkButton ID="Objetivo" runat="server" CommandArgument='<%#Bind("obj_id") %>'
                                    ToolTip="Alterar objetivo / conteúdos / habilidades" OnClick="lkbObjetivo_Click"
                                    Visible='<%#!editandoObjetivo %>' Text='<%#Bind("obj_descricao") %>' CssClass="wrap200px">
                                </asp:LinkButton>
                                <asp:TextBox ID="txtObjetivo" runat="server" Text='<%#Bind("obj_descricao") %>' TextMode="MultiLine"
                                    Columns="10" Rows="3" SkinID="limite4000" Visible='<%#editandoObjetivo %>'>
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvObjetivo" runat="server" ControlToValidate="txtObjetivo"
                                    SetFocusOnError="true" Display="Dynamic" ErrorMessage="Objetivo é obrigatório."
                                    ValidationGroup="OrientacaoCurricular">*</asp:RequiredFieldValidator>
                                <asp:Button ID="btnSalvarObjetivo" runat="server" Text="Salvar objetivo" OnClick="btnSalvarObjetivo_Click"
                                    Visible='<%#editandoObjetivo %>' CausesValidation="true" ValidationGroup="OrientacaoCurricular"
                                    OnClientClick="window.scrollTo(0,0);" />
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="False"
                                    Visible='<%#editandoObjetivo %>' OnClick="btnCancelar_Click" />
                            </td>
                            <td id="tdAdicionarConteudo" runat="server" visible="false">
                                <asp:Button ID="btnAdicionarConteudo" runat="server" Text="Adicionar conteúdo" OnClick="btnAdicionarConteudo_Click"
                                    CausesValidation="true" ToolTip="Adicionar grupo de conteúdos e habilidades."
                                    ValidationGroup="OrientacaoCurricular" />
                            </td>
                            <asp:Repeater ID="rptConteudos" runat="server" OnItemDataBound="rptConteudos_ItemDataBound">
                                <ItemTemplate>
                                    <td>
                                        <asp:ImageButton ID="btnExcluirConteudo" runat="server" OnClick="btnExcluirConteudo_Click"
                                            ToolTip="Excluir grupo de conteúdos e habilidades." SkinID="btExcluir" />
                                        <ul>
                                            <asp:Literal ID="litObj_id" runat="server" Text='<%#Bind("obj_id") %>' Visible="false"></asp:Literal>
                                            <asp:Literal ID="litCtd_id" runat="server" Text='<%#Bind("ctd_id") %>' Visible="false"></asp:Literal>
                                            <asp:Repeater ID="rptConteudoItem" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <asp:Literal ID="litCti_id" runat="server" Text='<%#Bind("cti_id") %>' Visible="false"></asp:Literal>
                                                        <asp:Label ID="lblDescricao" runat="server" Text='<%#Bind("cti_descricao") %>' Visible='<%#!editandoItemObjetivo %>'
                                                            CssClass="wrap250px">
                                                        </asp:Label>
                                                        <asp:TextBox ID="txtDescricao" runat="server" Text='<%#Bind("cti_descricao") %>'
                                                            TextMode="MultiLine" Columns="10" Rows="3" SkinID="limite4000" Visible='<%#editandoItemObjetivo %>'>
                                                        </asp:TextBox>
                                                    </li>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="btnNovoItem" runat="server" SkinID="btNovo" OnClick="btnNovoItemConteudo_Click"
                                                        CausesValidation="false" ToolTip="Adicionar item de conteúdo" Visible='<%#editandoItemObjetivo %>'
                                                        ValidationGroup="OrientacaoCurricular" />
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </ul>
                                        <asp:Button ID="btnAdicionarConteudo" runat="server" Text="Adicionar conteúdo" OnClick="btnAdicionarConteudo_Click"
                                            CausesValidation="false" ToolTip="Adicionar grupo de conteúdos e habilidades."
                                            Visible="false" ValidationGroup="OrientacaoCurricular" />
                                    </td>
                                    <td>
                                        <ul>
                                            <asp:Repeater ID="rptHabilidades" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <asp:Literal ID="litHbl_id" runat="server" Text='<%#Bind("hbl_id") %>' Visible="false"></asp:Literal>
                                                        <asp:Label ID="lblDescricao" runat="server" Text='<%#Bind("hbl_descricao") %>' Visible='<%#!editandoItemObjetivo %>'
                                                            CssClass="wrap250px">
                                                        </asp:Label>
                                                        <asp:TextBox ID="txtDescricao" runat="server" Text='<%#Bind("hbl_descricao") %>'
                                                            TextMode="MultiLine" Columns="10" Rows="3" SkinID="limite4000" Visible='<%#editandoItemObjetivo %>'>
                                                        </asp:TextBox>
                                                    </li>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="btnNovoItem" runat="server" SkinID="btNovo" OnClick="btnNovoItemHabilidade_Click"
                                                        ToolTip="Adicionar habilidade" Visible='<%#editandoItemObjetivo %>' ValidationGroup="OrientacaoCurricular"
                                                        CausesValidation="false" />
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </td>
                                    <asp:Repeater ID="rptTiposPeriodo" runat="server" OnItemDataBound="rptTiposPeriodo_ItemDataBound">
                                        <ItemTemplate>
                                            <td class="tbGrid center">
                                                <asp:Literal ID="lbltpc_id" runat="server" Text='<%#Bind("tpc_id") %>' Visible="false"></asp:Literal>
                                                <asp:CheckBox ID="chkPeriodo" runat="server" Text="" Checked='<%#Bind("ExisteConteudo") %>'
                                                    Enabled='<%#editandoItemObjetivo %>' />
                                            </td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    </tr>
                                    <tr class="gridRow">
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Button ID="btnAdicionarObjetivo" runat="server" Text="Adicionar objetivo" OnClick="btnAdicionarObjetivo_Click"
                    Visible="true" CausesValidation="true" ValidationGroup="OrientacaoCurricular" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divCopiarOrientacao" title="Copiar orientação curricular para outro calendário" class="hide">
        <asp:UpdatePanel ID="updConfirm" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:ValidationSummary ID="vsCopiar" runat="server" ValidationGroup="CopiarOrientacao" />
                <div id="divMsgCopiarOrientacao" runat="server" visible="false">
                    <asp:Label ID="lblMsgErroCopia" runat="server"></asp:Label>
                </div>
                <uc8:UCComboCalendario ID="UCComboCalendario2" runat="server" Obrigatorio="true" MostrarMensagemSelecione="true" 
                    ValidationGroup="CopiarOrientacao" />
                <br /><br />
                <asp:Button ID="btnSalvarCopia" runat="server" Text="Salvar" OnClick="btnSalvarCopia_Click"
                    CausesValidation="true"  ValidationGroup="CopiarOrientacao" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMsgSalvarDados" title="Atenção" class="hide">
        <br />
        <asp:Label ID="lblMsgSalvarDados" runat="server" Text="É necessário salvar todas as informações antes de copiar a orientação curricular."></asp:Label>
    </div>
</asp:Content>
