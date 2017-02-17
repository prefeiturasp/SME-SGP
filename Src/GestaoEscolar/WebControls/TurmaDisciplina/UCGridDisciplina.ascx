<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_TurmaDisciplina_UCGridDisciplina"
    CodeBehind="UCGridDisciplina.ascx.cs" %>
<%@ Import Namespace="MSTech.CoreSSO.BLL" %>
<%@ Register Src="~/WebControls/Combos/UCComboDocente.ascx" TagName="_UCComboDocente"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/ControleVigenciaDocentes/UCControleVigenciaDocentes.ascx"
    TagName="UCControleVigenciaDocentes" TagPrefix="uc2" %>
<fieldset>
    <legend>
        <asp:Label ID="lblTipoDisciplina" runat="server"></asp:Label>
    </legend>
    <asp:Label ID="lblMensagemControleSemestral" runat="server" Visible="false">
    </asp:Label>
    <asp:GridView ID="gvDisciplina" runat="server" AutoGenerateColumns="False" DataKeyNames="dis_id,tds_id,tud_id,crd_tipo,dis_nome"
        OnRowDataBound="gvDisciplina_RowDataBound">
        <Columns>
            <%--<asp:BoundField DataField="dis_nome" HeaderText="[MSG_DISCIPLINA]" />--%>
            <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                    <asp:Label ID="lblDisp" runat="server" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblDispNome" runat="server" Text='<%#Bind("dis_nome") %>' />
                    <br />
                    <asp:CheckBox runat="server" ID="chkAlunoDef" 
                        Text="<%$ Resources:WebControls, TurmaDisciplina.UCGridDisciplina.chkAlunoDef.Text %>" Visible="false" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                    <asp:Label ID="lbl" runat="server" Text="Qtde. aulas semanais *" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:TextBox ID="txtAulaSemanal" runat="server" SkinID="Numerico" MaxLength="5" Text='<%#Bind("SomaCargaHoraria") %>'></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvAulaSemanal" ControlToValidate="txtAulaSemanal"
                        runat="server" ErrorMessage="Qtde. de aulas semanais é obrigatório." Display="Dynamic">*
                    </asp:RequiredFieldValidator>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Docente" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Repeater ID="rptDocentes" runat="server" OnItemDataBound="rptDocentes_ItemDataBound">
                        <ItemTemplate>
                            <asp:Label ID="lbltud_id" runat="server" Text='<%#Bind("tud_id") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblposicao" runat="server" Text='<%#Bind("posicao") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblqtdedocentes" runat="server" Text='<%#Bind("qtdedocentes") %>'
                                Visible="false"></asp:Label>
                            <uc2:UCControleVigenciaDocentes ID="UCControleVigenciaDocentes" runat="server" />
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:CheckBox runat="server" ID="chkSemDocente" Text="Sem docente" Checked='<%#Bind("tud_semProfessor") %>' SkinID="checkboxSemProfessor" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Controle semestral" Visible="false" ItemStyle-HorizontalAlign="Center"
                HeaderStyle-CssClass="center">
                <ItemTemplate>
                    <asp:CheckBoxList ID="chkAvaliacoesPeriodicas" runat="server" Style="display: inline-block!important;"
                        RepeatLayout="Flow" RepeatDirection="Horizontal" DataValueField="fav_id_ava_id"
                        DataTextField="ava_nome">
                    </asp:CheckBoxList>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</fieldset>