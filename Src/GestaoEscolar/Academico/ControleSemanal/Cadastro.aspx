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
                    <td style="padding-top: 50px; width: 20px; vertical-align: bottom;" id="td_lkbAnterior" runat="server">
                        <asp:LinkButton Style="zoom: 140%; -moz-transform: scale(1.40);" ID="lkbAnterior" Text="|<" runat="server" OnClick="lkbAnterior_Click"
                            CssClass="ui-icon ui-icon-circle-triangle-w"></asp:LinkButton>
                    </td>
                    <td style="padding-top: 30px; vertical-align: bottom;">
                        <asp:Label ID="lblInicio" runat="server" Text="DD/MM/YYYY" AssociatedControlID="lkbProximo"
                            Font-Bold="True" class="lblInicio"/>
                    </td>
                    <td style="padding-top: 45px; vertical-align: bottom;">
                        <asp:Label ID="Label3" runat="server" Text=" - " AssociatedControlID="lkbProximo"
                            Font-Bold="True" />
                    </td>
                    <td class="clear"></td>
                    <td style="padding-top: 44px; vertical-align: bottom;">
                        <asp:Label ID="lblFim" runat="server" Text="DD/MM/YYYY" AssociatedControlID="lkbProximo"
                            Font-Bold="True" class="lblFim"/>
                    </td>
                    <td style="padding-top: 50px; width: 50px; vertical-align: bottom;" id="td_lkbProximo" runat="server">
                        <asp:LinkButton Style="zoom: 140%; -moz-transform: scale(1.40);" ID="lkbProximo" Text=">|" runat="server" OnClick="lkbProximo_Click"
                            CssClass="ui-icon ui-icon-circle-triangle-e"/>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        
        
        
    </fieldset>

</asp:Content>
