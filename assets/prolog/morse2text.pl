%MORSE --> TEXTO

code(a,'.-').
code(b,'-...').
code(c,'-.-.').
code(d,'-..').
code(e,'.').
code(f,'..-.').
code(g,'--.').
code(h,'....').
code(i,'..').
code(j,'.---').
code(k,'-.-').
code(l,'.-..').
code(m,'--').
code(n,'-.').
code(o,'---').
code(p,'.--.').
code(q,'--.-').
code(r,'.-.').
code(s,'...').
code(t,'-').
code(u,'..-').
code(v,'...-').
code(w,'.--').
code(x,'-..-').
code(y,'-.--').
code(z,'--..').
code('0','-----').
code('1','.----').
code('2','..---').
code('3','...--').
code('4','....-').
code('5','.....').
code('6','-....').
code('7','--...').
code('8','---..').
code('9','----.').

sequencia --> simb, sequencia.
sequencia --> simb.

simb --> ".".
simb --> "-".

sequencia([X|R]) --> simb(X), sequencia(R).
sequencia([X]) --> simb(X).

simb('.') --> ".".
simb('-') --> "-".

letra(A) --> sequencia(L), {atom_chars(AL,L), code(A,AL)}.

letras([X|R]) --> letra(X), " ", letras(R).
letras([X]) --> letra(X).

palavra(A) --> letras(L), {atom_chars(A,L)}.

morse([P|R]) --> palavra(P), "  ", morse(R).
morse([P]) --> palavra(P).

escrever_lista([],_) :- !.
escrever_lista([X|R],Saida) :-
                            write(Saida,X),
                            write(Saida,' '),
                            escrever_lista(R,Saida).

                      
descodif(Ficheiro_E,Ficheiro_S) :-
                              open(Ficheiro_E,read,Entrada),
                              open(Ficheiro_S,write,Saida),
                              read_line_to_codes(Entrada,L),
                              processa(Entrada,Saida,L),
                              close(Entrada),
                              close(Saida),
                              halt.
        
processa(_,_,end_of_file) :- !.
processa(Entrada,Saida,L) :-
                          morse(Mens,L,[]),
                          escrever_lista(Mens,Saida),
                          nl(Saida),
                          read_line_to_codes(Entrada,LN),
                          processa(Entrada,Saida,LN).

%:- descodif('morse.txt','text.txt').
