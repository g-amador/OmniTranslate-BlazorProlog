% MORSE -> TEXT

% Known Morse codes
code(a, ".-").
code(b, "-...").
code(c, "-.-.").
code(d, "-..").
code(e, ".").
code(f, "..-.").
code(g, "--.").
code(h, "....").
code(i, "..").
code(j, ".---").
code(k, "-.-").
code(l, ".-..").
code(m, "--").
code(n, "-.").
code(o, "---").
code(p, ".--.").
code(q, "--.-").
code(r, ".-.").
code(s, "...").
code(t, "-").
code(u, "..-").
code(v, "...-").
code(w, ".--").
code(x, "-..-").
code(y, "-.--").
code(z, "--..").

code('0', "-----").
code('1', ".----").
code('2', "..---").
code('3', "...--").
code('4', "....-").
code('5', ".....").
code('6', "-....").
code('7', "--...").
code('8', "---..").
code('9', "----.").

% --- DCG for parsing Morse input ---

% A Morse "word" is a list of Morse sequences separated by spaces
morse_words([W|R]) --> morse_word(W), "  ", morse_words(R).
morse_words([W])   --> morse_word(W).

% A Morse word is a list of Morse letters separated by single spaces
morse_word([L|R]) --> morse_letter(L), " ", morse_word(R).
morse_word([L])   --> morse_letter(L).

% A Morse letter is a sequence of dots and dashes
morse_letter(Char) -->
    morse_sequence(Seq),
    { atom_chars(Atom, Seq),
      (   code(Char, Atom)      % known Morse
      ->  true
      ;   Char = Atom           % unknown -> keep as-is
      )
    }.

% A sequence of dots/dashes
morse_sequence([X|R]) --> morse_symbol(X), morse_sequence(R).
morse_sequence([X])   --> morse_symbol(X).

morse_symbol('.') --> ".".
morse_symbol('-') --> "-".

% --- Public API predicate for Blazor ---

% Convert a Morse string into plain text (unknown sequences preserved)
morse_to_text(MorseString, Text) :-
    string_codes(MorseString, Codes),
    (   phrase(morse_words(Words), Codes)
    ->  decode_words(Words, Chars),
        string_chars(Text, Chars)
    ;   Text = ""   % if parsing fails entirely
    ).

% Decode a list of Morse words into characters
decode_words([], []).
decode_words([W|R], Chars) :-
    decode_word(W, WordChars),
    decode_words(R, Rest),
    append(WordChars, [' '], Temp),
    append(Temp, Rest, Chars).

decode_word([], []).
decode_word([C|R], [C|Rest]) :-
    decode_word(R, Rest).
