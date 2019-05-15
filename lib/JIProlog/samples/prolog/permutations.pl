/* 

Permutations: 
Example: permutations([1,2,3,4,5], X). 

*/

permutations([X|Y],Z):- 
	permutations(Y,W), 
	takeout(X,Z,W).   

permutations([],[]).

takeout(X,[X|R],R).

takeout(X,[F|R],[F|S]):- 
	takeout(X,R,S).

