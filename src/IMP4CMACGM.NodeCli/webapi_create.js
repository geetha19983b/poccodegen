const inquirer = require('inquirer')
import fs from 'fs';
import { promisify } from 'util';
import path from 'path';
import https from 'https';
const EventEmitter = require('events')
const ora = require('ora');
const axios = require('axios');
const self = this;
const instance = axios.create({
    httpsAgent: new https.Agent({
        rejectUnauthorized: false
    }),
    'Content-Type': 'application/json;charset=UTF-8'
});
module.exports = class Creator extends EventEmitter {
    constructor(name) {
        super()

        this.name = name
        const { presetPrompt, featurePrompt, namespacePrompt, swaggerPrompt, databasePrompt, messagingPrompt} = this.resolveIntroPrompts(self)
        this.presetPrompt = presetPrompt
        this.featurePrompt = featurePrompt
        this.namespacePrompt = namespacePrompt
        this.swaggerPrompt = swaggerPrompt
        this.databasePrompt = databasePrompt
        this.messagingPrompt = messagingPrompt
        this.isManualMode = false;
        this.isMessaging = false;

    }
    async create(options, preset = null) {

        options = {
            ...options,
            targetDirectory: options.targetDirectory || process.cwd()
        };

        const templateDir = path.resolve(
            __dirname,
            '../templates',
            options.template
        );
        options.templateDirectory = templateDir;
        options.projectName = this.name;
        this.isManualMode = options.preset;
        preset = await this.promptAndResolvePreset();
        this.spinner = ora('Creating Project').start();
        this.fs = fs;
        this.targetDirectory = options.targetDirectory;
        let self = this;
        this.sanitizePreset(options, preset).then(function (createRequest) {
            //Externalize to have this URL dynamic
            instance.post('http://localhost:2679/WebAPI', createRequest)
                .then((response) => {
                    fs.writeFile(`${self.name}.zip`, response.ProjectZip, { encoding: 'base64' }, function (err) {
                    });
                    self.spinner.succeed(`Done. ${self.name} Project is ready!`);
                }, (error) => {
                    console.log(error)
                    self.spinner.fail(`Failed. Sorry, try again after!`);
                });
        });
    }

    resolveIntroPrompts(self) {
        const namespacePrompt = {
            name: "namespace",
            type: "input",
            message: "What is the namespace of the project?"
        }
        const presetPrompt = {
            name: 'preset',
            type: 'list',
            //when: !self.isManualMode,
            default: 'default',
            message: `Please pick a preset:`,
            choices: [
                {
                    name: 'default(basic project with mandatory settings)',
                    value: 'default'
                },
                {
                    name: 'Manually select features',
                    value: 'manual'
                }
            ]
        }
        const featurePrompt = {
            name: 'features',
            type: 'checkbox',
            message: 'Check the features needed for your project:',
            choices: [
                {
                    name: 'Include Messaging',
                    value: 'usemessaging'
                },
                {
                    name: 'Include Circuit Breaker',
                    value: 'usecircuitbreaker'
                }]
        }
        const messagingPrompt = {
            name: 'messaging',
            type: 'checkbox',
            message: 'Check the type of Messaging needed for your project:',
            choices: [
                {
                    name: 'Apache Kafka',
                    value: 'kafka'
                },
                {
                    name: 'IBM MQ',
                    value: 'ibmmq'
                }]
        }
        const swaggerPrompt = {
            name: "swaggerPath",
            type: "input",
            message: "Please provide SwaggerPath",
            validate: function (input) {
                try {
                    var pathExpression = new RegExp("^([A-z0-9-_+]+\:+\/)*([A-z0-9-_+]+\/)*([A-z0-9]+\.(json|yml|yaml))$");
                    const urlExpression = /(http|https):\/\/([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:\/~+#-]*[\w@?^=%&\/~+#-])+\.(json|yml|yaml)$/gm;
                    let m;
                    if (pathExpression.test(input) || (m = urlExpression.exec(input)) !== null) {
                        return true;
                    }
                    return "Please provide valid SwaggerPath";

                } catch (error) {
                    console.log(error);
                }
            }
        }


        const databasePrompt = {
            name: 'database',
            type: 'checkbox',
            when: self.isManualMode,
            default: 0,
            message: `Please pick database(s) need:`,
            choices: [
                {
                    name: 'MongoDB',
                    value: 'mongodb'
                },
                {
                    name: 'Oracle',
                    value: 'oracle'
                }
            ]
        }
        return {
            presetPrompt,
            featurePrompt,
            namespacePrompt,
            swaggerPrompt,
            databasePrompt,
            messagingPrompt,
        }
    }
    async resolvePreset(answers) {
        answers.database = ['mongodb']
        answers.features = []
        answers.messaging = []
        return answers;
    }
    async promptAndResolvePreset() {
        // prompt
        let answers = {};
        await inquirer.prompt(this.swaggerPrompt).then(function (input) {
            answers.swaggerPath = input.swaggerPath;
        });
        await inquirer.prompt(this.namespacePrompt).then(function (input) {
            answers.namespace = input.namespace;
        });;
        await inquirer.prompt(this.presetPrompt).then(function (input) {
            answers.preset = input.preset;
        });;
        if (answers.preset && answers.preset !== 'manual') {
            answers.preset = 'default'
            answers = await this.resolvePreset(answers)
        } else {
            await inquirer.prompt(this.databasePrompt).then(function (input) {
                answers.database = input.database;
            });
            await inquirer.prompt(this.featurePrompt).then(function (input) {
                answers.features = input.features;
            });
            if (answers.features.indexOf("usemessaging") > -1)
                await inquirer.prompt(this.messagingPrompt).then(function (input) {
                    answers.messaging = input.messaging;
                });
            answers.features = answers.features || []
        }
        return answers;
    }

    resolveFinalPrompts() {

        const prompts = [
            this.swaggerPrompt,
            this.namespacePrompt,
            this.presetPrompt,
            this.databasePrompt,
            this.featurePrompt,
            this.messagingPrompt,
        ]
        return prompts
    }

    async sanitizePreset(options, answers) {

        let createPreset = {
            projectName: options.projectName,
            preset: answers.preset || 'webAPI',
            templateType: options.template,
            saveToGit: options.git,
            namespaceName: answers.namespace,
            swaggerPath: answers.swaggerPath,
            databases: answers.database || ['mongodb'],
            messaging: answers.messaging || [],
            gitUsername: options.gitUsername,
            gitPassword: options.gitPassword,
            gitURL: options.gitURL,
            useCircuitBreaker: answers.features.indexOf('usecircuitbreaker') > -1
        };
        return createPreset;
    }

}