var path = require('path');

module.exports = {
  context: path.resolve('js'),
  entry: ['./utils', './app'],
  output: {
    path: path.resolve('build/js'),
    publicPath: '/public/assets/js/',
    filename: 'bundle.js'
  },
  devServer: {
    contentBase: 'public'
  },
  module: {
    rules: [
      {
        test: /\.es6$/,
        enforce: "pre",
        exclude: /node_modules/,
        use: [{ loader: "jshint-loader"}]
      },
      {
        test: /\.es6$/,
        exclude: /node_modules/,
        use: { 
          loader: 'babel-loader', 
          options: { presets: ['env'] } 
        }
      }
    ],
  },
  resolve: {
    extensions: ['.js', '.es6']
  }
}