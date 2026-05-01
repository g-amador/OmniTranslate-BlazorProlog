% TEXT -> MORSE

code('.-', a).
code('-...', b).
code('-.-.', c).
code('-..', d).
code('.', e).
code('..-.', f).
code('--.', g).
code('....', h).
code('..', i).
code('.---', j).
code('-.-', k).
code('.-..', l).
code('--', m).
code('-.', n).
code('---', o).
code('.--.', p).
code('--.-', q).
code('.-.', r).
code('...', s).
code('-', t).
code('..-', u).
code('...-', v).
code('.--', w).
code('-..-', x).
code('-.--', y).
code('--..', z).

code('-----', '0').
code('.----', '1').
code('..---', '2').
code('...--', '3').
code('....-', '4').
code('.....', '5').
code('-....', '6').
code('--...', '7').
code('---..', '8').
code('----.', '9').

% --- Grammar for converting text to Morse ---

text([P|R]) --> word(P), " ", text(R).
text([P])   --> word(P).

word([M|R]) --> symbol(M), word(R).
word([M])   --> symbol(M).

% Known letter or digit
symbol(M) -->
    [C],
    { 0'a =< C, C =< 0'z,
      char_code(A, C),
      code(M, A)
    }.

symbol(M) -->
    [C],
    { 0'0 =< C, C =< 0'9,
      char_code(A, C),
      code(M, A)
    }.

% Unknown character -> return it unchanged
symbol(Unknown) -->
    [C],
    { char_code(Unknown, C) }.

% --- Public API predicates ---

% Convert plain text to Morse (unknown chars preserved)
text_to_morse(Text, MorseList) :-
    string_lower(Text, Lower),
    string_codes(Lower, Codes),
    phrase(text(MorseList), Codes).

% Convert Morse list back to text (unknown sequences preserved)
morse_to_text(MorseList, Text) :-
    decode_words(MorseList, Chars),
    string_chars(Text, Chars).

decode_words([], []).
decode_words([M|R], [C|Rest]) :-
    (   code(M, C)          % known Morse
    ->  true
    ;   C = M               % unknown -> keep as-is
    ),
    decode_words(R, Rest).
