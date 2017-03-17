<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCPlanejamentoProjetos.ascx.cs" Inherits="GestaoEscolar.WebControls.PlanejamentoAnual.UCPlanejamentoProjetos" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCComboGenerico.ascx" TagName="UCComboGenerico" TagPrefix="uc1" %>

<script type="text/javascript">
    //var idTextoGrande = '#<%--<%=txtTextoGrande.ClientID %>--%>';
    //var idRetornoTextoGrande = '#<%--<%=txtIdRetornoTexto.ClientID %>--%>';
    //var idlblTextoGrandeInfo = '#<%--<%=lblTextoGrandeInfo.ClientID %>--%>';

    function setaTxtPequeno(idTitulo, idTexto, idBotaoExpandir, idBotaoDiminuir, txtVisivel) {
        var strTitulo = $('#' + idTitulo).html();
        var strTexto = $('#' + idTexto).val();

        $('#' + idTexto).css('width', '480px');
        $('#' + idTexto).css('height', '100px');

        $('#' + idTexto).val(strTexto);

        if (txtVisivel == "true") {
            $('#' + idTexto).css('display', '');
        } else {
            $('#' + idTexto).css('display', 'none');
        }

        var desabilitadoEdicao = $('#' + idTexto).is('[readonly]');
        if (desabilitadoEdicao) {
            //$(idTextoGrande).attr('readonly', 'readonly');
            //$("input[id$='btnSalvarTexto']").css('display', 'none');
            $('#' + idTexto).attr('readonly', 'readonly');
        }
        else {
            //$(idTextoGrande).removeAttr('readonly');
            //$("input[id$='btnSalvarTexto']").css('display', 'inline-block');
            $('#' + idTexto).removeAttr('readonly');
        }

        $('#' + idBotaoExpandir).css('display', '');
        $('#' + idBotaoDiminuir).css('display', 'none');
    }

    function setaTxtGrande(idTitulo, idTexto, idBotaoExpandir, idBotaoDiminuir) {
        var strTitulo = $('#' + idTitulo).html();
        var strTexto = $('#' + idTexto).val();

        $('#' + idTexto).css('width', '99%');
        $('#' + idTexto).css('height', '300px');

        $('#' + idTexto).css('display', '');

        //$(idRetornoTextoGrande).val(idTexto);

        //$('#ui-dialog-title-divTextoGrande').html(strTitulo);
        //$(idTextoGrande).val(strTexto);

        $('#' + idTexto).val(strTexto);

        //Limpa mensagem.
        //$(idlblTextoGrandeInfo).val('');

        var desabilitadoEdicao = $('#' + idTexto).is('[readonly]');
        if (desabilitadoEdicao) {
            //$(idTextoGrande).attr('readonly', 'readonly');
            //$("input[id$='btnSalvarTexto']").css('display', 'none');
            $('#' + idTexto).attr('readonly', 'readonly');
        }
        else {
            //$(idTextoGrande).removeAttr('readonly');
            //$("input[id$='btnSalvarTexto']").css('display', 'inline-block');
            $('#' + idTexto).removeAttr('readonly');
        }

        //$('#divTextoGrande').dialog('open');

        $('#' + idBotaoExpandir).css('display', 'none');
        $('#' + idBotaoDiminuir).css('display', '');
    }

    function abrirTextoGrandeComMensagem(idTitulo, idTexto, mensagem, idBotaoExpandir, idBotaoDiminuir) {
        setaTxtGrande(idTitulo, idTexto, idBotaoExpandir, idBotaoDiminuir);

        infoTextarea();

        $('#' + idTexto).focus();
    }

    function abrirTextoGrande(idTitulo, idTexto, idBotaoExpandir, idBotaoDiminuir) {

        setaTxtGrande(idTitulo, idTexto, idBotaoExpandir, idBotaoDiminuir);

        $('#' + idTexto).focus();
    }

    function abrirTextoPequenoComMensagem(idTitulo, idTexto, mensagem, idBotaoExpandir, idBotaoDiminuir, txtVisivel) {
        
        setaTxtPequeno(idTitulo, idTexto, idBotaoExpandir, idBotaoDiminuir, txtVisivel);

        infoTextarea();

        $('#' + idTexto).focus();
    }

    function abrirTextoPequeno(idTitulo, idTexto, idBotaoExpandir, idBotaoDiminuir, txtVisivel) {

        setaTxtPequeno(idTitulo, idTexto, idBotaoExpandir, idBotaoDiminuir, txtVisivel);

        $('#' + idTexto).focus();
    }

    //function fecharTextoGrande() {
    //    var strRetorno = $(idRetornoTextoGrande).val();
    //    var strTexto = $(idTextoGrande).val();

    //    $('#' + strRetorno).val(strTexto);
    //    $('#divTextoGrande').dialog('close');
    //    $('#' + strRetorno).focus();

    //    // Limpa mensagem.
    //    $(idlblTextoGrandeInfo).val('');
    //    infoTextarea();
    //}
</script>

<%--<div id="divTextoGrande" title="Texto Grande" class="hide">
    <fieldset style="width: 75%; height: 75%">
        <div class="textareaComInfo" style="height: 100%;">
            <asp:Label ID="lblTextoGrandeInfo" CssClass="textareaInfo" runat="server"></asp:Label>
            <asp:TextBox ID="txtTextoGrande" runat="server" TextMode="MultiLine" SkinID="limite4000"
                Style="width: 98%; height: 98%"></asp:TextBox>
        </div>
        <asp:HiddenField ID="txtIdRetornoTexto" runat="server" />
        <div class="right">
            <asp:Button ID="btnSalvarTexto" runat="server" Text="Salvar" 
                OnClientClick="fecharTextoGrande(); return false;" />
            <asp:Button ID="btnCancelarTexto" runat="server" CausesValidation="false" Text="Cancelar"
                OnClientClick="$('#divTextoGrande').dialog('close'); return false;" />
        </div>
    </fieldset>
</div>--%>
<div runat="server" id="divPlanoAluno" class="hide divPlanoAluno">
    
    <asp:TextBox ID="txtPlanoAlunoPopUp" runat="server" ReadOnly="True"></asp:TextBox>
    <div class="right">
        <asp:Button ID="btnFecharPlanoAluno" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.btnFecharPlanoAluno.Text %>" OnClick="btnFecharPlanoAluno_Click" />
    </div>
</div>
<div id="divReplicarPlanejamentoAnual" title="Replicar planejamento" class="hide">
    <asp:UpdatePanel ID="updReplicarPlanejamentoAnual" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblMessageReplicar" runat="server" EnableViewState="false"></asp:Label>
            <fieldset>
                <asp:Label ID="lblMensagemReplicacaoAnual" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.lblMensagemReplicacaoAnual.Text %>"></asp:Label>
                <br />
                <br />
                <asp:CheckBoxList ID="chkTurmas" runat="server" DataTextField="tur_codigo" DataValueField="tud_id"></asp:CheckBoxList>
                <div class="right">
                    <asp:Button ID="btnReplicar" runat="server" Text="Replicar" OnClick="btnReplicar_Click" CausesValidation="false" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false" OnClientClick='$("#divReplicarPlanejamentoAnual").dialog("close"); return false;' />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div id="divTabs">
    <ul class="hide">
        <li runat="server" id="abaPlanoCiclo"><a href="#<%= divTabsPlanoCiclo.ClientID %>">
            <asp:Literal ID="litPlanoCiclo" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.litPlanoCiclo.Text %>"></asp:Literal></a></li>
        <li runat="server" id="abaPlanoAnual"><a href="#<%= divTabsPlanoAnual.ClientID %>">
            <asp:Literal ID="litPlanoAnual" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.litPlanoAnual.Text %>"></asp:Literal></a></li>
        <li runat="server" id="abaPlanoAluno"><a href="#<%= divTabsPlanoAluno.ClientID %>">
            <asp:Literal ID="litPlanoAluno" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.litPlanoAluno.Text %>"></asp:Literal></a></li>
        <%--<li><a href="#divTabs-Projetos">
            <asp:Literal ID="litProjetos" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.litProjetos.Text %>"></asp:Literal></a></li>--%>
        <li><a href="#divTabs-Documentos">
            <asp:Literal ID="litDocumentos" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.litDocumentos.Text %>"></asp:Literal></a></li>
        <li runat="server" id="abaobjAprendizagem"><a href="#<%= divTabsObjetoAprendizagem.ClientID %>">
            <asp:Literal ID="litObjetoAprendizagem" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.litObjetoAprendizagem.Text %>"></asp:Literal></a></li>
    </ul>
    <div id="divTabsPlanoCiclo" runat="server">
        <asp:UpdatePanel ID="updCiclo" runat="server">
            <ContentTemplate>
                <fieldset>
                    <asp:ValidationSummary ID="vsPlanoCiclo" runat="server" ValidationGroup="vgPlanoCiclo" />
                    <asp:Label ID="lblMensagemCiclo" runat="server" EnableViewState="false"></asp:Label>
                    <span id="spTituloCiclo" style="display: inline-block; font-size: large; font-weight: bold;">
                        <asp:Label ID="lblCiclo" runat="server"></asp:Label>
                        (<asp:LinkButton ID="lbkAlterarCiclo" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.lbkAlterarCiclo.Text %>"
                            OnClientClick="$('#spTrocarCiclo').css('display','inline-block');$('#spTrocarCiclo').find('select').focus();$('#spTituloCiclo').css('display','none');return false;">
                        </asp:LinkButton>)
                    </span>
                    <span id="spTrocarCiclo" class="hide">
                        <uc1:UCComboGenerico ID="uccTipoCiclo" runat="server"
                            MostrarMensagemSelecione="false" Obrigatorio="false"
                            ValorItemVazio="-1;-1;-1;-1" ConfirmExit="true"></uc1:UCComboGenerico>
                        <asp:ImageButton ID="btnCancelarAlterarCiclo" ToolTip="<%$ Resources:UserControl, UCPlanejamentoProjetos.btnCancelarAlterarCiclo.ToolTip %>" runat="server"
                            SkinID="btDesfazer" OnClientClick="$('#spTrocarCiclo').css('display','none');$('#spTituloCiclo').css('display','inline-block');return false;" />
                    </span>
                    <br />
                    <br />
                    <asp:Label runat="server" ID="lblMsgTodosProfessores" Text="<%$ Resources:WebControls, PlanejamentoAnual.UCPlanejamentoProjetos.lblMsgTodosProfessores.Text %>"
                        Style="<%$ Resources:WebControls, PlanejamentoAnual.UCPlanejamentoProjetos.lblMsgTodosProfessores.Style %>"></asp:Label>
                    <CKEditor:CKEditorControl ID="txtPlanoCiclo" BasePath="/includes/ckeditor/" runat="server" Enabled="false"></CKEditor:CKEditorControl>
                    <asp:RequiredFieldValidator ID="rfvPlanoCiclo" runat="server" ControlToValidate="txtPlanoCiclo" Display="Dynamic" ErrorMessage="Plano do ciclo é obrigatório." ValidationGroup="vgPlanoCiclo">*</asp:RequiredFieldValidator>
                    <br />
                    <div id="divAlteracaoCiclo" style="vertical-align: central;">
                        <asp:Label ID="lblUsuarioCiclo" runat="server"></asp:Label>
                        <asp:ImageButton ID="imgHistoricoAlteracaoCiclo" runat="server" SkinID="btDetalhar" ToolTip="<%$ Resources:UserControl, UCPlanejamentoProjetos.imgHistoricoAlteracaoCiclo.ToolTip %>"
                            OnClientClick="$('#divHistoricoAlteracoesCiclo').dialog('open'); return false;" />
                    </div>
                </fieldset>
                <div class="right" runat="server" id="divEditarPlanoCiclo">
                    <asp:Button ID="btnCancelarCiclo" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.btnCancelarCiclo.Text %>" Visible="false" OnClick="btnCancelarCiclo_Click" CausesValidation="false" />
                    <asp:Button ID="btnEditarPlanoCiclo" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.btnEditarPlanoCiclo.Text %>" OnClick="btnEditarPlanoCiclo_Click" CausesValidation="false" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divTabsPlanoAnual" runat="server">
        <asp:UpdatePanel ID="updAnual" runat="server">
            <ContentTemplate>
                <fieldset>
                    <asp:Label ID="lblMensagemAnual" runat="server" EnableViewState="false"></asp:Label>
                    <asp:Label ID="lblDiagnosticoInicial" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.lblDiagnosticoInicial.Text %>"></asp:Label>
                    <asp:ImageButton ID="btnTextoGrandeDiagnosticoInicial" runat="server" SkinID="btnExpandirCampo" />
                    <asp:ImageButton ID="btnVoltaEstadoAnteriorTextoDiagnosticoInicial" runat="server" SkinID="btnComprimirCampo" Style="display: none" />
                    <div class="textareaComInfo">
                        <asp:Label ID="lblDiagnosticoInicialInfo" CssClass="textareaInfo" runat="server"></asp:Label>
                        <asp:TextBox ID="txtDiagnosticoInicial" runat="server" TextMode="MultiLine" SkinID="text60C"></asp:TextBox>
                    </div>
                    <asp:Label ID="lblProposta" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.lblProposta.Text %>"></asp:Label>
                    <asp:ImageButton ID="btnTextoGrandeProposta" runat="server" SkinID="btnExpandirCampo" />
                    <asp:ImageButton ID="btnVoltaEstadoAnteriorTextoProposta" runat="server" SkinID="btnComprimirCampo" Style="display: none" />
                    <div class="textareaComInfo">
                        <asp:Label ID="lblPropostaInfo" CssClass="textareaInfo" runat="server"></asp:Label>
                        <asp:TextBox ID="txtProposta" runat="server" TextMode="MultiLine" SkinID="text60C"></asp:TextBox><br />
                    </div>
                    <fieldset>
                        <asp:Repeater ID="rptPlanejamentoBimestre" runat="server" OnItemDataBound="rptPlanejamentoBimestre_ItemDataBound">
                            <ItemTemplate>
                                <asp:Label ID="lblPlanejamentoBimestre" runat="server" Text='<%# Eval("cap_descricao") %>'></asp:Label>
                                <asp:ImageButton ID="btnTextoGrandePlanejamentoBimestre" runat="server" SkinID="btnExpandirCampo" />
                                <asp:ImageButton ID="btnVoltaEstadoAnteriorTextoPlanejamentoBimestre" runat="server" SkinID="btnComprimirCampo" Style="display: none" />
                                <div class="textareaComInfo">
                                    <asp:Label ID="lblPlanejamentoBimestreInfo" CssClass="textareaInfo" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtPlanejamentoBimestre" runat="server" TextMode="MultiLine" SkinID="text60C" 
                                        Text='<%# Eval("tdp_planejamento") %>'
                                        Enabled='<%#PermiteEdicao %>'></asp:TextBox><br />
                                    <asp:HiddenField ID="hdnBimestreVisivel" runat="server" Value="true" />
                                </div>
                                <asp:HiddenField ID="hdnTdpId" runat="server" Value='<%# Eval("tdp_id") %>' />
                                <asp:HiddenField ID="hdnTpcId" runat="server" Value='<%# Eval("tpc_id") %>' />
                            </ItemTemplate>
                        </asp:Repeater>
                    </fieldset>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divTabsPlanoAluno" runat="server">
        <asp:UpdatePanel ID="updAluno" runat="server">
            <ContentTemplate>
                <fieldset>
                    <asp:Label ID="lblMensagemAluno" runat="server" EnableViewState="false"></asp:Label>
                    <asp:Label ID="lblMensagemAviso" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.lblMensagemAviso.Text %>"
                        Font-Bold="true" ForeColor="#A52A2A"></asp:Label>
                    <br />
                    <br /> 
                    <asp:Label ID="lblSelecioneAluno" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.lblSelecioneAluno.Text %>"></asp:Label>
                    <uc1:UCComboGenerico ID="UCCAlunos" runat="server"
                        MostrarMensagemSelecione="true" Obrigatorio="false"
                        ValorItemVazio="-1"></uc1:UCComboGenerico>
                    <div runat="server" id="divAluno" visible="false">
                        <asp:CheckBoxList ID="chlTurmaDiscRelacionada" runat="server" DataTextField="dis_nome" 
                            DataValueField="tud_idRelacionado" 
                            Enabled='<%#PermiteEdicao %>'
                            RepeatDirection="Horizontal"></asp:CheckBoxList>
                        <asp:Label ID="lblDescTurmaAluno" runat="server"></asp:Label>
                        <CKEditor:CKEditorControl ID="txtPlanoAluno" BasePath="/includes/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                        <fieldset runat="server" id="fdsTurmasAluno" visible="false">
                            <asp:Label ID="lblTurmaAluno" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.lblTurmaAluno.Text %>"></asp:Label>
                            <uc1:UCComboGenerico ID="UCCTurmaAluno" runat="server"
                                MostrarMensagemSelecione="true" Obrigatorio="false"
                                ValorItemVazio="-1"></uc1:UCComboGenerico>
                            <div id="divTurmaDisciplinaAluno" runat="server" visible="false">
                                <asp:GridView runat="server" ID="grvTurmaDisciplinaAluno" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false" 
                                    DataKeyNames="tud_id,lancado,turma,planejamento" OnRowDataBound="grvTurmaDisciplinaAluno_RowDataBound" OnRowCommand="grvTurmaDisciplinaAluno_RowCommand">
                                    <Columns>
                                        <asp:BoundField HeaderText="<%$ Resources:UserControl, UCPlanejamentoProjetos.grvTurmaDisciplinaAluno.HeaderText.TurmaDisciplina %>"
                                            DataField="disciplina" />
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Image runat="server" ID="imgLancado" Visible="false" />
                                                <asp:ImageButton runat="server" ID="btnVisualizar" CommandArgument="Visualizar" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divTabs-Projetos" class="hide">
        <fieldset>
        </fieldset>
    </div>
    <div id="divTabs-Documentos">
        <asp:UpdatePanel ID="updDocumentos" runat="server">
            <ContentTemplate>
                <fieldset>
                    <asp:Label ID="lblSemAreas" runat="server" Text="<%$ Resources:UserControl, UCPlanejamentoProjetos.lblSemAreas.Text %>" Visible="false"></asp:Label>
                    <asp:Repeater ID="rptAreas" runat="server" OnItemDataBound="rptAreas_ItemDataBound">
                        <ItemTemplate>
                            <fieldset>
                                <legend><asp:Label ID="lblArea" runat="server" Text='<%# Eval("tad_nome") %>'></asp:Label></legend>
                                <asp:Label ID="lblSemDocumentos" runat="server" Visible="false"></asp:Label>
                                <table>
                                    <asp:Repeater ID="rptDocumentos" runat="server" OnItemDataBound="rptDocumentos_ItemDataBound" Visible="false">
                                        <ItemTemplate>
                                            <tr><td>
                                                <asp:Label ID="lblEspaco" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- "></asp:Label> 
                                                <asp:HyperLink ID="hplDocumento" runat="server" Text='<%# Eval("aar_descricao") %>'></asp:HyperLink>
                                            </td></tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </fieldset>
                        </ItemTemplate>
                    </asp:Repeater>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divTabsObjetoAprendizagem" runat="server">
        <asp:UpdatePanel ID="updObjetosAprendizagem" runat="server">
            <ContentTemplate>

                <asp:Repeater ID="rptobjAprendizagem" runat="server" OnItemDataBound="rptobjAprendizagem_ItemDataBound">
                    <HeaderTemplate>
                        <div>
                            <table id="tblObjetosAprendizagem" class="grid sortableAvaliacoes grid-responsive-list">
                                <thead>
                                    <tr class="gridHeader" style="height: 30px;">
                                        <th class="center">
                                            <asp:Label ID="_lblobjetos" runat="server" Text='Objetos de Aprendizagem'></asp:Label>
                                        </th>
                                        <asp:Repeater ID="rptBimestre" runat="server">
                                            <ItemTemplate>
                                                <th class="center {sorter :false}" style="border-left: 0.1em dotted #FFFFFF; padding-right: 3px;">
                                                    <asp:Label ID="lblPeriodo" runat="server" Text='<%# Eval("cap_descricao") %>' />
                                                    <asp:HiddenField ID="hdnPeriodo" runat="server" Value='<%# Eval("tpc_id") %>' />
                                                    <asp:HiddenField ID="hdnPeriodoOrdem" runat="server" Value='<%# Eval("tpc_ordem") %>' />
                                                    <asp:Label ID="lblNomeAbreviado" runat="server" Text='<%# Eval("tpc_nomeAbreviado") %>' style="display:none;" CssClass="abbr-periodo"/>
                                                    <asp:HiddenField ID="hdnIdAvaliacao" runat="server" />
                                                    <asp:HiddenField ID="hdnAvaliacaoTipo" runat="server" />
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                </thead>

                                <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="gridRow grid-linha-destaque">
                            <td>
                                <asp:Label ID="lblobjetos" runat="server" Text='<%# Eval("oap_descricao") %>'></asp:Label>
                            </td>
                            <asp:Repeater ID="rptchkBimestre" runat="server">
                                <ItemTemplate>
                                    <td>
                                        <asp:CheckBox ID="chkBimestre" runat="server" Text="" Style="display: inline-block;" />
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                        </table></div>
                    </FooterTemplate>
                </asp:Repeater>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <asp:HiddenField ID="selected_tab" runat="server" />
</div>

<div id="divHistoricoAlteracoesCiclo" title="Histórico de alterações" class="hide">
    <fieldset>
        <asp:UpdatePanel ID="updHistoricoCiclo" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="grvHistoricoCiclo" runat="server" AutoGenerateColumns="false"
                    EmptyDataText="Não existe alterações no plano de ciclo.">
                    <Columns>
                        <asp:BoundField HeaderText="Nome do docente" DataField="nomeUsuario" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Data de modificação" DataField="dataModificacao" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
                </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</div>