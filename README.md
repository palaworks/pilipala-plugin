# pilipala.plugin

噼哩啪啦插件存储库

plugins of pilipala

## 可选插件

这类插件的装载与否不会影响噼哩啪啦的正常工作。

### Llink

替换正文中的`<{}>`标记块。

### Summarizer

为`IPost`提供`Summary`字段（概要），并在必要时从正文生成概要。

### EmailNotifier

使用电子邮件对噼哩啪啦事件予以通知。

### Cacher

为文章和评论加载提供缓存加速。

### Markdown

翻译正文中的Markdown到HTML。

### ViewCount

提供文章访问计数。

### PostCover

提供文章封面。

### PostStatus

提供文章归档状态和计划状态。

## 默认集成的插件

噼哩啪啦内核为这些插件提供集成支持，始终应该启用这些插件以保证系统功能的完整性。

### UserName

为`IComment`和`IPost`提供`UserName`字段
