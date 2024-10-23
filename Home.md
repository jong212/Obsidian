
<%* 
const pages = dv.pages('"Posts"') .where(p => p.mtime >= dv.date("today") - dv.duration("7 days")) .sort(p => p.mtime, 'desc'); pages.forEach(p => tR += `- [${p.file.name}](${p.file.path}) (Modified: ${p.file.mtime})\n`); 
%>




<a href="https://hits.seeyoufarm.com"><img src="https://hits.seeyoufarm.com/api/count/incr/badge.svg?url=https%3A%2F%2Fpublish.obsidian.md%2Funity%2FHome&count_bg=%2379C83D&title_bg=%23555555&icon=&icon_color=%23E7E7E7&title=hits&edge_flat=false"/></a>