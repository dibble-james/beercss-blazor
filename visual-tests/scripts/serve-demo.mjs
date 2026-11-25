// Publishes the story app and serves the published output as static files on a fixed port, for
// Playwright's webServer. The app is a Blazor WebAssembly project, so the published output is a
// folder of static files, but Chromium still needs a real http origin: it cannot run the runtime
// from file://. Serving the publish output keeps baselines tied to the assets that ship.
import { spawn } from 'node:child_process';
import { createRequire } from 'node:module';
import { existsSync } from 'node:fs';
import path from 'node:path';
import { fileURLToPath } from 'node:url';

const require = createRequire(import.meta.url);
const repoRoot = path.resolve(path.dirname(fileURLToPath(import.meta.url)), '../..');
const publishDirectory = path.join(repoRoot, '.demo-publish');
const port = process.env.BEERCSS_DEMO_PORT ?? '4173';

function resolveDotnet() {
  const host = process.env.DOTNET_HOST_PATH;
  if (host && existsSync(host)) return host;

  return 'dotnet';
}

const published = await new Promise((resolve, reject) => {
  const publish = spawn(
    resolveDotnet(),
    [
      'publish',
      path.join(repoRoot, 'BeerCss.Blazor.Stories'),
      '--configuration',
      'Release',
      '--output',
      publishDirectory,
      '--nologo',
    ],
    { cwd: repoRoot, stdio: 'inherit', env: { ...process.env, DOTNET_CLI_TELEMETRY_OPTOUT: '1' } },
  );

  publish.on('error', reject);
  publish.on('exit', (code) =>
    code === 0
      ? resolve()
      : reject(new Error(`dotnet publish of the story demo failed with exit code ${code}`)),
  );
});

void published;

const server = spawn(
  process.execPath,
  [
    require.resolve('http-server/bin/http-server'),
    path.join(publishDirectory, 'wwwroot'),
    '--address',
    '127.0.0.1',
    '--port',
    port,
    '--silent',
  ],
  { stdio: 'inherit' },
);

for (const signal of ['SIGINT', 'SIGTERM']) {
  process.on(signal, () => server.kill(signal));
}

server.on('exit', (code) => process.exit(code ?? 0));
