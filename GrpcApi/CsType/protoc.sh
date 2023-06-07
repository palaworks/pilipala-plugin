#!/usr/bin/env bash

declare protoc_path=$HOME/.nuget/packages/grpc.tools/2.51.0/tools/linux_x64/protoc
declare grpc_csharp_plugin_path=$HOME/.nuget/packages/grpc.tools/2.51.0/tools/linux_x64/grpc_csharp_plugin

declare in_root=./src/grpc_proto
declare out_root=./src/grpc_code_gen

$protoc_path \
--plugin=protoc-gen-grpc=$grpc_csharp_plugin_path \
--proto_path=$in_root/comment \
--csharp_out=$out_root/comment \
--grpc_out=$out_root/comment \
$in_root/comment/*.proto \
&&
$protoc_path \
--plugin=protoc-gen-grpc=$grpc_csharp_plugin_path \
--proto_path=$in_root/post \
--csharp_out=$out_root/post \
--grpc_out=$out_root/post \
$in_root/post/*.proto \
&&
$protoc_path \
--plugin=protoc-gen-grpc=$grpc_csharp_plugin_path \
--proto_path=$in_root/token \
--csharp_out=$out_root/token \
--grpc_out=$out_root/token \
$in_root/token/*.proto
