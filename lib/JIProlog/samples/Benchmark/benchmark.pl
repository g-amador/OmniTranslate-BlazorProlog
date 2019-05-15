/******************************
 * JIProlog Benchmark
 * Copyright (C) 2002-2004 By Ugo Chirico
 * http://www.ugosweb.com/jiprolog
 *
 * This is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 ******************************/

/********************************************************************
 * the following predicates are for portability of this benchmark 
 *******************************************************************/
    
r_repeat(N):-    
    N > 0.
    
r_repeat(N):-
   N > 0,
   N1 is N - 1,
   r_repeat(N1).

r_time(T):-
    T is cputime.
    
/********************************************************************/   
listLenght(100). %100
nipsLength(3000). %3000

benchmark(Accuracy):-
    write('------------------'), nl,
    write('JIProlog Benchmark '), nl,
    write('By Ugo Chirico'), nl,
    write('------------------'), nl,
    write('Accuracy: '),
    write(Accuracy), nl,
    assert(count(0)),
    test(1, Accuracy),
    test(2, Accuracy),
    test(3, Accuracy),
    test(4, Accuracy),
    nips(Accuracy),
    write('------------------'), nl,
    write('Bye Bye'), nl.
    
test(N, Accuracy):-
    message(N, Msg),
    write(Msg), nl,
    listLenght(Length),
    fill(Length, List),
    abolish(timeVal/2),
    r_repeat(Accuracy),      
        r_time(Millis),                 
        test(N, List, RevList),
        r_time(Millis1),
        Time is Millis1 - Millis,
        addTime(N, Time),
        fail.
    
test(N, Accuracy):-
    timeVal(N, Time),
    AvgTime is Time / Accuracy,
    write('     Average time (ms) = '),
    write(AvgTime), nl.

% NIPS  (Number of Inference Per Second)
nips(Accuracy):-
   message(0, Msg),
   write(Msg), nl,
   nipsLength(N),
   fill(N, List),
   abolish(timeVal/2),
   r_repeat(Accuracy),     
      r_time(Millis),      
      naiveReverseAcc(List, RevList),
      r_time(Millis1),      
      Time is Millis1 - Millis,
      Nips is (((N + 4) / Time) * 1000),      
      addTime(0, Nips),
      fail.

nips(Accuracy):-
    timeVal(0, Time),
    AvgNips is Time / Accuracy,
    write('     Average nips = '),
    write(AvgNips), nl.

addTime(N, Time):-
    retract(timeVal(N, Sum)),
    NewTime is Sum + Time,
    assert(timeVal(N, NewTime)),
    !.

addTime(N, Time):-
    assert(timeVal(N, Time)),
    !.

% test
test(1, X, Y):-
    naiveReverseAcc(X, Y).
    
test(2, X, Y):-
    naiveReverseApp(X, Y).
    
test(3, X, Y):-
    assertList(X),
    retractList(X).
    
test(4, X, Y):-
    r_sumlist(X, Y).
    
    
% Messages
message(0, ' - NIPS by Naive Reverse with accumulator').
message(1, ' - Naive Reverse with accumulator').
message(2, ' - Naive Reverse with naive append/3').
message(3, ' - Assert and Retract a list of facts').
message(4, ' - Sum a list of numbers').


% sum a list of numbers
r_sumlist([X|Xs], Sum):-
    r_sumlist([X|Xs], Sum ,0).
    
r_sumlist([], Acc, Acc):-!.
    
r_sumlist([X|Xs], Sum, Acc):-
    N is Acc + X,
    r_sumlist(Xs, Sum, N).
        
    
% Naive Reverse with Accumulator (N + 2 inferences)
naiveReverseAcc([X|Xs], List):-
    naiveReverseAcc([X|Xs], List, []).

naiveReverseAcc([], Acc, Acc):-!.

naiveReverseAcc([X|XS], List, Acc):-
    naiveReverseAcc(XS, List, [X|Acc]).

    
% Naive Reverse with Append
naiveReverseApp([],[]):-!.
naiveReverseApp([X|Xs],List) :-
  naiveReverseApp(Xs,R),
  naiveAppend(R,[X],List) .
    
    
% Naive Append
naiveAppend([],List,List):-!.
naiveAppend([Head|Tail],List,[Head|New]) :-
  naiveAppend(Tail,List,New) .
                
    
% Assert and Retract
assertList([]):-!.
assertList([X|Xs]):-
    assert(fact(X)),
    assertList(Xs).
    
retractList([]):-!.
retractList([X|Xs]):-
    retract(fact(X)),
    retractList(Xs).
    

% fill a list of number
fill(0, []):-!.

fill(N, [N|Rest]):-
    N1 is N - 1,
    fill(N1, Rest).
        
    

    
    
