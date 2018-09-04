START cmd /k "cd WebsiteProtection && dotnet run"
START cmd /k "cd UmaProvider && dotnet run -f net461"
START cmd /k "cd OpenIdProvider && dotnet run -f net461"
START cmd /k "cd HierarchicalResource && dotnet run -f net461"
echo Applications are running ...