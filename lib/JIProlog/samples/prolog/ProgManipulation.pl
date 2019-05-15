interpret(true).

interpret((G1, G2)):-
   !, 
   interpret(G1), 
   interpret(G2).

interpret((G1; G2)):-
   !, 
   interpret(G1);interpret(G2).
   
interpret(Goal):-
   !,
   clause(Goal, Body),
   call(Body).
