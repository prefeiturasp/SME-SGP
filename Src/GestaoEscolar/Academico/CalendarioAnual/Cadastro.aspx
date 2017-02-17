<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_Calendario_Anual_Cadastro" CodeBehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Academico/CalendarioAnual/Busca.aspx" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoPeriodoCalendario.ascx" TagName="_UCComboTipoPeriodoCalendario"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="area-form">
        <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="calendario" />
        <fieldset>
            <legend>Calendário escolar</legend>
            <uc2:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
            <asp:Label ID="_lblAno" runat="server" Text="Ano letivo *" AssociatedControlID="_txtAno"></asp:Label>
            <asp:TextBox ID="_txtAno" runat="server" CssClass="numeric" SkinID="Numerico" MaxLength="4"></asp:TextBox>
            <asp:Label ID="LabelFormatoAno" runat="server" Text="(AAAA)"></asp:Label>
            <asp:RequiredFieldValidator ID="_rfvAno" runat="server" ErrorMessage="Ano letivo é obrigatório."
                ControlToValidate="_txtAno" ValidationGroup="calendario">*</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="_revAno" runat="server" ControlToValidate="_txtAno"
                ValidationGroup="calendario" Display="Dynamic" ErrorMessage="" ValidationExpression="^([0-9]){4}$">*</asp:RegularExpressionValidator>
            <asp:Label ID="_lblDescricao" runat="server" Text="Descrição do calendário escolar *"
                AssociatedControlID="_txtDescricao"></asp:Label>
            <asp:TextBox ID="_txtDescricao" runat="server" SkinID="text60C" MaxLength="200"></asp:TextBox>
            <asp:RequiredFieldValidator ID="_rfvDescricao" runat="server" ErrorMessage="Descrição do calendário escolar é obrigatório."
                ControlToValidate="_txtDescricao" ValidationGroup="calendario">*</asp:RequiredFieldValidator>
            <asp:Label ID="_lblDataInicio" runat="server" Text="Início do ano letivo *" AssociatedControlID="_txtDataInicio"></asp:Label>
            <asp:TextBox ID="_txtDataInicio" runat="server" CssClass="maskData" SkinID="Data"></asp:TextBox>
            <asp:RequiredFieldValidator ID="_rfvDataInicio" runat="server" ErrorMessage="Início do ano letivo é obrigatório."
                ControlToValidate="_txtDataInicio" Display="Dynamic" ValidationGroup="calendario">*</asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cvData" runat="server" ControlToValidate="_txtDataInicio"
                ValidationGroup="calendario" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
            <asp:Label ID="_lblDataFim" runat="server" Text="Fim do ano letivo *" AssociatedControlID="_txtDataFim"></asp:Label>
            <asp:TextBox ID="_txtDataFim" runat="server" CssClass="maskData" SkinID="Data"></asp:TextBox>
            <asp:RequiredFieldValidator ID="_rfvDataFim" runat="server" ErrorMessage="Fim do ano letivo é obrigatório."
                ControlToValidate="_txtDataFim" Display="Dynamic" ValidationGroup="calendario">*</asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cvData2" runat="server" ControlToValidate="_txtDataFim"
                ValidationGroup="calendario" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
            <asp:CustomValidator ID="cvDatasAnoLetivo" runat="server" ErrorMessage="Início do ano letivo não pode ser maior que o fim do ano letivo."
                Display="Dynamic" ValidationGroup="calendario" Visible="false" OnServerValidate="ValidarDatasPeriodoLetivo_ServerValidate">*</asp:CustomValidator>
            <asp:CheckBox ID="ckbPermiteRecesso" Checked="false" runat="server" Text="<%$ Resources:Academico, CalendarioAnual.Cadastro.ckbPermiteRecesso.Text %>" />
        </fieldset>
        <asp:UpdatePanel ID="_uppGridView" runat="server">
            <ContentTemplate>
                <uc1:UCLoader ID="UCLoader2" runat="server" AssociatedUpdatePanelID="_uppGridView" />
                <fieldset id="fdsPeriodos" runat="server" visible="true">
                    <legend>Cadastro de períodos</legend>
                    <asp:Label ID="_lblMessage2" runat="server" EnableViewState="False"></asp:Label>
                    <asp:GridView ID="_dgvCalendarioPeriodo" runat="server" AutoGenerateColumns="False"
                        OnRowCommand="_dgvCalendarioPeriodo_RowCommand" OnRowDataBound="_dgvCalendarioPeriodo_RowDataBound"
                        EmptyDataText="Não existem períodos cadastrados." SkinID="GridResponsive">
                        <Columns>
                            <asp:TemplateField HeaderText="cap_id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCap_id" runat="server" Text='<%#Bind("cap_id") %>' Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lbl" runat="server" Text="Descrição"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="responsive-clear">
                                        <div style="float: left">
                                            <asp:TextBox ID="txtDescricao" runat="server" Text='<%# Bind("cap_descricao") %>'
                                                CssClass="wrap150px" ValidationGroup="calendario" />
                                        </div>
                                        <div style="float: left">
                                            <asp:CustomValidator ID="cvDescricao" runat="server" ErrorMessage="" ValidationGroup="calendario">*</asp:CustomValidator>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Label ID="lbl" runat="server" Text="Tipo de período"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="responsive-clear">
                                        <div style="float: left">
                                            <asp:Label ID="tpc_id" runat="server" Text='<%# Bind("tpc_id") %>' Visible="false"></asp:Label>
                                            <asp:DropDownList ID="ddlTipoPeriodo" DataValueField="tpc_id" DataTextField="tpc_nome"
                                                runat="server">
                                            </asp:DropDownList>
                                        </div>
                                        <div style="float: left">
                                            <asp:CustomValidator ID="cvTipoPeriodo" runat="server" ErrorMessage="" ValidationGroup="calendario">*</asp:CustomValidator>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lbl" runat="server" Text="Início do período"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="responsive-clear">
                                        <div style="float: left">
                                            <asp:TextBox runat="server" ID="txtInicioPeriodo" SkinID="Data"></asp:TextBox>
                                        </div>
                                        <div style="float: left">
                                            <asp:CustomValidator ID="cvDataInicio" runat="server" ErrorMessage="" ValidationGroup="calendario">*</asp:CustomValidator>
                                        </div>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle CssClass="center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lbl" runat="server" Text="Fim do período"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="responsive-clear">
                                        <div style="float: left">
                                            <asp:TextBox runat="server" ID="txtFimPeriodo" SkinID="Data" />
                                        </div>
                                        <div style="float: left">
                                            <asp:CustomValidator ID="cvDataFim" runat="server" ErrorMessage="" ValidationGroup="calendario">*</asp:CustomValidator>
                                        </div>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle CssClass="center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <div class="responsive-clear">
                                        <div style="float: left">
                                            <asp:ImageButton ID="_btnExcluir" SkinID="btExcluir" runat="server" CommandName="Excluir"
                                                CausesValidation="False" Visible="false" />
                                        </div>
                                        <div style="float: left">
                                            <asp:ImageButton ID="ibtnAdd" SkinID="btNovo" runat="server" CommandName="Adicionar"
                                                CausesValidation="False" Visible="true" />
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Panel ID="pnlCursos" runat="server" GroupingText="Curso do calendário">
            <asp:UpdatePanel ID="updCursos" runat="server">
                <ContentTemplate>
                    <uc1:UCLoader ID="UCLoader3" runat="server" AssociatedUpdatePanelID="updCursos" />
                        <asp:Repeater ID="rptCursos" runat="server">
                            <HeaderTemplate>
                                <div></div>
                                <div class="checkboxlist-columns">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("cur_id") %>' />
                                <asp:CheckBox ID="ckbCurso" runat="server" Text='<%# Eval("cur_nome") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                </div>
                            </FooterTemplate>
                        </asp:Repeater>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <fieldset>
        <div class="right area-botoes-bottom">
            <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" OnClick="_btnSalvar_Click"
                ValidationGroup="calendario" />
            <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                OnClick="_btnCancelar_Click" />
            <input id="txtSelectedTab" type="hidden" />
        </div>
    </fieldset>
</asp:Content>
