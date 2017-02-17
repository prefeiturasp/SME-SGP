<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCEnsinoFundamental.ascx.cs" Inherits="GestaoEscolar.WebControls.HistoricoEscolar.UCEnsinoFundamental" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola"
    TagPrefix="uc1" %>
<%@ Register Src="../Combos/UCComboTipoCurriculoPeriodo.ascx" TagName="UCComboTipoCurriculoPeriodo"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Cidade/UCCadastroCidade.ascx" TagName="UCCadastroCidade"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoRedeEnsino.ascx" TagName="UCComboTipoRedeEnsino"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc6" %>
<%@ Register Src="UCAddResultadoFinal.ascx" TagName="UCAddResultadoFinal"
    TagPrefix="uc7" %>
<%@ Register Src="~/WebControls/Endereco/UCEnderecos.ascx" TagName="UCEnderecos"
    TagPrefix="uc8" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoRedeEnsino.ascx" TagName="UCComboTipoRedeEnsino"
    TagPrefix="uc9" %>

<asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
<fieldset>
    <legend>
        <asp:Label ID="lblLegend" Text="<%$ Resources:UserControl, UCEnsinoFundamental.lblLegend.Text %>" runat="server" EnableViewState="False"></asp:Label>
    </legend>
    <asp:UpdatePanel ID="updEnsinoFundamental" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="btnAddNovoHistorico" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.btnAddNovoHistorico.Text %>"
                OnClick="btnAddNovoHistorico_Click" CausesValidation="false" />
            <asp:Button ID="btnAddrProjAtivComplementar" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.btnAddrProjAtivComplementar.Text %>"
                OnClick="btnAddrProjAtivComplementar_Click" CausesValidation="false" />
            <%--EmptyDataText="<%$ Resources:UserControl, UCEnsinoFundamental.grvHistorico.EmptyDataText %>"--%>
            <asp:GridView ID="grvHistorico" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                BorderStyle="None" DataKeyNames="alh_id,alh_anoLetivo,permiteEditar"
                OnRowCommand="grvHistorico_RowCommand" OnRowDataBound="grvHistorico_RowDataBound"
                AllowSorting="False" EnableModelValidation="True">
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCEnsinoFundamental.grvHistorico.Header.Ano %>">
                        <ItemTemplate>
                            <asp:Label ID="lblAno" runat="server" Text='<%# Bind("alh_anoLetivo") %>'></asp:Label>
                            <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Alterar"
                                CausesValidation="False" Text='<%# Bind("alh_anoLetivo") %>' CssClass="wrap400px"></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="eco_nome" HeaderText="<%$ Resources:UserControl, UCEnsinoFundamental.grvHistorico.Header.Escola %>" />
                    <asp:BoundField DataField="crp_descricao" HeaderText="<%$ Resources:UserControl, UCEnsinoFundamental.grvHistorico.Header.Serie %>" />
                    <asp:BoundField DataField="alh_resultadoDescricaoText" HeaderText="<%$ Resources:UserControl, UCEnsinoFundamental.grvHistorico.Header.Resultado %>" />
                    <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCEnsinoFundamental.grvHistorico.Header.Excluir %>">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Deletar" SkinID="btExcluir"
                                CausesValidation="False" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" Width="80px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <fieldset id="fdsResFinal" runat="server" visible="false" class="areaResultados">
                <legend>
                    <asp:Label ID="lblLegendResultadoFinal" Text="<%$ Resources:UserControl, UCEnsinoFundamental.lblLegendResultadoFinal.Text %>" runat="server" EnableViewState="False"></asp:Label>
                </legend>
                <asp:Repeater ID="rptResFinal" runat="server" OnItemDataBound="rptResFinal_ItemDataBound">
                    <HeaderTemplate>
                        <div>
                            <table id="tabela" class="grid">
                                <thead>
                                    <tr class="gridHeader">
                                        <th class="center">
                                            <asp:Label ID="lblCompCurricular" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.lblCompCurricular.Text %>"></asp:Label>
                                        </th>
                                        <asp:Repeater ID="rptDadosResFinal" runat="server" OnItemDataBound="rptDadosResFinalHeader_ItemDataBound">
                                            <ItemTemplate>
                                                <th class="center">
                                                    <table id="tabela" class="grid">
                                                        <thead>
                                                            <tr class="gridHeader">
                                                                <th class="center">
                                                                    <asp:Label ID="lblAno" runat="server" Text='<%#Bind("tcp_descricao") %>'></asp:Label>
                                                                    <asp:Label ID="lblFav" runat="server" Text='<%#Bind("fav_id") %>' Visible="false"></asp:Label>
                                                                </th>
                                                            </tr>
                                                            <tr class="gridHeader">
                                                                <th class="center hasTableInside">
                                                                    <table id="tabelaHead" class="grid">
                                                                        <thead>
                                                                            <tr class="gridHeader">
                                                                                <th class="center">
                                                                                    <asp:Label ID="lblNota" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.lblNota.Text.Nota %>"></asp:Label>
                                                                                </th>
                                                                                <th class="center">
                                                                                    <asp:Label ID="lblFreq" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.lblFreq.Text %>"></asp:Label>
                                                                                </th>
                                                                            </tr>
                                                                        </thead>
                                                                    </table>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                    </table>
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                </thead>
                                <tbody>
                    </HeaderTemplate>
                    <AlternatingItemTemplate>
                        <tr class="gridAlternatingRow">
                            <td id="tdComCurricular" style="text-align: center;" runat="server">
                                <asp:Label ID="lbltds_id" runat="server" Text='<%#Bind("tds_id") %>' Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblahd_id" runat="server" Text='<%#Bind("ahd_id") %>' Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblCompCurricular" runat="server" Text='<%#Bind("tds_nome") %>'></asp:Label>
                            </td>
                            <asp:Repeater ID="rptDadosResFinal" runat="server" OnItemDataBound="rptDadosResFinal_ItemDataBound">
                                <ItemTemplate>
                                    <td style="text-align: center;" runat="server" id="tdAnos" class="hasTableInside">
                                        <asp:Label ID="lblalh_id" runat="server" Text='<%#Bind("alh_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblahd_id" runat="server" Text='<%#Bind("ahd_id") %>' Visible="false">
                                        </asp:Label>
                                        <table id="tabela" class="grid">
                                            <tr>
                                                <td id="nota">
                                                    <asp:Label ID="lblNota" runat="server" Text='<%#Bind("ahd_avaliacao") %>'></asp:Label>
                                                </td>
                                                <td id="freq">
                                                    <asp:Label ID="lblFreq" runat="server" Text='<%#Bind("ahd_frequencia") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </AlternatingItemTemplate>
                    <ItemTemplate>
                        <tr class="gridRow">
                            <td id="tdComCurricular" style="text-align: center;" runat="server">
                                <asp:Label ID="lbltds_id" runat="server" Text='<%#Bind("tds_id") %>' Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblCompCurricular" runat="server" Text='<%#Bind("tds_nome") %>'></asp:Label>
                            </td>
                            <asp:Repeater ID="rptDadosResFinal" runat="server" OnItemDataBound="rptDadosResFinal_ItemDataBound">
                                <ItemTemplate>
                                    <td style="text-align: center;" runat="server" id="tdAnos" class="hasTableInside">
                                        <asp:Label ID="lblalh_id" runat="server" Text='<%#Bind("alh_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblahd_id" runat="server" Text='<%#Bind("ahd_id") %>' Visible="false">
                                        </asp:Label>
                                        <table id="tabela" class="grid">
                                            <tr>
                                                <td id="nota">
                                                    <asp:Label ID="lblNota" runat="server" Text='<%#Bind("ahd_avaliacao") %>'></asp:Label>
                                                </td>
                                                <td id="freq">
                                                    <asp:Label ID="lblFreq" runat="server" Text='<%#Bind("ahd_frequencia") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                            </table>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:Repeater ID="rptEnrCurricular" runat="server" OnItemDataBound="rptEnrCurricular_ItemDataBound">
                    <HeaderTemplate>
                        <div>
                            <table id="tabela" class="grid">
                                <thead>
                                    <tr class="gridHeader">
                                        <th class="center">
                                            <asp:Label ID="lblEnrCurricular" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.lblEnrCurricular.Text %>"></asp:Label>
                                        </th>
                                        <asp:Repeater ID="rptDadosEnrCurricular" runat="server" OnItemDataBound="rptDadosEnrCurricularHeader_ItemDataBound">
                                            <ItemTemplate>
                                                <th class="center hasTableInside">
                                                    <table id="tabela" class="grid">
                                                        <thead>
                                                            <tr class="gridHeader">
                                                                <th class="center">
                                                                    <asp:Label ID="lblAno" runat="server" Text='<%#Bind("tcp_descricao") %>'></asp:Label>
                                                                </th>
                                                            </tr>
                                                            <tr class="gridHeader">
                                                                <th class="center">
                                                                    <asp:Label ID="lblFreq" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.lblFreq.Text %>"></asp:Label>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                    </table>
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                </thead>
                                <tbody>
                    </HeaderTemplate>
                    <AlternatingItemTemplate>
                        <tr class="gridAlternatingRow">
                            <td id="tdEnrCurricular" style="text-align: center;" runat="server">
                                <asp:Label ID="lbltds_id" runat="server" Text='<%#Bind("tds_id") %>' Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblEnrCurricular" runat="server" Text='<%#Bind("tds_nome") %>'></asp:Label>
                            </td>
                            <asp:Repeater ID="rptDadosEnrCurricular" runat="server" OnItemDataBound="rptDadosEnrCurricular_ItemDataBound">
                                <ItemTemplate>
                                    <td style="text-align: center;" runat="server" id="tdAnos">
                                        <asp:Label ID="lblalh_id" runat="server" Text='<%#Bind("alh_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblahd_id" runat="server" Text='<%#Bind("ahd_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblFreq" runat="server" Text='<%#Bind("ahd_resultado") %>'></asp:Label>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </AlternatingItemTemplate>
                    <ItemTemplate>
                        <tr class="gridRow">
                            <td id="tdEnrCurricular" style="text-align: center;" runat="server">
                                <asp:Label ID="lbltds_id" runat="server" Text='<%#Bind("tds_id") %>' Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblEnrCurricular" runat="server" Text='<%#Bind("tds_nome") %>'></asp:Label>
                            </td>
                            <asp:Repeater ID="rptDadosEnrCurricular" runat="server" OnItemDataBound="rptDadosEnrCurricular_ItemDataBound">
                                <ItemTemplate>
                                    <td style="text-align: center;" runat="server" id="tdAnos">
                                        <asp:Label ID="lblalh_id" runat="server" Text='<%#Bind("alh_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblahd_id" runat="server" Text='<%#Bind("ahd_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblFreq" runat="server" Text='<%#Bind("ahd_resultado") %>'></asp:Label>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                            </table>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:Repeater ID="rptProjeto" runat="server" OnItemDataBound="rptProjeto_ItemDataBound">
                    <HeaderTemplate>
                        <div>
                            <table id="tabela" class="grid">
                                <thead>
                                    <tr class="gridHeader">
                                        <th class="center">
                                            <asp:Label ID="lblProjAtivCompl" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.lblProjAtivCompl.Text %>"></asp:Label>
                                        </th>
                                        <asp:Repeater ID="rptDadosProjeto" runat="server" OnItemDataBound="rptDadosProjetoHeader_ItemDataBound">
                                            <ItemTemplate>
                                                <th class="center hasTableInside">
                                                    <table id="tabela" class="grid">
                                                        <thead>
                                                            <tr class="gridHeader">
                                                                <th class="center">
                                                                    <asp:Label ID="lblAno" runat="server" Text='<%#Bind("tcp_descricao") %>'></asp:Label>
                                                                </th>
                                                            </tr>
                                                            <tr class="gridHeader">
                                                                <th class="center">
                                                                    <asp:Label ID="lblFreq" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.lblFreq.Text %>"></asp:Label>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                    </table>
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                </thead>
                                <tbody>
                    </HeaderTemplate>
                    <AlternatingItemTemplate>
                        <tr class="gridAlternatingRow">
                            <td id="tdProjeto" style="text-align: center;" runat="server">
                                <asp:Label ID="lblahp_id" runat="server" Text='<%#Bind("ahp_id") %>' Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblProjeto" runat="server" Text='<%#Bind("ahp_nome") %>'></asp:Label>
                            </td>
                            <asp:Repeater ID="rptDadosProjeto" runat="server" OnItemDataBound="rptDadosProjeto_ItemDataBound">
                                <ItemTemplate>
                                    <td style="text-align: center;" runat="server" id="tdAnos">
                                        <asp:Label ID="lblalh_id" runat="server" Text='<%#Bind("alh_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblahd_id" runat="server" Text='<%#Bind("ahd_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblFreq" runat="server" Text='<%#Bind("ahd_resultado") %>'></asp:Label>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </AlternatingItemTemplate>
                    <ItemTemplate>
                        <tr class="gridRow">
                            <td id="tdProjeto" style="text-align: center;" runat="server">
                                <asp:Label ID="lblahp_id" runat="server" Text='<%#Bind("ahp_id") %>' Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblProjeto" runat="server" Text='<%#Bind("ahp_nome") %>'></asp:Label>
                            </td>
                            <asp:Repeater ID="rptDadosProjeto" runat="server" OnItemDataBound="rptDadosProjeto_ItemDataBound">
                                <ItemTemplate>
                                    <td style="text-align: center;" runat="server" id="tdAnos">
                                        <asp:Label ID="lblalh_id" runat="server" Text='<%#Bind("alh_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblahd_id" runat="server" Text='<%#Bind("ahd_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblFreq" runat="server" Text='<%#Bind("ahd_resultado") %>'></asp:Label>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                            </table>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:Repeater ID="rptParecer" runat="server" OnItemDataBound="rptParecer_ItemDataBound">
                    <HeaderTemplate>
                        <div>
                            <table id="tabela" class="grid">
                                <thead>
                                    <tr class="gridHeader">
                                        <th class="center">
                                            <asp:Label ID="lblParecer" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.lblParecer.Text %>"></asp:Label>
                                        </th>
                                        <asp:Repeater ID="rptDadosParecer" runat="server" OnItemDataBound="rptDadosParecerHeader_ItemDataBound">
                                            <ItemTemplate>
                                                <th class="center hasTableInside">
                                                    <table id="tabela" class="grid">
                                                        <thead>
                                                            <tr class="gridHeader">
                                                                <th class="center">
                                                                    <asp:Label ID="lblAno" runat="server" Text='<%#Bind("tcp_descricao") %>'></asp:Label>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                    </table>
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                </thead>
                                <tbody>
                    </HeaderTemplate>
                    <AlternatingItemTemplate>
                        <tr class="gridAlternatingRow">
                            <td id="tdParecer" style="text-align: center;" runat="server"></td>
                            <asp:Repeater ID="rptDadosParecer" runat="server" OnItemDataBound="rptDadosParecer_ItemDataBound">
                                <ItemTemplate>
                                    <td style="text-align: center;" runat="server" id="tdAnos">
                                        <asp:Label ID="lbltcp_id" runat="server" Text='<%#Bind("tcp_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblPar" runat="server" Text='<%#Bind("alh_resultado") %>'></asp:Label>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </AlternatingItemTemplate>
                    <ItemTemplate>
                        <tr class="gridRow">
                            <td id="tdParecer" style="text-align: center;" runat="server"></td>
                            <asp:Repeater ID="rptDadosParecer" runat="server" OnItemDataBound="rptDadosParecer_ItemDataBound">
                                <ItemTemplate>
                                    <td style="text-align: center;" runat="server" id="tdAnos">
                                        <asp:Label ID="lbltcp_id" runat="server" Text='<%#Bind("tcp_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblPar" runat="server" Text='<%#Bind("alh_resultado") %>'></asp:Label>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                            </table>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
            </fieldset>
            <div class="right">
                <asp:Button ID="btnVisualizarHistorico" runat="server" Text="Visualizar histórico" CausesValidation="false"
                    OnClick="btnVisualizarHistorico_Click" />
                <asp:Button ID="btnVoltar" runat="server" Text="Voltar" CausesValidation="false"
                    OnClick="btnVoltar_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</fieldset>
<div id="divAddHistorico" title="<%$ Resources:UserControl, UCEnsinoFundamental.divAddHistorico.title %>"
    class="hide divAddHistorico" runat="server">
    <asp:UpdatePanel ID="updAddHistorico" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessageAddHistorico" runat="server" EnableViewState="false"></asp:Label>
            <%--<uc6:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />--%>
            <asp:ValidationSummary ID="vasAddHistorico" runat="server" ValidationGroup="AddHistorico" />
            <fieldset id="fdsAddHistorico" runat="server">
                <fieldset>
                    <legend>
                        <asp:Label ID="lblLegendResFinalHist" Text="<%$ Resources:UserControl, UCEnsinoFundamental.lblLegendResFinalHist.Text %>" runat="server" EnableViewState="False"></asp:Label>
                    </legend>
                    <div>
                        <div class="itensInline">
                            <asp:Label ID="lblAnoConclusao" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.lblAnoConclusao.Text %>" AssociatedControlID="ddlAnoConclusao"></asp:Label>
                            <asp:DropDownList ID="ddlAnoConclusao" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAnoConclusao_SelectedIndexChanged"></asp:DropDownList>
                            <asp:CompareValidator ID="cpvAnoConclusao" runat="server" ErrorMessage="<%$ Resources:UserControl, UCEnsinoFundamental.cpvAnoConclusao.ErrorMessage %>"
                                ControlToValidate="ddlAnoConclusao" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
                                Visible="false">*</asp:CompareValidator>
                            <%--<asp:TextBox ID="txtAnoConclusao" runat="server" SkinID="Numerico" MaxLength="4" AutoPostBack="true"
                                OnTextChanged="txtAnoConclusao_TextChanged"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAnoConclusao" runat="server" ControlToValidate="txtAnoConclusao"
                                ErrorMessage="<%$ Resources:UserControl, UCEnsinoFundamental.rfvAnoConclusao.ErrorMessage %>" ValidationGroup="AddHistorico">*</asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="itensInline">
                            <uc3:UCComboTipoCurriculoPeriodo ID="UCComboTipoCurriculoPeriodo1" runat="server" />
                        </div>
                        <div class="itensInline">
                            <asp:Label ID="lblCargaHoraria" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.lblCargaHoraria.Text %>" AssociatedControlID="txtCargaHoraria"></asp:Label>
                            <asp:TextBox ID="txtCargaHoraria" runat="server" SkinID="text10C"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCargaHoraria" runat="server" ControlToValidate="txtCargaHoraria"
                                ErrorMessage="<%$ Resources:UserControl, UCEnsinoFundamental.rfvCargaHoraria.ErrorMessage %>" ValidationGroup="AddHistorico">*</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <br />
                    <fieldset>
                        <legend>
                            <asp:Label ID="lblLegendOrigem" Text="<%$ Resources:UserControl, UCEnsinoFundamental.lblLegendOrigem.Text %>" runat="server" EnableViewState="False"></asp:Label>
                        </legend>
                        <asp:RadioButtonList ID="rblOrigemEscola" runat="server" RepeatDirection="Horizontal"
                            OnSelectedIndexChanged="rblOrigemEscola_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="<%$ Resources:UserControl, UCEnsinoFundamental.rblOrigemEscola.DentroRede %>" Value="1"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:UserControl, UCEnsinoFundamental.rblOrigemEscola.ForaRede %>" Value="2"></asp:ListItem>
                        </asp:RadioButtonList>
                    </fieldset>
                    <div runat="server" id="divDentroRede" visible="false">
                        <uc1:UCComboUAEscola ID="UCComboUAEscola" runat="server" CarregarEscolaAutomatico="true"
                            MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true" OnIndexChangedUA="UCComboUAEscola_IndexChangedUA"
                            OnIndexChangedUnidadeEscola="UCComboUAEscola_IndexChangedUnidadeEscola" ValidationGroup="AddHistorico" />
                    </div>
                    <div id="divForaRede" runat="server" visible="false">
                        <asp:Label ID="lblEscolaOrigem" runat="server" Text="Escola *" AssociatedControlID="txtEscolaOrigem"></asp:Label>
                        <asp:TextBox ID="txtEscolaOrigem" runat="server" Width="445px" MaxLength="200" Enabled="false"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEscola" runat="server" Display="Dynamic" ErrorMessage="<%$ Resources:UserControl, UCEnsinoFundamental.rfvEscola.ErrorMessage %>"
                            ControlToValidate="txtEscolaOrigem" ValidationGroup="AddHistorico">*</asp:RequiredFieldValidator>
                        <asp:ImageButton ID="btnEscolaOrigem" runat="server" CausesValidation="false" SkinID="btPesquisar"
                            OnClick="btnEscolaOrigem_Click" />
                    </div>
                    <br />
                    <div runat="server" id="divResFinal" visible="false">
                        <uc7:UCAddResultadoFinal ID="UCAddResultadoFinal1" runat="server" />
                    </div>
                </fieldset>
                <div class="right">
                    <asp:Button ID="btnSalvarAddHistorico" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.btnSalvarAddHistorico.Text %>" OnClick="btnSalvarAddHistorico_Click"
                        ValidationGroup="AddHistorico" />
                    <asp:Button ID="btnCancelarAddHistorico" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.btnCancelarAddHistorico.Text %>" CausesValidation="false"
                        OnClientClick="$('#divAddHistorico').dialog('close');" OnClick="btnCancelarAddHistorico_Click" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div id="divProjAtivComplementar" title="<%$ Resources:UserControl, UCEnsinoFundamental.divProjAtivComplementar.title %>"
    class="hide divProjAtivComplementar" runat="server">
    <asp:UpdatePanel ID="updProjAtivComplementar" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessageProjAtivComplementar" runat="server" EnableViewState="false"></asp:Label>
            <%--<uc6:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server"  />--%>
            <asp:ValidationSummary ID="vasProjAtivComplementar" runat="server" ValidationGroup="ProjAtivComplementar" />
            <div id="divBuscaProjAtivComplementar" runat="server">
                <div class="right">
                    <asp:GridView ID="grvProjAtivComplementar" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                        BorderStyle="None" DataKeyNames="ahp_id,ahp_nome" EmptyDataText="<%$ Resources:UserControl, UCEnsinoFundamental.grvProjAtivComplementar.EmptyDataText %>"
                        OnRowDataBound="grvProjAtivComplementar_RowDataBound" OnRowCommand="grvProjAtivComplementar_RowCommand"
                        AllowSorting="False" EnableModelValidation="True">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCEnsinoFundamental.grvProjAtivComplementar.Header.Nome %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblNome" runat="server" Text='<%# Bind("ahp_nome") %>'></asp:Label>
                                    <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Alterar"
                                        CausesValidation="False" Text='<%# Bind("ahp_nome") %>' CssClass="wrap400px"></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle CssClass="left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCEnsinoFundamental.grvProjAtivComplementar.Header.Excluir %>">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Deletar" SkinID="btExcluir"
                                        CausesValidation="False" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center" Width="80px" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Button ID="btnAddProjAtivCompl" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.btnAddProjAtivCompl.Text %>"
                        OnClick="btnAddProjAtivCompl_Click" CausesValidation="false" />
                    <asp:Button ID="btnCancelarBuscaProjAtivCompl" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.btnCancelarBuscaProjAtivCompl.Text %>"
                        OnClick="btnCancelarBuscaProjAtivCompl_Click" CausesValidation="false" />
                </div>
                <br />
            </div>
            <div id="divCadastroProjAtivComplementar" runat="server" visible="false">
                <asp:Label ID="lblNomeProjeto" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.lblNomeProjeto.Text %>"
                    AssociatedControlID="txtNomeProjeto"></asp:Label>
                <asp:TextBox ID="txtNomeProjeto" runat="server" SkinID="text60C"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNomeProjeto" runat="server" ErrorMessage="<%$ Resources:UserControl, UCEnsinoFundamental.rfvNomeProjeto.ErrorMessage %>"
                    ControlToValidate="txtNomeProjeto" ValidationGroup="ProjAtivComplementar">*</asp:RequiredFieldValidator>
                <div class="right">
                    <asp:Button ID="btnSalvarProjAtivCompl" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.btnSalvarProjAtivCompl.Text %>"
                        OnClick="btnSalvarProjAtivCompl_Click" ValidationGroup="ProjAtivComplementar" />
                    <asp:Button ID="btnCancelarProjAtivCompl" runat="server" Text="<%$ Resources:UserControl, UCEnsinoFundamental.btnCancelarProjAtivCompl.Text %>"
                        OnClick="btnCancelarProjAtivCompl_Click" CausesValidation="false" />
                </div>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<!-- Busca de escola de origem -->
<div id="divBuscaEscolaOrigem" title="Busca de escola de origem" class="hide">
    <asp:UpdatePanel ID="updBuscaEscolaOrigem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset>
                <asp:Label ID="lblMessageBuscaEscolaOrigem" runat="server" EnableViewState="false"></asp:Label>
                <asp:Panel ID="pnlBuscaEscolaOrigemForaRede" runat="server" DefaultButton="btnPesquisarEscolaOrigem">
                    <uc5:UCComboTipoRedeEnsino ID="UCComboTipoRedeEnsinoBusca" runat="server" />
                    <asp:Label ID="lblBuscaEscolaOrigem" runat="server" Text="Escola de origem" AssociatedControlID="txtBuscaEscolaOrigem"
                        EnableViewState="false"></asp:Label>
                    <asp:TextBox ID="txtBuscaEscolaOrigem" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
                    <div class="right">
                        <asp:Button ID="btnPesquisarEscolaOrigem" runat="server" Text="Pesquisar" CausesValidation="false"
                            OnClick="btnPesquisarEscolaOrigem_Click" />
                        <asp:Button ID="btnNovoEscolaOrigem" runat="server" Text="Adicionar escola de origem"
                            CausesValidation="False" OnClick="btnNovoEscolaOrigem_Click" />
                        <asp:Button ID="btnCancelarBuscaEscolaOrigem" runat="server" Text="Cancelar" CausesValidation="False"
                            OnClientClick="$('#divBuscaEscolaOrigem').dialog('close');return false;" />
                    </div>
                </asp:Panel>
            </fieldset>
            <fieldset id="fdsResultadosEscolaOrigem" runat="server" visible="false">
                <legend>Resultados</legend>
                <asp:GridView ID="grvEscolaOrigem" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    DataKeyNames="eco_id,eco_nome" DataSourceID="odsEscolaOrigem" EmptyDataText="A pesquisa não encontrou resultados."
                    OnSelectedIndexChanging="grvEscolaOrigem_SelectedIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Escola de origem">
                            <ItemTemplate>
                                <asp:LinkButton ID="lkbSelecionar" runat="server" CausesValidation="False" CommandName="Select"
                                    Text='<%# Bind("eco_nome") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="eco_codigoInep" HeaderText="Código INEP" />
                    </Columns>
                </asp:GridView>
            </fieldset>
            <asp:ObjectDataSource ID="odsEscolaOrigem" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_AlunoEscolaOrigem"
                EnablePaging="True" MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords"
                SelectMethod="Select_EscolasPor_RedeEnsino_Nome" StartRowIndexParameterName="currentPage"
                TypeName="MSTech.GestaoEscolar.BLL.ACA_AlunoEscolaOrigemBO" OnSelecting="odsEscolaOrigem_Selecting"></asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsEscola" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ESC_Escola"
                EnablePaging="True" MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords"
                SelectMethod="GetSelect" StartRowIndexParameterName="currentPage" TypeName="MSTech.GestaoEscolar.BLL.ESC_EscolaBO"
                OnSelecting="odsEscola_Selecting" DeleteMethod="Delete" OldValuesParameterFormatString="original_{0}"
                UpdateMethod="Save"></asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<!-- Cadastro de escola de origem -->
<div id="divCadastroEscolaOrigem" title="Cadastro de escola de origem" class="hide">
    <asp:UpdatePanel ID="updCadastroEscolaOrigem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset>
                <asp:Label ID="lblMessageEscolaOrigem" runat="server" EnableViewState="False"></asp:Label>
                <asp:ValidationSummary ID="vlsEscolaOrigem" runat="server" ValidationGroup="EscolaOrigem"
                    EnableViewState="False" />
                <uc9:UCComboTipoRedeEnsino ID="UCComboTipoRedeEnsino1" runat="server" />
                <asp:Label ID="lblNomeEscolaOrigem" runat="server" Text="Escola de origem *" AssociatedControlID="txtNomeEscolaOrigem"
                    EnableViewState="False"></asp:Label>
                <asp:TextBox ID="txtNomeEscolaOrigem" runat="server" SkinID="text60C" MaxLength="200"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNomeEscolaOrigem" ControlToValidate="txtNomeEscolaOrigem"
                    ValidationGroup="EscolaOrigem" runat="server" ErrorMessage="Escola de origem é obrigatório.">*</asp:RequiredFieldValidator>
                <asp:Label ID="lblCodigoInepEscolaOrigem" runat="server" Text="Código INEP" AssociatedControlID="txtCodigoInepEscolaOrigem"
                    EnableViewState="False"></asp:Label>
                <asp:TextBox ID="txtCodigoInepEscolaOrigem" runat="server" SkinID="text20C" MaxLength="20"></asp:TextBox>
                <input id="txtCid_idMunicipioAluno" runat="server" type="hidden" class="tbCid_idMunicipioAluno_incremental" />
                <asp:Label ID="lblCidade" runat="server" Text="Cidade" AssociatedControlID="txtMunicipioAlunoDaEscola"></asp:Label>
                <asp:TextBox ID="txtMunicipioAlunoDaEscola" runat="server" MaxLength="200" Width="180px"
                    CssClass="tbMunicipioAluno_incremental"></asp:TextBox>
                <asp:ImageButton ID="btnCadastraCidadeAluno" runat="server" SkinID="btNovo" CausesValidation="false"
                    ToolTip="Cadastrar nova cidade" OnClick="btnCadastraCidadeAluno_Click" Style="vertical-align: middle" />
                <uc8:UCEnderecos ID="UCEnderecos2" runat="server" />
                <asp:Label ID="lblEstado" runat="server" AssociatedControlID="ddlEstado" Text="Unidade Federativa"></asp:Label>
                <asp:DropDownList ID="ddlEstado" CssClass="ddlEstadoAluno" runat="server" DataTextField="unf_nome"
                    DataValueField="unf_id" AppendDataBoundItems="True">
                </asp:DropDownList>
                <div class="right">
                    <asp:Button ID="btnIncluirEscolaOrigem" runat="server" Text="Incluir" OnClick="btnIncluirEscolaOrigem_Click"
                        ValidationGroup="EscolaOrigem" />
                    <asp:Button ID="btnCancelarEscolaOrigem" runat="server" Text="Cancelar" CausesValidation="False"
                        OnClientClick="$('#divCadastroEscolaOrigem').dialog('close');return false;" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<!-- Cadastro de cidades -->
<div id="divCadastroCidade" title="Cadastro de cidades" class="hide">
    <asp:UpdatePanel ID="updCadastroCidade" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc4:UCCadastroCidade ID="UCCadastroCidade1" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
