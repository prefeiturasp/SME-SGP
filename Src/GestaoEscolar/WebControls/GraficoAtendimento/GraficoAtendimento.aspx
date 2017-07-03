<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GraficoAtendimento.aspx.cs" Inherits="GestaoEscolar.WebControls.GraficoAtendimento.GraficoAtendimento" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <script src="../../Includes/underscore-min.js" type="text/javascript"></script>
    <script src="../../Includes/Charts/Chart.min.js"></script>
    <script src="../../Includes/jquery-2.0.3.min.js" type="text/javascript"></script>
    <script src="../../Includes/Angular/angular.js" type="text/javascript"></script>
    <script src="../../Includes/Angular/module.js" type="text/javascript"></script>
    <script src="../../Includes/Angular/angular-charts/angular-chart.min.js"></script>
    <script src="../../Includes/Angular/graficoAtendimento.controller.js" type="text/javascript"></script>
    <script type="text/javascript">
        var params =
            {
                gra_id : <%= gra_id %>,
                esc_id : <%= esc_id %>,
                uni_id: <%= uni_id %>,
                cur_id: <%= cur_id %>,
                crr_id: <%= crr_id %>,
                crp_id: <%= crp_id %>
                };

        var site = '';
        var core = '<%= MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB.UrlCoreSSO %>';
        var api = '<%= MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB.UrlGestaoAcademicaWebApi %>';

        var Usuario = '<%= Usuario %>';
        var Entidade = '<%= Entidade %>';
        var Grupo = '<%= Grupo %>';
        var Token = '<%= Token %>';
        var urlRetorno = '<%= URL_Retorno %>'
    </script>
    <style>
        @media print {
            @page {
                margin-top: 10mm;
                margin-bottom: auto;
                margin-left: 15mm;
                margin-right: 15mm;
                size: A4 landscape;
            }

            html, body {
                width: 297mm;
                height: 210mm;
            }
        }
    </style>
    <form id="form1" runat="server">
        <div class="graficoAtendimento">
            <asp:Panel ID="pnlGrafico" runat="server">
                <div ng-app="app">
                    <div ng-controller="GraficoAtendimentoController" ng-cloak>
                        <div style="float: right; margin-right: 5px;" ng-if="loaded">
                            <input type="button" class="btn" value="Voltar"  ng-click="voltar()"  />
                            <input type="button" class="btn" value="Imprimir" ng-click="imprimir()" />
                        </div>
                        <div class="grafico-nome">
                            <asp:Label ID="lblNomeGrafico" runat="server"></asp:Label>
                        </div>
                        <div class="grafico-cabecalho">
                            <asp:Label ID="lblCabecalho" runat="server"></asp:Label>
                        </div>
                        <div class="loader" ng-if="!mensagemErro && !mensagemAlerta && !loaded">
                            <img class="imgLoader" src="../../App_Themes/IntranetSME/images/ajax-loader.gif" style="border-width: 0px;">
                        </div>
                        <div ng-if="mensagemErro" class="summary" style="background: #fff url(/App_Themes/IntranetSME/images/error.png) no-repeat 45px 50%;">
                            {{mensagemErro}}
                        </div>
                        <div ng-if="mensagemAlerta" class="summary">
                            {{mensagemAlerta}}
                        </div>
                        <div id="graficoBarra" ng-if="exibeGrafico(1) && loaded">
                            <canvas id="grafico" class="chart chart-bar" chart-data="getGraphData()"
                                chart-labels="getGraphLabels()" chart-series="getGraphSeries()" chart-options="getGraphOptions()" chart-dataset-override="getGraphDatasetOverride()"></canvas>
                        </div>
                        <div id="graficoPizza" ng-if="exibeGrafico(2) && loaded">
                            <canvas id="grafico" class="chart chart-radar" chart-data="getGraphData()"
                                chart-labels="getGraphLabels()" chart-series="getGraphSeries()" chart-options="getGraphOptions()" chart-dataset-override="getGraphDatasetOverride()"></canvas>
                        </div>
                        <div style="float: right; margin-right: 5px;" ng-if="loaded">
                            <input type="button" class="btn" value="Voltar"  ng-click="voltar()"  />
                            <input type="button" class="btn" value="Imprimir" ng-click="imprimir()" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
