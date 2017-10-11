const devConfig = require('./webpack.config');

devConfig.module.rules.push({
  test: [/\.js$/, /\.es6$/],
  exclude: /node_modules/,
  loader: require('strip-loader').loader('console.log')
});

module.exports = devConfig;