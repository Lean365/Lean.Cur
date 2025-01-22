// 生成版本信息
const fs = require('fs');
const path = require('path');
const packageJson = require('../package.json');

const versionInfo = {
  version: packageJson.version,
  buildTime: new Date().toISOString(),
  nodeVersion: process.version
};

const versionFile = path.resolve(__dirname, '../src/version.json');
fs.writeFileSync(versionFile, JSON.stringify(versionInfo, null, 2)); 