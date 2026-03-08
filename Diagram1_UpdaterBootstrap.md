```mermaid
flowchart TD subgraph "UtilityPDF (C#)"
    A["Read UpdateURL from appsettings.json"] --> B["Validate URL: HTTPS + github.com + regex"]
    B -->|"❌ Invalid"| B1["ABORT: untrusted URL"]
    B -->|"✅ OK"| C["GET /repos/owner/repo/releases/latest"]
    C --> D["Parse release: tag_name, assets[]"]
    D --> E["Extract .7z/.sha256/.sig download URLs"]
    E --> F["Validate all asset URLs: github.com domain"]
    F --> G["Write update_in_progress.json with URLs"]
    G --> H["Verify UpdaterBootstrap.exe integrity"]
    H -->|"✅ OK"| I["Process.Start(UpdaterBootstrap)"]
end

subgraph "UpdaterBootstrap (C++)"
    I --> J["Read UpdateURL from appsettings.json"]
    J --> K["Validate URL: HTTPS + github.com + regex"]
    K -->|"❌ Invalid"| K1["ABORT: config tampered"]
    K -->|"✅ OK"| L["Read update_in_progress.json"]
    L --> M["Validate marker: sourceUpdateUrl == config UpdateURL"]
    M -->|"❌ Mismatch"| K1
    M -->|"✅ OK"| N["Validate all download URLs: HTTPS + github.com domain"]
    N -->|"❌ Untrusted"| K1
    N -->|"✅ OK"| O["Download .7z + .sha256 + .sig"]
    O --> P["Verify SHA-256 hash"]
    P -->|"❌ Mismatch"| P1["ABORT: corrupted/tampered"]
    P -->|"✅ OK"| Q["Verify RSA-4096 signature"]
    Q -->|"❌ Invalid"| P1
    Q -->|"✅ OK"| R["Backup → backups/pre-update_v1.7.5.zip"]
    R --> S["Extract archive into app directory"]
    S --> T["Post-update integrity check"]
    T -->|"❌ Failed"| U["Restore backup"]
    T -->|"✅ OK"| V["Restart UtilityPDF.exe"]
end
```
