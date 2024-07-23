---
cssclasses:
  - dashboard
dg-home: true
dg-publish: true
---
# 프로젝트 (`$=dv.pages('"03.Unity/Project"').length`)
```dataviewjs
dv.table(["제목", "최초작성일", "마지막 수정일", "태그"],
  dv.pages('"03.Unity/Project"')
    .sort(f => f.file.mtime.ts, "desc")
    .map(f => [
      f.file.link,
      dv.date(f.file.ctime).toISODate(), // 작성 날짜
      new Date(f.file.mtime).toISOString().split('T')[0], // 수정 날짜, 시간 부분 제거
      f["핵심기술"]
    ])
)
```

# 정리 (`$=dv.pages('"03.Unity/정리"').length`)
```dataviewjs
dv.table(["제목", "최초작성일", "마지막 수정일", "태그"],
  dv.pages('"03.Unity/정리"')
    .sort(f => f.file.mtime.ts, "desc")
    .map(f => [
      f.file.link,
      dv.date(f.file.ctime).toISODate(), // 작성 날짜
      new Date(f.file.mtime).toISOString().split('T')[0], // 수정 날짜, 시간 부분 제거
      f["MOC"]
    ])
)
```
# 💻 Simple Code (`$=dv.pages('"03.Unity/SimpleCode"').length`)
```dataviewjs
dv.table(["제목", "최초작성일", "마지막 수정일", "태그"],
  dv.pages('"03.Unity/SimpleCode"')
    .sort(f => f.file.mtime.ts, "desc")
    .map(f => [
      f.file.link,
      dv.date(f.file.ctime).toISODate(), // 작성 날짜
      new Date(f.file.mtime).toISOString().split('T')[0], // 수정 날짜, 시간 부분 제거
      f.tags
    ])
)
```

# ☁ Server (`$=dv.pages('"03.Unity/Server"').length`)
```dataviewjs
dv.table(["제목", "최초작성일", "마지막 수정일", "태그"],
  dv.pages('"03.Unity/Server"')
    .sort(f => f.file.mtime.ts, "desc")
    .map(f => [
      f.file.link,
      dv.date(f.file.ctime).toISODate(), // 작성 날짜
      new Date(f.file.mtime).toISOString().split('T')[0], // 수정 날짜, 시간 부분 제거
      f.tags
    ])
)
```

# Error (`$=dv.pages('"03.Unity/Error"').length`)
```dataviewjs
dv.table(["제목", "최초작성일", "마지막 수정일", "태그"],
  dv.pages('"03.Unity/Error"')
    .sort(f => f.file.mtime.ts, "desc")
    .map(f => [
      f.file.link,
      dv.date(f.file.ctime).toISODate(), // 작성 날짜
      new Date(f.file.mtime).toISOString().split('T')[0], // 수정 날짜, 시간 부분 제거
      f.tags
    ])
)
```

<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Black+Han+Sans&display=swap" rel="stylesheet">
