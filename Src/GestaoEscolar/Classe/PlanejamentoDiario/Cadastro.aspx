<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Classe.PlanejamentoDiario.Cadastro" %>

<%@ Register Src="~/WebControls/Mensagens/UCConfirmacaoOperacao.ascx" TagName="UCConfirmacaoOperacao" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCCCalendario" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:Label ID="lblAlerta" runat="server" EnableViewState="false" Visible="false"></asp:Label>
            <asp:Label ID="lblPeriodoEfetivado" runat="server" EnableViewState="false" Visible="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agenda" />

    <fieldset style="padding-top: 0; overflow: hidden" class="form-agenda">
        <legend>Gerar agenda</legend>
        <uc3:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <div id="divCalendario" runat="server">
            <uc2:UCCCalendario ID="UCCCalendario1" runat="server" />
            <br />
            <br />
        </div>
        <div id="divPeriodo" runat="server">
            <fieldset id="fdsPeriodo" runat="server" style="width: auto; display: inline; vertical-align: top;">
                <legend>
                    <asp:Label ID="lblPeriodo" runat="server" Text="<%$ Resources:Classe, PlanejamentoDiario.Cadastro.lblPeriodo.Text %>" /></legend>
                <asp:RadioButtonList ID="rbtPeriodo" RepeatDirection="Horizontal" runat="server"
                    DataValueField="tpc_id" DataTextField="tpc_nome" OnSelectedIndexChanged="rbtPeriodo_SelectedIndexChanged" AutoPostBack="true">
                </asp:RadioButtonList>
                <div>
                    <asp:CheckBox runat="server" ID="chkIntervalo" TextAlign="Right" CausesValidation="false" AutoPostBack="true"
                        Text="<%$ Resources:Classe, PlanejamentoDiario.Cadastro.chkIntervalo.Text %>" OnCheckedChanged="chkIntervalo_CheckedChanged" />
                    <div runat="server" id="divIntervalo" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDataInicio" runat="server" Text="Data início *" AssociatedControlID="txtDataInicio"></asp:Label>
                                    <asp:TextBox ID="txtDataInicio" runat="server" CssClass="maskData" SkinID="Data"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDataInicio" runat="server" ErrorMessage="Data de início é obrigatória."
                                        ControlToValidate="txtDataInicio" Display="Dynamic" ValidationGroup="agenda">*</asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvData" runat="server" ControlToValidate="txtDataInicio"
                                        ValidationGroup="agenda" Display="Dynamic" ErrorMessage="Data de início está inválida, deve estar no formato DD/MM/AAAA." 
                                        OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Label ID="lblDataFim" runat="server" Text="Data fim *" AssociatedControlID="txtDataFim"></asp:Label>
                                    <asp:TextBox ID="txtDataFim" runat="server" CssClass="maskData" SkinID="Data"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDataFim" runat="server" ErrorMessage="Data de fim é obrigatória."
                                        ControlToValidate="txtDataFim" Display="Dynamic" ValidationGroup="agenda">*</asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvData2" runat="server" ControlToValidate="txtDataFim"
                                        ValidationGroup="agenda" Display="Dynamic" ErrorMessage="Data de fim está inválida, deve estar no formato DD/MM/AAAA." 
                                        OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                                    <asp:CustomValidator ID="cvDatasAnoLetivo" runat="server" ErrorMessage="Data de início não pode ser maior que a data de fim."
                                        Display="Dynamic" ValidationGroup="agenda" Visible="false" OnServerValidate="ValidarDatasPeriodoLetivo_ServerValidate">*</asp:CustomValidator>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </fieldset>
        </div>

        <div id="divPlanejamentoDiario" runat="server" visible="true">
            <fieldset class="area-form">
                <legend><asp:Literal ID="lblPlanejamentoDiario" runat="server" Text="<%$ Resources:Classe, PlanejamentoDiario.Cadastro.lblPlanejamentoDiario.Text %>" /></legend>
                <asp:UpdatePanel ID="updAulas" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblAviso" runat="server" Visible="true"></asp:Label>
                        <asp:GridView ID="gdvAulas" runat="server" AutoGenerateColumns="false" OnRowDataBound="gdvAulas_RowDataBound"
                            DataKeyNames="uni_id, esc_id, cal_id, tur_id, tud_id, tud_cargaHorariaSemanal, TurmaDisciplina, tud_tipo, tdt_posicao, tud_idRelacionada, cap_dataInicio, cap_dataFim, cap_descricao, escola, curso, turno, fav_fechamentoAutomatico, fav_tipoApuracaoFrequencia"
                            AllowPaging="false" EmptyDataText="Não existem turmas para gerar a agenda." SkinID="GridResponsive">
                            <%--DataKeyNames="tud_id, tur_id, tdt_posicao, cal_id, esc_id, uni_id, tud_tipo, segunda, terca, quarta, quinta, sexta, sabado, tud_cargaHorariaSemanal, tud_idRelacionada"--%>
                            <Columns>
                                <asp:BoundField HeaderText="Escola" DataField="escola" HeaderStyle-CssClass="Center" />
                                <asp:BoundField HeaderText="Calendário" DataField="calendario" HeaderStyle-CssClass="Center" Visible="false" />
                                <asp:BoundField HeaderText="Curso" DataField="curso" HeaderStyle-CssClass="Center" HtmlEncode="false" />
                                <asp:BoundField HeaderText="Turno" DataField="turno" HeaderStyle-CssClass="Center" />
                                <asp:BoundField HeaderText="Turma" DataField="TurmaDisciplina" HeaderStyle-CssClass="Center" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="Seg">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAulasSegunda" runat="server" Text='<%#Bind("segunda") %>' SkinID="Numerico2c" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" Width="20px" />
                                    <ItemStyle HorizontalAlign="center" CssClass="grid-responsive-item-inline"></ItemStyle>  
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ter">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAulasTerca" runat="server" Text='<%#Bind("terca") %>' SkinID="Numerico2c" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" Width="20px" />
                                    <ItemStyle HorizontalAlign="center" CssClass="grid-responsive-item-inline"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qua">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAulasQuarta" runat="server" Text='<%#Bind("quarta") %>' SkinID="Numerico2c" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" Width="20px" />
                                    <ItemStyle HorizontalAlign="center" CssClass="grid-responsive-item-inline"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qui">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAulasQuinta" runat="server" Text='<%#Bind("quinta") %>' SkinID="Numerico2c" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" Width="20px" />
                                    <ItemStyle HorizontalAlign="center" CssClass="grid-responsive-item-inline"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sex">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAulasSexta" runat="server" Text='<%#Bind("sexta") %>' SkinID="Numerico2c" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" Width="20px" />
                                    <ItemStyle HorizontalAlign="center" CssClass="grid-responsive-item-inline"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sáb">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAulasSabado" runat="server" Text='<%#Bind("sabado") %>' SkinID="Numerico2c" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" Width="20px" />
                                    <ItemStyle HorizontalAlign="center" CssClass="grid-responsive-item-inline"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </div>

        <div class="right area-botoes-bottom">
            <asp:Button ID="btnGerar" runat="server" ValidationGroup="agenda" Text="Gerar" OnClick="btnGerar_Click" />
            <asp:Button ID="btnCancelar" runat="server" CausesValidation="false" Text="Cancelar" OnClick="btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
