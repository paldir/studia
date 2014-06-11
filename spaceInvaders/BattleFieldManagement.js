/// <reference path="ShipsAndMissiles.js" />
function BattleFieldManagement() {
    this.invaders = new Array(55);
    this.hero = new Hero();

    var numberOfRow = 1;

    for (var i = 0; i < this.invaders.length; i++) {
        var numberOfInvader = i % 11 + 1;
        this.invaders[i] = new Invader(numberOfInvader, numberOfRow);

        if (numberOfInvader == 11)
            numberOfRow++;
    }
}