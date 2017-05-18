'use strict';

(function (angular) {

    angular
        .module('app')
        .controller('SondagemController', SondagemController);

    SondagemController.$inject = ['$scope', '$timeout', '$http', '$location', '$filter', 'trocarAnoService'];

    function SondagemController($scope, $timeout, $http, $location, $filter, trocarAnoService) {
        this.reload = function () {
            initVars();
            getSondagens();
        };
        trocarAnoService.addSubscriber(this);

        function init() {
            configVariables();
        };

        function configVariables() {
            $scope.baseUrl = $location.absUrl().split("/");
            $scope.site = $scope.baseUrl[0] + "//" + $scope.baseUrl[2]; // site;
            $scope.logos = core;
            $scope.api = api;

            initVars();
            getSondagens();
        };

        function initVars() {
            $scope.listSondagens = [];
            $scope.params = params;
            $scope.mensagemErro = "";
            $scope.mensagemAlerta = "";
        };

        function getSondagens() {
            if (!$scope.params || !$scope.params.alu_id || !$scope.params.ano) {
                $scope.mensagemErro = "Parâmetros inválidos";
            } else {
                var url = $scope.api + "/sondagem?alu_id=" + $scope.params.alu_id + "&ano=" + $scope.params.ano;

                $http({
                    method: 'GET',
                    url: url
                }).then(function successCallback(response) {
                    if (response.data == null) {
                        $scope.mensagemErro = "Falha inesperada ao carregar as sondagens.";
                    }
                    else if (response.data[0] && response.data[0].Status && response.data[0].Status == 1) {
                        $scope.mensagemErro = response.data[0].StatusDescription;
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
                    if (response.status == 404)
                        $scope.mensagemErro = "Falha ao recuperar os dados - API indisponível";
                    else if (response.status == 500)
                        $scope.mensagemErro = "Falha ao recuperar os dados - erro na API";
                    else
                        $scope.mensagemErro = "Falha inesperada ao carregar as sondagens.";
                });
            }
        };

        function modelSondagens(list) {
            for (var s = 0; s < list.length; s++) {
                var respostas = []

                for (var a = 0; a < list[s].agendamentos.length; a++) {
                    for (var r = 0; r < list[s].agendamentos[a].respostasAluno.length; r++) {

                        var questao = $filter('filter')(list[s].questoes, { id: list[s].agendamentos[a].respostasAluno[r].idQuestao }, true);

                        if (questao.length) {

                            var subQuestao = $filter('filter')(list[s].subQuestoes, { id: list[s].agendamentos[a].respostasAluno[r].idSubQuestao }, true);
                            var resposta = $filter('filter')(list[s].respostas, { id: list[s].agendamentos[a].respostasAluno[r].idResposta }, true);

                            if (subQuestao.length) {
                                if (resposta.length) {
                                    respostas.push({ id: questao[0].id, subQuestao: subQuestao[0].descricao, resposta: resposta[0].descricao })
                                }
                                else {
                                    respostas.push({ id: questao[0].id, subQuestao: subQuestao[0].descricao, resposta: "" })
                                }
                            }
                            else {
                                if (resposta.length) {
                                    respostas.push({ id: questao.id[0], subQuestao: questao[0].descricao, resposta: resposta[0].descricao })
                                }
                                else {
                                    respostas.push({ id: questao.id[0], subQuestao: questao[0].descricao, resposta: "" })
                                }
                            }
                        }
                    }
                }

                for (var q = 0; q < list[s].questoes.length; q++) {
                    list[s].questoes[q]["respostas"] = $filter('filter')(respostas, { id: list[s].questoes[q].id }, true);
                }
            }

            $scope.listSondagens = list;
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
