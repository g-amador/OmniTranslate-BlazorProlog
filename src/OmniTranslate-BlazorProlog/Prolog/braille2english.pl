% BRAILLE → TEXT

% same braille/2 facts as above

% --- DCG for parsing Braille input ---

digit('0') --> "0".
digit('1') --> "1".

braille_cell([A,B,C,D,E,F]) -->
    digit(A), digit(B), digit(C), digit(D), digit(E), digit(F).

% Known Braille → letter
braille_letter(Char) -->
    braille_cell(Cell),
    {
        atom_chars(Pattern, Cell),
        (   braille(Char, Pattern)
        ->  true
        ;   Char = Pattern   % unknown → preserve
        )
    }.

% A word is letters separated by spaces
braille_word([C|R]) --> braille_letter(C), " ", braille_word(R).
braille_word([C])   --> braille_letter(C).

% A phrase is words separated by double spaces
braille_phrase([W|R]) --> braille_word(W), "  ", braille_phrase(R).
braille_phrase([W])   --> braille_word(W).

% --- Public API ---

braille_to_text(BrailleString, Text) :-
    string_codes(BrailleString, Codes),
    (   phrase(braille_phrase(Words), Codes)
    ->  decode_words(Words, Chars),
        string_chars(Text, Chars)
    ;   Text = ""
    ).

decode_words([], []).
decode_words([W|R], Chars) :-
    decode_word(W, WordChars),
    decode_words(R, Rest),
    append(WordChars, [' '], Temp),
    append(Temp, Rest, Chars).

decode_word([], []).
decode_word([C|R], [C|Rest]) :-
    decode_word(R, Rest).
