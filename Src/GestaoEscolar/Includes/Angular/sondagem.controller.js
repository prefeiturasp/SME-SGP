'use strict';

(function (angular) {


    angular
        .module('app')
        .requires.push('chart.js');

    angular
        .module('app')
        .requires.push('angular.filter');

    angular
        .module('app')
        .controller('SondagemController', SondagemController);

    SondagemController.$inject = ['$scope', '$timeout', '$http', '$location', '$filter', 'trocarAnoService', 'groupByFilter', '$q'];

    function SondagemController($scope, $timeout, $http, $location, $filter, trocarAnoService, groupByFilter, $q) {

        this.reload = function () {
            initVars();
            getSondagens();
        };
        trocarAnoService.addSubscriber(this);

        function init() {
            configVariables();
        };

        function configVariables() {
            $scope.sondagemLoaded = false;
            $scope.baseUrl = $location.absUrl().split("/");
            $scope.site = $scope.baseUrl[0] + "//" + $scope.baseUrl[2]; // site;
            $scope.logos = core;
            $scope.api = api;

            initVars();
            getSondagens();
        };

        function initVars() {
            $scope.sondagemLoaded = false;
            $scope.listSondagens = [];
            $scope.params = params;
            $scope.graph = {}
            $scope.mensagemErro = "";
            $scope.mensagemAlerta = "";
        };

        function getSondagens() {
            if (!$scope.params || !$scope.params.alu_id || !$scope.params.ano) {
                $scope.mensagemErro = "Parâmetros inválidos";
            } else {
                var url = $scope.api + "/sondagem?alu_id=" + $scope.params.alu_id + "&ano=" + $scope.params.ano;

                $http.defaults.headers.common.Authorization = 'Bearer ' + Token;

                $http({
                    method: 'GET',
                    url: url
                }).then(function successCallback(response) {
                    if (response.data == null) {
                        $scope.mensagemErro = "Falha inesperada ao carregar as sondagens.";
                    }
                    else if (response.data && response.data.Status && response.data.Status == 1) {
                        $scope.mensagemErro = response.data.StatusDescription;
                    }
                    else {
                        try {
                            modelSondagens(response.data.sondagens);
                        }
                        catch (e) {
                            console.log(e);
                            $scope.mensagemErro = "Ocorreu um erro ao carregar as sondagens.";
                        }
                    }
                }, function errorCallback(response) {
                    if (response.status == 401) {
                        RefreshToken();
                    }else if (response.status == 404)
                        $scope.mensagemErro = "Falha ao recuperar os dados - API indisponível";
                    else if (response.status == 500)
                        $scope.mensagemErro = "Falha ao recuperar os dados - erro na API";
                    else
                        $scope.mensagemErro = "Falha inesperada ao carregar as sondagens.";
                }).finally(function () {
                    $scope.sondagemLoaded = true;
                });
            }
        };

        function modelSondagens(list) {
            var respDic = {};
            var sondRespDic = {};
            var aux = 1;
            for (var s = 0; s < list.length; s++) {
                respDic[0] = "";
                sondRespDic[{ sondId: list[s].id, respId: 0 }] = 0;
                for (var r = 0; r < list[s].respostas.length; r++) {
                    respDic[aux] = list[s].respostas[r].descricao;
                    sondRespDic[parseInt(list[s].id.toString() + "0" + list[s].respostas[r].id.toString())] = aux;
                    aux++;
                }
            }

            for (var s = 0; s < list.length; s++) {
                var respostas = []
                var graphLabels = [];
                var graphSeries = [];
                var graphData = [];
                for (var a = 0; a < list[s].agendamentos.length; a++) {

                    if (graphLabels.indexOf(list[s].agendamentos[a].dataInicio) == -1) {
                        graphLabels.push(list[s].agendamentos[a].dataInicio);
                    }

                    for (var r = 0; r < list[s].agendamentos[a].respostasAluno.length; r++) {

                        var questao = $filter('filter')(list[s].questoes, { id: list[s].agendamentos[a].respostasAluno[r].idQuestao }, true);

                        if (questao.length) {
                            var subQuestao = $filter('filter')(list[s].subQuestoes, { id: list[s].agendamentos[a].respostasAluno[r].idSubQuestao }, true);
                            var resposta = $filter('filter')(list[s].respostas, { id: list[s].agendamentos[a].respostasAluno[r].idResposta }, true);

                            if (subQuestao.length) {

                                if (graphSeries.indexOf("Questão: " + questao[0].descricao + " | Subquestão: " + subQuestao[0].descricao) == -1) {
                                    graphSeries.push("Questão: " + questao[0].descricao + " | Subquestão: " + subQuestao[0].descricao);
                                }

                                if (resposta.length) {
                                    respostas.push({ id: questao[0].id, subQuestao: subQuestao[0].descricao, resposta: resposta[0].descricao })
                                    graphData.push({ questao: "Questão: " + questao[0].descricao + " | Subquestão: " + subQuestao[0].descricao, resposta: resposta[0].descricao, idResposta: resposta[0].id });
                                }
                                else {
                                    respostas.push({ id: questao[0].id, subQuestao: subQuestao[0].descricao, resposta: "" })
                                    graphData.push({ questao: "Questão: " + questao[0].descricao + " | Subquestão: " + subQuestao[0].descricao, resposta: "", idResposta: 0 });
                                }
                            }
                            else {

                                if (graphSeries.indexOf("Questão: " + questao[0].descricao) == -1) {
                                    graphSeries.push("Questão: " + questao[0].descricao);
                                }

                                if (resposta.length) {
                                    respostas.push({ id: questao.id[0], subQuestao: questao[0].descricao, resposta: resposta[0].descricao })
                                    graphData.push({ questao: "Questão: " + questao[0].descricao, resposta: resposta[0].descricao, idResposta: resposta[0].id });
                                }
                                else {
                                    respostas.push({ id: questao.id[0], subQuestao: questao[0].descricao, resposta: "" })
                                    graphData.push({ questao: "Questão: " + questao[0].descricao, resposta: "", idResposta: 0 });
                                }
                            }
                        }
                    }
                }

                for (var q = 0; q < list[s].questoes.length; q++) {
                    list[s].questoes[q]["respostas"] = $filter('filter')(respostas, { id: list[s].questoes[q].id }, true);
                }

                var dadosAgrup = $filter('toArray')($filter('groupBy')(graphData, 'questao'), true);
                var graphDataAgroup = [];
                for (var d = 0; d < dadosAgrup.length; d++) {
                    var resp = [];
                    for (var v = 0; v < dadosAgrup[d].length; v++) {
                        resp.push(parseInt(sondRespDic[list[s].id.toString() + "0" + dadosAgrup[d][v].idResposta.toString()]));
                    }
                    graphDataAgroup.push(resp);
                }

                list[s]["graphLabels"] = graphLabels;
                list[s]["graphSeries"] = graphSeries;
                list[s]["graphData"] = graphDataAgroup;
                list[s]["graphDatasetOverride"] = [{ yAxisID: 'yaxes' }];
                list[s]["graphOptions"] = {
                    responsive: true,
                    scales: {
                        xAxes: [{
                            display: true,
                            scaleLabel: {
                                display: true,
                                labelString: 'Data de agendamento'
                            },
                            gridLines: {
                                offsetGridLines: true
                            }
                        }],
                        yAxes: [
                            {
                                id: 'yaxes',
                                type: 'linear',
                                display: true,
                                position: 'left',
                                scaleLabel: {
                                    display: true,
                                    labelString: 'Respostas'
                                },
                                ticks: {
                                    userCallback: function (value, index, values) {
                                        if (respDic[value]) {
                                            return respDic[value];
                                        }
                                        else {
                                            return "";
                                        }
                                    }
                                }
                            }
                        ]
                    },
                    legend: {
                        display: true,
                        position: 'top'
                    },
                    tooltips: {
                        callbacks: {
                            label: function (tooltipItem, data) {
                                console.log(data.datasets[tooltipItem.datasetIndex]);
                                var resp = {};
                                if (respDic[data.datasets[tooltipItem.datasetIndex].data[0]]) {
                                    resp = " -> " + respDic[data.datasets[tooltipItem.datasetIndex].data[0]];
                                }
                                else {
                                    resp = "";
                                }
                                return data.datasets[tooltipItem.datasetIndex].label + resp;
                            }
                        }
                    }
                };
            }

            $scope.listSondagens = list;
        }

        function getToken() {
            var deferred = $q.defer();
            $http({
                method: "POST",
                url: "RelatorioPedagogico.aspx/CreateToken",
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
