function Invader(numberOfInvader, numberOfRow) {
    this.locationX = new Array((numberOfInvader - 1) * 4 + 2, (numberOfInvader - 1) * 4 + 3);
    this.locationY = new Array((numberOfRow - 1) * 4 + 2, (numberOfRow - 1) * 4 + 3);
    this.destroyed = false;
    this.wreck = 0;
}

function Hero() {
    this.locationX = new Array(42, 43);
    this.locationY = new Array(84, 85);
    //this.lifes = 3;
    this.shield = 0;
    Hero.lifes;
}

function MissileOfHero(hero) {
    this.locationX = hero.locationX[Math.round(Math.random())];
    this.locationY = hero.locationY[0] - 1;
}

function MissileOfInvader(invader) {
    this.locationX = invader.locationX[Math.round(Math.random())];
    this.locationY = invader.locationY[1] + 1;
}