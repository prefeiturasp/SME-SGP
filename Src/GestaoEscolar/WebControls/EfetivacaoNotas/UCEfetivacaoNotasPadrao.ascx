<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCEfetivacaoNotasPadrao.ascx.cs" Inherits="GestaoEscolar.WebControls.EfetivacaoNotas.UCEfetivacaoNotasPadrao" %>

<%@ Register Src="../../WebControls/Combos/UCComboOrdenacao.ascx" TagName="UCComboOrdenacao"
    TagPrefix="uc2" %>

<asp:UpdatePanel ID="uppGridAlunos" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Label ID="lblMessageInfo" runat="server" EnableViewState="False"></asp:Label>
        <asp:HiddenField ID="hdnIndiceBtnAtualizar" runat="server" />
        <div>
            <div style="float: left;">
                <div runat="server" id="divchkSemProfessor">
                    <asp:CheckBox ID="chkSemProfessor" runat="server" OnCheckedChanged="chkSemProfessor_CheckedChanged" AutoPostBack="true" Text="Sem professor" Style="display: inline-block;" />
                </div>
                <div runat="server" id="divchkNaoAvaliado">
                    <asp:CheckBox ID="chkNaoAvaliado" runat="server" AutoPostBack="true" Text="Não avaliados" OnCheckedChanged="chkNaoAvaliado_CheckedChanged" />
                </div>
                <div runat="server" id="divlblQtdeAulas">
                    <asp:Label ID="lblQtdeAulasCaption" runat="server" Text='Qtde. dias de aulas:'></asp:Label>
                    <asp:Label ID="lblQtdeAulas" runat="server" Text='<%#Bind("Frequencia")%>'></asp:Label>
                </div>
                <uc2:UCComboOrdenacao ID="_UCComboOrdenacao1" runat="server" />
            </div>
            <div style="float: right;">
                <asp:Label ID="lblTotalAulasExperiencia" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="lblQtdeAulasDadas" runat="server" Visible="false"></asp:Label>
            </div>
        </div>
        <div class="clear"></div>
        <div class="divScrollResponsivo">
            <asp:GridView ID="gvAlunos" runat="server" AutoGenerateColumns="False" EmptyDataText="Não foram encontrados alunos na turma selecionada."
                OnRowDataBound="gvAlunos_RowDataBound" DataKeyNames="tur_id,tud_id,alu_id,mtu_id,mtd_id,AvaliacaoID,tud_idPrincipal,mtd_idPrincipal,dispensadisciplina,alc_matricula,tur_codigo,mtd_numeroChamada,pes_nome,mtu_idAnterior,mtd_idAnterior,pes_dataNascimento"
                OnRowCommand="gvAlunos_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Nº chamada" HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblChamada" runat="server" Text='<%#Bind("mtd_numeroChamada") %>'>
                            </asp:Label>
                            <asp:HiddenField ID="hdnSituacao" runat="server" Value='<%# Bind("situacaoMatriculaAluno") %>' />
                            <asp:HiddenField ID="hdnDataMatricula" runat="server" Value='<%# Bind("dataMatricula") %>' />
                            <asp:HiddenField ID="hdnDataSaida" runat="server" Value='<%# Bind("dataSaida") %>' />
                            <asp:HiddenField ID="hdnAvaliado" runat="server" Value='<%# Bind("ala_avaliado") %>' />
                            <asp:HiddenField ID="hdnDispensaDisciplina" runat="server" Value='<%# Bind("dispensadisciplina") %>' />
                            <asp:HiddenField ID="hdnFrequenciaAjustada" runat="server" Value='<%# Bind("FrequenciaFinalAjustada") %>' />
                            <asp:HiddenField ID="hdnQtAulas" runat="server" Value='<%# Bind("QtAulasAluno") %>' />
                            <asp:HiddenField ID="hdnQtFaltasAnteriores" runat="server" Value='<%# Bind("faltasAnteriores") %>' />
                            <asp:HiddenField ID="hdnQtcompensadasAnteriores" runat="server" Value='<%# Bind("compensadasAnteriores") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Nome do aluno">
                        <ItemTemplate>
                            <asp:Label ID="lblAluno" runat="server" Text='<%#Bind("pes_nome") %>' CssClass="tamanho-lbl-aluno">
                            </asp:Label>
                            <asp:Label ID="lblNomeAluno" runat="server" Text='<%#Bind("pes_nome") %>' Visible="false">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Idade">
                        <ItemTemplate>
                            <asp:Label ID="lblIdade" runat="server">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField HeaderText="" DataField="tca_numeroAvaliacao" Visible="false" />
                    <%--Nota da avaliação adicional--%>
                    <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle CssClass="colunaNotaAdicional" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:TextBox ID="txtNotaAdicional" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                            <asp:DropDownList ID="ddlPareceresAdicional" runat="server">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hdnAvaliacaoAdicional" runat="server" Value='<%# Bind("AvaliacaoAdicional") %>' />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <%--Nota da avaliação normal--%>
                    <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle CssClass="colunaNota" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <div>
                                <asp:Label ID="lblFaltoso" runat="server" Text='<%#Bind("faltoso") %>' Visible="false"></asp:Label>
                                <asp:TextBox ID="txtNota" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                                <asp:CustomValidator ID="cvNotaMaxima" runat="server" ControlToValidate="txtNota"
                                    Display="Dynamic" OnServerValidate="cvNotaMaxima_Validar" Visible="false" ErrorMessage="">*</asp:CustomValidator>
                                <asp:DropDownList ID="ddlPareceres" runat="server">
                                </asp:DropDownList>
                                <asp:CustomValidator ID="cvParecerMaximo" runat="server" ControlToValidate="ddlPareceres"
                                    Display="Dynamic" OnServerValidate="cvParecerMaximo_Validar" Visible="false"
                                    ErrorMessage="">*</asp:CustomValidator>
                            </div>
                            <div id="divLancarRelatorio" style="display: inline; padding: 0 0 0 0;">
                                <asp:ImageButton ID="btnRelatorio" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                    CommandName="Relatorio" CausesValidation="false" />
                                <asp:Image ID="imgSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Relatório lançado"
                                    Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                <asp:HyperLink ID="hplAnexo" runat="server" SkinID="hplAnexo" ToolTip="Relatório anexo"
                                    Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                <asp:CustomValidator ID="cvRelatorioDesempenho" runat="server" OnServerValidate="cvRelatorioDesempenho_Validar"
                                    Visible="true" Display="Dynamic" ErrorMessage="">*</asp:CustomValidator>
                            </div>
                            <asp:ImageButton ID="btnAvaliacao" runat="server" SkinID="btAulas" Style="padding: 0; margin: 0; vertical-align: top;"
                                CommandName="Avaliacao" />
                            <asp:HiddenField ID="hdnNota" runat="server" Value='<%# Bind("Avaliacao") %>' />
                            <asp:HiddenField ID="hdnRecuperacaoPorNota" runat="server" Value='<%# Bind("recuperacaoPorNota") %>' />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <%-- Nota pós-conselho --%>
                    <asp:TemplateField HeaderText="Nota pós-conselho" HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle CssClass="colunaNota" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <div style="display: inline; padding: 0 0 0 0;">
                                <asp:TextBox ID="txtNotaPosConselho" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                                <asp:CustomValidator ID="cvNotaPosConselhoMaxima" runat="server" ControlToValidate="txtNotaPosConselho"
                                    Display="Dynamic" OnServerValidate="cvNotaMaxima_Validar" Visible="false" ErrorMessage="">*</asp:CustomValidator>
                                <asp:DropDownList ID="ddlPareceresPosConselho" runat="server">
                                </asp:DropDownList>
                                <asp:CustomValidator ID="cvParecerPosConselhoMaximo" runat="server" ControlToValidate="ddlPareceresPosConselho"
                                    Display="Dynamic" OnServerValidate="cvParecerMaximo_Validar" Visible="false"
                                    ErrorMessage="">*</asp:CustomValidator>
                                <asp:ImageButton ID="btnJustificativaPosConselho" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                    CommandName="JustificativaPosConselho" CausesValidation="false" />
                                <asp:Image ID="imgJustificativaPosConselhoSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Justificativa pós-conselho preenchida"
                                    Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                            </div>
                            <div id="divLancarRelatorioPosConselho" style="display: inline; padding: 0 0 0 0;">
                                <asp:ImageButton ID="btnRelatorioPosConselho" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                    CommandName="Relatorio" CausesValidation="false" />
                                <asp:Image ID="imgSituacaoPosConselho" runat="server" SkinID="imgConfirmar" ToolTip="Relatório lançado"
                                    Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                <asp:HyperLink ID="hplAnexoPosConselho" runat="server" SkinID="hplAnexo" ToolTip="Relatório anexo"
                                    Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                <asp:CustomValidator ID="cvRelatorioDesempenhoPosConselho" runat="server" OnServerValidate="cvRelatorioDesempenho_Validar"
                                    Visible="true" Display="Dynamic" ErrorMessage="">*</asp:CustomValidator>
                            </div>
                            <asp:HiddenField ID="hdnNotaPosConselho" runat="server" Value='<%# Bind("avaliacaoPosConselho") %>' />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" HorizontalAlign="Center" Width="200px" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Faltoso" HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-CssClass="ColunaFaltoso">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkFaltoso" runat="server" Text="" Checked='<%#Bind("faltoso") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Nota da avaliação normal e pós-conselho para disciplina de Regência --%>
                    <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnExpandir" runat="server" ToolTip=""
                                CssClass="ui-icon ui-icon-circle-triangle-s" OnClientClick="ExpandCollapse('.trExpandir', this); return false;" />
                            <input id="hfExpandido" runat="server" type="hidden" value="0" />
                        </ItemTemplate>
                        <HeaderTemplate>
                            <asp:Literal runat="server" ID="litNotaRegencia"></asp:Literal>
                            <asp:LinkButton ID="btnExpandir" runat="server" ToolTip="" Style="display: block; margin: auto;"
                                CssClass="ui-icon ui-icon-circle-triangle-s" OnClientClick="ExpandCollapseAll(this); return false;" />
                            <input id="hfExpandidoTodos" runat="server" type="hidden" value="0" />
                        </HeaderTemplate>
                        <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Qtde. dias de aulas" HeaderStyle-CssClass="center"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:TextBox ID="txtQtdeAula" runat="server" MaxLength="5" SkinID="Numerico2c" Text='<%#Bind("QtAulasAluno") %>'>
                            </asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" CssClass="colunaQtdeAula" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Qtde. faltas" HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:TextBox ID="txtQtdeFalta" runat="server" MaxLength="5" SkinID="Numerico2c" Text='<%#Bind("QtFaltasAluno") %>'>
                            </asp:TextBox>
                            <asp:Label ID="lblQtdeFaltaReposicao" runat="server" Text='<%#Bind("QtFaltasAlunoReposicao") %>' Visible="false"></asp:Label>
                            <asp:CustomValidator ID="cvQtFaltas" runat="server" ControlToValidate="txtQtdeFalta"
                                OnServerValidate="cvQtFaltas_Validar" EnableClientScript="true" Display="Dynamic"
                                ErrorMessage="A quantidade de faltas deve ser menor ou igual à quantidade de dias de aulas.">*</asp:CustomValidator>
                            <asp:ImageButton ID="btnFaltasExternas" runat="server" ToolTip="Exibir a quantidade de faltas fora da rede" SkinID="btDetalhar" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" CssClass="colunaQtdeFalta" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ausências Compensadas" HeaderStyle-CssClass="center"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAusenciasCompensadas" MaxLength="5" runat="server" SkinID="Numerico2c"
                                Text='<%#Bind("ausenciasCompensadas") %>'>
                            </asp:TextBox>
                            <asp:CustomValidator ID="cvQtAusenciasCompensadas" runat="server" ControlToValidate="txtAusenciasCompensadas"
                                OnServerValidate="cvQtAusenciasCompensadas_Validar" EnableClientScript="true" Display="Dynamic"
                                ErrorMessage="A quantidade de ausências compensadas deve ser menor ou igual à quantidade de faltas.">*</asp:CustomValidator>
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle CssClass="colunaQtdeCompensacao" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:TextBox ID="txtFrequencia" runat="server" SkinID="Decimal" MaxLength="6"
                                Text='<%# string.Format(VS_FormatacaoDecimaisFrequencia , Convert.ToDecimal(Eval("Frequencia"))) %>'>
                            </asp:TextBox>
                            <asp:CustomValidator ID="cvFrequencia" runat="server" Display="Dynamic" ControlToValidate="txtFrequencia"
                                OnServerValidate="cvFrequencia_Validar" ErrorMessage="Frequência deve ser maior ou igual a zero.">*</asp:CustomValidator>
                        </ItemTemplate>
                        <HeaderTemplate>
                            <asp:Label ID="lblTitulo" runat="server" Text="Frequência (%)"></asp:Label>
                        </HeaderTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" CssClass="colunaFrequencia" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:TextBox ID="txtFrequenciaAcumulada" runat="server" SkinID="Decimal" MaxLength="6">
                            </asp:TextBox>
                            <asp:CustomValidator ID="cvFrequenciaAcumulada" runat="server" Display="Dynamic"
                                ControlToValidate="txtFrequenciaAcumulada" OnServerValidate="cvFrequencia_Validar"
                                ErrorMessage="Frequência acumulada deve ser maior ou igual a zero.">*</asp:CustomValidator>
                        </ItemTemplate>
                        <HeaderTemplate>
                            <asp:Label ID="lblTituloAcumulada" runat="server" Text="Frequência acumulada (%)"></asp:Label>
                        </HeaderTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:TextBox ID="txtFrequenciaFinal" runat="server" SkinID="Decimal" MaxLength="6" Enabled="false"
                                Text='<%# string.Format(VS_FormatacaoDecimaisFrequencia , Convert.ToDecimal(Eval("frequenciaAcumulada"))) %>'></asp:TextBox>
                            <asp:CustomValidator ID="cvFrequenciaFinal" runat="server" Display="Dynamic"
                                ControlToValidate="txtFrequenciaFinal" OnServerValidate="cvFrequencia_Validar"
                                ErrorMessage="Frequência final deve ser maior ou igual a zero.">*</asp:CustomValidator>
                        </ItemTemplate>
                        <HeaderTemplate>
                            <asp:Label ID="lblTituloFreqFinal" runat="server" Text="Frequência final (%)"></asp:Label>
                        </HeaderTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:TextBox ID="txtFrequenciaFinalAjustada" runat="server" SkinID="Decimal" MaxLength="6" Enabled="false"
                                Text='<%# string.Format(VS_FormatacaoDecimaisFrequencia , Convert.ToDecimal(Eval("FrequenciaFinalAjustada"))) %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderTemplate>
                            <asp:Label ID="lblTituloFreqFinalAjustada" runat="server" Text="Freq. final (%)"></asp:Label>
                        </HeaderTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" CssClass="colunaFrequenciaAjustada" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Registro do professor" HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div id="divObservacao" style="display: inline; padding: 0 0 0 0;">
                                <asp:ImageButton ID="btnObservacao" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                    CommandName="ObservacaoDisciplina" CausesValidation="false" ToolTip="Preencher registro do professor" />
                                <asp:Image ID="imgObservacaoSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Registro do professor preenchido"
                                    Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                            </div>
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div id="divObservacaoConselho" style="display: inline; padding: 0 0 0 0;">
                                <asp:ImageButton ID="btnObservacaoConselho" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                    CommandName="ObservacaoConselho" CausesValidation="false" ToolTip="Preencher informações para o boletim do aluno" />
                                <asp:Image ID="imgObservacaoConselhoSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Observação preenchida"
                                    Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                            </div>
                        </ItemTemplate>
                        <HeaderTemplate>
                            <asp:Literal ID="litHeadParecerConclusivo" runat="server" Text="<%$ Resources:UserControl, EfetivacaoNotas.UCEfetivacaoNotas.RegistroConselho %>"></asp:Literal>
                        </HeaderTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnFrequencia" runat="server" SkinID="btAtualizar" ToolTip="Atualizar frequência do aluno"
                                CausesValidation="false" OnClick="btnFrequencia_Click" />
                        </ItemTemplate>
                        <HeaderTemplate>
                            <asp:ImageButton ID="btnTodasFrequencias" runat="server" SkinID="btAtualizar" ToolTip="Atualizar frequência de todos os alunos"
                                CausesValidation="false" OnClick="btnTodasFrequencias_Click" />
                        </HeaderTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" CssClass="colunaFrequencia" Width="25px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlResultado" runat="server">
                                <%--<asp:ListItem Text="-- Selecione um [MSG_RESULTADOEFETIVACAO] --" Value="-1"></asp:ListItem>--%>
                            </asp:DropDownList>
                            <asp:HiddenField ID="hdnResultado" runat="server" Value='<%# Bind("AvaliacaoResultado") %>' />
                        </ItemTemplate>
                        <HeaderTemplate>
                            <asp:Label ID="lblTituloResultadoEfetivacao" runat="server" Text=""></asp:Label>
                        </HeaderTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Boletim" HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnBoletim" runat="server" CausesValidation="false" CommandName="Boletim"
                                SkinID="btRelatorio" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <tr class="trExpandir" style="display: none;">
                                <td align="left" colspan="100%" class="divExpandeLinha">
                                    <asp:Repeater ID="rptComponenteRegencia" runat="server" OnItemDataBound="rptComponenteRegencia_ItemDataBound" OnItemCommand="rptComponenteRegencia_ItemCommand">
                                        <HeaderTemplate>
                                            <table style="margin: 0px;">
                                                <tr class="gridHeader" id="tr0" runat="server">
                                                    <th>
                                                        <asp:Literal ID="litHeadNomeDisciplina" runat="server"></asp:Literal></th>
                                                    <th>
                                                        <asp:Literal ID="litHeadNota" runat="server"></asp:Literal></th>
                                                    <th>
                                                        <asp:Literal ID="litHeadNotaPosConselho" runat="server"></asp:Literal></th>
                                                </tr>
                                        </HeaderTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <tr id="tr1" runat="server">
                                                <td style="border-top: 0px;">
                                                    <asp:Label ID="lblNomeDisciplina" runat="server" Text='<%#Bind("dis_nome") %>' Font-Bold="true"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hfDataKeys" />
                                                </td>
                                                <%-- Nota --%>
                                                <td class="colunaNota" style="border-top: 0px;">
                                                    <div>
                                                        <asp:Label ID="lblFaltoso" runat="server" Text='<%#Bind("faltoso") %>' Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txtNota" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                                                        <asp:CustomValidator ID="cvNotaMaxima" runat="server" ControlToValidate="txtNota"
                                                            Display="Dynamic" OnServerValidate="cvNotaMaxima_Validar" Visible="false" ErrorMessage="">*</asp:CustomValidator>
                                                        <asp:DropDownList ID="ddlPareceres" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:CustomValidator ID="cvParecerMaximo" runat="server" ControlToValidate="ddlPareceres"
                                                            Display="Dynamic" OnServerValidate="cvParecerMaximo_Validar" Visible="false"
                                                            ErrorMessage="">*</asp:CustomValidator>
                                                    </div>
                                                    <div id="divLancarRelatorio" style="display: inline; padding: 0 0 0 0;">
                                                        <asp:ImageButton ID="btnRelatorio" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                                            CommandName="Relatorio" CausesValidation="false" />
                                                        <asp:Image ID="imgSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Relatório lançado"
                                                            Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                        <asp:HyperLink ID="hplAnexo" runat="server" SkinID="hplAnexo" ToolTip="Relatório anexo"
                                                            Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                        <asp:CustomValidator ID="cvRelatorioDesempenho" runat="server" OnServerValidate="cvRelatorioDesempenho_Validar"
                                                            Visible="true" Display="Dynamic" ErrorMessage="">*</asp:CustomValidator>
                                                    </div>
                                                    <asp:HiddenField runat="server" ID="hdnAvaliacao" Value='<%# Bind("Avaliacao") %>' />
                                                </td>
                                                <%-- NotaPosConselho --%>
                                                <td class="colunaNota" style="border-top: 0px;">
                                                    <div style="display: inline; padding: 0 0 0 0;">
                                                        <asp:TextBox ID="txtNotaPosConselho" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                                                        <asp:CustomValidator ID="cvNotaPosConselhoMaxima" runat="server" ControlToValidate="txtNotaPosConselho"
                                                            Display="Dynamic" OnServerValidate="cvNotaMaxima_Validar" Visible="false" ErrorMessage="">*</asp:CustomValidator>
                                                        <asp:DropDownList ID="ddlPareceresPosConselho" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:CustomValidator ID="cvParecerPosConselhoMaximo" runat="server" ControlToValidate="ddlPareceresPosConselho"
                                                            Display="Dynamic" OnServerValidate="cvParecerMaximo_Validar" Visible="false"
                                                            ErrorMessage="">*</asp:CustomValidator>
                                                        <asp:ImageButton ID="btnJustificativaPosConselho" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                                            CommandName="JustificativaPosConselho" CausesValidation="false" />
                                                        <asp:Image ID="imgJustificativaPosConselhoSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Justificativa pós-conselho preenchida"
                                                            Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                    </div>
                                                    <div id="divLancarRelatorioPosConselho" style="display: inline; padding: 0 0 0 0;">
                                                        <asp:ImageButton ID="btnRelatorioPosConselho" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                                            CommandName="Relatorio" CausesValidation="false" />
                                                        <asp:Image ID="imgSituacaoPosConselho" runat="server" SkinID="imgConfirmar" ToolTip="Relatório lançado"
                                                            Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                        <asp:HyperLink ID="hplAnexoPosConselho" runat="server" SkinID="hplAnexo" ToolTip="Relatório anexo"
                                                            Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                        <asp:CustomValidator ID="cvRelatorioDesempenhoPosConselho" runat="server" OnServerValidate="cvRelatorioDesempenho_Validar"
                                                            Visible="true" Display="Dynamic" ErrorMessage="">*</asp:CustomValidator>
                                                    </div>
                                                    <asp:HiddenField runat="server" ID="hdnAvaliacaoPosConselho" Value='<%# Bind("avaliacaoPosConselho") %>' />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <HeaderStyle CssClass="hide" />
                        <ItemStyle CssClass="hide" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <br />
        <div id="divLegenda" runat="server" visible="false">
            <b>Legenda:</b>
            <div style="border-style: solid; border-width: thin;">
                <table id="tbLegenda" runat="server" style="border-collapse: separate !important; border-spacing: 2px !important;">
                    <tr runat="server" id="lnInativos">
                        <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                        <td>
                            <asp:Literal runat="server" ID="litInativo" Text="<%$ Resources:Mensagens, MSG_ALUNO_INATIVO %>"></asp:Literal>
                        </td>
                    </tr>
                    <tr runat="server" id="lnObrigatorio">
                        <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                        <td>
                            <asp:Literal runat="server" ID="litObrigPreenchimentoRelAval" Text="<%$ Resources:Mensagens, MSG_ALUNO_OBRIG_PREENCHIMENTO_REL_AVAL %>"></asp:Literal>
                        </td>
                    </tr>
                    <tr runat="server" id="lnAlunoNaoAvaliado">
                        <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                        <td>
                            <asp:Literal runat="server" ID="litNaoAvaliado" Text="<%$ Resources:Mensagens, MSG_ALUNO_NAO_AVALIADO %>"></asp:Literal>
                        </td>
                    </tr>
                    <tr runat="server" id="lnAlunoDispensado">
                        <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                        <td>
                            <asp:Literal runat="server" ID="litDispensado" Text="<%$ Resources:Mensagens, MSG_ALUNO_DISPENSADO %>"></asp:Literal>
                        </td>
                    </tr>
                    <tr runat="server" id="lnAlunoFrequencia">
                        <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                        <td>
                            <asp:Literal runat="server" ID="litBaixaFreq" Text="<%$ Resources:Mensagens, MSG_ALUNO_BAIXA_FREQ %>"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:HiddenField ID="hdnLocalImgCheckSituacao" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

<div id="divAvaliacao" runat="server" title="Avaliação do aluno na turma"
    class="hide">
    <asp:UpdatePanel ID="updAvaliacao" runat="server">
        <ContentTemplate>
            <asp:Label ID="Label1" runat="server" Text="Avaliação" AssociatedControlID="txtAvaliacao"></asp:Label>
            <asp:TextBox ID="txtAvaliacao" runat="server" TextMode="MultiLine" MaxLength="4000"
                ReadOnly="true" SkinID="limite4000"></asp:TextBox>
            <asp:HiddenField ID="hdnAlu_id" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="right">
        <asp:Button ID="btnCancelarAvaliacao" runat="server" Text="Voltar" OnClientClick="$('#divAvaliacao').dialog('close'); return false;" />
    </div>
</div>

<!-- Confirma quais campos deseja atualizar -->
<div id="divConfirmaAtualizacao" title="Confirmação atualização" class="hide">
    <asp:UpdatePanel ID="updConfirmaAtualizacao" runat="server">
        <ContentTemplate>
            <fieldset>
                <asp:Label ID="lblConfirmacao" Text="<b>Quais campos deseja atualizar?</b>" runat="server"></asp:Label>
                <br />
                <br />
                <div class="right">
                    <asp:Button ID="btnAtNotas" runat="server" Text="Notas" OnClick="btnAtNotas_Click" />
                    <asp:Button ID="btnAtFaltas" runat="server" Text="Faltas" OnClick="btnAtFaltas_Click" />
                    <asp:Button ID="btnAtNotaseFaltas" runat="server" Text="Notas e faltas" OnClick="btnAtNotaseFaltas_Click" />
                    <asp:Button ID="btnAtCancelar" runat="server" Text="Cancelar" OnClientClick="$('#divConfirmaAtualizacao').dialog('close'); return false;" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>


<!-- Confirma quais campos deseja atualizar -->
<div id="divFrequenciaExterna" title="Frequência de outras redes" class="hide">
    <fieldset>
        <asp:Label ID="Label2" Text="<b>Quantidade de aulas: </b>" runat="server"></asp:Label>
        <asp:Label ID="lblQtAulasExterna" Text="<b>Quantidade de aulas: </b>" runat="server"></asp:Label>
        <br />
    </fieldset>
</div>
