 [![Hits](https://hits.seeyoufarm.com/api/count/incr/badge.svg?url=https%3A%2F%2Fpublish.obsidian.md%2Funity%2FHome&count_bg=%2386B0E2&title_bg=%23555555&icon=&icon_color=%23E7E7E7&title=hits&edge_flat=false)](https://hits.seeyoufarm.com)

<%*
const dv = app.plugins.plugins["dataview"].api;
const openPublishPanel = app.commands.commands["publish:view-changes"].callback;

// 메인 페이지 파일 이름
const mainPageFileName = "Home.md";

// 각 폴더별로 Dataview 쿼리를 실행하여 게시글 목록을 가져옵니다.
const folders = [
    { name: "02.개인공부", displayName: "📂 개인공부" },
    { name: "01.프로젝트 모음", displayName: "📂 프로젝트 모음" },
    { name: "DesignPattern", displayName: "📂 디자인패턴" }
];

let mainPageContent = "---\n### 최근에 작성한 게시글 Limit 10\n\n\n\n";

// 각 폴더에 대해 반복하여 테이블을 생성합니다.
for (const folder of folders) {
    const query = `TABLE WITHOUT ID file.link AS "Note", dateformat(file.ctime, "yyyy년 MM월 dd일 HH시 mm분") AS "Created"
                   FROM "${folder.name}"
                   SORT file.ctime desc
                   LIMIT 10`; // 각 폴더에서 최근 생성된 게시물 10개만 가져옵니다.

    const queryOutput = await dv.queryMarkdown(query);

    // 폴더별 섹션 추가
    mainPageContent += `### ${folder.displayName}\n\n`;
    if (queryOutput && queryOutput.value) {
        mainPageContent += `${queryOutput.value}\n\n`;
    } else {
        mainPageContent += "게시글이 없습니다.\n\n";
    }
}

try {
    // 메인 페이지 파일을 찾고 업데이트합니다.
    let mainPageFile = tp.file.find_tfile(mainPageFileName);
    if (!mainPageFile) {
        await tp.file.create_new("", mainPageFileName);
        mainPageFile = tp.file.find_tfile(mainPageFileName);
    }

    await app.vault.modify(mainPageFile, mainPageContent);
    new Notice(`메인 페이지(${mainPageFile.basename})가 업데이트되었습니다.`);
} catch (error) {
    new Notice(`⚠️ 메인 페이지 업데이트 오류: ${error}`, 0);
}

// Publish 패널 열기
try {
    if (openPublishPanel) {
        openPublishPanel();
        new Notice("🟢 Publish 패널이 열렸습니다.");
    } else {
        new Notice("⚠️ Publish 명령을 찾을 수 없습니다. 명령어 ID를 확인하세요.");
    }
} catch (error) {
    new Notice(`⚠️ Publish 패널 열기 실패: ${error}`, 0);
    console.error("Error opening Publish panel:", error);
}
%>
