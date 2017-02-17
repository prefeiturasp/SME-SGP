function jsCadastroCurso() {

    createTabs("#divTabs", "input[id$='txtSelectedTab']");
        
    createDialog('.divPeriodo', 555, 0);
    createDialog('.divDisciplina', 555, 0);
    createDialog('.divDisciplinaPeriodo', 555, 0);    
    createDialog('.divEletivasAlunos', 750, 0); 
}
// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroCurso);
arrFNCSys.push(jsCadastroCurso);