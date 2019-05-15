/*****************************************************************************/
/*			 L I S P   I N T E R P R E T E R		     */
/*****************************************************************************/

/*
			AUTHOR	: A.S.Walker
			DATE	: 14 Nov 1984
			PROJECT : None


	This interpreter is based on the MICRO-LISP interpreter given in the
book LISP by Winston and Horn (Addison Wesley 1981) pages 303 to 314.

Built in functions are :
	DE	    : Define a new function
	SETQ	    : Bind a value to an object
	DSKIN	    : Read the lisp expressions from a file
	DSKOUT	    : Write the complete environment to a file
	READ	    : Read an expression from the terminal
	PRINT	    : Write an expression to the terminal
	TERPRI	    : Write a new line
	TYI	    : Value is the ascii value of the next character input
	TYO	    : Outputs its integer argument as ascii character
	CAR	    : Find the head of a list
	CDR	    : Find the tail of a list
	CONS	    : Form a dotted pair
	LIST	    : Form arguments into list
	APPEND	    : Append two list together
	PLUS	    : Add two numbers
	TIMES	    : Multiply two numbers
	DIFFERENCE  : Subtract second number from first
	QUOTIENT    : Divide first number by second
	REMAINDER   : The remainder of dividing first by second
	EVAL	    : Evaluate the argument
	APPLY	    : Evaluate first as function with second as its args
	MAPCAR	    : Result is list of results of applying first to each element of second
	QUOTE	    : Result is its argument unevaluated
	COND	    : Introduces a conditional statement
	LAMBDA	    : Introduces a lambda expression
	ATOMP	    : True if argument is atomic
	NULL	    : True if argument is NIL
	GREATERP    : True if first is larger than second
	LESSP	    : True if first is less than second
	EQ	    : True if first is the same as second
	AND	    : True if both arguments are true
	OR	    : True if either argument is true
	QUIT	    : Return to Prolog
	PROLOG	    : Drop down to Prolog in a break state
	T	    : Evaluates to T
	NIL	    : Evaluates to NIL



	It is hoped that this represents a reasonably complete set of LISP
functions. Notable short comings are:
	1) No in situ modification of S expressions (NCONC, RPLACA, RPLACD)
	2) User defined functions may only be of type LAMBDA
	3) No iterative control structures
	4) No property lists

	This is a piece of demonstration software designed to illustrate
some of the facilities of Prolog-2, in a realistic and hopefully useful way.
Of particular interest in this case is the programmable tokeniser, but also
notice the succinctness of the parser and the overall simplicity of the
evaluation routines.

*/


?- op(800,xfy,..).

/*---------------------------------------------------------------------------*/
/*	This is the top level READ-EVAL-PRINT loop			     */
/*---------------------------------------------------------------------------*/
lisp :-
	hello,
	repeat,
	  nl,nl,
	  write('-> '),
	  reset_context,			/* clear input buffer */
	  read_sexp(S_expression),
	  eval(S_expression,Result),
	  print1(Result),
	Result = quit,
	!,
	goodbye.



hello :-
	nl,nl,
	write('  MICRO LISP'),nl,
	write('    Expert Systems International'),nl,
	write('    14 November 1984'),nl,
	nl,nl,
	retractall('$lsp_environment'/2),
	tokeniser.



/*---------------------------------------------------------------------------*/
/* The following is the definition of the tokeniser for reading LISP	     */
/*---------------------------------------------------------------------------*/
tokeniser :-
	create_character_class(lisp,[26*6,7,6*6,4*1,9,1,5,2,3,4*1,4,1,10*0,70*1,128*8]),
	create_state_table(l_int,lisp,integer,l_int,[[1,255,255,255,255,255,0,255,255,255],[1,255,255,2,2,255,2,2,255,255]]),
	create_state_table(l_atm,lisp,atom,l_atm,[[255,1,255,255,255,255,255,255,255,255],[1,1,255,2,2,255,2,2,255,255]]),
	create_state_table(l_spc,lisp,none,l_spc,[[255,255,1,1,1,1,255,255,255,255],[2,2,2,2,2,2,2,2,255,255]]),
	create_state_table(l_eof,lisp,none,l_eof,[[255,255,255,255,255,255,255,1,255,255]]),
	create_state_table(l_com,lisp,none,l_com,[[255,255,255,255,255,255,255,255,255,1],[1,1,1,1,1,1,1,1,1,2],[3,3,3,3,3,3,3,3,3,3]]),
	create_token_class(lisp,lisp,6,[0-l_int, 1-l_atm, 2-l_spc, 3-l_spc, 4-l_spc, 5-l_spc, 7-l_eof, 9-l_com]),
	state(token_class,_,lisp).


goodbye :-
	nl,
	retractall('$lsp_environment'/2),
	write('Leaving MICRO LISP'),
	state(token_class,_,prolog),
	delete_token_class(lisp,_,_,_),
	delete_state_table(l_int,_,_,_,_),
	delete_state_table(l_atm,_,_,_,_),
	delete_state_table(l_spc,_,_,_,_),
	delete_state_table(l_eof,_,_,_,_),
	delete_state_table(l_com,_,_,_,_),
	delete_character_class(lisp,_),
	nl,nl.



/*---------------------------------------------------------------------------*/
/* Routines for translating internal form into Lisp text		     */
/*---------------------------------------------------------------------------*/
print1(Head .. Tail) :-
	!,
	write('('),
	print1(Head),
	print2(Tail),
	!.
print1(Atom) :-
	write(Atom).

print2(Head..Tail) :-
	!,
	write(' '),
	print1(Head),
	print2(Tail).
print2('NIL') :-
	!,
	write(')').
print2(Atom) :-
	write('.'),
	write(Atom),
	write(')').




/*---------------------------------------------------------------------------*/
/* The parser for LISP S expressions, uses the tokens generated by the	     */
/*	tokeniser defined above.					     */
/*---------------------------------------------------------------------------*/
read_sexp(S_expression) :-
	fetch_token(Token),
	sexp(Token, S_expression),
	!.
read_sexp(_) :-
	error(1),
	!,fail.


sexp([Atom,_,l_atm],Atom).
sexp([Integer,_,l_int],Integer).
sexp([none,2,_],S_expression) :-
	read_sexp_list(S_expression).
sexp([none,5,_],'QUOTE'..S_expression..'NIL') :-
	read_sexp(S_expression).
sexp([none,7,_],'QUOTE'..'EOF'..'NIL') :-
	get_character(_).



read_sexp_list(S_list) :-
	fetch_token(Token),
	sexp_list(Token,S_list),
	!.

sexp_list([none,3,_],'NIL').
sexp_list([none,4,_],S_expression) :-
	!,
	read_sexp(S_expression),
	fetch_token([none,3,_]).
sexp_list(Token,Sexp..Sexplist) :-
	sexp(Token,Sexp),
	read_sexp_list(Sexplist).





fetch_token(Token) :-
	repeat,
	  get_token(Token),
	Token \= [_,_,l_com],		/* ignore comments */
	!.




/*---------------------------------------------------------------------------*/
/* Evaluation of LISP expressions can be broken into two phase, the first    */
/*	deals with special functions that do not evaluate their arguments,   */
/*	the second (apply) actually applies a function to its evaluated      */
/*	arguments.							     */
/*---------------------------------------------------------------------------*/
eval('COND'..Tail , Result) :-
	eval_cond(Tail,Result),!.
eval('COND'.._ , _) :- !,fail.
eval('QUOTE'..S_expression..'NIL' , S_expression) :- !.
eval('SETQ'..Name..Value..'NIL', Result) :-
	eval(Value,Result),
	bind1(Name,Result),
	!.
eval('SETQ'.._ , _) :- !,fail.
eval('DE'..Name..Args..Body , Name) :-
	map(atom, Name..Args),
	check_exists(Name),
	assert('$lsp_environment'(Name, 'LAMBDA'..Args..Body),0),
	!.
eval('DE'..Rest, _) :-
	error(2),
	!,
	fail.
eval('DSKIN'..Filename..'NIL',Filename) :-
	!,
	default_name(Filename,'LSP',File),
	create_stream(Filename,read,ascii,file(File,1)),
	see(Filename),
	repeat, 				/* READ-EVAL-PRINT loop */
	  read_sexp(S_expression),		/*  do everything	*/
	  eval(S_expression,Result),		/*			*/
	  print1(Result),			/*  reporting to user	*/
	  nl,					/*			*/
	Result == 'EOF',                        /*  until end of file   */
	seen,
	delete_stream(Filename,_,_,_,_),
	!.

eval('DSKOUT'..Filename..'NIL',Filename) :-
	!,
	default_name(Filename,'LSP',File),
	create_stream(Filename,write,ascii,file(File,1)),
	tell(Filename),
	write_environment,
	told,
	delete_stream(Filename,_,_,_,_),
	!.

eval(Function..Arguments, Result) :-
	!,
	mapcar(eval,Arguments,Reslist),
	apply(Function,Reslist,Result).
eval('T','T') :- !.
eval('NIL','NIL') :- !.
eval(Number,Number) :-
	integer(Number),!.
eval(Name,Value) :-
	'$lsp_environment'(Name,Value),
	!.
eval(_,_) :-
	error(3),
	!,
	fail.



/*---------------------------------------------------------------------------*/
/* Evaluation of CONDitional statements needs some special actions	     */
/*---------------------------------------------------------------------------*/
eval_cond((Cond..Action)..Rest, Result) :-
	eval(Cond,B),
	not null(B),
	!,
	eval_body(Action,Result).
eval_cond(_..Rest, Result) :-
	eval_cond(Rest,Result).
eval_cond('NIL','NIL').



/*---------------------------------------------------------------------------*/
/* Since the body of a function can consist of a sequence of expressions we  */
/*	must evaluate each and return as the result the value of the last.   */
/*---------------------------------------------------------------------------*/
eval_body( Last_statement..'NIL' , Result) :-
	!,
	eval(Last_statement,Result).
eval_body( Statement..Rest , Result) :-
	eval(Statement,_),
	eval_body(Rest,Result),
	!.
eval_body('NIL','NIL').




/*---------------------------------------------------------------------------*/
/* This predicate does the real work of calculating results and coping with  */
/*	function calls. The basic function simply call Prolog to do the work */
/*	lambda expressions are the only exception.
/*---------------------------------------------------------------------------*/
apply('CAR' , (Head.._)..'NIL' , Head) :- !.
apply('CDR' , (_..Tail)..'NIL' , Tail) :- !.
apply('CONS' , A..B..'NIL' , A..B) :- !.
apply('LIST' , List , List ) :- !.
apply('ATOMP' , A..'NIL' , 'T' ) :- atomic(A),!.
apply('ATOMP' , _ , 'NIL') :- !.
apply('LESSP' , A..B..'NIL' , 'T') :- A < B,!.
apply('LESSP' , _ , 'NIL') :- !.
apply('GREATERP' , A..B..'NIL' , 'T') :- A > B,!.
apply('GREATERP' , _ , 'NIL') :- !.
apply('APPEND' , A..B..'NIL' , Result) :- xappend(A,B,Result),!.
apply('AND' , A..B..'NIL' , 'T') :- not null(A), not null(B), !.
apply('AND' , _ , 'NIL') :- !.
apply('OR' , A..B..'NIL' , 'T') :- (not null(A) ; not null(B)),!.
apply('OR' , _ , 'NIL') :- !.
apply('NULL' , 'NIL'..'NIL' , 'T' ) :- !.
apply('NULL', _ , 'NIL' ) :- !.
apply('EQ' , A..A..'NIL' , 'T' ) :- !.
apply('EQ' , _ , 'NIL' ) :- !.
apply('PRINT', T..'NIL' , T) :- print1(T),!.
apply('READ' , 'NIL' , S_expression) :- !,read_sexp(S_expression).
apply('TYO' , Ascii..'NIL' , Ascii) :- put(Ascii),!.
apply('TYI' , 'NIL' , Ascii) :- get0(Ascii),!.
apply('TERPRI' , 'NIL','T') :- nl,!.
apply('PLUS', A..B..'NIL' , R) :- R is A + B, !.
apply('DIFFERENCE' , A..B..'NIL' , R) :- R is A - B, !.
apply('TIMES' , A..B..'NIL' , R ) :- R is A * B, !.
apply('QUOTIENT' , A..B..'NIL' , R) :- R is A // B, !.
apply('REMAINDER', A..B..'NIL', R) :- R is A mod B, !.
apply('APPLY',A..B..'NIL',R) :- apply(A,B,R),!.
apply('EVAL',A..'NIL',R) :- eval(A,R),!.
apply('MAPCAR',A..B..'NIL',R) :- lisp_mapcar(A,B,R),!.
apply('LAMBDA'..Formal..Body , Actual , Result) :-
	!,
	bind_all(Formal,Actual),
	eval_body(Body,Result),
	restore(Formal),
	!.
apply('QUIT' , 'NIL',quit) :- !.
apply('PROLOG' , 'NIL','NIL') :-
	!,
	state(token_class,_,prolog),
	break,
	state(token_class,_,lisp),
	!.
apply(Function , Arguments , Result) :- 	/* Do user function call */
	atom(Function),
	eval(Function, L_exp),
	L_exp = ('LAMBDA'.._.._) ,
	!,
	apply(L_exp, Arguments , Result),
	!.






write_environment :-
	'$lsp_environment'(Name,Value),
	print1('SETQ'..Name..('QUOTE'..Value..'NIL')..'NIL'),
	nl,
	fail.
write_environment.







bind1(Name,Value) :-
	retract('$lsp_environment'/2,'$lsp_environment'(Name,_),_,_),
	assert('$lsp_environment'(Name,Value),0),
	!.
bind1(Name,Value) :-
	assert('$lsp_environment'(Name,Value),0).




bind_all(Formal_name..F , Actual_value..A) :-
	assert('$lsp_environment'(Formal_name,Actual_value),0),
	bind_all(F,A).
bind_all(_,_).


restore(Name..Others) :-
	retract('$lsp_environment'/2,'$lsp_environment'(Name,_),_,_),
	restore(Others).
restore(_).

/** Note that bind_all/2 & restore/1 treat '$lsp_environment'/2 as though
    it were a stack. That is there may be several clauses for a particular
    atom at any given time, of which the topmost is the current binding. This
    is for reasons of efficiency(speed).
**/




/*---------------------------------------------------------------------------*/
/* MAP type functions are a feature of LISP. They are higher order functions */
/*  in that they take as an argument another function. This function is then */
/*  applied to each element of a list argument. 			     */
/*---------------------------------------------------------------------------*/

map(F,'NIL') :- !.
map(F,A..R) :-
	G =.. [F,A],				/* Prolog call		*/
	G,					/*  without saving any	*/
	map(F,R).				/*  results		*/


mapcar( _ , 'NIL' , 'NIL' ) :- !.
mapcar( Fn , A..Rest , R..Others ) :-
	G =.. [Fn , A , R],			/* Prolog call		*/
	G,					/*  saving results	*/
	mapcar(Fn,Rest,Others).

lisp_mapcar(_,'NIL','NIL') :- !.
lisp_mapcar(Fn,A..Rest,R..Others) :-
	apply(Fn,A..'NIL',R),                    /* Lisp call            */
	lisp_mapcar(Fn,Rest,Others),		/*  saving results	*/
	!.





check_exists(Name) :-
	retract('$lsp_environment'/2,'$lsp_environment'(Name,_),_,_),
	nl,write('Redefining '),write(Name),nl,nl,!.
check_exists(_).




xappend(H..T,L,H..R) :- !,xappend(T,L,R).
xappend(_,L,L).




null('NIL').



/*---------------------------------------------------------------------------*/
/* Rudimentary error handling routines					     */
/*---------------------------------------------------------------------------*/
error(1) :- nl,write('Syntax error'),nl.
error(2) :- nl,write(' Wrong format for DE '),nl.
error(3) :- nl,write(' Atom unbound'),nl.





/*****************************************************************************/
/*		  E N D   O F	L I S P   I N T E R P R E T E R 	     */
/*****************************************************************************/
