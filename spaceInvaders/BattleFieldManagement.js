function BattleFieldManagement() {
    this.invaders = new Array(55);
    this.directionOfAttack = 'L';
    this.hero = new Hero();
    this.missilesOfHero = new Array(0);
    this.missilesOfInvaders = new Array(0);

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

    this.DestroyInvaders = function () {
        var numberOfLivingInvaders = 0;

        for (var i = 0; i < this.missilesOfHero.length; i++)
            for (var j = 0; j < this.invaders.length; j++)
                if (!this.invaders[j].destroyed)
                    if (this.missilesOfHero[i].locationX == this.invaders[j].locationX[0] || this.missilesOfHero[i].locationX == this.invaders[j].locationX[1])
                        if (this.missilesOfHero[i].locationY == this.invaders[j].locationY[0] || this.missilesOfHero[i].locationY == this.invaders[j].locationY[1]) {
                            this.missilesOfHero.splice(i, 1);
                            this.invaders[j].destroyed = true;
                        }
    }

    this.LaunchMissileOfInvader = function () {
        var numberOfInvader;

        do {
            numberOfInvader = Math.round(Math.random() * 54);
        } while (this.invaders[numberOfInvader].destroyed)

        this.missilesOfInvaders[this.missilesOfInvaders.length] = new MissileOfInvader(this.invaders[numberOfInvader]);
    }

    this.MoveMissilesOfInvaders = function () {
        for (var i = 0; i < this.missilesOfInvaders.length; i++) {
            this.missilesOfInvaders[i].locationY++;

            if (this.missilesOfInvaders[i].locationY > 87)
                this.missilesOfInvaders.splice(i, 1);
        }
    }

    this.DestroyHero = function () {
        var gameOver = false;

        for (var i = 0; i < this.missilesOfInvaders.length; i++)
            if (this.missilesOfInvaders[i].locationY >= this.hero.locationY[0])
                if (this.missilesOfInvaders[i].locationX == this.hero.locationX[0] || this.missilesOfInvaders[i].locationX == this.hero.locationX[1])
                    if (this.missilesOfInvaders[i].locationY == this.hero.locationY[0] || this.missilesOfInvaders[i].locationY == this.hero.locationY[1])
                        gameOver = true;

        return gameOver;
    }

    this.HandleKeys = function (pressedKeys) {
        if (pressedKeys[32])
            if (this.missilesOfHero.length == 0 || this.hero.locationY[0] - this.missilesOfHero[this.missilesOfHero.length - 1].locationY > 10) {
                this.missilesOfHero[this.missilesOfHero.length] = new MissileOfHero(this.hero);
            }

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