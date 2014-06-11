function Drawing(idOfCanvas) {
    this.canvasOfBattleField = document.getElementById(idOfCanvas);
    this.cellWidth = this.canvasOfBattleField.width / 44;
    this.contextOfCanvas = this.canvasOfBattleField.getContext('2d');

    this.DrawInvaders = function (invaders) {
        for (var i = 0; i < invaders.length; i++) {
            if (!invaders[i].destroyed) {
                this.contextOfCanvas.beginPath();
                this.contextOfCanvas.arc(invaders[i].locationX * this.cellWidth + 0.5 * this.cellWidth, invaders[i].locationY * this.cellWidth + 0.5 * this.cellWidth, 0.25 * this.cellWidth, 2 * Math.PI, false);
                this.contextOfCanvas.fillStyle = 'red';
                this.contextOfCanvas.fill();
                this.contextOfCanvas.moveTo(invaders[i].locationX * this.cellWidth, invaders[i].locationY * this.cellWidth + 0.5 * this.cellWidth);
                this.contextOfCanvas.lineTo(invaders[i].locationX * this.cellWidth + this.cellWidth, invaders[i].locationY * this.cellWidth + 0.5 * this.cellWidth);
                this.contextOfCanvas.moveTo(invaders[i].locationX * this.cellWidth, invaders[i].locationY * this.cellWidth);
                this.contextOfCanvas.lineTo(invaders[i].locationX * this.cellWidth, invaders[i].locationY * this.cellWidth + this.cellWidth);
                this.contextOfCanvas.moveTo(invaders[i].locationX * this.cellWidth + this.cellWidth, invaders[i].locationY * this.cellWidth);
                this.contextOfCanvas.lineTo(invaders[i].locationX * this.cellWidth + this.cellWidth, invaders[i].locationY * this.cellWidth + this.cellWidth);
                this.contextOfCanvas.lineWidth = 3;
                this.contextOfCanvas.strokeStyle = 'red';
                this.contextOfCanvas.stroke();
            }
        }
    }

    this.DrawHero = function (hero) {
        this.contextOfCanvas.beginPath();
        this.contextOfCanvas.moveTo(hero.locationX * this.cellWidth, hero.locationY * this.cellWidth + this.cellWidth);
        this.contextOfCanvas.lineTo(hero.locationX * this.cellWidth + this.cellWidth, hero.locationY * this.cellWidth + this.cellWidth);
        this.contextOfCanvas.lineTo(hero.locationX * this.cellWidth + this.cellWidth, hero.locationY * this.cellWidth + 0.5 * this.cellWidth);
        this.contextOfCanvas.lineTo(hero.locationX * this.cellWidth + 0.75 * this.cellWidth, hero.locationY * this.cellWidth + 0.75 * this.cellWidth);
        this.contextOfCanvas.lineTo(hero.locationX * this.cellWidth + 0.5 * this.cellWidth, hero.locationY * this.cellWidth);
        this.contextOfCanvas.lineTo(hero.locationX * this.cellWidth + 0.25 * this.cellWidth, hero.locationY * this.cellWidth + 0.75 * this.cellWidth);
        this.contextOfCanvas.lineTo(hero.locationX * this.cellWidth, hero.locationY * this.cellWidth + 0.5 * this.cellWidth);
        this.contextOfCanvas.closePath();
        this.contextOfCanvas.fillStyle = 'green';
        this.contextOfCanvas.fill();
    }

    this.DrawGrid = function () {
        for (var i = 0; i < 44; i++) {
            this.contextOfCanvas.beginPath();
            this.contextOfCanvas.moveTo(i * this.cellWidth, 0);
            this.contextOfCanvas.lineTo(i * this.cellWidth, this.canvasOfBattleField.height);
            this.contextOfCanvas.strokeStyle = 'white';
            this.contextOfCanvas.stroke();
        }

        for (var i = 0; i < 44; i++) {
            this.contextOfCanvas.beginPath();
            this.contextOfCanvas.moveTo(0, i * this.cellWidth);
            this.contextOfCanvas.lineTo(this.canvasOfBattleField.width, i * this.cellWidth);
            this.contextOfCanvas.strokeStyle = 'white';
            this.contextOfCanvas.stroke();
        }
    }
}