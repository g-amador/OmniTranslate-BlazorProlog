% MINION -> ENGLISH DICTIONARY

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

% --- DCG for parsing Minion words ---

sequence([X|R]) --> character(X), sequence(R).
sequence([X])   --> character(X).

character(Char) -->
    [C],
    { 0'a =< C, C =< 0'z,
      atom_codes(Char, [C])
    }.

% Known Minion word -> translate to English
word(English) -->
    sequence(Letters),
    {
        atom_chars(MinionAtom, Letters),
        (   minion_word(English, MinionAtom)
        ->  true
        ;   English = MinionAtom      % unknown -> keep original
        )
    }.

% A phrase is a list of words separated by spaces
phrase_english([W|R]) --> word(W), spaces, phrase_english(R).
phrase_english([W])   --> word(W).

spaces --> " ", spaces.
spaces --> " ".

% --- Public API predicate for Blazor ---

% Convert Minion text to English (unknown words preserved)
minion_to_text(MinionString, Words) :-
    string_lower(MinionString, Lower),
    string_codes(Lower, Codes),
    (   phrase(phrase_english(Words), Codes)
    ->  true
    ;   Words = []
    ).
