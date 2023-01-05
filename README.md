# pilipala.plugin

噼哩啪啦插件存储库

plugins of pilipala

## 目录

### Llink*

替换正文中的`<{}>`标记块。

### Summarizer

为`IPost`提供`Summary`字段（概要），并在必要时从正文生成概要。

### EmailNotifier

使用电子邮件对噼哩啪啦事件予以通知。

### Cacher

为加载文章和评论提供缓存加速。

### Markdown

翻译正文中的 Markdown 到 HTML。

### ViewCount

提供文章访问计数。

### PostCover

提供文章封面。

### PostStatus

提供文章归档状态和计划状态。

### PartialOrder

提供当前文章的前驱和后继ID导航。

### UserName

为评论提供`UserName`字段

### Mark

提供一个标记字符串，通常用于对文章进行元数据分类。

### WsApi

提供基于 WebSocket 的噼哩啪啦 API 访问

### GrpcApi

提供基于 GRPC 的噼哩啪啦 API 访问

## 注解

* 带有星号标识(*)的插件为测试项目，可能存在一些问题。
