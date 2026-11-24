using System.Runtime.CompilerServices;
using VerifyTests;

namespace BeerCss.Blazor.VisualTests;

internal static class VerifySetup
{
    [ModuleInitializer]
    internal static void Initialize()
    {
        // Never try to open a diff tool from a test run (headless machines and CI).
        Environment.SetEnvironmentVariable("DiffEngine_Disabled", "true");

        // Downloads Chromium for the pinned Playwright version on first use.
        VerifyPlaywright.Initialize(installPlaywright: true);

        PngComparer.Register();
    }
}
