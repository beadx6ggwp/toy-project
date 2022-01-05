function Pipe(W, H){
    this.height = H;
    this.width = W;
    this.x = this.width;
    this.Pw = 80;
    this.top =  random(0,H/2);
    this.bottom = this.height - random(0,H/2);
    this.speed = -5;
    this.c = "#FFF";
    
    this.render = function(){
        ctx.fillStyle = this.c;
        ctx.fillRect(this.x, 0, this.Pw, this.top);
        ctx.fillRect(this.x, this.bottom, this.Pw, H);
    }

    this.update = function(){
        this.x += this.speed;
    }
    this.offScreen = function(){
        if(this.x < -this.Pw){
            return true;
        }
        return false;
    }
    this.cheak = function(bird){
        if(bird.y-bird.r < this.top || bird.y+bird.r > this.bottom){
            if(bird.x+bird.r > this.x && bird.x-bird.r < this.x+this.Pw){
                this.c = "#F00";
                return true;
            }
        }
        return false;
    }
}