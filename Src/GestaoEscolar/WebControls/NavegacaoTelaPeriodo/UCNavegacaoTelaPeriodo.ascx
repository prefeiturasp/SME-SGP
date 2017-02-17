<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCNavegacaoTelaPeriodo.ascx.cs" Inherits="GestaoEscolar.WebControls.NavegacaoTelaPeriodo.UCNavegacaoTelaPeriodo" %>

<script type="text/javascript">
    var opcao;

    $(document).ready(function() {
        opcao = <%=(byte)VS_opcaoAbaAtual%>;

        $('.opcoes.mapDireita > span[class^="btn"]').children('.opcao_selecionada').parent().addClass('selecionado');
    });

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

    function SetaProximaOpcao(op) {
        opcao = op;
    }
</script>

<div runat="server" id="divBotoesNavegacao" class="planEscopo" style="text-align: right;">
    <div class="opcoes mapDireita">
        <%--<asp:Button ID="btnIndicadores" runat="server" Text="Dashboard indicadores" OnClick="btnIndicadores_Click" />--%>
        <span class="btnPlanejamentoAnual"><asp:Button ID="btnPlanejamentoAnual" runat="server" Text="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnPlanejamentoAnual.Text %>" ToolTip="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnPlanejamentoAnual.Text %>" OnClientClick="SetaProximaOpcao(1);" OnClick="btnPlanejamentoAnual_Click" /></span>
        <span class="btnDiarioClasse"><asp:Button ID="btnDiarioClasse" runat="server" Text="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnDiarioClasse.Text %>" ToolTip="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnDiarioClasse.Text %>" CssClass="opcao_selecionada" OnClientClick="SetaProximaOpcao(2);" OnClick="btnDiarioClasse_Click" /></span>
        <span class="btnListao"><asp:Button ID="btnListao" runat="server" Text="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnListao.Text %>" ToolTip="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnListao.Text %>" OnClientClick="SetaProximaOpcao(3);" OnClick="btnListao_Click" /></span>
        <span class="btnFrequencia"><asp:Button ID="btnFrequencia" runat="server" Text="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnFrequencia.Text %>" ToolTip="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnFrequencia.Text %>" OnClientClick="SetaProximaOpcao(7);" OnClick="btnFrequencia_Click" /></span>
        <span class="btnAvaliacao"><asp:Button ID="btnAvaliacao" runat="server" Text="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnAvaliacao.Text %>" ToolTip="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnAvaliacao.Text %>" OnClientClick="SetaProximaOpcao(8);" OnClick="btnAvaliacao_Click" /></span> 
        <span class="btnEfetivacao"><asp:Button ID="btnEfetivacao" runat="server" Text="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnEfetivacao.Text %>" ToolTip="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnEfetivacao.Text %>" OnClientClick="SetaProximaOpcao(4);" OnClick="btnEfetivacao_Click" /></span>
        <span class="btnAlunos"><asp:Button ID="btnAlunos" runat="server" Text="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnAlunos.Text %>" ToolTip="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnAlunos.Text %>" OnClientClick="SetaProximaOpcao(5);" OnClick="btnAlunos_Click" /></span>
        <span class="btnVoltar"><asp:Button ID="btnVoltar" runat="server" Text="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnVoltar.Text %>" ToolTip="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnVoltar.Text %>" OnClientClick="SetaProximaOpcao(0);" OnClick="btnVoltar_Click" /></span> 
    </div>
    <asp:Label ID="lblTitulo" runat="server" style="display:none;" CssClass="opcoes-titulo"></asp:Label>
    <div class="mapDireita opcoes-bimestre">
        <asp:Repeater ID="rptPeriodo" runat="server" OnItemDataBound="rptPeriodo_ItemDataBound">
            <ItemTemplate>
                <span class="botao-periodo">
                    <asp:Button ID="btnPeriodo" runat="server" Text='<%# Eval("cap_descricao") %>' 
                        OnClick="btnPeriodo_Click" OnClientClick="SetaAvaliacao(this);"/>
                    <asp:HiddenField ID="hdnPeriodo" runat="server" Value='<%# Eval("tpc_id") %>' />
                    <asp:HiddenField ID="hdnPeriodoOrdem" runat="server" Value='<%# Eval("tpc_ordem") %>' />
                    <asp:Label ID="lblNomeAbreviado" runat="server" Text='<%# Eval("tpc_nomeAbreviado") %>' style="display:none;" CssClass="abbr-periodo"/>
                    <asp:HiddenField ID="hdnIdAvaliacao" runat="server" />
                    <asp:HiddenField ID="hdnAvaliacaoTipo" runat="server" />
                </span>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div id="divDadosFechamento">
        <asp:HiddenField ID="hdnTudId" runat="server" />
        <asp:HiddenField ID="hdnTurId" runat="server" />
        <asp:HiddenField ID="hdnTpcId" runat="server" />
        <asp:HiddenField ID="hdnAvaId" runat="server" />
        <asp:HiddenField ID="hdnFavId" runat="server" />
        <asp:HiddenField ID="hdnTipoAvaliacao" runat="server" />
        <asp:HiddenField ID="hdnEsaId" runat="server" />
        <asp:HiddenField ID="hdnTipoEscala" runat="server" />
        <asp:HiddenField ID="hdnTipoEscalaDocente" runat="server" />
        <asp:HiddenField ID="hdnNotaMinima" runat="server" />
        <asp:HiddenField ID="hdnParecerMinimo" runat="server" />
        <asp:HiddenField ID="hdnTipoLancamento" runat="server" />
        <asp:HiddenField ID="hdnCalculoQtAulasDadas" runat="server" />
        <asp:HiddenField ID="hdnTurTipo" runat="server" />
        <asp:HiddenField ID="hdnCalId" runat="server" />
        <asp:HiddenField ID="hdnTudTipo" runat="server" />
        <asp:HiddenField ID="hdnTpcOrdem" runat="server" />
        <asp:HiddenField ID="hdnVariacao" runat="server" />
        <asp:HiddenField ID="hdnTipoDocente" runat="server" />
        <asp:HiddenField ID="hdnDisciplinaEspecial" runat="server" />
        <asp:HiddenField ID="hdnFechamentoAutomatico" runat="server" />
        <asp:HiddenField ID="hdnProcessarFilaFechamentoTela" runat="server" />
    </div>
</div>