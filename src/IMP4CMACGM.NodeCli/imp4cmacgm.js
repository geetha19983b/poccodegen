#!/usr/bin/env node

require = require('esm')(module /*, options*/);
const program = require('commander')
const chalk = require('chalk')
const minimist = require('minimist')

program
    .usage('<command> [options]')

program
    .command('create <app-name>')
    .description('create a new project')
    .option('-t, --type <projectType>', 'Skip prompts and use given project type. ex: webAPI')
    .option('-d, --default', 'Skip prompts and use default preset')
    .option('-c, --config <jsonPath>', 'Skip prompts and use the config file as preset.')
    .option('-n, --no-git', 'Skip Git push and download the generated file')
    .action((name, cmd) => {
        const options = cleanArgs(cmd)

        if (minimist(process.argv.slice(3))._.length > 1) {
            console.log(chalk.yellow('\n Info: You provided more than one argument. The first one will be used as the app\'s name, the rest are ignored.'))
        }
        if (process.argv.includes('-d') || process.argv.includes('--default')) {
            options.preset = "default"
        }
        //require('../lib/create')(name, options)
        require('./cli.js').cli(name, process.argv);
    })
// output help information on unknown commands
program
    .arguments('<command>')
    .action((cmd) => {
        program.outputHelp()
        console.log(`  ` + chalk.red(`Unknown command ${chalk.yellow(cmd)}.`))
        console.log()
    })

// add some useful info on help
program.on('--help', () => {
    console.log()
    console.log(`  Run ${chalk.cyan(`imp <command> --help`)} for detailed usage of given command.`)
    console.log()
})

program.commands.forEach(c => c.on('--help', () => console.log()))
program.parse(process.argv);
if (!process.argv.slice(2).length) {
    program.outputHelp()
}
//require('../src/cli.js').cli(process.argv);
function cleanArgs(cmd) {
    const args = {}
    cmd.options.forEach(o => {
        const key = camelize(o.long.replace(/^--/, ''))
        // if an option is not present and Command has a method with the same name
        // it should not be copied
        if (typeof cmd[key] !== 'function' && typeof cmd[key] !== 'undefined') {
            args[key] = cmd[key]
        }
    })
    return args
}
function camelize(str) {
    return str.replace(/-(\w)/g, (_, c) => c ? c.toUpperCase() : '')
}
