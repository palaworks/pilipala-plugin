{ pkgs ? import <nixpkgs> { } }:

(pkgs.buildFHSUserEnv {
  name = "_";

  /* runScript = ''
       bash --init-file <(echo -e 'export PATH=\"/home/thaumy/.nuget/packages/grpc.tools/2.51.0/tools/linux_x64:$PATH\"')
     '';
  */

  targetPkgs = pkgs: [ pkgs.glibc ];

}).env
