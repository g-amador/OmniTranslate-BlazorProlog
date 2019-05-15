/**********************
* Hanoi's Towers
* hanoi(height, tower1, tower2, tower3, moves)
* example: 
	hanoi(3, a,b,c, X).657
	hanoi1(3, a,b,c, X).
* hanoi1 is faster than hanoi
*******************************/
%hanoi/3

hanoi(1, A,B,C,[[A,B]]):-!.
 
hanoi(N, A,B,C,Moves):-
    N > 0,
	N1 is N - 1,
    hanoi(N1, A,C,B,Ms1),
	hanoi(N1, C,B,A,Ms2),
	append(Ms1, [[A,B]|Ms2], Moves),
    !.
 
%hanoi1/3
hanoi1(1, A,B,C,[[A,B]]).

hanoi1(N, A,B,C,Moves):-
	N > 1,
	N1 is N - 1,
	hanoi1(N1, A,C,B,Ms1),
	asserta((hanoi1(N1, A,C,B,Ms1):-!)),
	hanoi1(N1, C,B,A,Ms2),
	append(Ms1, [[A,B]|Ms2], Moves), 
	!.
