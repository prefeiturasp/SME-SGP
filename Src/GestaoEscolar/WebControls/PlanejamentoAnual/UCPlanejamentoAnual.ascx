<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCPlanejamentoAnual.ascx.cs" Inherits="GestaoEscolar.WebControls.PlanejamentoAnual.UCPlanejamentoAnual" %>

<%@ Register Src="~/WebControls/Combos/UCComboOrdenacao.ascx" TagName="UCComboOrdenacao" TagPrefix="uc1" %>

<script type="text/javascript">
    var idTextoGrande = '#<%=txtTextoGrande.ClientID %>';
    var idRetornoTextoGrande = '#<%=txtIdRetornoTexto.ClientID %>';
    var idlblTextoGrandeInfo = '#<%=lblTextoGrandeInfo.ClientID %>';
    var idDdlOrdenacaoAlunos = '#<%=UCComboOrdenacaoAlcance.ComboClientID%>';
    var idtableAlunos = '#<%= grvAlunos.ClientID %>';

    function setaTxtGrande(idTitulo, idTexto) {
        var strTitulo = $('#' + idTitulo).html();
        var strTexto = $('#' + idTexto).val();

        $(idRetornoTextoGrande).val(idTexto);

        $('#ui-dialog-title-divTextoGrande').html(strTitulo);
        $(idTextoGrande).val(strTexto);

        // Limpa mensagem.
        $(idlblTextoGrandeInfo).val('');

        $('#divTextoGrande').dialog('open');
    }

    function abrirTextoGrandeComMensagem(idTitulo, idTexto, mensagem) {

        setaTxtGrande(idTitulo, idTexto);

        // Seta mensagem no label.
        $(idlblTextoGrandeInfo).html(mensagem);
        infoTextarea();

        $(idTextoGrande).focus();
    }

    function abrirTextoGrande(idTitulo, idTexto) {
        setaTxtGrande(idTitulo, idTexto);

        $(idTextoGrande).focus();
    }

    function fecharTextoGrande() {
        var strRetorno = $(idRetornoTextoGrande).val();
        var strTexto = $(idTextoGrande).val();

        $('#' + strRetorno).val(strTexto);
        $('#divTextoGrande').dialog('close');
        $('#' + strRetorno).focus();

        // Limpa mensagem.
        $(idlblTextoGrandeInfo).val('');
        infoTextarea();
    }
</script>

<div id="divTextoGrande" title="Texto Grande" class="hide">
    <fieldset style="width: 75%; height: 75%">
        <div class="textareaComInfo" style="height: 100%;">
            <asp:Label ID="lblTextoGrandeInfo" CssClass="textareaInfo" runat="server"></asp:Label>
            <asp:TextBox ID="txtTextoGrande" runat="server" TextMode="MultiLine" SkinID="limite4000"
                Style="width: 98%; height: 98%"></asp:TextBox>
        </div>
        <asp:HiddenField ID="txtIdRetornoTexto" runat="server" />
        <div class="right">
            <asp:Button ID="btnSalvarTexto" runat="server" Text="Salvar" OnClientClick="fecharTextoGrande(); return false;" />
            <asp:Button ID="btnCancelarTexto" runat="server" CausesValidation="false" Text="Cancelar"
                OnClientClick="$('#divTextoGrande').dialog('close'); return false;" />
        </div>
    </fieldset>
</div>

<asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
<div id="divTabs">
    <asp:HiddenField ID="hdnPlanejamentoAnual" runat="server" />
    <ul class="hide">
        <li>
            <a href="#divTabs-0" id="aCOC0">
                <asp:Label ID="lbltabPlanejamaneto" runat="server" Text="<%$ Resources:WebControls, UCPlanejamentoAnual.lbltabPlanejamaneto.Text %>"></asp:Label>
            </a> 
        </li>
        <asp:Repeater ID="rptPeriodos" runat="server">
            <ItemTemplate>
                <li><a href='#<%# RetornaTabID((int)Eval("tpc_id"))%>'><%# Eval("cap_descricao") %></a></li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
    <div id="divTabs-0" class="divTabs-0">
        <fieldset id="fdsPlanejamentoAnual" runat="server">
            <fieldset>
                <asp:HiddenField ID="hdnPosicaoAnual" runat="server" />
                <asp:Label ID="lblDiagnosticoInicial" runat="server" Text="<%$ Resources:Mensagens, MSG_DIAGNOSTICOINICIAL %>"></asp:Label>
                <asp:ImageButton ID="btnTextoGrandeDiagnosticoInicial" runat="server" SkinID="btEstenderCampo" />
                <div class="textareaComInfo">
                    <asp:Label ID="lblDiagnosticoInicialInfo" CssClass="textareaInfo" runat="server"></asp:Label>
                    <asp:TextBox ID="txtDiagnosticoInicial" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                </div>
                <asp:Label ID="lblTdp_id_Anual" runat="server" Visible="false"></asp:Label>
            </fieldset>
            <fieldset id="fds0COC" runat="server">
                <asp:Label ID="lblInfo" runat="server"></asp:Label>
                <fieldset class="fsdTreeview">
                    <legend style="position: relative;">
                        <asp:Label ID="lblDiagnosticoInicialHabilidades" runat="server" Text="<%$ Resources:WebControls, UCPlanejamentoAnual.lblDiagnosticoInicialHabilidades.Text %>"></asp:Label>
                        <span style="position: absolute; right: 0; top: 4px;"><span style="display: inline-block; width: 265px; text-align: center; border-left: 1px solid #fff;">Alcançado</span> </span>
                    </legend>
                    <div style="clear: both;">
                        <div id="divArvorePlanAnual" class="divArvorePlanAnual">
                        </div>
                        <div class="divTreeviewScrollPlanAnual">
                            <asp:Repeater ID="rptHabilidades" runat="server" OnItemDataBound="rptHabilidades_ItemDataBound">
                                <ItemTemplate>
                                    <asp:Literal ID="litCabecalho" runat="server"></asp:Literal>
                                    <asp:HiddenField ID="hdnPermiteLancamento" runat="server" Value='<%# Eval("PermiteLancamento") %>' />
                                    <asp:HiddenField ID="hdnPosicao" runat="server" Value='<%# Eval("Posicao") %>' />
                                    <span style="padding-right: 245px; display: block;">
                                        <asp:Literal ID="litConteudo" runat="server"></asp:Literal></span>
                                    <div style="display: table-row;" id="divHabilidade" runat="server">
                                        <asp:HiddenField ID="hdnChave" runat="server" Value='<%# Eval("Chave") %>' />
                                        <span style="display: table-cell; text-align: left; vertical-align: top;">
                                            <asp:Literal ID="lblHabilidade" runat="server"></asp:Literal>
                                        </span><span style="display: table-cell; width: 245px; text-align: center; vertical-align: top; vertical-align: middle;">
                                            <asp:CheckBox ID="chkAlcancado" runat="server" Checked='<%# Eval("Alcancado") %>'></asp:CheckBox><br />
                                            <asp:Label ID="lblLegendaDiagInicialOrientacao" runat="server" Text="" Visible="true" 
                                                CssClass="nivelAprendizado"></asp:Label>
                                        </span>
                                    </div>
                                    <asp:Literal ID="litRodape" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </fieldset>
                <asp:Label ID="lblMsgMarcarAlcancadoAnoAnterior" runat="server"></asp:Label>
                <br />
                <div id="divLegendaNivelApDiagIni" runat="server" style="width: 300px; padding: 6px; float: left;" visible="false">
	                <b>Níveis de aprendizado:</b>
	                <div style="border-style: solid; border-width: thin;">
		                <table id="tbLegendaNivelAprendizado2" style="border-collapse: separate !important; border-spacing: 2px !important;">
			                <asp:Repeater ID="rptLegendaNiveisAprend" runat="server">
				                <ItemTemplate>
					                <tr>
						                <td>
							                <asp:Label ID="lblLegendaNivelAprend" runat="server" Text='<%# Bind("nivelAprendizado") %>'></asp:Label></td>
					                </tr>
				                </ItemTemplate>
			                </asp:Repeater>
		                </table>
	                </div>
	                <br />
                </div>
            </fieldset>
            <fieldset>                                               
                <asp:Label ID="lblPlanejamento" runat="server" Text="<%$ Resources:Mensagens, MSG_PROPOSTAMETODOLOGICA %>"></asp:Label>
                <asp:ImageButton ID="btnTextoGrandePlanejamento" runat="server" SkinID="btEstenderCampo" />
                <div class="textareaComInfo">
                    <asp:Label ID="lblPlanejamentoInfo" CssClass="textareaInfo" runat="server"></asp:Label>
                    <asp:TextBox ID="txtPlanejamento" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox><br />
                </div>
            </fieldset>
            <fieldset>
                <asp:Label ID="lblAvaliacaoTrabalho" runat="server" Text="<%$ Resources:Mensagens, MSG_AVALIACAO  %>"></asp:Label>
                <asp:ImageButton ID="btnTextoGrandeAvaliacaoTrabalho" runat="server" SkinID="btEstenderCampo" />
                <div class="textareaComInfo">
                    <asp:Label ID="lblAvaliacaoTrabalhoInfo" CssClass="textareaInfo" runat="server"></asp:Label>
                    <asp:TextBox ID="txtAvaliacaoTrabalho" runat="server" Style="overflow: auto;" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox><br />
                </div>
            </fieldset>
        </fieldset>
    </div>
    <asp:Repeater ID="rptPeriodosAbas" runat="server" OnItemDataBound="rptPeriodosAbas_ItemDataBound">
        <ItemTemplate>
            <div id='<%# RetornaTabID((int)Eval("tpc_id"))%>'>
                <asp:HiddenField ID="hdnTpcId" runat="server" Value='<%# Eval("tpc_id") %>' />
                <asp:HiddenField ID="hdnTpcOrdem" runat="server" Value='<%# Eval("tpc_ordem") %>' />
                <asp:HiddenField ID="hdnPosicao" runat="server" Value='<%# Eval("tdt_posicao") %>' />
                <fieldset id="fdsCOC" runat="server">
                    <div id="divCOC" runat="server">
                        <fieldset id="fdsHabilidadesCOC" runat="server" class="fsdTreeview">
                            <asp:Label ID="lblMensagemHabilidade" runat="server" CssClass="Info"></asp:Label>
                            <legend style="position: relative;"><span id="spanOrientacao" runat="server">Objetivos, conteúdos e habilidades das orientações
                                curriculares</span> <span style="position: absolute; right: 0; top: 4px;"><span style="display: inline-block; width: 115px; text-align: center; border-right: 1px solid white; border-left: 1px solid #fff;">Planejada</span> <span style="display: inline-block; width: 115px; text-align: center; border-right: 1px solid white">Trabalhada</span>
                                    <span style="display: inline-block; width: 120px; text-align: center;">Alcançada</span>
                                </span></legend>
                            <div style="clear: both;">
                                <div class="divTreeLayoutBim">
                                </div>
                                <div class="divTreeviewScrollPlanBim">
                                    <asp:Repeater ID="rptHabilidadesCOC" runat="server" OnItemDataBound="rptHabilidades_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:Literal ID="litCabecalho" runat="server"></asp:Literal>
                                            <asp:HiddenField ID="hdnPermiteLancamento" runat="server" Value='<%# Eval("PermiteLancamento") %>' />
                                            <asp:HiddenField ID="hdnPosicao" runat="server" Value='<%# Eval("Posicao") %>' />
                                            <asp:Literal ID="litConteudo" runat="server"></asp:Literal>
                                            <div style="display: table-row;" id="divHabilidade" runat="server">
                                                <asp:HiddenField ID="hdnChave" runat="server" />
                                                <span style="display: table-cell; text-align: left; vertical-align: top;">
                                                    <asp:Literal ID="lblHabilidade" runat="server"></asp:Literal>
                                                </span>
                                                <span style="display: table-cell; width: 120px; text-align: center; vertical-align: top; vertical-align: middle;">
                                                    <asp:CheckBox ID="chkPlanejado" runat="server" Checked='<%# Eval("Planejado") %>'></asp:CheckBox><br />
                                                    <asp:Label ID="lblLegendaPlanejado" runat="server" Text="" Visible="false"  CssClass="nivelAprendizado"></asp:Label>
                                                </span><span style="display: table-cell; width: 120px; text-align: center; vertical-align: top; vertical-align: middle;">
                                                    <asp:CheckBox ID="chkTrabalhado" runat="server" Checked='<%# Eval("Trabalhado") %>'></asp:CheckBox><br />
                                                    <asp:Label ID="lblLegendaTrabalhado" runat="server" Text="" Visible="false" CssClass="nivelAprendizado"></asp:Label>
                                                </span>
                                                <span style="display: table-cell; width: 115px; text-align: center; vertical-align: top; vertical-align: middle;">
                                                    <div id="divMarcarAlcancado" style="display: inline; padding: 0 0 0 0;">
                                                        <asp:ImageButton ID="imgMarcarAlcancado" runat="server" SkinID="btDetalhar" CausesValidation="false" OnClick="imgMarcarAlcancado_Click" />
                                                        <asp:Image ID="imgSituacaoEfetivada" runat="server" SkinID="imgConfirmar" ToolTip="Preenchimento de alcance efetivado" Visible="false" />
                                                        <asp:Image ID="imgSituacao" runat="server" SkinID="imgConfirmarAmarelo" ToolTip="Preenchimento de alcance realizado" Visible="false"/>
                                                        <br />
                                                        <asp:Label ID="lblLegendaAlcancado" runat="server" Text="" Visible="false"  CssClass="nivelAprendizado"></asp:Label>
                                                    </div>
                                                </span>
                                            </div>
                                            <asp:Literal ID="litRodape" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </fieldset>
                        <asp:Label ID="lblMsgMarcarHabilidades" runat="server" Text='<%# MSTech.CoreSSO.BLL.UtilBO.GetErroMessage("Marcar as habilidades planejadas a serem trabalhadas durante o COC.", MSTech.CoreSSO.BLL.UtilBO.TipoMensagem.Informacao) %>'></asp:Label>
                        <fieldset id="fdsLegendaNaoAlcancadasCOC" runat="server">
                            <div id="divLegendaNivelAprendizado" runat="server" style="width: 300px; padding: 6px; float: left;" visible="false">
                                <b>Níveis de aprendizado:</b>
                                <div style="border-style: solid; border-width: thin;">
                                    <table id="tbLegendaNivelAprendizado" style="border-collapse: separate !important; border-spacing: 2px !important;">
                                        <asp:Repeater ID="rptLegendaNivelAprendizado" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblLegendaNivelAprendizado" runat="server" Text='<%# Bind("nivelAprendizado") %>'></asp:Label></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                                <br />
                            </div>
                            <div style="float: left; padding-left: 10px; padding-top: 6px; padding-bottom: 6px;">
                                <b>Legenda:</b>
                                <div id="divLegenda" runat="server" style="border-style: solid; border-width: thin; width: 400px;">
                                    <table id="tbLegenda" runat="server" style="border-collapse: separate !important; border-spacing: 2px !important;">
                                        <tr>
                                            <td height="15px" width="25px"></td>
                                            <td>
                                                <span id="spanHabilidadePlanej" runat="server">Habilidades planejadas em bimestres anteriores</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="15px" width="25px"></td>
                                            <td>
                                                <span id="spanHabilidadeTrab" runat="server">Habilidades trabalhadas em bimestres anteriores</span>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                                        <asp:Label ID="lblDiagnosticoCOC" runat="server"></asp:Label>
                                        <asp:ImageButton ID="btnTextoGrandeDiagnosticoCOC" runat="server" SkinID="btEstenderCampo" />
                                        <div class="textareaComInfo">
                                            <asp:Label ID="lblDiagnosticoCOCInfo" CssClass="textareaInfo" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtDiagnosticoCOC" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                                        </div>
                                        <asp:Label ID="lblTdp_id_COC" runat="server" Visible="false" Text='<%# Eval("tdp_id") %>'></asp:Label>
                        </fieldset>
                        <fieldset>
                            <asp:Label ID="lblPlanejamentoCOC" runat="server"></asp:Label>
                            <asp:ImageButton ID="btnTextoGrandePlanejamentoCOC" runat="server" SkinID="btEstenderCampo" />
                            <div class="textareaComInfo">
                                <asp:Label ID="lblPlanejamentoCOCInfo" CssClass="textareaInfo" runat="server"></asp:Label>
                                <asp:TextBox ID="txtPlanejamentoCOC" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox><br />
                            </div>
                        </fieldset>
                        <fieldset>
                            <asp:Label ID="lblRecursosCOC" runat="server" Text="<%$ Resources:Mensagens, MSG_RECURSOSBIMESTRE %>"></asp:Label>
                            <asp:ImageButton ID="btnTextoGrandeRecursosCOC" runat="server" SkinID="btEstenderCampo" />
                            <div class="textareaComInfo">
                                <asp:Label ID="lblRecursosCOCInfo" CssClass="textareaInfo" runat="server"></asp:Label>
                                <asp:TextBox ID="txtRecursosCOC" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox><br />
                            </div>
                        </fieldset>
                        <fieldset>
                            <asp:Label ID="lblIntervencoesPedagogicasCOC" runat="server" Text="<%$ Resources:Mensagens, MSG_INTERVENCOESPEDAGOGICASBIMESTRE %>"></asp:Label>
                            <asp:ImageButton ID="btnTextoGrandeIntervencoesPedagogicasCOC" runat="server" SkinID="btEstenderCampo" />
                            <div class="textareaComInfo">
                                <asp:Label ID="lblIntervencoesPedagogicasCOCInfo" CssClass="textareaInfo" runat="server"></asp:Label>
                                <asp:TextBox ID="txtIntervencoesPedagogicasCOC" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox><br />
                            </div>
                        </fieldset>
                        <fieldset>
                            <asp:Label ID="lblRegistroIntervencoesCOC" runat="server" Text="<%$ Resources:Mensagens, MSG_REGISTROINTERVENCOESBIMESTRE %>"></asp:Label>
                            <asp:ImageButton ID="btnTextoGrandeRegistroIntervencoesCOC" runat="server" SkinID="btEstenderCampo" />
                            <div class="textareaComInfo">
                                <asp:Label ID="lblRegistroIntervencoesCOCInfo" CssClass="textareaInfo" runat="server"></asp:Label>
                                <asp:TextBox ID="txtRegistroIntervencoesCOC" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox><br />
                            </div>
                        </fieldset>
                    </div>
                </fieldset>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <asp:HiddenField ID="selected_tab" runat="server" />
</div>

<div id="divLancamentoAlcance" runat="server" title="Lançamento de alcance de objetivos de aprendizagem" class="divLancamentoAlcance hide">
    <asp:Label ID="lblMensagemAlcance" runat="server" EnableViewState="false"></asp:Label>
    <asp:Panel ID="pnlAlcance" runat="server">
        <uc1:UCComboOrdenacao ID="UCComboOrdenacaoAlcance" runat="server" />
        <asp:Label ID="lblInfoAlcance" runat="server"></asp:Label>
        <asp:Label ID="lblOrientacaoCurricular" runat="server"></asp:Label>
        <asp:GridView ID="grvAlunos" runat="server" AutoGenerateColumns="false" OnRowDataBound="grvAlunos_RowDataBound"
            DataKeyNames="tud_id,alu_id,mtu_id,mtd_id,ocr_id,aha_id,tpc_id" EmptyDataText="A pesquisa não encontrou resultados.">
            <Columns>
                <asp:BoundField HeaderText="Nº Chamada" DataField="numeroChamada" />
                <asp:BoundField HeaderText="Nome do aluno" DataField="nomeAluno" />
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblHeaderAlcance" runat="server" Text="Não alcançou"></asp:Label>
                        <br />
                        <br />
                        <asp:CheckBox ID="chkEfetivado" runat="server" Text="Efetivado" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkAlcancado" runat="server" Checked='<%# Bind("aha_alcancada") %>' />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                    <ItemStyle CssClass="center" HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <div class="right">
        <asp:Button ID="btnSalvarAlcance" runat="server" Text="Salvar" OnClick="btnSalvarAlcance_Click" />
        <asp:Button ID="btnCancelarAlcance" runat="server" Text="Cancelar" CausesValidation="false" OnClientClick="$('.divLancamentoAlcance').dialog('close');" />
    </div>
</div>
