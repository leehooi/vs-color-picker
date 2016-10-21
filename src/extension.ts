'use strict';
// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as VSCode from 'vscode';
import * as Process from 'child_process'

class VSColorPicker {
    constructor(private _extensionPath: string) {

    }

    private _timer: NodeJS.Timer = null;

    public OnTextEditorSelectionChange(event: VSCode.TextEditorSelectionChangeEvent) {
        if (this._timer != null) {
            clearTimeout(this._timer);
            this._timer = null;
        }

        let postion = event.selections[0].active;
        let beforeString = event.textEditor.document.getText(new VSCode.Range(postion.line, 0, postion.line, postion.character));
        if (/:\s*#$/.test(beforeString)) {
            let that = this;

            this._timer = setTimeout(function () {
                let afterString = event.textEditor.document.getText(new VSCode.Range(postion.line, postion.character, postion.line, postion.character + 20));
                let orignalColor = /^(\w*)[\s;]?/.exec(afterString)[1];

                that.LaunchColorPicker(orignalColor, (value: string) => {
                    event.textEditor.edit((edit: VSCode.TextEditorEdit) => {
                        edit.delete(new VSCode.Range(postion.line, postion.character, postion.line, postion.character + orignalColor.length));
                        edit.insert(postion, value);
                    });
                });
                that._timer = null;
            }, 100);
        }
    }

    private LaunchColorPicker(orignalColor: string, callback: (value: string) => void): void {
        Process.exec(`ColorPicker.exe ${orignalColor}`, { cwd: this._extensionPath },
            (error: Error, stdout: string, stderr: string) => {
                if (stdout.length == 0) {
                    return;
                }
                callback(stdout);
            });
    }

    dispose() {
    }
}

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: VSCode.ExtensionContext) {

    let trigger = new VSColorPicker(VSCode.extensions.getExtension('lihui.vs-color-picker').extensionPath);
    VSCode.window.onDidChangeTextEditorSelection((e) => { trigger.OnTextEditorSelectionChange(e) });

    // Add to a list of disposables which are disposed when this extension is deactivated.
    context.subscriptions.push(trigger);
}

// this method is called when your extension is deactivated
export function deactivate() {
}