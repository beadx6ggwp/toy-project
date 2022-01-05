function Bird(x, y, r){
    this.x = x;
    this.y = y;
    this.r = r;
    this.jump = -13;
    this.velocity = 0;
    this.gravity = 0.6;

    this.render = function(){
        ctx.beginPath();
        ctx.fillStyle = "#FFF";
        ctx.arc(this.x, this.y, this.r, 0, 2*PI);
        ctx.fill();
    }

    this.update = function(){
        this.velocity += this.gravity;
        this.y += this.velocity;

        this.offset();
        //console.log(this.y,this.velocity);
    }


    this.up = function(){
        this.velocity += this.jump;
    }

    this.offset = function(){
        if(this.y > Height){
            this.y = Height;
            this.velocity = 0;
        }

        if(this.y < 0){
            this.y = 0;
            this.velocity = 0;
        }

    }
}