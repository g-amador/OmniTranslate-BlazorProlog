% ENGLISH → MINION DICTIONARY

minion_word(hello, "bello").
minion_word(hi, "poopaye").
minion_word(what, "po ka").
minion_word(where, "po ka").
minion_word(you, "tu").
minion_word(me, "me").
minion_word(apple, "bapple").
minion_word(banana, "banana").
minion_word(friend, "buddies").
minion_word(sorry, "bi do").
minion_word(icecream, "gelato").
minion_word(fire, "bee do").
minion_word(stop, "stopa").
minion_word(yes, "si").
minion_word(no, "non").

% --- DCG for parsing English words ---

sequence([X|R]) --> character(X), sequence(R).
sequence([X])   --> character(X).

character(Char) -->
    [C],
    { 0'a =< C, C =< 0'z,
      atom_codes(Char, [C])
    }.

% Known English word → translate to Minion
word(Minion) -->
    sequence(Letters),
    {
        atom_chars(EngAtom, Letters),
        (   minion_word(EngAtom, Minion)
        ->  true
        ;   Minion = EngAtom      % unknown → keep original
        )
    }.

% A phrase is a list of words separated by spaces
phrase_minion([W|R]) --> word(W), spaces, phrase_minion(R).
phrase_minion([W])   --> word(W).

spaces --> " ", spaces.
spaces --> " ".

% --- Public API predicate for Blazor ---

% Convert English text to Minion (unknown words preserved)
text_to_minion(Text, MinionWords) :-
    string_lower(Text, Lower),
    string_codes(Lower, Codes),
    (   phrase(phrase_minion(MinionWords), Codes)
    ->  true
    ;   MinionWords = []
    ).
