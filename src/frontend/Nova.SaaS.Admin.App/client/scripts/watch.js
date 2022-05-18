//https://gist.github.com/int128/e0cdec598c5b3db728ff35758abdbafd

process.env.NODE_ENV = 'development';

const fs = require('fs-extra');
const paths = require('../config/paths');
const webpack = require('webpack');
const webpackconfig = require('../config/webpack.config.js');
const config = webpackconfig('development');

// removes react-dev-utils/webpackHotDevClient.js at first in the array
// config.entry.shift();

webpack(config).watch({}, (err, stats) => {
    if (err) {
        console.error(err);
    } else {
        copyPublicFolder();
    }
    console.error(stats.toString({
        chunks: false,
        colors: true
    }));
});

function copyPublicFolder() {
    fs.copySync(paths.appPublic, paths.appBuild, {
        dereference: true,
        filter: file => file !== paths.appHtml
    });
}
