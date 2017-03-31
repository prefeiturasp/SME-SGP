<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Academico.ControleSemanal.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Academico/ControleSemanal/Busca.aspx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var opcao;

        function SetaAvaliacao(btn) {
            var item = $(btn).parent();
            var tpc_id = item.find('input[name$="hdnPeriodo"]').val();
            var tpc_ordem = item.find('input[name$="hdnPeriodoOrdem"]').val();
            var ava_id = item.find('input[name$="hdnIdAvaliacao"]').val();
            var ava_tipo = item.find('input[name$="hdnAvaliacaoTipo"]').val();

            var dadosFechamento = $("#divDadosFechamento");

            dadosFechamento.find('input[name$="hdnTpcId"]').val(tpc_id);
            dadosFechamento.find('input[name$="hdnAvaId"]').val(ava_id);
            dadosFechamento.find('input[name$="hdnTipoAvaliacao"]').val(ava_tipo);
            dadosFechamento.find('input[name$="hdnTpcOrdem"]').val(tpc_ordem);
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upnMensagem" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <fieldset>
        <legend>Planejamento semanal</legend>
        <fieldset>
            <asp:Label runat="server" ID="lblCabecalho"></asp:Label>
        </fieldset>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="mapDireita opcoes-bimestre">
                    <asp:Repeater ID="rptPeriodo" runat="server" OnItemDataBound="rptPeriodo_ItemDataBound">
                        <ItemTemplate>
                            <span class="botao-periodo">
                                <asp:Button ID="btnPeriodo" runat="server" Text='<%# Eval("cap_descricao") %>' 
                                    OnClick="btnPeriodo_Click" OnClientClick="SetaAvaliacao(this);"/>
                                <asp:HiddenField ID="hdnPeriodo" runat="server" Value='<%# Eval("tpc_id") %>' />
                                <asp:HiddenField ID="hdnPeriodoOrdem" runat="server" Value='<%# Eval("tpc_ordem") %>' />
                                <asp:Label ID="lblNomeAbreviado" runat="server" Text='<%# Eval("tpc_nomeAbreviado") %>' style="display:none;" CssClass="abbr-periodo"/>
                            </span>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:HiddenField ID="hdnTudId" runat="server" />
                    <asp:HiddenField ID="hdnTurId" runat="server" />
                    <asp:HiddenField ID="hdnTpcId" runat="server" />
                    <asp:HiddenField ID="hdnTurTipo" runat="server" />
                    <asp:HiddenField ID="hdnCalId" runat="server" />
                    <asp:HiddenField ID="hdnTudTipo" runat="server" />
                    <asp:HiddenField ID="hdnTpcOrdem" runat="server" />
                    <asp:HiddenField ID="hdnTipoDocente" runat="server" />
                </div>
                <asp:Panel ID="pnlPlanejamentoSemanal" runat="server">
                    <table align="center" style="margin: 0 auto;">
                        <tr>
                            <td style="padding-top: 20px; width: 20px; vertical-align: bottom;" id="td_lkbAnterior" runat="server">
                                <asp:LinkButton Style="zoom: 140%; -moz-transform: scale(1.40);" ID="lkbAnterior" Text="|<" runat="server" OnClick="lkbAnterior_Click"
                                    CssClass="ui-icon ui-icon-circle-triangle-w"></asp:LinkButton>
                            </td>
                            <td style="padding-top: 0px; vertical-align: bottom;">
                                <asp:Label ID="lblInicio" runat="server" Text="DD/MM/YYYY" AssociatedControlID="lkbProximo"
                                    Font-Bold="True" class="lblInicio"/>
                            </td>
                            <td style="padding-top: 15px; vertical-align: bottom;">
                                <asp:Label ID="Label3" runat="server" Text=" - " AssociatedControlID="lkbProximo"
                                    Font-Bold="True" />
                            </td>
                            <td class="clear"></td>
                            <td style="padding-top: 14px; vertical-align: bottom;">
                                <asp:Label ID="lblFim" runat="server" Text="DD/MM/YYYY" AssociatedControlID="lkbProximo"
                                    Font-Bold="True" class="lblFim"/>
                            </td>
                            <td style="padding-top: 20px; width: 50px; vertical-align: bottom;" id="td_lkbProximo" runat="server">
                                <asp:LinkButton Style="zoom: 140%; -moz-transform: scale(1.40);" ID="lkbProximo" Text=">|" runat="server" OnClick="lkbProximo_Click"
                                    CssClass="ui-icon ui-icon-circle-triangle-e"/>
                            </td>
                        </tr>
                    </table>
                    <asp:Label runat="server" ID="lblMessageAulas" Visible="false"></asp:Label>
                    <div runat="server" id="divAulas" visible="false">
                        <div>
                            <table cellspacing="0" class="grid grid-responsive-list">
                                <thead>
                                    <tr class="gridHeader">
                                        <asp:Repeater runat="server" ID="rptDiasSemana">
                                            <ItemTemplate>
                                                <th class="center" style="border-left:1px solid; border-right:1px solid;">
                                                    <asp:Label ID="lblDataAula" runat="server" Text='<%#Bind("data") %>'></asp:Label>
                                                    <br />
                                                    <asp:Label ID="lblDiaSemana" runat="server" Text='<%#Bind("diaSemana") %>'></asp:Label>
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr class="gridRow">
                                        <asp:Repeater runat="server" ID="rptAulas" OnItemDataBound="rptAulas_ItemDataBound">
                                            <ItemTemplate>
                                                <td style="border-left:1px solid; border-right:1px solid; width:17%">
                                                    <div class="tdSemBordaInferior">
                                                        <asp:CheckBoxList ID="chlComponenteCurricular" runat="server" DataTextField="tud_nome"
                                                            DataValueField="tur_tud_id" CssClass="checkBoxListVertical">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                    <asp:HiddenField ID="hdftau_id" runat="server" Value='<%#Bind("tau_id") %>' />
                                                    <asp:HiddenField ID="hdnPermissaoAlteracao" runat="server" Value='<%#Bind("permissaoAlteracao") %>' />
                                                    <asp:HiddenField ID="hdfSemPlanoAula" runat="server" Value='<%#Bind("semPlanoAula") %>' />
                                                    <asp:Image ID="imgSemPlanoAula" runat="server" Visible="false" SkinID="imgStatusAlertaAulaSemPlano" Width="16px" Height="16px" ImageAlign="Top" />
                                                    <asp:TextBox ID="txtPlanoAula" runat="server" Text='<%#Bind("planoAula") %>' TextMode="MultiLine" SkinID="limite4000" Width="90%"></asp:TextBox>
                                                </td>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                        </tr>
                                                    </tbody>
                                                    </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                        </div>
                    </div>
                </asp:Panel>
                <div class="right divBtnCadastro area-botoes-bottom">
                    <asp:Button ID="btnSalvar" runat="server" Text="<%$ Resources: Academico, ControleSemanal.Cadastro.btnSalvar.Text %>" OnClick="btnSalvar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources: Academico, ControleSemanal.Cadastro.btnCancelar.Text %>" CausesValidation="false" OnClick="btnCancelar_Click" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>
