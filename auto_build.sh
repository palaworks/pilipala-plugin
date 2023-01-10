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

rm -rf ./auto_build

for it in "${plugins[@]}"; do
    dotnet build $it/$it.fsproj -c Release -o ./auto_build/$it
done
