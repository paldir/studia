function DrawBattleField(idOfCanvas, matrixOfBattleField) {
    var canvasOfBattleField = document.getElementById(idOfCanvas);
    var widthOfPoint = canvasOfBattleField.width / matrixOfBattleField.length;
    var contextOfCanvas = canvasOfBattleField.getContext('2d');

    for (var i = 0; i < matrixOfBattleField.length; i++) {
        for (var j = 0; j < matrixOfBattleField[i].length; j++) {
            switch (matrixOfBattleField[i][j]) {
                case 2:
                    color = 'red';
                    break;
                case 1:
                    color = 'green';
                    break;
                default:
                    color = 'black';
            }

            contextOfCanvas.beginPath();
            contextOfCanvas.rect(i, j, widthOfPoint, widthOfPoint);
            contextOfCanvas.fillStyle = color;
            contextOfCanvas.fill();
        }
    }
}