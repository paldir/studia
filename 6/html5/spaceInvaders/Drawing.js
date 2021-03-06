﻿function Drawing(idOfCanvas) {
    this.canvasOfBattleField = document.getElementById(idOfCanvas);
    this.cellWidth = this.canvasOfBattleField.width / 88;
    this.contextOfCanvas = this.canvasOfBattleField.getContext('2d');

    this.DrawInvaders = function (invaders) {
        for (var i = 0; i < invaders.length; i++) {
            if (!invaders[i].destroyed) {
                this.contextOfCanvas.beginPath();
                this.contextOfCanvas.arc(invaders[i].locationX[1] * this.cellWidth, invaders[i].locationY[1] * this.cellWidth, 0.5 * this.cellWidth, 2 * Math.PI, false);
                this.contextOfCanvas.fillStyle = 'red';
                this.contextOfCanvas.fill();
                this.contextOfCanvas.moveTo(invaders[i].locationX[0] * this.cellWidth, invaders[i].locationY[1] * this.cellWidth);
                this.contextOfCanvas.lineTo(invaders[i].locationX[1] * this.cellWidth + this.cellWidth, invaders[i].locationY[1] * this.cellWidth);
                this.contextOfCanvas.moveTo(invaders[i].locationX[0] * this.cellWidth, invaders[i].locationY[0] * this.cellWidth);
                this.contextOfCanvas.lineTo(invaders[i].locationX[0] * this.cellWidth, invaders[i].locationY[1] * this.cellWidth + this.cellWidth);
                this.contextOfCanvas.moveTo(invaders[i].locationX[1] * this.cellWidth + this.cellWidth, invaders[i].locationY[0] * this.cellWidth);
                this.contextOfCanvas.lineTo(invaders[i].locationX[1] * this.cellWidth + this.cellWidth, invaders[i].locationY[1] * this.cellWidth + this.cellWidth);

                this.contextOfCanvas.lineWidth = 3;
                this.contextOfCanvas.strokeStyle = 'red';

                this.contextOfCanvas.stroke();
            }
            else if (invaders[i].wreck > 0) {
                this.contextOfCanvas.beginPath();
                this.contextOfCanvas.moveTo(invaders[i].locationX[1] * this.cellWidth, invaders[i].locationY[0] * this.cellWidth);
                this.contextOfCanvas.lineTo(invaders[i].locationX[1] * this.cellWidth, invaders[i].locationY[0] * this.cellWidth + 0.5 * this.cellWidth);
                this.contextOfCanvas.moveTo(invaders[i].locationX[0] * this.cellWidth, invaders[i].locationY[1] * this.cellWidth);
                this.contextOfCanvas.lineTo(invaders[i].locationX[0] * this.cellWidth + 0.5 * this.cellWidth, invaders[i].locationY[1] * this.cellWidth);
                this.contextOfCanvas.moveTo(invaders[i].locationX[1] * this.cellWidth + 0.5 * this.cellWidth, invaders[i].locationY[1] * this.cellWidth);
                this.contextOfCanvas.lineTo(invaders[i].locationX[1] * this.cellWidth + this.cellWidth, invaders[i].locationY[1] * this.cellWidth);
                this.contextOfCanvas.moveTo(invaders[i].locationX[1] * this.cellWidth, invaders[i].locationY[1] * this.cellWidth + 0.5 * this.cellWidth);
                this.contextOfCanvas.lineTo(invaders[i].locationX[1] * this.cellWidth, invaders[i].locationY[1] * this.cellWidth + this.cellWidth);
                this.contextOfCanvas.moveTo(invaders[i].locationX[0] * this.cellWidth + 0.25 * this.cellWidth, invaders[i].locationY[0] * this.cellWidth + 0.25 * this.cellWidth);
                this.contextOfCanvas.lineTo(invaders[i].locationX[0] * this.cellWidth + 0.75 * this.cellWidth, invaders[i].locationY[0] * this.cellWidth + 0.75 * this.cellWidth);
                this.contextOfCanvas.moveTo(invaders[i].locationX[1] * this.cellWidth + 0.75 * this.cellWidth, invaders[i].locationY[0] * this.cellWidth + 0.25 * this.cellWidth);
                this.contextOfCanvas.lineTo(invaders[i].locationX[1] * this.cellWidth + 0.25 * this.cellWidth, invaders[i].locationY[0] * this.cellWidth + 0.75 * this.cellWidth);
                this.contextOfCanvas.moveTo(invaders[i].locationX[0] * this.cellWidth + 0.25 * this.cellWidth, invaders[i].locationY[1] * this.cellWidth + 0.75 * this.cellWidth);
                this.contextOfCanvas.lineTo(invaders[i].locationX[0] * this.cellWidth + 0.75 * this.cellWidth, invaders[i].locationY[1] * this.cellWidth + 0.25 * this.cellWidth);
                this.contextOfCanvas.moveTo(invaders[i].locationX[1] * this.cellWidth + 0.75 * this.cellWidth, invaders[i].locationY[1] * this.cellWidth + 0.75 * this.cellWidth);
                this.contextOfCanvas.lineTo(invaders[i].locationX[1] * this.cellWidth + 0.25 * this.cellWidth, invaders[i].locationY[1] * this.cellWidth + 0.25 * this.cellWidth);

                this.contextOfCanvas.lineWidth = 3;
                this.contextOfCanvas.strokeStyle = 'red';

                this.contextOfCanvas.stroke();

                invaders[i].wreck--;
            }
        }
    }

    this.DrawHero = function (hero) {
        this.contextOfCanvas.beginPath();
        this.contextOfCanvas.moveTo(hero.locationX[0] * this.cellWidth, hero.locationY[1] * this.cellWidth);
        this.contextOfCanvas.lineTo(hero.locationX[0] * this.cellWidth, hero.locationY[1] * this.cellWidth + this.cellWidth);
        this.contextOfCanvas.moveTo(hero.locationX[1] * this.cellWidth + this.cellWidth, hero.locationY[1] * this.cellWidth);
        this.contextOfCanvas.lineTo(hero.locationX[1] * this.cellWidth + this.cellWidth, hero.locationY[1] * this.cellWidth + this.cellWidth);

        this.contextOfCanvas.lineWidth = 2;
        this.contextOfCanvas.strokeStyle = 'green';

        this.contextOfCanvas.stroke();
        this.contextOfCanvas.beginPath();
        this.contextOfCanvas.moveTo(hero.locationX[1] * this.cellWidth, hero.locationY[0] * this.cellWidth);
        this.contextOfCanvas.lineTo(hero.locationX[0] * this.cellWidth + 0.5 * this.cellWidth, hero.locationY[1] * this.cellWidth + 0.5 * this.cellWidth);
        this.contextOfCanvas.lineTo(hero.locationX[1] * this.cellWidth + 0.5 * this.cellWidth, hero.locationY[1] * this.cellWidth + 0.5 * this.cellWidth);
        this.contextOfCanvas.closePath();
        this.contextOfCanvas.rect(hero.locationX[0] * this.cellWidth, hero.locationY[1] * this.cellWidth + 0.5 * this.cellWidth, 2 * this.cellWidth, 0.5 * this.cellWidth);

        this.contextOfCanvas.fillStyle = 'green';

        this.contextOfCanvas.fill();

        if (hero.shield > 0) {
            hero.shield--;

            this.contextOfCanvas.beginPath();
            this.contextOfCanvas.arc(hero.locationX[1] * this.cellWidth, hero.locationY[1] * this.cellWidth, 2 * this.cellWidth, 2 * Math.PI, false);

            this.contextOfCanvas.strokeStyle = 'green';
            this.contextOfCanvas.lineWidth = 2;

            this.contextOfCanvas.stroke();
        }
    }

    this.DrawMissilesOfHero = function (missilesOfHero) {
        for (var i = 0; i < missilesOfHero.length; i++) {
            this.contextOfCanvas.beginPath();
            this.contextOfCanvas.moveTo(missilesOfHero[i].locationX * this.cellWidth + 0.5 * this.cellWidth, missilesOfHero[i].locationY * this.cellWidth);
            this.contextOfCanvas.lineTo(missilesOfHero[i].locationX * this.cellWidth + 0.5 * this.cellWidth, missilesOfHero[i].locationY * this.cellWidth + this.cellWidth);

            this.contextOfCanvas.strokeStyle = 'rgb(128, 192, 128)';
            this.contextOfCanvas.lineWidth = 2;

            this.contextOfCanvas.stroke();
        }
    }

    this.DrawMissilesOfInvaders = function (missilesOfInvaders) {
        for (var i = 0; i < missilesOfInvaders.length; i++) {
            this.contextOfCanvas.beginPath();
            this.contextOfCanvas.moveTo(missilesOfInvaders[i].locationX * this.cellWidth + 0.5 * this.cellWidth, missilesOfInvaders[i].locationY * this.cellWidth);
            this.contextOfCanvas.lineTo(missilesOfInvaders[i].locationX * this.cellWidth + 0.5 * this.cellWidth, missilesOfInvaders[i].locationY * this.cellWidth + this.cellWidth);

            this.contextOfCanvas.strokeStyle = 'rgb(255, 128, 128)';
            this.contextOfCanvas.lineWidth = 2;

            this.contextOfCanvas.stroke();
        }
    }

    this.DrawGrid = function () {
        for (var i = 0; i < 88; i++) {
            this.contextOfCanvas.beginPath();
            this.contextOfCanvas.moveTo(i * this.cellWidth, 0);
            this.contextOfCanvas.lineTo(i * this.cellWidth, this.canvasOfBattleField.height);

            this.contextOfCanvas.strokeStyle = 'white';

            if ((i + 1) % 2 == 0)
                this.contextOfCanvas.lineWidth = 1
            else
                this.contextOfCanvas.lineWidth = 2;

            this.contextOfCanvas.stroke();
        }

        for (var i = 0; i < 88; i++) {
            this.contextOfCanvas.beginPath();
            this.contextOfCanvas.moveTo(0, i * this.cellWidth);
            this.contextOfCanvas.lineTo(this.canvasOfBattleField.width, i * this.cellWidth);

            this.contextOfCanvas.strokeStyle = 'white';

            if ((i + 1) % 2 == 0)
                this.contextOfCanvas.lineWidth = 1
            else
                this.contextOfCanvas.lineWidth = 2;

            this.contextOfCanvas.stroke();
        }
    }

    this.DrawStats = function (level, lifes) {
        this.contextOfCanvas.fillStyle = 'white';
        this.contextOfCanvas.font = '20px arial';

        this.contextOfCanvas.fillText('Level: ' + level, 10, 20);
        this.contextOfCanvas.fillText('Lifes: ' + lifes, 10, 40);
    }

    this.DrawEndOfGameCommunicate = function (text) {
        this.contextOfCanvas.fillStyle = 'white';
        this.contextOfCanvas.textBaseLine = 'middle';
        this.contextOfCanvas.textAlign = 'center';
        this.contextOfCanvas.font = '50px arial';

        this.contextOfCanvas.fillText(text, this.canvasOfBattleField.width / 2, this.canvasOfBattleField.height / 2);

        this.contextOfCanvas.font = '20px arial';
        this.contextOfCanvas.fillText('Press F5 or refresh page to play again. ', this.canvasOfBattleField.width / 2, this.canvasOfBattleField.height - 20);
    }

    this.ClearBattleField = function () {
        this.contextOfCanvas.clearRect(0, 0, this.canvasOfBattleField.width, this.canvasOfBattleField.height);
    }
}