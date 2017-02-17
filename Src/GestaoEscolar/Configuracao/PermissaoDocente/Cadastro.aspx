<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.PermissaoDocente.Cadastro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .tblBol {
            border: 1px solid #ccc;
            border-spacing: 0px;
            border-collapse: collapse;
            background-color: #fff;
            width: 100%;
            max-width: 100%;
        }

            .tblBol th {
                padding: 5px;
                border-right: 1px solid #ccc;
                background-color: #EEE;
                font-weight: normal;
                min-width: 48px;
            }

            .tblBol td {
                padding: 5px;
                border-right: 1px solid #ccc;
                background-color: #fff;
            }

            .tblBol th.nomePeriodo {
                font-weight: bold;
                text-align: center;
                background: #6D93D0;
                color: #fff;
            }

            .tblBol th.nomePeriodoColunas {
                text-align: center;
                background: #195383;
                color: #fff;
            }

            .tblBol td.nomeDisciplina, .tblBol th.nomeDisciplina {
                font-weight: bold;
            }

            .tblBol td span {
                text-align: left;
            }

            .table-responsive {
              min-height: .01%;
              overflow-x: auto;
            }

            .table-responsive {
                width: 100%;
                margin-bottom: 15px;
                overflow-y: hidden;
                -ms-overflow-style: -ms-autohiding-scrollbar;
                border: 1px solid #ddd;
            }
            .table-responsive > .table {
                margin-bottom: 0;
            }
            .table-responsive > .table > thead > tr > th,
            .table-responsive > .table > tbody > tr > th,
            .table-responsive > .table > tfoot > tr > th,
            .table-responsive > .table > thead > tr > td,
            .table-responsive > .table > tbody > tr > td,
            .table-responsive > .table > tfoot > tr > td {
                white-space: nowrap;
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updCadastroQualidade" runat="server">
        <ContentTemplate>
            <script>
                $(function () {
                    $(".table-responsive").hide().width($(".table-responsive").prev().outerWidth() - 10).show();
                    $(window).resize(function () {
                        $(".table-responsive").hide().width($(".table-responsive").prev().outerWidth() - 10).show();
                    });
                });
            </script>
            <fieldset>
                <legend>Cadastro de permissões dos docentes</legend>
                <div></div>

                        <div class="table-responsive">
                            <table class="tblBol table" rules="none">
                                <thead>
                                    <tr>
                                        <th rowspan="2" class="nomePeriodo" style="min-width: 80px;"></th>
                                        <asp:Repeater ID="rptDiciplinasEdiCons" runat="server">
                                            <ItemTemplate>
                                                <th class="nomePeriodo" colspan="2">
                                                    <%# this.GetDataItem().ToString() %>
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                        <asp:Repeater ID="rptDisciplinasConsPerm" runat="server">
                                            <ItemTemplate>
                                                <th class="nomePeriodo">
                                                    <%# this.GetDataItem().ToString() %>
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                    <tr>
                                        <asp:Repeater ID="rptVisaoEdicCons" runat="server">
                                            <ItemTemplate>
                                                <th class="nomePeriodoColunas">Consulta
                                                </th>
                                                <th class="nomePeriodoColunas">Edição 
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                        <asp:Repeater ID="rptVisaoCons" runat="server">
                                            <ItemTemplate>
                                                <th class="nomePeriodoColunas">Consulta
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptTipoDocente" runat="server">
                                        <ItemTemplate>
                                            <tr class="permissao">
                                                <td id="tdNomeDiciplina" runat="server" class="nomeDisciplina">
                                                    <%#Eval("DescricaoTipo")%>
                                                    <asp:HiddenField runat="server" ID="hdfTipoDocente" Value='<%#Eval("IdTipoDocente")%>' />
                                                </td>
                                                <asp:Repeater ID="rptSelecionarConsultaEdicao" runat="server" DataSource='<%#Eval("DocentesPermissoesConsEdic") %>'
                                                    OnItemDataBound="rptSelecionarConsultaEdicao_ItemDataBound">
                                                    <ItemTemplate>
                                                        <td class="permissao">
                                                            <asp:HiddenField runat="server" ID="hdfModuloConsEdic" Value='<%#Eval("IdModuloPermissao")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloConsEdicIdTitular" Value='<%#Eval("PdcIdTitular")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloConsEdicIdCompartilhado" Value='<%#Eval("PdcIdCompartilhado")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloConsEdicIdProjeto" Value='<%#Eval("PdcIdProjeto")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloConsEdicIdSubstituto" Value='<%#Eval("PdcIdSubstituto")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloConsEdicIdEspecial" Value='<%#Eval("PdcIdEspecial")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloConsEdicIdSegundo" Value='<%#Eval("PdcIdSegundo")%>' />

                                                            <asp:CheckBox ID="chkConsTitu" Text="T" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("ConsultaTitular")) == 1)? true: false %>' />
                                                            <asp:CheckBox ID="chkConsComp" Text="C" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("ConsultaCompartilhado")) == 1)? true: false %>' />
                                                            <asp:CheckBox ID="chkConsProj" Text="P" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("ConsultaProjeto")) == 1)? true: false %>' />
                                                            <asp:CheckBox ID="chkConsSubs" Text="S" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("ConsultaSubstituto")) == 1)? true: false %>' />
                                                            <asp:CheckBox ID="chkConsEspe" Text="E" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("ConsultaEspecial")) == 1)? true: false %>' />
                                                            <asp:CheckBox ID="chkConsSegu" Text="ST" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("ConsultaSegundo")) == 1)? true: false %>' />
                                                        </td>
                                                        <td class="permissao">
                                                            <asp:CheckBox ID="chkEdicTitu" Text="T" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("EditarTitular")) == 1)? true: false %>' />
                                                            <asp:CheckBox ID="chkEdicComp" Text="C" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("EditarCompartilhado")) == 1)? true: false %>' />
                                                            <asp:CheckBox ID="chkEdicProj" Text="P" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("EditarProjeto")) == 1)? true: false %>' />
                                                            <asp:CheckBox ID="chkEdicSubs" Text="S" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("EditarSubstituto")) == 1)? true: false %>' />
                                                            <asp:CheckBox ID="chkEdicEspe" Text="E" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("EditarEspecial")) == 1)? true: false %>' />
                                                            <asp:CheckBox ID="chkEdicSegu" Text="ST" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("EditarSegundo")) == 1)? true: false %>' />
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                                <asp:Repeater ID="rptSelecionarPermissaoConsEdic" runat="server" DataSource='<%#Eval("DocentesPermissoesPermConsEdic") %>'>
                                                    <ItemTemplate>
                                                        <td class="permissao">
                                                            <asp:HiddenField runat="server" ID="hdfModuloPermConsEdic" Value='<%#Eval("IdModuloPermissao")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloPermConsEdicIdTitular" Value='<%#Eval("PdcIdTitular")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloPermConsEdicIdCompartilhado" Value='<%#Eval("PdcIdCompartilhado")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloPermConsEdicIdProjeto" Value='<%#Eval("PdcIdProjeto")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloPermConsEdicIdSubstituto" Value='<%#Eval("PdcIdSubstituto")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloPermConsEdicIdEspecial" Value='<%#Eval("PdcIdEspecial")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloPermConsEdicIdSegundo" Value='<%#Eval("PdcIdSegundo")%>' />

                                                            <asp:CheckBox ID="chkConsTitu" Text="Sim" runat="server"
                                                                Checked='<%# (Convert.ToInt32(Eval("ConsultaTitular")) == 1)? true: false %>' />
                                                        </td>
                                                        <td class="permissao">
                                                            <asp:CheckBox ID="chkEdicTitu" Text="Sim" runat="server"
                                                                Checked='<%# (Convert.ToInt32(Eval("EditarTitular")) == 1)? true: false %>' />
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                                <asp:Repeater ID="rptSelecionarPermissaoCons" runat="server" DataSource='<%#Eval("DocentesPermissoesPermCons") %>'>
                                                    <ItemTemplate>
                                                        <td class="permissao">
                                                            <asp:HiddenField runat="server" ID="hdfModuloPermCons" Value='<%#Eval("IdModuloPermissao")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloPermConsModuloPermConsEdicIdTitular" Value='<%#Eval("PdcIdTitular")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloPermConsModuloPermConsEdicIdCompartilhado" Value='<%#Eval("PdcIdCompartilhado")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloPermConsModuloPermConsEdicIdProjeto" Value='<%#Eval("PdcIdProjeto")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloPermConsModuloPermConsEdicIdSubstituto" Value='<%#Eval("PdcIdSubstituto")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloPermConsModuloPermConsEdicIdEspecial" Value='<%#Eval("PdcIdEspecial")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloPermConsModuloPermConsEdicIdSegundo" Value='<%#Eval("PdcIdSegundo")%>' />

                                                            <asp:CheckBox ID="chkConsTitu" Text="Sim" runat="server"
                                                                Checked='<%# (Convert.ToInt32(Eval("ConsultaTitular")) == 1)? true: false %>' />
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                                <asp:Repeater ID="rptSelecionarConsulta" runat="server" DataSource='<%#Eval("DocentesPermissoesCons") %>'
                                                    OnItemDataBound="rptSelecionarConsulta_ItemDataBound">
                                                    <ItemTemplate>
                                                        <td class="permissao">
                                                            <asp:HiddenField runat="server" ID="hdfModuloCons" Value='<%#Eval("IdModuloPermissao")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloConsModuloPermConsModuloPermConsEdicIdTitular" Value='<%#Eval("PdcIdTitular")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloConsModuloPermConsModuloPermConsEdicIdCompartilhado" Value='<%#Eval("PdcIdCompartilhado")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloConsModuloPermConsModuloPermConsEdicIdProjeto" Value='<%#Eval("PdcIdProjeto")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloConsModuloPermConsModuloPermConsEdicIdSubstituto" Value='<%#Eval("PdcIdSubstituto")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloConsModuloPermConsModuloPermConsEdicIdEspecial" Value='<%#Eval("PdcIdEspecial")%>' />
                                                            <asp:HiddenField runat="server" ID="hdfModuloConsModuloPermConsModuloPermConsEdicIdSegundo" Value='<%#Eval("PdcIdSegundo")%>' />

                                                            <asp:CheckBox ID="chkConsTitu" Text="T" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("ConsultaTitular")) == 1)? true: false %>' />
                                                            <asp:CheckBox ID="chkConsComp" Text="C" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("ConsultaCompartilhado")) == 1)? true: false %>' />
                                                            <asp:CheckBox ID="chkConsProj" Text="P" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("ConsultaProjeto")) == 1)? true: false %>' />
                                                            <asp:CheckBox ID="chkConsSubs" Text="S" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("ConsultaSubstituto")) == 1)? true: false %>' />
                                                            <asp:CheckBox ID="chkConsEspe" Text="E" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("ConsultaEspecial")) == 1)? true: false %>' />
                                                            <asp:CheckBox ID="chkConsSegu" Text="ST" runat="server" Visible="false"
                                                                Checked='<%# (Convert.ToInt32(Eval("ConsultaSegundo")) == 1)? true: false %>' />
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>

                </div>
                <div align="right">
                    <asp:Button ID="bntSalvar" runat="server" Text="Salvar" OnClick="bntSalvar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                        OnClick="btnCancelar_Click" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
