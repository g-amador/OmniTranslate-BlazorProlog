// Simple helper to return the current viewport height in pixels.
// This is the cleanest and safest way to expose window.innerHeight to Blazor.
window.ScreenTools = {
    getHeight: () => window.innerHeight
};
