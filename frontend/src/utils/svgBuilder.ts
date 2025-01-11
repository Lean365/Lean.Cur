import { readFileSync, readdirSync } from 'fs';
import { resolve } from 'path';
import { Plugin } from 'vite';

const svgTitle = /<svg([^>+].*?)>/;
const clearHeightWidth = /(width|height)="([^>+].*?)"/g;
const hasViewBox = /(viewBox="[^>+].*?")/g;
const clearReturn = /(\r)|(\n)/g;

// 查找SVG文件
function findSvgFile(dir: string): string[] {
  const svgRes = [];
  const dirents = readdirSync(dir, { withFileTypes: true });
  for (const dirent of dirents) {
    if (dirent.isDirectory()) {
      svgRes.push(...findSvgFile(resolve(dir, dirent.name)));
    } else {
      const svg = resolve(dir, dirent.name);
      if (svg.endsWith('.svg')) svgRes.push(svg);
    }
  }
  return svgRes;
}

// 生成SVG雪碧图
function createSvgSprite(svgFiles: string[]) {
  const symbols = svgFiles.map((svg) => {
    let svgFile = readFileSync(svg, 'utf-8');
    const name = svg.slice(svg.lastIndexOf('/') + 1, -4);
    svgFile = svgFile.replace(clearReturn, '');
    svgFile = svgFile.replace(svgTitle, ($1, $2) => {
      let width = 0;
      let height = 0;
      let content = $2.replace(clearHeightWidth, (s1: string, s2: string, s3: number) => {
        if (s2 === 'width') {
          width = s3;
        } else if (s2 === 'height') {
          height = s3;
        }
        return '';
      });
      if (!hasViewBox.test($2)) {
        content += `viewBox="0 0 ${width} ${height}"`;
      }
      return `<symbol id="icon-${name}" ${content}>`;
    });
    svgFile = svgFile.replace('</svg>', '</symbol>');
    return svgFile;
  });
  return `<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" style="position: absolute; width: 0; height: 0">${symbols.join('')}</svg>`;
}

export function svgBuilder(path: string): Plugin {
  const svgFiles = findSvgFile(path);
  return {
    name: 'svg-transform',
    transformIndexHtml(html) {
      return html.replace(
        '<body>',
        `<body>${createSvgSprite(svgFiles)}`
      );
    }
  };
} 