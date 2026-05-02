// Provides a simple wrapper for copying text to the clipboard.
// This is used by your <CopyButton> component.
window.copyToClipboard = (text) => {
    navigator.clipboard.writeText(text);
};