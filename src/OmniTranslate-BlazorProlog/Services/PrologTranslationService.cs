using System.Diagnostics;

namespace OmniTranslate_BlazorProlog.Services;

public class PrologTranslationService
{
    public async Task<string> Translate(string mode, string input)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "swipl",
            Arguments = $"-q -s Prolog/translator.pl -g main -- {mode} \"{input}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);
        string output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();

        return output.Trim();
    }
}
