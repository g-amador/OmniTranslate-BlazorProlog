pascal(InList, OutList) :-
   extendPlus([0 | InList], InList, OutList).

extendPlus(L, [], L).
extendPlus([], L, L).

extendPlus([L | Ls], [M | Ms], [S | Ss]) :-
   S is L+M,
   extendPlus(Ls, Ms, Ss).

pascalFar(InList, 0, InList) :- !.
pascalFar(InList, N, OutList) :-
   N1 is N-1,
   pascal(InList, In1),
   pascalFar(In1, N1, OutList).