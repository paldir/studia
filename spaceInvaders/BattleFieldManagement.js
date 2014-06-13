﻿function BattleFieldManagement() {
    this.invaders = new Array(55);
    this.directionOfAttack = 'L';
    this.hero = new Hero();
    this.missilesOfHero = new Array(0);

    var numberOfRow = 1;

    for (var i = 0; i < this.invaders.length; i++) {
        var numberOfInvader = i % 11 + 1;
        this.invaders[i] = new Invader(numberOfInvader, numberOfRow);

        if (numberOfInvader == 11)
            numberOfRow++;
    }

    this.MoveInvaders = function () {
        var moveDown = false;

        if (this.directionOfAttack == 'L') {
            if (this.invaders[54].locationX[1] == 85) {
                this.directionOfAttack = 'R';
                moveDown = true;
            }
        }
        else
            if (this.invaders[0].locationX[0] == 2) {
                this.directionOfAttack = 'L';
                moveDown = true;
            }

        if (moveDown)
            for (var i = 0; i < this.invaders.length; i++) {
                this.invaders[i].locationY[0]++;
                this.invaders[i].locationY[1]++;
            }

        if (this.directionOfAttack == 'L')
            var displacementX = 1;
        else
            var displacementX = -1;

        for (var i = 0; i < this.invaders.length; i++) {
            this.invaders[i].locationX[0] += displacementX;
            this.invaders[i].locationX[1] += displacementX;
        }
    }

    this.MoveMissilesOfHero = function () {
        for (var i = 0; i < this.missilesOfHero.length; i++) {
            this.missilesOfHero[i].locationY--;

            if (this.missilesOfHero[i].locationY < 0)
                this.missilesOfHero.splice(i, 1);
        }
    }

    this.HandleKeys = function (pressedKeys) {
        if (pressedKeys[32])
            this.missilesOfHero[this.missilesOfHero.length] = new MissileOfHero(this.hero);

        if (pressedKeys[37])
            if (this.hero.locationX[0] > 0) {
                this.hero.locationX[0]--;
                this.hero.locationX[1]--;
            }

        if (pressedKeys[39])
            if (this.hero.locationX[1] < 87) {
                this.hero.locationX[0]++;
                this.hero.locationX[1]++;
            }
    }
}