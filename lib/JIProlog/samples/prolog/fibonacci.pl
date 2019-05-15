% ugo

fib(0,1):-!.
 
fib(1,1):-!.

fib(A,B):-     
   A1 is A - 1,
   A2 is A - 2,
   fib(A1,B1),
   fib(A2,B2),
   B is B1 + B2.
   


