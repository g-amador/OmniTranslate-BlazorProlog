% ORC → TEXT DICTIONARY

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

% --- DCG for parsing Orc words ---

sequence([X|R]) --> character(X), sequence(R).
sequence([X])   --> character(X).

% Orc alphabet (no e j q v w y z)
character(Char) -->
    [C],
    { \+ member(C, "ejqvwyz"),
      atom_codes(Char, [C])
    }.

% Known Orc word → translate to English
word(English) -->
    sequence(Letters),
    {
        atom_chars(OrcAtom, Letters),
        (   orc_word(English, OrcAtom)
        ->  true
        ;   English = OrcAtom      % unknown → keep original
        )
    }.

% A phrase is a list of words separated by spaces
phrase_orc([W|R]) --> word(W), spaces, phrase_orc(R).
phrase_orc([W])   --> word(W).

spaces --> " ", spaces.
spaces --> " ".

% --- Public API predicate for Blazor ---

% Convert Orc text to English (unknown words preserved)
orc_to_text(OrcString, Words) :-
    string_lower(OrcString, Lower),
    string_codes(Lower, Codes),
    (   phrase(phrase_orc(Words), Codes)
    ->  true
    ;   Words = []   % if parsing fails
    ).
