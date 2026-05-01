% ORC WORD DICTIONARY

orc_word(hi, charach).
orc_word(bye, futchi).
orc_word(yes, mok).
orc_word(no, nok).
orc_word(buy, goshak).
orc_word(weapon, porack).
orc_word(sword, burkabata).
orc_word(sabre, charcha).
orc_word(hatchet, hakhak).
orc_word(shortsword, burka).
orc_word(bow, batuk).
orc_word(arrows, pixo).
orc_word(armor, bora).
orc_word(shield, dora).
orc_word(brassshield, donga).
orc_word(studded, tulak).
orc_word(leather, aka).
orc_word(studdedhelmet, grofa).

% --- DCG for parsing text into words ---

sequence([X|R]) --> character(X), sequence(R).
sequence([X])   --> character(X).

character(Char) -->
    [C],
    { 0'a =< C, C =< 0'z,
      atom_codes(Char, [C])
    }.

% Known word -> translate to Orc
word(Orc) -->
    sequence(Letters),
    { atom_chars(Atom, Letters),
      (   orc_word(Atom, Orc)
      ->  true
      ;   Orc = Atom        % unknown -> keep original
      )
    }.

% A phrase is a list of words separated by spaces
phrase_orc([W|R]) --> word(W), spaces, phrase_orc(R).
phrase_orc([W])   --> word(W).

spaces --> " ", spaces.
spaces --> " ".

% --- Public API predicate for Blazor ---

% Convert plain text to Orc (unknown words preserved)
text_to_orc(Text, OrcWords) :-
    string_lower(Text, Lower),
    string_codes(Lower, Codes),
    (   phrase(phrase_orc(OrcWords), Codes)
    ->  true
    ;   OrcWords = []   % if parsing fails
    ).
