function BattleFieldManagement() {
    this.invaders = new Array(55);
    this.directionOfAttack = 'L';
    this.hero = new Hero();

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
            if (this.invaders[54].locationX == 42) {
                this.directionOfAttack = 'R';
                moveDown = true;
            }
        }
        else
            if (this.invaders[0].locationX == 1) {
                this.directionOfAttack = 'L';
                moveDown = true;
            }

        if (moveDown)
            for (var i = 0; i < this.invaders.length; i++)
                this.invaders[i].locationY++;

        if (this.directionOfAttack == 'L')
            var displacementX = 1;
        else
            var displacementX = -1;

        for (var i = 0; i < this.invaders.length; i++)
            this.invaders[i].locationX = this.invaders[i].locationX + displacementX;
    }
}