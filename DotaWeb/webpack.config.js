var webpack = require("webpack");
var ExtractTextPlugin = require("extract-text-webpack-plugin");
 
module.exports = {
    entry: "./src/index.js",
    output: {
        path: "MyProject/content",
        filename: "bundle.js",
        publicPath: "/content/"
    },  
    module: {
        loaders: [
            {
                test: /\.js$/,
                exclude: /(node_modules)/,
                                                loader: 'babel-loader',
                query: {
                                                    presets: ["latest", "stage-2", "react"]
                }
            },
            {
                test: /\.css$/,
                loader: ExtractTextPlugin.extract({
                                                    fallback: 'style-loader',
                                                    use: 'css-loader!autoprefixer-loader'
                })
            },
            {
                test: /\.scss$/,
                loader: ExtractTextPlugin.extract({
                                                    fallback: 'style-loader',
                                                    use: 'css-loader!autoprefixer-loader!sass-loader'
                })
            },
            { 
                test: /\.(jpg|png)$/, 
                                                loader: 'url-loader' 
            }
        ]
    },
    plugins: [ new ExtractTextPlugin("bundle.css") ]
};