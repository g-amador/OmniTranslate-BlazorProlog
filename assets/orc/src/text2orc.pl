%PALAVRAS EM ORC

palavras_orc(hi,charach).
palavras_orc(bye,futchi).
palavras_orc(yes,mok).
palavras_orc(no,nok).
palavras_orc(buy,goshak).
palavras_orc(weapon,porack).
palavras_orc(sword,burkabata).
palavras_orc(sabre,charcha).
palavras_orc(hatchet,hakhak).
palavras_orc(shortsword,burka).
palavras_orc(bow,batuk).
palavras_orc(arrows,pixo).
palavras_orc(armor,bora).
palavras_orc(shield,dora).
palavras_orc(brassshield,donga).
palavras_orc(studded,tulak).
palavras_orc(leather,aka).
palavras_orc(studdedhelmet,grofa).

sequencia([X|R]) --> caracter(X), sequencia(R).
sequencia([X]) --> caracter(X).

caracter(Caracter) --> [X], {0'a =< X, 0'z >= X, atom_codes(Caracter,[X])}.

palavra(Palavra) --> sequencia(L), {atom_chars(P,L), palavras_orc(P,Palavra)}.

frase([Palavra|Palavras]) --> palavra(Palavra), espacos, frase(Palavras).
frase([Palavra]) --> palavra(Palavra).

espacos --> " ", espacos.
espacos --> " ".

escrever_lista([],_) :- !.
escrever_lista([X|R],Saida) :-
                            write(Saida,X),
                            write(Saida,' '),
                            escrever_lista(R,Saida).

descodifica(Ficheiro_E,Ficheiro_S) :-
                                   open(Ficheiro_E,read,Entrada),
                                   open(Ficheiro_S,write,Saida),
                                   read_line_to_codes(Entrada,L),
                                   processa(Entrada,Saida,L),
                                   close(Entrada),
                                   close(Saida),
                                   halt.

processa(_,_,end_of_file) :- !.
processa(Entrada,Saida,L) :-
                          frase(Mens,L,[]),
                          escrever_lista(Mens,Saida),
                          nl(Saida),
                          read_line_to_codes(Entrada,LN),
                          processa(Entrada,Saida,LN).

%CONVERTER O TEXTO EM text.txt PARA ORC EM orc.txt
%:- descodifica('text.txt','orc.txt').
