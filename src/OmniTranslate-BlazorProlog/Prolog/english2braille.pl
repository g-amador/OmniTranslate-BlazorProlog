% TEXT -> BRAILLE (6‑dot patterns)

braille(a, "100000").
braille(b, "110000").
braille(c, "100100").
braille(d, "100110").
braille(e, "100010").
braille(f, "110100").
braille(g, "110110").
braille(h, "110010").
braille(i, "010100").
braille(j, "010110").

braille(k, "101000").
braille(l, "111000").
braille(m, "101100").
braille(n, "101110").
braille(o, "101010").
braille(p, "111100").
braille(q, "111110").
braille(r, "111010").
braille(s, "011100").
braille(t, "011110").

braille(u, "101001").
braille(v, "111001").
braille(w, "010111").
braille(x, "101101").
braille(y, "101111").
braille(z, "101011").

% digits (same as a–j)
braille('1', "100000").
braille('2', "110000").
braille('3', "100100").
braille('4', "100110").
braille('5', "100010").
braille('6', "110100").
braille('7', "110110").
braille('8', "110010").
braille('9', "010100").
braille('0', "010110").

% --- DCG for parsing text ---

sequence([X|R]) --> character(X), sequence(R).
sequence([X])   --> character(X).

character(Char) -->
    [C],
    { 0'a =< C, C =< 0'z,
      atom_codes(Char, [C])
    }.

character(Char) -->
    [C],
    { 0'0 =< C, C =< 0'9,
      atom_codes(Char, [C])
    }.

% Unknown character -> keep as-is
character(Char) -->
    [C],
    { atom_codes(Char, [C]) }.

% Convert a word
word([B|R]) --> character(C), { convert_char(C, B) }, word(R).
word([B])   --> character(C), { convert_char(C, B) }.

convert_char(Char, Braille) :-
    (   braille(Char, Braille)
    ->  true
    ;   Braille = Char   % unknown -> preserve
    ).

% A phrase is words separated by spaces
phrase_braille([W|R]) --> word(W), spaces, phrase_braille(R).
phrase_braille([W])   --> word(W).

spaces --> " ", spaces.
spaces --> " ".

% --- Public API ---

text_to_braille(Text, BrailleWords) :-
    string_lower(Text, Lower),
    string_codes(Lower, Codes),
    (   phrase(phrase_braille(BrailleWords), Codes)
    ->  true
    ;   BrailleWords = []
    ).
