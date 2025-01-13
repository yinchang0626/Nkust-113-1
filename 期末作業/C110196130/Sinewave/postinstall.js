// Usage: npm install

const writeTo = './wwwroot/lib'
const defaultPrefix = 'https://unpkg.com'
const files = {
    js: {
        'jquery.js':                          'https://code.jquery.com/jquery-3.7.1.js',
        'jquery.min.js':                      'https://code.jquery.com/jquery-3.7.1.min.js',
        'jquery.min.map':                     'https://code.jquery.com/jquery-3.7.1.min.map',
        'jquery.validate.js':                 '/jquery-validation@1/dist/jquery.validate.js',
        'jquery.validate.min.js':             '/jquery-validation@1/dist/jquery.validate.min.js',
        'additional-methods.min.js':          '/jquery-validation@1/dist/additional-methods.min.js',
        'jquery.validate.unobtrusive.js':     '/jquery-validation-unobtrusive@3/dist/jquery.validate.unobtrusive.js',
        'jquery.validate.unobtrusive.min.js': '/jquery-validation-unobtrusive@3/dist/jquery.validate.unobtrusive.js',
    },
    mjs: {
        'vue.mjs':                            '/vue@3/dist/vue.esm-browser.js',
        'vue.min.mjs':                        '/vue@3/dist/vue.esm-browser.prod.js',
        'servicestack-client.mjs':            '/@servicestack/client@2/dist/servicestack-client.mjs',
        'servicestack-client.min.mjs':        '/@servicestack/client@2/dist/servicestack-client.min.mjs',
        'servicestack-vue.mjs':               '/@servicestack/vue@3/dist/servicestack-vue.mjs',
        'servicestack-vue.min.mjs':           '/@servicestack/vue@3/dist/servicestack-vue.min.mjs',
    },
    typings: {
        'vue/index.d.ts':                     '/vue@3/dist/vue.d.ts',
        '@vue/compiler-core.d.ts':            '/@vue/compiler-core@3/dist/compiler-core.d.ts',
        '@vue/compiler-dom.d.ts':             '/@vue/compiler-dom@3/dist/compiler-dom.d.ts',
        '@vue/runtime-dom.d.ts':              '/@vue/runtime-dom@3/dist/runtime-dom.d.ts',
        '@vue/runtime-core.d.ts':             '/@vue/runtime-core@3/dist/runtime-core.d.ts',
        '@vue/reactivity.d.ts':               '/@vue/reactivity@3/dist/reactivity.d.ts',
        '@vue/shared.d.ts':                   '/@vue/shared@3/dist/shared.d.ts',
        '@servicestack/client/index.d.ts':    '/@servicestack/client@2/dist/index.d.ts',
        '@servicestack/vue/index.d.ts':       '/@servicestack/vue@3/dist/index.d.ts',
    },
    css: {
        'font-awesome/css/all.css':                   '/font-awesome-5-css@5/css/all.css',
        'font-awesome/css/all.min.css':               '/font-awesome-5-css@5/css/all.min.css',
        'font-awesome/css/brands.css':                '/font-awesome-5-css@5/css/brands.css',
        'font-awesome/css/brands.min.css':            '/font-awesome-5-css@5/css/brands.min.css',
        'font-awesome/css/fa-solid.css':              '/font-awesome-5-css@5/css/fa-solid.css',
        'font-awesome/css/fa-solid.min.css':          '/font-awesome-5-css@5/css/fa-solid.min.css',
        'font-awesome/webfonts/fa-brands-400.eot':    '/font-awesome-5-css@5/webfonts/fa-brands-400.eot',
        'font-awesome/webfonts/fa-brands-400.ttf':    '/font-awesome-5-css@5/webfonts/fa-brands-400.ttf',
        'font-awesome/webfonts/fa-brands-400.woff':   '/font-awesome-5-css@5/webfonts/fa-brands-400.woff',
        'font-awesome/webfonts/fa-brands-400.woff2':  '/font-awesome-5-css@5/webfonts/fa-brands-400.woff2',
        'font-awesome/webfonts/fa-regular-400.eot':   '/font-awesome-5-css@5/webfonts/fa-regular-400.eot',
        'font-awesome/webfonts/fa-regular-400.ttf':   '/font-awesome-5-css@5/webfonts/fa-regular-400.ttf',
        'font-awesome/webfonts/fa-regular-400.woff':  '/font-awesome-5-css@5/webfonts/fa-regular-400.woff',
        'font-awesome/webfonts/fa-regular-400.woff2': '/font-awesome-5-css@5/webfonts/fa-regular-400.woff2',
        'font-awesome/webfonts/fa-solid-900.eot':     '/font-awesome-5-css@5/webfonts/fa-solid-900.eot',
        'font-awesome/webfonts/fa-solid-900.ttf':     '/font-awesome-5-css@5/webfonts/fa-solid-900.ttf',
        'font-awesome/webfonts/fa-solid-900.woff':    '/font-awesome-5-css@5/webfonts/fa-solid-900.woff',
        'font-awesome/webfonts/fa-solid-900.woff2':   '/font-awesome-5-css@5/webfonts/fa-solid-900.woff2',
    }
}

const path = require('path')
const fs = require('fs').promises
const http = require('http')
const https = require('https')

const requests = []
Object.keys(files).forEach(dir => {
    const dirFiles = files[dir]
    Object.keys(dirFiles).forEach(name => {
        let url = dirFiles[name]
        if (url.startsWith('/'))
            url = defaultPrefix + url
        const toFile = path.join(writeTo, dir, name)
        requests.push(fetchDownload(url, toFile, 5))
    })
})

;(async () => {
    await Promise.all(requests)
})()

async function fetchDownload(url, toFile, retries) {
    const toDir = path.dirname(toFile)
    await fs.mkdir(toDir, { recursive: true })
    for (let i=retries; i>=0; --i) {
        try {
            let r = await fetch(url)
            if (!r.ok) {
                throw new Error(`${r.status} ${r.statusText}`);
            }
            let txt = await r.text()
            console.log(`writing ${url} to ${toFile}`)
            await fs.writeFile(toFile, txt)
            return
        } catch (e) {
            console.log(`get ${url} failed: ${e}${i > 0 ? `, ${i} retries remaining...` : ''}`)
        }
    }
}