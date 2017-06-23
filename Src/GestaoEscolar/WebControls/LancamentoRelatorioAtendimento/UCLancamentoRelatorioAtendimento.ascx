<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCLancamentoRelatorioAtendimento.ascx.cs" Inherits="GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento" %>

<%@ Register Src="~/WebControls/Combos/UCComboRacaCor.ascx" TagName="UCCRacaCor" TagPrefix="uc" %>
<asp:UpdatePanel ID="updLancamentoRelatorio" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divInformacao">
            <asp:Label ID="lblInformacaoAluno" runat="server"></asp:Label>
            <uc:UCCRacaCor ID="UCCRacaCor" runat="server" Visible="false" />
            <div class="clear"></div>
            <asp:Label ID="lblSituacao" runat="server" Text="Situação de preenchimento" AssociatedControlID="ddlSituacao"></asp:Label>
            <asp:DropDownList ID="ddlSituacao" runat="server"></asp:DropDownList>
            <div class="clear"></div>
            <div id="divDownloadAnexo" runat="server" class="relatorio-manual" visible="false">
                <asp:Label ID="lblDownloadAnexo" runat="server" Text="Instruções de preenchimento: "></asp:Label>
                <asp:HyperLink ID="hplDownloadAnexo" runat="server" SkinID="hplAnexo" ToolTip="Realizar o download de instruções de preenchimento"></asp:HyperLink>
            </div>
        </div>

        <div class="clear"></div>
        <div id="divTabsRelatorio">
            <ul class="hide">
                <li runat="server" id="liHipoteseDiagnostica"><a href="#divTabs-0">Hipótese diagnóstica</a></li>
                <asp:Repeater ID="rptAbaQuestionarios" runat="server">
                    <ItemTemplate>
                        <li><a href='#<%# RetornaTabID((int)Eval("qst_id"))%>'><%# Eval("qst_titulo") %></a></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <div id="divTabs-0">
                <fieldset id="fdsHipoteseDiagnostica" runat="server">
                    <asp:Repeater ID="rptTipoDeficiencia" runat="server" OnItemDataBound="rptTipoDeficiencia_ItemDataBound">
                        <ItemTemplate>
                            <fieldset>
                                <legend>
                                    <asp:Literal ID="litTipoDef" runat="server" Text='<%# Eval("tde_nome") %>'></asp:Literal></legend>
                                <div class="clear"></div>
                                <asp:HiddenField ID="hdnTdeId" runat="server" Value='<%# Eval("tde_id") %>' />
                                <asp:Repeater ID="rptHipoteseDiagnostica" runat="server">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnDfdId" runat="server" Value='<%# Eval("dfd_id") %>' />
                                        <asp:CheckBox ID="chkDeficienciaDetalhe" Text='<%# Eval("dfd_nome") %>' runat="server" Checked='<%# Eval("possuiDeficienciaDetalhe") %>' />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </fieldset>
                        </ItemTemplate>
                    </asp:Repeater>
                </fieldset>
            </div>
            <asp:Repeater ID="rptQuestionario" runat="server" OnItemDataBound="rptQuestionario_ItemDataBound">
                <ItemTemplate>
                    <div id='<%# RetornaTabID((int)Eval("qst_id"))%>'>
                        <fieldset>
                            <asp:HiddenField ID="hdnRaqId" runat="server" Value='<%# Eval("raq_id") %>' />
                            <asp:Repeater ID="rptConteudo" runat="server" OnItemDataBound="rptConteudo_ItemDataBound">
                                <ItemTemplate>
                                    <div class="quetionario-conteudo">
                                        <asp:HiddenField ID="hdnTipoResposta" runat="server" Value='<%# Eval("qtc_tipoResposta") %>' />
                                        <asp:HiddenField ID="hdnQtcId" runat="server" Value='<%# Eval("qtc_id") %>' />
                                        <asp:Label ID="lblTextoConteudo" runat="server" Text='<%# Eval("qtc_texto") %>' CssClass='<%# RetornaClasseQuestionarioConteudo((byte)Eval("qtc_tipo")) %>'></asp:Label>
                                        <asp:TextBox ID="txtResposta" runat="server" CssClass="questionario-conteudo-resposta-texto" Visible="false" TextMode="MultiLine"></asp:TextBox>
                                        <div class="clear"></div>
                                        <asp:Repeater ID="rptResposta" runat="server" OnItemDataBound="rptResposta_ItemDataBound">
                                            <ItemTemplate>
                                                <div class="questionario-conteudo-resposta">
                                                    <asp:HiddenField ID="hdnQtrId" runat="server" Value='<%# Eval("qtr_id") %>' />
                                                    <asp:CheckBox ID="chkResposta" runat="server" Text='<%# Eval("qtr_texto") %>' CssClass="questionario-conteudo-resposta-multi-selecao" Visible="false" />
                                                    <asp:RadioButton ID="rdbResposta" runat="server" Text='<%# Eval("qtr_texto") %>' CssClass="questionario-conteudo-resposta-selecao-unica" Visible="false" />
                                                    <asp:TextBox ID="txtRespostaTextoAdicional" runat="server" CssClass="questionario-conteudo-resposta-texto-adicional" Visible="false"></asp:TextBox>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <div class="clear"></div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </fieldset>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <asp:HiddenField ID="txtSelectedTab" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
