import arg from 'arg';
import inquirer from 'inquirer';
const Creator = require('./webapi_create');
function parseArgumentsIntoOptions(rawArgs) {
    const args = arg(
        {
            '--type': String,
            '-t': '--type',
            '--default': Boolean,
            '-d': '--default',
            '--config': String,
            '-c': '--config',
            '--no-git': Boolean,
            '-n': '--no-git',
        },
        {
            argv: rawArgs.slice(2),
        }
    );
    return {
        type: args['--type'] || '',
        preset: args['--default'] || false,
        config: args['--config'] || '',
        nogit: args['--no-git'] || false
    };
}

async function promptForMissingOptions(options) {
    const defaultTemplate = 'webAPI';

    const questions = [];
    if (!options.type) {
        questions.push({
            type: 'list',
            name: 'template',
            message: 'Please choose which project template to use',
            choices: ['webAPI', 'SPA'],
            default: defaultTemplate,
        });
    }


        questions.push({
            type: 'confirm',
            name: 'git',
            when: !options.nogit,
            message: 'Should the project be pushed to git?',
            default: false,
        });

        

    const answers = await inquirer.prompt(questions);
    let gitAnswers = {};
    const gitQuestions = [];
    if (answers.git) {
        gitQuestions.push({
            type: 'input',
            name: 'gitURL',
            when: answers.git,
            message: 'Please provide Git Repository Url',
        });


        gitQuestions.push({
            type: 'input',
            name: 'gitUsername',
            when: answers.git,
            message: 'Please provide Git Username',
        });

        gitQuestions.push({
            type: 'password',
            name: 'gitPassword',
            when: answers.git,
            message: 'Please provide Git Password',
        });
        gitAnswers = await inquirer.prompt(gitQuestions);
    }
    return {
        ...options,
        template: options.type || answers.template,
        git: options.git || answers.git,
        gitURL: gitAnswers.gitURL || '',
        gitUsername: gitAnswers.gitUsername || '',
        gitPassword: gitAnswers.gitPassword || ''
    };
}

export async function cli(name, args) {
    let options = parseArgumentsIntoOptions(args);
    options = await promptForMissingOptions(options);
    const creator = new Creator(name)
    await creator.create(options);
}
