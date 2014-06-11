function Invader(numberOfInvader, numberOfRow) {
    this.locationX = (numberOfInvader - 1) * 2 + 1;
    this.locationY = (numberOfRow - 1) * 2 + 1;
    this.destroyed = false;
}

function Hero() {
    this.locationX = 21
    this.locationY = 42;
}