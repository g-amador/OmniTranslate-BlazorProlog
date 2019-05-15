%%%
%%% meta-rules for bottom-up parsing with automatic generation of parse trees
%%%

:- op( 1200, xfx, ---> ).	% arrow operator for DCG-like rules


parse( Phrase, Tree ) -->
	leaf( SubPhrase, SubTree ),
	left_corner( SubPhrase, SubTree, Phrase, Tree ).


leaf( Phrase, [Tree] ) -->
	[Word],
	{
		dictionary( Word, Category, Attributes ),
		Phrase =.. [Category | Attributes],
		Tree   =.. [Category, Word | Attributes]
	}.

leaf( Phrase, [] ) -->
	{
		Phrase ---> []
	}.


left_corner( Phrase, Tree, Phrase, Tree ) --> [].

left_corner( SubPhrase, SubTree, SuperPhrase, SuperTree ) -->
	{
		Phrase ---> [SubPhrase | OtherPhrases]
	},
	parse_rest( OtherPhrases, OtherTrees ),
	{
		append( SubTree, OtherTrees, SubForest ),
		Tree =.. [Phrase | SubForest]
	},
	left_corner( Phrase, [Tree], SuperPhrase, SuperTree ).


parse_rest( [], [] ) --> [].

parse_rest( [Phrase | OtherPhrases], Forest ) -->
	parse( Phrase, Tree ),
	parse_rest( OtherPhrases, OtherTrees ),
	{
		append( Tree, OtherTrees, Forest )
	}.


%%%
%%% sample grammar
%%%

s ---> [np, vp].

np ---> [det, n, rel].
np ---> [pn].

vp ---> [tv, np].
vp ---> [iv].

rel ---> [].
rel ---> [relpron, vp].


%%%
%%% dictionary( Word, Category, Attributes )
%%%

dictionary( that, relpron, [] ).
dictionary( terry, pn, [] ).
dictionary( shrdlu, pn, [] ).
dictionary( writes, tv, [] ).
dictionary( halts, iv, [] ).
dictionary( a, det, [] ).
dictionary( program, n, [] ).


%%%
%%% examples (in JIProlog and SWI Prolog):
%%%

%  ?- ['bottom-up.pl'].
%  % bottom-up.prolog compiled 0.01 sec, 4,336 bytes
%  
%  Yes
%  ?- parse( Phrase, Tree, [terry, writes, shrdlu], [] ).
%  
%  Phrase = s
%  Tree = [s(np(pn(terry)), vp(tv(writes), np(pn(shrdlu))))] ;
%  
%  No
%  ?- parse( Phrase, Tree, [terry, writes, a, program, that, halts], [] ).
%  
%  Phrase = s
%  Tree = [s(np(pn(terry)), vp(tv(writes), np(det(a), n(program), rel(relpron(that), vp(iv(halts))))))] ;
%  
%  No