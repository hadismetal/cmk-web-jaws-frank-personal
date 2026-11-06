//Print-Dialog
function autoPrint() {
    window.focus();
    window.print();
}

//Close-Window
function closeWindow() {
    window.close();
}
function closeBack() {
    if (history.length == 1) //layar pop-up
        window.close();

    else
        history.back();
}
function closeReload() {
    window.opener.location.reload(true);
    window.close();
}

//Number-Format
function convertNum(foo) { return foo.replace(/\./g, "").replace(/,/g, "."); }
function numberToInd(txt) { txt.value = txt.value.replace(/\./g, ","); }
function numberFormat(txt) {
    if (txt.value != "") {
        ori = txt.value; raw1 = ""; raw2 = "";
        isNegative = false; isNilai = false;

        //Parsing
        for (i = 0; i < ori.length; i++) {
            c = ori.charAt(i);
            if (c == "-" && i == 0) {
                isNegative = true;
            }
            if (c == "0" || c == "1" || c == "2" || c == "3" || c == "4" ||
            c == "5" || c == "6" || c == "7" || c == "8" || c == "9") {
                if (!isNilai) raw1 += c; else raw2 += c;
            }
            if (c == "," && !isNilai) {
                isNilai = true;
            }
        }

        if (raw1 == "") txt.value = "";
        else {
            //Hapus nol didepan
            raw1 = raw1 * 1;
            raw1 = raw1 + "";

            if (raw1 != "") {
                //Ribuan
                ribuan = "";
                for (i = raw1.length; i > 0; i--) {
                    ribuan = raw1.charAt(i - 1) + ribuan
                    z = raw1.length - i + 1
                    if (z % 3 == 0 && z != 0) ribuan = "." + ribuan
                }
                if (ribuan.charAt(0) == ".")
                    ribuan = ribuan.substring(1, ribuan.length);

                fnl = "";
                if (isNilai)
                    fnl = ribuan + "," + raw2;
                else
                    fnl = ribuan;

                if (isNegative)
                    txt.value = "-" + fnl;
                else
                    txt.value = fnl;
            }
        }
    }
}

//Caps-Lock
function capsLock(e, foodiv) {
    var myKeyCode = 0;
    var myShiftKey = false;

    // IE
    if (document.all) {
        myKeyCode = e.keyCode;
        if (myKeyCode == 0) myKeyCode = e.which;
        myShiftKey = e.shiftKey;
    } else {
        myKeyCode = e.which;
        myShiftKey = e.shiftKey;
    }

    // Upper case letters are seen without depressing the Shift key, therefore Caps Lock is on
    if ((myKeyCode >= 65 && myKeyCode <= 90) && !myShiftKey) {
        document.getElementById(foodiv).style.display = '';
        // Lower case letters are seen while depressing the Shift key, therefore Caps Lock is on
    } else if ((myKeyCode >= 97 && myKeyCode <= 122) && myShiftKey) {
        document.getElementById(foodiv).style.display = '';
    }
    else {
        document.getElementById(foodiv).style.display = 'none';
    }
}

//Check-Box Control
function checkCtrl(foo, n) {
    var x = true; var i = 0;
    while (x) {
        if (document.getElementById(foo + "_" + i)) {
            if (!document.getElementById(foo + "_" + i).disabled) {
                if (n == "true")
                    document.getElementById(foo + "_" + i).checked = true;
                else
                    document.getElementById(foo + "_" + i).checked = false;
            }
            i++;
        } else { x = false; }
    }
}
