'use strict';

(function (angular) {
    
    angular
		.module('app', []);

    angular
        .module('app')
        .requires.push('chart.js');

    angular
        .module('app')
        .controller('GraficoAtendimentoController', GraficoAtendimentoController);

    GraficoAtendimentoController.$inject = ['$scope', '$timeout', '$http', '$location', '$q'];

    function GraficoAtendimentoController($scope, $timeout, $http, $location, $q) {
        function init() {
            configVariables();
        };

        $scope.getGraphLabels = _.memoize(function () {
            return $scope.graphLabels;
        });

        $scope.getGraphSeries = _.memoize(function () {
            return $scope.graphSeries;
        });

        $scope.getGraphData = _.memoize(function () {
            return $scope.graphData;
        });

        $scope.getGraphOptions = _.memoize(function () {
            return $scope.graphOptions;
        });

        $scope.exibeGrafico = function (tipo) {
            return tipo.toString() == $scope.TipoGrafico.toString();
        };

        $scope.getGraphDatasetOverride = _.memoize(function () {
            return $scope.graphDatasetOverride
        });

        function configVariables() {
            $scope.baseUrl = $location.absUrl().split("/");
            $scope.site = $scope.baseUrl[0] + "//" + $scope.baseUrl[2]; // site;
            $scope.logos = core;
            $scope.api = api;

            $scope.urlRetorno = urlRetorno;

            $scope.imprimir = function () {
                window.print();
            }

            $scope.voltar = function () {
                window.top.location = $scope.urlRetorno;
            }

            initVars();
            getGrafico();
        };

        function initVars() {
            $scope.loaded = false;
            $scope.params = params;
            $scope.graphLabels = [];
            $scope.graphSeries = [];
            $scope.graphData = [];
            $scope.graphOptions = {};
            $scope.graphDatasetOverride = [];
            $scope.TipoGrafico = 0;
            $scope.mensagemErro = "";
            $scope.mensagemAlerta = "";
        }

        function getGrafico() {
            if (!$scope.params || !$scope.params.gra_id || !$scope.params.esc_id || !$scope.params.uni_id) {
                $scope.mensagemErro = "Parâmetros inválidos";
            } else {
                var url = $scope.api + "/graficoAtendimento?gra_id=" + $scope.params.gra_id + "&esc_id=" + $scope.params.esc_id + "&uni_id=" + $scope.params.uni_id + "&cur_id=" + $scope.params.cur_id + "&crr_id=" + $scope.params.crr_id + "&crp_id=" + $scope.params.crp_id;

                $http.defaults.headers.common.Authorization = 'Bearer ' + Token;

                $http({
                    method: 'GET',
                    url: url
                }).then(function successCallback(response) {
                    //console.log(response);
                    if (response.data == null) {
                        $scope.mensagemErro = "Falha inesperada ao carregar o gráfico.";
                    }
                    else if (response.data.length == 0) {
                        $scope.mensagemAlerta = "Dados não encontrados.";
                    }
                    else if (response.data && response.data.Status && response.data.Status == 1) {
                        $scope.mensagemErro = response.data.StatusDescription;
                    }
                    else {
                        try {
                            modelGrafico(response.data);
                        }
                        catch (e) {
                            console.log(e);
                            $scope.mensagemErro = "Ocorreu um erro ao carregar o gráfico.";
                        }
                    }
                }, function errorCallback(response) {
                    if (response.status == 401) {
                        RefreshToken();
                    } else if (response.status == 404)
                        $scope.mensagemErro = "Falha ao recuperar os dados - API indisponível";
                    else if (response.status == 500)
                        $scope.mensagemErro = "Falha ao recuperar os dados - erro na API";
                    else
                        $scope.mensagemErro = "Falha inesperada ao carregar o gráfico.";
                }).finally(function () {
                    $scope.loaded = true;
                });;
            }
        }

        function modelGrafico(data) {
            if (data.Dados.length > 0) {
                var dados = [];
                $scope.graphSeries.push(data.Dados[0].Serie);
                for (var i = 0; i < data.Dados.length; i++) {
                    $scope.graphLabels.push(data.Dados[i].Label);
                    dados.push(data.Dados[i].Valor);
                }

                $scope.graphData.push(dados);

                $scope.graphOptions = {
                    responsive: true,
                    scales: {
                        xAxes: [{
                            display: true,
                            scaleLabel: {
                                display: true,
                                labelString: data.NomeEixoAgrupamento
                            }
                            ,
                            ticks: {
                                maxRotation: 45,
                                minRotation: 45
                            }
                        }],
                        yAxes: [
                            {
                                id: 'yaxes',
                                type: 'linear',
                                display: true,
                                position: 'left',
                                scaleLabel: {
                                    display: true
                                },
                                ticks: {
                                    beginAtZero: true,
                                    callback: function (value) { if (Number.isInteger(value)) { return value; } },
                                }
                            }
                        ]
                    },
                    legend: {
                        display: true,
                        position: 'top'
                    }
                }

                $scope.TipoGrafico = data.TipoGrafico;

                $scope.graphDatasetOverride = [{ yAxisID: 'yaxes' }];
            }
        }

        function getToken() {
            var deferred = $q.defer();
            $http({
                method: "POST",
                url: "GraficoAtendimento.aspx/CreateToken",
                dataType: 'json',
                data: '{ "usuario":  "' + Usuario + '", "entidade": "' + Entidade + '", "grupo": "' + Grupo + '" }',
                headers: {
                    "Content-Type": "application/json"
                }
            }).success(function (data) {
                deferred.resolve(data);
            });

            return deferred.promise;
        }

        function RefreshToken() {
            var promise = getToken();
            promise.then(function (data) {
                Token = data.d;
                initVars();
                getSondagens();
            });
        }


        /**
		 * @function Forçar a atualização (diggest) do angular
		 * @name safeApply
		 * @namespace FileController
		 * @memberOf Controller
		 * @public
		 * @param
		 * @return
		 */
        $scope.safeApply = function __safeApply() {
            var $scope, fn, force = false;
            if (arguments.length === 1) {
                var arg = arguments[0];
                if (typeof arg === 'function') {
                    fn = arg;
                } else {
                    $scope = arg;
                }
            } else {
                $scope = arguments[0];
                fn = arguments[1];
                if (arguments.length === 3) {
                    force = !!arguments[2];
                }
            }
            $scope = $scope || this;
            fn = fn || function () { };

            if (force || !$scope.$$phase) {
                $scope.$apply ? $scope.$apply(fn) : $scope.apply(fn);
            } else {
                fn();
            }
        };

        init();
    }
})(angular);
