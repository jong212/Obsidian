<%*
const dv = app.plugins.plugins["dataview"].api;
const openPublishPanel = app.commands.commands["publish:view-changes"].callback;

// 업데이트할 파일명과 쿼리를 설정합니다.
const fileAndQuery = new Map([
  [
    "최근 수정된 노트",
    `TABLE WITHOUT ID
    file.link AS "노트", dateformat(file.mtime, "yyyy-MM-dd HH:mm") AS "수정된 날짜"
    FROM ""
    WHERE publish = true
    SORT file.mtime DESC
    LIMIT 7`
  ],
  [
    "최근 생성된 노트",
    `TABLE WITHOUT ID
    file.link AS "노트", dateformat(file.ctime, "yyyy-MM-dd HH:mm") AS "생성된 날짜"
    FROM ""
    WHERE publish = true
    SORT file.ctime DESC
    LIMIT 7`
  ],
]);

// 각 파일에 대해 쿼리 결과를 마크다운으로 출력합니다.
for (let [filename, query] of fileAndQuery) {
  // 파일이 없으면 생성합니다.
  if (!app.vault.getAbstractFileByPath(filename + ".md")) {
    await app.vault.create(filename + ".md", "");
    new Notice(`${filename} 파일을 생성했습니다.`);
  }

  const tFile = app.vault.getAbstractFileByPath(filename + ".md");
  const queryOutput = await dv.queryMarkdown(query);

  // 파일의 내용을 구성합니다.
  const fileContent = `---
publish: true
---
<!-- 이 파일은 Templater 템플릿에 의해 자동으로 업데이트됩니다 -->

${queryOutput.value}`;

  try {
    // 파일을 수정합니다.
    await app.vault.modify(tFile, fileContent);
    new Notice(`${filename} 파일이 업데이트되었습니다.`);
  } catch (error) {
    new Notice(`⚠️ ${filename} 파일 업데이트 중 오류 발생`, 0);
    console.error(`Error updating ${filename}:`, error);
  }
}

// 퍼블리시 패널을 엽니다.
openPublishPanel();
%>
