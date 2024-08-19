{
  pkgs ? import <nixpkgs> { overlays = [ (import <bsuir-tex/nixpkgs>) ]; },
}:
with pkgs;
(mkShell.override { stdenv = pkgs.stdenvNoCC; }) rec {
  name = "SD-1";

  vscode-settings = writeText "settings.json" (
    builtins.toJSON { "dotnetAcquisitionExtension.sharedExistingDotnetPath" = DOTNET_ROOT; }
  );

  dotnetPkg = with dotnetCorePackages; combinePackages [ sdk_8_0 ];
  packages = [
    (texliveSmall.withPackages (_: with texlivePackages; [ bsuir-tex ]))
    python312Packages.pygments
    inkscape-with-extensions
    dotnetPkg
  ];

  DOTNET_ROOT = "${dotnetPkg}/dotnet";

  shellHook = ''
    mkdir .vscode &>/dev/null
    cp --force ${vscode-settings} .vscode/settings.json
  '';
}
