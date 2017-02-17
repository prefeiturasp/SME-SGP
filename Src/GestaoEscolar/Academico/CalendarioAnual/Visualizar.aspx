<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Academico_CalendarioAnual_Visualizar" CodeBehind="Visualizar.aspx.cs" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="ComboUAEscola"
    TagPrefix="uc1" %>
<%@ PreviousPageType VirtualPath="~/Academico/CalendarioAnual/Busca.aspx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary" runat="server" />
    <fieldset class="divInformacao">
        <legend>Calendário escolar</legend>
        <asp:Label ID="lblCalendario" runat="server"></asp:Label>
        <br />
        <asp:Label ID="lblAnoLetivo" runat="server"></asp:Label>
        <br />
        <asp:Label ID="lblDiasLetivosNoAno" runat="server"></asp:Label>
        <br />
        <asp:Repeater ID="rptDiasLetivos" runat="server">
            <ItemTemplate>
                <asp:Label ID="lblDiasLetivosPorPeriodo" runat="server" Text='<%# Eval("periodo","Dias letivos no {0}: ") + Eval("dias","<b>{0}</b>") %>'></asp:Label>
                <br />
            </ItemTemplate>
        </asp:Repeater>
    </fieldset>
    <fieldset>
        <legend>Filtro de eventos do calendário escolar</legend>
        <asp:Label ID="lblTipoEvento" runat="server" Text="Tipo de evento" AssociatedControlID="ddlComboTipoEvento"></asp:Label>
        <asp:DropDownList ID="ddlComboTipoEvento" runat="server" SkinID="text60C" AutoPostBack="true"
            OnSelectedIndexChanged="ddlComboTipoEvento_SelectedIndexChanged">
            <asp:ListItem Text="<%$ Resources:Academico, CalendarioAnual.Visualizar.ddlComboTipoEvento.EventoPadrao.Text %>" Value="0" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Eventos da escola" Value="2"></asp:ListItem>
        </asp:DropDownList>
        <div runat="server" id="divEscolas" visible="false">
            <uc1:ComboUAEscola ID="ucComboUAEscola" runat="server" CarregarEscolaAutomatico="true"
                ObrigatorioEscola="false" ObrigatorioUA="false" MostrarMessageSelecioneEscola="true"
                MostrarMessageSelecioneUA="true" />
        </div>
        <div runat="server" class="right" id="divBtnPesquisar" visible="false">
            <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click" />
        </div>
    </fieldset>
    <fieldset runat="server" id="fdsVisualizacao" visible="false">
        <legend>Visualização do calendário escolar</legend>
        <div class="area-form area-calendario">
            <div class="divScrollResponsivo" align="center">
                <asp:Repeater ID="rptCalendarios" runat="server" OnItemDataBound="rptCalendarios_ItemDataBound">
                    <HeaderTemplate>
                        <table border="0" cellpadding="0" cellspacing="20" width="910px">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# (Container.ItemIndex % 4 == 0) ? @"</tr><tr>" : string.Empty %>
                        <td style="vertical-align: top;">
                            <asp:Calendar ID="calMesPeriodo" runat="server" OnDayRender="calMesPeriodo_DayRender"
                                ShowNextPrevMonth="False" VisibleDate='<%#Bind("mes") %>' SelectionMode="None"
                                BorderColor="Black"></asp:Calendar>
                            <asp:Repeater ID="rptEventos" runat="server">
                                <ItemTemplate>
                                    <div align="left" style="width: 200px;">
                                        <asp:Label ID="lblEvento" runat="server" Text='<%#Bind("periodo") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <div style="font-weight: bold;">
                Legenda:
            </div>        
            <div id="divLegenda" runat="server" style="border-style: solid; border-width: thin; width: 230px; border-collapse: separate !important; border-spacing: 2px !important;">
                <%--criado no code behind--%>
            </div>
        </div>
        <div align="right" class="area-botoes-bottom">
            <asp:Button ID="_btnVoltar" runat="server" Text="Voltar" OnClick="_btnVoltar_Click" />
        </div>
    </fieldset>
</asp:Content>