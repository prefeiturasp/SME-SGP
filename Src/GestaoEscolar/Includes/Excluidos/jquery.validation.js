/*
	Plugin per la validazione automatica di campi di form.
	Autore: 	Marco Pegoraro
	Data:		28 Novembre 2006
	Url:		http://www.consulenza-web.com/jquery-validation-plugin.dc-13.html
	
	Questo plugin permette di filtrare i tasti accettati da un campo di testo in un form.
	Ad esempio и possibile accettare solo lettere o solo numeri o controlli piщ complessi.
	I filtri vengono applicati direttamente durante l'inserimento del testo.
	Viene inoltre eseguito un controllo al rilascio del campo per effettuare una validazione
	completa in base alle regole impostate.
	E' possibile associare al controllo delle funzioni callback per intercettare errori o
	gestire eventi quando la validazione и completa.
	
	Puoi utilizzare liberamente questo script senza alcun tipo di restrizione.
	Se applichi delle modifiche o se realizzi delle validazioni aggiuntive sei pregato
	di comunicarlo all'autore al seguente indirizzo mail:
		marco(dot)pegoraro(at)gmail(dot)com
	Specificando come oggetto "JQuery Validation Plugin.
	
	Ti sarei inoltre molto grato se mettessi un link al mio sito nelle pagine in cui
	viene utilizzato questo plugin:
		http://www.consulenza-web.com
	Non и una cosa obbligatoria, ma permetterebbe a molta piщ gente di trovare questo
	plugin (e magari altri che svilupperт) ed utilizzarli per rendere piщ facile la
	vita dei webmaster.
	
	Grazie per aver letto queste informazioni.
	Marco Pegoraro,
	www.consulenza-web.com
	
	
	Parametri di validazione:
	-------------------------------------------
		empty:	si|no - definisce se il campo accetta valori nulli. Default "si"
		type: 	Famiglia di validazione. Sono presenti delle famiglie di validazione
			  	predefinire elencate qui di seguito.
					int:			[09,.]
					lower:			[a-z]
					lowers:			[a-zтащийм]
					upper:			[A-Z]
					uppers:			[A-Zтащийм]
					alpha:			[a-zA-Z]
					alphas:			[a-zA-Zтащийм]
					lowerInt:		[a-z0-9]
					lowersInt:		[a-z0-9тащийм]
					upperInt:		[A-Z0-9]
					uppersInt:		[A-Z0-9тащийм]
					alphaInt:		[a-zA-Z0-9]
					alphasInt:		[a-zA-Z0-9тащийм]
					nick:			[a-zA-Z0-9-_ .@]
					
		add: 		definire una stringa di caratteri accettati oltre alla famiglia principale.		
		ctrl:		si|no - permette o meno l'utilizzo delle scorciatoie da tastiera per
					copia/taglia/incolla. Si default vengono accettate.
		onError: 	definire una funzione di callback in caso di errore di validazione finale.
		onDone:		definire una funzione di callback in caso di validazione passata.
	
	
	Utilizzo Pratico:
	-------------------------------------------
	In un documento vogliamo applicare un controllo al campo "Id=nick":
	
	...
	<input type="text" name="nick" id="nick">
	...
	<script type="text/javascript">
		$(document).ready(function() {
			$("#nick").validation({
				type:	"aplha",
				add:	"-_",
				error:	function() {
							alert("Errore di validazione del campo!");
							this.value = "";
						}
			});
		});
	</script>
*/

// type definisce il tipo di validazione da effettuare.
jQuery.fn.validation = function( cfg ) {
	// Preimpostazione per la variabile in input che и opzionale.
	cfg					= cfg || {};
	
	// Preimpostazioni delle opzioni di configurazione.
	cfg.type 			= cfg.type || "alphasInt";		// Famiglia di validazione.
	cfg.add				= cfg.add || "";				// Lista di caratteri eccezioni. Stringa
	cfg.empty			= cfg.empty || "si";			// Puт contenere il valore nullo?
	cfg.ctrl			= cfg.ctrl || "si";				// Permette le operazioni di copia/taglia/incolla
	cfg.onError			= typeof cfg.onError == "function" ? cfg.onError : function(){};
	cfg.onDone			= typeof cfg.onDone == "function" ? cfg.onDone : function(){};
	
	
	
	// Gestione dell'evento keyPress sull'oggetto.
	this.keypress(function(e) {
		// Ottengo il codice del tasto premuto.
		key = getKeyCode(e);
		
		// Salvo il riferimento all'oggetto in una variabile interna.
		cfg.OBJ = this;
		
		// Permetto l'utilizzo dei tasti di comando per le operazioni di copia incolla.
		// In base alla configurazione di sistema.
		if ( cfg.ctrl == "si" ) {
			// allow Ctrl+A
			if((e.ctrlKey && key == 97 /* firefox */) || (e.ctrlKey && key == 65) /* opera */) return true;
			// allow Ctrl+X (cut)
			if((e.ctrlKey && key == 120 /* firefox */) || (e.ctrlKey && key == 88) /* opera */) return true;
			// allow Ctrl+C (copy)
			if((e.ctrlKey && key == 99 /* firefox */) || (e.ctrlKey && key == 67) /* opera */) return true;
			// allow Ctrl+Z (undo)
			if((e.ctrlKey && key == 122 /* firefox */) || (e.ctrlKey && key == 90) /* opera */) return true;
			// allow or deny Ctrl+V (paste), Shift+Ins
			if((e.ctrlKey && key == 118 /* firefox */) || (e.ctrlKey && key == 86) /* opera */ || (e.shiftKey && key == 45)) return true;
		} else {
			// allow Ctrl+A
			if((e.ctrlKey && key == 97 /* firefox */) || (e.ctrlKey && key == 65) /* opera */) return false;
			// allow Ctrl+X (cut)
			if((e.ctrlKey && key == 120 /* firefox */) || (e.ctrlKey && key == 88) /* opera */) return false;
			// allow Ctrl+C (copy)
			if((e.ctrlKey && key == 99 /* firefox */) || (e.ctrlKey && key == 67) /* opera */) return false;
			// allow Ctrl+Z (undo)
			if((e.ctrlKey && key == 122 /* firefox */) || (e.ctrlKey && key == 90) /* opera */) return false;
			// allow or deny Ctrl+V (paste), Shift+Ins
			if((e.ctrlKey && key == 118 /* firefox */) || (e.ctrlKey && key == 86) /* opera */ || (e.shiftKey && key == 45)) return false;
		}
		
		if ( key == undefined ) return true;
		
		return validateChar(key);
	}); // Fine gestione dell'evento keyPress. ____________________________________________________
	
	// Gestione dell'abbandono del campo. Qui viene effettuato un'ulteriore controllo di 
	// validazione in caso ci sia stato un copia/incolla di dati potenzialmente errati.
	this.blur(function() {
		// Controllo di validazione sull'opzione "nullo".
		if ( cfg.empty == "no" && this.value.length == 0 ) {
			cfg.onError.apply(this);
			return false;
		} // Fine controllo sull'opzione "nullo".
		
		// Validazione dei caratteri della stringa.
		valid = 1;
		for ( j=0; j<this.value.length; j++)
			if ( validateChar(this.value.charCodeAt(j)) != true ) valid = 0;
		
		
		// Validazione aggiuntiva per il controllo MAIL.
		// Utilizzo una funzione che esegue una regexp su tutto il contenuto del campo.
		// Evito il controllo in caso di valore vuoto in quanto questa casistica viene
		// Giа considerata prima.
		if ( cfg.type == "mail" && this.value.length > 0 ) { valid = checkEmail(this.value); }
		
		
		// Lancio le funzioni di callback in base all'esito della validazione.
		if ( valid == 0 ) cfg.onError.apply(this); else cfg.onDone.apply(this);
	}); // Fine gestione dell'evento "blur". ______________________________________________________
	
	
	// Prende in input un codice di carattere ed esegue la validazione.
	function validateChar(key) {
		//  backspace       enter
		if ( key == 8 || key == 13 ) return true;
		
		
		// Effettuo il controllo sui caratteri aggiuntivi (parametro "add");
		if ( cfg.add != "" && keyAdd(key, cfg.add) ) return true;
	
		
		// Inizia la selezione e l'applicazione dei tipi di validazione previsti:
		if 		( cfg.type == 'int' )			return keyN(key);					// [0-9]
		else if ( cfg.type == 'lower' )			return keyL(key);					// [a-z]
		else if ( cfg.type == 'lowers' )		return ( keyL(key) || keyAdd(key, "тащийм") );	// [a-zтащийм]
		else if ( cfg.type == 'upper' )			return keyU(key);					// [A-Z]
		else if ( cfg.type == 'uppers' )		return ( keyU(key) || keyAdd(key, "тащийм") );	// [A-Zтащийм]
		else if ( cfg.type == 'alpha' )			return ( keyL(key) || keyU(key) );	// [a-zA-Z]
		else if ( cfg.type == 'alphas' )		return ( keyL(key) || keyU(key) || keyAdd(key, "тащийм") );	// [a-zA-Zтащийм]
		else if ( cfg.type == 'lowerInt' )		return ( keyL(key) || keyN(key) );	// [a-z0-9]
		else if ( cfg.type == 'lowersInt' )		return ( keyL(key) || keyN(key) || keyAdd(key, "тащийм") );	// [a-z0-9тащийм]
		else if ( cfg.type == 'upperInt' )		return ( keyU(key) || keyN(key) );	// [A-Z0-9]
		else if ( cfg.type == 'uppersInt' )		return ( keyU(key) || keyN(key) || keyAdd(key, "тащийм") );	// [A-Z0-9тащийм]
		else if	( cfg.type == 'alphaInt')		return ( keyL(key) || keyU(key) || keyN(key) );	// [a-zA-Z0-9]
		else if	( cfg.type == 'alphasInt')		return ( keyL(key) || keyU(key) || keyN(key) || keyAdd(key, "тащийм") );	// [a-zA-Z0-9тащийм]
		else if ( cfg.type == 'nick' )			return ( keyL(key) || keyU(key) || keyN(key) || keyAdd(key, "-_ .@") ); // [a-zA-Z0-9-_ .@]
		else if ( cfg.type == 'mail' )			return keyMail(key); // Validazione mail.
	} // Fine "validateChar()" ____________________________________________________________________
	
	
	// Prende in input un oggetto Event e fornisce il codice dell'ultimo carattere
	// premuto sulla tastiera.
	function getKeyCode(e) {
		if ( window.event ) return e.keyCode;
		else if ( e.which ) return e.which;
	} // Fine "getKeyCode()" ______________________________________________________________________
	
	// Prende in input un codice di carattere ed una stringa. Se il carattere и presente in tale
	// stringa restituisce "true". Il confronto viene eseguito sul codice numerico del carattere.
	// Viene utilizzata in "keypress" per gestire delle eccezioni alle regole.
	function keyAdd(key, add) {
		for ( i=0; i<add.length; i++ )
			if ( key == add.charCodeAt(i) ) return true;
		return false;
	}// Fine "keyAdd()" ___________________________________________________________________________
	
	
	
	
	
	
	
	
	// [0-9]
	function keyN(k) {
		return ( k >= 48 && k <= 57 )
	} // Fine "keyN()" ____________________________________________________________________________
	
	// [A-Z]
	function keyU(k) {
		return ( k >= 65 && k <= 90 );
	} // Fine "keyU()" ____________________________________________________________________________
	
	// [a-z]
	function keyL(k) {
		return ( k >= 97 && k <= 122 )
	} // Fine "keyL()" ____________________________________________________________________________
	
	// Validazione formale su inserimento di indirizzi e-mail. Viene effettuato un controllo sui
	// caratteri accettati ed un ulteriore controllo sul "quando" и possibile utilizzare determinati
	// catatteri. Ed non и possibile utilizzare 2 volte la "@".
	function keyMail(k) {
		// Controllo formale sul range di caratteri accettati.
		if ( keyL(k) || keyU(k) || keyN(k) || keyAdd(k, "-_.@") ) {			
	
			// Non si puт iniziare con un carattere speciale:
			//       .          -          _          @
			if ( ( k == 46 || k == 45 || k == 95 || k == 64 ) && cfg.OBJ.value.length == 0 ) return false;
			
			// Non si puт iniziare con un numero:
			if ( keyN(k) && cfg.OBJ.value.length == 0 ) return false;
			
			// Non puт esserci un carattere speciale dopo un punto:
			if ( ( k == 46 || k == 45 || k == 95 || k == 64 ) && cfg.OBJ.value[cfg.OBJ.value.length-1] == "." ) return false;
			
			// Non puт esserci un punto o una chiocciola dopo un carattere speciale:
			cb = cfg.OBJ.value[cfg.OBJ.value.length-1];
			if ( ( k == 46 || k == 64 ) && ( cb == "-" || cb == "_" || cb == "@" || cb == "." ) ) return false;
			
			// Devono esserci almeno 3 caratteri per poter inserire la @:
			if ( k == 64 && cfg.OBJ.value.length < 3 ) return false;
			
			// Controlli quando и giа presente una chiocciola (@)
			if ( cfg.OBJ.value.indexOf("@") != -1 ) {
				// Non sono ammesse altre chiocciole oltre alla prima!
				if ( k == 64 ) return false;
				
				// Calcolo l'offset della stringa dopo la chiocciola per la gestione dei caratteri speciali.
				offSet = cfg.OBJ.value.length - cfg.OBJ.value.indexOf("@");
				
				// Non si puт iniziare con un carattere speciale appena dopo la chiocciola:
				if ( ( k == 46 || k == 45 || k == 95 ) && offSet == 1 ) return false;
				
				// Se si digita il punto "offSet" deve essere almeno di 4 caratteri:
				if ( k == 46 && offSet < 4 ) return false;
			} // Fine controlli quando и giа presente una chiocciola.
			
		} else return false;
	} // Fine "keyMail()" _________________________________________________________________________
	
	// Used for debugging while developing :-)
	function db(s) { $("#test").html($("#test").html()+"<br>" + s + " - " + String.fromCharCode(s) ); }
	function db1(s) { $("#test").html( $("#test").html()+"<br>" + s ); }
	
	
	// Controllo formale di un indirizzo mail utilizzando espressioni regolari.
	function checkEmail(toCheck) {
		if (/^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/.test(toCheck)) return true; else return false;
	} // Fine "checkEmail()" ______________________________________________________________________
}


