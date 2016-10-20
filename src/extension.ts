'use strict';
// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as VSCode from 'vscode';
import * as Process from 'child_process'
class VSColorPicker {
    OnTextEditorSelectionChange(event: VSCode.TextEditorSelectionChangeEvent) {
        let postion = event.selections[0].active;
        let beforeString = event.textEditor.document.getText(new VSCode.Range(postion.line, 0, postion.line, postion.character));
        if (/:\s*#$/.test(beforeString)) {
            let afterString = event.textEditor.document.getText(new VSCode.Range(postion.line, postion.character, postion.line, postion.character + 20));
            let color = /^(\w*)[\s;]?/.exec(afterString)[1];
            Process.exec(`ColorPicker.exe ${color}`,
                {
                    cwd: VSCode.extensions.getExtension('lihui.vs-color-picker').extensionPath
                },
                (error: Error, stdout: string, stderr: string) => {
                    if (stdout.length == 0) {
                        return;
                    }
                    event.textEditor.edit((edit: VSCode.TextEditorEdit) => {
                        edit.delete(new VSCode.Range(postion.line, postion.character, postion.line, postion.character + color.length));
                        edit.insert(postion, stdout);
                    });
                });
        }
    }

    dispose() {
    }
}

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: VSCode.ExtensionContext) {

    let trigger = new VSColorPicker();
    VSCode.window.onDidChangeTextEditorSelection((e) => { trigger.OnTextEditorSelectionChange(e) });

    // Add to a list of disposables which are disposed when this extension is deactivated.
    context.subscriptions.push(trigger);
}

// this method is called when your extension is deactivated
export function deactivate() {
}