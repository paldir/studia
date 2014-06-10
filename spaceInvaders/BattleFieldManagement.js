function BattleFieldManagement() {
    this.matrixOfBattleField = new Array(272);

    for (var i = 0; i < this.matrixOfBattleField.length; i++) {
        this.matrixOfBattleField[i] = new Array(272);

        for (var j = 0; j < this.matrixOfBattleField[i].length; j++) {
            this.matrixOfBattleField[i][j] = 0;
        }
    }

    this.FillMatrix = function () {
        this.matrixOfBattleField[104][101] = 1;
        this.matrixOfBattleField[105][101] = 1;
        this.matrixOfBattleField[106][101] = 1;
        this.matrixOfBattleField[106][102] = 1;
        this.matrixOfBattleField[104][103] = 1;
        this.matrixOfBattleField[105][103] = 1;
        this.matrixOfBattleField[106][103] = 1;
        this.matrixOfBattleField[101][104] = 1;
        this.matrixOfBattleField[102][104] = 1;
        this.matrixOfBattleField[103][104] = 1; 
        this.matrixOfBattleField[104][104] = 1;
        this.matrixOfBattleField[105][104] = 1;
        this.matrixOfBattleField[106][104] = 1;
        this.matrixOfBattleField[104][105] = 1;
        this.matrixOfBattleField[105][105] = 1;
        this.matrixOfBattleField[106][105] = 1;
        this.matrixOfBattleField[106][106] = 1;
        this.matrixOfBattleField[104][107] = 1;
        this.matrixOfBattleField[105][107] = 1;
        this.matrixOfBattleField[106][107] = 1;
    }
}