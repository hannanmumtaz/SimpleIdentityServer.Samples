param
(
    $config = 'Release'
)

$migrationSln = Resolve-Path .\Migration.sln
$accountFilterSln = Resolve-Path .\OpenId\AccountFilter\AccountFilter.sln
$authenticationSln = Resolve-Path .\OpenId\Authentication\OpenIdWebsiteAuthentication.sln
$customOpenIdSln = Resolve-Path .\OpenId\CustomOpenidUi\CustomOpenidUi.sln

dotnet build $migrationSln -c $config
dotnet build $accountFilterSln -c $config
dotnet build $authenticationSln -c $config
dotnet build $customOpenIdSln -$config