'use strict';
/**
 * function BoletimController
 * @namespace Controller
 */
(function (angular) {

	//~SETTER
	angular
		.module('app', []);

	//~GETTER
	angular
		.module('app')
		.controller("BoletimController", BoletimController);


	BoletimController.$inject = ['$scope', '$timeout', '$http', '$location', '$q'];

	function BoletimController($scope, $timeout, $http, $location, $q) {

		function init() {
			configVariables();
		};

		/**
		 * @function 
		 * @name 
		 * @namespace BoletimController
		 * @memberOf Controller
		 * @private
		 * @param
		 * @return
		 */
		function configVariables() {

			$scope.baseUrl = $location.absUrl().split("/");
			$scope.site = $scope.baseUrl[0] + "//" + $scope.baseUrl[2]; // site;
			$scope.logos = core;
			$scope.api = api;

			$scope.trocarTpc = function (tpc) {
				initVars();
				$scope.params.TpcId = tpc;
				getBoletins();
			}

			$scope.imprimir = function () {
				window.print();
			}

			initVars();
			getBoletins();
		};

		function initVars() {
			$scope.listBoletins = [];
			$scope.matter = [];
			$scope.recovery = [];
			$scope.aee = [];
			$scope.enrichment = [];
			$scope.params = params;
			$scope.showCurricularEnrichment = false;
			$scope.showRecuperacao = false;
			$scope.showAEE = false;
			$scope.docenciaCompartilhada = [];
			$scope.docenciaCompartilhadaSign = "*";
			$scope.mensagemErro = "";
			$scope.mensagemAlerta = "";
			$scope.BoletimLiberado = true;
		}

		/**
		 * @function 
		 * @name 
		 * @namespace BoletimController
		 * @memberOf Controller
		 * @private
		 * @param
		 * @return
		 */
		function getBoletins() {
			if (!$scope.params || !$scope.params.AluIds || !$scope.params.TpcId) {
				$scope.mensagemErro = "Parâmetros inválidos";
			}
			else {
			    var url = $scope.api + "/ApiListagemBoletimEscolarAluno/GetBoletimEscolarDosAlunos/?alu_ids=" + $scope.params.AluIds + "&mtu_ids=" + $scope.params.MtuIds + "&tpc_id=" + $scope.params.TpcId;

			    $http.defaults.headers.common.Authorization = 'Bearer ' + Token;

				$http({
					method: 'GET',
					url: url
				}).then(function successCallback(response) {
					console.log(response);
					if (response.data == null) {
						$scope.mensagemErro = "Falha inesperada ao carregar o boletim.";
					}
					else if (response.data.length == 0) {
					    $scope.mensagemAlerta = msgAlunoSemDadosPeriodo;
					}
					else if (response.data[0].BoletimLiberado !== true) {
					    $scope.mensagemAlerta = msgSemEventoLiberado;
					    $scope.BoletimLiberado = false;
					}
					else if (response.data[0] && response.data[0].Status && response.data[0].Status == 1) {
						$scope.mensagemErro = response.data[0].StatusDescription;
					}
					else {
					    try
					    {
					        modelNotas(response.data);
					    }
					    catch (e)
					    {
					        console.log(e);
					        $scope.mensagemErro = "Ocorreu um erro ao carregar o boletim.";
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
						$scope.mensagemErro = "Falha inesperada ao carregar o boletim.";
				});
			}
		};

		/**
		 * @function 
		 * @name 
		 * @namespace BoletimController
		 * @memberOf Controller
		 * @private
		 * @param
		 * @return
		 */
		function modelNotas(list) {

			var i = 0, j = 0, k = 0, maxI = list.length, maxJ, maxK, listaJ, listaK, notas = [];

			for (i; i < maxI; i++) {
				maxJ = list[i].todasDisciplinas.length;
				listaJ = list[i].todasDisciplinas;
				j = 0;
				$scope.matter = [];
				$scope.recovery = [];
				$scope.enrichment = [];
				$scope.aee = [];
				$scope.showCurricularEnrichment = false;
				$scope.showRecuperacao = false;
				$scope.showAEE = false;
				$scope.docenciaCompartilhada = [];
				$scope.docenciaCompartilhadaSign = "*";
				for (j; j < maxJ; j++) {
					maxK = listaJ[j].notas.length;
					listaK = listaJ[j].notas;
					checkCurricularEnrichment(listaJ[j]);
					k = 0
					for (k; k < maxK; k++) {
					    if (listaK[k].nota != undefined && listaK[k].nota != null) {
					        notas.push({ conceito: listaK[k].nota.Nota });
					        notas.push({ faltas: listaK[k].nota.numeroFaltas });
					    }
					    else
					    {
					        notas.push({ conceito: "-" });
					        notas.push({ faltas: "-" });
					    }
					}//for K
					listaJ[j]["avaliacao"] = notas;
					notas = [];
				}//for j
				list[i]["matter"] = $scope.matter;
				list[i]["recovery"] = $scope.recovery;
				list[i]["aee"] = $scope.aee;
				list[i]["showRecuperacao"] = $scope.showRecuperacao;
				list[i]["showAEE"] = $scope.showAEE;
				list[i]["enrichment"] = $scope.enrichment;
				list[i]["showCurricularEnrichment"] = $scope.showCurricularEnrichment;
				list[i]["docenciaCompartilhada"] = $scope.docenciaCompartilhada;
			}//for i
			$scope.listBoletins = list;
		};

		function checkCurricularEnrichment(item) {

			if (item.enriquecimentoCurricular) {
				$scope.showCurricularEnrichment = true;

				if (item.tipoDocenciaCompartilhada && item.disRelacionadas != "") {
					item.Disciplina += $scope.docenciaCompartilhadaSign;
					$scope.docenciaCompartilhada.push($scope.docenciaCompartilhadaSign + " " + msgDocenciaCompartilhada + " " + item.disRelacionadas);

					$scope.docenciaCompartilhadaSign += "*";
				}

				$scope.enrichment.push(item);
			} else if (item.recuperacao) {
				$scope.showRecuperacao = true;
				$scope.recovery.push(item);
			} else if (item.aee) {
			    $scope.showAEE = true;
			    $scope.aee.push(item);
			} else {
				$scope.matter.push(item);
			};
		};

		function getToken() {
		    var deferred = $q.defer();
		    $http({
		        method: "POST",
		        url: "Busca.aspx/CreateToken",
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
		        getBoletins();
		    });
		}

		/**
		 * @function 
		 * @name 
		 * @namespace BoletimController
		 * @memberOf Controller
		 * @public
		 * @param
		 * @return
		 */
		$scope.getPhotoStudent = function __getPhotoStudent(id) {
			if (!id) return "../../App_Themes/IntranetSME/images/imgsAreaAluno/fotoAluno.png"
			else return $scope.site + "/WebControls/BoletimCompletoAluno/Imagem.ashx?idfoto=" + id;
		};

		$scope.getLogoSystem = function __getLogoSystem() {
			return $scope.logos + "/imagem.ashx?picture=%2fIntranetSME%2fimages%2flogos%2fLOGO_GERAL_SISTEMA.png";
		};

		/**
		 * @function 
		 * @name 
		 * @namespace BoletimController
		 * @memberOf Controller
		 * @public
		 * @param {int}
		 * @return {String}
		 */
		$scope.checkParImpar = function __checkParImpar(id) {

			if (id % 2 == 0) {
				return "linhaImpar";
			} else if (id % 2 == 1) {
				return "linhaPar";
			}//else
		};


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