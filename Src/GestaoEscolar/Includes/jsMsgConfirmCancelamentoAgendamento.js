
var btnAction="";var executeExcluir=false;function jsMsgConfirm(){$('.btCancelarAgendamento').die('click').live('click',function(){btnAction="#"+this.id;if($("#divConfirm").length>0)
$("#divConfirm").remove();$("<div id=\"divConfirm\" title=\"Confirmação\">Confirma o cancelamento do agendamento?</div>").dialog({bgiframe:true,autoOpen:false,resizable:false,modal:true,buttons:{"Sim":function(){$(this).dialog("close");executeExcluir=true;$(btnAction).click();},"Não":function(){$(this).dialog("close");}}});if(!executeExcluir){$("#divConfirm").dialog("open");}
var result=executeExcluir;executeExcluir=false;return result;});}
arrFNC.push(jsMsgConfirm);