<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_TurmaDisciplina_UCRepeaterDisciplina"
    CodeBehind="UCRepeaterDisciplina.ascx.cs" %>
<%@ Register Src="~/WebControls/Combos/UCComboDocente.ascx" TagName="_UCComboDocente"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/ControleVigenciaDocentes/UCControleVigenciaDocentes.ascx" TagName="UCControleVigenciaDocentes"
    TagPrefix="uc2" %>
<fieldset>
    <legend>
        <asp:Label ID="lblTipoDisciplina" runat="server"></asp:Label>
    </legend>
    <asp:Label ID="lblSemDisciplinasEletivas" runat="server" CssClass="summary"></asp:Label>
    <asp:Label ID="lblMensagemControleSemestral" runat="server" Visible="false"></asp:Label>
    <asp:Repeater ID="rptDisciplinasEletivas" runat="server" OnItemDataBound="rptDisciplinasEletivas_DataBound"
        Visible="false">
        <HeaderTemplate>
            <table class="tbGrid" cellspacing="0" width="100%">
                <tr class="gridHeader">
                    <th>
                        <asp:Label ID="lbl1" runat="server" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" />
                    </th>
                    <th>
                        <asp:Label ID="lbl2" runat="server" Text="Qtde. aulas semanais *" />
                    </th>
                    <th style="text-align: left">
                        <asp:Label ID="lbl4" runat="server" Text="Docente" />
                    </th>
                    <th id="tdAvaliacoesPeriodicasHeader" runat="server" visible="false">
                        <asp:Label ID="Label1" runat="server" Text="Controle semestral" />
                    </th>
                    <asp:Repeater ID="rptPeriodos" runat="server">
                        <ItemTemplate>
                            <th class="center">
                                <%#Eval("tpc_nome") %>
                            </th>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr id="trSeparator" runat="server" visible="false" class="gridAlternatingRow">
                <td id="tdSeparator" runat="server" style="height: 30px;">
                    <asp:Label ID="lblCdeNome" runat="server" Text='<%#Bind("cde_nome") %>' Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr class="gridRow">
                <td>
                    <asp:Label ID="lblCde_id" runat="server" Visible="false" Text='<%#Bind("cde_id") %>'></asp:Label>
                    <asp:Label ID="lblDis_Nome" runat="server" Text='<%#Bind("dis_nome") %>'></asp:Label>
                    <br />
                    <asp:CheckBox runat="server" ID="chkAlunoDef" 
                        Text="<%$ Resources:WebControls, TurmaDisciplina.UCRepeaterDisciplina.chkAlunoDef.Text %>" Visible="false" />
                    <asp:Label ID="lblTud_ID" runat="server" Visible="false" Text='<%#Bind("tud_id") %>'></asp:Label>
                    <asp:Label ID="lblDis_ID" runat="server" Visible="false" Text='<%#Bind("dis_id") %>'></asp:Label>
                    <asp:Label ID="lblCrd_Tipo" runat="server" Visible="false" Text='<%#Bind("crd_tipo") %>'></asp:Label>
                </td>
                <td class="center">
                    <asp:TextBox ID="txtAulaSemanal" runat="server" SkinID="Numerico" MaxLength="5" Text='<%#Bind("SomaCargaHoraria") %>'></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvAulaSemanal" ControlToValidate="txtAulaSemanal"
                        runat="server" ErrorMessage="Qtde. de aulas semanais é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>
                </td>

                <td style="text-align: left">

                    <asp:Repeater ID="rptDocentes" runat="server" OnItemDataBound="rptDocentes_ItemDataBound">
                        <ItemTemplate>
                            <asp:Label ID="lbltud_id" runat="server" Text='<%#Bind("tud_id") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblposicao" runat="server" Text='<%#Bind("posicao") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblqtdedocentes" runat="server" Text='<%#Bind("qtdedocentes") %>' Visible="false"></asp:Label>
                            <uc2:UCControleVigenciaDocentes ID="UCControleVigenciaDocentes" runat="server" />
                        </ItemTemplate>
                    </asp:Repeater>
                    <br />
                    <asp:CheckBox runat="server" ID="chkSemDocente" Text="Sem docente" Checked='<%#Bind("tud_semProfessor") %>'  />
                </td>
                <td class="center" id="tdAvaliacoesPeriodicas" runat="server" visible="false">
                    <asp:CheckBoxList ID="chkAvaliacoesPeriodicas" runat="server" Style="display: inline-block!important;"
                        RepeatLayout="Flow" DataValueField="fav_id_ava_id" DataTextField="ava_nome">
                    </asp:CheckBoxList>
                </td>
                <asp:Repeater ID="rptPeriodos" runat="server" OnItemDataBound="rptPeriodos_ItemDataBound">
                    <ItemTemplate>
                        <td class="center">
                            <asp:Label ID="lbl_tpc_id" runat="server" Visible="false" Text='<%#Bind("tpc_id") %>'></asp:Label>
                            <asp:CheckBox ID="chkPeriodo" runat="server" />
                        </td>
                    </ItemTemplate>
                </asp:Repeater>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</fieldset>
