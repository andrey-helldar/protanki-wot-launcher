/*
 *  Отключение ошибок JavaScript
 */
function noError() { return true; }
window.onerror = noError;


/*
 *  Сохранение данных в файл
 */
function WriteToFile() {
    var fso, s, text;
    var fso = new ActiveXObject('Scripting.FileSystemObject');
    var s = fso.CreateTextFile("C:\\1test.txt", true);
    var text = document.getElementById("id_login").innerText;
    s.WriteLine(text);
    s.WriteLine('***********************');
    s.Close();

    alert(text);
}
$('input[type=submit]').onClick(WriteToFile());
