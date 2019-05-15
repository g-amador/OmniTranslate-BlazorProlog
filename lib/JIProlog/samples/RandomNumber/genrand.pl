
% generate a list of rundom number
genlist(0, []):-
   !.

genlist(N, [X|Rest]):-
   N1 is N - 1,
   xcall("RandomNumberGen3", [0, 100, X]),
   genlist(N1, Rest),
   !.





   
