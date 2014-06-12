function Invader(numberOfInvader, numberOfRow) {
    //this.locationX = (numberOfInvader - 1) * 2 + 1;
    this.locationX = new Array((numberOfInvader - 1) * 4 + 2, (numberOfInvader - 1) * 4 + 3);
    //this.locationY = (numberOfRow - 1) * 2 + 1;
    this.locationY = new Array((numberOfRow - 1) * 4 + 2, (numberOfRow - 1) * 4 + 3);
    this.destroyed = false;
}

function Hero() {
    this.locationX = new Array(42, 43);
    this.locationY = new Array(84, 85);
}