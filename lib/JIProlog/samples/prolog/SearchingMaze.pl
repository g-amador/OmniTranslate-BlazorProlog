d(a, b).
d(b, e).
d(b, c).
d(d, e).
d(c, d).
d(e, f).

go(X, X, T).

go(X, Y, T):-d(X, Z), \+ member(Z, T), go(Z, Y, [Z|T]).

go(X, Y, T):-d(Z, Y), \+ member(Z, T), go(Z, Y, [Z|T]).



