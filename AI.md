# OmniTranslate – AI Assistant Guide

OmniTranslate includes a simple AI chat assistant connected to an Azure OpenAI endpoint.  
It functions only as a **general-purpose chatbot** inside the application.

The AI assistant does not have access to the OmniTranslate codebase, files, dictionaries, or internal logic.  
It only knows what the user types into the chat window.

---

---

## 📑 <a name="toc">Table of Contents

1. [🧠 What the AI Assistant Does](#what-ai-does)
2. [📦 Where the AI Logic Lives](#ai-logic-location)
3. [✨ Using External AI Tools to Generate New Languages](#using-ai-tools)
4. [🧩 Recommended Prompt for AI Code‑Generation Tools](#recomendations)  
5. [📌 Notes](#notes)

---

## 🧠 <a name="what-ai-does"> What the AI Assistant Does

- Answer general questions the user asks
- Provide explanations or examples **only based on the text the user provides**
- Help generate code or Prolog rules **when explicitly requested**
- Assist contributors by responding to prompts the user writes manually

The AI assistant does **not**:

- Translate text
- Access or modify language dictionaries
- Generate Prolog rules automatically
- Interact with the translation pipeline
- Know anything about OmniTranslate unless the user explains it in the chat

All translation is handled by **Prolog + C# translators**, not AI.

[Back to Table of contents](#toc)

---

## 📦 <a name="ai-logic-location"> Where the AI Logic Lives

The AI assistant logic is contained in:

```
/Services/AiChatService.cs
```


It is only used by the chat UI component.

[Back to Table of contents](#toc)

---

## ✨ <a name="using-ai-tools"> Using External AI Tools to Generate New Languages

Contributors may use external AI tools (ChatGPT, Claude, Copilot, etc.) to help generate:

- Prolog dictionaries
- Translator classes
- Example phrases

To keep contributions consistent, use the following prompt.

[Back to Table of contents](#toc)

---

## 🧩 <a name="recomendations"> Recommended Prompt for AI Code‑Generation Tools

```
You are contributing to OmniTranslate, a Blazor + Prolog translation engine.

Your task:

1. Generate a Prolog dictionary for a fictional or constructed language.
Use the pattern: <language>_word("source", "target").
Provide 20–40 entries that match the style and lore of the language.

2. Generate two C# translator classes:
- EnglishTo<Language>Translator
- <Language>ToEnglishTranslator

Both must:
- Implement the ITranslator interface
- Load the .pl dictionary you created
- Follow the existing translator structure in the project

Do not modify any other files.
Return only the code.
```

[Back to Table of contents](#toc)

---

## 📌 <a name="notes"> Notes

- The AI assistant inside OmniTranslate is **just a chatbox**, not part of the translation engine.
- It cannot read or understand the project unless the user pastes information into the chat.
- All translation logic must remain in **Prolog + C# translators**.
- Contributors are free to use external AI tools to generate code, but contributions must follow the project structure.

[Back to Table of contents](#toc)

---
