%TEXTO --> MORSE

code('.-',a).
code('-...',b).
code('-.-.',c).
code('-..',d).
code('.',e).
code('..-.',f).
code('--.',g).
code('....',h).
code('..',i).
code('.---',j).
code('-.-',k).
code('.-..',l).
code('--',m).
code('-.',n).
code('---',o).
code('.--.',p).
code('--.-',q).
code('.-.',r).
code('...',s).
code('-',t).
code('..-',u).
code('...-',v).
code('.--',w).
code('-..-',x).
code('-.--',y).
code('--..',z).
code('-----','0').
code('.----','1').
code('..---','2').
code('...--','3').
code('....-','4').
code('.....','5').
code('-....','6').
code('--...','7').
code('---..','8').
code('----.','9').

palavra([M|R]) --> simb(M), palavra(R).
palavra([M]) --> simb(M).

simb(M) --> [C], {0'a =< C, C=<0'z, char_code(A,C), code(M,A)}.
simb(M) --> [C], {0'0 =< C, C=<0'9,  char_code(A,C), code(M,A)}.

texto([P|R]) --> palavra(P), " ", texto(R).
texto([P]) --> palavra(P).

escrever_sublista([],_) :- !.
escrever_sublista([X|R],Saida) :-
                               write(Saida,X),
                               write(Saida,' '),
                               escrever_sublista(R,Saida).

escrever_lista([],_) :- !.
escrever_lista([X|R],Saida) :-
                            escrever_sublista(X,Saida),
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
                          texto(Mens,L,[]),
                          escrever_lista(Mens,Saida),
                          nl(Saida),
                          read_line_to_codes(Entrada,LN),
                          processa(Entrada,Saida,LN).
                          
%:- descodif('text.txt','morse.txt').
