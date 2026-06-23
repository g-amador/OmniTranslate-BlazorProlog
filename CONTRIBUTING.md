# OmniTranslate – How to Contribute

Thank you for your interest in contributing to OmniTranslate!

This project is designed to be easy to extend.  
Below are the two main areas where contributions are welcome:

- Adding new fictional or constructed languages  
- Exposing the translation engine through a REST API  

---

## 📑 <a name="toc">Table of Contents

1. [➕ Adding New Languages](#adding-new-languages)
2. [🌍 Languages You Can Contribute](#new-languages)
3. [🌐 Exposing OmniTranslate Through an API](#exposing-omnitranslate)
4. [🧪 Tests](#tests)  
5. [❤️ Thank You](#thanks)

---

## ➕ <a name="adding-new-languages"> Adding New Languages

OmniTranslate supports modular language packs.  
To add a new language, follow these steps:

### 📄 Create a Prolog Dictionary

Add a new `.pl` file under:

```
/Prolog/
```

Example:

```
klingon.pl
```

Inside the file, define translation rules using this pattern:

```prolog
klingon_word("hello", "nuqneH").
klingon_word("friend", "jup").
```

Rules must follow:

```prolog
<language>_word("source", "target").
```

This allows the Prolog engine to load your dictionary automatically.

### 🧠 Create Two Translators
Add two C# classes under:

```
/Translators/
```

Example:

```
EnglishToKlingonTranslator.cs
KlingonToEnglishTranslator.cs
```

Both must:
- Implement the ITranslator interface
- Load your .pl dictionary
- Provide the language name and direction

Once these steps are completed, the new language will appear in the OmniTranslate UI.


Once added, the new language becomes available in the UI.

[Back to Table of contents](#toc)

---

## 🌍 <a name="new-languages"> Languages You Can Contribute

The following fictional and constructed languages are planned for OmniTranslate and open for contribution:

- **Klingon** (Star Trek)
- **Dovahzul** (Skyrim)
- **Hylian** (Zelda)
- **Aurebesh** (Star Wars)
- **Tolkien Elvish** (Sindarin / Quenya)

[Back to Table of contents](#toc)

---

## 🌐 <a name="exposing-omnitranslate"> Exposing OmniTranslate Through an API

Contributors may also help expose the translation engine via a REST API.

Planned endpoints include:

- `POST /translate` — Translate text between supported languages  
- `GET /languages` — List available languages  

Contributions may include:

- Implementing API controllers  
- Creating request/response models  
- Adding validation  
- Writing API documentation  
- Adding tests for API behavior  

[Back to Table of contents](#toc)

---

## 🧪 <a name="tests"> Tests

If you add a new language or API feature, please include tests under:

```
/Tests/
```

[Back to Table of contents](#toc)

---

## ❤️ <a name="thanks"> Thank You
Your contributions help expand OmniTranslate into a universal translator for fictional and constructed languages.

[Back to Table of contents](#toc)
