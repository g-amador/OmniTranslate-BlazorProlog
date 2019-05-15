/*

queens(N,[N1,N2,...,NN]) is true if you can put a queen on an NxN
in the ith rank of the nth file, s.t. no
queen attacks any other queen.

*/

queens(N,Qs):-
	range(1,N,Ns), 
	queens(Ns,[],Qs).
	
queens(UnplacedQs, SafeQs, Qs):-
	select(Q, UnplacedQs, UnplacedQs1),
	\+ attack(Q,SafeQs),
	queens(UnplacedQs1,[Q|SafeQs],Qs).

queens([],Qs,Qs).

attack(X,Xs):-
	attack(X, 1, Xs).
	
attack(X,N,[Y|_]):- 
	X is Y+N.
	
attack(X,N,[Y|_]):- 	 
	X is Y-N.

attack(X,N,[_|Ys]):- 
	N1 is N+1, 
	attack(X,N1,Ys).

permutation(Xs,[Z|Zs]):- 
	select(Z,Xs,Ys), 
	permutation(Ys,Zs).
	
permutation([],[]).

range(M,N,[M|Ns]):- 
	M < N, M1 is M+1, 
	range(M1,N,Ns).

range(N,N,[N]).

select(X,[X|Xs],Xs).  

select(X,[Y|Ys],[Y|Zs]):- 
	select(X,Ys,Zs). 

/*	
%Try
%queens(4,P).
%
% The result P=[3,1,4,2] means the queens go like this:
%    *****************
% 4  *   *   * Q *   *
%    *****************
% 3  * Q *   *   *   *
%    *****************
% 2  *   *   *   * Q *
%    *****************
% 1  *   * Q *   *   *
%    *****************
%      1   2   3   4
*/