#!/usr/bin/env bash

declare -a plugins=(
    "Mark"
    "Llink"
    "WsApi"
    "Cacher"
    "Topics"
    "GrpcApi"
    "UserName"
    "Markdown"
    "ViewCount"
    "PostCover"
    "PostStatus"
    "Summarizer"
    "UserSiteUrl"
    "PartialOrder"
    "EmailNotifier"
    "UserAvatarUrl"
)

rm -rf ./auto-build
mkdir ./auto-build
touch ./auto-build/.gitignore
printf "*\n!.gitignore" > ./auto-build/.gitignore

for it in "${plugins[@]}"; do
    dotnet build $it/$it.fsproj -c Release -o ./auto-build/$it
done
