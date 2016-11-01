'use strict';
// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as VSCode from 'vscode';
import * as Process from 'child_process'

class VSColorPicker {
    private _config = { autoLaunch: true, autoLaunchDelay: 100 };
    constructor(private _extensionPath: string) {
        let configSection = VSCode.workspace.getConfiguration('vs-color-picker');

        this._config.autoLaunch = configSection.get<boolean>('autoLaunch', this._config.autoLaunch);
        this._config.autoLaunchDelay = configSection.get<number>('autoLaunchDelay', this._config.autoLaunchDelay);
    }

    readonly MAX_VALUE_LEN: number = 20;

    private _timer: NodeJS.Timer = null;

    public OnTextEditorSelectionChange(event: VSCode.TextEditorSelectionChangeEvent) {
        if (!this._config.autoLaunch) {
            return;
        }
        if (this._timer != null) {
            clearTimeout(this._timer);
            this._timer = null;
        }

        let postion = event.selections[0].active;
        let beforeString = event.textEditor.document.getText(new VSCode.Range(postion.line, 0, postion.line, postion.character));
        if (/:\s*#$/.test(beforeString)) {
            let that = this;

            this._timer = setTimeout(function () {
                that.LaunchColorPicker();
                that._timer = null;
            }, this._config.autoLaunchDelay);
        }
    }

    private LaunchColorPickerWindow(orignalColor: string, callback: (value: string) => void): void {
        Process.exec(`ColorPicker.exe ${orignalColor}`, { cwd: this._extensionPath },
            (error: Error, stdout: string, stderr: string) => {
                if (stdout.length == 0) {
                    return;
                }
                callback(stdout);
            });
    }

    public LaunchColorPicker(): void {
        let editor = VSCode.window.activeTextEditor;
        let position = editor.selection.active;
        let line = position.line;
        let character = position.character;
        let str1 = this.Match(/:\s*#(\w*)$/, editor.document.getText(new VSCode.Range(line, 0, line, character)));
        let str2 = this.Match(/^(\w*)[\s;]?/, editor.document.getText(new VSCode.Range(line, character, line, character + this.MAX_VALUE_LEN)));
        this.LaunchColorPickerWindow(str1 + str2, (value: string) => {
            editor.edit((edit: VSCode.TextEditorEdit) => {
                edit.delete(new VSCode.Range(line, character, line, character + str2.length));
                edit.delete(new VSCode.Range(line, character - str1.length, line, character));
                edit.insert(editor.selection.active, value);
            });
        });
    }

    private Match(reg: RegExp, input: string): string {
        if (!reg.test(input)) {

            return '';
        }
        return reg.exec(input)[1];
    }

    dispose() {
    }
}

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: VSCode.ExtensionContext) {
    let picker = new VSColorPicker(VSCode.extensions.getExtension('lihui.vs-color-picker').extensionPath);
    VSCode.window.onDidChangeTextEditorSelection((e) => { picker.OnTextEditorSelectionChange(e) });
    VSCode.commands.registerCommand('extension.vs-color-picker', () => { picker.LaunchColorPicker() });
    // Add to a list of disposables which are disposed when this extension is deactivated.
    context.subscriptions.push(picker);
}

// this method is called when your extension is deactivated
export function deactivate() {
}